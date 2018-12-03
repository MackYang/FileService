using SoEasy.Common;
using SoEasy.Init;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileOutAPI
{
    public class InitConfigDataEx : InitConfigData
    {

        /// <summary>
        /// 初始化系统变量
        /// </summary>
        /// <param name="configFilePhysicsPath">配置文件物理路径</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <returns>true表示成功</returns>
        public static bool InitVars(string configFilePhysicsPath,OPResult opRes, bool throwException = true)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                if (InitConfigData.InitSettings(configFilePhysicsPath,opRes))
                {
                    VarsEx.ImageCacheRootPath = ImageCacheRootPath;
                    VarsEx.FileUploadRootPath = FileUploadRootPath;
                    return true;
                }
                return false;
            }, opRes,throwException);
        }

        /// <summary>
        /// 缓存图片存放的网站路径
        /// </summary>
        private static string ImageCacheRootPath
        {
            get
            {
                return XMLHelper.GetXmlNodeValue(InitConfigData.configFilePhysicsPath, ConstantEx.ImageCacheRootPath, null);
            }
        }
        /// <summary>
        /// 获取文件来源的顶级物理路径
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