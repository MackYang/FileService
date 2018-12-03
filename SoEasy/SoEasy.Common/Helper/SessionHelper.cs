using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SoEasy.Common
{
    /// <summary>
    /// Session对象的辅助类
    /// </summary>
    /// <typeparam name="T">要存入Session中的类型</typeparam>
    public class SessionHelper<T>
    {
        /// <summary>
        /// 从Session中获取一个对象
        /// </summary>
        /// <param name="key">对象的键</param>
        /// <returns></returns>
        public static T GetSessionObject(string key)
        {
            try
            {
                object obj = HttpContext.Current.Session[key];
                if (obj == null)
                    return default(T);
                else
                    return (T)obj;
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("获取Session数据时发生异常sessionKey=" + key + ",exMsg=：" + ex.Message);
                return default(T);
            }

        }

        /// <summary>
        /// 将某类型的对象存入Session中
        /// </summary>
        /// <param name="key">对象的键</param>
        /// <param name="obj">对象的值</param>
        public static void SetSessionObject(string key, T obj)
        {
            HttpContext.Current.Session[key] = obj;

        }

    }
}
