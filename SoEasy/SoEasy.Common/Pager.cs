using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoEasy.Common
{
    /// <summary>
    /// 分页辅助类
    /// </summary>
    public class Pager
    {
        /// <summary>
        /// 每页面显示多少条数据，默认20
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 当前页面索引
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 总行数
        /// </summary>
        public int RowCount { get; set; }

        public Pager()
        {
            PageIndex = 1;
            PageSize = 20;
        }
        public Pager(int pageSize)
        {
            PageIndex = 1;
            PageSize = pageSize;
        }

        public Pager(int pageIndex,int pageSize)
        {
            PageIndex =pageIndex;
            PageSize = pageSize;
        }

        /// <summary>
        /// 验证参数,如果不合法,则自动重将PageIndex设置初始值
        /// </summary>
        public void ValidArgs()
        {
            int pageBegin = (PageIndex - 1) * PageSize;
            if (PageIndex < 1 || pageBegin < 1 || pageBegin > int.MaxValue)
            {
                PageIndex = 1;
            }
            if (PageSize < 1 || PageSize > 100)
            {
                PageSize = 20;
            }
        }
    }
}
