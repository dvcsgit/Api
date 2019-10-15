using System;
using System.Linq;
using System.Web.Http;
using NLog;
using ShipCheck.Server.DataSource;
using ShipCheck.Server.Web.Areas.WebApi.Models;

namespace ShipCheck.Server.Web.Areas.WebApi.Controllers
{
    public class LoginController : ApiController
    {
        protected static Logger _logger = LogManager.GetLogger("LoginApi");

        // GET api/login
        public UserLoginInfo Get(string account, string password)
        {
            UserLoginInfo viewModel = new UserLoginInfo();
            try
            {
                _logger.Debug("start");
                _logger.Debug("param: account:{0}", account);
                using (BookingCarEntities db = new BookingCarEntities())
                {
                    var user = db.Users.Where(x => x.Account == account).FirstOrDefault();
                    if (user != null)
                    {
                        if (user.PasswordHash == password)
                        {
                            viewModel.Account = user.Account;
                            viewModel.Name = user.Name;
                            viewModel.IsLoginValid = true;
                            //viewModel.FacID = "未知";
                            //viewModel.UserTitle = user.UserTitle;
                        }
                        else
                        {
                            viewModel.ErrorMessage = "密碼錯誤";
                        }
                    }
                    else
                    {
                        viewModel.ErrorMessage = string.Format("使用者:{0}不存在", account);
                    }
                }
            }
            catch (Exception ex)
            {

                viewModel.ErrorMessage = ex.Message;
                _logger.Error("發生錯誤:{0}", ex.Message);
            }
            finally
            {
                _logger.Debug("end");
            }
            
            return viewModel;
        }
    }
}
