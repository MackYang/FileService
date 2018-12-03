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
    /// 
    /// </summary>
    [Serializable]
    public class OrderInfo : Parent
    {
        public override string RequriedFields
        {
            get
            {
                return "";
            }
        }
        public override System.Collections.Hashtable MappingTableInfo()
        {
            Hashtable ht = new Hashtable();
            ht.Add(Constants.STR_DB_TABLE, "order_info");
            ht.Add(Constants.STR_DB_PK, "ID");
            return ht;
        }
        public override Parent GetModelFromDataTable(DataTable dt)
        {
            OrderInfo x = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                x = new OrderInfo();
                DataRow dr = dt.Rows[0];
                x.Id = dr["Id"].ToString();
                x.Person_Id = dr["Person_Id"].ToString();
                x.Product_Id = dr["Product_Id"].ToString();
                x.Amount = long.Parse(dr["Amount"].ToString());
                x.Total_Price = decimal.Parse(dr["Total_Price"].ToString());
                x.Op_Time = DateTime.Parse(dr["Op_Time"].ToString());

            }
            return x;
        }



        string id;

        /// <summary>
        ///
        /// </summary>
        [MaxLength(36)]
        public string Id
        {
            get { return id; }
            set { id = value; SetFieldMapping("Id", value); }
        }


        string person_id;

        /// <summary>
        ///
        /// </summary>
        [MaxLength(36)]
        public string Person_Id
        {
            get { return person_id; }
            set { person_id = value; SetFieldMapping("Person_Id", value); }
        }


        string product_id;

        /// <summary>
        ///
        /// </summary>
        [MaxLength(36)]
        public string Product_Id
        {
            get { return product_id; }
            set { product_id = value; SetFieldMapping("Product_Id", value); }
        }


        long amount;

        /// <summary>
        ///
        /// </summary>
        public long Amount
        {
            get { return amount; }
            set { amount = value; SetFieldMapping("Amount", value); }
        }


        decimal total_price;

        /// <summary>
        ///
        /// </summary>
        [Range(-99999999999.9999, 99999999999.9999)]
        public decimal Total_Price
        {
            get { return total_price; }
            set { total_price = value; SetFieldMapping("Total_Price", value); }
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
