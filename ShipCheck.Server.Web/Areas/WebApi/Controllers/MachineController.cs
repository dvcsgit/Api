using NLog;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LinqKit;
using System.Globalization;
using ShipCheck.Server.Web.Areas.WebApi.Models;
using System.Data.SqlClient;
using ShipCheck.Server.Web.Library.Extensions;
using System.Threading.Tasks;
using ShipCheck.Server.DataSource;
namespace ShipCheck.Server.Web.Areas.WebApi.Controllers
{
    public class MachineController : ApiController
    {
        protected static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 取得故障清單
        /// </summary>
        /// <param name="type">取得 電的還機的單 EX: M or E</param>
        /// <param name="facID">廠別 EX: 1 or 2 or 3</param>
        /// <param name="isDone">是否有處理 [optional] true or false</param>
        /// <param name="isDebug">給我查看時間串法有沒有問題 [optional] true or false</param>
        /// <returns></returns>
        public IEnumerable<MachineItem> Get(string type, string facID = "", bool? isDone = null, bool? isDebug = null)
        {
            List<MachineItem> viewModel = new List<MachineItem>();
            _logger.Info("Get start");
            try
            {
                //NOTE:
                //T_II_HANG_SILK_MASTER	(掛絲資訊主檔) 取得 FAC_ID		TWISTING_MACH_NO 排序 新增時間 取得第一筆 抓 FALL_SERIAL
                //得到 FAC_ID TWISTING_MACH_NO FALL_SERIAL
                //對應 T_II_START_NEW	(撚絲機開機時間紀錄)
                //拿 T_II_HANG_SILK_DETAIL 用 UP_SILK_NO 找第一筆產品
                //找 產品對應到的 T_II_PROD_TIME 找這項產品要做幾分鐘

                _logger.Info("type:{0}", type);
                _logger.Info("facID:{0}", facID);
                _logger.Info("isDone:{0}", isDone);
                string templateFormat = "yyyyMMddHHmm";

                using (QIS_NEWEntities db = new QIS_NEWEntities())
                {
                    var query = db.T_III_MACHINE_CHECK.AsQueryable();

                    query = query.Where(x => x.EXCEPTION_TYPE == type && x.CONFIRM_TIME == null && x.EXCEPTION_TIME.CompareTo("20151109000000") > 0);
                    
                    if(isDone.HasValue)
                    {
                        if (isDone.Value)
                        {
                            //已經做完
                            query = query.Where(x => x.REPAIR_PERSON != null);
                        }
                        else
                        {
                            //需要前端做維修
                            query = query.Where(x => x.REPAIR_PERSON == null);
                        }
                    }

                    if(!string.IsNullOrEmpty(facID))
                    {
                        query = query.Where(x => x.FAC_ID == facID);
                    }

                    var resultList = query.Select(x => new MachineItem
                        {

                            RecordKey = x.SEQ_NO.ToString(),
                            FacID = x.FAC_ID,
                            TwistingMachNo = x.TWISTING_MACH_NO,
                            RegFlagNo = x.RGE_FLAG_NO,
                            ExceptionTime = x.EXCEPTION_TIME,
                            ExceptionID = x.EXCEPTION_ID,
                            ExceptionName = x.EXCEPTION_NAME,
                            IsDone = x.REPAIR_PERSON != null ? true : false ,
                            RepairTime = x.REPAIR_TIME ,
                            RepairPerson = x.REPAIR_PERSON ,
                            TransTime = x.TRANS_TIME,
                            TransPerson = x.TRANS_PERSON

                    });
                    viewModel = resultList.ToList();

                    // 準備計算停機時間
                    var queryKeyList = viewModel.Select(x => new { x.FacID, x.TwistingMachNo }).Distinct().ToList();

                    List<TempFacIDSilkProTime> tempFacIDSilkProTimeList = new List<TempFacIDSilkProTime>();
                    List<SqlParameter> temp_param = new List<SqlParameter>();


                    var proLenTimeList = db.T_III_PROD_SILK_LEN_TIME.ToList();//目前資料很少

                    // 取得 T_II_START_NEW (串目前最新運行時間)
                    var startNewPredicate = PredicateBuilder.False<T_II_START_NEW>();
                    foreach (var item in queryKeyList)
                    {
                        startNewPredicate = startNewPredicate.Or(x =>
                            x.FAC_ID == item.FacID &&
                            x.TWISTING_MACH_NO == item.TwistingMachNo
                        );
                    }
                    var startNewQuery = db.T_II_START_NEW.AsExpandable().Where(startNewPredicate);
                    var startNewMap = startNewQuery
                        .GroupBy(x => new { x.FAC_ID, x.TWISTING_MACH_NO })
                         .Select(g => g.OrderByDescending(p => p.START_TM)
                                .FirstOrDefault()
                        )
                        .ToList();


                    // 抓取基台 產品需要做的時間
                    // 整合 語法 JEFF:給的語法 1 2 3(串一句SQL) 再靠Memory 去抓產品 (主要加快速度減少SQL問的時間)



                    var query_idx = 0;
                    var tempWhereText = "";
                    foreach (var item in queryKeyList)
                    {
                        var fac_id = "@FAC_ID_" + query_idx;
                        var twising_id = "@TWISTING_MACH_NO_" + query_idx;

                        tempWhereText += string.Format("(f.FAC_ID = {0} and f.TWISTING_MACH_NO= {1}) OR", fac_id, twising_id);

                        temp_param.Add(new SqlParameter(fac_id, item.FacID));
                        temp_param.Add(new SqlParameter(twising_id, item.TwistingMachNo));

                        query_idx++;
                    }

                    if (!string.IsNullOrEmpty(tempWhereText))
                    {
                        tempWhereText = tempWhereText.SQLRemoveEndSyntax();
                    }

                    var sqlText = string.Format(@"
                        SELECT * FROM (

                        SELECT f.FAC_ID,f.TWISTING_MACH_NO,f.FALL_SERIAL,m.UP_SILK_NO FROM T_II_TWISTING_MACH_FALL_NO f
                        JOIN T_II_HANG_SILK_MASTER m ON f.FAC_ID = m.FAC_ID AND f.TWISTING_MACH_NO = m.TWISTING_MACH_NO AND f.FALL_SERIAL = m.FALL_SERIAL
                        Where {0}

                        ) A

                        LEFT JOIN 

                        (
                        SELECT DISTINCT A.UP_SILK_NO,  B.PROD_SPEC , substring(B.PROD_SPEC,1,charindex('/',B.PROD_SPEC)-1) PROD_SPEC_NEW FROM QIS_NEW.DBO.T_II_HANG_SILK_DETAIL A
                         JOIN QIS_NEW.DBO.T_PROD_SPEC B ON SUBSTRING(A.PROD_NO,2,3)=SUBSTRING(B.PROD_NO,2,3)
                                            
                        ) B ON A.UP_SILK_NO = B.UP_SILK_NO

                    ", tempWhereText);


                    var proInfoMap = db.Database.SqlQuery<TempProData>(sqlText, temp_param.ToArray()).ToList();

                    if (proInfoMap.Count > 0)
                    {
                        Parallel.ForEach(proInfoMap.AsEnumerable(), item =>
                        {
                            if (item.PROD_SPEC_NEW != null)
                            {
                                var proTimeItem = proLenTimeList.Where(x => x.FAC_ID == item.FAC_ID && item.PROD_SPEC_NEW.Contains(x.PROD_SPEC)).FirstOrDefault();
                                if (proTimeItem != null)
                                {
                                    item.SILK_MINS = proTimeItem.SILK_MINS;
                                }
                            }
                            
                        });
                    }



                    //loop data

                    //開始回填 停機時間,產品,開機時間
                    Parallel.ForEach(viewModel.AsEnumerable(), item =>
                    {
                        //將 錠號補0
                        if (!string.IsNullOrEmpty(item.RegFlagNo))
                        {
                            item.RegFlagNo = item.RegFlagNo.PadLeft(2, '0');
                        }

                        var baseTimeStart = startNewMap.Where(x =>
                            x.FAC_ID == item.FacID &&
                            x.TWISTING_MACH_NO == item.TwistingMachNo
                        ).FirstOrDefault();

                        var endTimeInfo = proInfoMap.Where(x =>
                             x.FAC_ID == item.FacID &&
                             x.TWISTING_MACH_NO == item.TwistingMachNo
                        ).FirstOrDefault();

                        if (endTimeInfo != null)
                        {
                            item.CurrentProdNo = endTimeInfo.PROD_SPEC;
                        }

                        if (baseTimeStart != null)
                        {
                            DateTime startTime;
                            if (DateTime.TryParseExact(baseTimeStart.START_TM, "yyyyMMddHHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startTime))
                            {
                                //填入目前開機時間
                                item.CurrentStartTime = startTime.ToString(templateFormat);
                                //計算結束時間
                                if (endTimeInfo != null)
                                {
                                    if (endTimeInfo.SILK_MINS.HasValue)
                                    {
                                        var endTime = startTime.AddMinutes(endTimeInfo.SILK_MINS.Value);
                                        item.EstimateStopTime = endTime.ToString("yyyyMMddHHmm");
                                    }

                                }
                                

                            }
                        }

                    });

                    
                }

 

            }
            catch (Exception ex)
            {
                _logger.Error("發生錯誤:{0}", ex.Message);
            }
            finally
            {
                _logger.Info("Get end");
            }

            if(isDebug.HasValue && isDebug.Value)
            {
                viewModel = viewModel.Where(x => x.EstimateStopTime == "NA").ToList();
            }


            

            _logger.Info("viewModel :count {0}",viewModel.Count);

            return viewModel;
        }

        public class TempProData
        {
            public string FAC_ID { get; set; }
            public string TWISTING_MACH_NO { get; set; }
            public string FALL_SERIAL { get; set; }
            public string UP_SILK_NO { get; set; }
            public string PROD_SPEC { get; set; }
            public string PROD_SPEC_NEW { get; set; }
            public int? SILK_MINS { get; set; }
        }

    }
}
