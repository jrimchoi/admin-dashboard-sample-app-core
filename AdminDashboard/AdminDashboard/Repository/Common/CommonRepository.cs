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
    public class CommonRepository
    {
        private readonly OracleDapper _dapper;
        public CommonRepository(OracleDapper dapper)
        {
            _dapper = dapper;
            Log.Debug("CommonRepository calling...");
        }

        // 사용자별 메뉴가져오기 
        public List<Dictionary<string, string>> GetUserMenu(BaseSearchModel model)
        {
            return _dapper.Query4NoModel(CommonDapper.GetUserMenu(model));
        }

        // 데이터 중복여부  
        public bool DataExistCheck(object model, string tblNm, string keyType, string keyId, string compareId)
        {
            return DataExistCheck(model, tblNm, keyType, keyId, compareId, "", "");
        }

        public bool DataExistCheck(object model, string tblNm, string keyType, string keyId, string compareId, string keyId2, string compareId2)
        {
            long cnt = _dapper.Count(CommonDapper.DataExistCheck(model, tblNm, keyType, keyId, compareId, keyId2, compareId2));

            if (cnt > 0) return true;  // 존재 

            return false;
        }

        // 중복체크 
        public bool DupicateCheck(object model, string tblNm, string keyType, string keyId, string compareId)
        {
            return DupicateCheck(model, tblNm, keyType, keyId, compareId, "", "");
        }

        public bool DupicateCheck(object model, string tblNm, string keyType, string keyId, string compareId, string keyId2, string compareId2)
        {
            long cnt = _dapper.Count(CommonDapper.DupicateCheck(model, tblNm, keyType, keyId, compareId, keyId2, compareId2));

            if (cnt > 0) return false;  // 중복 

            return true;
        }

        // 키값 채번 
        public int GetSeqence(object model, string seqId)
        {
            int sequence = _dapper.Sequence(CommonDapper.GetSeqence(model, seqId));

            Log.Debug("sequence : " + sequence);

            return sequence;
        }

        // 오라클 프로시져 (처리) 
        public void SPExec(string spName, OracleDynamicParameters parameters)
        {
            _dapper.SPExec(spName, parameters);
        }

        // HUB 연동 시 필요
        public SessionModel GetLoginInfo(LoginModel model)
        {
            return _dapper.QuerySingle<SessionModel>(LoginDapperSql.GetLoginInfo(model));
        }
    }
}
