using Microsoft.Practices.EnterpriseLibrary.Data;
using SoEasy.Common;
using SoEasy.Model.BaseEntity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Linq;

namespace SoEasy.DB.DAO
{
    /// <summary>
    /// 数据库操作的基础类
    /// </summary>
    abstract public class BaseDAO
    {
        static protected Database db = DatabaseFactory.CreateDatabase();

        #region 子类必须实现的抽象方法和属性
        /// <summary>
        /// 获取某类中数据库的参数标识符,如SqlServer返回@,Oracle返回:
        /// </summary>
        public abstract string GetDBParmString { get; }
        /// <summary>
        /// 获取某种数据库产生随机数的SQL语句,例如Oracle的是dbms_random.random
        /// </summary>
        public abstract string GetRandomString { get; }

        /// <summary>
        /// 获取一个某种数据库类型的参数
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="paramValue">参数值</param>
        /// <returns>返回一种数据库参数的对象,如OracleParam</returns>
        public abstract DbParameter GetDBParam(string paramName, object paramValue);

        /// <summary>
        /// 获取分页SQL
        /// </summary>
        /// <param name="pager">分页对象</param>
        /// <param name="innerSQL">内部SQL,基于此SQL的查询结果做分页</param>
        /// <returns></returns>
        public abstract string GetPageSQL(Pager pager, string innerSQL);

        /// <summary>
        /// 连接SQL字符串
        /// </summary>
        /// <param name="strAs">要连接的多个字符串</param>
        /// <returns>拼接后的字符串</returns>
        public abstract string ConcatenateString(params string[] strAs);
        #endregion

        /// <summary>
        /// SQL语句条件设置类型
        /// </summary>

        private enum DB_Condition
        {
            /// <summary>
            /// 只设定键
            /// </summary>
            Key,
            /// <summary>
            /// 只设定值
            /// </summary>
            Value,
            /// <summary>
            /// 设定键值对(Update操作)
            /// </summary>
            All
        }

        #region Base

