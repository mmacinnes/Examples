using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Adapted from
// Online Multiplayer Word Game With Blazor and SignalR on .NetCore
// Bora Kaşmer
// https://borakasmer.medium.com/word-game-with-blazor-and-signalr-on-netcore-e14e125233f2
// https://github.com/borakasmer/BlazorWordGame.git

namespace blazorWords
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://localhost:5923","https://localhost:7923");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
