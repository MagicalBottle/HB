using HB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HB.Admin.Services
{

    public class PermissionService : IPermissionService
    {
        private readonly IWorkContextService _workContext;
        private readonly IMenuService _menuService;
        private readonly ICacheManagerService _cache;

        public PermissionService(IWorkContextService workContext,
            IMenuService menuService,
        ICacheManagerService cache)
        {
            _workContext = workContext;
            _menuService = menuService;
            _cache = cache;
        }

        /// <summary>
        /// 判定权限
        /// </summary>
        /// <param name="functionSystermName">权限名称</param>
        /// <returns>true 有此权限；false 无此权限</returns>
        public bool Authorize(string functionSystermName)
        {
            if (string.IsNullOrWhiteSpace(functionSystermName))
            {
                return false;
            }
            return this.Authorize(functionSystermName, _workContext.Admin);
        }
        /// <summary>
        ///  判定权限
        /// </summary>
        /// <param name="functionSystermName">权限名称</param>
        /// <param name="admin">当前用户</param>
        /// <returns>true 有此权限；false 无此权限</returns>
        public bool Authorize(string functionSystermName, SysAdmin admin)
        {
            if (string.IsNullOrWhiteSpace(functionSystermName))
            {
                return false;
            }
            if (admin == null || admin.Menus == null)
            {
                return false;
            }
            foreach (var f in admin.Menus.Where(m => m.MenuType == MenuType.Function))
            {
                if (functionSystermName.Equals(f.MenuSystermName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
