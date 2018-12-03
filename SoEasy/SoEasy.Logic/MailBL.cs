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
    /// 邮件业务类
    /// </summary>
    public class MailBL : BaseBL
    {
        static MailBL instance = null;
        static CommonBL comBL = CommonBL.CreateInstance();
        static object locker = new object();

        private MailBL()
        {

        }
        /// <summary>
        /// 创建一个实例
        /// </summary>
        /// <returns></returns>
        public static MailBL CreateInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    instance = new MailBL();
                }
            }
            return instance;
        }

        /// <summary>
        /// 记录发送出去的邮件
        /// </summary>
        /// <param name="mailInfo">邮件信息实体</param>
        public static void RecordSendedMail(MailInfo mailInfo)
        {
            SysRecMailModel rec = new SysRecMailModel();
            rec.Id = rec.GetGuid();
            rec.Content = mailInfo.Content;
            rec.Mail_Address = mailInfo.ToMail;
            rec.Op_Id = mailInfo.OperaterID;
            rec.Title = mailInfo.Title;
            rec.Op_Ip = mailInfo.OperaterIP;
            if (!comBL.Insert(rec, null))
            {
                Utility.Logger.Error("插入邮件发送记录数据失败");
            }
        }

        /// <summary>
        /// 检查能否发送邮件
        /// </summary>
        /// <param name="mailInfo">邮件信息实体</param>
        public static OPResult CheckMail(MailInfo mailInfo,OPResult opRes)
        {
            opRes.State = Enums.OPState.Success;

            SysRecMailModel qm = new SysRecMailModel();
            qm.Op_Ip = mailInfo.OperaterIP;
            qm.OtherCondition = new NotEqualCondition();
            qm.OtherCondition.AddCondition("OP_Time>=:yesterday", "yesterday", DateTime.Now.AddDays(-1));

            DataTable dt= comBL.Select(qm, "OP_Time", opRes);
            if (dt != null)
            {
                if (dt.Rows.Count >= Vars.MaxMailSendCount)
                {
                    opRes.State = Enums.OPState.Fail;
                    opRes.Data = "您发送的邮件次数过于频繁,请24小时后再试!";
                }

                if (dt.Rows.Count > 0)
                {
                    DateTime privTime = Utility.GetValidData(dt.Rows[0]["OP_Time"], DateTime.MinValue);
                    if (privTime > DateTime.Now.AddMinutes(-1))
                    {
                        opRes.State = Enums.OPState.Fail;
                        opRes.Data = "您发送的邮件频率过快,请1分钟后再试!";
                    }
                }
            }

            return opRes;
        }
    }
}
