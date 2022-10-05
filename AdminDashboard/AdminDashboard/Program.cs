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
        CreateWebHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateWebHostBuilder(string[] args)
    {
        // temp initial logging
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        var builder = Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webHostBuilder
                => webHostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                        _ = config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                            .AddJsonFile("appsettings.json", false, true))
                    .UseStartup<Startup>())
            .UseSerilog(
                (hostingContext, loggerConfig) =>
                    loggerConfig
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext(),
                writeToProviders: true);
        return builder;
    }
}