﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShipCheck.Server.Web {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ShipCheck.Server.Web.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 確定要刪除所選取資料?.
        /// </summary>
        internal static string MsgAllDelete {
            get {
                return ResourceManager.GetString("MsgAllDelete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 請至少選取一筆資料.
        /// </summary>
        internal static string MsgAllDeleteAtLeast {
            get {
                return ResourceManager.GetString("MsgAllDeleteAtLeast", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 確定要刪除該筆資料?.
        /// </summary>
        internal static string MsgDelete {
            get {
                return ResourceManager.GetString("MsgDelete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 刪除成功!.
        /// </summary>
        internal static string MsgDeleteSucess {
            get {
                return ResourceManager.GetString("MsgDeleteSucess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 系統發生錯誤.
        /// </summary>
        internal static string MsgError {
            get {
                return ResourceManager.GetString("MsgError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 資料匯出失敗.
        /// </summary>
        internal static string MsgErrorExport {
            get {
                return ResourceManager.GetString("MsgErrorExport", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 密碼不正確.
        /// </summary>
        internal static string Password {
            get {
                return ResourceManager.GetString("Password", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to yyyy-MM-dd.
        /// </summary>
        internal static string StrDateFormat {
            get {
                return ResourceManager.GetString("StrDateFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0:yyyy-MM-dd}.
        /// </summary>
        internal static string StrDateFormatForModel {
            get {
                return ResourceManager.GetString("StrDateFormatForModel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to yyyy-MM-dd HH:mm:ss.
        /// </summary>
        internal static string StrDateTimeFormat {
            get {
                return ResourceManager.GetString("StrDateTimeFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0:yyyy-MM-dd HH:mm:ss}.
        /// </summary>
        internal static string StrDateTimeFormatForModel {
            get {
                return ResourceManager.GetString("StrDateTimeFormatForModel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 暫無資料.
        /// </summary>
        internal static string StrEmptyRow {
            get {
                return ResourceManager.GetString("StrEmptyRow", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 該{0}已存在.
        /// </summary>
        internal static string StrExistFormat {
            get {
                return ResourceManager.GetString("StrExistFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 總數: {0} 筆 目前顯示第 {1} 筆至第 {2} 筆。.
        /// </summary>
        internal static string StrPagerInfo {
            get {
                return ResourceManager.GetString("StrPagerInfo", resourceCulture);
            }
        }
    }
}
