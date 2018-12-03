using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SoEasy.Common
{
    public class FileHelper
    {
        /// <summary>  
        /// 写入文件  
        /// </summary>  
        /// <param name="filePath">文件名</param>  
        /// <param name="content">文件内容</param>  
        public static bool WriteFile(string filePath, string content, OPResult opRes)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                bool flag = false;
                try {
                    File.WriteAllText(filePath, content);
                    flag = true;
                }
                catch (Exception ex) {
                    throw new Exception("保存文件时发生异常:" + ex);
                }
                return flag;
            }, opRes, false);
        }

        /// <summary>  
        /// 读取文件  
        /// </summary>  
        /// <param name="filePath">文件路径</param>  
        /// <returns></returns>  
        public static string ReadFile(string filePath, OPResult opRes)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                try {
                    return File.ReadAllText(filePath);
                }
                catch (Exception ex) {
                    throw new Exception("读取文件时发生异常:" + ex);
                }
            }, opRes, false);


        }

        /// <summary>
        /// 获取某目录下的所有文件名称列表
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> GetFileList(string path)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                try {
                    return Directory.GetFiles(path).ToList();
                }
                catch (Exception ex) {
                    throw new Exception("获取" + path + "目录下的文件列表时发生异常:" + ex);
                }
            }, null, false);

        }

        /// <summary>
        /// 自动清除24小时以前创建的缓存文件,每一小时运行一次
        /// </summary>
        public static void AutoClearCacheFile()
        {
            string cachePath = HttpContext.Current.Server.MapPath(Vars.CacheFilePath);
            TimeSpan timeSpan = new TimeSpan(1, 0, 0);//1小时 
            while (true) {
                try {
                    List<string> fileList = GetFileList(cachePath);
                    if (fileList != null && fileList.Count > 0) {
                        foreach (string item in fileList) {
                            DateTime cacheTime = File.GetCreationTime(item);
                            if (DateTime.Now.Subtract(new TimeSpan(24, 0, 0)) > cacheTime) {
                                File.Delete(item);
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    Utility.Logger.Error("删除缓存文件时发生异常:" + ex);
                }

                Thread.Sleep(timeSpan);
            }
        }

        /// <summary>
        /// 创建目录(如果目录不存在)
        /// </summary>
        /// <param name="path">要创建的目录</param>
        public static void CreatePathIfNotExists(string path)
        {
            try {
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex) {
                Utility.Logger.Error("检查及创建文件目录时发生异常:" + ex);
            }

        }

        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetExName(string fileName)
        {
            try {
                return fileName.Substring(fileName.LastIndexOf('.'));
            }
            catch (Exception) {

                return "unknowType";
            }

        }
    }
}
