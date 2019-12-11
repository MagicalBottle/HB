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
    public class SysApiUserService : BaseService<SysApiUser>, ISysApiUserService
    {

        public SysApiUserService(IHBDbContext dbContext)
              : base(dbContext)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public SysApiUser GetUserByAppId(int appId)
        {
            if (appId<=0)
            {
                return null;
            }

            var param = new DynamicParameters();
            param.Add(name: "@AppId", value: appId, dbType: DbType.Int32, direction: ParameterDirection.Input);

            //用户信息
            string sql = "SELECT * FROM SYS_APIUser WHERE AppId=@AppId";

            var user = Connection.QueryFirstOrDefault<SysApiUser>(sql, param);

            return user;
        }
    }
}
