using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace HB.Data
{
   public static class HBDbContextExtensions
    {
        public static IServiceCollection AddHBDbContext(
           this IServiceCollection services,
           Action<HBDbContextOptionBuilder> options = null)
        {
            HBDbContextOptionBuilder option = new HBDbContextOptionBuilder();
            options?.Invoke(option);
            services.AddSingleton(option);
            return services;
        }

        public static HBDbContextOptionBuilder UseMySql(this HBDbContextOptionBuilder builder, string connectionString)
        {
            builder.DataBaseType = DataBaseType.MySql;
            builder.ConnectionString = connectionString;
            return builder;
        }

        public static HBDbContextOptionBuilder UseMsSql(this HBDbContextOptionBuilder builder,string connectionString)
        {
            builder.DataBaseType = DataBaseType.MsSql;
            builder.ConnectionString = connectionString;
            return builder;
        }

        public static HBDbContextOptionBuilder UseOracle(this HBDbContextOptionBuilder builder, string connectionString)
        {
            builder.DataBaseType = DataBaseType.Oracle;
            builder.ConnectionString = connectionString;
            return builder;
        }
    }
}
