using SoEasy.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace System.Data
{

    /// <summary>
    /// DataTable扩展类
    /// </summary>
    public static class DataTableExtension
    {
        /// <summary>
        /// 从表中的某列数据获取List<T>
        /// </summary>
        /// <typeparam name="T">要获取的类型</typeparam>
        /// <param name="dt"></param>
        /// <param name="fieldName">表中的数据列名称,表示将哪列数据作为List的数据源</param>
        /// <param name="opRes"></param>
        /// <returns></returns>
        public static List<T> FieldToList<T>(this DataTable dt, string fieldName, OPResult opRes)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                List<T> list = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    list = new List<T>();
                    DataColumn col = dt.Columns[fieldName];
                    if (col == null)
                    {
                        throw new Exception("从DataTable获取List<T>时发生异常:表中不存在名为" + fieldName + "的列");
                    }
                    else
                    {
                        Type dataType = typeof(T);
                        foreach (DataRow item in dt.Rows)
                        {
                            T t = (T)Convert.ChangeType(item[fieldName], dataType);
                            list.Add(t);
                        }
                    }

                }

                return list;
            }, opRes, false);
        }

        /// <summary>
        /// DataTable转成字典List,在将DataTable转成Json字符串之前,要先调用此方法转成List,DataTable不能直接转Json
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ToDictionaryList(this DataTable dt)
        {
            if (dt == null) { return null; }
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)//每一行信息，新建一个Dictionary<string,object>,将该行的每列信息加入到字典
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc].ToString());
                }
                list.Add(result);
            }
            return list;
        }

        /// <summary>
        /// DataTable转成Json字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJsonString(this DataTable dt)
        {
            return JsonHelper.ToJsonString(dt.ToDictionaryList());
        }

        /// <summary>
        /// 从DataTable中获取分页后的数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="pager"></param>
        /// <param name="opRes"></param>
        /// <returns></returns>
        public static DataTable SelectPage(this DataTable dt, Pager pager, OPResult opRes)
        {
            DataTable dtRes = null;
            if (dt != null && pager != null)
            {
                pager.ValidArgs();
                int pageBegin = (pager.PageIndex - 1) * pager.PageSize;
                int pageEnd = pageBegin + pager.PageSize;
                pager.RowCount = dt.Rows.Count;
                if (!dt.Columns.Contains("Row_Number"))
                {
                    dt = dt.AddRowNumber(opRes);
                }
                dtRes = dt.SelectReturnTable("Row_Number>" + pageBegin + " and Row_Number<=" + pageEnd, opRes);

            }
            else
            {
                return dt;
            }
            return dtRes;
        }

        /// <summary>
        /// 判断一个表中是否是空表(没有数据也视为空表)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>是返回True</returns>
        public static bool IsNullOrEmpty(this DataTable dt)
        {
            return dt == null || dt.Rows.Count < 1;
        }

        /// <summary>
        /// 过虑DT的数据,并返回过虑后的结果表
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filterExpress">过虑表达式</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <returns></returns>
        public static DataTable SelectReturnTable(this DataTable dt, string filterExpress, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                DataTable dtRes = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    dtRes = dt.Clone();
                    DataRow[] drs = null;
                    try
                    {
                        drs = dt.Select(filterExpress);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("从DataTable中筛选数据时发生异常,请检查过虑表达式" + filterExpress + "的拼写是否正确.", ex);
                    }

                    if (drs != null && drs.Length > 0)
                    {
                        foreach (DataRow row in drs)
                        {
                            dtRes.Rows.Add(row.ItemArray);
                        }

                    }
                }
                return dtRes;
            }, opRes, throwException);
        }


        /// <summary>
        /// 返回行表中的第一行
        /// </summary>
        /// <param name="drs"></param>
        /// <returns></returns>
        public static DataRow First(this DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0];
            }
            return null;
        }


        /// <summary>
        /// 比较两个DataTable是否一样,比较构架和表中的数据值
        /// </summary>
        /// <param name="tableA">表A</param>
        /// <param name="tableB">表B</param>
        /// <returns>相同返回true</returns>
        public static bool IsEqualDataTable(this DataTable tableA, DataTable tableB)
        {
            if (tableA == null || tableB == null)
            {
                return false;
            }
            if (tableA.Columns.Count != tableB.Columns.Count) { return false; }

            if (tableA.Rows.Count != tableB.Rows.Count) { return false; }

            for (int i = 0; i < tableA.Columns.Count; i++)//比较架构
            {
                if (tableA.Columns[i].DataType != tableB.Columns[i].DataType || tableA.Columns[i].ColumnName != tableB.Columns[i].ColumnName)
                {
                    return false;
                }

            }
            for (int i = 0; i < tableA.Rows.Count; i++)//比较数据
            {
                object[] arrA = tableA.Rows[i].ItemArray;
                object[] arrB = tableB.Rows[i].ItemArray;
                for (int j = 0; j < arrA.Length; j++)
                {
                    if (!arrA[j].Equals(arrB[j]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 添加整数的序号,新添加的列名默认为Row_Number
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnName">列名称</param>
        /// <returns></returns>
        public static DataTable AddRowNumber(this DataTable dt, OPResult opRes, string columnName = "Row_Number", bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
                {
                    if (dt != null)
                    {
                        int index = 1;
                        try
                        {
                            dt.Columns.Add(columnName, typeof(Int32));
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("添加数据列 " + columnName + " 失败:" + ex);
                        }

                        foreach (DataRow item in dt.Rows)
                        {
                            item[columnName] = index;
                            ++index;
                        }
                    }
                    return dt;
                }, opRes, throwException);

        }

        /// <summary>
        /// 更新符合条件的行
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="setValue">要更新的列和值,更新多个列时用逗号分开 如 Creater='Admin',Name='UserName' </param>
        /// <param name="condition">更新条件,当条件成立时才更新,多个条件用 and 连接 如IsDelete=0 and IsEnable=1</param>
        /// <returns></returns>
        public static DataTable UpdateRows(this DataTable dt, string setValue, string condition, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
               {
                   if (!string.IsNullOrWhiteSpace(setValue))
                   {
                       List<string> setValueColumns = setValue.Split(',').ToList();
                       if (!string.IsNullOrWhiteSpace(condition))
                       {
                           DataRow[] removeRows = dt.Select(condition);
                           if (removeRows != null && removeRows.Length > 0)
                           {
                               foreach (var item in removeRows)
                               {
                                   foreach (string tmp in setValueColumns)
                                   {
                                       item[tmp.Split('=')[0]] = tmp.Split('=')[1];
                                   }
                               }
                           }
                       }
                       else
                       {
                           foreach (DataRow item in dt.Rows)
                           {
                               foreach (string tmp in setValueColumns)
                               {
                                   item[tmp.Split('=')[0].ToString()] = tmp.Split('=')[1];
                               }
                           }

                       }
                   }
                   return dt;
               }, opRes, throwException);
        }

        /// <summary>
        /// 删除符合条件的行
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static DataTable RemoveRowsWhere(this DataTable dt, string condition, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                if (!string.IsNullOrWhiteSpace(condition))
                {
                    DataRow[] removeRows = dt.Select(condition);
                    if (removeRows != null && removeRows.Length > 0)
                    {
                        foreach (var item in removeRows)
                        {
                            dt.Rows.Remove(item);
                        }
                    }
                }
                return dt;
            }, opRes, throwException);
        }

        /// <summary>
        /// 对表中的数字列四舍五入后保留指定位小数,默认保留2位,不改变原有列类型
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataTable FormatNumber(this DataTable dt, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
               {
                   if (dt != null && dt.Rows.Count > 0)
                   {
                       List<string> numCol = new List<string>();
                       foreach (DataColumn col in dt.Columns)
                       {
                           if (col.DataType == typeof(decimal) || col.DataType == typeof(double) || col.DataType == typeof(float))
                           {
                               numCol.Add(col.ColumnName);
                           }
                       }

                       foreach (DataRow row in dt.Rows)
                       {
                           foreach (string col in numCol)
                           {
                               if (row[col] != DBNull.Value)
                               {
                                   if (((decimal)row[col]) != 0)
                                   {
                                       row[col] = Math.Round((decimal)row[col], 2);
                                   }
                               }

                           }
                       }
                   }
                   return dt;
               }, opRes, throwException);
        }


        /// <summary>
        /// 根据表中指定列数据,找出最大值的行并返回
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="DataColName">求最大值的列名称,注意：此列中存放的数据要求必须可以转换成decimal</param>
        /// <returns></returns>
        public static DataRow Max(this DataTable dt, string DataColName, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                if (dt == null || dt.Rows.Count == 0) { return null; }
                try
                {
                    return dt.AsEnumerable().OrderByDescending(x => decimal.Parse(x[DataColName].ToString())).ToList<DataRow>()[0];
                }
                catch
                {

                    throw new Exception("所提供的列'" + DataColName + "'中的数据不能转换成decimal类型。");
                }
            }, opRes, throwException);
        }

        /// <summary>
        /// 根据表中指定列数据,找出最小值的行并返回
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="DataColName">求最小值的列名称,注意：此列中存放的数据要求必须可以转换成decimal</param>
        /// <returns></returns>
        public static DataRow Min(this DataTable dt, string DataColName, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
         {
             if (dt == null || dt.Rows.Count == 0) { throw new Exception("此表中没有数据!"); }
             try
             {
                 return dt.AsEnumerable().OrderBy(x => decimal.Parse(x[DataColName].ToString())).ToList<DataRow>()[0];
             }
             catch
             {

                 throw new Exception("所提供的列'" + DataColName + "'中的数据不能转换成decimal类型。");
             }
         }, opRes, throwException);
        }

        /// <summary>
        /// 向表中添加多行数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rows">要添加的行</param>
        /// <returns></returns>
        public static DataTable AddRows(this DataTable dt, DataRow[] rows, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                try
                {
                    foreach (DataRow row in rows)
                    {
                        dt.Rows.Add(row.ItemArray);
                    }
                    return dt;
                }
                catch
                {
                    throw new Exception("被添加的行和表结构不相同.");
                }
            }, opRes, throwException);
        }
        /// <summary>
        /// 对表中的AB两列数据进行计算(A+B,A-B,A*B,A/B的任意一个)后,将值填充在结果列中
        /// </summary>
        /// <param name="dt">表</param>
        /// <param name="columnNameA">A列的名称</param>
        /// <param name="columnNameB">B列的名称</param>
        /// <param name="columnNameRes">结果列的名称</param>
        /// <param name="calcType">四则运算类型</param>
        /// <param name="opRes">操作结果</param>
        /// <param name="throwException">发生异常是否抛出</param>
        /// <returns></returns>
        public static DataTable Calc(this DataTable dt, string columnNameA, string columnNameB, string columnNameRes, Enums.CalcType calcType, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
           {
               if (dt != null && dt.Rows.Count > 0)
               {
                   dt.Columns.Add(new DataColumn(columnNameRes, typeof(decimal)));
                   foreach (DataRow row in dt.Rows)
                   {
                       decimal a = 0;
                       decimal b = 0;

                       a = decimal.Parse(row[columnNameA].ToString());
                       b = decimal.Parse(row[columnNameB].ToString());

                       switch (calcType)
                       {
                           case Enums.CalcType.Add:
                               row[columnNameRes] = a + b;
                               break;
                           case Enums.CalcType.Subduction:
                               row[columnNameRes] = a - b;
                               break;
                           case Enums.CalcType.Multiply:
                               row[columnNameRes] = a * b;
                               break;
                           case Enums.CalcType.Division:
                               row[columnNameRes] = a / b;
                               break;
                       }

                   }
               }
               return dt;
           }, opRes, throwException);
        }

        /// <summary>
        /// 以指定的默认值替换单元格null值(当单元格类型为string或数字型的时候)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataTable CellNotNullStringTypeAndNumberType(this DataTable dt, int numberDefault = 0, string strDefault = "")
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (string.IsNullOrWhiteSpace(item[col].ToString()))
                        {
                            string typeName = col.DataType.FullName.ToLower();

                            switch (typeName.GetLikeFirstElement(new string[] { "string", "int", "float", "double", "decimal" }))
                            {
                                case "string":
                                    item[col] = strDefault;
                                    break;
                                case "int":
                                    item[col] = numberDefault;
                                    break;
                                case "float":
                                    item[col] = numberDefault;
                                    break;
                                case "double":
                                    item[col] = numberDefault;
                                    break;
                                case "decimal":
                                    item[col] = numberDefault;
                                    break;
                            }
                        }
                    }
                }
            }
            return dt;
        }


        /// <summary>
        /// 为表中条件成立的数据加上指定颜色(注意:调用此方法后表中的所有列类型均转成string)
        /// </summary>
        /// <param name="dt">数据源表</param>
        /// <param name="conditionAndColor">条件和颜色的对应集合,比如dic.Add("Age=14","#FBA"),意思是当年龄=14时,以代号为#FBA的颜色显示该数据</param>
        /// <returns>加了颜色的目标表</returns>
        public static DataTable AddHTMLColorReturnNewDataTable(this DataTable dt, Dictionary<string, string> conditionAndColor, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
         {

             if (dt != null)
             {
                 DataTable dtRes = new DataTable();
                 string[] conditionCollection = { "=", "<>", "!=", ">", ">=", "<", "<=", " like ", " not like ", " is ", " is not " };
                 //根据条件筛选的结果集列表
                 Dictionary<string, DataRow[]> dicRes = new Dictionary<string, DataRow[]>();
                 Dictionary<string, string> dicColor = new Dictionary<string, string>();
                 foreach (string dicKey in conditionAndColor.Keys)
                 {
                     string colName = string.Empty;
                     #region 获取条件中的列名称

                     colName = dicKey.Split(conditionCollection, StringSplitOptions.RemoveEmptyEntries)[0];
                     if (colName.Contains('('))//如果包含,则视为用函数计算后再比较
                     {
                         //最后一个左括号的位置
                         int left = colName.LastIndexOf('(');
                         //第一个右括号的位置
                         int right = colName.IndexOf(')');
                         //取值范围,取多长才是列名称
                         int range = right - left;

                         colName = colName.Substring(left, range).Trim();

                     }
                     colName = colName.ToLower();
                     #endregion
                     try
                     {
                         dicRes.Add(colName, dt.Select(dicKey));//向结果集列表中添加筛选的结果
                         dicColor.Add(colName, conditionAndColor[dicKey]);
                     }
                     catch (Exception)
                     {
                         continue;
                     }

                 }
                 foreach (DataColumn col in dt.Columns)
                 {
                     dtRes.Columns.Add(col.ColumnName, typeof(string));//为了加颜色标记,统一存成字符串
                 }
                 foreach (DataRow row in dt.Rows)
                 {
                     DataRow drRes = dtRes.NewRow();
                     foreach (DataColumn col in dt.Columns)
                     {
                         string tmpCol = col.ColumnName.ToLower().Trim();
                         if (dicRes.Keys.Contains(tmpCol) && dicRes[tmpCol] != null)//如果此列作为筛选条件并筛选出来的结果不为空
                         {
                             DataRow[] drs = dicRes[tmpCol];
                             if (drs.ToList<DataRow>().ContainsRow(row))//如果集合中有任意一行与此行相等
                             {
                                 drRes[col.ColumnName] = row[col].ToString().AddColorForHTML(dicColor[tmpCol]);//为该列添加指定的颜色
                             }
                             else
                             {
                                 drRes[col.ColumnName] = row[col].ToString();
                             }

                         }
                         else
                         {
                             drRes[col.ColumnName] = row[col].ToString();
                         }
                     }
                     dtRes.Rows.Add(drRes);
                 }

                 dt = dtRes;
             }
             return dt;
         }, opRes, throwException);
        }

        /// <summary>
        /// 将左表(本表)与右表(rightDataTable)做LeftJoin操作,两个表中除了join字段,其它的字段不能相同
        /// </summary>
        /// <param name="dt">左表</param>
        /// <param name="joinFieldName">左表join的字段名</param>
        /// <param name="rightDataTable">右表</param>
        /// <param name="rightDataTableJoinFieldName">右表join的字段名</param>
        /// <returns>关联后的新表,目标表join的字段名不会出现在结果中</returns>
        public static DataTable LeftJoinReturnNewDataTable(this DataTable dt, string joinFieldName, DataTable rightDataTable, string rightDataTableJoinFieldName, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
         {

             if (dt != null && rightDataTable != null)
             {
                 bool flag = false;
                 foreach (DataColumn item in dt.Columns)
                 {
                     if (item.ColumnName.ToLower() == joinFieldName.ToLower()) { flag = true; break; }
                 }
                 if (!flag) { throw new Exception("源表中不存在字段" + joinFieldName); }
                 dt.PrimaryKey = new DataColumn[] { dt.Columns[joinFieldName] };
                 DataTable dtRes = dt.Copy();
                 flag = false;
                 foreach (DataColumn item in rightDataTable.Columns)
                 {
                     if (item.ColumnName.ToLower() == rightDataTableJoinFieldName.ToLower()) { flag = true; }
                     else
                     {
                         dtRes.Columns.Add(new DataColumn(item.ColumnName, item.DataType));
                     }
                 }
                 if (!flag) { throw new Exception("目标表中不存在字段" + rightDataTableJoinFieldName); }
                 rightDataTable.PrimaryKey = new DataColumn[] { rightDataTable.Columns[rightDataTableJoinFieldName] };
                 foreach (DataRow item in dtRes.Rows)
                 {
                     DataRow[] drs = rightDataTable.Select(rightDataTableJoinFieldName + "='" + item[joinFieldName] + "'");
                     if (drs != null && drs.Length == 1)
                     {
                         DataRow drTmp = drs[0];
                         foreach (DataColumn col in rightDataTable.Columns)
                         {
                             string colName = col.ColumnName.ToLower();
                             if (colName != rightDataTableJoinFieldName.ToLower())
                             {
                                 item[colName] = drTmp[colName];
                             }
                         }
                     }

                 }
                 dt = dtRes;
             }
             return dt;
         }, opRes, throwException);
        }
        /// <summary>
        /// 计算指定列集合中各单元格数据在该列总值(sum值)中所占百分比
        /// </summary>
        /// <param name="dt">源数据表</param>
        /// <param name="columnNameCollects">要计算的指定列名称集合</param>
        /// <param name="throwEx">是否抛出异常</param>
        /// <returns>会在源表基础上添加 以指定列集合中 各列名称+Rate做为计算的结果列 填充数据并返回</returns>
        public static DataTable CalcRate(this DataTable dt, List<string> columnNameCollects, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (columnNameCollects != null && columnNameCollects.Count > 0)
                    {
                        foreach (string item in columnNameCollects)
                        {
                            dt = dt.CalcRate(item, opRes, throwException);
                        }
                    }
                }
                return dt;
            }, opRes, throwException);
        }

        /// <summary>
        /// 计算指定列各单元格数据在该列总值(sum值)中所占百分比,格式如78.23等,已经将比值*100了的
        /// </summary>
        /// <param name="dt">源数据表</param>
        /// <param name="columnName">要计算的指定列名称</param>
        /// <param name="throwException">是否抛出异常</param>
        /// <returns>会在源表基础上添加以指定列名称+Rate做为计算的结果列填充数据并返回</returns>
        public static DataTable CalcRate(this DataTable dt, string columnName, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
           {
               if (dt != null && dt.Rows.Count > 0 && !string.IsNullOrWhiteSpace(columnName))
               {
                   string resColName = columnName + "Rate";
                   dt.Columns.Add(new DataColumn(resColName, typeof(decimal)));

                   if (dt.Columns.Contains(columnName))
                   {
                       decimal sumVal = decimal.Parse(dt.Compute("sum(" + columnName + ")", null).ToString());
                       foreach (DataRow row in dt.Rows)
                       {
                           decimal curVal = decimal.Parse(row[columnName].ToString());
                           row[resColName] = Math.Round(curVal / sumVal * 100, 2);
                       }
                   }

               }
               return dt;
           }, opRes, throwException);
        }

        /// <summary>
        /// 获取DataTable中某列内数据内容,以特定字符作分隔符将各单元格的数据分开后去重,不分大小写
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnName">列名称</param>
        /// <param name="opRes"></param>
        /// <param name="splitChar">分隔字符,默认为|</param>
        /// <param name="throwException"></param>
        /// <returns></returns>
        public static List<string> DistinctContent(this DataTable dt, string columnName, OPResult opRes,char splitChar='|', bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                List<string> listRes = new List<string>();
                List<string> listTmp = new List<string>();
                if (dt != null && dt.Rows.Count > 0 && !string.IsNullOrWhiteSpace(columnName))
                {
                    if (dt.Columns.Contains(columnName))
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            string tmp = item[columnName].ToString();
                            string tmpLower = tmp.ToLower();
                            string[] tmpArr = tmp.Split(splitChar);
                            string[] tmpLowerArr = tmpLower.Split(splitChar);

                            for (int i = 0; i < tmpLowerArr.Length; i++)
                            {
                                if (!listTmp.Contains(tmpLowerArr[i]))
                                {
                                    listTmp.Add(tmpLowerArr[i]);
                                    listRes.Add(tmpArr[i]);
                                }
                            }
                        }
                    }
                }
                return listRes;
            }, opRes, throwException);
        }

        /// <summary>
        /// 将一个表按指定字段拆分成多个,一般用于拆分后对某表数据去重,再与不去重的拆分表一起返回前端绑定数据的情况,目的是少查一次数据库,比如订单列表和订单明细可以join后,拆分,去重订单信息,不去重明细信息,后来发现可用dt.DefaultView.ToDataTable方法来取代
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fieldList">要拆分出来的表字段集合,一个列表项表示一个表的字段,字段间用逗号分开</param>
        /// <param name="opRes"></param>
        /// <param name="throwException"></param>
        /// <returns></returns>
        public static List<DataTable> Split(this DataTable dt, List<string> fieldList, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
           {
               if (dt != null)
               {
                   List<DataTable> listDt = new List<DataTable>();

                   try
                   {
                       foreach (string item in fieldList)
                       {
                           DataTable dtTmp = new DataTable();
                           List<string> tmpFields = item.Split(',').ToList();
                           List<string> listFieldRes = new List<string>();

                           //根据指定字段,构造拆分表的列
                           foreach (string f in tmpFields)
                           {
                               if (dt.Columns.Contains(f))//列名是不区分大小写的
                               {
                                   DataColumn tmpCol = new DataColumn(f);
                                   dtTmp.Columns.Add(tmpCol);
                                   listFieldRes.Add(f);
                               }
                               else
                               {
                                   throw new Exception("拆分DataTable时发生异常,原不中不存在指定的列" + f);
                               }
                           }

                           //填充折出表的数据
                           foreach (DataRow row in dt.Rows)
                           {
                               DataRow tmpRow = dtTmp.NewRow();

                               foreach (string col in listFieldRes)
                               {
                                   tmpRow[col] = row[col];
                               }
                               dtTmp.Rows.Add(tmpRow);
                           }

                           listDt.Add(dtTmp);

                       }

                       return listDt;
                   }
                   catch (Exception ex)
                   {
                       throw new Exception("拆分DataTable时发生异常: " + ex);
                   }
                  
               }
               return null;
           }, opRes, throwException);
        }
    }
}
