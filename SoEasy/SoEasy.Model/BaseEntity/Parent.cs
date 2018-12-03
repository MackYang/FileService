using System;
using System.Collections;
using SoEasy.Common;
using System.Data;
using System.Collections.Generic;

namespace SoEasy.Model.BaseEntity
{
    /// <summary>
    /// 实体父类
    /// </summary>
    [Serializable]
    public abstract class Parent
    {
        /// <summary>
        /// 保存所有数据表字段与实体属性的映射关系
        /// </summary>
        private List<Hashtable> lst = new List<Hashtable>();

        /// <summary>
        /// 保存所有数据表字段与实体属性的映射关系--代码优化所添加,可以快速判断某映射是否存在
        /// </summary>
        private Hashtable htFields = new Hashtable();


        /// <summary>
        /// 设定字段与属性的映射关系
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="value">字段值</param>
        protected void SetFieldMapping(string fieldName, object value)
        {
            if (!htFields.ContainsKey(fieldName))
            {
                Hashtable ht = new Hashtable();
                ht.Add(Constants.STR_KEY, fieldName);
                ht.Add(Constants.STR_VALUE, value);
                htFields.Add(fieldName, ht);
                lst.Add(ht);

            }
            else
            {
                Hashtable ht = htFields[fieldName] as Hashtable;
                ht[Constants.STR_VALUE] = value;//Hashtable 是引用类型,所以lst中的值也会随之改变
            }
        }

        /// <summary>
        /// 计算已设定值的字段数
        /// </summary>
        /// <returns></returns>
        public int CountFields()
        {
            return lst.Count;
        }

        /// <summary>
        /// 得到指定位置的列名
        /// </summary>
        /// <param name="index">保存列的索引</param>
        /// <returns></returns>
        public string ColumnName(int index)
        {
            string name = "";
            if (lst.Count > index)
            {
                Hashtable ht = lst[index] as Hashtable;
                name = ht[Constants.STR_KEY].ToString();
            }
            return name;
        }

        /// <summary>
        /// 判断某字段是否设置了值
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        public bool HasValue(string fieldName)
        {
            Hashtable tmp = htFields[fieldName] as Hashtable;
            return tmp != null;
        }

        /// <summary>
        /// 得到指定某列处保存的值
        /// </summary>
        /// <param name="index">保存列的索引</param>
        /// <returns></returns>
        public object ColumnValue(int index)
        {
            object obj = null;
            if (lst.Count > index)
            {
                Hashtable ht = lst[index] as Hashtable;
                obj = ht[Constants.STR_VALUE];
            }
            return obj;
        }

        /// <summary>
        /// 非=号的查询限制条件
        /// </summary>
        public NotEqualCondition OtherCondition { get; set; }

        /// <summary>
        /// 获取GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public string GetGuid()
        {
            return Guid.NewGuid().ToString();
        }


        /// <summary>
        /// 映射表名及主键字段信息
        /// 子类必需实现该方法
        /// </summary>
        /// <returns></returns>
        abstract public Hashtable MappingTableInfo();

        /// <summary>
        /// 将DataTable中的首行数据转成对应的实体对象
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>返回实体对象,null表示转换失败</returns>
        abstract public Parent GetModelFromDataTable(DataTable dt);
        /// <summary>
        /// 插入时必须赋值的字段
        /// </summary>
        abstract public string RequriedFields { get; }

    }
}
