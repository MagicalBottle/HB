using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;

namespace HB.Api
{
    public class Program
    {
        //https://github.com/serilog/serilog-docker/blob/master/web-sample/src/Program.cs
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
       .AddEnvironmentVariables()
       .Build();

        public static void Main(string[] args)
        {
            //https://stackoverflow.com/questions/46516359/filter-serilog-logs-to-different-sinks-depending-on-context-source
            Log.Logger = new LoggerConfiguration()
                   .MinimumLevel.Information()
                   .WriteTo.ColoredConsole()
                   .Enrich.FromLogContext()
                   //.Enrich.With(new LogTypeEnricher())
                   .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.RollingFile(pathFormat: @"Logs\Debug-{Date}.log", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}"))
                   .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.RollingFile(pathFormat: @"Logs\Info-{Date}.log", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}"))
                   .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.RollingFile(pathFormat: @"Logs\Warning-{Date}.log", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}"))
                   .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.RollingFile(pathFormat: @"Logs\Error-{Date}.log", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}"))
                   .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.RollingFile(pathFormat: @"Logs\Fatal-{Date}.log", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}"))
                   .WriteTo.File($"{AppDomain.CurrentDomain.BaseDirectory}\\.log", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:HH:mm} $$$ {Level} $$$ {SourceContext:l} $$$ {Message} $$$ {Exception} $$$&end {NewLine}")
                   .CreateLogger();

            #region 配合使用，可以按实际业务，分别存储日记文件，例如订单，接口
            /*
             //https://www.cnblogs.com/mq0036/p/8479956.html
             //.WriteTo.Logger(lc => lc
             //          .Filter.ByIncludingOnly(Matching.FromSource("HB.Api"))
             //          .Filter.ByIncludingOnly(Matching.WithProperty<string>("LogType", p => p == "Api"))
             //          .WriteTo.RollingFile(pathFormat: @"logs/api-{Date}.log", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}"))

            //using (LogContext.PushProperty("LogType", "Order"))
            //{
            //    _logger.LogInformation(user.UserName);
            //}
            //using (LogContext.PushProperty("LogType", "Api"))
            //{
            //    _logger.LogInformation(user.Password);
            //}
            */
            #endregion

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();

     

      private  class LogTypeEnricher : ILogEventEnricher
        {
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                        "LogType", new string[] { "Order","Api" },true));
            }
        }


    }
}
