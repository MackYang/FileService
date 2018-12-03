using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using SoEasy.Model.BaseEntity;
using SoEasy.Common;
using System.Data;


namespace SoEasy.Model
{
    /// <summary>
    /// 访问记录表
    /// </summary>
    [Serializable]
    public class SysRecVisitModel : Parent
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
            ht.Add(Constants.STR_DB_TABLE, "sys_rec_visit");
            ht.Add(Constants.STR_DB_PK, "Id");
            return ht;
        }
        public override Parent GetModelFromDataTable(DataTable dt)
        {
            SysRecVisitModel x = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                x = new SysRecVisitModel();
                DataRow dr = dt.Rows[0];
                x.Id = dr["Id"].ToString();
                x.Ip = dr["Ip"].ToString();
                x.Province = dr["Province"].ToString();
                x.City = dr["City"].ToString();
                x.District = dr["District"].ToString();
                x.Street = dr["Street"].ToString();
                x.Street_Number = dr["Street_Number"].ToString();
                x.Full_Address = dr["Full_Address"].ToString();
                x.Create_Time = dr["Create_Time"] != DBNull.Value ? DateTime.Parse(dr["Create_Time"].ToString()) : default(DateTime);
                x.Platform = dr["Platform"] != DBNull.Value ? int.Parse(dr["Platform"].ToString()) : default(int);

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


        string ip;

        /// <summary>
        ///访问者IP
        /// </summary>
        [MaxLength(20)]
        public string Ip
        {
            get { return ip; }
            set { ip = value; SetFieldMapping("Ip", value); }
        }


        string province;

        /// <summary>
        ///省名称
        /// </summary>
        [MaxLength(10)]
        public string Province
        {
            get { return province; }
            set { province = value; SetFieldMapping("Province", value); }
        }


        string city;

        /// <summary>
        ///市名称
        /// </summary>
        [MaxLength(10)]
        public string City
        {
            get { return city; }
            set { city = value; SetFieldMapping("City", value); }
        }


        string district;

        /// <summary>
        ///区县名称
        /// </summary>
        [MaxLength(10)]
        public string District
        {
            get { return district; }
            set { district = value; SetFieldMapping("District", value); }
        }


        string street;

        /// <summary>
        ///街道名称
        /// </summary>
        [MaxLength(20)]
        public string Street
        {
            get { return street; }
            set { street = value; SetFieldMapping("Street", value); }
        }


        string street_number;

        /// <summary>
        ///门牌号
        /// </summary>
        [MaxLength(10)]
        public string Street_Number
        {
            get { return street_number; }
            set { street_number = value; SetFieldMapping("Street_Number", value); }
        }


        string full_address;

        /// <summary>
        ///将省市县等拼起来
        /// </summary>
        [MaxLength(100)]
        public string Full_Address
        {
            get { return full_address; }
            set { full_address = value; SetFieldMapping("Full_Address", value); }
        }


        DateTime create_time;

        /// <summary>
        ///访问时间有默认值
        /// </summary>
        public DateTime Create_Time
        {
            get { return create_time; }
            set { create_time = value; SetFieldMapping("Create_Time", value); }
        }


        int platform;

        /// <summary>
        ///访问平台,请见PlatformType枚举
        /// </summary>
        public int Platform
        {
            get { return platform; }
            set { platform = value; SetFieldMapping("Platform", value); }
        }



    }
}
