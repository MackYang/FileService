using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoEasy.Common
{
    public class Vars
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public static string CompanyName;
        /// <summary>
        /// 网站名称
        /// </summary>
        public static string SiteName;
        /// <summary>
        /// 分页大小
        /// </summary>
        public static int PageSize;
        /// <summary>
        /// 邮件服务器地址
        /// </summary>
        public static string EmailHost;
        /// <summary>
        /// 邮件账号
        /// </summary>
        public static string EmailID;
        /// <summary>
        /// 邮件密码
        /// </summary>
        public static string EmailPassword;
        /// <summary>
        /// 邮件模板
        /// </summary>
        public static string EmailTemplate;
        /// <summary>
        /// IP信息查询接口URL
        /// </summary>
        public static string IPQueryURL;
        /// <summary>
        /// 网站域名,带http和3w
        /// </summary>
        public static string DoMain;
        /// <summary>
        /// 网站底部的版权及备案信息
        /// </summary>
        public static string SiteFooterInfo;
        /// <summary>
        /// 所使用的数据库类型
        /// </summary>
        public static string DBType;
        /// <summary>
        /// 图片上传的基础路径
        /// </summary>
        public static string ImageUpLoadRootPath;
        /// <summary>
        /// 系统ID
        /// </summary>
        public static string SystemID;
        /// <summary>
        /// 异常通知邮件,发生异常时将将此邮件发送通知
        /// </summary>
        public static string ExceptionNotifyEmail;
        /// <summary>
        /// 缓存文件存放路径
        /// </summary>
        public static string CacheFilePath;
        /// <summary>
        /// IP白名单列表,列表中的IP在发送邮件和短信时不受次数限制
        /// </summary>
        public static List<string> IPWhiteList;
        /// <summary>
        /// 同一IP24小时内可以调用系统发送多少封邮件
        /// </summary>
        public static int MaxMailSendCount;
        /// <summary>
        /// 同一IP24小时内可以调用系统发送多少条短信
        /// </summary>
        public static int MaxSMSSendCount;
        /// <summary>
        /// 热线电话
        /// </summary>
        public static string HotLine;
        /// <summary>
        /// 发送短信的API 
        /// </summary>
        public static string SMSAPI;
      
    }
}
