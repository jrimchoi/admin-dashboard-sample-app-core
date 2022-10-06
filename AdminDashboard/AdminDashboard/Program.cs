using AdminDashboard;
using AdminDashboard.Data;
using AdminDashboard.Data.Models;
using DSELN.Cmm.DataBase;
using DSELN.Cmm.Filters;
using DSELN.Cmm.Helper;
using DSELN.Cmm.Utils;
using DSELN.Repository.CodeMng;
using DSELN.Repository.Common;
using DSELN.Repository.DashBoard;
using DSELN.Repository.SysMng;
using DSELN.Service.CodeMng;
using DSELN.Service.Common;
using DSELN.Service.DashBoard;
using DSELN.Service.SysMng;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders;
using Quickwire;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;

public class Program
{
    public static void Main(string[] args)
    {
        var _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                // Read from your appsettings.json.
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json")
                // Read from your secrets.
                .AddUserSecrets<Program>(optional: true)
                .AddEnvironmentVariables()
                .Build();

        Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(_config)
                    .CreateLogger();
        CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseSerilog()
            .UseStartup<Startup>();

}