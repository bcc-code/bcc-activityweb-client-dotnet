using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Tests
{
    public class ConfigHelper
    {
        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets("0504daaf-b91e-4576-889f-5c0e0ccd0a88")
                .AddEnvironmentVariables()
                .Build();
        }

        public static ActivityWebOptions GetApplicationConfiguration(string outputPath)
        {
            var configuration = new ActivityWebOptions();

            var configRoot = GetIConfigurationRoot(outputPath);

            configRoot
                .GetSection("ActivityWeb")
                .Bind(configuration);

            return configuration;
        }
    }
}
