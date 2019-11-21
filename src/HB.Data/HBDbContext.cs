using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HB.Data
{
   public class HBDbContext: IHBDbContext
    {
        HBDbContextOptionBuilder _builder;
        public HBDbContext(HBDbContextOptionBuilder builder)
        {
            _builder = builder;
        }
        private IDbConnection _connection = null;
        public IDbConnection Connection
        {            
            get {
                if (_connection != null)
                {
                    return _connection;
                }
                switch(_builder.DataBaseType)
                {
                    //case DataBaseType.MsSql:
                    //    return new MySqlConnection(_builder.ConnectionString);
                    case DataBaseType.MySql:
                         _connection=new MySqlConnection(_builder.ConnectionString);
                        break;
                    default:
                       _connection= new MySqlConnection(_builder.ConnectionString);
                        break;
                }
                return _connection;
            }
        }
    }
}
