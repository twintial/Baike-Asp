using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CLRForBaike;

namespace BaikeAsp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(ResourcePath.ROOT);
            Console.WriteLine(ResourcePath.IMG);
            Console.WriteLine(ResourcePath.TEMP);
            Console.WriteLine(ResourcePath.USER_ICON);
            Console.WriteLine(ResourcePath.VIDEO);
            Console.WriteLine(ResourcePath.VIDEO_COVER);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseKestrel(options => {
                        // 接触文件上传限制
                        options.Limits.MaxRequestBodySize = null;
                    });
                });
    }
}
