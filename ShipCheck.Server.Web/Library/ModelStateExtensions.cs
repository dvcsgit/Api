using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace ShipCheck.Server.Web.Library
{
    public static class ModelStateExtensions
    {
        /// <summary>
        /// 將 ModelState的Error轉成 IEnumerable<string>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetErrors(this ModelStateDictionary source)
        {
            return source.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }

        /// <summary>
        /// 將 ModelState的Error轉成 string
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetErrorsString(this ModelStateDictionary source)
        {
            return string.Join(";", source.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage)));
        }

    }
}