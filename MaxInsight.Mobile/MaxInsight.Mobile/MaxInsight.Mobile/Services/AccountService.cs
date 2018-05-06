using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Services
{
    public interface IAccountService
    {
        Task<APIResult> Login(string userName, string password);
        Task<APIResult> UpdatePsw(string userId, string newPassword);
    }
    public class AccountService : IAccountService
    {
        CommonHelper _commonHelper;
        Config.Config _config;
        string baseUrl = "";
        public AccountService(Config.Config config)
        {
            _commonHelper = new CommonHelper(config);
            _config = config;
            baseUrl = _config.Get<string>(Config.Config.Endpoints_BaseUrl);
        }
        public async Task<APIResult> Login(string userName, string password)
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter { Name = "UserName", Value = userName },
                new RequestParameter {Name = "Password",Value = password }
            };
            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.account"), param);
            return info;
        }

        public async Task<APIResult> UpdatePsw(string userId, string newPassword)
        {
            EmpPswDto empPswDto = new EmpPswDto
            {
                UserId = userId,
                NewPassword = newPassword
            };
            APIResult info = await _commonHelper.HttpPOST<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.UpdatePsw"), empPswDto);

            return info;
        }
    }
}
