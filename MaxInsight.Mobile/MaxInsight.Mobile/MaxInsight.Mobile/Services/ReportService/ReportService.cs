using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLabs.Ioc;

namespace MaxInsight.Mobile.Services.ReportService
{
    public interface IReportService
    {
        Task<APIResult> GetAttachmentByUserId(int userId, string sourceType, string sDate, string eDate);
        Task<APIResult> UpdateAttachmentDownloadCnt(int id);

    }
    public class ReportService : IReportService
    {
        CommonHelper _commonHelper;
        Config.Config _config;
        string baseUrl = "";
        public ReportService(Config.Config config)
        {
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _config = config;
            baseUrl = _config.Get<string>(Config.Config.Endpoints_BaseUrl);
        }
        public async Task<APIResult> GetAttachmentByUserId(int userId, string sourceType, string sDate, string eDate)
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter { Name = "userId", Value = userId.ToString() },
                new RequestParameter {Name = "sDate",Value = sDate },
                new RequestParameter {Name = "eDate",Value = eDate },
                new RequestParameter {Name = "sourceType",Value = sourceType }
            };
            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.GetAttachmentByUserId"), param);
            return info;
        }

        public async Task<APIResult> UpdateAttachmentDownloadCnt(int id)
        {
            var info = await _commonHelper.HttpPOST<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.UpdateAttachmentDownloadCnt"), id);
            return info;
        }
    }
}
