using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto.Improve;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLabs.Ioc;

namespace MaxInsight.Mobile.Services.ImproveService
{
    public interface IImproveService
    {
        Task<APIResult> GetResult(string itemName, string sDate, string eDate, int inUserId, string statusType, string status, int disId, int depId, int planid,string sourceType="");
        Task<APIResult> GetImpPlanOrResultDetail(string improvementId, string searchType, string impResultId, string tPId, string itemId);
        Task<APIResult> SaveImprovementItem(string improvementContent, string expectedTime, string improvementId, string inUserId, List<AttachDto> attachList, string saveStatus);
        Task<APIResult> SaveImprovementResult(string improvementId, string impResultId, string resultStatus, string resultContent, List<AttachDto> attachList);
        Task<APIResult> SaveImproveDistribution(string tPId,string itemId,string departmentId,string improvementCaption,string lostDescription, string inUserId,bool allocateYN=false);
        Task<APIResult> GetAllTaskOfPlanForImp(string inUserId);
    }
    public class ImproveService : IImproveService
    {
        CommonHelper _commonHelper;
        Config.Config _config;
        string baseUrl = "";
        public ImproveService(Config.Config config)
        {
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _config = config;
            baseUrl = _config.Get<string>(Config.Config.Endpoints_BaseUrl);
        }
        //获取计划,结果
        public async Task<APIResult> GetResult(string itemName, string sDate, string eDate, int inUserId, string statusType, string status, int disId, int depId, int planid,string sourceType="")
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter { Name = "itemName", Value = itemName },
                new RequestParameter {Name = "sDate",Value = sDate },
                new RequestParameter {Name = "eDate",Value = eDate },
                new RequestParameter {Name = "inUserId",Value = inUserId.ToString() },
                new RequestParameter {Name = "statusType",Value = statusType },
                new RequestParameter {Name = "status",Value = status },
                new RequestParameter {Name = "disId",Value = disId.ToString() },
                new RequestParameter {Name = "depId",Value = depId.ToString() },
                new RequestParameter {Name = "planId",Value = planid.ToString() },
                 new RequestParameter {Name = "sourceType",Value = sourceType}
            };
            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.ImproveGetResult"), param);
            return info;
        }

        #region ImpPlanCommit

        public async Task<APIResult> GetImpPlanOrResultDetail(string improvementId, string searchType, string impResultId, string tPId, string itemId)
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter { Name = "ImprovementId", Value = improvementId },
                new RequestParameter {Name = "SearchType",Value = searchType },
                new RequestParameter {Name = "ImpResultId",Value = impResultId },
                new RequestParameter {Name = "TPId",Value = tPId },
                new RequestParameter {Name = "ItemId",Value = itemId }
            };
            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.GetImpPlanOrResultDetail"), param);
            return info;
        }

        public async Task<APIResult> SaveImprovementItem(string improvementContent,
                                                        string expectedTime,
                                                        string improvementId,
                                                        string inUserId,
                                                        List<AttachDto> attachList,
                                                        string saveStatus)
        {
            ImpPlanApproveDto impPlanApproveDto = new ImpPlanApproveDto
            {
                ImprovementContent = improvementContent,
                ExpectedTime = expectedTime,
                ImprovementId = int.Parse(improvementId),
                InUserId = int.Parse(CommonContext.Account.UserId),
                AttachList = attachList,
                SaveStatus = saveStatus
            };
            APIResult info = await _commonHelper.HttpPOST<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.SaveImprovementItem"), impPlanApproveDto);

            return info;
        }
        public async Task<APIResult> SaveImprovementResult(string improvementId,
                                                        string impResultId,
                                                        string resultStatus,
                                                        string resultContent,
                                                        List<AttachDto> attachList)
        {
            ImpResultApproveDto impResultApproveDto = new ImpResultApproveDto
            {
                ImprovementId = improvementId,
                ImpResultId = impResultId,
                ResultStatus = resultStatus,
                ResultContent = resultContent,
                InUserId = CommonContext.Account.UserId,
                AttachList = attachList
            };
            APIResult info = await _commonHelper.HttpPOST<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.SaveImprovementResult"), impResultApproveDto);

            return info;
        }

        public async Task<APIResult> SaveImproveDistribution(string tPId, string itemId, string departmentId, string improvementCaption, string lostDescription, string inUserId,bool allocateYN=false)
        {
            ImprovementParamDto improvementParamDto = new ImprovementParamDto
            {
                 tPId=Convert.ToInt32(tPId),
                 itemId= Convert.ToInt32(itemId),
                 departmentId= Convert.ToInt32(departmentId),
                 improvementCaption=improvementCaption,
                 lostDescription=lostDescription,
                 inUserId= Convert.ToInt32(inUserId),
                 allocateYN=allocateYN?"true":"false"
            };
            APIResult info = await _commonHelper.HttpPOST<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.InsertAllocateImprovementItem"), improvementParamDto);

            return info;
        }
        #endregion

        #region ImpTaskOfPlan
        public async Task<APIResult> GetAllTaskOfPlanForImp(string inUserId)
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter { Name = "inUserId", Value = inUserId }
            };
            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.GetAllTaskOfPlanForImp"), param);
            return info;
        }
        #endregion
    }
}
