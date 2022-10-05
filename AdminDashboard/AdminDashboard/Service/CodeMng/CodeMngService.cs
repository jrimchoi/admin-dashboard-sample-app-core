using DSELN.Cmm.Helper;
using DSELN.Models;
using DSELN.Models.CodeMng;
using DSELN.Repository.CodeMng;
using DSELN.Repository.Common;
using DSELN.Service.Common;
using System;
using System.Collections.Generic;

namespace DSELN.Service.CodeMng
{
    public interface ICodeMngService
    {
        List<CodeType> GetCodeTypeList(BaseSearchModel model); // 코드유형 조회 
        List<CodeType> CodeTypeSave(List<CodeType> model); // 코드유형 저장  
        List<CodeGroup> GetCodeGroupList(CodeGroupSearch model); // 코드그룹 리스트 조회 
        CodeGroup GetCodeGroup(CodeGroupSearch model); // 코드그룹 마스터 조회 
        List<CodeDetail> GetCodeDetail(CodeGroupSearch model); // 코드그룹 디테일  조회 
        List<CodeGroup> SaveCodeGroup(List<CodeGroup> model); // 코드그룹 저장  
        CodeGroupMD SaveCodeMD(CodeGroupMD model); // 코드그룹 마스터/디테일 저장 
        List<CodeDetail> SaveCodeDetail(List<CodeDetail> model); // 코드그룹 디테일 저장 
        List<CodeDetail> DeleteCodeDetail(List<CodeDetail> model); // 코드그룹 디테일 삭제  
    }

    public class CodeMngService : ICodeMngService
    {
        private readonly CodeMngRepository _codeMngRepository;
        private readonly CommonRepository _cmmRepository;

        public CodeMngService(CodeMngRepository codeMngRepository, CommonRepository cmmRepository)
        {
            _codeMngRepository = codeMngRepository;  // dot net DI 
            _cmmRepository = cmmRepository;
        }

        // 코드유형 리스트 조회 
        public List<CodeType> GetCodeTypeList(BaseSearchModel model)
        {
            return _codeMngRepository.GetCodeTypeList(model);
        }

