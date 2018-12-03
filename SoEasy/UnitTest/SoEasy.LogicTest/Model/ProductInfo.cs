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
    /// 产品信息表
    /// </summary>
    [Serializable]
    public class ProductInfo : Parent
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
            ht.Add(Constants.STR_DB_TABLE, "product_info");
            ht.Add(Constants.STR_DB_PK, "Id");
            return ht;
        }
        public override Parent GetModelFromDataTable(DataTable dt)
        {
            ProductInfo x = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                x = new ProductInfo();
                DataRow dr = dt.Rows[0];
                x.Id = dr["Id"].ToString();
                x.Name = dr["Name"].ToString();
                x.Detal_Info = dr["Detal_Info"].ToString();
                x.Price = decimal.Parse(dr["Price"].ToString());
                x.Is_Delete = int.Parse(dr["Is_Delete"].ToString());
                x.Op_Time = DateTime.Parse(dr["Op_Time"].ToString());
                x.Store_Num = long.Parse(dr["Store_Num"].ToString());

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
        ///名称
        /// </summary>
        [MaxLength(20)]
        public string Name
        {
            get { return name; }
            set { name = value; SetFieldMapping("Name", value); }
        }


        string detal_info;

        /// <summary>
        ///语言学
        /// </summary>
        public string Detal_Info
        {
            get { return detal_info; }
            set { detal_info = value; SetFieldMapping("Detal_Info", value); }
        }


        decimal price;

        /// <summary>
        ///价格
        /// </summary>
        [Range(-999999.9999, 999999.9999)]
        public decimal Price
        {
            get { return price; }
            set { price = value; SetFieldMapping("Price", value); }
        }


        int is_delete;

        /// <summary>
        ///
        /// </summary>
        [Range(-9, 9)]
        public int Is_Delete
        {
            get { return is_delete; }
            set { is_delete = value; SetFieldMapping("Is_Delete", value); }
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


        long store_num;

        /// <summary>
        ///
        /// </summary>
        [Range(-9999999999, 9999999999)]
        public long Store_Num
        {
            get { return store_num; }
            set { store_num = value; SetFieldMapping("Store_Num", value); }
        }



    }
}
