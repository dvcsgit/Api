using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Http;
using Ionic.Zip;
using NLog;
using ShipCheck.Server.DataSource;
using ShipCheck.Server.Web.Areas.WebApi.Models;
using ShipCheck.Server.Web.Library;

namespace ShipCheck.Server.Web.Areas.WebApi.Controllers
{
    public class UploadController : UploadBaseController
    {
        private static readonly string CONST_NAME = "UploadAPI";
        protected static NLog.Logger _logger = LogManager.GetLogger(CONST_NAME);
        private const string yyyyMMddhhmmss = "yyyy-MM-dd hh:mm:ss";
        public UploadController()
            : base(CONST_NAME, "Inventory")
        {

        }

        [HttpPost]
        public HttpResponseMessage Sync()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            _logger.Info("{0} start", CONST_NAME);
            try
            {

                var httpRequest = HttpContext.Current.Request;

                if (httpRequest.Form.Count != 0)
                {
                    string jsonStringValue = httpRequest.Form[0];
                    _logger.Info("jsonStringValue :{0}", jsonStringValue);
                    
                    ////處理附檔
                    //var postedFile = httpRequest.Files[0];

                    //var guid = Guid.NewGuid().ToString();

                    //var folder = _uploadPath;

                    //Directory.CreateDirectory(folder);

                    //var fileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.ToString("MMdd"), formInput.StationCode, guid, "Inventory.Upload.zip");

                    //postedFile.SaveAs(Path.Combine(folder, fileName));
                    String fakeAccount = "Admin";
                    List<string> uploadFileList = base.PutUploadFileToTempTask(httpRequest, fakeAccount);
                    if (uploadFileList.Count > 0)
                    {
                        Task.Run(() => processDataAsync(uploadFileList, fakeAccount));
                        // return result
                        response = Request.CreateResponse(HttpStatusCode.Created, new { Success = true, Message = "已接收到檔案,主機處理中" });

                    }
                    else
                    {
                        string system_no_file = "偵測不到有檔案上傳的痕跡";
                        response = Request.CreateResponse(HttpStatusCode.BadRequest, new { Success = false, Message = system_no_file });
                    }


                    response = Request.CreateResponse(HttpStatusCode.OK, new { IsSuccess = true, Message = "Success!" });

                }
                else
                {
                    _logger.Info("httpRequest.Form.Count == 0");
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError, new { IsSuccess = false });
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error:{0}", ex.Message);
            }
            finally
            {
                _logger.Info("{0} end", CONST_NAME);
            }

            return response;
        }


       
        /// <summary>
        /// 將前端送來的檔案丟到暫存檔 允許多個檔案
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private List<string> putUploadFileToTempTask(HttpRequest request, string account)
        {
            var result = new List<string>();
            var saveFileName = string.Format("Upload_{0}_{1}.zip", account, Guid.NewGuid());
            if (request.Files.Count > 0)
            {
                foreach (string file in request.Files)
                {
                    var postedFile = request.Files[file];
                    var filePath = System.IO.Path.Combine(_uploadPath, saveFileName);
                    postedFile.SaveAs(filePath);
                    result.Add(filePath);
                }
            }
            return result;
        }



        
        /// <summary>
        /// 建立目錄
        /// </summary>
        /// <param name="path"></param>
        private static void createDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 資料處理流程 非同步
        /// 錯誤流程 自己紀錄
        /// </summary>
        /// <param name="uploadFileList"></param>
        /// <param name="account"></param>
        private async void processDataAsync(List<string> uploadFileList, string account)
        {
            //1.解壓縮
            try
            {
                _logger.Info("processDataAsync start");
                var extractZipList = base.ExtractZipDeepTask(uploadFileList);

                //點檢資料
                List<string> equipCheckFile = new List<string>();
                List<string> errorCodeFile = new List<string>();
                string coreDBFile = "";
                foreach (var extractZip in extractZipList)
                {
                    //2.更新DB資料 
                    //Note:此專案原則檔案只會只有一個
                    //讀取路徑
                    var sqliteDBFiles = Directory.GetFiles(extractZip, "*.db");//只抓取.db檔案
                    foreach (var sqliteDB in sqliteDBFiles)
                    {
                        var fileName = Path.GetFileName(sqliteDB);

                        

                        var dataSource = string.Format("Data Source={0}", sqliteDB);
                        
                        DataTable CopperFoildt = new DataTable();
                        DataTable GlassFibreClothdt = new DataTable();
                        
                        
                        using (SQLiteConnection conn = new SQLiteConnection(dataSource))
                        {
                            var sqlText = "SELECT * FROM CopperFoil WHERE CheckAccount !='' AND CheckTime !='' ";
                            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlText, conn))
                            {
                                adapter.Fill(CopperFoildt);
                            }


                            var sqlFixRawText = "SELECT * FROM GlassFibreCloth WHERE CheckAccount !='' AND CheckTime !='' ";
                            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlFixRawText, conn))
                            {
                                adapter.Fill(GlassFibreClothdt);
                            }

                        
                        }

