using SoEasy.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoEasy.LogicTest
{
    public class TestBL : BaseBL
    {
        static TestBL instance = null;
        static object locker = new object();
        private TestBL()
        { }

        public static TestBL CreateInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    instance = new TestBL();
                }
            }
            return instance;
        }

        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <returns>false表示操作失败</returns>
        public bool ExecuteNonQuery()
        {
            string cmdText = "update Person_Info set Name=:newName where name=:oldName";
            DbParameter[] paras = { dao.GetDBParam("newName", "YJY"), dao.GetDBParam("oldName", "杨家勇") };
            return dao.ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        /// <summary>
        /// 执行SQL查询命令
        /// 返回首行首列值
        /// Exception:NULL
        /// </summary>
        /// <returns>null表示操作失败</returns>
        public object ExecuteSingleValue()
        {
            string cmdText = "select count(1) from Person_Info where name=:oldName";
            DbParameter[] paras = { dao.GetDBParam("oldName", "杨家勇") };
            return dao.ExecuteSingleValue(cmdText, CommandType.Text, paras);

        }

        /// <summary>
        /// 执行SQL查询命令
        /// 返回:DataSet
        /// Exception:NULL
        /// </summary>
        /// <returns>null表示操作失败</returns>
        public DataSet ExecuteQueryAsDataSet()
        {
            string cmdText = "select * from Person_Info where name=:oldName";
            DbParameter[] paras = { dao.GetDBParam("oldName", "杨家勇") };
            return dao.ExecuteQueryAsDataSet(cmdText, CommandType.Text, paras);
        }

        /// <summary>
        /// 执行SQL查询命令
        /// 返回:DataTable
        /// Exception:NULL
        /// </summary>
        /// <returns>null表示操作失败</returns>
        public DataTable ExecuteQueryAsDataTable()
        {
            string cmdText = "select * from Person_Info where name=:oldName";
            DbParameter[] paras = { dao.GetDBParam("oldName", "杨家勇") };
            return dao.ExecuteQueryAsDataTable(cmdText, CommandType.Text, paras, null);
        }

        /// <summary>
        /// 执行多条非查询的SQL命令
        /// 支持事务
        /// 返回：True 事务执行成功;False 执行失败，事务回滚
        /// </summary>
        /// <returns>false表示操作失败</returns>
        public bool TransExecuteNonQuery()
        {
            Dictionary<string, DbParameter[]> dict = new Dictionary<string, DbParameter[]>();

            string insertText1 = "insert into Person_Info(ID,Name,Age)values(:id,:name,:age)";
            DbParameter[] insertParas1 = { dao.GetDBParam("id", Guid.NewGuid().ToString()), dao.GetDBParam("name", "?:test"), dao.GetDBParam("age", 15) };

            string insertText = @"insert into order_info (Person_ID,Product_ID,Amount,Total_price)
                                        values(:personID,:productID,:amount,:total_price)";
            DbParameter[] insertParas = { 
                                            dao.GetDBParam("personID", "tran_per_id_1"), 
                                            dao.GetDBParam("productID", "tran_pro_id_1"), 
                                            dao.GetDBParam("amount",3), 
                                            dao.GetDBParam("total_price",103.69) 
                                        };

            string insertTransData = @"insert into order_info(PERSON_ID) select ID  from Person_Info where OP_Time>:oldTime";
            DbParameter[] insTPara = { dao.GetDBParam("oldTime", DateTime.Now.AddMinutes(-10)) };

            string updateText = "update product_Info set Store_num=Store_num-1  where ID=:ID";
            DbParameter[] updateParas = { dao.GetDBParam("ID", "tran_pro_id_1") };

            string deleteText = "delete from Person_Info where 1=2";

            dict.Add(insertText, insertParas);
            dict.Add(updateText, updateParas);
            dict.Add(deleteText, null);


            dict.Add(insertText1, insertParas1);
            dict.Add(insertTransData, insTPara);

            return dao.TransExecuteNonQuery(dict);
        }
    }
}
