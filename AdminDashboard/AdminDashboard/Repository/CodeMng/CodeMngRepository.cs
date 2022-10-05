using DSELN.DapperSql.CodeMng;
using DSELN.Cmm.DataBase;
using DSELN.Models.Common;
using DSELN.Models.CodeMng;
using DSELN.Models;
using Serilog;
using System.Collections.Generic;

namespace DSELN.Repository.CodeMng
{
    public class CodeMngRepository
    {
        private readonly OracleDapper _dapper;
        public CodeMngRepository(OracleDapper dapper)
        {
            _dapper = dapper;
            Log.Debug("CommonRepository calling...");
        }

        // 코드유형 
        public List<CodeType> GetCodeTypeList(BaseSearchModel model)
        {
            return _dapper.Query<CodeType>(CodeMngDapper.GetCodeTypeList(model));
        }

        public int CodeTypeInsert(CodeType model)
        {
            return _dapper.Execute(CodeMngDapper.CodeTypeInsert(model));
        }

        public int CodeTypeUpdate(CodeType model)
        {
            return _dapper.Execute(CodeMngDapper.CodeTypeUpdate(model));
        }

        public int CodeTypeDelete(CodeType model)
        {
            return _dapper.Execute(CodeMngDapper.CodeTypeDelete(model));
        }


        // 코드그룹 리스트 조회 
        public List<CodeGroup> GetCodeGroupList(CodeGroupSearch model)
        {
            return _dapper.Query<CodeGroup>(CodeMngDapper.GetCodeGroupList(model));
        }

        // 코드그룹 마스터 조회 
        public CodeGroup GetCodeGroup(CodeGroupSearch model)
        {
            return _dapper.QuerySingle<CodeGroup>(CodeMngDapper.GetCodeGroup(model));
        }

        // 코드그룹 마스터 저장 
        public int CodeGroupInsert(CodeGroup model)
        {
            return _dapper.Execute(CodeMngDapper.CodeGroupInsert(model));
        }

        public int CodeGroupUpdate(CodeGroup model)
        {
            return _dapper.Execute(CodeMngDapper.CodeGroupUpdate(model));
        }

        public int CodeGroupDelete(CodeGroup model)
        {
            return _dapper.Execute(CodeMngDapper.CodeGroupDelete(model));
        }

        // 코드그룹 디테일  조회 
        public List<CodeDetail> GetCodeDetail(CodeGroupSearch model)
        {
            return _dapper.Query<CodeDetail>(CodeMngDapper.GetCodeDetail(model));
        }

        // 코드그룹 디테일 저장 
        public int CodeDetailInsert(CodeDetail model)
        {
            return _dapper.Execute(CodeMngDapper.CodeDetailInsert(model));
        }

        public int CodeDetailUpdate(CodeDetail model)
        {
            return _dapper.Execute(CodeMngDapper.CodeDetailUpdate(model));
        }

        public int CodeDetailDelete(CodeDetail model)
        {
            return _dapper.Execute(CodeMngDapper.CodeDetailDelete(model));
        }



    }
}
