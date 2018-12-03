using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoEasy.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoEasy.Common;
using SoEasy.DB;
using SoEasy.LogicTest;
using SoEasy.Model.BaseEntity;
using System.Data;
using SoEasy.LogicTest.Model;
using SoEasy.Init;
namespace SoEasy.Logic.Tests
{
    [TestClass()]
    public class CommonBLTests
    {
        CommonBL bl = null;
        TestBL testBL = null;
        public CommonBLTests()
        {
            InitConfigData.InitSettings("Set.config", null);
            bl = CommonBL.CreateInstance();
            testBL = TestBL.CreateInstance();

        }

        [TestMethod()]
        public void SelectUnionTest()
        {

            PersonInfo p = new PersonInfo();
            p.Age = 10;

            OrderInfo o = new OrderInfo();
            o.Person_Id = "tran_per_id_1";

            Union u = new Union();
            u.AModel = p;
            u.AShowFields = "Age";
            u.BModel = o;
            u.BShowFields = "Amount";

            DataTable dt = bl.SelectUnion(u, null);

            Assert.IsTrue(dt != null && dt.Rows.Count > 0);


        }

        [TestMethod()]
        public void SelectUnionRandomTest()
        {
            PersonInfo p = new PersonInfo();
            p.Age = 10;

            OrderInfo o = new OrderInfo();
            o.Person_Id = "tran_per_id_1";

            Union u = new Union();
            u.AModel = p;
            u.AShowFields = "Age";
            u.BModel = o;
            u.BShowFields = "Amount";

            DataTable dt = bl.SelectUnionRandom(u, null);

            Assert.IsTrue(dt != null && dt.Rows.Count > 0);
        }

        [TestMethod()]
        public void SelectUnionPageTest()
        {
            PersonInfo p = new PersonInfo();
            p.Age = 10;

            OrderInfo o = new OrderInfo();
            o.Person_Id = "tran_per_id_1";

            Union u = new Union();
            u.AModel = p;
            u.AShowFields = "Age";
            u.BModel = o;
            u.BShowFields = "Amount";

            Pager page = new Pager(1, 2);

            DataTable dt = bl.SelectUnionPage(u, page, null);

            Assert.IsTrue(dt != null && dt.Rows.Count > 0);
        }

        [TestMethod()]
        public void SelectUnionPageRandomTest()
        {
            PersonInfo p = new PersonInfo();
            p.Age = 10;

            OrderInfo o = new OrderInfo();
            o.Person_Id = "tran_per_id_1";

            Union u = new Union();
            u.AModel = p;
            u.AShowFields = "Age";
            u.BModel = o;
            u.BShowFields = "Amount";

            Pager page = new Pager(1, 2);

            DataTable dt = bl.SelectUnionPageRandom(u, page, null);

            Assert.IsTrue(dt != null && dt.Rows.Count > 0);
        }


        public T SelectFirst<T>(string orderByColumnName = "Create_Time", string orderMode = " Desc") where T : Parent, new()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("NAME");
            dt.Columns.Add("AGE");
            dt.Columns.Add("OP_TIME");
            DataRow dr = dt.NewRow();
            dr.ItemArray = new object[] { Guid.NewGuid().ToString(), "我是杨家勇", 25, DateTime.Now };
            dt.Rows.Add(dr);
            return dt.GetTModel<T>(null);
        }


        [TestMethod()]
        public void ATest()
        {
            PersonInfo p = SelectFirst<PersonInfo>("OP_TIME");
            Assert.IsTrue(p.Age == 25);

        }

        [TestMethod()]
        public void SelectFirstTest()
        {
            //TransInsertTest();
            PersonInfo p = new PersonInfo();
            p.Name = "tran_per_name_2";
            PersonInfo resP = bl.SelectFirst(p, null);
            Assert.IsTrue(resP.Name == "tran_per_name_2");
        }

        [TestMethod()]
        public void TransInsertTest()
        {
            List<Parent> list = new List<Parent>();
            for (int i = 0; i < 10; i++)
            {
                PersonInfo per = new PersonInfo();
                per.Name = "tran_per_name_" + (i + 1);
                per.Age = i + 1;
                per.Id = per.GetGuid();

                ProductInfo pro = new ProductInfo();
                pro.Id = per.GetGuid();
                pro.Detal_Info = pro.GetGuid();
                pro.Name = "tran_pro_name_" + (i + 1);
                pro.Price = (decimal)new Random().NextDouble() * 100;
                pro.Store_Num = new Random().Next(15, 150);

                list.Add(per);
                list.Add(pro);
            }
            Assert.IsTrue(bl.TransInsert(list, null));
        }

