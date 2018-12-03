using ImgInAPI;
using SoEasy.Common;
using SoEasy.Init;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileInAPI
{
    public class InitConfigEx : InitConfigData
    {
        /// <summary>
        /// 初始化系统变量
        /// </summary>
        /// <param name="configFilePhysicsPath">配置文件物理路径</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <returns>true表示成功</returns>
        public static new bool InitSettings(string configFilePhysicsPath, OPResult opRes, bool throwException = true)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                if (InitConfigData.InitSettings(configFilePhysicsPath, opRes)) {
                    VarsEx.FileUpLoadRootPath = FileUploadRootPath;
                    VarsEx.FileOutUrl = FileOutServerURL;
                    VarsEx.FileMaxSize = FileMaxSize;
                    AutoExecTask();
                    return true;
                }
                return false;
            }, opRes, throwException);
        }

        /// <summary>
        /// 自动执行的任务
        /// </summary>
        private static void AutoExecTask()
        {
            Jobs.DeleteTempFile();

            //每天执行
            TaskHelper.AddTask(TimeSpan.Parse("00:00:00"), Jobs.DeleteTempFile);

            //添加完后别忘了启动任务
            TaskHelper.Start();
        }

        /// <summary>
        /// 最大文件大小,M为单位
        /// </summary>
        private static int FileMaxSize
        {
            get
            {
                return Utility.GetValidData(XMLHelper.GetXmlNodeValue(InitConfigData.configFilePhysicsPath, ConstantEx.FileMaxSize, null), 100);
            }
        }

        /// <summary>
        /// 文件输出服务器地址
        /// </summary>
        private static string FileOutServerURL
        {
            get
            {
                return XMLHelper.GetXmlNodeValue(InitConfigData.configFilePhysicsPath, ConstantEx.FileOutServerURL, null);
            }
        }


        /// <summary>
        /// 文件上传的根目录 
        /// </summary>
        private static string FileUploadRootPath
        {
            get
            {
                return XMLHelper.GetXmlNodeValue(InitConfigData.configFilePhysicsPath, ConstantEx.FileUploadRootPath, null);
            }
        }
    }
}