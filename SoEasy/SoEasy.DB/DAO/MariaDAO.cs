using MySql.Data.MySqlClient;
using SoEasy.Common;
using SoEasy.DB.Interface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoEasy.DB.DAO
{
    public class MariaDAO : BaseDAO, IBaseDAO
    {
        public override string GetDBParmString
        {
            get { return "?"; }
        }

        public override string GetRandomString
        {
            get { return "rand()"; }
        }


        public override DbParameter GetDBParam(string paramName, object paramValue)
        {
            
            return new MySqlParameter(paramName, paramValue);
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
            int pageBegin = (pager.PageIndex - 1) * pager.PageSize;
            string sqlPage = string.Format(@"
                {0} limit {1},{2}", innerSQL, pageBegin, pager.PageSize);
            return sqlPage;
        }

        /// <summary>
        /// 拼接SQL字符串
        /// </summary>
        /// <param name="strAs">要接招的多个字符串</param>
        /// <returns>拼接后的字符串</returns>
        public override string ConcatenateString(params string[] strAs)
        {
            StringBuilder sbSQL = new StringBuilder(" Concat(");
            foreach (string item in strAs)
            {
                sbSQL.Append(item + ",");
            }
            sbSQL.Remove(sbSQL.Length - 1, 1);
            sbSQL.Append(") ");
            return sbSQL.ToString();
        }
    }
}
