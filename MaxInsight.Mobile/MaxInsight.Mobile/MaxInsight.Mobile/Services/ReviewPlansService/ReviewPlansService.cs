    using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto.ReviewPlans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLabs.Ioc;

namespace MaxInsight.Mobile.Services.ReviewPlansService
{

    public interface IReviewPlansService
    {
        Task<APIResult> GetReviewPlansList_Mobile(string userId);
        Task<APIResult> GetPlansDtlList(string Id);
        Task<APIResult> ReviewPlans(string Id, string PStatus, string RejectReason, string UserId);
        Task<APIResult> GetPushInfo(string Id);
    }
    public class ReviewPlansService : IReviewPlansService
    {
        CommonHelper _commonHelper;
        Config.Config _config;
        string baseUrl = "";
        public ReviewPlansService(Config.Config config)
        {
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _config = config;
            baseUrl = _config.Get<string>(Config.Config.Endpoints_BaseUrl);
        }

        public async Task<APIResult> GetReviewPlansList_Mobile(string userId)
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter { Name = "UserId", Value = userId }
            };
            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.GetReviewPlansList"), param);
            return info;
        }

        public async Task<APIResult> GetPlansDtlList(string Id)
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter { Name = "Id", Value = Id }
            };
            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.GetPlansDtlList"), param);
            return info;
        }
        public async Task<APIResult> ReviewPlans(string id, string pStatus, string rejectReason, string userId)
        {
            ReviewPlansDto paramDto = new ReviewPlansDto()
            {
                Id = id,
                PStatus = pStatus,
                RejectReason = rejectReason,
                UserId = userId
            };
            APIResult info = await _commonHelper.HttpPOST<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.ReviewPlans"), paramDto);
            return info;
        }
        public async Task<APIResult> GetPushInfo(string Id)
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter { Name = "Id", Value = Id }
            };
            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.GetPushInfo"), param);
            return info;

        }

    }
}
