using AutoMapper;
using EasyCaching.Core;
using EasyCaching.InMemory;
using FluentValidation;
using FluentValidation.AspNetCore;
using HB.Api.Configuration;
using HB.Api.Filters;
using HB.Api.Models;
using HB.Api.Services;
using HB.Data;
using LogDashboard;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HB.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(option => option.AddPolicy("cors", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().AllowAnyOrigin()));
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddLogDashboard((option)=> { option.RootPath = AppDomain.CurrentDomain.BaseDirectory;option.FileFieldDelimiter = "$$$";option.FileEndDelimiter = "&end"; });

            HBConfiguration hbConfiguration = Configuration.GetSection("HBConfiguration").Get<HBConfiguration>();
            services.TryAddSingleton(hbConfiguration);

            services.AddEasyCaching(option => option.UseInMemory("hb_memory_cache"));
            services.AddScoped<ICacheManagerService, MemoryCacheManagerService>();//封装一层，缓存用的是EasyCaching
            #region JWT
            ////源码分析https://www.cnblogs.com/lex-wu/p/10512424.html
            //// https://github.com/aspnet/Security/issues/1310
            ////就是身份验证失败也会继续下一个中间件执行，所以通常不需要重写这里的方法
            ////那么只能放到权限验证中判断
            ////401等不正常访问处理 http://www.voidcn.com/article/p-ckgqcgds-btn.html
            ////https://blog.csdn.net/jasonsong2008/article/details/89226705
            ////16，JwtBearerEvents 中的OnAuthenticationFailed 设置 StatusCode得到Kestrell抛出StatusCode cannot be set because the response has already started 异常。
            ////这也是因为 [Authority] 属性会调用Challenge，那里会设置statuscode，和 error head。 所以从返回的header中读取错误信息即可。OnAuthenticationFailed应该永远不使用。
            ////https://www.jianshu.com/p/f9d9d51b489b
            ////http://www.voidcn.com/article/p-ckgqcgds-btn.html  UseStatusCodePages
            ////https://www.cnblogs.com/CreateMyself/category/735077.html 参考博客
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = hbConfiguration.AuthenticateScheme;
            //    options.DefaultChallengeScheme = hbConfiguration.AuthenticateScheme;

            //}).AddJwtBearer(hbConfiguration.AuthenticateScheme,
            //      (jwtBearerOptions) =>
            //      {

            //          //   if (Options.SaveToken)
            //          //{
            //          //  tokenValidatedContext.Properties.StoreTokens(new[]
            //          //  {
            //          //        new AuthenticationToken { Name = "access_token", Value = token }
            //          //    });
            //          //}
            //          //jwtBearerOptions.SaveToken = true;
            //          jwtBearerOptions.Events = new JwtBearerEvents
            //          {

            //              //源码 https://github.com/aspnet/Security/blob/master/src/Microsoft.AspNetCore.Authentication.JwtBearer
            //              //实例 https://github.com/RainingNight/AspNetCoreSample/blob/master/src/Functional/Authentication/JwtBearerSample/Startup.cs
            //              //token 默认从  string authorization = Request.Headers["Authorization"];  "Bearer "获取
            //              //OnMessageReceived可以控制token从URL中获取
            //              OnMessageReceived = (context) =>
            //              {
            //                  //context.Token = context.Request.Query["access_token"];
            //                  context.Token = context.Request.Query["token"];
            //                  return Task.CompletedTask;
            //              }
            //              ////此处为权限验证失败后（401）触发的事件，因为只处理401，所以不在这里处理，用UseStatusCodePages通已处理非200的
            //              //,OnChallenge = (context) =>
            //              //{
            //              //    return Task.CompletedTask;
            //              //}
            //              //////此处为出现异常后触发的事件
            //              //,OnAuthenticationFailed = (context) =>
            //              //{
            //              //    return Task.CompletedTask;
            //              //}
            //          };
            //          jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
            //          {
            //              ValidateIssuerSigningKey = true,
            //              IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(hbConfiguration.Key)),//秘钥
            //              ValidateIssuer = true,
            //              ValidIssuer = hbConfiguration.Issuer,
            //              ValidateAudience = true,
            //              ValidAudience = hbConfiguration.Audience

            //              //ValidateLifetime = true,
            //              //ClockSkew = TimeSpan.FromMinutes(5)                      
            //          };
            //});
            #endregion

            //使用AddAuthorization不好控制，所以使用Filters
            //https://docs.microsoft.com/zh-cn/aspnet/core/mvc/controllers/filters?view=aspnetcore-3.1#dependency-injection
            //services.AddSingleton<IAuthorizationHandler, ApiAuthorizationHandler>();
            //services.AddAuthorization(options =>
            //   options.AddPolicy("Something",
            //   policy => policy.AddRequirements(new ApiAuthorizationRequirement())));

            services.AddScoped<IHBDbContext, HBDbContext>();
            //services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddScoped<IWorkContextService, WorkContextService>();
            services.AddScoped<IMebUserService, MebUserService>();
            services.AddScoped<ISysApiUserService, SysApiUserService>();
            services.AddScoped<IAuthenticationService, JwtAuthenticationService>();
            services.AddHBDbContext(options => options.UseMySql(Configuration.GetConnectionString("MySqlConnection")));
            var mvcBuilder = services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidateSignatureFilterAttribute));       // An instance
                options.Filters.Add(typeof(ValidateAccessTokenFilterAttribute));       // An instance
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            mvcBuilder.AddFluentValidation(option =>
            {
                //https://docs.microsoft.com/zh-cn/aspnet/core/mvc/advanced/app-parts?view=aspnetcore-2.2
                var assemblies = mvcBuilder.PartManager.ApplicationParts
                 .OfType<AssemblyPart>()
                 .Where(part => part.Name.StartsWith("HB", StringComparison.InvariantCultureIgnoreCase))
                 .Select(part => part.Assembly);
                option.RegisterValidatorsFromAssemblies(assemblies);//从程序集中加载继承AbstractValidator<T> 的 public类，且是非 abstract的
                option.RunDefaultMvcValidationAfterFluentValidationExecutes = false;//去掉默认验证，只运行FluentValidation验证
                option.ImplicitlyValidateChildProperties = true;//支持子属性是类的验证      
            });
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure; //开启全局设置，第一个验证失败立即停止验证

            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            IdentityModelEventSource.ShowPII = true;
            //处理非200的请求
            app.UseStatusCodePages((context) =>
            {
                var response = context.HttpContext.Response;
                if (response.StatusCode != (int)HttpStatusCode.OK)
                {
                    var result = new
                    {
                        Code = "Error",
                        Message = "状态码为:" + response.StatusCode,
                        Data = new { StatusCode = response.StatusCode }
                    };

                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Clear();
                    response.ContentType = "application/json;charset=utf-8";
                    //response.ContentType = "text/html;charset=utf-8"; 
                    response.WriteAsync(JsonConvert.SerializeObject(result).ToString(), Encoding.UTF8);
                }
                return Task.CompletedTask;
            });
            // Write streamlined request completion events, instead of the more verbose ones from the framework.
            // To use the default framework request logging instead, remove this line and set the "Microsoft"
            // level in appsettings.json to "Information".
            app.UseSerilogRequestLogging();

            app.UseCors("cors");
            //app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseLogDashboard();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "default",
                   template: "api/{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
