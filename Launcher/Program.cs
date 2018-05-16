using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quick.Properties.Utils;

namespace Launcher
{
    public class Program
    {
        public static IDictionary<string, string> GlobalProperties;
        public static void Main(string[] args)
        {
            Console.WriteLine("Environment.CurrentDirectory: " + Environment.CurrentDirectory);
            Console.WriteLine("Directory.GetCurrentDirectory(): " + Directory.GetCurrentDirectory());

            //加载全局配置
            GlobalProperties = PropertyUtils.Load("include=*.properties", Environment.CurrentDirectory);
            BuildWebHost(args, GlobalProperties["server.urls"].Split(';')).Run();
        }

        public static IWebHost BuildWebHost(string[] args, string[] urls) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls(urls)
                .UseStartup<Startup>()
                .Build();
    }
}
