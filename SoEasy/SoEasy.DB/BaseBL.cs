using SoEasy.Common;
using SoEasy.DB.DAO;
using SoEasy.DB.Interface;
using System;

namespace SoEasy.DB
{
    public class BaseBL
    {
        protected static IBaseDAO dao = null;
        static object locker = new object();
        public BaseBL()
        {
            if (dao == null)
            {
                lock (locker)
                {
                    try
                    {
                        string databaseType = Vars.DBType;
                        if (!string.IsNullOrWhiteSpace(databaseType))
                        {
                            databaseType = databaseType.ToLower();
                            string[] typeArr = { "mariadb", "oracle", "sqlserver", "sqlite" };
                            string currentDatabaseType = databaseType.GetLikeFirstElement(typeArr);
                            switch (currentDatabaseType)
                            {
                                case "mariadb":
                                    dao = new MariaDAO();
                                    break;
                                case "oracle":
                                    dao = new OracleDAO();
                                    break;
                                case "sqlserver":
                                    dao = new SQLServerDAO();
                                    break;
                                case "sqlite":
                                    dao = new SQLiteDAO();
                                    break;
                                default:
                                    throw new Exception("暂不支持此种数据库类型.");
                            }

                        }
                        else
                        {
                            throw new Exception("读取默认的数据库配置失败,请确保配置文件Config/DBType节点值不为空.");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("创建数据库操作对象失败:" + ex);
                    }

                }
            }

        }


    }
}
