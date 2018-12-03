using SoEasy.Common;
using SoEasy.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using SoEasy.Logic;
using System.Drawing.Imaging;
using Common;
using Model;
using MiddleWare;
using System.Text;
using Newtonsoft.Json;
using ImgInAPI;

namespace FileInAPI.Controllers
{

    public class FileController : Controller
    {
        static CommonBL bl = CommonBL.CreateInstance();
        static long maxFileByte = VarsEx.FileMaxSize * 1024 * 1024;

        public FileController()
        {
            if (!CommonVarsEx.IsLoadSystemList) {

                LoadSystemTable(null);
                CommonVarsEx.IsLoadSystemList = true;
            }
        }


        [HttpPost]
        [ArgsFilter]
        public string AddFiles(string systemId, string createId, int isTempFile = 0)
        {

            OPResult opRes = new OPResult();
            try {

                var fileCount = Request.Files.Count;

                if (fileCount > 0) {

                    List<OPResult> resList = new List<OPResult>();

                    for (int i = 0; i < fileCount; i++) {
                        resList.Add(AddFileInternal(Request.Files[i], systemId, createId, isTempFile));
                    }
                    return JsonConvert.SerializeObject(resList);
                }
                else {
                    opRes.Data = "上传的文件不能为空";
                }
            }
            catch (Exception ex) {

                Utility.Logger.Error("上传多文件时发生异常:" + ex.Message, ex);
                opRes.Data = "上传多文件时发生异常:" + ex.Message;
                opRes.State = Enums.OPState.Exception;

            }
            return opRes.ToJsonString();
        }


        [HttpPost]
        [ArgsFilter]
        public string AddFile(string systemId, string createId, int isTempFile = 0)
        {

            OPResult opRes = new OPResult();
            try {

                if (Request.Files.Count > 0) {

                    var file = Request.Files[0];

                    opRes = AddFileInternal(file, systemId, createId, isTempFile);
                }
                else {
                    opRes.Data = "上传的文件不能为空";
                }
            }
            catch (Exception ex) {

                Utility.Logger.Error("上传单文件时发生异常:" + ex.Message, ex);
                opRes.Data = "上传单文件时发生异常:" + ex.Message;
                opRes.State = Enums.OPState.Exception;

            }
            return opRes.ToJsonString();
        }

