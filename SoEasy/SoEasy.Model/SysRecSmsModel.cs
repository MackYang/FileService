using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using SoEasy.Model.BaseEntity;
using SoEasy.Common;
using System.Data;


namespace SoEasy.Model
{
    /// <summary>
    /// 短信发送记录表
    /// </summary>
    [Serializable]
    public class SysRecSmsModel : Parent
    {
        public override string RequriedFields
        {
            get
            {
                return "Id,Phone_Num,Content,Op_Ip";
            }
        }
        public override System.Collections.Hashtable MappingTableInfo()
        {
            Hashtable ht = new Hashtable();
            ht.Add(Constants.STR_DB_TABLE, "sys_rec_sms");
            ht.Add(Constants.STR_DB_PK, "Id");
            return ht;
        }
        public override Parent GetModelFromDataTable(DataTable dt)
        {
            SysRecSmsModel x = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                x = new SysRecSmsModel();
                DataRow dr = dt.Rows[0];
                x.Id = dr["Id"].ToString();
                x.Phone_Num = dr["Phone_Num"].ToString();
                x.Content = dr["Content"].ToString();
                x.Op_Time = dr["Op_Time"] != DBNull.Value ? DateTime.Parse(dr["Op_Time"].ToString()) : default(DateTime);
                x.Op_Id = dr["Op_Id"].ToString();
                x.Op_Ip = dr["Op_Ip"].ToString();

            }
            return x;
        }



        string id;

        /// <summary>
        ///
        /// </summary>
        [Required]
        [MaxLength(36)]
        public string Id
        {
            get { return id; }
            set { id = value; SetFieldMapping("Id", value); }
        }


        string phone_num;

        /// <summary>
        ///手机号
        /// </summary>
        [Required]
        [MaxLength(15)]
        public string Phone_Num
        {
            get { return phone_num; }
            set { phone_num = value; SetFieldMapping("Phone_Num", value); }
        }


        string content;

        /// <summary>
        ///短信内容
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Content
        {
            get { return content; }
            set { content = value; SetFieldMapping("Content", value); }
        }


        DateTime op_time;

        /// <summary>
        ///发送时间
        /// </summary>
        public DateTime Op_Time
        {
            get { return op_time; }
            set { op_time = value; SetFieldMapping("Op_Time", value); }
        }


        string op_id;

        /// <summary>
        ///发送者ID
        /// </summary>
        [MaxLength(36)]
        public string Op_Id
        {
            get { return op_id; }
            set { op_id = value; SetFieldMapping("Op_Id", value); }
        }


        string op_ip;

        /// <summary>
        ///发送者IP
        /// </summary>
        [Required]
        [MaxLength(15)]
        public string Op_Ip
        {
            get { return op_ip; }
            set { op_ip = value; SetFieldMapping("Op_Ip", value); }
        }



    }
}
