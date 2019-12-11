using Dapper;
using HB.Data;
using HB.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HB.Api.Services
{
    public class MebUserService : BaseService<MebUser>, IMebUserService
    {
        public MebUserService(IHBDbContext dbContext)
              : base(dbContext)
        {

        }

        /// <summary>
        /// 根据用户名账号信息
        /// </summary>
        /// <param name="userName">登录名</param>
        /// <returns></returns>
        public MebUser GetUserByUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return null;

            var param = new DynamicParameters();
            param.Add(name: "@UserName", value: userName, dbType: DbType.String, direction: ParameterDirection.Input);

            //用户信息
            string sql = "SELECT * FROM Meb_User WHERE UserName=@UserName";

            var user = Connection.QueryFirstOrDefault<MebUser>(sql, param);

            return user;
        }

    }
}
