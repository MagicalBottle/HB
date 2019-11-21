using Dapper;
using HB.Data;
using HB.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HB.Services
{
    public class SysAdminService : BaseService<SysAdmin>, ISysAdminService
    {
        public SysAdminService(IHBDbContext dbContext)
            : base(dbContext)
        {

        }

        /// <summary>
        ///  更新一个账号
        /// </summary>
        /// <param name="admin">账号实体</param>
        /// <param name="roleIds">账号分配了的角色id</param>
        /// <returns></returns>
        public int UpdateAdmin(SysAdmin admin, List<int> roleIds)
        {
            int result = -1;
            result = BeginTransaction(() =>
            {
                string sql = "UPDATE Sys_Admin SET UserName=UserName WHERE ID=1";
                result = Connection.Execute(sql);
                sql = "SELECT *FROM SYS_ADMIN";
                var adminRoles = GetAll();
            });
            return result;
        }
    }
}
