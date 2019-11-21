using HB.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HB.Services
{
    public interface ISysAdminService : IBaseService<SysAdmin>
    {

        /// <summary>
        ///  更新一个账号
        /// </summary>
        /// <param name="admin">账号实体</param>
        /// <param name="roleIds">账号分配了的角色id</param>
        /// <returns></returns>
        int UpdateAdmin(SysAdmin admin, List<int> roleIds);
    }
}
