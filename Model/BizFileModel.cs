using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using SoEasy.Model.BaseEntity;
using SoEasy.Common;
using System.Data;


namespace Model
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class BizFileModel : Parent
    {
        public override string RequriedFields
        {
            get
            {
                return "Id,File_Name,File_Type,File_Size,Content_Type,Md5,Width,Height,Biz_System_Id";
            }
        }
        public override System.Collections.Hashtable MappingTableInfo()
        {
            Hashtable ht = new Hashtable();
            ht.Add(Constants.STR_DB_TABLE, "biz_file");
            ht.Add(Constants.STR_DB_PK, "Id");
            return ht;
        }
        public override Parent GetModelFromDataTable(DataTable dt)
        {
            BizFileModel x = null;
            if (dt != null && dt.Rows.Count > 0) {
                x = new BizFileModel();
                DataRow dr = dt.Rows[0];
                x.Id = dr["Id"].ToString();
                x.File_Name = dr["File_Name"].ToString();
                x.File_Type = dr["File_Type"].ToString();
                x.File_Size = dr["File_Size"] != DBNull.Value ? long.Parse(dr["File_Size"].ToString()) : default(long);
                x.Content_Type = dr["Content_Type"].ToString();
                x.Ref_Id = dr["Ref_Id"].ToString();
                x.Url = dr["Url"].ToString();
                x.Md5 = dr["Md5"].ToString();
                x.Width = dr["Width"] != DBNull.Value ? int.Parse(dr["Width"].ToString()) : default(int);
                x.Height = dr["Height"] != DBNull.Value ? int.Parse(dr["Height"].ToString()) : default(int);
                x.Create_Id = dr["Create_Id"].ToString();
                x.Create_Time = dr["Create_Time"] != DBNull.Value ? DateTime.Parse(dr["Create_Time"].ToString()) : default(DateTime);
                x.Biz_System_Id = dr["Biz_System_Id"].ToString();
                x.Is_Temp = dr["Is_Temp"] != DBNull.Value ? int.Parse(dr["Is_Temp"].ToString()) : default(int);

            }
            return x;
        }



        string id;

        /// <summary>
        ///主键
        /// </summary>
        [Required]
        [MaxLength(36)]
        public string Id
        {
            get { return id; }
            set { id = value; SetFieldMapping("Id", value); }
        }


        string file_name;

        /// <summary>
        ///文件名称
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string File_Name
        {
            get { return file_name; }
            set { file_name = value; SetFieldMapping("File_Name", value); }
        }


        string file_type;

        /// <summary>
        ///文件类型
        /// </summary>
        [Required]
        [MaxLength(36)]
        public string File_Type
        {
            get { return file_type; }
            set { file_type = value; SetFieldMapping("File_Type", value); }
        }


        long file_size;

        /// <summary>
        ///
        /// </summary>
        [Required]
        public long File_Size
        {
            get { return file_size; }
            set { file_size = value; SetFieldMapping("File_Size", value); }
        }


        string content_type;

        /// <summary>
        ///内容类型
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Content_Type
        {
            get { return content_type; }
            set { content_type = value; SetFieldMapping("Content_Type", value); }
        }


        string ref_id;

        /// <summary>
        ///引用记录的ID,如果上传的文件已存在,直接引用首次上传的记录ID
        /// </summary>
        [MaxLength(36)]
        public string Ref_Id
        {
            get { return ref_id; }
            set { ref_id = value; SetFieldMapping("Ref_Id", value); }
        }


        string url;

        /// <summary>
        ///文件路径,不含服务器前缀,直接从目录开始
        /// </summary>
        [MaxLength(255)]
        public string Url
        {
            get { return url; }
            set { url = value; SetFieldMapping("Url", value); }
        }


        string md5;

        /// <summary>
        ///文件的MD5值,通过此值判断文件是否已存在
        /// </summary>
        [Required]
        [MaxLength(32)]
        public string Md5
        {
            get { return md5; }
            set { md5 = value; SetFieldMapping("Md5", value); }
        }


        int width;

        /// <summary>
        ///图片宽度,单位像素,非图片填写0
        /// </summary>
        [Required]
        public int Width
        {
            get { return width; }
            set { width = value; SetFieldMapping("Width", value); }
        }


        int height;

        /// <summary>
        ///图片高度,单位像素,非图片填写0
        /// </summary>
        [Required]
        public int Height
        {
            get { return height; }
            set { height = value; SetFieldMapping("Height", value); }
        }


        string create_id;

        /// <summary>
        ///
        /// </summary>
        [MaxLength(36)]
        public string Create_Id
        {
            get { return create_id; }
            set { create_id = value; SetFieldMapping("Create_Id", value); }
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


        string biz_system_id;

        /// <summary>
        ///业务系统ID
        /// </summary>
        [Required]
        [MaxLength(36)]
        public string Biz_System_Id
        {
            get { return biz_system_id; }
            set { biz_system_id = value; SetFieldMapping("Biz_System_Id", value); }
        }


        int is_temp;

        /// <summary>
        ///是否是默认文件
        /// </summary>
        public int Is_Temp
        {
            get { return is_temp; }
            set { is_temp = value; SetFieldMapping("Is_Temp", value); }
        }



    }
}
