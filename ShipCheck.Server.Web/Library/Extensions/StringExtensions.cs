using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ShipCheck.Server.Web.Library.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// 將sql 語法 結尾有 AND OR NOT 的語法給移除掉
        /// 用在loop串接AND...語句時
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string SQLRemoveEndSyntax(this string source)
        {
            return Regex.Replace(source, @"(AND|OR|NOT)\z", string.Empty);
        }

    }
}