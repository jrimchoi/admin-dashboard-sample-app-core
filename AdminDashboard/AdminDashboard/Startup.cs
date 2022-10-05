using AdminDashboard.Cmm.Filters;
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
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.WebEncoders;
using Quickwire;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace AdminDashboard
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            HostingEnvironment = env;
        }

        public IWebHostEnvironment HostingEnvironment { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Sets the root-relative application path to '<ServerRoot>/app'
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UsePathBase("/");
            app.UseHttpsRedirection();
            app.UseStaticFiles();  // ### using... wwwroot  static files 

            app.UseRouting();

            // session #2 
            app.UseSession();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
            });
            app.Build();

        }
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddCors();
            services.AddAntiforgery();

            // Add services to the container.
            var provider = services.BuildServiceProvider();

            // SeriLog  : SeriLog.AspdotNet
            var configuration4seri = new ConfigurationBuilder()
                // Read from your appsettings.json.
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json")
                // Read from your secrets.
                .AddUserSecrets<Program>(optional: true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration4seri)
                .CreateLogger();


            var configuration = provider.GetService<IConfiguration>();
            ConfigUtil.Initialize(configuration);
            // ### ajax json, application/x-www-form-urlencoded; charset=utf-8  -->  many rows allowed ..... ### 
            services.Configure<FormOptions>(x =>
            {
                x.ValueCountLimit = int.MaxValue;
            });

            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All); // 한글이 인코딩되는 문제 해결  filter --> message --> alert("&#x ~~~~")
            });


            // services.AddControllersWithViews();
            services.AddControllersWithViews().AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;  // turn off camelCasing in JSON response ASP.NET Core 
            });

            // filters 
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(SessionCheckActionFilterAttribute));  // session check 
                options.Filters.Add(typeof(TransactionalAttribute));  // rollback 
                options.Filters.Add(typeof(IntegratedExceptionFilterAttribute));  // intergrated exception filter 
            });

            //session #1
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                //options.IdleTimeout = TimeSpan.FromSeconds(600);//60초
                options.IdleTimeout = TimeSpan.FromMinutes(30);//30분
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddTransient<IStartupFilter, AddVirtualDirectoryToBasePathStartupFilter>();
            // DI  : AddScoped / AddTransient / AddSingleton
            //services.AddScoped<IDapper, OracleDapper>();  // AddSingleton ?   <--------- eroor 확인할것...
            services.AddScoped<OracleDapper>();  // AddSingleton ? 
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // service, repository, dapperSql 
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<CommonRepository>();
            services.AddScoped<CodeRepository>();

            services.AddScoped<ICodeMngService, CodeMngService>();
            services.AddScoped<CodeMngRepository>();

            services.AddScoped<ISysMngService, SysMngService>();   // 시스템관리 : menu, role ...
            services.AddScoped<SysMngRepository>();


            services.AddScoped<IDashBoardService, DashBoardService>();
            services.AddScoped<DashBoardRepository>();


            // for QuickWire DI 
            services.AddControllers().AddControllersAsServices();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.ScanCurrentAssembly();

            services.AddMvc(options =>
            {
                options.InputFormatters.Insert(0, new RawStringBodyInputFormatter());
            });
            AppHttpContext.Services = (IServiceProvider)services;
            // for get session without controuctor 

        }
    }
}
