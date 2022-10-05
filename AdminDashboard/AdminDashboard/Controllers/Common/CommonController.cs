using Microsoft.AspNetCore.Mvc;
using DSELN.Cmm.Filters;
using DSELN.Service.Common;
using DSELN.Models;
using DSELN.Models.CodeMng;
using Newtonsoft.Json;
using Serilog;
using System.Text.RegularExpressions;
using DSELN.Service.CodeMng;
using DSELN.Models.Common;
using System.Collections.Generic;

namespace DSELN.Controllers.Common
{
    public class CommonController : BaseController
    {
        private readonly ICommonService _cmmService;

        public CommonController(ICommonService cmmService)
        {
            _cmmService = cmmService;
        }

        /****************************************************************
        // Index page view 
        ****************************************************************/
        //[ValidateAntiForgeryToken] // xss 
        //[ValidateModel]                  // validation interceptor 
        [HttpGet, HttpPost]
        [Route("Home/Index")]
        public IActionResult Index(BaseSearchModel model)
        {
            //List<Dictionary<string, string>> list = _cmmService.GetUserMenu(model); // 사용자별 메뉴담기  --> 로그인시 메뉴리스트를 세션정보에 담아둔다. 
            //ViewBag._USER_MENU = list;

            return View("../Home/Index");
        }

        /****************************************************************
        // 코드성 항목 가져오기 
        ****************************************************************/
        //[ValidateAntiForgeryToken] // xss 
        //[ValidateModel]                  // validation interceptor 
        [HttpGet, HttpPost]
        public JsonResult GetCodeHelp(CodeCondition model)
        {
            List<Dictionary<string, string>> list = _cmmService.GetCodeHelp(model);
            return Json(list); // for dataSource read
        }

        /****************************************************************
        // 사용자별 메뉴 가져오기  
        ****************************************************************/
        [Route("Common/GetUserMenu")]
        [HttpGet, HttpPost]
        public IActionResult GetUserMenu(BaseSearchModel model)
        {
            List<Dictionary<string, string>> list = _cmmService.GetUserMenu(model);
            return Json(list);
        }
    }
}
