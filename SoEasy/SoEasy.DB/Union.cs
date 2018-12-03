using SoEasy.Model.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoEasy.DB
{
    /// <summary>
    /// 合并两个查询的结果集,对应数据库的Union/Union All 操作
    /// </summary>
    public class Union
    {
        /// <summary>
        /// 查询实体A
        /// </summary>
        public Parent AModel { get; set; }
        /// <summary>
        /// 实体A要显示的字段,多个字段之间用逗号分开
        /// </summary>
        public string AShowFields { get; set; }
        /// <summary>
        /// 查询实体B
        /// </summary>
        public Parent BModel { get; set; }
        /// <summary>
        /// 实体B要显示的字段,多个字段之间用逗号分开
        /// </summary>
        public string BShowFields { get; set; }


        /// <summary>
        /// 合并类型
        /// </summary>
        public UnionType UnionType { get; set; }

        public Union()
        {
            UnionType = UnionType.UnionAll;
        }

    }

    public enum UnionType
    {
        Union, UnionAll
    }

    public static class UnionEx
    {
        public static string GetString(this UnionType unionType)
        {
            switch (unionType)
            {
                case UnionType.Union:
                    return " union ";
                case UnionType.UnionAll:
                    return " union all ";
                default:
                    return " union ";
            }
        }
    }
}
