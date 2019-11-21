using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HB.Admin.Models
{
    public class AdminAuthorizationRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Policy { get; set; }
    }
}
