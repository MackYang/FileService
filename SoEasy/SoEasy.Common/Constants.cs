using System;

namespace SoEasy.Common
{
    /// <summary>
    /// 系统常量定义类
    /// </summary>
    public class Constants
    {

        #region XML路径

        /// <summary>
        /// 配置所使用数据库类型的XML路径
        /// </summary>
        public const string DBTypePath = "Config/DBType";
        /// <summary>
        ///配置分页大小的XML路径
        /// </summary>
        public const string PageSizePath = "Config/Page/PageSize";
        /// <summary>
        /// 配置公司名称的XML路径
        /// </summary>
        public const string CompanyNamePath = "Config/CompanyName";

        /// <summary>
        /// 配置网站名称的XML路径
        /// </summary>
        public const string SiteNamePath = "Config/SiteName";

        /// <summary>
        /// 配置网站域名的XML路径
        /// </summary>
        public const string SiteDomain = "Config/DoMain";

        /// <summary>
        /// 配置网站底部版权及备案信息的XML路径
        /// </summary>
        public const string SiteFooterInfo = "Config/SiteFooterInfo";

        /// <summary>
        /// 配置IP查询接口的XML路径
        /// </summary>
        public const string IPQueryPath = "Config/API/IPQuery";

        /// <summary>
        /// 配置图片上传基本路径 的XML路径
        /// </summary>
        public const string ImageUpLoadRootPath = "Config/Image/UpLoadPath";

        /// <summary>
        /// 配置系统ID的XML路径
        /// </summary>
        public const string SystemIDPath = "Config/SystemID";
        /// <summary>
        /// 配置异常通知邮件地址的XML路径
        /// </summary>
        public const string ExceptionNotifyEmailPath = "Config/Notify/ExceptionNotifyEmail";
        /// <summary>
        /// 配置 缓存文件存放路径 的XML路径
        /// </summary>
        public const string CacheFilePath = "Config/CacheFilePath";

        /// <summary>
        /// 配置 IP白名单 的XML路径,被配置的IP在发送邮件和短信时,不受发送次数的限制
        /// </summary>
        public const string IPWhiteListPath = "Config/Limit/IPWhiteList/IP";

        /// <summary>
        ///配置 同一IP24小时内可以调用系统发送多少封邮件 的XML路径
        /// </summary>
        public const string MaxMailSendCountPath = "Config/Limit/MaxMailSendCount";

        /// <summary>
        ///配置 同一IP24小时内可以调用系统发送多少条短信 的XML路径
        /// </summary>
        public const string MaxSMSSendCountPath = "Config/Limit/MaxSMSSendCount";


        #region Emil
        /// <summary>
        /// 配置服务邮件服务器地址的XML路径
        /// </summary>
        public const string EmailHost = "Config/Email/Host";

        /// <summary>
        /// 配置服务邮件账号的XML路径
        /// </summary>
        public const string EmailAccount = "Config/Email/Account";

        /// <summary>
        /// 配置服务邮件密码的XML路径
        /// </summary>
        public const string EmailPassword = "Config/Email/Password";
        /// <summary>
        /// 配置 邮件模板 的XML路径
        /// </summary>
        public const string EmailTemplate = "Config/Email/Template";

        #endregion

        #region HotLine and SMS
        /// <summary>
        /// 配置热线电话的XML路径
        /// </summary>
        public const string HotLinePath = "Config/HotLine";
        /// <summary>
        /// 配置短信接口的XML路径
        /// </summary>
        public const string SMSAPIPath = "Config/SMSAPI";

        #endregion

        #endregion

        #region 系统基础配置

        ///<summary>表名</summary>
        public const String STR_DB_TABLE = "TableName";
        ///<summary>主键</summary>
        public const String STR_DB_PK = "PrimaryKey";

        public const string STR_KEY = "key";
        public const string STR_VALUE = "value";

        ///<summary>系统加密解密安全码</summary>
        public const string SYS_SECURITY_CODE = "YH18213045893Base";
        #endregion

        #region SessionKey
        /// <summary>
        /// 存放用户信息的SessionKey
        /// </summary>
        public const string SessionKey_UserInfo = "UserInfo";
        /// <summary>
        /// 存放验证码的SessionKey
        /// </summary>
        public const string SessionKey_VCode = "VCode";
        /// <summary>
        /// 存放登录失败次数的SessionKey
        /// </summary>
        public const string SessionKey_LoginFailCount = "LoginFailCount";
        /// <summary>
        /// 存放IP地址的SessionKey,在Session中存放IP是为了统计在线匿名用户
        /// </summary>
        public const string SessionKey_IP = "IP";
        /// <summary>
        /// 存放在线人数的ApplicationKey
        /// </summary>
        public const string ApplicationKey_OnLineUserCount = "OnLineUserCount";

        #endregion

        #region 操作结果

        /// <summary>
        /// 通过的操作结果,用于将当前操作结果置于通过状态时用.
        /// </summary>
        static public OPResult OK = new OPResult { State = Enums.OPState.Success };

        static private OPResult _noRight = new OPResult { Data = "您没有对应的数据操作权限", State = Enums.OPState.Fail };
        /// <summary>
        /// 没有数据操作权限时的返回结果
        /// </summary>
        static public OPResult NoRight { get { return _noRight; } }//由于引用类型的常量只能用null作为初始值,所以用只读属性间接实现常量

        static private OPResult _stateErr = new OPResult { Data = "当前数据状态下,您无法进行此操作", State = Enums.OPState.Fail };
        /// <summary>
        /// 数据状态与操作不匹配时的返回结果
        /// </summary>
        static public OPResult StateErr { get { return _stateErr; } }

        static private OPResult _vcodeErr = new OPResult { Data = "验证码不正确或已过期,请重新输入", State = Enums.OPState.Fail };

        /// <summary>
        /// 验证码不正确时的返回结果
        /// </summary>
        static public OPResult VCodeErr { get { return _vcodeErr; } }

        static private OPResult _noData = new OPResult { Data = "数据不存在,请检查参数是否正确", State = Enums.OPState.Fail };
        /// <summary>
        /// 没有数据时返回的结果,特别适用于根据ID获取某数据,而结果为空时
        /// </summary>
        static public OPResult NoData { get { return _noData; } }
        #endregion

        #region 其它字符串常量
        /// <summary>
        /// 空格字符串,目前用于清缓存时缓存参数的默认值,因为任何非空字符串都包括""和string.Empty,所以不能以""作为默认值
        /// </summary>
        public const string Space = " ";
        /// <summary>
        /// 跳转到登录页面时的目标URL
        /// </summary>
        public const string TargetURL = "targetURL";
        #endregion

    }

}
