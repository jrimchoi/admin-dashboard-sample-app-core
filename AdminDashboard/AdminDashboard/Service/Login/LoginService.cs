using DSELN.Models;
using DSELN.Models.Login;
using DSELN.Repository.Common;
using DSELN.Repository.Login;
using Microsoft.Extensions.DependencyInjection;
using Quickwire.Attributes;
using System.Collections.Generic;

namespace DSELN.Service.Login
{
    public interface ILoginService
    {
        //
    }

    [RegisterService(ServiceLifetime.Scoped)]
    public class LoginService : ILoginService
    {
        // QuickWire DI like as Spriing 
        [InjectService]
        public LoginRepository? _loginRepository { get; private set; }

        private readonly CommonRepository _cmmRepository;

        public LoginService(CommonRepository cmmRepository)
        {
            _cmmRepository = cmmRepository;
        }

        public SessionModel GetLoginInfo(LoginModel model)
        {
            return _loginRepository.GetLoginInfo(model);
        }

        public List<Dictionary<string, string>> GetUserMenu()
        {
            return _cmmRepository.GetUserMenu(new BaseSearchModel() { });
        }
    }
}
