using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using SoEasy.Model.BaseEntity;
using SoEasy.Common;
using System.Data;


namespace SoEasy.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class SysRecMailModel : Parent
    {
        public override string RequriedFields
        {
            get
            {
                return "Id,Mail_Address,Title,Content,Op_Ip";
            }
        }
        public override System.Collections.Hashtable MappingTableInfo()
        {
            Hashtable ht = new Hashtable();
            ht.Add(Constants.STR_DB_TABLE, "sys_rec_mail");
            ht.Add(Constants.STR_DB_PK, "Id");
            return ht;
        }
        public override Parent GetModelFromDataTable(DataTable dt)
        {
            SysRecMailModel x = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                x = new SysRecMailModel();
                DataRow dr = dt.Rows[0];
                x.Id = dr["Id"].ToString();
                x.Mail_Address = dr["Mail_Address"].ToString();
                x.Title = dr["Title"].ToString();
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


        string mail_address;

        /// <summary>
        ///邮件地址
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string Mail_Address
        {
            get { return mail_address; }
            set { mail_address = value; SetFieldMapping("Mail_Address", value); }
        }


        string title;

        /// <summary>
        ///邮件标题
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Title
        {
            get { return title; }
            set { title = value; SetFieldMapping("Title", value); }
        }


        string content;

        /// <summary>
        ///邮件内容
        /// </summary>
        [Required]
        [MaxLength(21000)]
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
