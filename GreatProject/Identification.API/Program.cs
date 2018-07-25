using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Com.Ctrip.Framework.Apollo;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Identification.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: true)
                .Build();

            return WebHost.CreateDefaultBuilder(args)
                    .UseUrls(config["server.urls"])
                    //.ConfigureAppConfiguration((hostingContext, builder) => {
                    //    builder
                    //    .AddApollo(builder.Build().GetSection("apollo"))
                    //    .AddDefault()
                    //    .AddNamespace("Ferdinand.Common")
                    //    ;
                    //})
                    //.UseConfiguration(config)
                    .UseStartup<Startup>();
        }
    }
}
