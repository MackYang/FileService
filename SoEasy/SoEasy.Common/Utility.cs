using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System.Web.UI.WebControls;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Xml;
using System.Xml.XPath;
using log4net;
using System.Drawing;
using System.Web;
using System.Collections;

namespace SoEasy.Common
{

    /// <summary>
    /// 系统辅助工具单例类
    /// </summary>
    public class Utility
    {
        private static ILog log = null;
        private static object locker = new object();
       

        /// <summary>
        /// 获取日志记录对象
        /// </summary>
        public static ILog Logger
        {
            get
            {
                if (log == null)
                {
                    lock (locker)
                    {
                        log = LogManager.GetLogger(typeof(Utility));
                    }
                }
                return log;
            }

        }

        #region 获取当前用户信息

        /// <summary>
        /// 获取当前普通用户信息
        /// </summary>
        /// <returns></returns>
        public static OPResult GetGenUserInfo()
        {
            OPResult res = new OPResult();

            UserInfo userInfo = SessionHelper<UserInfo>.GetSessionObject(Constants.SessionKey_UserInfo);
            if (userInfo != null)
            {
                if (userInfo.UserType == Enums.UserType.GenUser.GetHashCode())
                {
                    res.Data = userInfo;
                    res.State = Enums.OPState.Success;
                }
                else
                {
                    res.Data = "当前用户不是普通用户,不能进行普通用户才能执行的相关操作";
                }

            }
            else
            {
                res.Data = "当前用户未登录";
            }

            return res;
        }


        /// <summary>
        /// 获取当前商家用户信息
        /// </summary>
        /// <returns></returns>
        public static OPResult GetShopUserInfo()
        {
            OPResult res = new OPResult();

            UserInfo userInfo = SessionHelper<UserInfo>.GetSessionObject(Constants.SessionKey_UserInfo);
            if (userInfo != null)
            {
                if (userInfo.UserType == Enums.UserType.ShopUser.GetHashCode() || userInfo.UserType == Enums.UserType.ShopCustomerServiceUser.GetHashCode())
                {
                    res.Data = userInfo;
                    res.State = Enums.OPState.Success;
                }
                else
                {
                    res.Data = "当前用户不是商家用户,不能进行商家用户才能执行的相关操作";
                }

            }
            else
            {
                res.Data = "当前用户未登录";
            }

            return res;
        }

        /// <summary>
        /// 获取当前后台用户信息
        /// </summary>
        /// <returns></returns>
        public static OPResult GetPlatformUserInfo()
        {
            OPResult res = new OPResult();

            UserInfo userInfo = SessionHelper<UserInfo>.GetSessionObject(Constants.SessionKey_UserInfo);
            if (userInfo != null)
            {
                if (userInfo.UserType == Enums.UserType.PlatformUser.GetHashCode())
                {
                    res.Data = userInfo;
                    res.State = Enums.OPState.Success;
                }
                else
                {
                    res.Data = "当前用户不是后台用户,不能进行后台用户才能执行的相关操作";
                }

            }
            else
            {
                res.Data = "当前用户未登录";
            }

            return res;
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        public static OPResult GetCurrentUserInfo()
        {
            OPResult res = new OPResult();

            UserInfo userInfo = SessionHelper<UserInfo>.GetSessionObject(Constants.SessionKey_UserInfo);
            if (userInfo != null)
            {
                res.Data = userInfo;
                res.State = Enums.OPState.Success;
            }
            else
            {
                res.Data = "当前用户未登录";
            }

            return res;
        }

        #endregion

        /// <summary>
        /// 设置当前用户信息
        /// </summary>
        /// <param name="userInfo"></param>
        public static OPResult SetCurrentUserInfo(UserInfo userInfo)
        {
            OPResult opRes = new OPResult();
            try
            {
                SessionHelper<UserInfo>.SetSessionObject(Constants.SessionKey_UserInfo, userInfo);
                opRes.State = Enums.OPState.Success;
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("保存当前用户信息到Sesion中时发生异常:" + ex);
                opRes.State = Enums.OPState.Exception;
            }
            return opRes;


        }

        /// <summary>
        /// 检查参数不能为空
        /// </summary>
        /// <param name="argsDic">参数集合</param>
        /// <returns></returns>
        public static OPResult CheckArgsNotNullOrEmpty(Dictionary<string, string> argsDic)
        {
            OPResult opRes = new OPResult();
            if (argsDic != null && argsDic.Count > 0)
            {
                foreach (string item in argsDic.Keys)
                {
                    if (string.IsNullOrWhiteSpace(argsDic[item]))
                    {
                        opRes.Data = "参数 " + item + " 的值不能为空";
                        break;
                    }
                }
                if (opRes.Data == null)
                {
                    opRes.State = Enums.OPState.Success;
                }
            }
            return opRes;
        }

        /// <summary>
        /// 检查参数不能为空
        /// </summary>
        ///<param name="argsName">参数名</param>
        ///<param name="argsValue">参数值</param>
        /// <returns></returns>
        public static OPResult CheckArgsNotNullOrEmpty(string argsName, string argsValue)
        {
            OPResult opRes = new OPResult();

            if (string.IsNullOrWhiteSpace(argsValue))
            {
                opRes.Data = "参数 " + argsName + " 的值不能为空";
            }

            else
            {
                opRes.State = Enums.OPState.Success;
            }
            return opRes;
        }

        /// <summary>
        /// 获取有效的数据,将objValue的值转换成defValue的类型,如果转换失败,返回defValue
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="objValue">要转换的原数据</param>
        /// <param name="defValue">转换失败时返回的默认值</param>
        /// <returns></returns>
        static public T GetValidData<T>(object objValue, T defValue)
        {
            try
            {
                return (T)Convert.ChangeType(objValue, typeof(T));
            }
            catch
            {

                return defValue;
            }

        }

    }

}
