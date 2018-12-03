using System.Data.Common;
using System.Data;
using SoEasy.Common;
using SoEasy.Model.BaseEntity;
using System.Collections.Generic;
namespace SoEasy.DB.Interface
{
    /// <summary>
    /// 数据操作接口
    /// </summary>
    public interface IBaseDAO
    {

        #region Base

        /// <summary>
        /// 严格模式(字段内的多个关键子用and ,字段间用or)获取关键字的内部SQL条件,将对keyword空格分割后分别like,并在paramList中添加对应的参数
        /// </summary>
        /// <param name="fieldsName">包含关键字的数据库字段集合,也就是用于like%X%的字段</param>
        /// <param name="keyword">关键字内容</param>
        /// <param name="paramList">查询参数列表</param>
        /// <returns></returns>
        string GetKeywordSQLStrict(string keyword, List<DbParameter> paramList, params string[] fieldsArr);

        /// 严格模式(字段内的多个关键子用and ,字段间用or)获取关键字的内部SQL条件,将对keyword空格分割后分别like,并在paramList中添加对应的参数
        /// </summary>
        /// <param name="fieldsName">包含关键字的数据库字段集合,也就是用于like%X%的字段</param>
        /// <param name="keyword">关键字内容</param>
        /// <param name="paramList">查询参数列表</param>
        /// <returns></returns>
        string GetKeywordSQLStrict(string keyword, List<Args> paramList, params string[] fieldsArr);


        /// <summary>
        /// 获取关键字的内部SQL条件,将对keyword空格分割后分别like,并在paramList中添加对应的参数
        /// </summary>
        /// <param name="fieldsName">包含关键字的数据库字段集合,也就是用于like%X%的字段</param>
        /// <param name="keyword">关键字内容</param>
        /// <param name="paramList">查询参数列表</param>
        /// <returns></returns>
        string GetKeywordSQL(string keyword, List<DbParameter> paramList, params string[] fieldsArr);

        /// 获取关键字的内部SQL条件,将对keyword空格分割后分别like,并在paramList中添加对应的参数
        /// </summary>
        /// <param name="fieldsName">包含关键字的数据库字段集合,也就是用于like%X%的字段</param>
        /// <param name="keyword">关键字内容</param>
        /// <param name="paramList">查询参数列表</param>
        /// <returns></returns>
        string GetKeywordSQL(string keyword, List<Args> paramList, params string[] fieldsArr);


        /// <summary>
        /// 获取where字句,用于动态拼接where子句时
        /// </summary>
        /// <param name="whereStr">已有的where子句,没有就传null</param>
        ///<param name="condition">条件,如age>16</param>
        /// <returns></returns>
        string GetWhere(string whereStr, string condition);

        /// <summary>
        /// 根据表名获取表结构
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <returns>null表示操作失败</returns>
        DataTable GetTableStructByTableName(string tableName);

        /// <summary>
        /// 以实体对象的值作为条件统计
        /// </summary>
        /// <param name="p">实体</param>
        /// <returns>-1表示操作失败</returns>
        long Count(Parent p);

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="p">实体对象</param>
        /// <returns>-1表示操作失败</returns>
        int Insert(Parent p);

        /// <summary>
        /// 删除符合实体条件的记录
        /// </summary>
        /// <param name="p">实体对象</param>
        /// <returns>-1表示操作失败</returns>
        int Delete(Parent p);

        /// <summary>
        /// 更新符合实体对象属性值作为条件的记录
        /// </summary>
        /// <param name="p">实体对象</param>
        /// <returns>-1表示操作失败</returns>
        int Update(Parent p);

        /// <summary>
        /// 以实体对象属性值作为条件查询
        /// </summary>
        /// <param name="p">实体对象</param>
        /// <param name="columnsName">要查询的列名</param>
        /// <param name="orderByColumnName">排序的列</param>
        /// <param name="orderMode">排序模式</param>
        /// <returns>null表示操作失败</returns>
        DataTable Select(Parent p, string columnsName, string orderByColumnName, string orderMode);

        /// <summary>
        /// 以实体对象属性值作为条件随机查询，让每次返回的数据顺序不一样
        /// </summary>
        /// <param name="columnsName">要查询的列名,用逗号分开</param>
        /// <param name="p">实体对象</param>
        /// <returns>null表示操作失败</returns>
        DataTable SelectRandom(Parent p, string columnsName);

