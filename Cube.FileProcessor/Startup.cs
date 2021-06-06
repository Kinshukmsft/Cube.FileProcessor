using System;
using System.Collections.Generic;
using System.Text;
using Cube.FileProcessor;
using Cube.FileProcessor.Configuration;
using Cube.FileProcessor.Services.FileShareService;
using Cube.FileProcessor.Services.SearchService;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Cube.FileProcessor
{
   public class Startup: FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddCpaOptions();
            builder.Services.AddScoped<IFileShareService, FileShareService>();
            builder.Services.AddScoped<ISearchService, SearchService>();
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();

             builder.ConfigurationBuilder
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, $"appsettings.{context.EnvironmentName}.json"), optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();
        }

    }
}