        /// <summary>
        /// 严格模式(字段内的多个关键子用and ,字段间用or)获取关键字的内部SQL条件,将对keyword空格分割后分别like,并在paramList中添加对应的参数
        /// </summary>
        /// <param name="fieldsName">包含关键字的数据库字段集合,也就是用于like%X%的字段</param>
        /// <param name="keyword">关键字内容</param>
        /// <param name="paramList">查询参数列表</param>
        /// <returns></returns>
        public string GetKeywordSQLStrict(string keyword, List<DbParameter> paramList, params string[] fieldsArr)
        {
            string inner = "", outer = "";
            StringBuilder sb = new StringBuilder();
            List<string> nameList = new List<string>();
            paramList.ForEach(x => nameList.Add(x.ParameterName));

            keyword = keyword.ToLower();
            string[] keywordArr = keyword.SingleSpace().Split(' ');
            keywordArr = keywordArr.Distinct().ToArray();

            //如果关键字数组和字段都大于1,那么字段内部用and like ,字段间用or 筛选
            if (fieldsArr.Length > 1 && keywordArr.Length > 1)
            {
                outer = " or ";
                inner = " and ";
                foreach (string field in fieldsArr)
                {
                    StringBuilder innerSb = new StringBuilder();
                    for (int i = 0; i < keywordArr.Length; i++)
                    {
                        string argsName = "kwd" + i;
                        innerSb.Append(inner + field + " like " + ConcatenateString(" '%'", ":" + argsName, "'%'"));
                        if (!nameList.Contains(argsName))
                        {
                            paramList.Add(GetDBParam(argsName, keywordArr[i]));
                            nameList.Add(argsName);
                        }
                    }
                    innerSb.Remove(0, inner.Length);
                    innerSb.Insert(0, "(");
                    innerSb.Append(")");
                    sb.AppendLine(outer + innerSb.ToString());
                }
                sb.Remove(0, outer.Length);
                sb.Insert(0, "(");
                sb.Append(")");
            }
            else if (fieldsArr.Length > 1)//如果字段数大于1,但是关键字数组只有1个元素时,任意字段包含了关键字,都合格
            {
                outer = " or ";
                string argsName = "keyword";
                foreach (string item in fieldsArr)
                {
                    sb.Append(outer + item + " like " + ConcatenateString(" '%'", ":" + argsName, "'%'"));
                }
                sb.Remove(0, outer.Length);
                sb.Insert(0, "(");
                sb.Append(")");
                if (!nameList.Contains(argsName))
                {
                    paramList.Add(GetDBParam(argsName, keywordArr[0]));
                    nameList.Add(argsName);
                }
            }
            else if (keywordArr.Length > 1)//如果只有一个字段,但是有多个关键字,那么只筛选出字段中同时包含这些关键字的记录
            {
                inner = " and ";
                StringBuilder innerSb = new StringBuilder();
                for (int i = 0; i < keywordArr.Length; i++)
                {
                    string argsName = "kwd" + i;
                    innerSb.Append(inner + fieldsArr[0] + " like " + ConcatenateString(" '%'", ":" + argsName, "'%'"));
                    if (!nameList.Contains(argsName))
                    {
                        paramList.Add(GetDBParam(argsName, keywordArr[i]));
                        nameList.Add(argsName);
                    }
                }
                innerSb.Remove(0, inner.Length);
                innerSb.Insert(0, "(");
                innerSb.Append(")");
                sb.Append(innerSb.ToString());

            }
            else//只有一个字段,也只有一个关键字时,就普通的情况了,直接筛选出字段包含关键字的记录
            {
                string argsName = "keyword";
                sb.Append(fieldsArr[0] + " like " + ConcatenateString(" '%'", ":" + argsName, "'%'"));
                if (!nameList.Contains(argsName))
                {
                    paramList.Add(GetDBParam(argsName, keywordArr[0]));
                    nameList.Add(argsName);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 严格模式(字段内的多个关键子用and ,字段间用or)获取关键字的内部SQL条件,将对keyword空格分割后分别like,并在paramList中添加对应的参数
        /// </summary>
        /// <param name="fieldsName">包含关键字的数据库字段集合,也就是用于like%X%的字段</param>
        /// <param name="keyword">关键字内容</param>
        /// <param name="paramList">查询参数列表</param>
        /// <returns></returns>
        public string GetKeywordSQLStrict(string keyword, List<Args> paramList, params string[] fieldsArr)
        {
            string inner = "", outer = "";
            StringBuilder sb = new StringBuilder();
            List<string> nameList = new List<string>();
            paramList.ForEach(x => nameList.Add(x.Name));

            keyword = keyword.ToLower();
            string[] keywordArr = keyword.SingleSpace().Split(' ');
            keywordArr = keywordArr.Distinct().ToArray();

            //如果关键字数组和字段都大于1,那么字段内部用and like ,字段间用or 筛选
            if (fieldsArr.Length > 1 && keywordArr.Length > 1)
            {
                outer = " or ";
                inner = " and ";
                foreach (string field in fieldsArr)
                {
                    StringBuilder innerSb = new StringBuilder();
                    for (int i = 0; i < keywordArr.Length; i++)
                    {
                        string argsName = "kwd" + i;
                        innerSb.Append(inner + field + " like " + ConcatenateString(" '%'", ":" + argsName, "'%'"));
                        if (!nameList.Contains(argsName))
                        {
                            paramList.Add(new Args(argsName, keywordArr[i]));
                            nameList.Add(argsName);
                        }
                    }
                    innerSb.Remove(0, inner.Length);
                    innerSb.Insert(0, "(");
                    innerSb.Append(")");
                    sb.AppendLine(outer + innerSb.ToString());
                }
                sb.Remove(0, outer.Length);
                sb.Insert(0, "(");
                sb.Append(")");
            }
            else if (fieldsArr.Length > 1)//如果字段数大于1,但是关键字数组只有1个元素时,任意字段包含了关键字,都合格
            {
                outer = " or ";
                string argsName = "keyword";
                foreach (string item in fieldsArr)
                {
                    sb.Append(outer + item + " like " + ConcatenateString(" '%'", ":" + argsName, "'%'"));
                }
                sb.Remove(0, outer.Length);
                sb.Insert(0, "(");
                sb.Append(")");
                if (!nameList.Contains(argsName))
                {
                    paramList.Add(new Args(argsName, keywordArr[0]));
                    nameList.Add(argsName);
                }
            }
            else if (keywordArr.Length > 1)//如果只有一个字段,但是有多个关键字,那么只筛选出字段中同时包含这些关键字的记录
            {
                inner = " and ";
                StringBuilder innerSb = new StringBuilder();
                for (int i = 0; i < keywordArr.Length; i++)
                {
                    string argsName = "kwd" + i;
                    innerSb.Append(inner + fieldsArr[0] + " like " + ConcatenateString(" '%'", ":" + argsName, "'%'"));
                    if (!nameList.Contains(argsName))
                    {
                        paramList.Add(new Args(argsName, keywordArr[i]));
                        nameList.Add(argsName);
                    }
                }
                innerSb.Remove(0, inner.Length);
                innerSb.Insert(0, "(");
                innerSb.Append(")");
                sb.Append(innerSb.ToString());

            }
            else//只有一个字段,也只有一个关键字时,就普通的情况了,直接筛选出字段包含关键字的记录
            {
                string argsName = "keyword";
                sb.Append(fieldsArr[0] + " like " + ConcatenateString(" '%'", ":" + argsName, "'%'"));
                if (!nameList.Contains(argsName))
                {
                    paramList.Add(new Args(argsName, keywordArr[0]));
                    nameList.Add(argsName);
                }
            }

            return sb.ToString();
        }


        /// <summary>
        /// 获取关键字的内部SQL条件,将对keyword空格分割后分别like,并在paramList中添加对应的参数
        /// </summary>
        /// <param name="fieldsName">包含关键字的数据库字段集合,也就是用于like%X%的字段</param>
        /// <param name="keyword">关键字内容</param>
        /// <param name="paramList">查询参数列表</param>
        /// <returns></returns>
        public string GetKeywordSQL(string keyword, List<DbParameter> paramList, params string[] fieldsArr)
        {
            string inner = "", outer = "";
            StringBuilder sb = new StringBuilder();
            List<string> nameList = new List<string>();
            paramList.ForEach(x => nameList.Add(x.ParameterName));

            keyword = keyword.ToLower();
            string[] keywordArr = keyword.SingleSpace().Split(' ');
            keywordArr = keywordArr.Distinct().ToArray();

            //如果关键字数组和字段都大于1,那么字段内部用or like ,字段间用and 筛选
            if (fieldsArr.Length > 1 && keywordArr.Length > 1)
            {
                outer = " and ";
                inner = " or ";
                foreach (string field in fieldsArr)
                {
                    StringBuilder innerSb = new StringBuilder();
                    for (int i = 0; i < keywordArr.Length; i++)
                    {
                        string argsName = "kwd" + i;
                        innerSb.Append(inner + field + " like " + ConcatenateString(" '%'", ":" + argsName, "'%'"));
                        if (!nameList.Contains(argsName))
                        {
                            paramList.Add(GetDBParam(argsName, keywordArr[i]));
                            nameList.Add(argsName);
                        }
                    }
                    innerSb.Remove(0, inner.Length);
                    innerSb.Insert(0, "(");
                    innerSb.Append(")");
                    sb.AppendLine(outer + innerSb.ToString());
                }
                sb.Remove(0, outer.Length);
                sb.Insert(0, "(");
                sb.Append(")");
            }
            else if (fieldsArr.Length > 1)//如果字段数大于1,但是关键字数组只有1个元素时,任意字段包含了关键字,都合格
            {
                outer = " or ";
                string argsName = "keyword";
                foreach (string item in fieldsArr)
                {
                    sb.Append(outer + item + " like " + ConcatenateString(" '%'", ":" + argsName, "'%'"));
                }
                sb.Remove(0, outer.Length);
                sb.Insert(0, "(");
                sb.Append(")");
                if (!nameList.Contains(argsName))
                {
                    paramList.Add(GetDBParam(argsName, keywordArr[0]));
                    nameList.Add(argsName);
                }
            }
            else if (keywordArr.Length > 1)//如果只有一个字段,但是有多个关键字,那么只筛选出字段中同时包含这些关键字的记录
            {
                inner = " and ";
                StringBuilder innerSb = new StringBuilder();
                for (int i = 0; i < keywordArr.Length; i++)
                {
                    string argsName = "kwd" + i;
                    innerSb.Append(inner + fieldsArr[0] + " like " + ConcatenateString(" '%'", ":" + argsName, "'%'"));
                    if (!nameList.Contains(argsName))
                    {
                        paramList.Add(GetDBParam(argsName, keywordArr[i]));
                        nameList.Add(argsName);
                    }
                }
                innerSb.Remove(0, inner.Length);
                innerSb.Insert(0, "(");
                innerSb.Append(")");
                sb.Append(innerSb.ToString());

            }
            else//只有一个字段,也只有一个关键字时,就普通的情况了,直接筛选出字段包含关键字的记录
            {
                string argsName = "keyword";
                sb.Append(fieldsArr[0] + " like " + ConcatenateString(" '%'", ":" + argsName, "'%'"));
                if (!nameList.Contains(argsName))
                {
                    paramList.Add(GetDBParam(argsName, keywordArr[0]));
                    nameList.Add(argsName);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取关键字的内部SQL条件,将对keyword空格分割后分别like,并在paramList中添加对应的参数
        /// </summary>
        /// <param name="fieldsName">包含关键字的数据库字段集合,也就是用于like%X%的字段</param>
        /// <param name="keyword">关键字内容</param>
        /// <param name="paramList">查询参数列表</param>
        /// <returns></returns>
        public string GetKeywordSQL(string keyword, List<Args> paramList, params string[] fieldsArr)
        {
            string inner = "", outer = "";
            StringBuilder sb = new StringBuilder();
            List<string> nameList = new List<string>();
            paramList.ForEach(x => nameList.Add(x.Name));

            keyword = keyword.ToLower();
            string[] keywordArr = keyword.SingleSpace().Split(' ');
            keywordArr = keywordArr.Distinct().ToArray();

            //如果关键字数组和字段都大于1,那么字段内部用or like ,字段间用and 筛选
            if (fieldsArr.Length > 1 && keywordArr.Length > 1)
            {
                outer = " and ";
                inner = " or ";
                foreach (string field in fieldsArr)
                {
                    StringBuilder innerSb = new StringBuilder();
                    for (int i = 0; i < keywordArr.Length; i++)
                    {
                        string argsName = "kwd" + i;
                        innerSb.Append(inner + field + " like " + ConcatenateString(" '%'", ":" + argsName, "'%'"));
                        if (!nameList.Contains(argsName))
                        {
                            paramList.Add(new Args(argsName, keywordArr[i]));
                            nameList.Add(argsName);
                        }
                    }
                    innerSb.Remove(0, inner.Length);
                    innerSb.Insert(0, "(");
                    innerSb.Append(")");
                    sb.AppendLine(outer + innerSb.ToString());
                }
                sb.Remove(0, outer.Length);
                sb.Insert(0, "(");
                sb.Append(")");
            }
            else if (fieldsArr.Length > 1)//如果字段数大于1,但是关键字数组只有1个元素时,任意字段包含了关键字,都合格
            {
                outer = " or ";
                string argsName = "keyword";
                foreach (string item in fieldsArr)
                {
                    sb.Append(outer + item + " like " + ConcatenateString(" '%'", ":" + argsName, "'%'"));
                }
                sb.Remove(0, outer.Length);
                sb.Insert(0, "(");
                sb.Append(")");
                if (!nameList.Contains(argsName))
                {
                    paramList.Add(new Args(argsName, keywordArr[0]));
                    nameList.Add(argsName);
                }
            }
            else if (keywordArr.Length > 1)//如果只有一个字段,但是有多个关键字,那么只筛选出字段中同时包含这些关键字的记录
            {
                inner = " and ";
                StringBuilder innerSb = new StringBuilder();
                for (int i = 0; i < keywordArr.Length; i++)
                {
                    string argsName = "kwd" + i;
                    innerSb.Append(inner + fieldsArr[0] + " like " + ConcatenateString(" '%'", ":" + argsName, "'%'"));
                    if (!nameList.Contains(argsName))
                    {
                        paramList.Add(new Args(argsName, keywordArr[i]));
                        nameList.Add(argsName);
                    }
                }
                innerSb.Remove(0, inner.Length);
                innerSb.Insert(0, "(");
                innerSb.Append(")");
                sb.Append(innerSb.ToString());

            }
            else//只有一个字段,也只有一个关键字时,就普通的情况了,直接筛选出字段包含关键字的记录
            {
                string argsName = "keyword";
                sb.Append(fieldsArr[0] + " like " + ConcatenateString(" '%'", ":" + argsName, "'%'"));
                if (!nameList.Contains(argsName))
                {
                    paramList.Add(new Args(argsName, keywordArr[0]));
                    nameList.Add(argsName);
                }
            }

            return sb.ToString();
        }


        /// <summary>
        /// 获取where字句,用于动态拼接where子句时
        /// </summary>
        /// <param name="whereStr">已有的where子句,没有就传null</param>
        ///<param name="condition">条件,如age>16</param>
        /// <returns></returns>
        public string GetWhere(string whereStr, string condition)
        {
            if (string.IsNullOrWhiteSpace(condition)) { return whereStr; }
            if (string.IsNullOrWhiteSpace(whereStr))
            {
                return " where " + condition;
            }
            else
            {
                return whereStr + " and " + condition;
            }
        }


        /// <summary>
        /// 根据表名获取表结构
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <returns>null表示操作失败</returns>
        public DataTable GetTableStructByTableName(string tableName)
        {
            string sql = "select * from " + tableName.NOSQL() + " where 1=2";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            return InnerExecuteQueryAsDataTable(cmd);

        }

        /// <summary>
        /// 以实体对象的值作为条件统计
        /// </summary>
        /// <param name="p">实体</param>
        /// <returns>-1表示操作失败</returns>
        public long Count(Parent p)
        {
            string sql = GetCountStr(p);
            long rec_Count = -1;
            try
            {
                DbCommand cmd = AddArgsReturnCMD(p, sql);
                rec_Count = long.Parse(InnerExecuteSingleValue(cmd).ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("用实体类统计数据时发生异常: " + ex);
            }
            return rec_Count;
        }

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="p">实体对象</param>
        /// <returns>-1表示操作失败</returns>
        public int Insert(Parent p)
        {
            string validMsg = "";
            if (!p.InsertValid(out validMsg)) { throw new Exception("类型 " + p.ToString() + " 的实体对象中的数据未通过验证,不能保存到数据库,原因:" + validMsg); }
            int res = -1;
            Hashtable ht = p.MappingTableInfo();
            if (p.CountFields() > 0)
            {
                try
                {
                    string table = ht[Constants.STR_DB_TABLE].ToString();
                    string key = GetFields(p, DB_Condition.Key);
                    string value = GetFields(p, DB_Condition.Value);
                    string cmdText = string.Format("INSERT INTO {0}({1}) VALUES({2})", table, key, value);
                    DbCommand cmd = AddArgsReturnCMD(p, cmdText);
                    res = InnerExecuteNonQuery(cmd);
                }
                catch (Exception ex)
                {
                    throw new Exception("使用实体类插入数据时发生异常: " + ex);
                }
            }
            return res;
        }

        /// <summary>
        /// 删除符合实体条件的记录
        /// </summary>
        /// <param name="p">实体对象</param>
        /// <returns>-1表示操作失败</returns>
        public int Delete(Parent p)
        {
            int res = -1;
            try
            {
                Hashtable ht = p.MappingTableInfo();
                string table = ht[Constants.STR_DB_TABLE].ToString();
                string where = GetWhereCondition(p);
                if (string.IsNullOrWhiteSpace(where))
                {
                    throw new Exception("删除数据前,必须为实体" + p.ToString() + "的属性赋值作为删除条件");
                }
                string cmdText = string.Format("DELETE FROM {0}{1}", table, where);
                DbCommand cmd = AddArgsReturnCMD(p, cmdText);
                res = InnerExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("使用实体类删除数据时发生异常: " + ex);
            }

            return res;
        }

        /// <summary>
        /// 更新符合实体对象属性值作为条件的记录
        /// </summary>
        /// <param name="p">实体对象</param>
        /// <returns>-1表示操作失败</returns>
        public int Update(Parent p)
        {
            string validMsg = "";
            if (!p.UpdateValid(out validMsg)) { throw new Exception("类型 " + p.ToString() + " 的实体对象中的数据未通过验证,不能保存到数据库,原因:" + validMsg); }
            int res = -1;
            if (p.CountFields() > 0)
            {
                try
                {
                    Hashtable ht = p.MappingTableInfo();
                    string table = ht[Constants.STR_DB_TABLE].ToString();
                    string pk = p.MappingTableInfo()[Constants.STR_DB_PK].ToString();
                    string update = GetFields(p, DB_Condition.All);
                    string where = "";
                    if (p.HasValue(pk))//如果主键设置了值
                    {
                        where = " where " + pk + "=" + GetDBParmString + pk;
                    }
                    if (p.OtherCondition != null && !string.IsNullOrWhiteSpace(p.OtherCondition.ConditionSQL))
                    {
                        where = GetWhere(where, p.OtherCondition.ConditionSQL.Replace(":", GetDBParmString));
                    }

                    string cmdText = string.Format("UPDATE {0} SET {1} {2}", table, update, where);
                    DbCommand cmd = AddArgsReturnCMD(p, cmdText);
                    res = InnerExecuteNonQuery(cmd);
                }
                catch (Exception ex)
                {
                    throw new Exception("使用实体类更新数据时发生异常: " + ex);
                }
            }
            return res;
        }

        /// <summary>
        /// 以实体对象属性值作为条件查询
        /// </summary>
        /// <param name="p">实体对象</param>
        /// <param name="columnsName">要查询的列名</param>
        /// <param name="orderByColumnName">排序的列</param>
        /// <param name="orderMode">排序模式，默认是Desc</param>
        /// <returns>null表示操作失败</returns>
        public DataTable Select(Parent p, string columnsName, string orderByColumnName, string orderMode)
        {
            string sql = GetSelectStr(p, columnsName) + GetOrderString(orderByColumnName, orderMode);
            return InnerExecuteQueryAsDataTable(AddArgsReturnCMD(p, sql));
        }

        /// <summary>
        /// 以实体对象属性值作为条件随机查询，让每次返回的数据顺序不一样
        /// </summary>
        /// <param name="columnsName">要查询的列名,用逗号分开</param>
        /// <param name="p">实体对象</param>
        /// <returns>null表示操作失败</returns>
        public DataTable SelectRandom(Parent p, string columnsName)
        {
            return Select(p, columnsName, GetRandomString, "ASC");
        }


        /// <summary>
        ///  以实体对象属性值作为条件查询,带分页
        /// </summary>
        /// <param name="p">实体对象</param>
        /// <param name="columnsName">要查询的列名,用逗号分开</param>
        ///<param name="pager">分页对象</param>
        /// <param name="orderByColumnName">排序字段名称,默认Create_Time</param>
        /// <param name="orderMode">排序模式，默认Desc</param>
        /// <returns>null表示操作失败</returns>
        public DataTable SelectPage(Parent p, Pager pager, string columnsName, string orderByColumnName, string orderMode)
        {
            string sqlData = GetSelectStr(p, columnsName) + GetOrderString(orderByColumnName, orderMode);

            string sqlCount = GetCountStr(p);

            #region 获取总行数


            object o = InnerExecuteSingleValue(AddArgsReturnCMD(p, sqlCount));
            if (o != null)
            {
                pager.RowCount = Int32.Parse(o.ToString());
            }
            #endregion

            string sqlPage = GetPageSQL(pager, sqlData);

            return InnerExecuteQueryAsDataTable(AddArgsReturnCMD(p, sqlPage));


        }


        /// <summary>
        /// 以实体对象属性值作为条件随机查询，让每次返回的数据顺序不一样,带分页
        /// </summary>
        /// <param name="p">实体对象</param>
        /// <param name="pager">分页对象</param>
        /// <param name="columnsName">要查询的列名,用逗号分开</param>
        /// <returns>null表示操作失败</returns>
        public DataTable SelectPageRandom(Parent p, Pager pager, string columnsName)
        {
            return SelectPage(p, pager, columnsName, GetRandomString, "ASC");
        }

        /// <summary>
        /// 两表join查询
        /// </summary>
        /// <param name="join">join对象</param>
        /// <param name="orderByColumnName"></param>
        /// <param name="orderMode"></param>
        /// <returns>null表示操作失败</returns>
        public DataTable SelectJoin(Join join, string orderByColumnName, string orderMode)
        {
            string sql = GetJoinSQL(join) + GetOrderString(orderByColumnName, orderMode);
            return InnerExecuteQueryAsDataTable(AddArgsReturnCMD(new Parent[] { join.LeftModel, join.RightModel }, sql));

        }

        /// <summary>
        /// 两表join查询
        /// </summary>
        /// <param name="join">join对象</param>
        /// <param name="orderByColumnName">排序列名称</param>
        /// <param name="orderMode">排序模式,升还是降</param>
        /// <returns>null表示操作失败</returns>
        public DataTable SelectJoinRandom(Join join)
        {
            return SelectJoin(join, GetRandomString, "Asc");
        }

        /// <summary>
        /// 两表join分页查询
        /// </summary>
        /// <param name="join">join对象</param>
        /// <param name="pager">分页对象</param>
        /// <param name="orderByColumnName">排序列名称</param>
        /// <param name="orderMode">排序模式,升还是降</param>
        /// <returns>null表示操作失败</returns>
        public DataTable SelectJoinPage(Join join, Pager pager, string orderByColumnName, string orderMode)
        {
            string joinSql = GetJoinSQL(join) + GetOrderString(orderByColumnName, orderMode);
            #region 获取总行数

            string sqlCount = "select count(1) from (" + joinSql + ") r ";
            object o = InnerExecuteSingleValue(AddArgsReturnCMD(new Parent[] { join.LeftModel, join.RightModel }, sqlCount));
            if (o != null)
            {
                pager.RowCount = Int32.Parse(o.ToString());
            }
            #endregion

            string sqlPage = GetPageSQL(pager, joinSql);
            return InnerExecuteQueryAsDataTable(AddArgsReturnCMD(new Parent[] { join.LeftModel, join.RightModel }, sqlPage));


        }

        /// <summary>
        /// 两表join分页查询后,随机返回记录
        /// </summary>
        /// <param name="join">join对象</param>
        /// <param name="pager">分页对象</param>
        /// <returns>null表示操作失败</returns>
        public DataTable SelectJoinPageRandom(Join join, Pager pager)
        {
            return SelectJoinPage(join, pager, GetRandomString, "ASC");
        }



        /// <summary>
        /// 两表Union查询
        /// </summary>
        /// <param name="union">Union对象</param>
        /// <param name="orderByColumnName">排序字段,请用AModel中的字段</param>
        /// <param name="orderMode">排序模式</param>
        /// <returns>null表示操作失败</returns>
        public DataTable SelectUnion(Union union, string orderByColumnName, string orderMode)
        {
            string sql = GetUnionSQL(union) + GetOrderString(orderByColumnName, orderMode);
            return InnerExecuteQueryAsDataTable(AddArgsReturnCMD(new Parent[] { union.AModel, union.BModel }, sql));

        }

        /// <summary>
        /// 两表Union查询
        /// </summary>
        /// <param name="union">Union对象</param>
        /// <param name="orderByColumnName">排序列名称,请用AModel中的字段</param>
        /// <param name="orderMode">排序模式,升还是降</param>
        /// <returns>null表示操作失败</returns>
        public DataTable SelectUnionRandom(Union union)
        {
            return SelectUnion(union, GetRandomString, "Asc");
        }

        /// <summary>
        /// 两表Union分页查询
        /// </summary>
        /// <param name="union">Union对象</param>
        /// <param name="pager">分页对象</param>
        /// <param name="orderByColumnName">排序列名称,请用AModel中的字段</param>
        /// <param name="orderMode">排序模式,升还是降</param>
        /// <returns>null表示操作失败</returns>
        public DataTable SelectUnionPage(Union union, Pager pager, string orderByColumnName, string orderMode)
        {
            string UnionSql = GetUnionSQL(union) + GetOrderString(orderByColumnName, orderMode);
            #region 获取总行数

            string sqlCount = "select count(1) from (" + UnionSql + ") r ";
            object o = InnerExecuteSingleValue(AddArgsReturnCMD(new Parent[] { union.AModel, union.BModel }, sqlCount));
            if (o != null)
            {
                pager.RowCount = Int32.Parse(o.ToString());
            }
            #endregion

            string sqlPage = GetPageSQL(pager, UnionSql);
            return InnerExecuteQueryAsDataTable(AddArgsReturnCMD(new Parent[] { union.AModel, union.BModel }, sqlPage));


        }

        /// <summary>
        /// 两表Union分页查询后,随机返回记录
        /// </summary>
        /// <param name="union">Union对象</param>
        /// <param name="pager">分页对象</param>
        /// <returns>null表示操作失败</returns>
        public DataTable SelectUnionPageRandom(Union union, Pager pager)
        {
            return SelectUnionPage(union, pager, GetRandomString, "ASC");
        }

        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <param name="paras">命令参数</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">SQL语句,如果语句中带参数,参数前缀统一用:来表示(注意!参数标示统一用冒号:做前缀,将在底层统一替换)</param>
        /// <returns>false表示操作失败</returns>
        public bool ExecuteNonQuery(string cmdText, CommandType cmdType, DbParameter[] paras)
        {

            bool flag = false;
            using (DbConnection con = db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdText.Replace(":", GetDBParmString);
                if (paras != null && paras.Length > 0)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(paras);
                }
                try
                {
                    db.ExecuteNonQuery(cmd);
                    flag = true;
                }
                catch (Exception ex)
                {
                    string exceptionSQL = BuildExceptionSQL(cmd);
                    throw new Exception("执行非查询语句 " + exceptionSQL + " 时出现异常: " + ex);
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            return flag;
        }

        /// <summary>
        /// 执行SQL查询命令
        /// 返回首行首列值
        /// Exception:NULL
        /// </summary>
        /// <param name="cmdText">命令语句</param>
        /// <param name="cmdText">SQL语句,如果语句中带参数,参数前缀统一用:来表示(注意!参数标示统一用冒号:做前缀,将在底层统一替换)</param>
        /// <param name="paras">命令参数</param>
        /// <returns>null表示操作失败</returns>
        public object ExecuteSingleValue(string cmdText, CommandType cmdType, DbParameter[] paras)
        {
            object o = null;
            using (DbConnection con = db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdText.Replace(":", GetDBParmString);
                if (paras != null && paras.Length > 0)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(paras);
                }
                try
                {
                    o = db.ExecuteScalar(cmd);
                }
                catch (Exception ex)
                {
                    string exceptionSQL = BuildExceptionSQL(cmd);
                    throw new Exception("执行查询语句 " + exceptionSQL + " 时出现异常: " + ex);
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            return o;
        }

        /// <summary>
        /// 执行SQL查询命令
        /// 返回:DataSet
        /// Exception:NULL
        /// </summary>
        /// <param name="cmdText">SQL语句,如果语句中带参数,参数前缀统一用:来表示(注意!参数标示统一用冒号:做前缀,将在底层统一替换)</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="paras">命令参数</param>
        /// <returns>null表示操作失败</returns>
        public DataSet ExecuteQueryAsDataSet(string cmdText, CommandType cmdType, DbParameter[] paras)
        {
            DataSet ds = null;
            using (DbConnection con = db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdText.Replace(":", GetDBParmString);
                if (paras != null && paras.Length > 0)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(paras);
                }
                try
                {
                    ds = db.ExecuteDataSet(cmd);
                }
                catch (Exception ex)
                {
                    string exceptionSQL = BuildExceptionSQL(cmd);
                    throw new Exception("执行查询语句 " + exceptionSQL + " 时出现异常: " + ex);
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            return ds;
        }

        /// <summary>
        /// 执行SQL查询命令
        /// 返回:DataTable
        /// Exception:NULL
        /// </summary>
        /// <param name="cmdText">SQL语句,如果语句中带参数,参数前缀统一用:来表示(注意!参数标示统一用冒号:做前缀,将在底层统一替换)</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="paras">命令参数</param>
        /// <param name="pager">分页对象</param>
        /// <returns>null表示操作失败</returns>
        public DataTable ExecuteQueryAsDataTable(string cmdText, CommandType cmdType, DbParameter[] paras, Pager pager)
        {
            DataTable dt = null;
            using (DbConnection con = db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdText.Replace(":", GetDBParmString);

                if (paras != null && paras.Length > 0)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(paras);
                }
                try
                {
                    dt = db.ExecuteDataSet(cmd).Tables[0];
                }
                catch (Exception ex)
                {
                    string exceptionSQL = BuildExceptionSQL(cmd);
                    throw new Exception("执行查询语句 " + exceptionSQL + " 时出现异常: " + ex);
                }
                finally
                {
                    cmd.Dispose();
                }
            }

            /*
             *通常调用此方法执行的语句都是较为复杂的语句,
             *所以获取分页的总记录数时不要再查一次数据库,
             * 而是直接不分页查询,然后在内存中筛选
             * 更好的方式是不传此参数,通过在外层使用缓存辅助类CacheHelper.GetPageDataTable这个方法来将此方法返回的所有结果缓存后,分页输出
             */
            if (pager != null)
            {
                dt = dt.SelectPage(pager, null);
            }
            return dt;
        }

        #endregion

        #region Trans

        /// <summary>
        /// 在同一事务中插入个实体对象,可用不同的实体对象向多表中插入数据
        /// </summary>
        /// <param name="p">实体对象集合</param>
        /// <returns>false表示操作失败</returns>
        public bool TransInsert(List<Parent> listP)
        {
            if (listP == null || listP.Count < 1) { return true; }
            bool flag = false;
            using (DbConnection con = db.CreateConnection())
            {
                DbTransaction trans = null;
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    string validMsg = "";
                    foreach (Parent p in listP)
                    {
                        if (!p.InsertValid(out validMsg)) { throw new Exception("类型 " + p.ToString() + " 的实体对象中的数据未通过验证,不能保存到数据库,因为:" + validMsg); }
                        Hashtable ht = p.MappingTableInfo();
                        if (p.CountFields() > 0)
                        {
                            string table = ht[Constants.STR_DB_TABLE].ToString();
                            string key = GetFields(p, DB_Condition.Key);
                            string value = GetFields(p, DB_Condition.Value);
                            string cmdText = string.Format("INSERT INTO {0}({1}) VALUES({2})", table, key, value);
                            DbCommand cmd = AddArgsReturnCMD(p, cmdText);
                            db.ExecuteNonQuery(cmd, trans);
                        }

                    }
                    trans.Commit();
                    flag = true;
                }
                catch (Exception ex)
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    throw new Exception("使用事务批量插入实体类数据时发生异常: " + ex);
                }
                finally
                {
                    if (trans != null)
                    {
                        trans.Dispose();
                    }

                }
            }
            return flag;
        }

        /// <summary>
        /// 在同一事务中更新多个实体对象属性值作为条件的记录,可用不同的实体对象更新多表的数据
        /// </summary>
        /// <param name="p">实体对象集合</param>
        /// <returns>false表示操作失败</returns>
        public bool TransUpdate(List<Parent> listP)
        {
            if (listP == null || listP.Count < 1) { return true; }
            bool flag = false;
            using (DbConnection con = db.CreateConnection())
            {
                DbTransaction trans = null;
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    string validMsg = "";
                    foreach (Parent p in listP)
                    {
                        if (!p.UpdateValid(out validMsg)) { throw new Exception("类型 " + p.ToString() + " 的实体对象中的数据未通过验证,不能保存到数据库,因为:" + validMsg); }
                        Hashtable ht = p.MappingTableInfo();
                        string table = ht[Constants.STR_DB_TABLE].ToString();
                        string pk = p.MappingTableInfo()[Constants.STR_DB_PK].ToString();
                        string update = GetFields(p, DB_Condition.All);
                        string where = "";
                        if (p.HasValue(pk))//如果主键设置了值
                        {
                            where = " where " + pk + "=" + GetDBParmString + pk;
                        }
                        if (p.OtherCondition != null && !string.IsNullOrWhiteSpace(p.OtherCondition.ConditionSQL))
                        {
                            where = GetWhere(where, p.OtherCondition.ConditionSQL.Replace(":", GetDBParmString));
                        }
                        string cmdText = string.Format("UPDATE {0} SET {1} {2}", table, update, where);
                        DbCommand cmd = AddArgsReturnCMD(p, cmdText);
                        db.ExecuteNonQuery(cmd, trans);
                    }
                    trans.Commit();
                    flag = true;
                }
                catch (Exception ex)
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    throw new Exception("使用事务批量更新实体类数据时发生异常: " + ex);
                }
                finally
                {
                    if (trans != null)
                    {
                        trans.Dispose();
                    }
                }
            }
            return flag;
        }

        /// <summary>
        /// 在同一事务中删除个实体对象属性值作为条件的记录,可用不同类型的实体对象删除多张表
        /// </summary>
        /// <param name="p">实体对象集合</param>
        /// <returns>false表示操作失败</returns>
        public bool TransDelete(List<Parent> listP)
        {
            if (listP == null || listP.Count < 1) { return true; }
            bool flag = false;
            using (DbConnection con = db.CreateConnection())
            {
                DbTransaction trans = null;
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    foreach (Parent p in listP)
                    {
                        Hashtable ht = p.MappingTableInfo();
                        string table = ht[Constants.STR_DB_TABLE].ToString();
                        string where = GetWhereCondition(p);
                        if (string.IsNullOrWhiteSpace(where))
                        {
                            throw new Exception("删除数据必须带条件");
                        }
                        string cmdText = string.Format("DELETE FROM {0}{1}", table, where);
                        DbCommand cmd = AddArgsReturnCMD(p, cmdText);
                        db.ExecuteNonQuery(cmd, trans);

                    }
                    trans.Commit();
                    flag = true;
                }
                catch (Exception ex)
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    throw new Exception("使用事务批量删除实体类数据时发生异常: " + ex);
                }
                finally
                {
                    if (trans != null)
                    {
                        trans.Dispose();
                    }

                }
            }
            return flag;
        }

        /// <summary>
        /// 执行多条非查询的SQL命令
        /// 支持事务
        /// 返回：True 事务执行成功;False 执行失败，事务回滚
        /// </summary>
        /// <param name="cmdTextDic">命令集合，键为执行SQL语句..如果语句中带参数,参数前缀统一用:来表示(注意!参数标示统一用冒号:做前缀,将在底层统一替换),  值为SQL语句参数的集合</param>
        /// <returns>false表示操作失败</returns>
        public bool TransExecuteNonQuery(Dictionary<string, DbParameter[]> cmdTextDic)
        {
            if (cmdTextDic == null || cmdTextDic.Count < 1) { return true; }
            bool flag = false;
            using (DbConnection con = db.CreateConnection())
            {
                DbTransaction trans = null;
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    foreach (string cmdText in cmdTextDic.Keys)
                    {
                        DbCommand cmd = con.CreateCommand();
                        cmd.Transaction = trans;
                        cmd.CommandType = CommandType.Text;

                        DbParameter[] paras = cmdTextDic[cmdText];
                        if (paras != null && paras.Length > 0)
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddRange(paras);
                        }
                        cmd.CommandText = cmdText.Replace(":", GetDBParmString);
                        cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                    flag = true;
                }
                catch (Exception ex)
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    throw new Exception("在同一事务中执行多条语句时出现异常: " + ex);
                }
                finally
                {
                    if (trans != null)
                    {
                        trans.Dispose();
                    }
                }
            }
            return flag;
        }

        /// <summary>
        /// 在同一事务中对多个表进行增删改操作,根据多个实体与操作类型操作数据库
        /// </summary>
        /// <param name="dicModel">实体与操作映射的集合</param>
        /// <returns></returns>
        public bool TransExecuteModels(Dictionary<Parent, Enums.DbOptionType> dicModel)
        {
            bool res = false;
            if (dicModel != null && dicModel.Count > 0)
            {
                using (DbConnection con = db.CreateConnection())
                {
                    con.Open();
                    DbTransaction tran = con.BeginTransaction();
                    try
                    {
                        foreach (Parent p in dicModel.Keys)
                        {
                            Enums.DbOptionType opType = dicModel[p];
                            Hashtable ht = p.MappingTableInfo();
                            string table = ht[Constants.STR_DB_TABLE].ToString();
                            string validMsg = "";
                            switch (opType)
                            {
                                case Enums.DbOptionType.Insert:

                                    if (!p.InsertValid(out validMsg)) { throw new Exception("类型 " + p.ToString() + " 的实体对象中的数据未通过验证,不能保存到数据库,因为:" + validMsg); }
                                    if (p.CountFields() > 0)
                                    {
                                        string key = GetFields(p, DB_Condition.Key);
                                        string value = GetFields(p, DB_Condition.Value);
                                        string cmdText = string.Format("INSERT INTO {0}({1}) VALUES({2})", table, key, value);
                                        DbCommand cmd = AddArgsReturnCMD(p, cmdText);
                                        db.ExecuteNonQuery(cmd, tran);
                                    }
                                    break;
                                case Enums.DbOptionType.Update:

                                    if (!p.UpdateValid(out validMsg)) { throw new Exception("类型 " + p.ToString() + " 的实体对象中的数据未通过验证,不能保存到数据库,因为:" + validMsg); }
                                    if (p.CountFields() > 0)
                                    {
                                        string pk = p.MappingTableInfo()[Constants.STR_DB_PK].ToString();
                                        string update = GetFields(p, DB_Condition.All);
                                        string where = "";
                                        if (p.HasValue(pk))//如果主键设置了值
                                        {
                                            where = " where " + pk + "=" + GetDBParmString + pk;
                                        }
                                        if (p.OtherCondition != null && !string.IsNullOrWhiteSpace(p.OtherCondition.ConditionSQL))
                                        {
                                            where = GetWhere(where, p.OtherCondition.ConditionSQL.Replace(":", GetDBParmString));
                                        }

                                        string cmdText = string.Format("UPDATE {0} SET {1} {2}", table, update, where);
                                        DbCommand cmd = AddArgsReturnCMD(p, cmdText);
                                        db.ExecuteNonQuery(cmd, tran);
                                    }
                                    break;
                                case Enums.DbOptionType.Delete:

                                    string deleteWhere = GetWhereCondition(p);
                                    if (string.IsNullOrWhiteSpace(deleteWhere))
                                    {
                                        throw new Exception("删除数据前,必须为实体" + p.ToString() + "的属性赋值作为删除条件");
                                    }
                                    string deleteCmdText = string.Format("DELETE FROM {0}{1}", table, deleteWhere);
                                    DbCommand deleteCMD = AddArgsReturnCMD(p, deleteCmdText);
                                    db.ExecuteNonQuery(deleteCMD, tran);
                                    break;
                            }
                        }

                        tran.Commit();
                        tran.Dispose();
                        res = true;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        tran.Dispose();
                        throw new Exception("批量用实体对数据库进行某操作时发生异常:" + ex);
                    }
                }
            }
            return res;
        }

        #endregion

        #region  私有方法

        /// <summary>
        /// 执行SQL查询命令 (DAO内使用)
        /// 返回:DataTable
        /// Exception:NULL
        /// </summary>
        /// <param name="cmd">命令语句</param>
        /// <returns></returns>
        private DataTable InnerExecuteQueryAsDataTable(DbCommand cmd)
        {
            DataTable dt = null;
            using (DbConnection con = db.CreateConnection())
            {
                try
                {
                    dt = db.ExecuteDataSet(cmd).Tables[0];
                }
                catch (Exception ex)
                {
                    string exceptionSQL = BuildExceptionSQL(cmd);
                    throw new Exception("执行查询语句 " + exceptionSQL + " 时出现异常: " + ex);
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            return dt;
        }

        /// <summary>
        /// 查询单一值 (DAO内使用)
        /// 返回:首行首列结果
        /// Exception: Null
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <returns>object</returns>
        private object InnerExecuteSingleValue(DbCommand cmd)
        {
            object obj = null;
            using (DbConnection con = db.CreateConnection())
            {
                try
                {
                    obj = db.ExecuteScalar(cmd);
                }
                catch (Exception ex)
                {
                    string exceptionSQL = BuildExceptionSQL(cmd);
                    throw new Exception("执行查询语句 " + exceptionSQL + " 时出现异常: " + ex);
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            return obj;
        }

        /// <summary>
        /// 执行非查询语句 (DAO内使用)
        /// 返回:受影响行数
        /// Exception: Null
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <returns>object</returns>
        private int InnerExecuteNonQuery(DbCommand cmd)
        {
            int count = -1;
            using (DbConnection con = db.CreateConnection())
            {
                try
                {
                    count = db.ExecuteNonQuery(cmd);
                }
                catch (Exception ex)
                {
                    string exceptionSQL = BuildExceptionSQL(cmd);
                    throw new Exception("执行非查询语句 " + exceptionSQL + " 时出现异常: " + ex);
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            return count;
        }


        #endregion

        #region 辅助方法
        /// <summary>
        /// 构建发生异常的SQL语句,将参数替换成具体值后存放到日志中,方便调试的.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private string BuildExceptionSQL(DbCommand cmd)
        {
            if (cmd != null)
            {
                string sql = cmd.CommandText;
                foreach (DbParameter item in cmd.Parameters)
                {
                    string argsValue = "";
                    object o = item.Value;
                    if (o.GetType().ToString().ContainsUL("string") || o.GetType().ToString().ContainsUL("datetime"))
                    {
                        argsValue = "'" + item.Value + "'";
                    }
                    else
                    {
                        argsValue = o.ToString();
                    }
                    sql = sql.Replace(GetDBParmString + item.ParameterName, argsValue);
                }
                return sql;
            }
            return "";
        }



        /// <summary>
        /// 获取WHERE部分语句
        /// </summary>
        /// <param name="p">实体父类</param>
        /// <returns></returns>
        private string GetWhereCondition(Parent p)
        {
            string where = "";
            int count = p.CountFields();
            if (count > 0)
            {
                string columnName = "";
                for (int i = 0; i < count; i++)
                {
                    columnName = p.ColumnName(i);
                    where += string.Format(" AND {0} = " + GetDBParmString + "{1}", columnName, columnName);
                }
                where = where.Substring(4);
                where = " where " + where;
            }

            if (p.OtherCondition != null && !string.IsNullOrWhiteSpace(p.OtherCondition.ConditionSQL))
            {
                if (string.IsNullOrWhiteSpace(where))
                {
                    where += " where " + p.OtherCondition.ConditionSQL.Replace(":", GetDBParmString);
                }
                else
                {
                    where += " and " + p.OtherCondition.ConditionSQL.Replace(":", GetDBParmString);
                }
            }

            return where;
        }

        /// <summary>
        /// 准备SQL条件语句
        /// </summary>
        /// <param name="p">实体父类</param>
        /// <param name="c">条件类型</param>
        /// <returns></returns>
        private string GetFields(Parent p, DB_Condition c)
        {
            string condition = "";
            int count = p.CountFields();
            string column = "";
            string pk = p.MappingTableInfo()[Constants.STR_DB_PK].ToString();
            object value;
            for (int i = 0; i < count; i++)
            {
                column = p.ColumnName(i);
                value = p.ColumnValue(i);

                switch (c)
                {
                    case DB_Condition.Key:
                        condition += string.Format("{0},", column); break;
                    case DB_Condition.Value:

                        condition += string.Format("{0},", GetDBParmString + column); break;
                    case DB_Condition.All:
                        //Update操作时忽略主键的更新
                        if (column.Equals(pk))
                        {
                            continue;
                        }
                        else
                        {
                            condition += string.Format("{0} = " + GetDBParmString + "{1},", column, column);
                        }
                        break;
                }
            }
            if (condition.Length > 0)
            {
                condition = condition.Substring(0, condition.Length - 1);
            }
            return condition;
        }

        /// <summary>
        /// 添加参数并返回数据库处理命令
        /// </summary>
        /// <param name="p">实体父类</param>
        /// <param name="cmdText">SQL命令</param>
        /// <returns></returns>
        private DbCommand AddArgsReturnCMD(Parent p, string cmdText)
        {
            try
            {
                DbCommand cmd = db.GetSqlStringCommand(cmdText);

                string column = "";
                object value;
                int count = p.CountFields();
                for (int i = 0; i < count; i++)
                {
                    column = p.ColumnName(i);
                    value = p.ColumnValue(i);
                    if (value == null)
                    {
                        value = DBNull.Value;
                    }
                    DbParameter para = GetDBParam(column, value);
                    cmd.Parameters.Add(para);
                }
                if (p.OtherCondition != null && !string.IsNullOrWhiteSpace(p.OtherCondition.ConditionSQL) && p.OtherCondition.ArgsArr != null)
                {
                    foreach (Args item in p.OtherCondition.ArgsArr)
                    {
                        if (!cmd.Parameters.Contains(item.Name))
                        {
                            cmd.Parameters.Add(GetDBParam(item.Name, item.Value));
                        }

                    }
                }
                return cmd;
            }
            catch (Exception ex)
            {
                throw new Exception("给SQL语句添加参数时出现异常: " + ex);
            }

        }



        private DbCommand AddArgsReturnCMD(Parent[] pArr, string cmdText)
        {
            try
            {
                DbCommand cmd = db.GetSqlStringCommand(cmdText);

                string column = "";
                object value;
                foreach (Parent p in pArr)
                {
                    int count = p.CountFields();
                    for (int i = 0; i < count; i++)
                    {
                        column = p.ColumnName(i);
                        value = p.ColumnValue(i);
                        if (value == null)
                        {
                            value = DBNull.Value;
                        }
                        if (!cmd.Parameters.Contains(column))
                        {
                            cmd.Parameters.Add(GetDBParam(column, value));
                        }

                    }
                    if (p.OtherCondition != null && !string.IsNullOrWhiteSpace(p.OtherCondition.ConditionSQL) && p.OtherCondition.ArgsArr != null)
                    {
                        foreach (Args item in p.OtherCondition.ArgsArr)
                        {
                            if (!cmd.Parameters.Contains(item.Name))
                            {
                                cmd.Parameters.Add(GetDBParam(item.Name, item.Value));
                            }

                        }
                    }
                }


                return cmd;
            }
            catch (Exception ex)
            {
                throw new Exception("用多个实体给SQL语句添加参数时出现异常: " + ex);
            }

        }

        /// <summary>
        /// 根据实体获取查询字符串
        /// </summary>
        /// <param name="p">实体</param>
        /// <param name="columnsName">要查询的列名称,用逗号分开,默认查询*</param>
        /// <returns></returns>
        private string GetSelectStr(Parent p, string columnsName)
        {
            Hashtable ht = p.MappingTableInfo();
            string sql = "select * from " + ht[Constants.STR_DB_TABLE].ToString();
            if (!string.IsNullOrWhiteSpace(columnsName))
            {
                sql = sql.Replace("*", " " + columnsName + " ");
            }
            sql += "\r\n" + GetWhereCondition(p);

            return sql;
        }

        /// <summary>
        /// 根据实体获取统计字符串
        /// </summary>
        /// <param name="p">实体</param>
        /// <returns></returns>
        private string GetCountStr(Parent p)
        {
            Hashtable ht = p.MappingTableInfo();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select count(1) from " + ht[Constants.STR_DB_TABLE].ToString());
            sb.AppendLine(GetWhereCondition(p));
            return sb.ToString();
        }


        /// <summary>
        /// 获取join查询的SQL
        /// </summary>
        /// <param name="join">join对象</param>
        /// <returns></returns>
        string GetJoinSQL(Join join)
        {
            string lFields = join.LeftShowFields;
            string rFields = join.RightShowFields;
            if (lFields != "/")
            {
                lFields = string.IsNullOrWhiteSpace(join.LeftShowFields) ? " l.* " : "l." + join.LeftShowFields.Replace(",", ",l.");
            }
            if (rFields != "/")
            {
                rFields = string.IsNullOrWhiteSpace(join.RightShowFields) ? " r.* " : "r." + join.RightShowFields.Replace(",", ",r.");
            }

            string tmpSQL = "select ";
            if (lFields != "/" && rFields != "/")
            {
                tmpSQL += lFields + "," + rFields;
            }
            else if (lFields != "/")
            {
                tmpSQL += lFields;
            }
            else if (rFields != "/")
            {
                tmpSQL += rFields;
            }
            else
            {
                throw new Exception("join 查询时必须至少指定一个表返回的字段");
            }
            string sql = tmpSQL + @" from ({0}) l "

                + join.JoinType.GetString() + " ({1}) r"
                + " on {2}";
            string[] onL = join.LeftOnFields.Split(',');
            string[] onR = join.RightOnFields.Split(',');
            if (onL.Length != onR.Length)
            {
                return null;
            }
            StringBuilder sbOn = new StringBuilder();
            for (int i = 0; i < onL.Length; i++)
            {
                sbOn.AppendFormat("and l.{0}=r.{1}\r\n ", onL[i], onR[i]);
            }
            sbOn.Remove(0, 3);
            string sqlL = GetSelectStr(join.LeftModel, join.LeftInnerQueryFields);
            string sqlR = GetSelectStr(join.RightModel, join.RightInnerQueryFields);
            sql = string.Format(sql, sqlL, sqlR, sbOn.ToString());
            return sql;
        }


        /// <summary>
        /// 获取union查询的SQL
        /// </summary>
        /// <param name="union">union对象</param>
        /// <returns></returns>
        string GetUnionSQL(Union union)
        {
            string aFields = string.IsNullOrWhiteSpace(union.AShowFields) ? " * " : union.AShowFields;
            string bFields = string.IsNullOrWhiteSpace(union.BShowFields) ? " * " : union.BShowFields;

            string[] aFieldArr = aFields.Split(',');
            string[] bFieldArr = bFields.Split(',');

            if (aFieldArr.Length != bFieldArr.Length)
            {
                throw new Exception("进行Union操作的两个表,显示的 字段个数 必须相等");
            }

            string sql = @" select * from ({0} " + union.UnionType.GetString() + " {1}) res ";

            string sqlA = GetSelectStr(union.AModel, aFields);
            string sqlB = GetSelectStr(union.BModel, bFields);
            sql = string.Format(sql, sqlA, sqlB);
            return sql;
        }

        /// <summary>
        /// 获取排序字符串
        /// </summary>
        /// <param name="orderByColumnName">排序列名</param>
        /// <param name="orderMode">排序模式</param>
        /// <returns></returns>
        private static string GetOrderString(string orderByColumnName, string orderMode)
        {
            string orderString = "";
            if (!string.IsNullOrWhiteSpace(orderByColumnName) && !string.IsNullOrWhiteSpace(orderMode))
            {
                orderString = " order by " + orderByColumnName + " " + orderMode + " ";
            }
            return orderString;
        }

        #endregion

    }
}