        /// <summary>
        ///  以实体对象属性值作为条件查询,带分页
        /// </summary>
        /// <param name="p">实体对象</param>
        /// <param name="columnsName">要查询的列名,用逗号分开</param>
        ///<param name="pager">分页对象</param>
        /// <param name="orderByColumnName">排序字段名称,默认Create_Time</param>
        /// <param name="orderMode">排序模式，默认Desc</param>
        /// <returns>null表示操作失败</returns>
        DataTable SelectPage(Parent p, Pager pager, string columnsName, string orderByColumnName, string orderMode);

        /// <summary>
        /// 以实体对象属性值作为条件随机查询，让每次返回的数据顺序不一样,带分页
        /// </summary>
        /// <param name="p">实体对象</param>
        /// <param name="pager">分页对象</param>
        /// <param name="columnsName">要查询的列名,用逗号分开</param>
        /// <returns>null表示操作失败</returns>
        DataTable SelectPageRandom(Parent p, Pager pager, string columnsName);

        /// <summary>
        /// 两表join查询
        /// </summary>
        /// <param name="join">join对象</param>
        /// <param name="orderByColumnName">排序列名称</param>
        /// <param name="orderMode">排序模式,升还是降</param>
        /// <returns>null表示操作失败</returns>
        DataTable SelectJoin(Join join, string orderByColumnName, string orderMode);

        /// <summary>
        /// 两表join查询后,随机返回记录
        /// </summary>
        /// <param name="join">Join对象</param>
        /// <returns>null表示操作失败</returns>
        DataTable SelectJoinRandom(Join join);

        /// <summary>
        /// 两表join分页查询
        /// </summary>
        /// <param name="join">join对象</param>
        /// <param name="pager">分页对象</param>
        /// <param name="orderByColumnName">排序列名称</param>
        /// <param name="orderMode">排序模式,升还是降</param>
        /// <returns>null表示操作失败</returns>
        DataTable SelectJoinPage(Join join, Pager pager, string orderByColumnName, string orderMode);

        /// <summary>
        /// 两表join分页查询后,随机返回记录
        /// </summary>
        /// <param name="join">join对象</param>
        /// <param name="pager">分页对象</param>
        /// <returns>null表示操作失败</returns>
        DataTable SelectJoinPageRandom(Join join, Pager pager);

        /// <summary>
        /// 两表Union查询
        /// </summary>
        /// <param name="union">Union对象</param>
        /// <param name="orderByColumnName">排序字段,请用AModel中的字段</param>
        /// <param name="orderMode">排序模式</param>
        /// <returns>null表示操作失败</returns>
        DataTable SelectUnion(Union union, string orderByColumnName, string orderMode);

        /// <summary>
        /// 两表Union查询
        /// </summary>
        /// <param name="union">Union对象</param>
        /// <param name="orderByColumnName">排序列名称,请用AModel中的字段</param>
        /// <param name="orderMode">排序模式,升还是降</param>
        /// <returns>null表示操作失败</returns>
        DataTable SelectUnionRandom(Union union);

        /// <summary>
        /// 两表Union分页查询
        /// </summary>
        /// <param name="union">Union对象</param>
        /// <param name="pager">分页对象</param>
        /// <param name="orderByColumnName">排序列名称,请用AModel中的字段</param>
        /// <param name="orderMode">排序模式,升还是降</param>
        /// <returns>null表示操作失败</returns>
        DataTable SelectUnionPage(Union union, Pager pager, string orderByColumnName, string orderMode);

        /// <summary>
        /// 两表Union分页查询后,随机返回记录
        /// </summary>
        /// <param name="union">Union对象</param>
        /// <param name="pager">分页对象</param>
        /// <returns>null表示操作失败</returns>
        DataTable SelectUnionPageRandom(Union union, Pager pager);

        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <param name="paras">命令参数</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">SQL语句,如果语句中带参数,参数前缀统一用:来表示(注意!参数标示统一用冒号:做前缀,将在底层统一替换)</param>
        /// <returns>false表示操作失败</returns>
        bool ExecuteNonQuery(string cmdText, CommandType cmdType, DbParameter[] paras);

