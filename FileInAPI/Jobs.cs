using FileInAPI;
using Model;
using SoEasy.Common;
using SoEasy.Logic;
using SoEasy.Model.BaseEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace ImgInAPI
{
    public class Jobs
    {
        static CommonBL bl = CommonBL.CreateInstance();

        /// <summary>
        /// 删除临时文件
        /// </summary>
        public static void DeleteTempFile()
        {
            BizFileModel qm = new BizFileModel();
            qm.Is_Temp = 1;

            NotEqualCondition nec = new NotEqualCondition();
            nec.AddCondition("create_time<=:yesterday", "yesterday", DateTime.Now.AddDays(-1));
            qm.OtherCondition = nec;

            try {
                DataTable dt = bl.Select(qm, null, null,null);
                if (dt != null && dt.Rows.Count > 0) {

                    foreach (DataRow item in dt.Rows) {

                        string id = item["id"].ToString();
                        string md5 = item["md5"].ToString();
                        string url = item["url"].ToString();

                        if (bl.Count(new BizFileModel { Md5 = md5 }, null) == 1)//如果文件没有被其它记录引用
                       {
                            //删除实际文件
                            string fileName = Path.Combine(VarsEx.FileUpLoadRootPath, url.Replace("/", @"\").TrimStart('\\'));
                            try {
                                System.IO.File.Delete(fileName);
                            }
                            catch (Exception ex) {
                                Utility.Logger.Error("删除文件时出现异常:" + ex);
                            }
                            bl.Delete(new BizFileModel { Id = id }, null);//删除文件信息

                        }
                    }
                }
            }
            catch (Exception ex) {
                Utility.Logger.Error("删除文件时发生异常:" + ex.Message, ex);
            }
        }
    }
}