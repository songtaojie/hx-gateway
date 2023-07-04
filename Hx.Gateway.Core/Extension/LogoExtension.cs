using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Core
{
    /// <summary>
    /// logo显示
    /// </summary>
    public static class LogoExtension
    {
        /// <summary>
        /// 添加logo显示
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddLogoDisplay(this IServiceCollection services)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@" _     _          _     _ 
| |   | |        \ \  / / 
| |___| |    _    \ \/ / 
| |___| |   (_)    \/\/  
| |   | |          /\/\ 
| |   | |         / /\ \ 
|_|   |_|        /_/  \_\
");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"源码地址: https://gitee.com/songtaojie/hx-gateway");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"网关服务！");
            return services;
        }
    }
}
