using Common;
using MiddleWare;
using Model;
using SoEasy.Common;
using SoEasy.DB;
using SoEasy.Logic;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;


namespace FileOutAPI.Controllers
{

    public class FileController : Controller
    {
        static Cache cache = HttpRuntime.Cache;
        static CommonBL bl = CommonBL.CreateInstance();




        public FileController()
        {
            if (!CommonVarsEx.IsLoadSystemList) {

                LoadSystemTable(null);
                CommonVarsEx.IsLoadSystemList = true;
            }
        }



        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        /// <param name="addWater">是否添加水印true或1为是</param>
        /// <param name="waterFontSize">水印字号大小</param>
        /// <param name="waterText">水印内容</param>
        /// <returns>操作结果的json字符串</returns>
        [HttpGet]
        [ArgsFilter]
        public ActionResult GetFile(string fileId, int? width, int? height, int? addWater, int? waterFontSize, string waterText, string systemId)
        {
            FileResult response = null;

            OPResult opRes = Utility.CheckArgsNotNullOrEmpty("fileId", fileId);

            if (opRes.State == Enums.OPState.Success) {
                opRes.State = Enums.OPState.Fail;

                string cacheKey = fileId + "|" + width + "|" + height + "|" + addWater + "|" + waterFontSize + "|" + waterText;

                //目标文件物理路径
                string destFilePhysicsPath = null;
                //目标文件名称
                string destFileName = null;
                //目标文件响应内容类型
                string destContentType = null;

                //判断缓存中是否存在
                object o = cache.Get(cacheKey);
                if (o != null) {
                    var cacheObj = (OPResultCache)o;
                    destFilePhysicsPath = cacheObj.FilePhysicalFullPath;
                    destFileName = cacheObj.FileName;
                    destContentType = cacheObj.ContentType;
                }
                else {

                    BizFileModel model = new BizFileModel();
                    model.Id = fileId;
                    model.Biz_System_Id = systemId;

                    model = bl.SelectFirst(model, opRes, null);
                    if (model != null) {

                        string filePhysicsPath = Path.Combine(VarsEx.FileUploadRootPath, model.Url.Replace("/", @"\").TrimStart('\\'));
                        bool isImage = ImageHelper.IsImage(model.File_Name);
                        if (isImage) {//如果是图片才做特殊处理

                            filePhysicsPath = GetImage(model, filePhysicsPath, width, height, addWater, waterFontSize, waterText, opRes);
                        }

                        destFilePhysicsPath = filePhysicsPath;
                        destFileName = model.File_Name;
                        destContentType = model.Content_Type;

                        CacheItemRemovedCallback onRemove = new CacheItemRemovedCallback(DeleteCacheFile);
                        OPResultCache cacheData = new OPResultCache { FilePhysicalFullPath = filePhysicsPath, ContentType = destContentType, FileName = destFileName, IsImage = isImage };
                        cache.Insert(cacheKey, cacheData, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60), CacheItemPriority.High, onRemove);

                    }
                    else {

                        Utility.Logger.Error($"文件信息不存在,fileId={fileId},systemId={systemId}");
                        opRes.Data = $"文件信息不存在,fileId={fileId},systemId={systemId}";
                    }
                }

                if (!string.IsNullOrWhiteSpace(destFilePhysicsPath)) {

                    if (System.IO.File.Exists(destFilePhysicsPath)) {

                        try {

                            using (FileStream fs = new FileStream(destFilePhysicsPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                                long size = fs.Length;//获取文件大小
                                byte[] array = new byte[size];
                                fs.Read(array, 0, array.Length);//将文件读到byte数组中
                                fs.Close();
                                fs.Dispose();
                                
                                FileResult result = File(array, destContentType);
                                result.FileDownloadName = destFileName;
                                response = result;
                                array = null;
                                GC.Collect();
                            }
                        }
                        catch (Exception ex) {
                            Utility.Logger.Error("将文件转换成响应流的时候发生异常:" + ex.Message, ex);
                            opRes.Data = "将文件转换成响应流的时候发生异常:" + ex.Message;
                            opRes.State = Enums.OPState.Exception;
                        }
                    }
                    else {
                        Utility.Logger.Error($"读取文件输出给响应流时,找不到fileId={fileId}的物理文件{destFilePhysicsPath},cacheKey={cacheKey}");
                        opRes.Data = "该文件不存在";
                    }
                }
            }



            if (response != null) {
                return response;
            }
            else {
                return Json(opRes, JsonRequestBehavior.AllowGet);
            }
        }


        private string GetImage(BizFileModel model, string filePhysicsPath, int? width, int? height, int? addWater, int? waterFontSize, string waterText, OPResult opRes)
        {
            string fileId = model.Id;

            string guid = Guid.NewGuid().ToString();
            //用GUID做缓存图片的名称
            string fileGuid = guid + model.File_Type;

            //最终图片的物理存放路径
            string imagePhysicsPath = Path.Combine(VarsEx.ImageCacheRootPath, model.Biz_System_Id, DateTime.Now.ToString("yyyy-MM-dd"), fileGuid);

            //原始图片
            string originalImage = filePhysicsPath;
            lock (string.Intern(originalImage)) {
                if (System.IO.File.Exists(originalImage)) {
                    List<string> listTempFile = new List<string>();

                    bool isChange = false;
                    if (width > 1 || height > 1 || (addWater == 1)) {
                        if (addWater == 1 && waterFontSize > 0 && waterFontSize < 72 && !string.IsNullOrWhiteSpace(waterText))//加水印
                        {
                            //临时水印图片存放路径
                            string waterTmpFileName = imagePhysicsPath.Replace(".", "w.");

                            if (ImageHelper.AddWatermarkText(originalImage, waterTmpFileName, (int)waterFontSize, waterText, opRes)) {
                                //将水印图片做为后续缩放的原图片
                                originalImage = waterTmpFileName;
                                listTempFile.Add(waterTmpFileName);
                                isChange = true;
                            }
                            else {
                                Utility.Logger.Error("图片添加水印失败:" + fileId);
                            }
                        }

                        if (width > 0 && height > 0 && width < model.Width && height < model.Height)//缩放
                        {
                            //临时缩放图片存放路径
                            string zoomTmpFileName = imagePhysicsPath.Replace(".", "r.");

                            if (ImageHelper.CreateThumbnail(originalImage, zoomTmpFileName, (int)width, (int)height, opRes)) {
                                originalImage = zoomTmpFileName;
                                listTempFile.Add(zoomTmpFileName);
                                isChange = true;
                            }
                            else {
                                Utility.Logger.Error("图片缩放失败:" + fileId);
                            }

                        }

                    }


                    FileHelper.CreatePathIfNotExists(Path.GetDirectoryName(imagePhysicsPath));
                    if (isChange) {
                        try {
                            System.IO.File.Move(originalImage, imagePhysicsPath);
                            filePhysicsPath = imagePhysicsPath;
                        }
                        catch (Exception ex) {
                            Utility.Logger.Error("移动生成的图片" + originalImage + "到缓存目录" + imagePhysicsPath + "时发生异常:" + ex);
                            opRes.State = Enums.OPState.Exception;
                        }
                    }
                    else {
                        try {
                            System.IO.File.Copy(originalImage, imagePhysicsPath, true);//将生成的文件复制到缓存文件目录
                            filePhysicsPath = imagePhysicsPath;
                        }
                        catch (Exception ex) {
                            Utility.Logger.Error("复制生成的图片" + originalImage + "到缓存目录" + imagePhysicsPath + "时发生异常:" + ex);
                            opRes.State = Enums.OPState.Exception;
                        }
                    }

                    foreach (string item in listTempFile) {
                        try {
                            if (System.IO.File.Exists(item)) {
                                System.IO.File.Delete(item);
                            }
                        }
                        catch (Exception ex) {
                            Utility.Logger.Error("删除临时生成的图片时发生异常:" + ex);
                        }
                    }
                }
                else {
                    Utility.Logger.Error($"对图片进行加工处理时,找不到fileId={fileId}的物理文件{originalImage}");
                    opRes.Data = "图片文件不存在";
                }
            }

            return filePhysicsPath;
        }

        private void DeleteCacheFile(string key, Object value, CacheItemRemovedReason reason)
        {
            //删除实际生成的文件
            OPResultCache res = (OPResultCache)value;

            string cacheFileName = res.FilePhysicalFullPath;
            if (res.IsImage && System.IO.File.Exists(cacheFileName)) {
                try {
                    System.IO.File.Delete(cacheFileName);
                }
                catch (Exception ex) {
                    Utility.Logger.Error("移除图片缓存时,删除生成的缓存图片" + cacheFileName + "发生异常:" + ex);
                }
            }

        }


        class OPResultCache
        {
            /// <summary>
            /// 缓存文件的真实路径
            /// </summary>
            public string FilePhysicalFullPath { get; set; }

            public bool IsImage { get; set; }

            public string FileName { get; set; }

            public string ContentType { get; set; }

        }


        private void LoadSystemTable(OPResult opRes)
        {
            BizSystemModel qm = new BizSystemModel();
            qm.Enable = 1;
            CommonVarsEx.SystemTable = bl.Select(qm, null, opRes, null, null);
        }

        [HttpPost]
        public string RefreshSystemList(string systemId)
        {
            OPResult opRes = new OPResult { State = Enums.OPState.Success };

            if (systemId == Vars.SystemID) {
                LoadSystemTable(opRes);
            }
            else {
                opRes.State = Enums.OPState.Fail;
                opRes.Data = "systemId不正确";
            }

            return opRes.ToJsonString();
        }



    }
}
