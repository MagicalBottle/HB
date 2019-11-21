using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HB.Admin.Services
{
    public interface IPermissionService
    {
        /// <summary>
        /// 判定权限
        /// </summary>
        /// <param name="name">权限名称</param>
        /// <returns>true 有此权限；false 无此权限</returns>
        bool Authorize(string name);
    }
}
