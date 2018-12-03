using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using SoEasy.Model.BaseEntity;
using SoEasy.Common;
using System.Data;


namespace SoEasy.Model
{
    /// <summary>
    /// 用户基础表
    /// </summary>
    [Serializable]
    public class SysUserModel : Parent
    {
        public override string RequriedFields
        {
            get
            {
                return "Id,Password,User_Type";
            }
        }
        public override System.Collections.Hashtable MappingTableInfo()
        {
            Hashtable ht = new Hashtable();
            ht.Add(Constants.STR_DB_TABLE, "sys_user");
            ht.Add(Constants.STR_DB_PK, "Id");
            return ht;
        }
        public override Parent GetModelFromDataTable(DataTable dt)
        {
            SysUserModel x = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                x = new SysUserModel();
                DataRow dr = dt.Rows[0];
                x.Id = dr["Id"].ToString();
                x.User_Name = dr["User_Name"].ToString();
                x.Password = dr["Password"].ToString();
                x.Alias = dr["Alias"].ToString();
                x.User_Type = dr["User_Type"] != DBNull.Value ? int.Parse(dr["User_Type"].ToString()) : default(int);
                x.Create_Time = dr["Create_Time"] != DBNull.Value ? DateTime.Parse(dr["Create_Time"].ToString()) : default(DateTime);
                x.Update_Time = dr["Update_Time"] != DBNull.Value ? DateTime.Parse(dr["Update_Time"].ToString()) : default(DateTime);
                x.Creater_Id = dr["Creater_Id"].ToString();
                x.Updater_Id = dr["Updater_Id"].ToString();
                x.Data_State = dr["Data_State"] != DBNull.Value ? int.Parse(dr["Data_State"].ToString()) : default(int);
                x.Mail = dr["Mail"].ToString();
                x.Phone_Num = dr["Phone_Num"].ToString();
                x.Icon_Img_Id = dr["Icon_Img_Id"].ToString();
                x.Register_Address = dr["Register_Address"].ToString();
                x.Last_Login_Date = dr["Last_Login_Date"] != DBNull.Value ? DateTime.Parse(dr["Last_Login_Date"].ToString()) : default(DateTime);
                x.Last_Login_Address = dr["Last_Login_Address"].ToString();
                x.Remark = dr["Remark"].ToString();

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


        string user_name;

        /// <summary>
        ///用户名
        /// </summary>
        [MaxLength(20)]
        public string User_Name
        {
            get { return user_name; }
            set { user_name = value; SetFieldMapping("User_Name", value); }
        }


        string password;

        /// <summary>
        ///密码,MD5加密
        /// </summary>
        [Required]
        [MaxLength(32)]
        public string Password
        {
            get { return password; }
            set { password = value; SetFieldMapping("Password", value); }
        }


        string alias;

        /// <summary>
        ///别名,可用来存放用户昵称,姓名等
        /// </summary>
        [MaxLength(20)]
        public string Alias
        {
            get { return alias; }
            set { alias = value; SetFieldMapping("Alias", value); }
        }


        int user_type;

        /// <summary>
        ///用户类型,0表示普通用户,1表示商家用户,2表示后台用户,请见UserType枚举
        /// </summary>
        [Required]
        public int User_Type
        {
            get { return user_type; }
            set { user_type = value; SetFieldMapping("User_Type", value); }
        }


        DateTime create_time;

        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime Create_Time
        {
            get { return create_time; }
            set { create_time = value; SetFieldMapping("Create_Time", value); }
        }


        DateTime update_time;

        /// <summary>
        ///更新时间
        /// </summary>
        public DateTime Update_Time
        {
            get { return update_time; }
            set { update_time = value; SetFieldMapping("Update_Time", value); }
        }


        string creater_id;

        /// <summary>
        ///创建者ID
        /// </summary>
        [MaxLength(36)]
        public string Creater_Id
        {
            get { return creater_id; }
            set { creater_id = value; SetFieldMapping("Creater_Id", value); }
        }


        string updater_id;

        /// <summary>
        ///更新者ID
        /// </summary>
        [MaxLength(36)]
        public string Updater_Id
        {
            get { return updater_id; }
            set { updater_id = value; SetFieldMapping("Updater_Id", value); }
        }


        int data_state;

        /// <summary>
        ///数据状态,请见DataState枚举
        /// </summary>
        public int Data_State
        {
            get { return data_state; }
            set { data_state = value; SetFieldMapping("Data_State", value); }
        }


        string mail;

        /// <summary>
        ///邮件地址
        /// </summary>
        [MaxLength(30)]
        public string Mail
        {
            get { return mail; }
            set { mail = value; SetFieldMapping("Mail", value); }
        }


        string phone_num;

        /// <summary>
        ///手机号
        /// </summary>
        [MaxLength(15)]
        public string Phone_Num
        {
            get { return phone_num; }
            set { phone_num = value; SetFieldMapping("Phone_Num", value); }
        }


        string icon_img_id;

        /// <summary>
        ///用户头像图片ID
        /// </summary>
        [MaxLength(36)]
        public string Icon_Img_Id
        {
            get { return icon_img_id; }
            set { icon_img_id = value; SetFieldMapping("Icon_Img_Id", value); }
        }


        string register_address;

        /// <summary>
        ///注册地点
        /// </summary>
        [MaxLength(100)]
        public string Register_Address
        {
            get { return register_address; }
            set { register_address = value; SetFieldMapping("Register_Address", value); }
        }


        DateTime last_login_date;

        /// <summary>
        ///最近一次登录时间
        /// </summary>
        public DateTime Last_Login_Date
        {
            get { return last_login_date; }
            set { last_login_date = value; SetFieldMapping("Last_Login_Date", value); }
        }


        string last_login_address;

        /// <summary>
        ///最近一次登录地点
        /// </summary>
        [MaxLength(100)]
        public string Last_Login_Address
        {
            get { return last_login_address; }
            set { last_login_address = value; SetFieldMapping("Last_Login_Address", value); }
        }


        string remark;

        /// <summary>
        ///备注
        /// </summary>
        [MaxLength(100)]
        public string Remark
        {
            get { return remark; }
            set { remark = value; SetFieldMapping("Remark", value); }
        }



    }
}
