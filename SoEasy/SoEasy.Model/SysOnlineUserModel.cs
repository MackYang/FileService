using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using SoEasy.Model.BaseEntity;
using SoEasy.Common;
using System.Data;


namespace SoEasy.Model
{
    /// <summary>
    /// 当前在线用户
    /// </summary>
    [Serializable]
    public class SysOnlineUserModel : Parent
    {
        public override string RequriedFields
        {
            get
            {
                return "Id";
            }
        }
        public override System.Collections.Hashtable MappingTableInfo()
        {
            Hashtable ht = new Hashtable();
            ht.Add(Constants.STR_DB_TABLE, "sys_online_user");
            ht.Add(Constants.STR_DB_PK, "Id");
            return ht;
        }
        public override Parent GetModelFromDataTable(DataTable dt)
        {
            SysOnlineUserModel x = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                x = new SysOnlineUserModel();
                DataRow dr = dt.Rows[0];
                x.Id = dr["Id"].ToString();
                x.Ip = dr["Ip"].ToString();
                x.Ip_Info = dr["Ip_Info"].ToString();
                x.User_Id = dr["User_Id"].ToString();
                x.User_Name = dr["User_Name"].ToString();
                x.Platform = dr["Platform"].ToString();
                x.Create_Time = dr["Create_Time"] != DBNull.Value ? DateTime.Parse(dr["Create_Time"].ToString()) : default(DateTime);

            }
            return x;
        }



        string id;

        /// <summary>
        ///有默认值
        /// </summary>
        [Required]
        [MaxLength(36)]
        public string Id
        {
            get { return id; }
            set { id = value; SetFieldMapping("Id", value); }
        }


        string ip;

        /// <summary>
        ///
        /// </summary>
        [MaxLength(20)]
        public string Ip
        {
            get { return ip; }
            set { ip = value; SetFieldMapping("Ip", value); }
        }


        string ip_info;

        /// <summary>
        ///IP信息
        /// </summary>
        [MaxLength(200)]
        public string Ip_Info
        {
            get { return ip_info; }
            set { ip_info = value; SetFieldMapping("Ip_Info", value); }
        }


        string user_id;

        /// <summary>
        ///
        /// </summary>
        [MaxLength(36)]
        public string User_Id
        {
            get { return user_id; }
            set { user_id = value; SetFieldMapping("User_Id", value); }
        }


        string user_name;

        /// <summary>
        ///
        /// </summary>
        [MaxLength(20)]
        public string User_Name
        {
            get { return user_name; }
            set { user_name = value; SetFieldMapping("User_Name", value); }
        }


        string platform;

        /// <summary>
        ///平台名称
        /// </summary>
        [MaxLength(20)]
        public string Platform
        {
            get { return platform; }
            set { platform = value; SetFieldMapping("Platform", value); }
        }


        DateTime create_time;

        /// <summary>
        ///有默认值
        /// </summary>
        public DateTime Create_Time
        {
            get { return create_time; }
            set { create_time = value; SetFieldMapping("Create_Time", value); }
        }



    }
}
