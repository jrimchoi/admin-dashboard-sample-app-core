using Microsoft.AspNetCore.Mvc;
using DSELN.Cmm.Filters;
using DSELN.Service.Common;
using DSELN.Models;
using DSELN.Models.CodeMng;
using Newtonsoft.Json;
using Serilog;
using System.Text.RegularExpressions;
using DSELN.Service.CodeMng;
using DSELN.Cmm.Utils;
using System.Collections.Generic;

namespace DSELN.Controllers.CodeMng
{
    [AuthorizeFilter(Role.ADMIN)]  // method 단위로 설정가능, (Role.ADMIN, Role.USER) 로 다중 롤권한 부여가능 
    public class CodeMngController : BaseController
    {
        private readonly ICommonService _cmmService;
        private readonly ICodeMngService _codeMngService;

        public CodeMngController(ICommonService cmmService, ICodeMngService codeMngService)
        {
            _cmmService = cmmService;
            _codeMngService = codeMngService;
        }

        /****************************************************************
        // 코드유형 
        ****************************************************************/
        // 코드유형 page view 
        [HttpGet, HttpPost]
        [Route("CodeMng/CodeType")]
        public IActionResult CodeType(BaseSearchModel model)
        {
            // 1010. 코드담기 
            base.getComCodeList(new string[] { "CD_TYP", "LANG_CD" }, _cmmService);

            // 2010. ViewData 에 부가 정보를 담는다. 
            ViewData["_POPUP_YN"] = model._POPUP_YN;

            // 3010. view page  
            return View("Views/CodeMng/CodeType.cshtml", model);
        }

        // 코드유형 조회 
        [ValidateAntiForgeryToken] // xss 
        [ValidateModel]                  // validation interceptor 
        [HttpPost]
        //[Route("CodeGroupSearch")]  
        public JsonResult GetCodeTypeList(BaseSearchModel model)
        {
            //model.PrintModelValues("조회조건");
            List<CodeType> list = _codeMngService.GetCodeTypeList(model);
            return Json(this.returnSearchOK(list, model));
        }

        // 코드유형 저장  
        [ValidateAntiForgeryToken] // xss 
        [ValidateModel]                  // validation interceptor 
        [HttpPost]
        //[Route("CodeTypeSave")]  
        public JsonResult CodeTypeSave(List<CodeType> model)
        {
            //Log.Debug("SaveCodeGroup  ... : " + model.Count().ToString());
            _codeMngService.CodeTypeSave(model);
            return Json(this.returnOK(model));
        }

        /****************************************************************
        // 코드그룹 
        ****************************************************************/
        // 코드그룹 page view 
        [HttpGet, HttpPost]
        [Route("CodeMng/CodeGroup")]
        public IActionResult CodeGroup(BaseSearchModel model) // page view 
        {
            // 1010. 코드담기 
            base.getComCodeList(new string[] { "CD_TYP", "LANG_CD" }, _cmmService);

            // 2010. ViewData 에 부가 정보를 담는다. 
            ViewData["_POPUP_YN"] = model._POPUP_YN;

            // 3010. view page  
            return View("Views/CodeMng/CodeGroup.cshtml", model);
        }

        // 코드그룹 리스트 조회 
        [ValidateAntiForgeryToken] // xss 
        [ValidateModel]                  // validation interceptor 
        [HttpPost]
        public JsonResult GetCodeGroupList(CodeGroupSearch model)
        {
            //model.PrintModelValues("조회조건");
            List<CodeGroup> list = _codeMngService.GetCodeGroupList(model);
            Log.Debug("result " + list.Count);
            return Json(this.returnSearchOK(list, model));
        }

        // 코드그룹 저장 
        [ValidateAntiForgeryToken] // xss 
        [ValidateModel]                  // validation interceptor 
        [HttpPost]
        //[Route("SaveCodeGroup")]  
        public JsonResult SaveCodeGroup(List<CodeGroup> model)
        {
            //foreach (var item in model)
            //{
            //    item.PrintModelValues("detail");
            //}

            _codeMngService.SaveCodeGroup(model);

            return Json(this.returnOK(model));
        }

