using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 返回yyyy-MM-dd HH:mm:ss 格式的时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToBestString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
