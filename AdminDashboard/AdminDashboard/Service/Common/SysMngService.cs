using DSELN.Cmm.Helper;
using DSELN.Models;
using DSELN.Models.CodeMng;
using DSELN.Models.Common;
using DSELN.Repository.CodeMng;
using DSELN.Repository.Common;
using DSELN.Repository.SysMng;
using DSELN.Service.Common;
using System;
using System.Collections.Generic;

namespace DSELN.Service.SysMng
{
    public interface ISysMngService
    {
        List<Menu> GetMenuList(MenuSearch model); // 메뉴등록 조회 
        List<Menu> SaveMenuList(List<Menu> model); // 메뉴등록 저장  
        List<Menu> DeleteMenuList(List<Menu> model); // 메뉴등록 삭제   
    }

    public class SysMngService : ISysMngService
    {
        private readonly SysMngRepository _repository;
        private readonly CommonRepository _cmmRepository;
        public SysMngService(SysMngRepository repository, CommonRepository cmmRepository)
        {
            _repository = repository;  // dot net DI 
            _cmmRepository = cmmRepository;
        }

        // 메뉴등록 조회 
        public List<Menu> GetMenuList(MenuSearch model)
        {
            return _repository.GetMenuList(model);
        }

        // 메뉴리스트 저장 
        public List<Menu> SaveMenuList(List<Menu> model)
        {
            foreach (var item in model)
            {
                this.SaveMenu(item);
            }

            // 3010. post-process 

            return model;
        }

        // 메뉴리스트 삭제  
        public List<Menu> DeleteMenuList(List<Menu> model)
        {
            foreach (var item in model)
            {
                item._PROC_TYPE = "DELETE";
                item._ROW_TYPE = "D";

                this.SaveMenu(item);
            }

            // 3010. post-process 

            return model;
        }

        // 메뉴 IUD 
        public Menu SaveMenu(Menu model)
        {
            // 1010. pre-process  
            // 00. _ROW_TYPE : I / U / D 
            model.SetRowType(model.MENU_CD_KEY);

            // 10. 데이터 존재여부 
            if (("U".Equals(model._ROW_TYPE) || "D".Equals(model._ROW_TYPE)) && !_cmmRepository.DataExistCheck(model, "ELN_IF.TB_ESA_AUMM", "_KEY", "MENU_CD", "MENU_CD"))
            {
                TransactionHelper.SetRollbackOnly("처리할 데이터가 존재하지 않습니다.");
            }

            // 20. 중복체크 (DB table pk or unique index 도 추가할것.) 
            if (("I".Equals(model._ROW_TYPE) || "U".Equals(model._ROW_TYPE)) && !_cmmRepository.DupicateCheck(model, "ELN_IF.TB_ESA_AUMM", "_KEY", "MENU_CD", "MENU_CD"))
            {
                TransactionHelper.SetRollbackOnly("메뉴코드가 중복 입력되었습니다.");
            }

            // 2010. IUD process  
            int applied = 0;  // fail 
            if ("I".Equals(model._ROW_TYPE))
            {
                applied = _repository.MenuInsert(model);
            }
            else if ("U".Equals(model._ROW_TYPE))
            {
                applied = _repository.MenuUpdate(model);
            }
            else if ("D".Equals(model._ROW_TYPE))
            {
                applied = _repository.MenuDelete(model);
            }
            else
            {
                new Exception("_ROW_TYPE omission....");
            }

            if (applied == 0)
            {
                // 처리시 오류로 간주 
                TransactionHelper.SetRollbackOnly("적용된 건수가 0 입니다.");
            }

            // 3010. post-process 

            return model;
        }


    }
}
