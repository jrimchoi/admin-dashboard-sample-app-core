using DSELN.Cmm.Helper;
using DSELN.Cmm.Utils;
using DSELN.Models;
using DSELN.Models.CodeMng;
using DSELN.Models.DashBoard;
using DSELN.Models.Sample;
using DSELN.Repository.CodeMng;
using DSELN.Repository.Common;
using DSELN.Repository.DashBoard;
using DSELN.Service.Common;
using Serilog;
using System.Collections.Generic;

namespace DSELN.Service.DashBoard
{
    public interface IDashBoardService
    {
        List<Dictionary<string, string>> GetExpNoteList(DashBoardSearch model);
        List<Dictionary<string, string>> GetTemplateList(DashBoardSearch model);
        List<Dictionary<string, string>> GetExpResultList(DashBoardSearch model);
        bool CheckAuthDashBoard4Admin(DashBoardSearch model);
        bool CheckAuthDashBoardData(DashBoardSearch model);
    }

    public class DashBoardService : IDashBoardService
    {
        private readonly DashBoardRepository _repository;

        public DashBoardService(DashBoardRepository repository)
        {
            _repository = repository;
        }

        // DashBoard (관리자) 권한체크 
        public bool CheckAuthDashBoard4Admin(DashBoardSearch model)
        {
            // 관리자, 소장, 팀장 : 대시보드(관리자) 
            if (Role.ADMIN.Equals(model.SessionInfo.USER_ROLE) || Position.DIRECTOR.Equals(model.SessionInfo.USER_POSITION_CD) || Position.TEAM_LEADER.Equals(model.SessionInfo.USER_POSITION_CD))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // DashBoard 데이터 접근 권한 체크 
        public bool CheckAuthDashBoardData(DashBoardSearch model)
        {
            // 조회시 로그인 사용자의 팀(하위팀)인지 체크 
            long count = _repository.GetBelongCount(model);

            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // DashBoard (관리자, 연구자)  > 연구노트 리스트 조회 
        public List<Dictionary<string, string>> GetExpNoteList(DashBoardSearch model)
        {
            return _repository.GetExpNoteList(model);
        }

        // DashBoard (관리자, 연구자)  > 템플릿 리스트 조회
        public List<Dictionary<string, string>> GetTemplateList(DashBoardSearch model)
        {
            return _repository.GetTemplateList(model);
        }

        // DashBoard (관리자, 연구자)  > 실험결과 리스트 조회
        public List<Dictionary<string, string>> GetExpResultList(DashBoardSearch model)
        {
            return _repository.GetExpResultList(model);
        }

    }
}
