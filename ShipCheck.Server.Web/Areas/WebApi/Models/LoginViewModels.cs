using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShipCheck.Server.Web.Areas.WebApi.Models
{
    public class UserLoginInfo
    {
        /// <summary>
        /// 是否成功登入
        /// </summary>
        public bool IsLoginValid { get; set; }

        /// <summary>
        /// 無法登入錯誤訊息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 成功登入後-帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 成功登入後-姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 廠代號
        /// </summary>
        public string FacID { get; set; }

        /// <summary>
        /// 使用者類型
        /// </summary>
        public string UserTitle { get; set; }
    }
}