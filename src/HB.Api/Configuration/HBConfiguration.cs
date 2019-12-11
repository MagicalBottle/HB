using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HB.Api.Configuration
{
    public class HBConfiguration
    {
        /// <summary>
        /// 网站域名
        /// </summary>
        public string Domian { get; set; }

        /// <summary>
        /// 默认的缓存时间60分钟
        /// </summary>
        public int CacheTime { get; set; } = 60;

        /// <summary>
        /// 登录用户缓存名称
        /// </summary>
        public string MebUserCacheKey { get; set; }

        /// <summary>
        /// 登录用户缓存前缀
        /// </summary>
        public string MebUserCachePrefixKey { get; set; }

        /// <summary>
        /// 缓存名称格式{0}代表具体的appid
        /// </summary>
        public string ApiUserCacheKey { get; set; }

        /// <summary>
        /// 缓存名称前缀格式，用于删除缓存
        /// </summary>
        public string ApiUserCachePrefixKey { get; set; }

        #region jwt

        /// <summary>
        /// 令牌发行人
        /// </summary>
        public string Issuer { get; set; }


        public string Audience { get; set; }

        /// <summary>
        /// 验证架构名称
        /// </summary>
        public string AuthenticateScheme { get; set; }

        /// <summary>
        /// 秘钥
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// //token失效时间分钟
        /// </summary>
        public int ExpiresTime { get; set; }

        /// <summary>
        /// 请求的超时时间（单位：秒）
        /// </summary>
        public int NonceExpiresTime { get; set; }
        

        #endregion


    }
}
