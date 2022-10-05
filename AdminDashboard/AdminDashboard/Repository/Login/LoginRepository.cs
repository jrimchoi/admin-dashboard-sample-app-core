using DSELN.DapperSql.Login;
using DSELN.Cmm.DataBase;
using DSELN.Models.Login;
using Quickwire.Attributes;
using Serilog;
using Microsoft.Extensions.DependencyInjection;

namespace DSELN.Repository.Login
{
    [RegisterService(ServiceLifetime.Scoped)]
    public class LoginRepository
    {
        private readonly OracleDapper _dapper;
        public LoginRepository(OracleDapper dapper)
        {
            _dapper = dapper;
            Log.Debug("LoginRepository calling...");
        }

        public SessionModel GetLoginInfo(LoginModel model)
        {
            return (SessionModel)_dapper.QuerySingle<SessionModel>(LoginDapperSql.GetLoginInfo(model));
        }

    }

}
