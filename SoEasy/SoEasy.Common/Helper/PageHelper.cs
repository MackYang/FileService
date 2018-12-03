using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoEasy.Common
{
    public class PageHelper
    {
        /// <summary>
        /// 根据传入的参数：当前页，总页数，翻页按钮总数，来计算在页面上应该出现的翻页按钮的起始和结尾区间。
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="btnCount">页面上需要显示的翻页按钮总数</param>
        /// <param name="pageStart">计算出来的翻页按钮起始位置</param>
        /// <param name="pageEnd">计算出来的翻页按钮结束位置</param>
        private static void CalcPaging(int pageIndex, int pageCount, int btnCount, out int pageStart, out int pageEnd)
        {
            // 当总页数小于翻页按钮总数时，返回整个闭合区间。
            if (pageCount <= btnCount)
            {
                pageStart = 1;
                pageEnd = pageCount;
                return;
            }
            int half = btnCount / 2;
            int iOperator = btnCount % 2;
            if (iOperator == 0)
            {
                pageStart = (pageIndex - half < 1 ? 1 : pageIndex - half);
                pageEnd = (pageIndex + half - 1 > pageCount ? pageCount : pageIndex + half - 1);
            }
            else
            {
                pageStart = (pageIndex - half < 1 ? 1 : pageIndex - half);
                pageEnd = (pageIndex + half > pageCount ? pageCount : pageIndex + half);
            }
            int section = pageEnd - pageStart;
            if (section == btnCount - 1)
            {
                return;
            }
            else
            {
                if (pageStart == 1)
                {
                    pageEnd = pageEnd + (btnCount - 1 - section);
                }
                else if (pageEnd == pageCount)
                {
                    pageStart = pageStart - (btnCount - 1 - section);
                }
            }


            pageStart = (pageStart < 1 ? 1 : pageStart);
            pageEnd = (pageEnd > pageCount ? pageCount : pageEnd);
        }

        /// <summary>
        /// 获取翻页链接
        /// </summary>
        /// <param name="linkTo">到什么页面</param>
        /// <param name="linkCount">希望每页最多出现几个翻页链接</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="rowCount">总数据行数</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>翻页链接</returns>
        public static string GetPagesLink(string linkTo, int linkCount, int pageIndex, int rowCount, int pageSize)
        {
            StringBuilder sb = new StringBuilder();
            int pageStart, pageEnd, maxPage = (rowCount - 1) / pageSize + 1;
            CalcPaging(pageIndex, maxPage, linkCount, out pageStart, out pageEnd);
            if (pageStart != pageEnd)
            {
                if (pageStart != 1)
                {
                    sb.Append("<a href='" + linkTo + "?pageIndex=" + 1 + "'><strong>首页</strong></a>&nbsp;&nbsp;&nbsp;");
                }
                for (; pageStart <= pageEnd; pageStart++)
                {
                    if (pageStart != pageIndex)
                    {
                        sb.Append("<a href='" + linkTo + "?pageIndex=" + pageStart + "'><strong>第" + pageStart + "页</strong></a>&nbsp;&nbsp;&nbsp;");
                    }
                    else
                    {
                        sb.Append("<strong><font style='background-color:Blue;' >第" + pageStart + "页</font></strong>&nbsp;&nbsp;&nbsp;");
                    }

                }
                if (pageEnd != maxPage)
                {
                    sb.Append("<a href='" + linkTo + "?pageIndex=" + maxPage + "'><strong>尾页</strong></a>&nbsp;&nbsp;&nbsp;");
                }
                return "</br></br><div style='width:100%;'><table style='width:100%;'><tr><td colspan='3' align='center'>" + sb.ToString() + "</td></tr></table>";
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取翻页链接方法
        /// </summary>
        /// <param name="jsMethodName">点翻页时调用哪个js方法,不要写括号</param>
        /// <param name="linkCount">希望每页最多出现几个翻页链接</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="rowCount">总数据行数</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>翻页链接方法</returns>
        public static string GetPagesAction(string jsMethodName, int linkCount, int pageIndex, int rowCount, int pageSize)
        {
            StringBuilder sbRes = new StringBuilder("<ul>");
            sbRes.AppendLine("{0}");
            sbRes.AppendLine("</ul>");

            StringBuilder sb = new StringBuilder();
            int pageStart, pageEnd, maxPage = (rowCount - 1) / pageSize + 1;
            CalcPaging(pageIndex, maxPage, linkCount, out pageStart, out pageEnd);
            if (pageStart != pageEnd)
            {
                if (pageStart != 1)
                {
                    sb.AppendLine("<li><a href='javascript:void(0);' onclick='" + jsMethodName + "(" + 1 + ");'>第一页</a></li>");
                }
                for (; pageStart <= pageEnd; pageStart++)
                {
                    if (pageStart != pageIndex)
                    {
                        sb.AppendLine("<li><a href='javascript:void(0);' onclick='" + jsMethodName + "(" + pageStart + ");'>&nbsp;" + pageStart + "&nbsp;</a></li>");
                    }
                    else
                    {
                        sb.AppendLine("<li>&nbsp;" + pageStart + "&nbsp;</li>");
                    }

                }
                if (pageEnd != maxPage)
                {
                    sb.AppendLine("<li><a href='javascript:void(0);' onclick='" + jsMethodName + "(" + maxPage + ");'>最后一页</a></li>");
                }
                
            }
            return sbRes.Replace("{0}", sb.ToString() + "<b>&nbsp;共" + rowCount + "条数据&nbsp;</b>").ToString();
        }
    }
}
