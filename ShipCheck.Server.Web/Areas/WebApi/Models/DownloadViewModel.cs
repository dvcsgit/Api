using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShipCheck.Server.Web.Areas.WebApi.Models
{
    public class DownloadViewModel
    {
        public List<GlassFibreClothItem> GlassFibreCloth { get; set; }

        public List<CopperFoilItem> CopperFoil { get; set; }

        public DownloadViewModel()
        {
            this.GlassFibreCloth = new List<GlassFibreClothItem>();
            this.CopperFoil = new List<CopperFoilItem>();
        }
    }

    /// <summary>
    //[CUFN] [nvarchar](90) NULL,
    //[ODNO] [nvarchar](10) NOT NULL,
    //[UNPKWGT] [numeric](7, 3) NULL,
    //[PDID] [nvarchar](5) NULL,
    //[CLHRLNO] [nvarchar](10) NOT NULL,
    //[L] [int] NULL,
    //[GRSWGT] [numeric](7, 3) NULL,
    //[ROWIdx] [int] IDENTITY(1,1) NOT NULL,
    //[DateCreated] [datetime] NULL,
    //[SHA1] [nvarchar](100) NULL,
    //[ateModified] [datetime] NULL,
    //[Flag] [nvarchar](1) NULL,
    //[PrintTime] [datetime] NULL,
    //[CheckAccount] [nvarchar](10) NULL,
    //[CheckTime] [datetime] NULL,
    //[OUTMDAT] [nvarchar](7) NULL,
    /// </summary>
    public class GlassFibreClothItem
    {
        [MaxLength(90)]
        public string CUFN { get; set; }

        [MaxLength(10)]
        public string ODNO { get; set; }

        [MaxLength(20)]
        public string UNPKWGT { get; set; }

        [MaxLength(5)]
        public string PDID { get; set; }

        [MaxLength(10)]
        public string CLHRLNO { get; set; }

        public int? L { get; set; }
        
        [MaxLength(20)]
        public string GRSWGT { get; set; }

        [MaxLength(10)]
        public int ROWIdx { get; set; }

        [MaxLength(20)]
        public string DateCreated { get; set; }

        [MaxLength(20)]
        public string PrintTime { get; set; }

        [MaxLength(10)]
        public string CheckAccount { get; set; }

        [MaxLength(20)]
        public string CheckTime { get; set; }

        [MaxLength(7)]
        public string OUTMDAT { get; set; }

    }

    /// <summary>
    /// 	[CUNO] [nvarchar](4000) NULL,
	///     [OMNO] [nvarchar](24) NULL,
	///     [KD] [nvarchar](90) NULL,
	///     [SPEC] [nvarchar](4000) NULL,
	///     [IKPCNO] [nvarchar](24) NOT NULL,
	///     [L] [numeric](12, 3) NOT NULL,
	///     [NETWGT] [int] NULL,
	///     [ROWIdx] [int] IDENTITY(1,1) NOT NULL,
	///     [DateCreated] [datetime] NULL,
	///     [SHA1] [nvarchar](100) NULL,
	///     [DateModified] [datetime] NULL,
	///     [Flag] [nvarchar](1) NULL,
	///     [PrintTime] [datetime] NULL,
	///     [CheckAccount] [nvarchar](10) NULL,
	///     [CheckTime] [datetime] NULL,
	///     [IKDAT] [nvarchar](7) NULL,
    /// </summary>
    public class CopperFoilItem
    {
        [MaxLength(4000)]
        public string CUNO { get; set; }

        [MaxLength(24)]
        public string OMNO { get; set; }

        [MaxLength(90)]
        public string KD { get; set; }

        [MaxLength(4000)]
        public string SPEC { get; set; }

        [MaxLength(24)]
        public string IKPCNO { get; set; }

        [MaxLength(20)]
        public string L { get; set; }

        public int? NETWGT { get; set; }

        public int ROWIdx { get; set; }

        [MaxLength(20)]
        public string DateCreated { get; set; }

        [MaxLength(20)]
        public string PrintTime { get; set; }

        [MaxLength(10)]
        public string CheckAccount { get; set; }

        [MaxLength(20)]
        public string CheckTime { get; set; }

        [MaxLength(7)]
        public string IKDAT { get; set; }
    }

}