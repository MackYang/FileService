using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace SoEasy.Common
{
    public class ControlHelper
    {

        /// <summary>
        /// 绑定List控件(DropDownList,CheckBoxList,RadioButtonList...)
        /// </summary>
        /// <param name="dt">绑定的数据源</param>
        /// <param name="text">控件绑定的显示内容</param>
        /// <param name="value">控件绑定的值内容</param>
        /// <param name="control">要绑定的List控件</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <param name="isAddBlank">是否添加空白项</param>

        public static bool BindListControl(DataTable dt, string text, string value, ListControl control, bool isAddBlank,OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
           {
               bool flag = false;
               try
               {
                   string oldValue = control.SelectedValue;                    //先记录之前的选择项                
                   control.Items.Clear();                                      //清空后重新绑定
                   if (isAddBlank)
                   {
                       control.Items.Add(new ListItem("--请选择--", "-1"));
                   }
                   if (dt == null || dt.Rows.Count <= 0)
                   {
                       return flag;
                   }
                   foreach (DataRow row in dt.Rows)
                   {
                       ListItem item = new ListItem(row[text].ToString(), row[value].ToString());
                       if (!string.IsNullOrWhiteSpace(oldValue) && !oldValue.Equals("0") && oldValue.Equals(row[value].ToString()))
                       {
                           item.Selected = true;//绑定时重新选中原来的选择项                               
                       }
                       control.Items.Add(item);
                   }
                   flag = true;
               }
               catch (Exception ex)
               {
                   throw new Exception("绑定控件" + control.ID + "的数据源时发生异常:" + ex);
               }
               return flag;
           }, opRes,throwException);
        }

        /// <summary>
        /// 绑定GridView控件数据
        /// </summary>
        /// <param name="gridView">要绑定的数据控件</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <param name="dt">绑定的数据源</param>
        public static bool BindGridViewData(GridView gridView, DataTable dt,OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                bool flag = false;
                if (dt == null)
                {
                    return flag;
                }
                try
                {
                    if (dt.Rows.Count == 0)
                    {
                        dt.Rows.Add(dt.NewRow());
                        gridView.DataSourceID = String.Empty;
                        gridView.DataSource = dt;
                        gridView.DataBind();
                        gridView.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                        gridView.Rows[0].Cells[0].Text = string.IsNullOrWhiteSpace(gridView.EmptyDataText) ? "暂无数据显示!" : gridView.EmptyDataText;
                        gridView.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        while (gridView.Rows[0].Cells.Count > 1)
                        {
                            gridView.Rows[0].Cells.RemoveAt(1);
                        }
                    }
                    else
                    {
                        gridView.DataSourceID = String.Empty;
                        gridView.DataSource = dt;
                        gridView.DataBind();
                    }
                    flag = true;
                }
                catch (Exception ex)
                {
                    throw new Exception("绑定" + gridView.ID + "控件的数据源时发生异常:" + ex);

                }
                return flag;
            }, opRes,throwException);
        }
    }
}
