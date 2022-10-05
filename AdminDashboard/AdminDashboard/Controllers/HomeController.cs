using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using Oracle.ManagedDataAccess.Client;
using DSELN.Models;
using Quickwire.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System;

namespace DSELN.Controllers
{

    [RegisterService(ServiceLifetime.Transient)]
    public class HomeController : Controller      // localhost:port/home
    {
        // .net core DI 
        private readonly IWebHostEnvironment webHostEnvironment;



        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;   // DI  : 생성자에서 파라미터로 받아서 자동적으로 생성
        }

        // localhost:port/home/Index
        public IActionResult Index()  // IActionResult --> view 
        {

            return View("");
        }

        // localhost:port/home/Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}