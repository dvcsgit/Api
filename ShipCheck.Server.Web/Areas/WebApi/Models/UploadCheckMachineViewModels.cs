using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShipCheck.Server.Web.Areas.WebApi.Models
{
    public class UploadCheckMachineItem
    {
        [Display(Name = "帳號")]
        [Required(ErrorMessage = "{0}必填")]
        public string Account { get; set; }

        [Display(Name = "修復編號")]
        [Required(ErrorMessage = "{0}必填")]
        public string RecordKey { get; set; }
    }
}