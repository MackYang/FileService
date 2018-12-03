using SoEasy.Common;
using SoEasy.DB;
using SoEasy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoEasy.Logic
{
    /// <summary>
    /// 在线用户业务类
    /// </summary>
    public class OnlineUserBL:BaseBL
    {
        static OnlineUserBL instance = null;
        static CommonBL comBL = CommonBL.CreateInstance();
        static object locker = new object();
        private OnlineUserBL()
        { }

        public static OnlineUserBL CreateInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    instance = new OnlineUserBL();
                }
            }
            return instance;
        }

        /// <summary>
        /// 添加在线用户
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="userInfo">用户信息</param>
        /// <param name="platform">平台名称</param>
        public void AddOnlineUser(string ip,UserInfo userInfo,string platform)
        {
            SysOnlineUserModel m = new SysOnlineUserModel();
            m.Id = m.GetGuid();
            m.Ip = ip;
            m.Platform = platform;
            IPInfo ipInfo = NetHelper.QueryIPInfoIP138(ip, null);
            if (ipInfo != null)
            {
                m.Ip_Info = ipInfo.FullAddress;
            }
            if (userInfo != null)
            {
                m.User_Id = userInfo.UserID;
                m.User_Name = userInfo.UserName;
            }
            if (!comBL.Insert(m, null))
            {
                Utility.Logger.Error("添加在线用户失败");
            }
        }
        /// <summary>
        /// 删除在线用户
        /// </summary>
        /// <param name="userID">用户ID,如果没有可传NULL</param>
        /// <param name="ip">用户IP</param>
        public void DeleteOnlineUser(string userID, string ip)
        {
            SysOnlineUserModel m = new SysOnlineUserModel();
            if (!string.IsNullOrWhiteSpace(userID))
            {
                m.User_Id = userID;
            }
            else
            {
                m.Ip = ip;
            }
            if (!comBL.Delete(m, null))
            {
                Utility.Logger.Error("删除在线用户失败");
            }
        }

        /// <summary>
        /// 清除所有在线用户
        /// </summary>
        public void ClearAllOnlineUser()
        {
            SysOnlineUserModel m = new SysOnlineUserModel();
            m.OtherCondition = new Model.BaseEntity.NotEqualCondition { ConditionSQL="1=1" };
            if (!comBL.Delete(m, null))
            {
                Utility.Logger.Error("清除所有在线用户失败");
            }
        }

    }
}
