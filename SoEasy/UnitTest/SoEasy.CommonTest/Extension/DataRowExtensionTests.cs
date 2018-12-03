using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Data.Tests
{
    [TestClass()]
    public class DataRowExtensionTests
    {
        [TestMethod()]
        public void FirstTest()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            List<DataRow> drList = new List<DataRow>();
            for (int i = 0; i < 10; i++)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = i+1;
                dr["Name"] = "name"+i;
                drList.Add(dr);
            }
            DataRow drRes=drList.ToArray().First();

            Assert.IsNotNull(drRes);
        }
       
    }
}
