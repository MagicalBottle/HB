using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HB.Api.Models
{

    /// <summary>
    /// 返回成功失败
    /// </summary>
    public class ReponseOutPut
    {
        /// <summary>
        /// 状态, 0失败 ，1成功 默认1
        /// </summary>
        public ReutnStatus Status { get; set; } = ReutnStatus.Success;

        /// <summary>
        /// 错误编码
        /// </summary>
        public string Code { get; set; } = "Success";

        /// <summary>
        /// 提示信息 默认Success
        /// </summary>
        public string Message { get; set; } = "成功";

        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; } = new { };

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public object Data { get; set; } =null;
    }

    /// <summary>
    /// 返回分页查询结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedListReponseOutPut<T> : ReponseOutPut
    {
        /// <summary>
        /// 数据集合
        /// </summary>
        [JsonIgnore]
        public PagedList<T> Rows { get; set; }

        /// <summary>
        /// 总条数，查询条件能查询到数据库包含的总数据条数。不一定等于返回集合的总条数
        /// </summary>
        [JsonIgnore]
        public int Total => Rows.TotalCount;


        /// <summary>
        /// 返回数据
        /// </summary>
        public new object Data => new { Rows, Total = Rows.TotalCount };
    }

    /// <summary>
    /// 返回集合结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListReponseOutPut<T> : ReponseOutPut
    {
        /// <summary>
        /// 数据集合
        /// </summary>
        [JsonIgnore]
        public List<T> Rows { get; set; } = new List<T>();

        /// <summary>
        /// 总条数=集合总条数
        /// </summary>
        [JsonIgnore]
        public int Total => Rows.Count();

        /// <summary>
        /// 返回数据
        /// </summary>
        public new object Data => new { Rows, Total = Rows.Count() };

    }

    /// <summary>
    /// 返回标识
    /// </summary>
    public enum ReutnStatus
    {
        /// <summary>
        /// 失败
        /// </summary>
        Error = 0,

        /// <summary>
        /// 成功
        /// </summary>
        Success = 1
    }
}
