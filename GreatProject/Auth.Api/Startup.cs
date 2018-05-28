using Framework.Core.DbContextCore.CodeFirst;
using Framework.Core.Extensions;
using Framework.Core.IoC;
using Framework.Core.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Auth.Api
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
            var dangguiPrivateCert = new X509Certificate2(Path.Combine(".", "dangguiPrivate.pfx"), "Assassin016");
            var dangguiPublicCert = new X509Certificate2(Path.Combine(".", "dangguiPublic.cer"), "Assassin016");

            services.AddIdentityServer()
                .AddSigningCredential(dangguiPrivateCert)
                .AddInMemoryApiResources(Config.GetResources())
                .AddInMemoryClients(Config.GetClients())
                .AddValidationKey(dangguiPublicCert)
                .AddJwtBearerClientAuthentication()
                .AddValidationKey(dangguiPublicCert)
                .AddValidators()
                .AddInMemoryCaching()
                .AddJwtBearerClientAuthentication()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()    //账号密码认证
                ;


            InitIoC(services);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            app.UseMvc();
        }


        private IServiceProvider InitIoC(IServiceCollection services)
        {
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
                .AddScopedAssembly("Auth.IRespositories", "Auth.Respositories")//注入服务
                .AddScopedAssembly("Auth.IServices", "Auth.Services");//注入服务

            #endregion

            services.AddOptions();

            return AspectCoreContainer.BuildServiceProvider(services);//接入AspectCore.Injector
        }
    }
}
