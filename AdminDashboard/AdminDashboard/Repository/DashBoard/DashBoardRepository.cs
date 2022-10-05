using DSELN.Cmm.DataBase;
using DSELN.DapperSql.Analysis;
using DSELN.DapperSql.DashBoard;
using DSELN.Models.Analysis;
using DSELN.Models.DashBoard;
using DSELN.Models.Sample;
using Serilog;
using System.Collections.Generic;

namespace DSELN.Repository.DashBoard
{
    public class DashBoardRepository
    {
        private readonly OracleDapper _dapper;
        public DashBoardRepository(OracleDapper dapper)
        {
            _dapper = dapper;
            Log.Debug("DashBoardnRepository calling...");
        }

        // 소속 연구원/팀 count 
        public long GetBelongCount(DashBoardSearch model)
        {
            return _dapper.Count(DashBoardDapper.GetBelongCount(model));
        }

        // DashBoard (관리자, 연구자)  > 연구노트 리스트 조회 
        public List<Dictionary<string, string>> GetExpNoteList(DashBoardSearch model)
        {
            return _dapper.Query4NoModel(DashBoardDapper.GetExpNoteList(model));
        }

        // DashBoard (관리자, 연구자)  > 템플릿 리스트 조회
        public List<Dictionary<string, string>> GetTemplateList(DashBoardSearch model)
        {
            return _dapper.Query4NoModel(DashBoardDapper.GetTemplateList(model));
        }

        // DashBoard (관리자, 연구자)  > 템플릿 리스트 조회
        public List<Dictionary<string, string>> GetExpResultList(DashBoardSearch model)
        {
            return _dapper.Query4NoModel(DashBoardDapper.GetExpResultList(model));
        }
    }
}