        // 코드유형 저장  
        public List<CodeType> CodeTypeSave(List<CodeType> model)
        {
            foreach (var item in model)
            {
                // 1010. pre-process  
                // 10. 데이터 존재여부 
                if (("U".Equals(item._ROW_TYPE) || "D".Equals(item._ROW_TYPE)) && !_cmmRepository.DataExistCheck(item, "ELN_IF.TB_ESA_CDTP", "_KEY", "CD_TYP", "CD_TYP"))
                {
                    TransactionHelper.SetRollbackOnly("처리할 데이터가 존재하지 않습니다.");
                }

                // 20. 중복체크 (DB table pk or unique index 도 추가할것.) 
                if (("I".Equals(item._ROW_TYPE) || "U".Equals(item._ROW_TYPE)) && !_cmmRepository.DupicateCheck(item, "ELN_IF.TB_ESA_CDTP", "_KEY", "CD_TYP", "CD_TYP"))
                {
                    TransactionHelper.SetRollbackOnly("코드유형이 중복 입력되었습니다.");
                }

                // 2010. IUD process  
                int applied = 0;  // fail 
                if ("I".Equals(item._ROW_TYPE))
                {
                    applied = _codeMngRepository.CodeTypeInsert(item);
                }
                else if ("U".Equals(item._ROW_TYPE))
                {
                    applied = _codeMngRepository.CodeTypeUpdate(item);
                }
                else if ("D".Equals(item._ROW_TYPE))
                {
                    TransactionHelper.SetRollbackOnly("삭제할수 없습니다.");
                    applied = _codeMngRepository.CodeTypeDelete(item);
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
            }

            // 3010. post-process 

            return model;
        }


        // 코드그룹 리스트 조회 
        public List<CodeGroup> GetCodeGroupList(CodeGroupSearch model)
        {
            return _codeMngRepository.GetCodeGroupList(model);
        }

        // 코드그룹 저장 (리스트)  (Workflow)  
        public List<CodeGroup> SaveCodeGroup(List<CodeGroup> model)
        {

            foreach (var item in model)
            {
                CodeGroup rtn = this.SaveCodeGroup(item);
            }

            return model;
        }

        // 코드그룹 마스터 조회 
        public CodeGroup GetCodeGroup(CodeGroupSearch model)
        {
            return _codeMngRepository.GetCodeGroup(model);
        }

        // 코드그룹 디테일  조회 
        public List<CodeDetail> GetCodeDetail(CodeGroupSearch model)
        {
            return _codeMngRepository.GetCodeDetail(model);
        }

        // 코드그룹 마스터/디테일 저장 (Workflow)
        public CodeGroupMD SaveCodeMD(CodeGroupMD model)
        {
            if (model.master != null)
            {
                // 10. 전처리 
                if (model.master.GRP_CD_KEY == null || model.master.GRP_CD_KEY == "") model.master._ROW_TYPE = 'I'.ToString();
                if (!(model.master.GRP_CD_KEY == null || model.master.GRP_CD_KEY == "")) model.master._ROW_TYPE = 'U'.ToString();



                // 20. 저장 
                CodeGroup rtn = this.SaveCodeGroup(model.master);

                // 30. 후처리  

            }

            if (model.detail != null)
            {
                // 10. 전처리 
                string grpCd = model.GRP_CD;
                string grpCdKey = model.GRP_CD_KEY;

                foreach (var item in model.detail)
                {
                    item.GRP_CD = grpCd;  // 마스터 키 
                    item.GRP_CD_KEY = grpCdKey;

                    // 20. 저장 
                    CodeDetail rtn = this.SaveCodeDetail(item);

                    // 30. 후처리 
                }
            }

            // 90. 최종 후처리 

            return model;
        }

        // 코드그룹 디테일 저장   (Workflow) 
        public List<CodeDetail> SaveCodeDetail(List<CodeDetail> model)
        {
            if (model != null)
            {
                foreach (var item in model)
                {
                    CodeDetail rtn = this.SaveCodeDetail(item);
                }
            }

            return model;
        }


        // 코드그룹 디테일 삭제  (Workflow) 
        public List<CodeDetail> DeleteCodeDetail(List<CodeDetail> model)
        {
            if (model != null)
            {
                foreach (var item in model)
                {
                    CodeDetail rtn = this.DeleteCodeDetail(item);
                }
            }

            return model;
        }

        // 코드그룹 마스터 저장 (IUD) 
        public CodeGroup SaveCodeGroup(CodeGroup model)
        {
            if (model != null)
            {
                // IUD 따라 분기 
                int applied = 0;  // fail 
                if ("I".Equals(model._ROW_TYPE))
                {
                    // 10. 신규입력전 체크사항 

                    // 20. 신규입력 
                    applied = _codeMngRepository.CodeGroupInsert(model);

                    // 30. 신규입력 후처리 

                }
                else if ("U".Equals(model._ROW_TYPE))
                {
                    // 10. 수정전 체크사항 

                    // 20. 수정  
                    applied = _codeMngRepository.CodeGroupUpdate(model);

                    // 30. 수정 후처리 

                }
                else
                {
                    // 삭제는 로직을 따로 분리한다. 
                    new Exception("_ROW_TYPE omission....");
                }

                if (applied == 0)
                {
                    // 처리시 오류로 간주 
                    TransactionHelper.SetRollbackOnly("적용된 건수가 0 입니다.");
                }

            }

            return model;
        }

        // 코드그룹 디테일 저장 (IUD) 
        public CodeDetail SaveCodeDetail(CodeDetail model)
        {
            if (model != null)
            {
                // IUD 따라 분기 
                int applied = 0;  // fail 

                if ("I".Equals(model._ROW_TYPE))
                {
                    // 10. 디테일 신규입력시 체크사항 

                    //20. 디테일 신규입력 
                    applied = _codeMngRepository.CodeDetailInsert(model);
                }
                else if ("U".Equals(model._ROW_TYPE))
                {
                    // 10. 디테일 수정전 체크 

                    // 20. 디테일 수정 
                    applied = _codeMngRepository.CodeDetailUpdate(model);
                }
                else
                {
                    // 삭제는 로직을 따로 분리한다. 
                    new Exception("_ROW_TYPE omission....");
                }

                if (applied == 0)
                {
                    // 처리시 오류로 간주 
                    TransactionHelper.SetRollbackOnly("적용된 건수가 0 입니다.");
                }

            }

            return model;
        }

        // 코드그룹 디테일 삭제  (IUD) 
        public CodeDetail DeleteCodeDetail(CodeDetail model)
        {
            if (model != null)
            {
                // 10. 삭제전 체크 

                // 20. 삭제 
                int applied = _codeMngRepository.CodeDetailDelete(model);

                if (applied == 0)
                {
                    // 처리시 오류로 간주 
                    TransactionHelper.SetRollbackOnly("적용된 건수가 0 입니다.");
                }

                // 30. 삭제후 처리 
            }

            return model;
        }


    }
}
