using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace System.Data
{
    public static class DataRowExtension
    {
        /// <summary>
        /// 返回行数组中的第一行
        /// </summary>
        /// <param name="drs"></param>
        /// <returns></returns>
        public static DataRow First(this DataRow[] drs)
        {
            if (drs == null || drs.Length < 1)
            {
                return null;
            }
            return drs[0];
        }

        /// <summary>
        /// 比较两个行里存放的值是否相等
        /// </summary>
        /// <param name="dr">源行</param>
        /// <param name="drTarget">目标行</param>
        /// <returns>相等返回true</returns>
        static public bool EqualValue(this DataRow dr, DataRow drTarget)
        {
            object[] src = dr.ItemArray;
            object[] dest = drTarget.ItemArray;
            if (src.Length != dest.Length) { return false; }
            for (int i = 0; i < src.Length; i++)
            {
                if (!src[i].Equals(dest[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 行集合中是否包含与目标行值相等的行
        /// </summary>
        /// <param name="drs">行集合</param>
        /// <param name="dtTarget">目标行</param>
        /// <returns>包含返回true</returns>
        static public bool ContainsRow(this List<DataRow> drs, DataRow dtTarget)
        {
            foreach (DataRow item in drs)
            {
                if (item.EqualValue(dtTarget))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
