using SoEasy.Common;
using SoEasy.Model.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;
using System.Linq;

namespace SoEasy.Model.BaseEntity
{
    public static class ModelExtension
    {
        /// <summary>
        /// 将实体类转成表(实体类的属性要赋值后才能转)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(this Parent p)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < p.CountFields(); i++)
            {
                dt.Columns.Add(new DataColumn(p.ColumnName(i), p.ColumnValue(i).GetType()));
            }
            DataRow dr = dt.NewRow();
            for (int i = 0; i < p.CountFields(); i++)
            {
                dr[i] = p.ColumnValue(i);
            }
            dt.Rows.Add(dr);
            return dt;
        }

        /// <summary>
        /// 验证实体更新的数据是否符合要求
        /// </summary>
        /// <param name="p">被验证的实体</param>
        /// <param name="validateFailMsg">验证失败时的信息</param>
        /// <returns>符合返回true</returns>
        public static bool UpdateValid(this Parent p, out string validateFailMsg)
        {
            if (p == null || p.CountFields() == 0) { validateFailMsg = "请先对要更新的实体属性赋值."; return false; }
            StringBuilder failMsg = new StringBuilder();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < p.CountFields(); i++)
            {
                sb.Append("," + p.ColumnName(i));
            }
            if (sb.Length > 0)
            {
                sb = sb.Remove(0, 1);
            }


            string setFields = sb.ToString();

            //更新时既没带其它条件,也没设置主键的值时,抛出异常
            if (p.OtherCondition == null || string.IsNullOrWhiteSpace(p.OtherCondition.ConditionSQL))
            {
                string pk = p.MappingTableInfo()[Constants.STR_DB_PK].ToString();
                if (!p.HasValue(pk))
                {
                    validateFailMsg = "更新实体时,实体的OtherCondition 或 主键字段 其中一个必须赋值!";
                    return false;
                }
            }

            var context = new ValidationContext(p);

            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(p, context, results, true);

            bool tmpFlag = true;
            if (!isValid)
            {
                failMsg.Append("以下字段不符合要求:");
                string[] setFieldArr = setFields.Split(',');
                foreach (ValidationResult item in results)
                {
                    if (setFieldArr.ContainsElement((string[])item.MemberNames))
                    {
                        failMsg.Append("," + item.ErrorMessage);
                        tmpFlag = false;
                    }

                }
                if (failMsg.Length > 0)
                {
                    failMsg = failMsg.Remove(0, 1);
                }
            }

            validateFailMsg = failMsg.ToString();
            return tmpFlag;


        }


        /// <summary>
        /// 验证实体插入的数据是否符合要求
        /// </summary>
        /// <param name="p">被验证的实体</param>
        /// <param name="validateFailMsg">验证失败时的信息</param>
        /// <returns>符合返回true</returns>
        public static bool InsertValid(this Parent p, out string validateFailMsg)
        {
            bool isValid = false;
            if (p == null) { validateFailMsg = "请先对要验证的实体赋值."; return false; }
            StringBuilder failMsg = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(p.RequriedFields))
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < p.CountFields(); i++)
                {
                    sb.Append("," + p.ColumnName(i));
                }
                if (sb.Length > 0)
                {
                    sb = sb.Remove(0, 1);
                }

                string setFields = sb.ToString();
                string[] req = p.RequriedFields.Split(',');
                string[] cur = setFields.Split(',');
                string[] diff = req.Except(cur).ToArray();
                if (diff.Length > 0)
                {
                    failMsg.Append("插入到数据库前必须对以下字段赋值:");
                    foreach (string item in diff)
                    {
                        failMsg.Append(item + ",");
                    }
                    failMsg.Remove(failMsg.Length - 1, 1);

                }
            }

            var context = new ValidationContext(p);
            var results = new List<ValidationResult>();
            isValid = Validator.TryValidateObject(p, context, results, true);

            if (!isValid)
            {
                foreach (ValidationResult item in results)
                {
                    failMsg.Append("," + item.ErrorMessage);
                }
                if (failMsg.Length > 0)
                {
                    failMsg = failMsg.Remove(0, 1);
                }
            }

            validateFailMsg = failMsg.ToString();
            return isValid;


        }


        /// <summary>
        /// 将DataTable转换成T类型的实体对象
        /// </summary>
        /// <typeparam name="T">T类必须继承自Parent类</typeparam>
        /// <param name="dt">DataTable数据源</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <returns>T类实体对象,null表示转换失败</returns>
        public static T GetTModel<T>(this DataTable dt, OPResult opRes, bool throwException = false) where T : Parent, new()
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {

                T t = null;
                try
                {
                    t = new T();
                    t = (T)t.GetModelFromDataTable(dt);
                }
                catch (Exception ex)
                {
                    string typeName = t.ToString();
                    t = null;
                    throw new Exception("将DataTable转换成实体类 " + typeName + " 的对象时发现异常:" + ex);
                }
                return t;
            }, opRes, throwException);
        }
    }
}
