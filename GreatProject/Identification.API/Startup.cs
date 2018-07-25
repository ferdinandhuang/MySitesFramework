using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Framework.Core.DbContextCore.CodeFirst;
using Framework.Core.IoC;
using Framework.Core.Extensions;
using Framework.Core.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;

namespace Identification.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //var redis = Configuration.GetValue<string>("test");

            return InitIoC(services);
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
                app.UseHsts();
            }

            // swagger 
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/{documentName}/swagger.json";
            })
            .UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(string.Format("/api/{0}/swagger.json", Configuration["Service:Swagger:DocName"])
                    , string.Format("{0} {1}", Configuration["Service:Name"], Configuration["Service:Swagger:Version"]));
            });

            //app.UseHttpsRedirection();
            app.UseMvc();
        }


        private IServiceProvider InitIoC(IServiceCollection services)
        {
            #region Swagger
            // IoC - ISwaggerProvider
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Configuration["Service:Swagger:DocName"], new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = Configuration["Service:Swagger:Title"],
                    Version = Configuration["Service:Swagger:Version"],
                    Description = Configuration["Service:Swagger:Description"],
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact
                    {
                        Name = Configuration["Service:Swagger:Contacter"],
                        Email = Configuration["Service:Swagger:ContacterEmail"]
                    }
                });
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, Configuration["Service:Swagger:XmlFile"]);
                c.IncludeXmlComments(xmlPath);
            });
            #endregion

            #region Redis

            var redisConnectionString = Configuration.GetConnectionString("Redis");
            //启用Redis
            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = redisConnectionString;//redis连接字符串
                option.InstanceName = "mysites";//Redis实例名称
            });
            //全局设置Redis缓存有效时间为5分钟。
            services.Configure<DistributedCacheEntryOptions>(option =>
                option.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5));

            #endregion

            #region MemoryCache

            //启用MemoryCache
            services.AddMemoryCache();

            #endregion

            #region 配置DbContextOption

            //database connectionstring
            var dbConnectionString = Configuration.GetConnectionString("DataBase");//连接字符串
            //配置DbContextOption
            services.Configure<DbContextOption>(options =>
            {
                options.ConnectionString = dbConnectionString;
                options.ModelAssemblyName = "MySites.DataModels";
            });

            #endregion

            #region 注入Database
            var dataBaseType = Configuration.GetConnectionString("DataBaseType");//数据库类型
            if (dataBaseType == "MsSqlServer")
            {
                services.AddSingleton<IDbContextCore, SqlServerDbContext>();//注入EF上下文
            }
            else if (dataBaseType == "MySql")
            {
                services.AddSingleton<IDbContextCore, MySqlDbContext>();//注入EF上下文
            }
            else
            {
                throw new Exception("未能找到相应的数据库连接！");
            }
            #endregion

            #region 各种注入
            services.AddSingleton(Configuration)//注入Configuration，ConfigHelper要用
                //.AddScopedAssembly("Auth.IRespositories", "Auth.Respositories")//注入服务
                //.AddScopedAssembly("Auth.IServices", "Auth.Services")//注入服务
                ;
            #endregion

            services.AddOptions();

            return AspectCoreContainer.BuildServiceProvider(services);//接入AspectCore.Injector
        }
    }
}
