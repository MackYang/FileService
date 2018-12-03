using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoEasy.Common
{
    /// <summary>
    /// 枚举类
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// 请求类型
        /// </summary>
        public enum RequestType
        {
            Get = 0,
            Post = 1
        }


        /// <summary>
        /// 用户类型
        /// </summary>
        public enum UserType
        {

            /// <summary>
            /// 普通用户
            /// </summary>
            GenUser = 0,
            /// <summary>
            /// 商家用户
            /// </summary>
            ShopUser = 1,
            /// <summary>
            /// 后台用户
            /// </summary>
            PlatformUser = 2,

            /// <summary>
            /// 店铺客服用户
            /// </summary>
            ShopCustomerServiceUser = 3

        }

        /// <summary>
        /// 四则运算枚举
        /// </summary>
        public enum CalcType
        {
            /// <summary>
            /// 加
            /// </summary>
            Add = 0,
            /// <summary>
            /// 减
            /// </summary>
            Subduction = 1,
            /// <summary>
            /// 乘
            /// </summary>
            Multiply = 2,
            /// <summary>
            /// 除
            /// </summary>
            Division = 3
        }

        /// <summary>
        /// 表示操作状态的枚举
        /// </summary>
        public enum OPState
        {
            /// <summary>
            /// 成功
            /// </summary>
            Success = 1,
            /// <summary>
            /// 失败
            /// </summary>
            Fail = 0,
            /// <summary>
            /// 异常
            /// </summary>
            Exception = -1
        }

        /// <summary>
        /// 数据库操作类型，用于在同一事务中对多个实体进行不同操作
        /// </summary>
        public enum DbOptionType
        {
            /// <summary>
            /// 插入操作
            /// </summary>
            Insert = 0,
            /// <summary>
            /// 更新操作
            /// </summary>
            Update = 1,
            /// <summary>
            /// 删除操作
            /// </summary>
            Delete = 2
        }


        /// <summary>
        /// 日志级别
        /// </summary>
        public enum LogLevel
        {
            /// <summary>
            /// 调试日志
            /// </summary>
            DEBUG = 0,
            /// <summary>
            /// 一般信息记录
            /// </summary>
            INFO = 1,
            /// <summary>
            /// 警告日志
            /// </summary>
            WARN = 2,
            /// <summary>
            /// 错误日志
            /// </summary>
            ERROR = 3,
            /// <summary>
            /// 致命错误信息日志,代表错误相当严重
            /// </summary>
            FATAL = 4,
            /// <summary>
            /// 所有
            /// </summary>
            All = -1
        }

        /// <summary>
        /// 平台类型
        /// </summary>
        public enum PlatformType
        {
            /// <summary>
            /// Web页面
            /// </summary>
            Web = 0,
            /// <summary>
            /// Android客户端
            /// </summary>
            Android = 1,
            /// <summary>
            /// IOS客户端
            /// </summary>
            IOS = 2,
            /// <summary>
            /// 其它客户端
            /// </summary>
            Other = 3,

        }
    }
}
