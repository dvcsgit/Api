using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using NLog;

namespace ShipCheck.Server.Web.Library
{
    /// <summary>
    /// 給api 共用的方法
    /// </summary>
    public class ApiUtils
    {
        protected static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 處理Android 丟上來的時間
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTime? stringTimeToDataTimeParse(string source)
        {
            DateTime? result = null;
            if (!string.IsNullOrEmpty(source))
            {
                try
                {
                    result = DateTime.ParseExact(source, Resource.StrDateTimeFormat, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {


                }

            }
            return result;
        }

    }
}