﻿using Framework.Core.IoC;
using Microsoft.Extensions.Configuration;

namespace Framework.Core.Helpers
{
    public class ConfigHelper
    {
        private static IConfiguration configuration = AspectCoreContainer.Resolve<IConfiguration>();

        public static IConfigurationSection GetSection(string key)
        {
            return configuration.GetSection(key);
        }

        public static string GetConfigurationValue(string key)
        {
            return configuration[key];
        }

        public static string GetConfigurationValue(string section, string key)
        {
            return GetSection(section)?[key];
        }

        public static string GetConnectionString(string key)
        {
            return configuration.GetConnectionString(key);
        }
    }
}
