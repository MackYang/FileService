using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace SoEasy.Common
{

    /// <summary>
    /// 代表一切返回DataTable,并且有一个OPResult参数的方法 的委托
    /// </summary>
    /// <param name="opRes"></param>
    /// <returns></returns>
    public delegate DataTable Delegate_ReturnDataTable(OPResult opRes);
    /// <summary>
    /// 代表一切返回T类型对象,并且有一个OPResult参数的方法 的委托
    /// </summary>
    /// <typeparam name="T">指定任意类型</typeparam>
    /// <param name="opRes"></param>
    /// <returns></returns>
    public delegate T Delegate_ReturnAnyType<T>(OPResult opRes);


    /// <summary>
    /// 缓存辅助类
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 存放缓存的Key,方便清除缓存时检索用
        /// </summary>
        static List<string> listKeys = new List<string>();
        /// <summary>
        /// 临时的缓存Key数组,清除缓存时从listKeys复制一份值过来修改,不然会报集合已修改无法遍历的异常
        /// </summary>
        static string[] tmpKeyArr = null;
        static Cache cache = HttpRuntime.Cache;
        static CacheItemRemovedCallback onRemove = new CacheItemRemovedCallback(DeleteCacheFile);
        /// <summary>
        /// 对象锁
        /// </summary>
        static object locker = new object();
        #region 私有方法
        static private void DeleteCacheFile(string key, Object value, CacheItemRemovedReason reason)
        {
            string cacheFileName = (string)value;
            if (System.IO.File.Exists(cacheFileName))
            {
                try
                {
                    System.IO.File.Delete(cacheFileName);
                }
                catch (Exception ex)
                {
                    Utility.Logger.Error("移除缓存时,删除缓存文件" + cacheFileName + "发生异常:" + ex);
                }
            }


        }

        /// <summary>
        /// 将对象保存到缓存文件中
        /// </summary>
        /// <param name="cacheKey">缓存的key</param>
        /// <param name="obj">要缓存的对象</param>
        /// <param name="howLong">要缓存多少分钟,默认是20</param>
        /// <param name="compelExpiry">是否强制过期,强制过期就是指存放多久后就过期,否则是多久没有访问才过期,默认是后者</param>
        private static void CacheDataToFile(string cacheKey, Object obj, int howLong = 20, bool compelExpiry = false)
        {
            if (obj != null)
            {
                OPResult opRes = new OPResult();
                string base64Str = SerializeHelper.SerializeObject(obj, opRes);
                if (!string.IsNullOrWhiteSpace(base64Str))
                {
                    string fileID = Guid.NewGuid().ToString();
                    string filePath = Path.Combine(HttpContext.Current.Server.MapPath(Vars.CacheFilePath), fileID + ".cache");
                    FileHelper.WriteFile(filePath, base64Str, opRes);
                    if (opRes.State != Enums.OPState.Exception)
                    {
                        if (compelExpiry)//缓存时间到了就过期
                        {
                            cache.Insert(cacheKey, filePath, null, DateTime.Now.AddMinutes(howLong), Cache.NoSlidingExpiration, CacheItemPriority.High, onRemove);
                        }
                        else//多久没有访问过期
                        {
                            cache.Insert(cacheKey, filePath, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(howLong), CacheItemPriority.High, onRemove);
                        }
                        listKeys.Add(cacheKey);
                    }
                }
            }
        }

        /// <summary>
        /// 从缓存文件中读取指定类型的对象
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="readCacheOK">是否成功读取缓存,true 是,false 否</param>
        /// <returns></returns>
        private static T GetDataFromCacheFile<T>(string cacheKey, out bool readCacheOK)
        {
            readCacheOK = false;
            T t = default(T);
            string filePath = (string)cache.Get(cacheKey);
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                OPResult opRes = new OPResult();
                string base64Str = FileHelper.ReadFile(filePath, opRes);
                if (opRes.State != Enums.OPState.Exception)
                {
                    if (!string.IsNullOrWhiteSpace(base64Str))
                    {
                        t = SerializeHelper.Desrialize<T>(base64Str, opRes);
                        readCacheOK = true;
                    }
                }
            }
            return t;
        }

        #endregion

        /// <summary>
        /// 清除符合条件的缓存
        /// </summary>
        /// <param name="classAndMethodName">被清除缓存的类名和方法名xxx.yyy</param>
        /// <param name="args">缓存数据时的参数,比如当前用户的ID等,当缓存键中同时包含这些参数才会被清除</param>
        public static void Clear(string classAndMethodName, params object[] args)
        {
            Clear(classAndMethodName, args.ToList());
        }

        /// <summary>
        /// 清除符合条件的缓存
        /// </summary>
        /// <param name="classAndMethodName">被清除缓存的类名和方法名xxx.yyy</param>
        /// <param name="args">缓存数据时的参数,比如当前用户的ID等,当缓存键中同时包含这些参数才会被清除</param>
        public static void Clear(string classAndMethodName, List<object> args)
        {
            lock (locker)
            {
                tmpKeyArr = new string[listKeys.Count];
                listKeys.CopyTo(tmpKeyArr);
                IEnumerable<string> listRemove = tmpKeyArr.Where(x => x.StartsWith(classAndMethodName));

                foreach (object item in args)
                {
                    if (listRemove.Count() > 0)
                    {
                        listRemove = listRemove.Where(x => x.Contains(item.ToString()));
                    }
                    else
                    {
                        break;
                    }
                }

                if (listRemove != null && listRemove.Count() > 0)
                {
                    foreach (string item in listRemove)
                    {
                        cache.Remove(item);
                        listKeys.Remove(item);
                    }
                }
            }

        }
        /// <summary>
        /// 从类名.方法名以及参数集合中生成一个缓存的键
        /// </summary>
        /// <param name="o">参数集合</param>
        /// <param name="classAndMethodName">类名和方法名,以类名.方法名的格式传递</param>
        /// <returns></returns>
        public static string GenCacheKey(string classAndMethodName, params object[] o)
        {
            string res = "";
            foreach (object item in o)
            {
                if (item != null)
                {
                    res += item.ToString() + ",";
                }
            }
            if (res.Length > 0)
            {
                res = res.Substring(0, res.Length - 1);
            }
            res = classAndMethodName + ":" + res;
            return res;
        }


        #region DataTable 相关操作

        /// <summary>
        /// 从缓存中获取带分页的DataTable数据
        /// </summary>
        /// <param name="cacheKey">缓存的键</param>
        /// <param name="pager">分页对象</param>
        /// <param name="opRes">操作结果实体</param>
        /// <param name="action">返回DataTable,并且有一个OPResult参数的方法,当缓存中不存在数据时,将调用此方法并将返回值存入缓存中</param>
        /// <param name="howLong">要缓存多少分钟,默认是20</param>
        /// <param name="compelExpiry">是否强制过期,强制过期就是指存放多久后就过期,否则是多久没有访问才过期,默认是后者</param>
        /// <returns></returns>
        public static DataTable GetPageDataTable(string cacheKey, Pager pager, OPResult opRes, Delegate_ReturnDataTable action, int howLong = 20, bool compelExpiry = false)
        {
            bool readCacheOK = false;
            DataTable dtCache = CacheHelper.GetDataFromCacheFile<DataTable>(cacheKey, out readCacheOK);
#if DEBUG
            readCacheOK = false;
#endif
            if (!readCacheOK)//如果没有缓存数据,读取并缓存
            {
                dtCache = action.Invoke(opRes);
                if (opRes.State != Enums.OPState.Exception)
                {
                    CacheHelper.CacheDataToFile(cacheKey, dtCache, howLong, compelExpiry);
                }
            }
            if (pager != null)
            {
                dtCache.AddRowNumber(opRes);
                return dtCache.SelectPage(pager, opRes);
            }
            return dtCache;
        }

        #endregion

        #region 泛型对象相关操作
        /// <summary>
        /// 从缓存中读取指定类型的对象并返回
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="cacheKey">缓存的键</param>
        /// <param name="opRes">操作结果实体</param>
        /// <param name="action">一个带有OPResult参数并且返回T对象的匿名方法,它用于产生T对象的数据源,当缓存中不存在数据时,将调用此方法并将返回值存入缓存中</param>
        /// <param name="howLong">要缓存多少分钟,默认是20</param>
        /// <param name="compelExpiry">是否强制过期,强制过期就是指存放多久后就过期,否则是多久没有访问才过期,默认是后者</param>
        /// <returns></returns>
        public static T GetData<T>(string cacheKey, OPResult opRes, Delegate_ReturnAnyType<T> action, int howLong = 20, bool compelExpiry = false)
        {
            bool readCacheOK = false;
            T data = CacheHelper.GetDataFromCacheFile<T>(cacheKey, out readCacheOK);
#if DEBUG
            readCacheOK = false;
#endif
            if (!readCacheOK)//如果没有缓存数据
            {
                data = action.Invoke(opRes);
                if (opRes.State != Enums.OPState.Exception)
                {
                    CacheHelper.CacheDataToFile(cacheKey, data, howLong, compelExpiry);
                }
            }
            return data;
        }
        #endregion

    }
}
