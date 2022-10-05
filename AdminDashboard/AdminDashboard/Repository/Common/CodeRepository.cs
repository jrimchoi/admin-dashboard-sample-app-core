using DSELN.DapperSql.Common;
using DSELN.Cmm.DataBase;
using DSELN.Models.Common;
using DSELN.Models.CodeMng;
using DSELN.Models;
using Serilog;
using Dapper.Oracle;
using System.Data;
using DSELN.Models.Login;
using DSELN.Cmm.Utils;
using DSELN.DapperSql.Login;
using System.Collections.Generic;

namespace DSELN.Repository.Common
{
    public class CodeRepository
    {
        private readonly OracleDapper _dapper;
        public CodeRepository(OracleDapper dapper)
        {
            _dapper = dapper;
            Log.Debug("CommonRepository calling...");
        }

        // 공통 코드   
        public List<CodeModel> GetCodeList(CodeCondition model)
        {
            return _dapper.Query<CodeModel>(CodeDapper.GetCodeList(model));
        }

        // Role 코드 
        public List<CodeModel> GetRoleList(CodeCondition model)
        {
            return _dapper.Query<CodeModel>(CodeDapper.GetRoleList(model));
        }

        // BU 코드 
        public List<CodeModel> GetBUList(CodeCondition model)
        {
            return _dapper.Query<CodeModel>(CodeDapper.GetBUList(model));
        }

        // TEAM 코드 
        public List<CodeModel> GetDashBoardTeamList(CodeCondition model)
        {
            return _dapper.Query<CodeModel>(CodeDapper.GetDashBoardTeamList(model));
        }

        // 장비
        public List<CodeModel> GetEquip4Code(CodeCondition model)
        {
            return _dapper.Query<CodeModel>(CodeDapper.GetEquip4Code(model));
        }

        // 분석항목
        public List<CodeModel> GetAnalysisItem4Code(CodeCondition model)
        {
            return _dapper.Query<CodeModel>(CodeDapper.GetAnalysisItem4Code(model));
        }

        // 분석 템플릿
        public List<CodeModel> GetAnalysisTemplate4Code(CodeCondition model)
        {
            return _dapper.Query<CodeModel>(CodeDapper.GetAnalysisTemplate4Code(model));
        }

        // 템플릿속성  
        public List<CodeModel> GetTemplateAttr4Code(CodeCondition model)
        {
            return _dapper.Query<CodeModel>(CodeDapper.GetTemplateAttr4Code(model));
        }

        // 사용자 리스트 
        public List<Dictionary<string, string>> GetUserList(CodeCondition model)
        {
            return _dapper.Query4NoModel(CodeDapper.GetUserList(model));   //  _dapper.Query<Dictionary<string, object>>(CommonDapper.GetUserList(model));
        }

        /// <summary>
        /// Dropdown List를 위해 속성 단위 조회
        /// </summary>
        /// <param name="model"></param>
        /// <returns>조회 결과 리턴</returns>
        public List<CodeModel> GetUnitData4Code(CodeCondition model)
        {
            return _dapper.Query<CodeModel>(CodeDapper.GetUnitData4Code(model));
        }

        /// <summary>
        /// QR 장비 관리 화면의 장비 담당자 Dropdown List를 위해 장비 담당자 조회
        /// </summary>
        /// <param name="model"></param>
        /// <returns>조회 결과 리턴</returns>
        public List<CodeModel> GetEquipCharger4Code(CodeCondition model)
        {
            return _dapper.Query<CodeModel>(CodeDapper.GetEquipCharger4Code(model));
        }

        /// <summary>
        /// QR 장비 관리 화면의 장비 위치 Dropdown List를 위해 장비 위치 조회
        /// </summary>
        /// <param name="model"></param>
        /// <returns>조회 결과 리턴</returns>
        public List<CodeModel> GetEquipLoc4Code(CodeCondition model)
        {
            return _dapper.Query<CodeModel>(CodeDapper.GetEquipLoc4Code(model));
        }
    }
}
