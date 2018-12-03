using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using SoEasy.Model.BaseEntity;
using SoEasy.Common;
using System.Data;


namespace SoEasy.Model
{
    /// <summary>
    /// 角色表
    /// </summary>
    [Serializable]
    public class SysRoleModel : Parent
    {
        public override string RequriedFields
        {
            get
            {
                return "Id,Role_Name,Data_State,Op_Id";
            }
        }
        public override System.Collections.Hashtable MappingTableInfo()
        {
            Hashtable ht = new Hashtable();
            ht.Add(Constants.STR_DB_TABLE, "sys_role");
            ht.Add(Constants.STR_DB_PK, "Id");
            return ht;
        }
        public override Parent GetModelFromDataTable(DataTable dt)
        {
            SysRoleModel x = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                x = new SysRoleModel();
                DataRow dr = dt.Rows[0];
                x.Id = dr["Id"].ToString();
                x.Role_Name = dr["Role_Name"].ToString();
                x.Remark = dr["Remark"].ToString();
                x.Data_State = int.Parse(dr["Data_State"].ToString());
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


        string role_name;

        /// <summary>
        ///
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Role_Name
        {
            get { return role_name; }
            set { role_name = value; SetFieldMapping("Role_Name", value); }
        }


        string remark;

        /// <summary>
        ///备注
        /// </summary>
        [MaxLength(50)]
        public string Remark
        {
            get { return remark; }
            set { remark = value; SetFieldMapping("Remark", value); }
        }


        int data_state;

        /// <summary>
        ///数据状态,请见DataState枚举
        /// </summary>
        [Required]
        public int Data_State
        {
            get { return data_state; }
            set { data_state = value; SetFieldMapping("Data_State", value); }
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
        ///
        /// </summary>
        public DateTime Op_Time
        {
            get { return op_time; }
            set { op_time = value; SetFieldMapping("Op_Time", value); }
        }



    }
}
