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
    /// 短信业务类
    /// </summary>
    public class SMSBL : BaseBL
    {
        static SMSBL instance = null;
        static CommonBL comBL = CommonBL.CreateInstance();
        static object locker = new object();

        private SMSBL()
        {
 
        }

        /// <summary>
        /// 创建一个实例
        /// </summary>
        /// <returns></returns>
        public static SMSBL CreateInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    instance = new SMSBL();
                }
            }
            return instance;
        }

        /// <summary>
        /// 记录发送出去的短信
        /// </summary>
        /// <param name="smsInfo">短信信息实体</param>
        public static void RecordSendedSMS(SMSInfo smsInfo)
        {
            SysRecSmsModel rec = new SysRecSmsModel();
            rec.Id = rec.GetGuid();
            rec.Content = smsInfo.SMSContent;
            rec.Phone_Num = smsInfo.ToPhone;
            rec.Op_Id = smsInfo.OperaterID;
            rec.Op_Ip = smsInfo.OperaterIP;
            if (!comBL.Insert(rec, null))
            {
                Utility.Logger.Error("插入短信发送记录数据失败");
            }
        }

        /// <summary>
        /// 检查能否发送短信
        /// </summary>
        /// <param name="smsInfo">短信信息实体</param>
        public static OPResult CheckSMS(SMSInfo smsInfo,OPResult opRes)
        {
            opRes.State = Enums.OPState.Success;

            SysRecSmsModel qm = new SysRecSmsModel();
            qm.Op_Ip = smsInfo.OperaterIP;
            qm.OtherCondition = new NotEqualCondition();
            qm.OtherCondition.AddCondition("OP_Time>=:yesterday", "yesterday", DateTime.Now.AddDays(-1));

            DataTable dt = comBL.Select(qm, "OP_Time", opRes);
            if (dt != null)
            {
                if (dt.Rows.Count >= Vars.MaxSMSSendCount)
                {
                    opRes.State = Enums.OPState.Fail;
                    opRes.Data = "您发送的短信次数过于频繁,请24小时后再试!";
                }

                if (dt.Rows.Count > 0)
                {
                    DateTime privTime = Utility.GetValidData(dt.Rows[0]["OP_Time"], DateTime.MinValue);
                    if (privTime > DateTime.Now.AddMinutes(-2))
                    {
                        opRes.State = Enums.OPState.Fail;
                        opRes.Data = "您发送的短信频率过快,请2分钟后再试!";
                    }
                }
            }

            return opRes;
        }
    }
}
