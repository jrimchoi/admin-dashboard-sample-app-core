using DSELN.Cmm.Exceptions;
using DSELN.Cmm.Filters;
using DSELN.Cmm.Filters.FilterTask;
using DSELN.Cmm.Utils;
using DSELN.Models;
using DSELN.Models.Analysis;
using DSELN.Models.Common;
using DSELN.Models.DashBoard;
using DSELN.Service.Common;
using DSELN.Service.DashBoard;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DSELN.Controllers.DashBoard
{
    public class DashBoardController : BaseController
    {
        private readonly ICommonService _cmmService;
        private readonly IDashBoardService _service;

        public DashBoardController(ICommonService cmmService, IDashBoardService service)
        {
            _cmmService = cmmService;
            _service = service;
        }

        /****************************************************************
        // DashBoard  
        ****************************************************************/
        // DashBoard   page view 
        //[SkipSessionCheckTask]
        [HttpGet, HttpPost]
        [Route("DashBoard/DashBoard")]
        public IActionResult DashBoard(DashBoardSearch model)
        {
            // 관리자, 소장, 팀장 : 대시보드(관리자) 
            if (_service.CheckAuthDashBoard4Admin(model))
            {
                return RedirectToAction("DashBoard4Admin", "DashBoard");
            }
            else
            {
                return RedirectToAction("DashBoard4User", "DashBoard");
            }
        }

        /****************************************************************
        // DashBoard (관리자) 
        ****************************************************************/
        // DashBoard (관리자)  page view 
        //[SkipSessionCheckTask]
        [HttpGet, HttpPost]
        [Route("DashBoard/DashBoard4Admin")]
        public IActionResult DashBoard4Admin(DashBoardSearch model)
        {
            // 0000. 권한체크 
            //if (!_service.CheckAuthDashBoard4Admin(model))
            //{
            //    throw new CustomValidationException("화면에 대한 권한이 없습니다. [관리자, 소장, 팀장]");
            //}

            //if (!_service.CheckAuthDashBoardData(model))
            //{
            //    throw new CustomValidationException("데이터를 조회할 권한이 없습니다.");
            //}

            // 1010. 코드를 담는다. 
            var constProp = new List<CodeCondition>()  // new version 
            {
                new CodeCondition(){ID = "BU_CODE"},
                new CodeCondition(){ID = "DASHBOARD_TEAM_CODE"},
            };
            base.getComCodeList(constProp, _cmmService);

            return View("Views/DashBoard/DashBoard4Admin.cshtml");
        }

        // DashBoard (관리자) 조회 
        //[SkipSessionCheckTask]
        //[ValidateAntiForgeryToken] // xss 
        [ValidateModel]                  // validation interceptor 
        [HttpPost]
        [Route("DashBoard/GetDashBoard4Admin")]
        public JsonResult GetDashBoard4Admin(DashBoardSearch model)
        {
            // 0000. 권한체크 
            //if (!_service.CheckAuthDashBoard4Admin(model))
            //{
            //    throw new CustomValidationException("화면에 대한 권한이 없습니다. [관리자, 소장, 팀장]");
            //}

            //if (!_service.CheckAuthDashBoardData(model))
            //{
            //    throw new CustomValidationException("데이터를 조회할 권한이 없습니다.");
            //}

            // 조회 기간 
            int period = Convert.ToInt32(Regex.Replace(model.PERIOD, @"[^0-9]", ""));
            model.FR_DATE = Utils.GetDashBoardPeriod("FR_DATE", period);
            model.TO_DATE = Utils.GetDashBoardPeriod("TO_DATE", period);

            List<Dictionary<string, string>> list = _service.GetExpNoteList(model);
            List<Dictionary<string, string>> list2 = _service.GetTemplateList(model);
            List<Dictionary<string, string>> list3 = _service.GetExpResultList(model);

            return Json(this.returnSearchOK(list, list2, list3, model));
        }

        /****************************************************************
        // DashBoard (연구원) 
        ****************************************************************/
        // DashBoard (연구원)  page view 
        //[SkipSessionCheckTask]
        [HttpGet, HttpPost]
        [Route("DashBoard/DashBoard4User")]
        public IActionResult DashBoard4User(DashBoardSearch model)
        {
            // 0000. 권한체크 
            if (!_service.CheckAuthDashBoardData(model))
            {
                throw new CustomValidationException("데이터를 조회할 권한이 없습니다.");
            }

            // 1010. 코드를 담는다. 
            var constProp = new List<CodeCondition>()  // new version 
            {
                new CodeCondition(){ID = "BU_CODE"},
                new CodeCondition(){ID = "DASHBOARD_TEAM_CODE", SUB_ID = "DASHBOARD_USER"},
            };
            base.getComCodeList(constProp, _cmmService);

            //Log.Debug("model.USER_NAME : " + model.USER_NAME);

            // 2010. viewdata 
            ViewData["USER_ID"] = model.USER_ID;  // 관리자대시보드 --> 연구원대시보드 이동 
            ViewData["USER_NAME"] = model.USER_NAME;
            ViewData["TEAM_CODE"] = model.TEAM_CODE;

            return View("Views/DashBoard/DashBoard4User.cshtml");
        }

        // DashBoard (연구원) 조회 
        //[ValidateAntiForgeryToken] // xss 
        //[SkipSessionCheckTask]
        [ValidateModel]                  // validation interceptor 
        [HttpPost]
        [Route("DashBoard/GetDashBoard4User")]
        public JsonResult GetDashBoard4User(DashBoardSearch model)
        {
            // 1010. 조회 기간 
            int period = Convert.ToInt32(Regex.Replace(model.PERIOD, @"[^0-9]", ""));
            model.FR_DATE = Utils.GetDashBoardPeriod("FR_DATE", period);
            model.TO_DATE = Utils.GetDashBoardPeriod("TO_DATE", period);

            // 2010. 체크사항
            // 10. 작성자 필수 체크 (관리자, 연구원 동일 모델을 사용해서.. 여기서 체크하겠다.)
            if (string.IsNullOrEmpty(model.USER_ID))
            {
                throw new CustomValidationException("작성자는 필수입력 사항입니다.");
            }

            // 20. 조회 파라미터 기준으로 조회권한 여부 체크 
            if (!_service.CheckAuthDashBoardData(model))
            {
                throw new CustomValidationException("데이터를 조회할 권한이 없습니다.");
            }

            List<Dictionary<string, string>> list = _service.GetExpNoteList(model);
            List<Dictionary<string, string>> list2 = _service.GetTemplateList(model);

            return Json(this.returnSearchOK(list, list2, model));
        }

    }

}
