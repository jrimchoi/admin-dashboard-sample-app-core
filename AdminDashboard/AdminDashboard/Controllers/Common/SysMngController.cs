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
using DSELN.Service.SysMng;
using DSELN.Cmm.Utils;
using System.Collections.Generic;

namespace DSELN.Controllers.SysMng
{
    [AuthorizeFilter(Role.ADMIN)]
    public class SysMngController : BaseController
    {
        private readonly ICommonService _cmmService;
        private readonly ISysMngService _service;

        public SysMngController(ICommonService cmmService, ISysMngService service)
        {
            _cmmService = cmmService;
            _service = service;
        }

        /****************************************************************
        // 메뉴등록 
        ****************************************************************/
        // 메뉴등록  page view
        [HttpGet, HttpPost]
        [Route("SysMng/Menu")]
        public IActionResult Menu(MenuSearch model)
        {
            // 1010. 코드담기 
            base.getComCodeList(new string[] { "LANG_CD", "ROLE_CD" }, _cmmService);

            // 2010. 페이지 view  
            return View("Views/SysMng/Menu.cshtml", model);
        }

        // 메뉴등록  조회 
        [ValidateAntiForgeryToken] // xss 
        [ValidateModel]                  // validation interceptor 
        [HttpGet, HttpPost]
        [Route("SysMng/GetMenuList")]
        public IActionResult GetMenuList(MenuSearch model)
        {
            List<Menu> list = _service.GetMenuList(model);
            return Json(this.returnSearchOK(list, model));
        }

        // 메뉴등록  저장  
        [ValidateAntiForgeryToken] // xss 
        [ValidateModel]                  // validation interceptor 
        [HttpPost]
        [Route("SysMng/SaveMenuList")]
        public JsonResult SaveMenuList(List<Menu> model)
        {
            _service.SaveMenuList(model);
            return Json(this.returnOK(model));
        }

        // 메뉴등록  삭제   
        [ValidateAntiForgeryToken] // xss 
        [ValidateModel]                  // validation interceptor 
        [HttpPost]
        [Route("SysMng/DeleteMenuList")]
        public JsonResult DeleteMenuList(List<Menu> model)
        {
            _service.DeleteMenuList(model);
            return Json(this.returnOK(model));
        }
    }
}
