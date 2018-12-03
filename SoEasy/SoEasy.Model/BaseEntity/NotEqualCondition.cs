
using System;
using System.Collections.Generic;
namespace SoEasy.Model.BaseEntity
{
    /// <summary>
    /// 非=号的查询限制条件
    /// </summary>
    [Serializable]
    public class NotEqualCondition
    {
        List<Args> _argsArr = new List<Args>();

        /// <summary>
        /// 限制条件SQL,不以where and之类的开头,在:后写参数名称(注意!参数标示统一用冒号:做前缀,将在底层统一替换)
        /// </summary>
        public string ConditionSQL { get; set; }
        /// <summary>
        /// 参数列表
        /// </summary>
        public List<Args> ArgsArr { get { return _argsArr; } set { _argsArr = value; } }

        /// <summary>
        /// 添加一个参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        public void AddArgs(string name, object value)
        {
            _argsArr.Add(new Args(name, value));
        }

        /// <summary>
        /// 添加条件,用于需要动态拼条件的场景
        /// </summary>
        /// <param name="conditionSQL">条件SQL,不要带where</param>
        /// <param name="argsName">参数名称</param>
        /// <param name="argsValue">参数值</param>
        public void AddCondition(string conditionSQL, string argsName, object argsValue)
        {
            if (!string.IsNullOrWhiteSpace(conditionSQL))
            {
                if (string.IsNullOrWhiteSpace(ConditionSQL))
                {
                    ConditionSQL = conditionSQL;
                }
                else
                {
                    ConditionSQL = ConditionSQL + " and " + conditionSQL;
                }
                if (!string.IsNullOrWhiteSpace(argsName))
                {
                    AddArgs(argsName, argsValue);
                }
            }
          
        }
    }

    /// <summary>
    /// 参数类
    /// </summary>
    [Serializable]
    public class Args
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public object Value { get; set; }

        public Args() { }
        /// <summary>
        /// 参数类构造器
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        public Args(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