        [TestMethod()]
        public void TransUpdateTest()
        {
            NotEqualCondition ncePro = new NotEqualCondition();
            ncePro.ConditionSQL = "price>:price and id>:id";
            ncePro.AddArgs("price", 100);
            ncePro.AddArgs("id", "tran_pro_id_8");

            List<Parent> list = new List<Parent>();
            for (int i = 100; i < 130; i++)
            {
                PersonInfo per = new PersonInfo();
                per.Id = "tran_per_id_" + (i + 1);
                per.Name = "tran_per_name_" + (i + 1);
                per.Age = i + 1;

                ProductInfo pro = new ProductInfo();
                pro.Id = "tran_pro_id_" + (i + 1);
                pro.Detal_Info = Guid.NewGuid().ToString();
                pro.Name = "tran_pro_name_" + (i + 1);
                pro.Price = (decimal)new Random().NextDouble() * 100;
                pro.Store_Num = new Random().Next(15, 150);
                pro.OtherCondition = ncePro;
                list.Add(per);
                list.Add(pro);
            }

            Assert.IsTrue(bl.TransUpdate(list, null));
        }

        [TestMethod()]
        public void TransDeleteTest()
        {
            NotEqualCondition ncePro = new NotEqualCondition();
            ncePro.ConditionSQL = "price>:price and id>:id";
            ncePro.AddArgs("price", 100);
            ncePro.AddArgs("id", "tran_pro_id_50");

            List<Parent> list = new List<Parent>();
            for (int i = 100; i < 105; i++)
            {
                PersonInfo per = new PersonInfo();
                per.Id = "tran_per_id_" + (i + 1);
                per.Name = "tran_per_name_" + (i + 1);
                per.Age = i + 1;

                ProductInfo pro = new ProductInfo();
                pro.Id = "tran_pro_id_" + (i + 1);
                pro.Name = "tran_pro_name_" + (i + 1);
                pro.OtherCondition = ncePro;
                list.Add(per);
                list.Add(pro);
            }
            Assert.IsTrue(bl.TransDelete(list, null));
        }

        [TestMethod()]
        public void TransExecuteNonQueryTest()
        {
            Assert.IsTrue(testBL.TransExecuteNonQuery());
        }

        [TestMethod()]
        public void GetTableStructByTableNameTest()
        {
            Assert.IsNotNull(bl.GetTableStructByTableName("Person_Info", null));
        }

        [TestMethod()]
        public void CountTest()
        {
            Assert.IsTrue(bl.Count(new PersonInfo(), null) != -1);
        }

        [TestMethod()]
        public void InsertTest()
        {
            PersonInfo p = new PersonInfo();
            p.Name = "Atame/阿米";
            p.Id = p.GetGuid();
            p.Age = 17;

            Assert.IsTrue(bl.Insert(p, null));
        }

        [TestMethod()]
        public void FailInsertTest()
        {
            PersonInfo p = new PersonInfo();
            p.Age = 9;
            Assert.IsFalse(bl.Insert(p, null));
        }

        [TestMethod()]
        public void DeleteTest()
        {
            PersonInfo p = new PersonInfo();
            p.Age = 100;
            Assert.IsTrue(bl.Delete(p, null));
        }

        [TestMethod()]
        public void UpdateTest()
        {
            PersonInfo p = new PersonInfo();
            p.Age = 16;
            p.Id = "123";
            p.OtherCondition = new Model.BaseEntity.NotEqualCondition { ConditionSQL = "Age>:oldAge", ArgsArr = new List<Args> { new Args("oldAge", 16) } };
            Assert.IsTrue(bl.Update(p, null));
        }

        [TestMethod()]
        public void FailUpdateTest()
        {
            PersonInfo p = new PersonInfo();
            p.Name = null;
            p.OtherCondition = new Model.BaseEntity.NotEqualCondition { ConditionSQL = "Age>:age", ArgsArr = new List<Args> { new Args("age", 16) } };
            Assert.IsTrue(bl.Update(p, null));

        }

