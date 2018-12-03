using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using SoEasy.Model.BaseEntity;
using SoEasy.Common;
using System.Data;


namespace SoEasy.Model
{
    /// <summary>
    /// 地址表
    /// </summary>
    [Serializable]
    public class SysAddressModel : Parent
    {
        public override string RequriedFields
        {
            get
            {
                return "Id,Code,Parent_Code,Name,Level";
            }
        }
        public override System.Collections.Hashtable MappingTableInfo()
        {
            Hashtable ht = new Hashtable();
            ht.Add(Constants.STR_DB_TABLE, "sys_address");
            ht.Add(Constants.STR_DB_PK, "Id");
            return ht;
        }
        public override Parent GetModelFromDataTable(DataTable dt)
        {
            SysAddressModel x = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                x = new SysAddressModel();
                DataRow dr = dt.Rows[0];
                x.Id = int.Parse(dr["Id"].ToString());
                x.Code = dr["Code"].ToString();
                x.Parent_Code = dr["Parent_Code"].ToString();
                x.Name = dr["Name"].ToString();
                x.Level = int.Parse(dr["Level"].ToString());

            }
            return x;
        }



        int id;

        /// <summary>
        ///
        /// </summary>
        [Required]
        public int Id
        {
            get { return id; }
            set { id = value; SetFieldMapping("Id", value); }
        }


        string code;

        /// <summary>
        ///
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string Code
        {
            get { return code; }
            set { code = value; SetFieldMapping("Code", value); }
        }


        string parent_code;

        /// <summary>
        ///
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string Parent_Code
        {
            get { return parent_code; }
            set { parent_code = value; SetFieldMapping("Parent_Code", value); }
        }


        string name;

        /// <summary>
        ///
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name
        {
            get { return name; }
            set { name = value; SetFieldMapping("Name", value); }
        }


        int level;

        /// <summary>
        ///
        /// </summary>
        [Required]
        public int Level
        {
            get { return level; }
            set { level = value; SetFieldMapping("Level", value); }
        }



    }
}
