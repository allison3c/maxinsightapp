using MaxInsight.Mobile.Module;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using MaxInsight.Mobile.Helpers;
using XLabs.Ioc;
using MaxInsight.Mobile.Module.Dto.Notifi;

namespace MaxInsight.Mobile.Services.NotifiService
{
    public interface INotifiMngService
    {
        Task<APIResult> GetNotifiListOfMake(string searchType, string fromDate, string toDate, string inUserID, string needReply = "", string noticeReaders = "", string status = "", string title = "", string noticeNo = "");
        Task<APIResult> GetNotifiListOfFeebB(string type, string sDate, string eDate, string userID, string replyYN = "", string approvalStatus = "", string title = "", string noticeNo = "");
        //暂存或者提交通知
        Task<APIResult> SaveMadeNotifi(string noticeTitle,
                                       string sDate,
                                       string eDate,
                                       string disdepList,
                                       string reply,
                                       string noticeContent,
                                       List<AttachDto> attachmentList,
                                       string saveSatus,
                                       int noticeId,
                                       int inUserId);
        //通知查询
        Task<APIResult> SearchMadeNotifiList(string fromDate,
                                                   string toDate,
                                                   string noticeReaders,
                                                   string status,
                                                   string needReply,
                                                   string title,
                                                   string noticeNo,
                                                   string inUserId);
        //通知详细
        Task<APIResult> SearchMadeNoticeDetailInfo(string noticeId);
        Task<APIResult> UpdateReaderReadStatus(NoticeReadStatusDto readStatusDto);
        Task<APIResult> SearchNoticeReaders(string noticeId, string inUserId);

        //通知审核
        Task<APIResult> NoticeApprovalS(NeedApproalParams dto);
        //通知审核List查询

