using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using NLog;
using FTC.SI.Web.Utils.Extensions;

using ShipCheck.Server.Web.Areas.WebApi.Models;
using ShipCheck.Server.Web.Library;
using ShipCheck.Server.DataSource;

namespace ShipCheck.Server.Web.Areas.WebApi.Controllers
{
    /// <summary>
    /// 前端 上傳 巡檢員已經確認排定要修復
    /// </summary>
    public class UploadCheckMachineController : ApiController
    {
        protected static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public HttpResponseMessage Post(UploadCheckMachineItem model)
        {
            _logger.Debug("Post start");
            HttpResponseMessage result = null;

            try
            {
                _logger.Debug("Param:{0}", JsonConvert.SerializeObject(model));

                if (ModelState.IsValid)
                {
                    using (QIS_NEWEntities db = new QIS_NEWEntities())
                    {
                        var record = db.T_III_MACHINE_CHECK.Where(x => x.SEQ_NO.ToString() == model.RecordKey).FirstOrDefault();

                        if(record!=null)
                        {
                            if(string.IsNullOrEmpty(record.REPAIR_PERSON) && string.IsNullOrEmpty(record.REPAIR_TIME))
                            {
                                record.REPAIR_PERSON = model.Account;
                                record.REPAIR_TIME = DateTime.Now.ToString("yyyyMMddHHmmss");

                                db.SaveChanges();
                            }
                            else
                            {
                                throw new Exception(string.Format("此資料已在{0}被:{1}確認過",record.REPAIR_TIME, record.REPAIR_PERSON));
                            }
                        }
                        else
                        {
                            throw new Exception("此資料不存在");
                        }

                        result = Request.CreateResponse(HttpStatusCode.Created, new { Success = true, Message = "資料更新成功" });
                    }

                }
                else
                {
                    
                    result = Request.CreateResponse(HttpStatusCode.Created, new { Success = false, Message = ModelState.GetErrorsString() });
                }

            }
            catch (DbEntityValidationException dbEx)
            {
                var errors = dbEx.GetErrors();
                var errorMessage = errors.ToLogString();
                _logger.Error("Save 發生錯誤:{0}", errors.ToLogString());
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, new { Success = false, Message = errorMessage });
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                _logger.Error("Save 發生錯誤:{0}", ex.Message);
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, new { Success = false, Message = errorMessage });
            }
            finally
            {
                _logger.Debug("Post end");
            }

            return result;


        }
    }
}
