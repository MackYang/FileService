using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using SoEasy.Model.BaseEntity;
using SoEasy.Common;
using System.Data;


namespace SoEasy.Model
{
    /// <summary>
    /// 角色用户映射表
    /// </summary>
    [Serializable]
    public class SysRoleUserMapModel : Parent
    {
        public override string RequriedFields
        {
            get
            {
                return "Id,Role_Id,User_Id,Op_Id";
            }
        }
        public override System.Collections.Hashtable MappingTableInfo()
        {
            Hashtable ht = new Hashtable();
            ht.Add(Constants.STR_DB_TABLE, "sys_role_user_map");
            ht.Add(Constants.STR_DB_PK, "Id");
            return ht;
        }
        public override Parent GetModelFromDataTable(DataTable dt)
        {
            SysRoleUserMapModel x = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                x = new SysRoleUserMapModel();
                DataRow dr = dt.Rows[0];
                x.Id = dr["Id"].ToString();
                x.Role_Id = dr["Role_Id"].ToString();
                x.User_Id = dr["User_Id"].ToString();
                x.Op_Id = dr["Op_Id"].ToString();
                x.Op_Time = DateTime.Parse(dr["Op_Time"].ToString());

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


        string role_id;

        /// <summary>
        ///角色ID
        /// </summary>
        [Required]
        [MaxLength(36)]
        public string Role_Id
        {
            get { return role_id; }
            set { role_id = value; SetFieldMapping("Role_Id", value); }
        }


        string user_id;

        /// <summary>
        ///用户ID
        /// </summary>
        [Required]
        [MaxLength(36)]
        public string User_Id
        {
            get { return user_id; }
            set { user_id = value; SetFieldMapping("User_Id", value); }
        }


        string op_id;

        /// <summary>
        ///
        /// </summary>
        [Required]
        [MaxLength(36)]
        public string Op_Id
        {
            get { return op_id; }
            set { op_id = value; SetFieldMapping("Op_Id", value); }
        }


        DateTime op_time;

        /// <summary>
        ///操作时间
        /// </summary>
        public DateTime Op_Time
        {
            get { return op_time; }
            set { op_time = value; SetFieldMapping("Op_Time", value); }
        }



    }
}
