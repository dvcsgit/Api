using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using FTC.SI.Web.Utils.Utils;
using Ionic.Zip;
using NLog;

namespace ShipCheck.Server.Web.Areas.WebApi.Controllers
{
    public class UploadBaseController : ApiController
    {
        /// <summary>
        /// 拿來記錄API LOG NAME
        /// </summary>
        protected readonly string CONST_NAME;

        protected readonly string _uploadPath = System.Web.Hosting.HostingEnvironment.MapPath(CommonUtils.AppSettings("UploadFolder"));
        protected readonly string _tempPath = System.Web.Hosting.HostingEnvironment.MapPath(CommonUtils.AppSettings("TempFolder"));
        protected readonly string MODULE;
        protected static NLog.Logger _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="const_name"></param>
        /// <param name="module"></param>
        public UploadBaseController(string const_name, string module)
        {
            CONST_NAME = const_name;
            MODULE = module;
            _logger = LogManager.GetLogger(const_name);
        }


        /// <summary>
        /// 將前端送來的檔案丟到暫存檔 允許多個檔案
        /// </summary>
        /// <param name="request"></param>
        /// <param name="account">上傳者</param>
        /// <returns>存放到主機暫存檔的位置</returns>
        protected List<string> PutUploadFileToTempTask(HttpRequest request, string account)
        {
            var uploadFolder = Path.Combine(_uploadPath, MODULE);
            createDirectory(uploadFolder);
            var result = new List<string>();
            var saveFileName = string.Format("{0}_{1}_{2}.zip", CONST_NAME, account, Guid.NewGuid());
            if (request.Files.Count > 0)
            {
                _logger.Debug("前端傳送檔案數量:{0}筆", request.Files.Count);
                foreach (string file in request.Files)
                {
                    var postedFile = request.Files[file];
                    var filePath = System.IO.Path.Combine(uploadFolder, saveFileName);
                    postedFile.SaveAs(filePath);
                    _logger.Debug("已接收到檔案:存檔至:{0}", filePath);
                    result.Add(filePath);
                }
            }
            return result;
        }

        /// <summary>
        /// 解壓縮檔案
        /// </summary>
        /// <param name="tempFileList"></param>
        /// <returns>解壓縮檔案後的存放路徑</returns>
        protected List<string> ExtractZipTask(List<string> tempFileList)
        {
            var result = new List<string>();
            foreach (var tempFile in tempFileList)
            {
                var tempFileName = Path.GetFileNameWithoutExtension(tempFile);
                string zipExtract = Path.Combine(_tempPath, MODULE, tempFileName);
                createDirectory(zipExtract);
                using (ZipFile zip = ZipFile.Read(tempFile))
                {
                    foreach (ZipEntry e in zip)
                    {
                        e.Extract(zipExtract, ExtractExistingFileAction.OverwriteSilently);
                        result.Add(zipExtract);
                    }
                }
            }
            return result;

        }

        protected List<string> ExtractZipDeepTask(List<string> tempFileList)
        {
            var tempResult = new List<string>();
            var result = new List<string>();
            foreach (var tempFile in tempFileList)
            {
                var tempFileName = Path.GetFileNameWithoutExtension(tempFile);
                string zipExtract = Path.Combine(_tempPath, MODULE, tempFileName);
                createDirectory(zipExtract);
                using (ZipFile zip = ZipFile.Read(tempFile))
                {
                    foreach (ZipEntry e in zip)
                    {
                        e.Extract(zipExtract, ExtractExistingFileAction.OverwriteSilently);
                        tempResult.Add(zipExtract);
                    }
                }


                var zipFileList = Directory.GetFiles(zipExtract, "*.zip");//只抓取.db檔案

                foreach (var item in zipFileList)
                {

                    var tempfileName = Path.GetFileNameWithoutExtension(item);

                    //using (ZipFile zip = ZipFile.Read(item))    
                    //{
                    //    foreach (ZipEntry e in zip)
                    //    {
                    //        e.Extract(zipExtract, ExtractExistingFileAction.OverwriteSilently);
                    //        result.Add(zipExtract);
                    //    }
                    //}
                    using (ZipInputStream stream = new ZipInputStream(item))
                    {
                        ZipEntry e;
                        //while ((e = stream.GetNextEntry()) != null)
                        ////foreach( ZipEntry e in zip)
                        //{
                        //    e.Extract(zipExtract, ExtractExistingFileAction.OverwriteSilently);
                        //}

                        while ((e = stream.GetNextEntry()) != null)
                        {
                            int size = 2048;
                            byte[] data = new byte[2048];

                            Console.WriteLine("正在解壓縮: " + e.FileName);
                            var unzipFileName = string.Format("{0}_{1}", tempfileName, e.FileName);
                            // 寫入檔案
                            var realname = Path.Combine(zipExtract, unzipFileName);
                            using (FileStream fs = new FileStream(realname, FileMode.Create))
                            {
                                while (true)
                                {
                                    size = stream.Read(data, 0, data.Length);

                                    if (size > 0)
                                        fs.Write(data, 0, size);
                                    else
                                        break;
                                }

                            }
                        }

                    }



                }
                result.Add(zipExtract);


            }
            return result;

        }

        /// <summary>
        /// 建立目錄
        /// </summary>
        /// <param name="path"></param>
        private void createDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                _logger.Warn("創建目錄:{0}", path);
                Directory.CreateDirectory(path);
            }
        }
    }
}