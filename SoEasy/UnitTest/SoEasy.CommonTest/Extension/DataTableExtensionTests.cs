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
    public class DataTableExtensionTests
    {
        DataTable dtDefault = new DataTable();
        public DataTableExtensionTests()
        {
            dtDefault.Columns.Add("ID", typeof(int));
            dtDefault.Columns.Add("Name");
            for (int i = 0; i < 10; i++)
            {
                DataRow dr = dtDefault.NewRow();
                dr["ID"] = i + 1;
                dr["Name"] = "name" + i;
                dtDefault.Rows.Add(dr);
            }
        }

        [TestMethod()]
        public void SelectReturnTableTest()
        {
            DataTable dtRes = dtDefault.SelectReturnTable("ID<5", null);
            Assert.AreEqual(dtRes.Rows.Count, 4);
        }

        [TestMethod()]
        public void FirstTest()
        {
            DataRow dr = dtDefault.First();
            Assert.AreEqual(dr["ID"].ToString(), "1");
        }

        [TestMethod()]
        public void IsEqualDataTableTest()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name");
            for (int i = 0; i < 10; i++)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = i + 1;
                dr["Name"] = "name" + i;
                dt.Rows.Add(dr);
            }
            Assert.AreEqual(dt.IsEqualDataTable(dtDefault), true);
            dt.Rows.RemoveAt(0);
            Assert.AreEqual(dt.IsEqualDataTable(dtDefault), false);

        }

        [TestMethod()]
        public void AddRowNumberTest()
        {
            DataTable dt = dtDefault.Copy();
            dt.AddRowNumber(null);
            Assert.AreEqual(dt.Columns.Count, dtDefault.Columns.Count + 1);
            Assert.AreEqual(1, dt.Rows[0]["Row_Number"]);
        }

        [TestMethod()]
        public void ToJsonTest()
        {
            Assert.IsTrue(dtDefault.ToJsonString().Length > 0);
        }



        [TestMethod()]
        public void RemoveRowsWhereTest()
        {
            DataTable dt = dtDefault.Copy();
            dt.RemoveRowsWhere("ID=6", null);
            Assert.IsTrue(dt.Rows.Count == 9);
        }

        [TestMethod()]
        public void UpdateRowsTest()
        {
            DataTable dt = dtDefault.Copy();
            dt.UpdateRows("ID=7", "ID=6", null);
            Assert.IsTrue(dt.Select("ID=6").Length == 0);
        }

        [TestMethod()]
        public void FormatNumberTest()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Price", typeof(decimal));
            DataRow dr = dt.NewRow();
            dr[0] = 762.493232;
            dt.Rows.Add(dr);
            dt.FormatNumber(null);
            Assert.IsTrue(((decimal)dt.Rows[0][0]) == 762.49M);
        }

        [TestMethod()]
        public void MaxTest()
        {
            DataTable dt = dtDefault.Copy();
            Assert.IsTrue(int.Parse(dt.Max("ID", null)["ID"].ToString()) == 10);
        }

        [TestMethod()]
        public void MinTest()
        {
            DataTable dt = dtDefault.Copy();
            Assert.IsTrue(int.Parse(dt.Min("ID", null)["ID"].ToString()) == 1);
        }

        [TestMethod()]
        public void AddRowsTest()
        {
            DataTable dt = dtDefault.Copy();
            dt.AddRows(dt.Select("ID>5"), null);
            Assert.IsTrue(dt.Rows.Count == 15);
        }

        [TestMethod()]
        public void CalcTest()
        {
            DataTable dt = dtDefault.Copy();
            dt.Columns.Add("C", typeof(int));
            for (int i = 0; i < 10; i++)
            {
                dt.UpdateRows("C=" + (i * 10), "ID=" + (i + 1), null);
            }
            dt.Calc("C", "ID", "ResSu", SoEasy.Common.Enums.CalcType.Subduction, null);
            Assert.IsTrue(int.Parse(dt.Rows[1]["ResSu"].ToString()) == 8);
        }

        [TestMethod()]
        public void CellNotNullStringTypeAndNumberTypeTest()
        {
            DataTable dt = dtDefault.Copy();
            dt.Columns.Add("C", typeof(int));
            for (int i = 0; i < 10; i++)
            {
                DataRow dr = dt.NewRow();
                dr.ItemArray = new object[] { null, null };
                dt.Rows.Add(dr);
            }
            dt.CellNotNullStringTypeAndNumberType();
            Assert.IsTrue(int.Parse(dt.Rows[10][0].ToString()) == 0);
        }

        [TestMethod()]
        public void AddHTMLColorForDataTest()
        {
            DataTable dt = dtDefault.Copy();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("ID=9", "RED");
            dt = dt.AddHTMLColorReturnNewDataTable(dic, null);

            Assert.IsTrue(dt.Rows[8][0].ToString().Length > 4);
        }

        [TestMethod()]
        public void LeftJoinTest()
        {
            DataTable dt = dtDefault.Copy();
            DataTable dt2 = dtDefault.Copy();
            dt2.Columns["Name"].ColumnName = "Name2";
            dt = dt.LeftJoinReturnNewDataTable("ID", dt2, "ID", null);

            Assert.IsTrue(dt.Columns.Count == 3 && dt.Rows.Count == 10);
        }

        [TestMethod()]
        public void CalcRateTest()
        {
            DataTable dt = dtDefault.Copy();
            dt.CalcRate("ID", null);

            Assert.IsTrue(decimal.Parse(dt.Rows[0]["IDRate"].ToString()) == 1.82M);
        }

        [TestMethod()]
        public void FieldToListTest()
        {
            List<string> list = dtDefault.FieldToList<string>("Name", null);
            List<int> listAge = dtDefault.FieldToList<int>("ID", null);
            Assert.IsTrue(list.Count > 0 && listAge.Count > 0);
        }

        [TestMethod()]
        public void IsNullOrEmptyTest()
        {
            DataTable dt = new DataTable();
            DataTable dtNull = null;
            
            DataTable dtData = new DataTable();
            dtData.Columns.Add("a");
            DataRow dr = dtData.NewRow();
            dr["a"] = "abc";
            dtData.Rows.Add(dr);

            Assert.IsTrue(dt.IsNullOrEmpty());
            Assert.IsTrue(dtNull.IsNullOrEmpty());
            Assert.IsFalse(dtData.IsNullOrEmpty());
        }

        [TestMethod()]
        public void SplitTest()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("SumPrice");
            dt.Columns.Add("Order_ID");
            dt.Columns.Add("Product_ID");
            dt.Columns.Add("Product_Img");

            #region 构造数据
            DataRow dr1 = dt.NewRow();
            dr1.ItemArray=new object[]{1,84.23,1,"pid1","img1"};
            dt.Rows.Add(dr1);

            DataRow dr2 = dt.NewRow();
            dr2.ItemArray = new object[] { 1, 12.43, 1, "pid2", "img2" };
            dt.Rows.Add(dr2);

            DataRow dr3 = dt.NewRow();
            dr3.ItemArray = new object[] { 1, 31.43, 1, "pid3", "img3" };
            dt.Rows.Add(dr3);

            DataRow dr4 = dt.NewRow();
            dr4.ItemArray = new object[] { 2, 91.66, 2, "pid4", "img4" };
            dt.Rows.Add(dr4);

            DataRow dr5 = dt.NewRow();
            dr5.ItemArray = new object[] { 2, 18.16, 2, "pid5", "img5" };
            dt.Rows.Add(dr5);

            #endregion

            List<string> listFields = new List<string>();
            listFields.Add("ID,SumPrice");
            listFields.Add("Order_ID,Product_ID,Product_IMG");

            List<DataTable> listDt = dt.Split(listFields, null);

            Assert.IsTrue(listDt!=null&&listDt.Count==2);
        }

    }
}
