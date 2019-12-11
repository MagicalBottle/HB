using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HB.Api.Models
{
    public class ApiResult
    {

        private const int STATUS_CODE_OK = (int)HttpStatusCode.OK;
        private const string CONTENT_TYPE = "application/json;charset=utf-8";
        private readonly static Dictionary<string, string> Message = new Dictionary<string, string>()
        {
            { "Success","成功"},
            { "Error","失败"},
            {"Error_Server", "服务器异常，请稍后重试"},
            {"Error_Param", "参数校验失败："},
            {"Error_Signature", "验签失败"},
            {"Error_AccessToken", "AccessToken校验失败"}
        };

        /// <summary>
        /// 获取成功
        /// </summary>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
        public static ContentResult Success(object data = null)
        {
            return GetContent(STATUS_CODE_OK, CONTENT_TYPE, "Success", Message["Success"], data);
        }

        /// <summary>
        /// 获取失败
        /// </summary>
        /// <param name="message">错误提示信息</param>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
        public static ContentResult Error(string message = null, object data = null)
        {

            return GetContent(STATUS_CODE_OK, CONTENT_TYPE, "Error", Message["Error"] + (message == null ? "" : "," + message), data);
        }

        /// <summary>
        /// 验签失败
        /// </summary>
        /// <param name="message">错误提示信息</param>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
        public static ContentResult Error_Signature(string message = null, object data = null)
        {
            return GetContent(STATUS_CODE_OK, CONTENT_TYPE, "Error_Signature", Message["Error_Signature"] + (message == null ? "" : "," + message), data);
        }


        /// <summary>
        /// 验参失败
        /// </summary>
        /// <param name="message">错误提示信息</param>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
        public static ContentResult Error_Param(string message = null, object data = null)
        {
            return GetContent(STATUS_CODE_OK, CONTENT_TYPE, "Error_Param", Message["Error_Param"] + (message == null ? "" : "," + message), data);
        }

        /// <summary>
        /// 异常返回
        /// </summary>
        /// <param name="message">错误提示信息</param>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
        public static ContentResult Error_Server(string message = null, object data = null)
        {
            return GetContent(STATUS_CODE_OK, CONTENT_TYPE, "Error_Server", Message["Error_Server"] + (message == null ? "" : "," + message), data);
        }


        /// <summary>
        /// AccessToken校验失败
        /// </summary>
        /// <param name="message">错误提示信息</param>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
        public static ContentResult ErrorAccessToken(string message = null, object data = null)
        {
            return GetContent(STATUS_CODE_OK, CONTENT_TYPE, "Error_AccessToken", Message["Error_AccessToken"] + (message == null ? "" : "," + message), data);
        }

        /// <summary>
        /// 获取失败
        /// </summary>
        /// <param name="code">错误码</param>
        /// <param name="message">错误提示信息</param>
        /// <param name="data">附带数据</param>
        /// <returns></returns>
        public static ContentResult Error(string code, string message, object data = null)
        {

            return GetContent(STATUS_CODE_OK, CONTENT_TYPE, code, message, data);
        }


        /// <summary>
        /// 接口响应内容
        /// </summary>
        /// <param name="statusCode">状态码，默认200</param>
        /// <param name="contentType">返回类型，默认application/json;charset=utf-8</param>
        /// <param name="code">逻辑响应状态码</param>
        /// <param name="message">逻辑响应提示信息</param>
        /// <param name="data">逻辑响应返回数据</param>
        /// <returns></returns>
        private static ContentResult GetContent(int statusCode, string contentType, string code, string message, object data)
        {
            var content = new
            {
                Code = code,
                Message = message,
                Data = data ?? new { }
            };
            return new ContentResult
            {
                StatusCode = statusCode,
                ContentType = contentType,
                Content = JsonConvert.SerializeObject(content).ToString()
            };
        }

    }

}
