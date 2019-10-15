using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using FTC.SI.Web.Utils.Utils;
using NLog;
using ShipCheck.Server.DataSource;
using ShipCheck.Server.Web.Areas.WebApi.Models;
using ShipCheck.Server.Web.Library;

namespace ShipCheck.Server.Web.Areas.WebApi.Controllers
{
    public class DownloadController : ApiController
    {
        protected static Logger _logger = LogManager.GetLogger("DownloadApi");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouseID"></param>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            //_logger.Debug("Download 參數 warehouseID:{0}", warehouseID);
            var isDebug = bool.Parse(CommonUtils.AppSettings("IsDebug"));
            var preDay = -6; // 往前抓幾天!?

            DownloadViewModel viewModel = new DownloadViewModel();
            string packageFileName = string.Format("Shipcheck_{0}.zip", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            var currentDate = DateTime.Now.ToString("yyyyMMdd hhmmss");

            try
            {
                _logger.Debug("download start...");
                using (BookingCarSSISEntities db = new BookingCarSSISEntities())
                {
                    //createMapInit();
                    var startDate = DateTime.Today.AddDays(preDay);

                    var glassFibreCloth = db.v28caxrf_s2
                        .Where(x => string.IsNullOrEmpty(x.CheckAccount))
                        .Where(x => DbFunctions.TruncateTime(x.DateCreated) >= startDate)
                        .AsEnumerable()
                        .Select(x => new GlassFibreClothItem
                        {
                            ROWIdx = x.ROWIdx,
                            CUFN = x.CUFN,
                            ODNO = x.ODNO,
                            UNPKWGT = x.UNPKWGT.HasValue ? x.UNPKWGT.Value.ToString() : null,
                            PDID = x.PDID,
                            CLHRLNO = x.CLHRLNO,
                            L = x.L,
                            GRSWGT = x.GRSWGT.HasValue ? x.GRSWGT.Value.ToString() : null,
                            DateCreated = x.DateCreated.HasValue ? x.DateCreated.Value.ToString(Resource.StrDateTimeFormat) : null,
                            PrintTime = x.PrintTime.HasValue ? x.PrintTime.Value.ToString(Resource.StrDateTimeFormat) : null,
                            CheckAccount = x.CheckAccount,
                            CheckTime = x.CheckTime.HasValue ? x.CheckTime.Value.ToString(Resource.StrDateTimeFormat) : null,
                            OUTMDAT = x.OUTMDAT
                        })
                        .OrderBy(x => x.ROWIdx)
                        .ToList();





                    viewModel.GlassFibreCloth = glassFibreCloth;


                    var copperFoil = db.V28EFQ7J
                        .Where(x => string.IsNullOrEmpty(x.CheckAccount))
                        .Where(x => DbFunctions.TruncateTime(x.DateCreated) >= startDate)
                        .AsEnumerable()
                        .Select(x => new CopperFoilItem
                        {
                            ROWIdx = x.ROWIdx,
                            CUNO = x.CUNO,
                            OMNO = x.OMNO,
                            KD = x.KD,
                            SPEC = x.SPEC,
                            IKPCNO = x.IKPCNO,
                            L = x.L.ToString(),
                            NETWGT = x.NETWGT.HasValue ? x.NETWGT.Value : (int?)null,
                            DateCreated = x.DateCreated.HasValue ? x.DateCreated.Value.ToString(Resource.StrDateTimeFormat) : null,
                            PrintTime = x.PrintTime.HasValue ? x.PrintTime.Value.ToString(Resource.StrDateTimeFormat) : null,
                            CheckAccount = x.CheckAccount,
                            CheckTime = x.CheckTime.HasValue ? x.CheckTime.Value.ToString(Resource.StrDateTimeFormat) : null,
                            IKDAT = x.IKDAT
                        })
                        .OrderBy(x => x.ROWIdx)
                        .ToList();


                    viewModel.CopperFoil = copperFoil;
         
                    //產出SQLite
                    var tempSqliteFilePath = SQLiteUtils.GenerateSQLiteZip(viewModel);

                    if (isDebug)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        if (File.Exists(tempSqliteFilePath))
                        {
                            FileStream fileStream = new FileStream(tempSqliteFilePath, FileMode.Open, FileAccess.Read)
                            {

                            };
                            response = Request.CreateResponse(HttpStatusCode.OK);
                            response.Content = new StreamContent(fileStream);
                            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                            {
                                FileName = packageFileName
                            };
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.Error("發生錯誤:{0}", ex.Message);
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                throw ex;
            }
            finally
            {
                _logger.Debug("download done...");
            }

            return response;
        }

        ///// <summary>
        ///// AutoMappper對應設定
        ///// </summary>
        //private void createMapInit()
        //{

        //    Mapper.CreateMap<PFG.Inventory.DataSource.Inventory, InventoryItem>()
        //        .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateModified.HasValue ? src.DateModified.Value.ToString(Resources.StrDateTimeFormat) : null))
        //        .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated.HasValue ? src.DateCreated.Value.ToString(Resources.StrDateTimeFormat) : null));

        //}
    }
}