using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ruanmou.Redis.Init
{
    /// <summary>
    /// Redis管理中心
    /// </summary>
    public class RedisManager
    {
        /// <summary>
        /// redis配置文件信息
        /// </summary>
        private static RedisConfigInfo RedisConfigInfo = new RedisConfigInfo();

        /// <summary>
        /// Redis客户端池化管理
        /// </summary>
        private static PooledRedisClientManager prcManager;

        /// <summary>
        /// 静态构造方法，初始化链接池管理对象
        /// </summary>
        static RedisManager()
        {
            CreateManager();
        }

        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        private static void CreateManager()
        {
            string[] WriteServerConStr = RedisConfigInfo.WriteServerList.Split(',');
            string[] ReadServerConStr = RedisConfigInfo.ReadServerList.Split(',');

            RedisClientManagerConfig redisClientManagerConfig = new RedisClientManagerConfig();
            {
                redisClientManagerConfig.MaxWritePoolSize = RedisConfigInfo.MaxWritePoolSize;
                redisClientManagerConfig.MaxReadPoolSize = RedisConfigInfo.MaxReadPoolSize;
                redisClientManagerConfig.AutoStart = RedisConfigInfo.AutoStart;
            }
            prcManager = new PooledRedisClientManager(ReadServerConStr, WriteServerConStr, redisClientManagerConfig);
        }

        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        public static IRedisClient GetClient()
        {
            return prcManager.GetClient();
        }
    }
}