        /****************************************************************
        // 코드관리
        ****************************************************************/
        // 코드관리  page view
        [HttpGet, HttpPost]
        [Route("CodeMng/CodeReg")]
        public IActionResult CodeReg(BaseSearchModel model)
        {
            model.PrintModelValues("page view....");
            base.getComCodeList(new string[] { "CD_TYP", "LANG_CD" }, _cmmService);
            return View("Views/CodeMng/CodeReg.cshtml", model);
        }

        // 코드관리 리스트 조회 
        [ValidateAntiForgeryToken] // xss 
        [ValidateModel]                  // validation interceptor 
        [HttpPost]
        public JsonResult GetCodeMngList(CodeGroupSearch model)
        {
            model.PrintModelValues("조회조건");
            List<CodeGroup> list = _codeMngService.GetCodeGroupList(model);

            return Json(this.returnSearchOK(list, model));
        }

        // 코드관리 코드그룹(마스터) 조회 
        [ValidateAntiForgeryToken] // xss 
        [ValidateModel]                  // validation interceptor 
        [HttpPost]
        public JsonResult GetCodeMng(CodeGroupSearch model)
        {
            model.PrintModelValues("조회조건");
            CodeGroup master = _codeMngService.GetCodeGroup(model);
            List<CodeDetail> detail = _codeMngService.GetCodeDetail(model);

            return Json(this.returnSearchOK(master, detail, model));
        }

        [ValidateAntiForgeryToken] // xss 
        [ValidateModel]                  // validation interceptor 
        [HttpPost]
        public JsonResult SaveCodeMD(CodeGroupMD model)
        {
            model.master.PrintModelValues("master");

            if (model.detail != null)
            {
                foreach (var item in model.detail)
                {
                    item.PrintModelValues("detail");
                }
            }

            Log.Debug("GRP_CD_KEY : " + model.GRP_CD_KEY);

            _codeMngService.SaveCodeMD(model);

            return Json(this.returnOK(model.master, model.detail));
        }

        [ValidateAntiForgeryToken] // xss 
        [ValidateModel]                  // validation interceptor 
        [HttpPost]
        public JsonResult SaveCodeDetail(List<CodeDetail> detail)
        {

            if (detail != null)
            {
                foreach (var item in detail)
                {
                    item.PrintModelValues("detail");
                }
            }

            _codeMngService.SaveCodeDetail(detail);

            return Json(this.returnOK(detail));
        }

        [ValidateAntiForgeryToken] // xss 
        [ValidateModel]                  // validation interceptor 
        [HttpPost]
        public JsonResult DeleteCodeDetail(List<CodeDetail> detail)
        {

            if (detail != null)
            {
                foreach (var item in detail)
                {
                    item.PrintModelValues("detail");
                }
            }

            _codeMngService?.DeleteCodeDetail(detail);

            return Json(this.returnOK(detail));
        }







        [ValidateAntiForgeryToken] // xss 
        [ValidateModel]                  // validation interceptor 
        [HttpPost]
        public JsonResult TestSaveMD(CodeType master, List<CodeGroup> detail)
        {
            master.PrintModelValues("master");
            foreach (var item in detail)
            {
                item.PrintModelValues("detail");
            }

            return Json(this.returnOK(master));
        }

        [ValidateAntiForgeryToken] // xss 
        [ValidateModel]                  // validation interceptor 
        [HttpPost]
        //[Route("SaveCodeGroup")]  
        //public JsonResult TestSaveMD(CodeType master, List<CodeGroup> detail)
        public JsonResult TestSaveMD2(CodeGroupMD model)
        {

            model.master.PrintModelValues("master");

            foreach (var item in model.detail)
            {
                item.PrintModelValues("detail");
            }

            return Json(this.returnOK(model));
        }

    }
}
