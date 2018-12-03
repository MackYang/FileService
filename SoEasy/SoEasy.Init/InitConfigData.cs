using SoEasy.Common;
using SoEasy.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SoEasy.Init
{

    /// <summary>
    /// 初始化配置类，用于读取配置文件中的数据,在程序启动时必须调用一次,将值取出存在Vars类对应的变量中
    /// </summary>
    public class InitConfigData
    {
        protected static string configFilePhysicsPath = "";

        /// <summary>
        /// 初始化系统变量
        /// </summary>
        /// <param name="configFilePhysicsPath">配置文件物理路径</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <returns>true表示成功</returns>
        public static bool InitSettings(string configFilePhysicsPath, OPResult opRes, bool throwException = true)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                bool flag = false;
                if (string.IsNullOrWhiteSpace(configFilePhysicsPath))
                {
                    return flag;
                }
                InitConfigData.configFilePhysicsPath = configFilePhysicsPath;
                try
                {
                    Vars.DBType = GetSingleConfigData(Constants.DBTypePath, "");
                    Vars.ImageUpLoadRootPath = GetSingleConfigData(Constants.ImageUpLoadRootPath, "");
                    Vars.PageSize = GetSingleConfigData(Constants.PageSizePath, 20);
                    Vars.IPQueryURL = GetSingleConfigData(Constants.IPQueryPath, "");
                    Vars.EmailHost = GetSingleConfigData(Constants.EmailHost, "");
                    Vars.EmailID = GetSingleConfigData(Constants.EmailAccount, "");
                    Vars.EmailPassword = GetSingleConfigData(Constants.EmailPassword, "");
                    Vars.CompanyName = GetSingleConfigData(Constants.CompanyNamePath, "");
                    Vars.DoMain = GetSingleConfigData(Constants.SiteDomain, "");
                    Vars.SiteName = GetSingleConfigData(Constants.SiteNamePath, "");
                    Vars.SystemID = GetSingleConfigData(Constants.SystemIDPath, "");
                    Vars.ExceptionNotifyEmail = GetSingleConfigData(Constants.ExceptionNotifyEmailPath, "");
                    Vars.CacheFilePath = GetSingleConfigData(Constants.CacheFilePath, "");
                    Vars.SiteFooterInfo = GetSingleConfigData(Constants.SiteFooterInfo, "");
                    Vars.IPWhiteList = GetCollectConfigData(Constants.IPWhiteListPath);
                    Vars.MaxMailSendCount = GetSingleConfigData(Constants.MaxMailSendCountPath, 10);
                    Vars.MaxSMSSendCount = GetSingleConfigData(Constants.MaxSMSSendCountPath, 5);
                    Vars.EmailTemplate = GetSingleConfigData(Constants.EmailTemplate, "");
                    Vars.HotLine = GetSingleConfigData(Constants.HotLinePath, "");
                    Vars.SMSAPI = GetSingleConfigData(Constants.SMSAPIPath, "");

                    string imgUpLoadRootPhysicalPath = HttpContext.Current.Server.MapPath(Vars.ImageUpLoadRootPath);
                    string cachePhysicalPath = HttpContext.Current.Server.MapPath(Vars.CacheFilePath);

                    FileHelper.CreatePathIfNotExists(imgUpLoadRootPhysicalPath);
                    FileHelper.CreatePathIfNotExists(cachePhysicalPath);

                    //订阅事件

                    //发送前检查
                    NetHelper.OnCheckMail += MailBL.CheckMail;
                    NetHelper.OnCheckSMS += SMSBL.CheckSMS;

                    //发送后记录
                    NetHelper.OnRecordMail += MailBL.RecordSendedMail;
                    NetHelper.OnRecordSMS += SMSBL.RecordSendedSMS;

                    flag = true;
                }
                catch (Exception ex)
                {
                    throw new Exception("初始化变量时出错：" + ex);
                }
                return flag;
            }, opRes, throwException);
        }
        /// <summary>
        /// 获取单一的配置数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataPath">数据在XML文件中的路径</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        protected static T GetSingleConfigData<T>(string dataPath, T defValue)
        {
            return Utility.GetValidData(XMLHelper.GetXmlNodeValue(configFilePhysicsPath, dataPath, null), defValue);
        }

        /// <summary>
        /// 获取节点集合的配置数据
        /// </summary>
        /// <param name="dataPath">数据在XML文件中的路径</param>
        /// <returns></returns>
        protected static List<string> GetCollectConfigData(string dataPath)
        {
            return XMLHelper.GetXmlNodeCollectValue(configFilePhysicsPath, dataPath, null);
        }

        /// <summary>
        /// 获取节点集合键值对的配置数据
        /// </summary>
        /// <param name="dataPath">数据在XML文件中的路径</param>
        /// <param name="keyAttrName">存放键的属性名称</param>
        /// <param name="valueAttrName">存放值的属性名称</param>
        /// <returns></returns>
        protected static Hashtable GetHashtableConfigData(string dataPath, string keyAttrName, string valueAttrName)
        {
            return XMLHelper.GetHashtableFromXmlNodeCollectAttr(InitConfigData.configFilePhysicsPath, dataPath, keyAttrName, valueAttrName, null);
        }
    }
}
