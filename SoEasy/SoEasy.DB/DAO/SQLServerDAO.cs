using SoEasy.DB.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoEasy.DB.DAO
{
    public class SQLServerDAO : BaseDAO, IBaseDAO
    {
        public override string GetDBParmString
        {
            get { return "@"; }
        }

        public override string GetRandomString
        {
            get { throw new NotImplementedException(); }
        }
 

        public override System.Data.Common.DbParameter GetDBParam(string paramName, object paramValue)
        {
            return new SqlParameter(paramName, paramValue);
        }

        public override string GetPageSQL(Common.Pager pager, string innerSQL)
        {
            throw new NotImplementedException();
        }

        public override string ConcatenateString(params string[] strAs)
        {
            throw new NotImplementedException();
        }
    }
}
