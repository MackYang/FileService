using SoEasy.DB;
using SoEasy.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoEasy.Logic
{
    /// <summary>
    /// 地址业务操作类
    /// </summary>
    public class AddressBL : BaseBL
    {
        static AddressBL instance = null;
        static CommonBL comBL = CommonBL.CreateInstance();
        static object locker = new object();
        static DataTable dtAddress = null;
        private AddressBL()
        {
            SysAddressModel s = new SysAddressModel();
            dtAddress = dao.Select(s, null, "Code,Parent_Code,Name", "asc");
        }

        /// <summary>
        /// 创建一个实例
        /// </summary>
        /// <returns></returns>
        public static AddressBL CreateInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    instance = new AddressBL();
                }
            }
            return instance;
        }

        /// <summary>
        /// 获取直接下级地址列表
        /// </summary>
        /// <param name="currentCode">当前地址编码</param>
        /// <returns></returns>
        public DataTable GetSubAddressList(string currentCode)
        {
            if (!string.IsNullOrWhiteSpace(currentCode))
            {
                return dtAddress.SelectReturnTable("Parent_Code='" + currentCode + "'", null);
            }
            return null;
        }

        /// <summary>
        /// 根据地址编码获取地址名称
        /// </summary>
        /// <param name="code">地址编码</param>
        /// <returns></returns>
        public string GetName(string code)
        {
            if (!string.IsNullOrWhiteSpace(code))
            {
                DataRow[] drs = dtAddress.Select("Code='" + code + "'");
                if (drs != null && drs.Length == 1)
                {
                    return drs[0]["Name"].ToString();
                }
            }
            return "";
        }
    }
}
