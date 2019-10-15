using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShipCheck.Server.Web.Areas.WebApi.Models
{
    public class MachineItem
    {
        /// <summary>
        /// [SEQ_NO]
        /// </summary>
        public string RecordKey { get; set; }

        /// <summary>
        /// 廠處
        /// </summary>
        public string FacID { get; set; }

        /// <summary>
        /// 撚絲機號
        /// </summary>
        public string TwistingMachNo { get; set; }


        /// <summary>
        /// 錠位
        /// </summary>
        public string RegFlagNo { get; set; }

        /// <summary>
        /// 掛牌時間 異常時間
        /// </summary>
        public string ExceptionTime { get; set; }

        /// <summary>
        /// 異常代號
        /// </summary>
        public string ExceptionID { get; set; }

        /// <summary>
        /// 異常中文
        /// </summary>
        public string ExceptionName { get; set; }



        /// <summary>
        /// 目前正在運行的產品
        /// </summary>
        public string CurrentProdNo { get; set; }

        /// <summary>
        /// 目前正在運行的產品
        /// </summary>
        public string CurrentStartTime { get; set; }

        /// <summary>
        /// 預計停機時間
        /// </summary>
        public string EstimateStopTime { get; set; }

        /// <summary>
        /// 是否已經處理做 True => 有做過了 False=> 尚未做
        /// </summary>
        public bool IsDone { get; set; }

        /// <summary>
        /// 修的時間
        /// </summary>
        public string RepairTime { get; set; }

        /// <summary>
        /// 修的人
        /// </summary>
        public string RepairPerson { get; set; }

        /// <summary>
        /// 轉單時間
        /// </summary>
        public string TransTime { get; set; }

        /// <summary>
        /// 轉的人
        /// </summary>
        public string TransPerson { get; set; }

        public MachineItem()
        {
            //this.EstimateStopTime = "NA";
        }

    }

    /// <summary>
    /// 各廠處產品需要製作的時間
    /// </summary>
    public class TempFacIDSilkProTime
    {
        public string FAC_ID { get; set; }
        public string UP_SILK_NO { get; set; }
        public string PROD_NO { get; set; }
        public int PROD_TIME { get; set; }
        public string TWISTING_MACH_NO { get; set; }
    }

    public class MachineParameter
    {
        //string type ,string facID ="",bool? isDone = null
        public string Type { get; set; }
        public string FacID { get; set; }
        public bool? IsDone { get; set; }
        

        public MachineParameter()
        {
            this.FacID = "";
        }
    }
}