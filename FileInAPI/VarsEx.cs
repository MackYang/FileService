using SoEasy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileInAPI
{
    public class VarsEx:Vars
    {
        /// <summary>
        /// 文件输出服务器访问地址
        /// </summary>
        static public string FileOutUrl;
        /// <summary>
        /// 文件上传的根路径
        /// </summary>
        static public string FileUpLoadRootPath;
        /// <summary>
        /// 最大文件大小,M为单位
        /// </summary>
        static public int FileMaxSize;
    }
}