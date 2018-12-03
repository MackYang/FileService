using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using SoEasy.Model.BaseEntity;
using SoEasy.Common;
using System.Data;


namespace SoEasy.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class SysLogModel : Parent
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
            ht.Add(Constants.STR_DB_TABLE, "sys_log");
            ht.Add(Constants.STR_DB_PK, "Id");
            return ht;
        }
        public override Parent GetModelFromDataTable(DataTable dt)
        {
            SysLogModel x = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                x = new SysLogModel();
                DataRow dr = dt.Rows[0];
                x.Id = dr["Id"] != DBNull.Value ? int.Parse(dr["Id"].ToString()) : default(int);
                x.Platform = dr["Platform"] != DBNull.Value ? int.Parse(dr["Platform"].ToString()) : default(int);
                x.Logtime = dr["Logtime"].ToString();
                x.Thread = dr["Thread"].ToString();
                x.Loglevel = dr["Loglevel"].ToString();
                x.Logger = dr["Logger"].ToString();
                x.Logmessage = dr["Logmessage"].ToString();

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


        int platform;

        /// <summary>
        ///请见PlatformType枚举
        /// </summary>
        public int Platform
        {
            get { return platform; }
            set { platform = value; SetFieldMapping("Platform", value); }
        }


        string logtime;

        /// <summary>
        ///
        /// </summary>
        [MaxLength(25)]
        public string Logtime
        {
            get { return logtime; }
            set { logtime = value; SetFieldMapping("Logtime", value); }
        }


        string thread;

        /// <summary>
        ///
        /// </summary>
        [MaxLength(10)]
        public string Thread
        {
            get { return thread; }
            set { thread = value; SetFieldMapping("Thread", value); }
        }


        string loglevel;

        /// <summary>
        ///
        /// </summary>
        [MaxLength(10)]
        public string Loglevel
        {
            get { return loglevel; }
            set { loglevel = value; SetFieldMapping("Loglevel", value); }
        }


        string logger;

        /// <summary>
        ///
        /// </summary>
        [MaxLength(2000)]
        public string Logger
        {
            get { return logger; }
            set { logger = value; SetFieldMapping("Logger", value); }
        }


        string logmessage;

        /// <summary>
        ///
        /// </summary>
        public string Logmessage
        {
            get { return logmessage; }
            set { logmessage = value; SetFieldMapping("Logmessage", value); }
        }



    }
}
