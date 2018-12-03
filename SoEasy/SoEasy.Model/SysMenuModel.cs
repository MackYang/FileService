using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using SoEasy.Model.BaseEntity;
using SoEasy.Common;
using System.Data;


namespace SoEasy.Model
{
    /// <summary>
    /// 菜单表
    /// </summary>
    [Serializable]
    public class SysMenuModel : Parent
    {
        public override string RequriedFields
        {
            get
            {
                return "Id,Menu_Name,Menu_No,Data_State,Op_Id";
            }
        }
        public override System.Collections.Hashtable MappingTableInfo()
        {
            Hashtable ht = new Hashtable();
            ht.Add(Constants.STR_DB_TABLE, "sys_menu");
            ht.Add(Constants.STR_DB_PK, "Id");
            return ht;
        }
        public override Parent GetModelFromDataTable(DataTable dt)
        {
            SysMenuModel x = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                x = new SysMenuModel();
                DataRow dr = dt.Rows[0];
                x.Id = dr["Id"].ToString();
                x.Menu_Name = dr["Menu_Name"].ToString();
                x.Menu_Url = dr["Menu_Url"].ToString();
                x.Menu_No = dr["Menu_No"].ToString();
                x.Child_Count = int.Parse(dr["Child_Count"].ToString());
                x.Sort_Num = int.Parse(dr["Sort_Num"].ToString());
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


        string menu_name;

        /// <summary>
        ///菜单名称
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Menu_Name
        {
            get { return menu_name; }
            set { menu_name = value; SetFieldMapping("Menu_Name", value); }
        }


        string menu_url;

        /// <summary>
        ///菜单的URL
        /// </summary>
        [MaxLength(500)]
        public string Menu_Url
        {
            get { return menu_url; }
            set { menu_url = value; SetFieldMapping("Menu_Url", value); }
        }


        string menu_no;

        /// <summary>
        ///菜单编号 每4位 做为一级
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Menu_No
        {
            get { return menu_no; }
            set { menu_no = value; SetFieldMapping("Menu_No", value); }
        }


        int child_count;

        /// <summary>
        ///子节点数,冗余字段,方便显示树的时候不用查子节点
        /// </summary>
        public int Child_Count
        {
            get { return child_count; }
            set { child_count = value; SetFieldMapping("Child_Count", value); }
        }


        int sort_num;

        /// <summary>
        ///排序号,程序计算填充
        /// </summary>
        public int Sort_Num
        {
            get { return sort_num; }
            set { sort_num = value; SetFieldMapping("Sort_Num", value); }
        }


        int data_state;

        /// <summary>
        ///数据状态,请请DataState枚举 
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
