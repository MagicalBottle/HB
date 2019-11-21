using System;
using System.Collections.Generic;
using System.Text;

namespace HB.Data
{
   public class HBDbContextOptionBuilder
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType { get; set; }
    }

    public enum DataBaseType
    {
        MySql=0,
        MsSql=1,
        Oracle=2
    }
}
