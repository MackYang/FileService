using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileOutAPI
{
    public class ConstantEx:SoEasy.Common.Constants
    {
        /// <summary>
        /// 存放文件源的顶级目录,配置中要存放的是物理路径
        /// </summary>
        public const string FileUploadRootPath = "Config/Ex/File/FileUploadRootPath";

        /// <summary>
        /// 缓存中的图片存放顶级目录
        /// </summary>
        public const string ImageCacheRootPath = "Config/Ex/File/ImageCacheRootPath";
    }
}