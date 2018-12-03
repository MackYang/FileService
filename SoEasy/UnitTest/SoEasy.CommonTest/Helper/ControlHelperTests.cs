using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoEasy.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.UI.WebControls;
using System.Data;
namespace SoEasy.Common.Tests
{
    [TestClass()]
    public class ControlHelperTests
    {
        DataTable dtDefault = new DataTable();
        public ControlHelperTests()
        {
            dtDefault.Columns.Add("ID");
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
        public void BindListControlTest()
        {
            DropDownList list = new DropDownList();
            ControlHelper.BindListControl(dtDefault, "Name", "ID", list, true,null);

            Assert.IsTrue(list.Items[0].Text != null);
        }

        [TestMethod()]
        public void BindGridViewDataTest()
        {
            GridView gd = new GridView();
            ControlHelper.BindGridViewData(gd, dtDefault,null);

            Assert.IsTrue(((DataTable)gd.DataSource).Rows[0]["ID"].ToString() == "1");
        }
    }
}
