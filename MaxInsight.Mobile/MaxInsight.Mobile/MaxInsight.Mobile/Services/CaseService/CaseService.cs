using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto.Case;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLabs.Ioc;

namespace MaxInsight.Mobile.Services.CaseService
{
    public interface ICaseService
    {
        Task<APIResult> GetCaseList(int inUserId, string sDate, string eDate, string caseType, string content);
        Task<APIResult> GetCaseDetailInfo(string caseId);
        Task<APIResult> GetTypeFromHiddenCode(string GroupCode);
        Task<APIResult> InsertOrUpdateCasesInfo(CasesParamDto caseDto);
        Task<APIResult> DeleteCasesInfo(CasesDelParamDto delDto);

    }
    public class CaseService : ICaseService
    {
        CommonHelper _commonHelper;
        Config.Config _config;
        string baseUrl = "";
        public CaseService(Config.Config config)
        {
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _config = config;
            baseUrl = _config.Get<string>(Config.Config.Endpoints_BaseUrl);
        }
        public async Task<APIResult> GetCaseList(int inUserId, string sDate, string eDate, string caseType, string content)
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter { Name = "inUserId", Value = inUserId.ToString() },
                new RequestParameter {Name = "sDate",Value = sDate },
                new RequestParameter {Name = "eDate",Value = eDate },
                new RequestParameter {Name = "caseType",Value = caseType },
                new RequestParameter {Name = "content",Value = content }
            };
            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.CaseSearch"), param);
            return info;
        }

        public async Task<APIResult> GetTypeFromHiddenCode(string GroupCode)
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter { Name = "GroupCode", Value = GroupCode }
            };
            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.GetTypeFromHiddenCode"), param);
            return info;
        }

        public async Task<APIResult> GetCaseDetailInfo(string caseId)
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter { Name = "caseId", Value = caseId }
            };
            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.CaseSearch"), param);
            return info;
        }

        public async Task<APIResult> InsertOrUpdateCasesInfo(CasesParamDto caseDto)
        {
            var info = await _commonHelper.HttpPOST<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.InsertOrUpdateCasesInfo"), caseDto);
            return info;
        }

        public async Task<APIResult> DeleteCasesInfo(CasesDelParamDto delDto)
        {
            var info = await _commonHelper.HttpPOST<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.CaseDelete"), delDto);
            return info;
        }
    }
}