        /// <summary>
        /// 执行SQL查询命令
        /// 返回首行首列值
        /// Exception:NULL
        /// </summary>
        /// <param name="cmdText">命令语句</param>
        /// <param name="cmdText">SQL语句,如果语句中带参数,参数前缀统一用:来表示(注意!参数标示统一用冒号:做前缀,将在底层统一替换)</param>
        /// <param name="paras">命令参数</param>
        /// <returns>null表示操作失败</returns>
        object ExecuteSingleValue(string cmdText, CommandType cmdType, DbParameter[] paras);

        /// <summary>
        /// 执行SQL查询命令
        /// 返回:DataSet
        /// Exception:NULL
        /// </summary>
        /// <param name="cmdText">SQL语句,如果语句中带参数,参数前缀统一用:来表示(注意!参数标示统一用冒号:做前缀,将在底层统一替换)</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="paras">命令参数</param>
        /// <returns>null表示操作失败</returns>
        DataSet ExecuteQueryAsDataSet(string cmdText, CommandType cmdType, DbParameter[] paras);

        /// <summary>
        /// 执行SQL查询命令
        /// 返回:DataTable
        /// Exception:NULL
        /// </summary>
        /// <param name="cmdText">SQL语句,如果语句中带参数,参数前缀统一用:来表示(注意!参数标示统一用冒号:做前缀,将在底层统一替换)</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="paras">命令参数</param>
        /// <param name="pager">分页对象</param>
        /// <returns>null表示操作失败</returns>
        DataTable ExecuteQueryAsDataTable(string cmdText, CommandType cmdType, DbParameter[] paras, Pager pager);

        #endregion

        #region Trans

        /// <summary>
        /// 在同一事务中插入个实体对象,可用不同的实体对象向多表中插入数据
        /// </summary>
        /// <param name="p">实体对象集合</param>
        /// <returns>false表示操作失败</returns>
        bool TransInsert(List<Parent> listP);

        /// <summary>
        /// 在同一事务中更新多个实体对象属性值作为条件的记录,可用不同的实体对象更新多表的数据
        /// </summary>
        /// <param name="p">实体对象集合</param>
        /// <returns>false表示操作失败</returns>
        bool TransUpdate(List<Parent> listP);

        /// <summary>
        /// 在同一事务中删除个实体对象属性值作为条件的记录,可用不同类型的实体对象删除多张表
        /// </summary>
        /// <param name="p">实体对象集合</param>
        /// <returns>false表示操作失败</returns>
        bool TransDelete(List<Parent> listP);

        /// <summary>
        /// 执行多条非查询的SQL命令
        /// 支持事务
        /// 返回：True 事务执行成功;False 执行失败，事务回滚
        /// </summary>
        /// <param name="cmdTextDic">命令集合，键为执行SQL语句..如果语句中带参数,参数前缀统一用:来表示(注意!参数标示统一用冒号:做前缀,将在底层统一替换),  值为SQL语句参数的集合</param>
        /// <returns>false表示操作失败</returns>
        bool TransExecuteNonQuery(Dictionary<string, DbParameter[]> cmdTextDic);

        /// <summary>
        /// 在同一事务中对多个表进行增删改操作,根据多个实体与操作类型操作数据库
        /// </summary>
        /// <param name="dicModel">实体与操作映射的集合</param>
        /// <returns></returns>
        bool TransExecuteModels(Dictionary<Parent, Enums.DbOptionType> dicModel);
        #endregion

        #region Difference

        /// <summary>
        /// 获取某种数据库产生随机数的SQL语句,例如Oracle的是dbms_random.random
        /// </summary>
        string GetRandomString { get; }

        /// <summary>
        /// 获取一个某种数据库类型的参数
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="paramValue">参数值</param>
        /// <returns>返回一种数据库参数的对象,如OracleParam</returns>
        DbParameter GetDBParam(string paramName, object paramValue);

        /// <summary>
        /// 拼接SQL字符串
        /// </summary>
        /// <param name="strAs">要拼接的多个字符串</param>
        /// <returns>拼接后的字符串</returns>
        string ConcatenateString(params string[] strAs);
        #endregion


    }
}
