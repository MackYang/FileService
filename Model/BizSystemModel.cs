using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using SoEasy.Model.BaseEntity;
using SoEasy.Common;
using System.Data;


namespace Model
{
    /// <summary>
    /// 业务系统表
    /// </summary>
    [Serializable]
    public class BizSystemModel : Parent
    {
        public override string RequriedFields
        {
            get
            {
                return "Id,Name";
            }
        }
        public override System.Collections.Hashtable MappingTableInfo()
        {
            Hashtable ht = new Hashtable();
            ht.Add(Constants.STR_DB_TABLE, "biz_system");
            ht.Add(Constants.STR_DB_PK, "Id");
            return ht;
        }
        public override Parent GetModelFromDataTable(DataTable dt)
        {
            BizSystemModel x = null;
            if (dt != null && dt.Rows.Count > 0) {
                x = new BizSystemModel();
                DataRow dr = dt.Rows[0];
                x.Id = dr["Id"].ToString();
                x.Name = dr["Name"].ToString();
                x.Create_Time = dr["Create_Time"] != DBNull.Value ? DateTime.Parse(dr["Create_Time"].ToString()) : default(DateTime);
                x.Enable = dr["Enable"] != DBNull.Value ? int.Parse(dr["Enable"].ToString()) : default(int);

            }
            return x;
        }



        string id;

        /// <summary>
        ///
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Id
        {
            get { return id; }
            set { id = value; SetFieldMapping("Id", value); }
        }


        string name;

        /// <summary>
        ///业务系统名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name
        {
            get { return name; }
            set { name = value; SetFieldMapping("Name", value); }
        }


        DateTime create_time;

        /// <summary>
        ///
        /// </summary>
        public DateTime Create_Time
        {
            get { return create_time; }
            set { create_time = value; SetFieldMapping("Create_Time", value); }
        }


        int enable;

        /// <summary>
        ///是否启用
        /// </summary>
        public int Enable
        {
            get { return enable; }
            set { enable = value; SetFieldMapping("Enable", value); }
        }



    }
}
