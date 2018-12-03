using SoEasy.Common;
using SoEasy.DB;
using SoEasy.Model.BaseEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoEasy.Logic
{
    /// <summary>
    /// 通用的数据库操作类
    /// </summary>
    public class CommonBL : BaseBL
    {
        static CommonBL instance = null;
        static object locker = new object();
        private CommonBL()
        { }

        /// <summary>
        /// 创建一个实例
        /// </summary>
        /// <returns></returns>
        public static CommonBL CreateInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    instance = new CommonBL();
                }
            }
            return instance;
        }

        /// <summary>
        /// 检查是否存在数据,是为true,否为false,并自动设置opRes的值
        /// </summary>
        /// <param name="p"></param>
        /// <param name="opRes"></param>
        /// <returns></returns>
        public bool HasData(Parent p, OPResult opRes)
        {
            if (p != null)
            {
                return true;
            }
            else
            {
                opRes.SetData(Constants.NoData);
                return false;
            }
        }

        /// <summary>
        /// 检查是否存在数据,是为true,否为false,并自动设置opRes的值
        /// </summary>
        /// <param name="p"></param>
        /// <param name="opRes"></param>
        /// <returns></returns>
        public bool HasData(DataTable dt, OPResult opRes)
        {
            if (!dt.IsNullOrEmpty())
            {
                return true;
            }
            else
            {
                opRes.SetData(Constants.NoData);
                return false;
            }
        }


        /// <summary>
        /// 拼接SQL字符串
        /// </summary>
        /// <param name="strAs">要接连的多个字符串</param>
        /// <returns>拼接后的字符串</returns>
        public string ConcatenateString(params string[] strAs)
        {
            return dao.ConcatenateString(strAs);
        }

        /// <summary>
        /// 拼接字符串,以符合likeArg参数内容开头的查询条件
        /// </summary>
        /// <param name="likeArg">likeArg参数,以:开头</param>
        /// <returns>拼接后的字符串</returns>
        public string StartWith(string likeArg)
        {
            return " like " + dao.ConcatenateString(" " + likeArg, "'%'");
        }

        /// <summary>
        /// 拼接字符串,以符合likeArg参数内容结尾的查询条件
        /// </summary>
        /// <param name="likeArg">likeArg参数,以:开头</param>
        /// <returns>拼接后的字符串</returns>
        public string EndWith(string likeArg)
        {
            return " like " + dao.ConcatenateString(" '%'", likeArg);
        }

        /// <summary>
        /// 拼接字符串,以符合包含likeArg参数内容的查询条件
        /// </summary>
        /// <param name="likeArg">likeArg参数,以:开头</param>
        /// <returns>拼接后的字符串</returns>
        public string Contain(string likeArg)
        {
            return " like " + dao.ConcatenateString(" '%'", likeArg, "'%'");
        }


        /// <summary>
        /// 获取关键字的内部SQL条件,将对keyword空格分割后分别like,并在paramList中添加对应的参数
        /// </summary>
        /// <param name="fieldsName">包含关键字的数据库字段集合,也就是用于like%X%的字段</param>
        /// <param name="keyword">关键字内容</param>
        /// <param name="paramList">查询参数列表</param>
        /// <returns></returns>
        public string GetKeywordSQL(string keyword, List<Args> paramList, params string[] fieldsArr)
        {
            return dao.GetKeywordSQL(keyword, paramList, fieldsArr);

        }


        /// <summary>
        /// 严格模式(字段内的多个关键子用and ,字段间用or)获取关键字的内部SQL条件,将对keyword空格分割后分别like,并在paramList中添加对应的参数
        /// </summary>
        /// <param name="fieldsName">包含关键字的数据库字段集合,也就是用于like%X%的字段</param>
        /// <param name="keyword">关键字内容</param>
        /// <param name="paramList">查询参数列表</param>
        /// <returns></returns>
        public string GetKeywordSQLStrict(string keyword, List<Args> paramList, params string[] fieldsArr)
        {
            return dao.GetKeywordSQLStrict(keyword, paramList, fieldsArr);

        }
        /// <summary>
        /// 将符合实体对象属性作为查询条件的首条数据,填充到新的对象实体中返回
        /// </summary>
        /// <typeparam name="T">实体对象类型</typeparam>
        /// <param name="t">用于查询的实体对象</param>
        /// <param name="orderByColumnName">排序字段名称</param>
        /// <param name="orderMode">排序模式</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <returns>返回实体对象,为null表示没有符合条件的数据,或发生异常</returns>
        public T SelectFirst<T>(T t, OPResult opRes, string orderByColumnName = null, string orderMode = null, bool throwException = false) where T : Parent, new()
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return Select(t, null, opRes, orderByColumnName, orderMode).GetTModel<T>(opRes);
            }, opRes, throwException);
        }

        /// <summary>
        /// 以实体对象属性值作为条件查询
        /// </summary>
        /// <param name="p">实体对象</param>
        /// <param name="columnsName">要查询的列名</param>
        /// <param name="orderByColumnName">排序的列,默认是OP_Time</param>
        /// <param name="orderMode">排序模式，默认是Desc</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <returns>null表示操作失败</returns>
        public DataTable Select(Parent p, string columnsName, OPResult opRes, string orderByColumnName = "OP_Time", string orderMode = " Desc", bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.Select(p, columnsName, orderByColumnName, orderMode);
            }, opRes, throwException);
        }

        /// <summary>
        /// 以实体对象属性值作为条件随机查询，让每次返回的数据顺序不一样
        /// </summary>
        /// <param name="columnsName">要查询的列名,用逗号分开</param>
        /// <param name="p">实体对象</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <returns>null表示操作失败</returns>
        public DataTable SelectRandom(Parent p, string columnsName, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.SelectRandom(p, columnsName);
            }, opRes, throwException);
        }

        /// <summary>
        ///  以实体对象属性值作为条件查询,带分页
        /// </summary>
        /// <param name="p">实体对象</param>
        /// <param name="columnsName">要查询的列名,用逗号分开</param>
        ///<param name="pager">分页对象</param>
        /// <param name="orderByColumnName">排序字段名称,默认OP_Time</param>
        /// <param name="orderMode">排序模式，默认Desc</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <returns>null表示操作失败</returns>
        public DataTable SelectPage(Parent p, Pager pager, string columnsName, OPResult opRes, string orderByColumnName = "OP_Time", string orderMode = " Desc", bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.SelectPage(p, pager, columnsName, orderByColumnName, orderMode);
            }, opRes, throwException);
        }

        /// <summary>
        /// 两表join查询
        /// </summary>
        /// <param name="join">join对象</param>
        /// <param name="orderByColumnName">排序列名称</param>
        /// <param name="orderMode">排序模式,升还是降</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <returns>null表示操作失败</returns>
        public DataTable SelectJoin(Join join, OPResult opRes, string orderByColumnName = "l.OP_Time", string orderMode = " Desc", bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.SelectJoin(join, orderByColumnName, orderMode);
            }, opRes, throwException);
        }

        /// <summary>
        /// 两表join查询后,随机返回记录
        /// </summary>
        /// <param name="join">Join对象</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <returns>null表示操作失败</returns>
        public DataTable SelectJoinRandom(Join join, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.SelectJoinRandom(join);
            }, opRes, throwException);
        }

        /// <summary>
        /// 两表join分页查询
        /// </summary>
        /// <param name="join">join对象</param>
        /// <param name="pager">分页对象</param>
        /// <param name="orderByColumnName">排序列名称</param>
        /// <param name="orderMode">排序模式,升还是降</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <returns>null表示操作失败</returns>
        public DataTable SelectJoinPage(Join join, Pager pager, OPResult opRes, string orderByColumnName = "l.OP_Time", string orderMode = " Desc", bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.SelectJoinPage(join, pager, orderByColumnName, orderMode);
            }, opRes, throwException);
        }

        /// <summary>
        /// 两表join分页查询后,随机返回记录
        /// </summary>
        /// <param name="join">join对象</param>
        /// <param name="pager">分页对象</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <returns>null表示操作失败</returns>
        public DataTable SelectJoinPageRandom(Join join, Pager pager, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.SelectJoinPageRandom(join, pager);
            }, opRes, throwException);
        }


        /// <summary>
        /// 两表Union查询
        /// </summary>
        /// <param name="union">Union对象</param>
        /// <param name="orderByColumnName">排序字段,请用AModel中的字段</param>
        /// <param name="orderMode">排序模式</param>
        /// <returns>null表示操作失败</returns>
        public DataTable SelectUnion(Union union, OPResult opRes, bool throwException = false, string orderByColumnName = "", string orderMode = "")
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.SelectUnion(union, orderByColumnName, orderMode);
            }, opRes, throwException);

        }

        /// <summary>
        /// 两表Union查询
        /// </summary>
        /// <param name="union">Union对象</param>
        /// <param name="orderByColumnName">排序列名称,请用AModel中的字段</param>
        /// <param name="orderMode">排序模式,升还是降</param>
        /// <returns>null表示操作失败</returns>
        public DataTable SelectUnionRandom(Union union, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.SelectUnionRandom(union);
            }, opRes, throwException);
        }

        /// <summary>
        /// 两表Union分页查询
        /// </summary>
        /// <param name="union">Union对象</param>
        /// <param name="pager">分页对象</param>
        /// <param name="orderByColumnName">排序列名称,请用AModel中的字段</param>
        /// <param name="orderMode">排序模式,升还是降</param>
        /// <returns>null表示操作失败</returns>
        public DataTable SelectUnionPage(Union union, Pager pager, OPResult opRes, bool throwException = false, string orderByColumnName = "", string orderMode = "")
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.SelectUnionPage(union, pager, orderByColumnName, orderMode);
            }, opRes, throwException);
        }

        /// <summary>
        /// 两表Union分页查询后,随机返回记录
        /// </summary>
        /// <param name="union">Union对象</param>
        /// <param name="pager">分页对象</param>
        /// <returns>null表示操作失败</returns>
        public DataTable SelectUnionPageRandom(Union union, Pager pager, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.SelectUnionPageRandom(union, pager);
            }, opRes, throwException);
        }


        /// <summary>
        /// 根据表名获取表结构
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <returns>null表示操作失败</returns>
        public DataTable GetTableStructByTableName(string tableName, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.GetTableStructByTableName(tableName);
            }, opRes, throwException);
        }


        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="p">实体对象</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <returns>false表示操作失败</returns>
        public bool Insert(Parent p, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.Insert(p) != -1 ? true : false;
            }, opRes, throwException);
        }

        /// <summary>
        /// 更新符合实体对象属性值作为条件的记录
        /// </summary>
        /// <param name="p">实体对象</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <returns>false表示操作失败</returns>
        public bool Update(Parent p, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.Update(p) != -1 ? true : false;
            }, opRes, throwException);
        }

        /// <summary>
        /// 删除符合实体条件的记录
        /// </summary>
        /// <param name="p">实体对象</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <returns>false表示操作失败</returns>
        public bool Delete(Parent p, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.Delete(p) != -1 ? true : false;
            }, opRes, throwException);
        }

        /// <summary>
        /// 以实体对象的值作为条件统计
        /// </summary>
        /// <param name="p">实体</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <returns>-1表示操作失败</returns>
        public long Count(Parent p, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.Count(p);
            }, opRes, throwException);
        }

        /// <summary>
        /// 在同一事务中插入个实体对象,可用不同的实体对象向多表中插入数据
        /// </summary>
        /// <param name="listP">实体对象集合</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <returns>false表示操作失败</returns>
        public bool TransInsert(List<Parent> listP, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.TransInsert(listP);
            }, opRes, throwException);
        }

        /// <summary>
        /// 在同一事务中删除个实体对象属性值作为条件的记录,可用不同类型的实体对象删除多张表
        /// </summary>
        /// <param name="listP">实体对象集合</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <returns>false表示操作失败</returns>
        public bool TransDelete(List<Parent> listP, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.TransDelete(listP);
            }, opRes, throwException);
        }

        /// <summary>
        /// 在同一事务中更新多个实体对象属性值作为条件的记录,可用不同的实体对象更新多表的数据
        /// </summary>
        /// <param name="listP">实体对象集合</param>
        /// <param name="throwException">出现异常时是否抛出</param>
        /// <returns>false表示操作失败</returns>
        public bool TransUpdate(List<Parent> listP, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.TransUpdate(listP);
            }, opRes, throwException);
        }

        /// <summary>
        /// 执行SQL查询命令
        /// 返回:DataTable
        /// Exception:NULL
        /// </summary>
        /// <param name="cmdText">SQL语句,如果语句中带参数,参数前缀统一用:来表示(注意!参数标示统一用冒号:做前缀,将在底层统一替换)</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="paras">命令参数</param>
        /// <param name="opRes">操作结果</param>
        /// <param name="pager">分页对象</param>
        /// <param name="throwException">发生异常是否抛出</param>
        /// <returns>null表示操作失败</returns>
        public DataTable ExecuteQueryAsDataTable(string cmdText, CommandType cmdType, DbParameter[] paras, Pager pager, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.ExecuteQueryAsDataTable(cmdText, cmdType, paras, pager);
            }, opRes, throwException);
        }

        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <param name="cmdText">SQL语句,如果语句中带参数,参数前缀统一用:来表示(注意!参数标示统一用冒号:做前缀,将在底层统一替换)</param>
        /// <param name="paras">命令参数</param>
        /// <param name="cmdType">命令类型</param>
        /// <returns>false表示操作失败</returns>
        public bool ExecNoneQuery(string cmdText, CommandType cmdType, DbParameter[] paras, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.ExecuteNonQuery(cmdText, cmdType, paras);
            }, opRes, throwException);
        }

        /// <summary>
        /// 在同一事务中对多个表进行增删改操作,根据多个实体与操作类型操作数据库
        /// </summary>
        /// <param name="dicModel">实体与操作映射的集合</param>
        /// <returns></returns>
        public bool TransExecuteModels(Dictionary<Parent, Enums.DbOptionType> dicModel, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.TransExecuteModels(dicModel);
            }, opRes, throwException);
        }

        /// <summary>
        /// 执行多条非查询的SQL命令
        /// 支持事务
        /// 返回：True 事务执行成功;False 执行失败，事务回滚
        /// </summary>
        /// <param name="cmdTextDic">命令集合，键为执行SQL语句..如果语句中带参数,参数前缀统一用:来表示(注意!参数标示统一用冒号:做前缀,将在底层统一替换),  值为SQL语句参数的集合</param>
        /// <returns>false表示操作失败</returns>
        public bool TransExecuteNonQuery(Dictionary<string, DbParameter[]> cmdTextDic, OPResult opRes, bool throwException = false)
        {
            return ExceptionHelper.ExceptionRecord(() =>
            {
                return dao.TransExecuteNonQuery(cmdTextDic);
            }, opRes, throwException);
        }
    }
}
