using DSELN.DapperSql.CodeMng;
using DSELN.Cmm.DataBase;
using DSELN.Models.Common;
using DSELN.Models.CodeMng;
using DSELN.Models;
using Serilog;
using DSELN.DapperSql.SysMng;
using System.Collections.Generic;

namespace DSELN.Repository.SysMng
{
    public class SysMngRepository
    {
        private readonly OracleDapper _dapper;
        public SysMngRepository(OracleDapper dapper)
        {
            _dapper = dapper;
        }

        // 메뉴등록 조회 
        public List<Menu> GetMenuList(MenuSearch model)
        {
            return _dapper.Query<Menu>(SysMngDapper.GetMenuList(model));
        }

        public int MenuInsert(Menu model)
        {
            return _dapper.Execute(SysMngDapper.MenuInsert(model));
        }

        public int MenuUpdate(Menu model)
        {
            return _dapper.Execute(SysMngDapper.MenuUpdate(model));
        }

        public int MenuDelete(Menu model)
        {
            return _dapper.Execute(SysMngDapper.MenuDelete(model));
        }

    }
}
