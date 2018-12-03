using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoEasy.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using SoEasy.LogicTest.Model;
namespace SoEasy.Common.Tests
{
    [TestClass()]
    public class SerializeHelperTests
    {

        DataTable dt = new DataTable();
        ProductInfo p = new ProductInfo();
        public SerializeHelperTests()
        {
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            for (int i = 0; i < 10; i++)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = i + 1;
                dr["Name"] = "name" + i;
                dt.Rows.Add(dr);
            }

            p.Op_Time = DateTime.Now;
            p.Name = "IPhone";
            p.Price = 5288.23M;
        }

        [TestMethod()]
        public void SerializeObjectTest()
        {
            Assert.IsTrue(SerializeHelper.SerializeObject(dt,null).Length > 0);
            Assert.IsTrue(SerializeHelper.SerializeObject(p,null).Length > 0);

        }

        [TestMethod()]
        public void DesrializeTest()
        {

            string dtString=SerializeHelper.SerializeObject(dt,null);
            string pString = SerializeHelper.SerializeObject(p,null);

            DataTable dtRes = SerializeHelper.Desrialize<DataTable>(dtString,null);
            ProductInfo pRes = SerializeHelper.Desrialize<ProductInfo>(pString,null);


            Assert.IsTrue(dtRes.Rows.Count>0);
            Assert.IsTrue(pRes.Name=="IPhone");
        }
    }
}
