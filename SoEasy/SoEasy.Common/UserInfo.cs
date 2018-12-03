using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoEasy.Common
{
    /// <summary>
    /// 存放在session中的用户数据
    /// </summary>
    [Serializable]
    public class UserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID;
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName;
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserAlias;
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string PhoneNum;
        /// <summary>
        /// 用户邮件地址
        /// </summary>
        public string Email;
        /// <summary>
        /// 用户类型
        /// </summary>
        public int UserType;

    }
}