        [TestMethod()]
        public void SelectTest()
        {
            Assert.IsNotNull(bl.Select(new PersonInfo(), "Name,Age", null));
        }

        [TestMethod()]
        public void SelectPageTest()
        {
            Pager page = new Pager();
            Assert.IsNotNull(bl.SelectPage(new PersonInfo(), page, null, null));
        }

        [TestMethod()]
        public void SelectRandomTest()
        {
            Assert.IsNotNull(bl.SelectRandom(new PersonInfo(), null, null));
        }

        [TestMethod()]
        public void SelectJoinTest()
        {
            Join join = new Join();
            join.LeftModel = new OrderInfo();
            join.LeftOnFields = "Person_ID";

            join.RightModel = new PersonInfo();
            join.RightShowFields = "Name,Age";
            join.RightOnFields = "ID";

            Assert.IsNotNull(bl.SelectJoin(join, null));
        }

        [TestMethod()]
        public void SelectJoinPageTest()
        {
            Join join = new Join();
            join.LeftModel = new OrderInfo();
            join.LeftOnFields = "Person_ID";

            join.RightModel = new PersonInfo();
            join.RightShowFields = "Name,Age";
            join.RightOnFields = "ID";

            Pager page = new Pager();
            Assert.IsNotNull(bl.SelectJoinPage(join, page, null));
        }

        [TestMethod()]
        public void SelectJoinRandomTest()
        {
            Join join = new Join();
            join.LeftModel = new OrderInfo();
            join.LeftOnFields = "Person_ID";

            join.RightModel = new PersonInfo();
            join.RightShowFields = "Name,Age";
            join.RightOnFields = "ID";

            Assert.IsNotNull(bl.SelectJoinRandom(join, null));
        }

        [TestMethod()]
        public void SelectJoinPageRandomTest()
        {

            Join join = new Join();
            join.LeftModel = new OrderInfo();
            join.LeftOnFields = "Person_ID";

            join.RightModel = new PersonInfo();
            join.RightShowFields = "Name,Age";
            join.RightOnFields = "ID";

            Pager page = new Pager();

            Assert.IsNotNull(bl.SelectJoinPageRandom(join, page, null));
        }

        [TestMethod()]
        public void ExecuteNonQueryTest()
        {
            Assert.IsTrue(testBL.ExecuteNonQuery());
        }

        [TestMethod()]
        public void ExecuteSingleValueTest()
        {
            Assert.IsNotNull(testBL.ExecuteSingleValue());
        }

        [TestMethod()]
        public void ExecuteQueryAsDataSetTest()
        {
            Assert.IsNotNull(testBL.ExecuteQueryAsDataSet());
        }

        [TestMethod()]
        public void ExecuteQueryAsDataTableTest()
        {
            Assert.IsNotNull(testBL.ExecuteQueryAsDataTable());
        }


        [TestMethod()]
        public void TransExecuteModelsTest()
        {
            PersonInfo pInsert = new PersonInfo();
            pInsert.Name = "Atame/阿米";
            pInsert.Id = pInsert.GetGuid();
            pInsert.Age = 17;

            PersonInfo pUpdate = new PersonInfo();
            pUpdate.Age = 16;
            pUpdate.OtherCondition = new Model.BaseEntity.NotEqualCondition { ConditionSQL = "Age>:oldAge", ArgsArr = new List<Args> { new Args("oldAge", 17) } };

            PersonInfo pDelete = new PersonInfo();
            pDelete.Age = 100;

            Dictionary<Parent, Enums.DbOptionType> dic = new Dictionary<Parent, Enums.DbOptionType>();
            dic.Add(pInsert, Enums.DbOptionType.Insert);
            dic.Add(pUpdate, Enums.DbOptionType.Update);
            dic.Add(pDelete, Enums.DbOptionType.Delete);

            Assert.IsTrue(bl.TransExecuteModels(dic, null));

            PersonInfo p = new PersonInfo();
            long count = bl.Count(p, null);


            pInsert.Id = pInsert.GetGuid();
            pUpdate.Age = 9999;

            Assert.IsFalse(bl.TransExecuteModels(dic, null));

            long c2 = bl.Count(p, null);

            Assert.IsTrue(count == c2);

        }



    }
}
