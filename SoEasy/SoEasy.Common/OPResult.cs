using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoEasy.Common
{
    /// <summary>
    /// 封装了操作结果的类
    /// </summary>
    [Serializable]
    public class OPResult
    {
        object _data;
        Enums.OPState _state;
        public OPResult()
        {
            State = Enums.OPState.Fail;
        }

        /// <summary>
        /// 操作状态
        /// </summary>
        public Enums.OPState State
        {
            get { return _state; }
            set { if (_state != Enums.OPState.Exception) { _state = value; } }
        }

        /// <summary>
        /// 结果数据
        /// </summary>
        public object Data
        {
            get { return _data; }
            set
            {
                if (_state != Enums.OPState.Exception)
                {
                    _data = value;
                }
            }
        }
        
    }

    /// <summary>
    /// 操作结果的扩展类
    /// </summary>
    public static class OPResultEx
    {
        /// <summary>
        /// 将操作结果转换成Json字符串
        /// </summary>
        /// <param name="opRes"></param>
        /// <returns></returns>
        public static string ToJsonString(this OPResult opRes)
        {
            return JsonHelper.ToJsonString(opRes);
        }

        /// <summary>
        /// 为了防止覆盖异常时的信息,不直接opRes=xxx,而是采用分别给属性赋值的方式,在赋值时,如果原值状态为异常,将不影响原值
        /// </summary>
        /// <param name="opRes">原值</param>
        /// <param name="newData">要赋的新值</param>
        public static void SetData(this OPResult opRes, OPResult newData)
        {
            opRes.Data = newData.Data;
            opRes.State = newData.State;
        }

        /// <summary>
        /// 清空Data的内容,减少网络传输负载
        /// </summary>
        /// <param name="opRes"></param>
        public static void ClearData(this OPResult opRes)
        {
            if (opRes.State == Enums.OPState.Success)
            {
                opRes.Data = null;
            }
        }

        /// <summary>
        /// 如果操作成功,清除指定方法的缓存
        /// </summary>
        /// <param name="opRes"></param>
        /// <param name="classAndMethodName">被清除缓存的类名和方法名xxx.yyy</param>
        /// <param name="args">缓存数据时的参数,比如当前用户的ID等,当缓存键中同时包含这些参数才会被清除</param>
        public static void ClearCache(this OPResult opRes, string classAndMethodName, params object[] args)
        {
            if (opRes.State == Enums.OPState.Success)
            {
                CacheHelper.Clear(classAndMethodName, args);
            }
        }

        /// <summary>
        /// 如果操作成功,清除指定方法的缓存
        /// </summary>
        /// <param name="opRes"></param>
        /// <param name="classAndMethodName">被清除缓存的类名和方法名xxx.yyy</param>
        /// <param name="args">缓存数据时的参数,比如当前用户的ID等,当缓存键中同时包含这些参数才会被清除</param>
        public static void ClearCache(this OPResult opRes, string classAndMethodName, List<object> args)
        {
            if (opRes.State == Enums.OPState.Success)
            {
                CacheHelper.Clear(classAndMethodName, args);
            }
        }
    }
}