        Task<APIResult> GetNeedApprovalDtoList(int userId);
        //查询通知审核详细
        Task<APIResult> GetNoticeApprovalDetail(int noticeReaderId);
        //查询审核Log
        Task<APIResult> GetNoticeApprovalLog(int noticeReaderId);
        //Task<APIResult> SearchNoticeReaders(string noticeId);
        Task<APIResult> searchNotiFeedBMstListByUserId(string UserId);
        Task<APIResult> SaveFeedBackNotice(FeedBackInfoDto dto);
        Task<APIResult> SearchNoticeFeedBackDtl(string NoticeId, string UserId, string DisId, string DepartId);
        Task<APIResult> AllItemsForMobile(string UserId);
    }
    public class NotifiMngService : INotifiMngService
    {
        CommonHelper _commonHelper;
        Config.Config _config;
        string baseUrl = "";
        public NotifiMngService(Config.Config config)
        {
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _config = config;
            baseUrl = _config.Get<string>(Config.Config.Endpoints_BaseUrl);
        }
        public async Task<APIResult> GetNotifiListOfMake(string searchType, string fromDate, string toDate, string inUserID, string needReply = "", string noticeReaders = "", string status = "", string title = "", string noticeNo = "")
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter { Name = "SearchType", Value = searchType },
                new RequestParameter {Name = "FromDate",Value = fromDate },
                new RequestParameter {Name = "ToDate",Value = toDate },
                new RequestParameter {Name = "NoticeReaders",Value = noticeReaders },
                new RequestParameter {Name = "Status",Value = status },
                new RequestParameter {Name = "NeedReply",Value = needReply },
                new RequestParameter {Name = "InUserID",Value = inUserID },
                new RequestParameter {Name = "Title",Value = title },
                new RequestParameter {Name = "NoticeNo",Value = noticeNo }
            };
            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.NotifiMng"), param);
            return info;
        }
        public async Task<APIResult> GetNotifiListOfFeebB(string type, string sDate, string eDate, string userID, string replyYN = "", string approvalStatus = "", string title = "", string noticeNo = "")
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter { Name = "Type", Value = type },
                new RequestParameter {Name = "SDate",Value = sDate },
                new RequestParameter {Name = "EDate",Value = eDate },
                new RequestParameter {Name = "UserID",Value = userID },
                new RequestParameter {Name = "ReplyYN",Value = replyYN },
                new RequestParameter {Name = "ApprovalStatus",Value = approvalStatus },
                new RequestParameter {Name = "Title",Value = title },
                new RequestParameter {Name = "NoticeNo",Value = noticeNo }
            };
            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.NotifiFeedB"), param);
            return info;
        }
        //暂存或者提交通知
        public async Task<APIResult> SaveMadeNotifi(string noticeTitle,
                                                    string sDate,
                                                    string eDate,
                                                    string disdepList,
                                                    string reply,
                                                    string noticeContent,
                                                    List<AttachDto> attachmentList,
                                                    string saveSatus,
                                                    int noticeId,
                                                    int inUserId)
        {
            NoticeInfoDto paramDto = new NoticeInfoDto()
            {
                Title = noticeTitle,
                SDate = sDate,
                EDate = eDate,
                NeedReply = reply,
                Content = noticeContent,
                Status = saveSatus,
                InUserId = inUserId,
                NoticeId = noticeId,
                NoticeReaders = disdepList,
                AttachList = attachmentList
            };
            APIResult info = await _commonHelper.HttpPOST<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.SaveMadeNotifi"), paramDto);
            return info;
        }
        //通知查询
        public async Task<APIResult> SearchMadeNotifiList(string fromDate,
                                                   string toDate,
                                                   string noticeReaders,
                                                   string status,
                                                   string needReply,
                                                   string title,
                                                   string noticeNo,
                                                   string inUserId)
        {
            SearchParamDto paramDto = new SearchParamDto();
            paramDto.FromDate = fromDate;
            paramDto.ToDate = toDate;
            paramDto.NoticeReaders = noticeReaders;
            paramDto.Status = status;
            paramDto.NeedReply = needReply;
            paramDto.Title = title;
            paramDto.NoticeNo = noticeNo;
            paramDto.InUserId = inUserId;
            APIResult info = await _commonHelper.HttpPOST<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.SearchMadeNoticeList"), paramDto);
            return info;
        }
        //通知详细
        public async Task<APIResult> SearchMadeNoticeDetailInfo(string noticeId)
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter {Name = "id",Value = noticeId }
            };
            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.SearchMadeNoticeInfo"), param);
            return info;
        }
        //将未读变更为已读
        public async Task<APIResult> UpdateReaderReadStatus(NoticeReadStatusDto readStatusDto)
        {
            APIResult info = await _commonHelper.HttpPOST<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.UpdateReaderReadStatus"), readStatusDto);
            return info;
        }
        //查询通知对象
        public async Task<APIResult> SearchNoticeReaders(string noticeId, string inUserId)
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter {Name = "noticeId",Value = noticeId },
                new RequestParameter {Name = "inUserId",Value = inUserId }
            };
            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.GetNoticeReaders"), param);
            return info;
        }
        //通知审核
        public async Task<APIResult> NoticeApprovalS(NeedApproalParams dto)
        {
            APIResult info = await _commonHelper.HttpPOST<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.NoticeApprovalS"), dto);
            return info;
        }
        //待审核的通知
        public async Task<APIResult> GetNeedApprovalDtoList(int userId)
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter {Name = "userId",Value = userId.ToString() }
            };

            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.GetNeedApprovalDtoList"), param);
            return info;
        }

        public async Task<APIResult> GetNoticeApprovalDetail(int noticeReaderId)
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter {Name = "noticeReaderId",Value = noticeReaderId.ToString() }
            };

            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.GetNoticeApprovalDetail"), param);
            return info;
        }

        public async Task<APIResult> GetNoticeApprovalLog(int noticeReaderId)
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter {Name = "noticeReaderId",Value = noticeReaderId.ToString() }
            };

            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.GetNoticeApprovalLog"), param);
            return info;
        }
        //根据登录名查询出需要反馈的通知List
        public async Task<APIResult> searchNotiFeedBMstListByUserId(string UserId)
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter {Name = "UserID",Value = UserId }
            };
            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.SearchNotiFeedBMstListByUserId"), param);
            return info;
        }
        public async Task<APIResult> SaveFeedBackNotice(FeedBackInfoDto dto)
        {
            APIResult info = await _commonHelper.HttpPOST<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.SaveFeedBackNotice"), dto);
            return info;
        }
        public async Task<APIResult> SearchNoticeFeedBackDtl(string NoticeId, string UserId, string disId, string departId)
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter {Name = "NoticeId",Value = NoticeId },
                new RequestParameter {Name = "UserId",Value = UserId },
                new RequestParameter {Name = "DisId",Value = disId },
                new RequestParameter {Name = "DepartId",Value = departId }
            };
            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.SearchNoticeFeedBackDtl"), param);
            return info;
        }

        public async Task<APIResult> AllItemsForMobile(string UserId)
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter {Name = "UserId",Value = UserId }
            };
            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.AllItemsForMobile"), param);
            return info;
        }
    }
}
