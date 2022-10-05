using DSELN.Cmm.Helper;
using DSELN.Models;
using DSELN.Models.Common;
using DSELN.Models.Login;
using DSELN.Repository.Common;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Collections.Generic;

namespace DSELN.Service.Common
{
    public interface ICommonService
    {
        List<Dictionary<string, string>> GetUserMenu(BaseSearchModel model);   // 사용자별 메뉴가져오기 
        Dictionary<string, List<CodeModel>> GetCodeList(List<CodeCondition> constProp); // 공통 코드 가져오기 
        List<Dictionary<string, string>> GetCodeHelp(CodeCondition model);        // 코드성 항목 가져오기 (코드헬프) 
        SessionModel GetLoginInfo(LoginModel model);
    }

    public class CommonService : ICommonService
    {
        private readonly CodeRepository _repository;
        private readonly CommonRepository _cmmRepository;

        public CommonService(CodeRepository repository, CommonRepository cmmRepository)
        {
            _repository = repository;  // dot net DI 
            _cmmRepository = cmmRepository;
        }

        // 사용자별 메뉴가져오기 
        public List<Dictionary<string, string>> GetUserMenu(BaseSearchModel model)
        {
            List<Dictionary<string, string>> list = _cmmRepository.GetUserMenu(model);
            return list;
        }

