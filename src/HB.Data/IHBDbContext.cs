using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HB.Data
{
    public  interface IHBDbContext
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        IDbConnection Connection { get; }
    }
}
