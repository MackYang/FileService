using SoEasy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileOutAPI
{
    public class VarsEx:Vars
    {
        /// <summary>
        /// 缓存图片存放的网站路径
        /// </summary>
        public static string ImageCacheRootPath;
        /// <summary>
        /// 文件来源的顶级物理路径
        /// </summary>
        public static string FileUploadRootPath;
    }
}