        /// <summary>
        /// 内部添加文件的方法
        /// </summary>
        /// <param name="file"></param>
        /// <param name="systemId"></param>
        /// <param name="createId"></param>
        /// <param name="isTempFile">是否是临时文件1是</param>
        /// <returns></returns>
        private OPResult AddFileInternal(HttpPostedFileBase file, string systemId, string createId, int isTempFile)
        {

            OPResult opRes = new OPResult();
            try {

                BizFileModel fileModel = new BizFileModel();
                fileModel.Id = fileModel.GetGuid();
                fileModel.Biz_System_Id = systemId;
                fileModel.Content_Type = file.ContentType;
                fileModel.Create_Id = createId ?? systemId;
                fileModel.File_Name = file.FileName;
                fileModel.File_Size = file.ContentLength;
                fileModel.File_Type = FileHelper.GetExName(fileModel.File_Name);
                fileModel.Is_Temp = isTempFile;

                if (fileModel.File_Size > maxFileByte) {
                    opRes.Data = $"上传的文件不能大于{VarsEx.FileMaxSize}M";

                }

                var streamLength = file.InputStream.Length;
                byte[] streamByteArr = new byte[streamLength];
                file.InputStream.Read(streamByteArr, 0, streamByteArr.Length);
                fileModel.Md5 = GetMD5(streamByteArr);


                BizFileModel qmModel = new BizFileModel { Md5 = fileModel.Md5 };
                BizFileModel oldModel = bl.SelectFirst(qmModel, opRes, "Create_Time", "asc");

                if (oldModel == null) {//如果文件从未上传过

                    string fileUrl = $"/{systemId}/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + fileModel.Id + fileModel.File_Type;


                    int imageWidth = 0;
                    int imageHeight = 0;
                    if (!ImageHelper.IsImage(fileModel.File_Name)) {//如果不是图片,加个扩展名,防止上传恶意文件

                        fileUrl += ".gz_file";

                    }
                    else {

                        Image image = Image.FromStream(file.InputStream);
                        imageWidth = image.Width;
                        imageHeight = image.Height;
                    }

                    fileModel.Url = fileUrl;
                    fileModel.Width = imageWidth;
                    fileModel.Height = imageHeight;

                    //保存文件
                    string filePath = Path.Combine(VarsEx.FileUpLoadRootPath, fileModel.Url.Replace("/", @"\").TrimStart('\\').TrimStart('\\'));
                    FileHelper.CreatePathIfNotExists(Path.GetDirectoryName(filePath));
                    file.SaveAs(filePath);

                }
                else {
                    //文件已经存在,只存引用即可
                    fileModel.Ref_Id = oldModel.Id;
                    fileModel.Url = oldModel.Url;
                    fileModel.Width = oldModel.Width;
                    fileModel.Height = oldModel.Height;

                }

                if (bl.Insert(fileModel, opRes)) {

                    opRes.State = Enums.OPState.Success;
                    opRes.Data = new
                    {
                        FileId = fileModel.Id,
                        AccessUrl = VarsEx.FileOutUrl + $"/file/GetFile?systemId={systemId}&fileId={fileModel.Id}",
                        FileName = file.FileName
                    };
                }

            }
            catch (Exception ex) {

                Utility.Logger.Error("上传文件时发生异常:" + ex.Message, ex);
                opRes.Data = "上传文件时发生异常:" + ex.Message + ",FileName=" + file.FileName;
                opRes.State = Enums.OPState.Exception;
            }
            return opRes;
        }


        public string GetMD5(byte[] bytedata)
        {
            string res = null;
            try {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(bytedata);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++) {
                    sb.Append(retVal[i].ToString("x2"));
                }

                res = sb.ToString();
            }
            catch (Exception ex) {
                Utility.Logger.Error("对byte[]计算MD5时发生异常:" + ex.Message, ex);
            }

            return res;
        }



        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>操作结果的json字符串</returns>
        [HttpPost]
        [ArgsFilter]
        public string DeleteFile(string fileId, string systemId, string createId)
        {
            OPResult opRes = Utility.CheckArgsNotNullOrEmpty("fileId", fileId);
            if (opRes.State == SoEasy.Common.Enums.OPState.Success) {

                opRes.State = SoEasy.Common.Enums.OPState.Fail;

                BizFileModel model = new BizFileModel();
                model.Id = fileId;
                model.Create_Id = createId ?? systemId;

                try {
                    model = bl.SelectFirst(model, opRes, null);
                    if (model != null) {
                        if (bl.Count(new BizFileModel { Md5 = model.Md5 }, opRes) == 1)//如果文件没有被其它记录引用
                        {
                            //删除实际文件
                            string fileName = Path.Combine(VarsEx.FileUpLoadRootPath, model.Url.Replace("/", @"\").TrimStart('\\'));
                            try {
                                System.IO.File.Delete(fileName);
                            }
                            catch (Exception ex) {
                                Utility.Logger.Error("删除文件时出现异常:" + ex);
                                opRes.State = SoEasy.Common.Enums.OPState.Exception;
                                opRes.Data = "删除文件失败.";
                                return opRes.ToJsonString();
                            }

                        }
                        if (bl.Delete(new BizFileModel { Id = fileId }, opRes))//删除文件信息
                        {
                            opRes.State = SoEasy.Common.Enums.OPState.Success;
                        }
                        else {
                            opRes.Data = "删除文件失败.";
                        }

                    }
                    else {
                        opRes.Data = "文件信息不存在.";
                    }
                }
                catch (Exception ex) {

                    Utility.Logger.Error("删除文件时发生异常:" + ex.Message, ex);
                    opRes.Data = "删除文件时发生异常:" + ex.Message;
                    opRes.State = Enums.OPState.Exception;
                }
            }
            return opRes.ToJsonString();
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
