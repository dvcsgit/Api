using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

using NLog;
using ShipCheck.Server.DataSource;



namespace ShipCheck.Server.Web.Areas.WebApi.Controllers
{
    public class VerController : ApiController
    {
        protected static Logger _logger = LogManager.GetCurrentClassLogger();

        public HttpResponseMessage Get(string appName)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                using (BookingCarEntities db = new BookingCarEntities())
                {
                    var version = db.Version.Where(x => x.AppName == appName).OrderByDescending(x => x.VerCode).FirstOrDefault();

                    if(version!=null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { Success = true, Data = version });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { Success = false, Data = "找不到此APP相關資訊" });
                    }
                }

                
            }
            catch (Exception ex)
            {


                _logger.Error("發生錯誤:{0}", ex.Message);
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, new { Success = false, Message = "發生錯誤" });
                
            }

            

            return response;
        }

    }
}
