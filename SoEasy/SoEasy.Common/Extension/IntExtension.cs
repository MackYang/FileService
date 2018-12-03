using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    static public class IntExtension
    {
        /// <summary>
        /// 判断某数字是否存在于数组中
        /// </summary>
        /// <param name="x"></param>
        /// <param name="arr"></param>
        /// <returns></returns>
        static public bool In(this int x, int[] arr)
        {
            foreach (int item in arr)
            {
                if (x == item)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
