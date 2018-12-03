using SoEasy.Common;
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
    /// 用户业务操作类
    /// </summary>
    public class UserBL : BaseBL
    {

        static CommonBL comBL = CommonBL.CreateInstance();
        static UserBL instance = null;
        static object locker = new object();
        private UserBL()
        { }

        /// <summary>
        /// 创建一个实例
        /// </summary>
        /// <returns></returns>
        public static UserBL CreateInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    instance = new UserBL();
                }
            }
            return instance;
        }

        /// <summary>
        /// 根据用户名，手机号，邮箱等任意一个获取用户实体,排除已删除的用户
        /// </summary>
        /// <param name="X">可为用户名，手机号，邮箱的其中一个</param>
        public SysUserModel GetUserByX(string X, OPResult opRes)
        {
            SysUserModel u = new SysUserModel();
            u.OtherCondition = new Model.BaseEntity.NotEqualCondition();

            u.OtherCondition.AddCondition("Data_State<>2 and (User_Name=:x or Phone_Num=:x or Mail=:x)", "x", X);

            DataTable dt = comBL.Select(u, null, opRes, null);
            if (dt != null && dt.Rows.Count == 1)
            {
                return (SysUserModel)u.GetModelFromDataTable(dt);
            }
            return null;

        }

        /// <summary>
        /// 检查指定的用户名是否已被占用
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>true表示已经被占用</returns>
        public bool IsExistsUserName(string userName, OPResult opRes)
        {
            SysUserModel u = new SysUserModel();
            u.User_Name = userName;

            DataTable dt = comBL.Select(u, "ID", opRes, null);
            return (dt != null && dt.Rows.Count > 0);
        }

        /// <summary>
        /// 检查指定的手机号是否已被占用
        /// </summary>
        /// <param name="phoneNum">手机号</param>
        /// <returns>true表示已经被占用</returns>
        public bool IsExistsPhoneNum(string phoneNum, OPResult opRes)
        {
            SysUserModel u = new SysUserModel();
            u.Phone_Num = phoneNum;

            DataTable dt = comBL.Select(u, "ID", opRes, null);
            return (dt != null && dt.Rows.Count > 0);

        }

        /// <summary>
        /// 检查指定的邮箱是否已被占用
        /// </summary>
        /// <param name="mail">邮件地址</param>
        /// <returns>true表示已经被占用</returns>
        public bool IsExistsEmail(string mail, OPResult opRes)
        {
            SysUserModel u = new SysUserModel();
            u.Mail = mail;

            DataTable dt = comBL.Select(u, "ID", opRes, null);
            return (dt != null && dt.Rows.Count > 0);

        }



    }
}