                        var CopperFoil = CopperFoildt.DataTableToList<CopperFoilItem>();
                        var GlassFibreCloth = GlassFibreClothdt.DataTableToList<GlassFibreClothItem>();

                        processDataToServer(GlassFibreCloth,CopperFoil);

                    }
                }

                


            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                if (ex is Ionic.Zip.ZipException)
                {
                    _logger.Error("{0} 發生錯誤:{1} 檔案無法解壓縮", CONST_NAME, ex.Message);
                }
                else if (ex is DbEntityValidationException)
                {
                    var dbEx = (DbEntityValidationException)ex;
                    var message = CONST_NAME + " 發生錯誤: ";
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            message += string.Format("屬性名稱: {0} 錯誤訊息: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }

                    _logger.Error(message);
                }
                else
                {
                    _logger.Error("{0} 發生錯誤:{1}", CONST_NAME, ex.Message);

                }

            }
            finally
            {
                _logger.Info("processDataAsync end");
            }


        }


        /// <summary>
        /// 將記憶體的資料丟進去資料庫裡
        /// NOTE:因FEMS沒有實體建立key entity不能新增
        /// </summary>
        /// <param name="invSource">盤點</param>
        /// <param name="fixSource">維修</param>
        /// <param name="attendSource">考勤</param>
        /// <param name="parameter"></param>
        private void processDataToServer(List<GlassFibreClothItem> glassfibreClothList, List<CopperFoilItem> copperFoilList)
        {
            var currentDate = DateTime.Now;
            //物件轉成資料庫物件

            using (TransactionScope scope = new TransactionScope())
            {
                using (BookingCarSSISEntities db = new BookingCarSSISEntities())
                {
                    #region gFibreClothList
                    _logger.Debug("GlassFibreCloth => v28caxrf_s2 資料總筆數:{0}", glassfibreClothList.Count);

                    foreach (var item in glassfibreClothList)
                    {
                        var existItem = db.v28caxrf_s2.Where(x => x.ROWIdx == item.ROWIdx).FirstOrDefault();
                        _logger.Debug("RowIdx:{0}", item.ROWIdx);
                        if (existItem != null)
                        {
                            DateTime? checkTime = ApiUtils.stringTimeToDataTimeParse(item.CheckTime);
                            existItem.CheckAccount = item.CheckAccount;
                            existItem.CheckTime = checkTime;
                            _logger.Debug("RowIdx:{0}... has update CheckAccount:{1};CheckTime:{2}", item.ROWIdx, item.CheckAccount, item.CheckTime);
                            db.SaveChanges();
                            
                        }
                        else
                        {
                            _logger.Debug("RowIdx:{0} not found", item.ROWIdx);
                        }
                        
                    }
                    #endregion

                    _logger.Debug("CopperFoil => V28EFQ7J 資料總筆數:{0}", copperFoilList.Count);
                    foreach (var item in copperFoilList)
                    {
                        
                        var existItem = db.V28EFQ7J.Where(x => x.ROWIdx == item.ROWIdx).FirstOrDefault();
                        if (existItem != null)
                        {
                            DateTime? checkTime = ApiUtils.stringTimeToDataTimeParse(item.CheckTime);
                            existItem.CheckAccount = item.CheckAccount;
                            existItem.CheckTime = checkTime;
                            _logger.Debug("RowIdx:{0}... has update CheckAccount:{1};CheckTime:{2}", item.ROWIdx, item.CheckAccount, item.CheckTime);
                            db.SaveChanges();
                        } else
                        {
                            _logger.Debug("RowIdx:{0} not found", item.ROWIdx);
                        }

                    }

                }

                scope.Complete();
            }

        }
    }
}