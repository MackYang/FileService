using SoEasy.Common;
using SoEasy.Model.BaseEntity;
using System;

namespace SoEasy.DB
{
    /// <summary>
    /// 用于设置Join查询的类
    /// </summary>
    public class Join
    {
        public Parent LeftModel { get; set; }
        /// <summary>
        /// 左表要显示的列,逗号分开,输入/表示不返回任何列
        /// </summary>
        public string LeftShowFields { get; set; }
        /// <summary>
        /// 左表的关联字段,用逗号分开
        /// </summary>
        public string LeftOnFields { get; set; }
        /// <summary>
        /// 左表内部查询出来供关联的字段,如果不赋值表示查所有字段
        /// </summary>
        public string LeftInnerQueryFields { get; set; }


        public Parent RightModel { get; set; }
        /// <summary>
        /// 右表要显示的列,逗号分开,输入/表示不返回任何列
        /// </summary>
        public string RightShowFields { get; set; }
        /// <summary>
        /// 右表的关联字段,用逗号分开
        /// </summary>
        public string RightOnFields { get; set; }

        /// <summary>
        /// 右表内部查询出来供关联的字段,如果不赋值表示查所有字段
        /// </summary>
        public string RightInnerQueryFields { get; set; }

        /// <summary>
        /// 连接类型
        /// </summary>
        public JoinType JoinType { get; set; }

        public Join()
        {
            JoinType = JoinType.Join;
        }

    }

    public enum JoinType
    {
        Join, LeftJoin, RightJoin
    }

    public static class JoinEx
    {
        public static string GetString(this JoinType joinType)
        {
            switch (joinType)
            {
                case JoinType.Join:
                    return " Join ";
                case JoinType.LeftJoin:
                    return " Left Join ";
                case JoinType.RightJoin:
                    return " Right Join ";
                default:
                    return " Join ";
            }
        }
    }
}
