using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using SoEasy.Model.BaseEntity;
using SoEasy.Common;
using System.Data;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.OracleClient;
using SoEasy.DB.Interface;

namespace SoEasy.DB.DAO
{
    public class OracleDAO : BaseDAO, IBaseDAO
    {
        public override string GetDBParmString
        {
            get { return ":"; }
        }

        public override string GetRandomString
        {
            get { return "dbms_random.random"; }
        }


        public override DbParameter GetDBParam(string paramName, object paramValue)
        {
            return new OracleParameter(paramName, paramValue);
        }

        /// <summary>
        /// 获取分页SQL
        /// </summary>
        /// <param name="pager">分页对象</param>
        /// <param name="innerSQL">内部SQL,基于此SQL的查询结果做分页</param>
        /// <returns></returns>
        public override string GetPageSQL(Pager pager, string innerSQL)
        {
            pager.ValidArgs();
            int pageBegin = (pager.PageIndex - 1) * pager.PageSize + 1;
            int pageEnd = pager.PageIndex * pager.PageSize;
            string sqlPage = string.Format(@"
                SELECT T.* FROM
                (
                    SELECT T.*, ROWNUM ROW_NUM
                    FROM ({0}) T {1}
                ) T {2}",
                  innerSQL, " WHERE ROWNUM <= " + pageEnd, " WHERE ROW_NUM >= " + pageBegin);
            return sqlPage;
        }

        /// <summary>
        /// 拼接SQL字符串
        /// </summary>
        /// <param name="strAs">要接招的多个字符串</param>
        /// <returns>拼接后的字符串</returns>
        public override string ConcatenateString(params string[] strAs)
        {
            StringBuilder sbSQL = new StringBuilder();
            foreach (string item in strAs)
            {
                sbSQL.Append(item + "||");
            }
            sbSQL.Remove(sbSQL.Length - 2, 2);
            return " "+sbSQL.ToString()+" ";
        }
    }
}
