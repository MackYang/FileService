using SoEasy.Common;
using SoEasy.DB;
using SoEasy.Model;
using SoEasy.Model.BaseEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoEasy.Logic
{
    /// <summary>
    /// 访问记录业务类
    /// </summary>
    public class VisitRecordBL : BaseBL
    {
        static VisitRecordBL instance = null;
        static CommonBL comBL = CommonBL.CreateInstance();
        static object locker = new object();
        private VisitRecordBL()
        { }

        public static VisitRecordBL CreateInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    instance = new VisitRecordBL();
                }
            }
            return instance;
        }

        /// <summary>
        /// 从DB中查询IP信息
        /// </summary>
        /// <param name="IP">要查询的IP</param>
        /// <returns></returns>
        public IPInfo GetIpInfoFromDB(string IP)
        {
            SysRecVisitModel vr = new SysRecVisitModel();
            vr.Ip = IP;
            NotEqualCondition nec = new NotEqualCondition();
            nec.ConditionSQL = " Create_Time between :startTime and :endTime";
            nec.AddArgs("startTime", DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")));
            nec.AddArgs("endTime", DateTime.Now);

            vr.OtherCondition = nec;
            DataTable dt = dao.Select(vr, "", null, null);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                IPInfo ii = new IPInfo();
                ii.IP = dr["Ip"].ToString();
                ii.City = dr["City"].ToString();
                ii.District = dr["District"].ToString();
                ii.FullAddress = dr["Full_Address"].ToString();
                ii.Province = dr["Province"].ToString();
                ii.Street = dr["Street"].ToString();
                ii.StreetNum = dr["Street_Number"].ToString();
                return ii;
            }
            return null;
        }
        /// <summary>
        /// 添加访问记录
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="platform">调用平台</param>
        public void AddVisitRecord(string ip, int platform)
        {
            SysRecVisitModel vr = new SysRecVisitModel();
            vr.Id = vr.GetGuid();
            vr.Ip = ip;
            vr.Platform = platform;
            IPInfo ipInfo = GetIpInfoFromDB(ip);
            if (ipInfo == null)
            {
                ipInfo = NetHelper.QueryIPInfoIP138(ip, null);
            }

            if (ipInfo != null)
            {
                vr.City = ipInfo.City;
                vr.District = ipInfo.District;
                vr.Full_Address = ipInfo.FullAddress;
                vr.Province = ipInfo.Province;
                vr.Street = ipInfo.Street;
                vr.Street_Number = ipInfo.StreetNum;
            }
            if (!comBL.Insert(vr, null))
            {
                Utility.Logger.Error("添加访问者信息到DB失败.");
            }
        }
    }
}
