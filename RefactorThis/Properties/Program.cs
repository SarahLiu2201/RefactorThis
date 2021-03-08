using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace RefactorThis
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
           .AddCommandLine(args)
           .Build();

            try
            {
                var myHost = new WebHostBuilder()
                    .UseConfiguration(config)                    
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseKestrel()                    
                    .UseIISIntegration()
                    .UseStartup<Startup>()
                    ;
                return myHost;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }
    }
}