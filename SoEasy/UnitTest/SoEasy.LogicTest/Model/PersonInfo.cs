using SoEasy.Common;
using SoEasy.Model.BaseEntity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoEasy.LogicTest.Model
{
    /// <summary>
    /// a
    /// </summary>
    [Serializable]
    public class PersonInfo : Parent
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
            ht.Add(Constants.STR_DB_TABLE, "person_info");
            ht.Add(Constants.STR_DB_PK, "Id");
            return ht;
        }
        public override Parent GetModelFromDataTable(DataTable dt)
        {
            PersonInfo x = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                x = new PersonInfo();
                DataRow dr = dt.Rows[0];
                x.Id = dr["Id"].ToString();
                x.Name = dr["Name"].ToString();
                x.Age = dr["Age"] != DBNull.Value ? int.Parse(dr["Age"].ToString()) : default(int);
                x.Op_Time = dr["Op_Time"] != DBNull.Value ? DateTime.Parse(dr["Op_Time"].ToString()) : default(DateTime);

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


        string name;

        /// <summary>
        ///
        /// </summary>
        [MaxLength(20)]
        public string Name
        {
            get { return name; }
            set { name = value; SetFieldMapping("Name", value); }
        }


        int age;

        /// <summary>
        ///
        /// </summary>
        [Range(-999, 999)]
        public int Age
        {
            get { return age; }
            set { age = value; SetFieldMapping("Age", value); }
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