        // 공통 코드 가져오기
        public Dictionary<string, List<CodeModel>> GetCodeList(List<CodeCondition> constProp)
        {
            Dictionary<string, List<CodeModel>> codeDic = new Dictionary<string, List<CodeModel>>();

            foreach (var item in constProp)
            {
                string grpCd = item.ID.ToString();

                if ("ANAL_ITEMS".Equals(grpCd))  // 분석항목 
                {
                    codeDic.Add(grpCd, _repository.GetAnalysisItem4Code(item));
                }
                else if ("TMPL_ID".Equals(grpCd)) // 분석 템플릿
                {
                    codeDic.Add(grpCd, _repository.GetAnalysisTemplate4Code(item));
                }
                else if ("SMPL_ATTR".Equals(grpCd)) // 샘플항목 속성 
                {
                    item.SUB_ID = "SAMPLE";
                    codeDic.Add(grpCd, _repository.GetTemplateAttr4Code(item));
                }
                else if ("ANAL_ATTR".Equals(grpCd)) // 분석항목 속성 
                {
                    item.SUB_ID = "ANALYSIS";
                    codeDic.Add(grpCd, _repository.GetTemplateAttr4Code(item));
                }
                else if ("SMPL_ANAL_ATTR".Equals(grpCd)) // 샘플분석 속성  
                {
                    item.SUB_ID = "SMPL_ANAL";
                    codeDic.Add(grpCd, _repository.GetTemplateAttr4Code(item));
                }
                else if ("EQUIP".Equals(grpCd)) // 장비 
                {
                    codeDic.Add(grpCd, _repository.GetEquip4Code(item));
                }
                else if ("EQUIP_IMAGE".Equals(grpCd))
                {
                    item.PARAM1 = "LAS_TYPE";
                    item.PARAM1_VALUE = "IMAGE";
                    codeDic.Add(grpCd, _repository.GetEquip4Code(item));
                }
                else if ("TPL_ATTR".Equals(grpCd)) // 템플릿속성 ?? 
                {
                    codeDic.Add(grpCd, _repository.GetTemplateAttr4Code(item));
                }
                else if ("UNIT_DATA".Equals(grpCd)) // QR 장비의 속성 단위(UNIT_DATA)의 Dropdown List를 사용하기 위해 필요
                {
                    List<CodeModel> codeList = _repository.GetUnitData4Code(item);
                    List<CodeModel> newList = new List<CodeModel>();

                    codeList.ForEach(item =>
                    {
                        if (!string.IsNullOrEmpty(item.TEXT))
                        {
                            string[] arr = item?.TEXT.Trim().Split(","); // 속성 단위는 ','로 구분

                            for (int i = 0; i < arr.Length; i++)
                            {
                                CodeModel codeModel = new CodeModel();
                                codeModel.TEXT = arr[i].ToString().Trim();
                                codeModel.VALUE = item?.VALUE + "|" + arr[i].ToString().Trim(); // Dropdown List의 Value는 Unique해야 하기 때문에 필요
                                codeModel.ATT1 = item?.ATT1;

                                newList.Add(codeModel);
                            }
                        }

                    });

                    codeDic.Add(grpCd, newList);
                }
                else if ("QR_EQUIP_ID".Equals(grpCd)) // QR 장비 관리 화면의 장비ID Dropdown List를 사용하기 위해 필요
                {
                    // TB_EQUIPMENT 테이블에서 LAS_TYPE이 'QR'인 장비만 조회하기 위해 필요
                    item.PARAM1 = "LAS_TYPE";
                    item.PARAM1_VALUE = "QR";

                    List<CodeModel> list = _repository.GetEquip4Code(item);
                    list.ForEach(item => item.TEXT = item.VALUE); // Dropdwon List에서 선택 시 장비 ID를 보여주기 위함
                    codeDic.Add(grpCd, list);
                }
                else if ("CHARGER".Equals(grpCd)) // QR 장비 관리 화면의 장비 담당자 Dropdown List를 사용하기 위해 필요
                {
                    // TB_EQUIPMENT 테이블에서 LAS_TYPE이 'QR'인 장비의 담당자만 조회하기 위해 필요
                    item.PARAM1 = "LAS_TYPE";
                    item.PARAM1_VALUE = "QR";

                    codeDic.Add(grpCd, _repository.GetEquipCharger4Code(item));
                }
                else if ("QR_EQUIP".Equals(grpCd)) // QR 장비 관리 화면의 장비명 Dropdown List를 사용하기 위해 필요
                {
                    // TB_EQUIPMENT 테이블에서 LAS_TYPE이 'QR'인 장비만 조회하기 위해 필요
                    item.PARAM1 = "LAS_TYPE";
                    item.PARAM1_VALUE = "QR";

                    codeDic.Add(grpCd, _repository.GetEquip4Code(item));
                }
                else if ("QR_EQUIP_LOC".Equals(grpCd)) // QR 장비 관리 화면의 장비 위치 Dropdown List를 사용하기 위해 필요
                {
                    // TB_EQUIPMENT 테이블에서 LAS_TYPE이 'QR'인 장비의 위치만 조회하기 위해 필요
                    item.PARAM1 = "LAS_TYPE";
                    item.PARAM1_VALUE = "QR";

                    codeDic.Add(grpCd, _repository.GetEquipLoc4Code(item));
                }
                else if ("ROLE_CD".Equals(grpCd))  // Role 코드  
                {
                    codeDic.Add(grpCd, _repository.GetRoleList(item));
                }
                else if ("BU_CODE".Equals(grpCd))  // BU 코드  
                {
                    codeDic.Add(grpCd, _repository.GetBUList(item));
                }
                else if ("DASHBOARD_TEAM_CODE".Equals(grpCd))  // TEAM 코드  
                {
                    codeDic.Add(grpCd, _repository.GetDashBoardTeamList(item));
                }
                else  // 공통코드 
                {
                    codeDic.Add(grpCd, _repository.GetCodeList(item));
                }

            }

            return codeDic;
        }

        // 코드성 항목 가져오기 (코드헬프) 
        public List<Dictionary<string, string>> GetCodeHelp(CodeCondition model)
        {
            List<Dictionary<string, string>> list = null;

            if ("USER".Equals(model.ID) || "USER".Equals(model.CONST))
            {
                list = _repository.GetUserList(model);
            }

            return list;
        }

        // HUB 연동 시 필요
        public SessionModel GetLoginInfo(LoginModel model)
        {
            return _cmmRepository?.GetLoginInfo(model);
        }

    }

}
