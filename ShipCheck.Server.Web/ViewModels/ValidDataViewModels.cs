using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShipCheck.Server.Web.ViewModels
{
    public class ValidDataViewModel
    {
        public ValidDataParameter Parameter { get; set; }
        public ValidDataResult ValidDataResult { get; set; }
    }

    /// <summary>
    /// 各廠處產品需要製作的時間
    /// </summary>
    public class ValidDataResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public string FALL_SERIAL { get; set; }
        public string UP_SILK_NO { get; set; }
        public string PROD_SPEC { get; set; }
        public string SILK_MINS { get; set; }

        public string PROD_NO { get; set; }
        public string PROD_TIME { get; set; }
    }

    public class ValidDataParameter
    {
        //string type ,string facID ="",bool? isDone = null
        public string Type { get; set; }
        public string FacID { get; set; }
        public string TwistingMachNo { get; set; }
        

        public ValidDataParameter()
        {
            this.FacID = "";
        }
    }
}