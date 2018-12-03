using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoEasy.Model.BaseEntity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Collections;
using SoEasy.LogicTest.Model;

namespace SoEasy.Model.BaseEntity.Tests
{
    [TestClass()]
    public class ModelExtensionTests
    {
        [TestMethod()]
        public void ToDataTableTest()
        {
            ProductInfo p = new ProductInfo();
            p.Name = "IPhone";
            p.Price = 123;
            DataTable dt = p.ToDataTable();
            Assert.IsTrue(dt.Rows.Count==1);
        }

        
        [TestMethod()]
        public void UpdateValidTest()
        {
            PersonInfo pUpdate = new PersonInfo();
            pUpdate.Name = "FD";
            pUpdate.Id = pUpdate.GetGuid();
            string s = "";
            Assert.IsTrue(pUpdate.UpdateValid(out s) == true);
            string updateValidMsg = "";
            Assert.IsTrue(pUpdate.UpdateValid(out updateValidMsg) == true && updateValidMsg.Length == 0);

            pUpdate.Name = "faflaksdfoqiweeeeDasdfsdsss";

            Assert.IsTrue(pUpdate.UpdateValid(out s) == false);
            Assert.IsTrue(pUpdate.UpdateValid(out updateValidMsg) == false && updateValidMsg.Length > 0);
        }

         

        [TestMethod()]
        public void InsertValidTest()
        {
            PersonInfo pInsert = new PersonInfo();
            pInsert.Age = 2;
            pInsert.Name = "FD";
            string errMsg="";

            Assert.IsTrue(pInsert.InsertValid(out errMsg) == false);
            string insertValidMsg = "";
            Assert.IsTrue(pInsert.InsertValid(out insertValidMsg) == false && insertValidMsg.Length > 0);

            pInsert.Id = pInsert.GetGuid();

            Assert.IsTrue(pInsert.InsertValid(out errMsg) == true);
            Assert.IsTrue(pInsert.InsertValid(out insertValidMsg) == true && insertValidMsg.Length == 0);

        }
      
    }
}
