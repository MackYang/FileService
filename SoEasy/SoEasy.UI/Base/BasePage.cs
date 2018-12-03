using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoEasy.UI.Base
{
    public class BasePage : System.Web.UI.Page
    {
        #region 获取 QueryString,FormValue

        /// <summary>
        /// 获取传入的值
        /// </summary>
        /// <param name="key">查询字符串的Key</param>
        /// <param name="defaultValue">当Key不存在时返回的默认值</param>
        /// <returns>查询字符串的值</returns>
        public string GetQueryString(string key, string defaultValue = "")
        {
            string tmp = Request.QueryString[key];
            return string.IsNullOrWhiteSpace(tmp) ? defaultValue : tmp;
        }


        /// <summary>
        /// 获取传入的值
        /// </summary>
        /// <param name="key">查询字符串的Key</param>
        /// <param name="defaultValue">当Key不存在时返回的默认值</param>
        /// <returns>查询字符串的值</returns>
        public string GetFormValue(string key, string defaultValue = "")
        {
            string tmp = Request.Form[key];
            return string.IsNullOrWhiteSpace(tmp) ? defaultValue : tmp;
        }


        #endregion
    }
}