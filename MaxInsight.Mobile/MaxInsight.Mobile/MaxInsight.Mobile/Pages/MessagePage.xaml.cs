using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto;
using MaxInsight.Mobile.Module.Dto.Improve;
using MaxInsight.Mobile.Module.Dto.Notifi;
using MaxInsight.Mobile.Pages.Cases;
using MaxInsight.Mobile.Pages.Improve;
using MaxInsight.Mobile.Pages.Notifi;
using MaxInsight.Mobile.Pages.ReviewPlans;
using MaxInsight.Mobile.Services.NotifiService;
using MaxInsight.Mobile.ViewModels.Cases;
using MaxInsight.Mobile.ViewModels.Improve;
using MaxInsight.Mobile.ViewModels.Notifi;
using MaxInsight.Mobile.ViewModels.ReviewPlans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.Pages
{
    public partial class MessagePage
    {
        CommonHelper _commonHelper;
        INotifiMngService _noticeMngService;
        public MessagePage()
        {
            InitializeComponent();
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _noticeMngService = Resolver.Resolve<INotifiMngService>();


            MessagingCenter.Unsubscribe<string>(this, "MessagePageReSearch");
            MessagingCenter.Subscribe<string>(this, "MessagePageReSearch", (c) =>
            {
                SetData();
            });
        }

        private async void SetData()
        {
            lstMessages.ItemsSource = await GetMessageData2();
        }

        //private async void GoMessageItemDetail(object sender, ItemTappedEventArgs args)
        //{
        //    var notice = (args.Item as NoticeListInfoDto);
        //    try
        //    {
        //        await Navigation.PushAsync(ViewFactory.CreatePage<NotifiMngViewModel, NotifiMngPage>((vm, v) =>
        //        vm.Init(notice.NoticeId.ToString(), notice.DisId, notice.DepartId, feedbackYN: notice.FeedbackYN, noticeStatus: notice.Status)) as Page, true);
        //        MessagingCenter.Send<string>(notice.Status, MessageConst.NOTICE_MADE_SETCONTROLROLE);
        //    }
        //    catch (System.Exception)
        //    {
        //    }
        //}

        private async void GoMessageItemDetail(object sender, ItemTappedEventArgs args)
        {
            var notice = (args.Item as MessageDto);
            try
            {
                switch (notice.DataType)
                {
                    //通知反馈
                    case "ANoticeF":
                        if (notice.Id.Split(',').Length > 4)
                        {
                            string[] ids = notice.Id.Split(',');
                            await Navigation.PushAsync(ViewFactory.CreatePage<NotifiFeedbackViewModel, NotifiFeedbackPage>((vm, v) =>
                            vm.Init(Convert.ToInt32(ids[0]), int.Parse(ids[2]), int.Parse(ids[3]), notice.Status)) as Page, true);
                        }
                        break;
                    //通知审核
                    case "ANoticeA":
                        await Navigation.PushAsync(ViewFactory.CreatePage<NoticeApproalViewModel, NoticeApproalPage>((vm, v) =>
                              vm.Init(Convert.ToInt32(notice.Id), "W")) as Page, true);
                        break;
                    //计划任务审批
                    case "Plan":
                        await Navigation.PushAsync(ViewFactory.CreatePage<ReviewPlansViewModel, ReviewPlansDtlPage>((vm, v) =>
                        vm.Init(notice.Id)) as Page, true);
                        break;
                    //改善计划审批
                    case "BImpA":

                        if (notice.Id.Split(',').Length > 5)
                        {
                            string[] ids = notice.Id.Split(',');
                            List<RequestParameter> list = new List<RequestParameter>();
                            list.Add(new RequestParameter { Name = "improvementId", Value = ids[0] });
                            list.Add(new RequestParameter { Name = "impResultId", Value = "0" });
                            list.Add(new RequestParameter { Name = "tPId", Value = ids[1] });
                            list.Add(new RequestParameter { Name = "itemId", Value = ids[2] });
                            list.Add(new RequestParameter { Name = "planApproalYN", Value = ids[3] });
                            list.Add(new RequestParameter { Name = "PLANSTATUS", Value = notice.Status });
                            list.Add(new RequestParameter { Name = "AllocateYN", Value = ids[5] });
                            ImprovementMngDto paramDto = new ImprovementMngDto();
                            paramDto.ImprovementId =Convert.ToInt32(ids[0]);
                            paramDto.ImpResultId = 0;
                            paramDto.TPId= Convert.ToInt32(ids[1]);
                            paramDto.ItemId = Convert.ToInt32(ids[2]);
                            await Navigation.PushAsync(ViewFactory.CreatePage<ImpPlanCommitViewModel, ImpPlanCommitPage>((vm, v) => vm.Init(paramDto,list)) as Page, true);
                        }
                        break;
                    //Push详细    
                    case "Push":
                        await Navigation.PushAsync(ViewFactory.CreatePage<PushInfoViewModel, PushInfoPage>((vm, v) =>
                        vm.Init(notice.Id)) as Page, true);
                        break;
                    //改善结果审批
                    case "CImpR":
                        if (notice.Id.Split(',').Length == 7)
                        {
                            string[] ids = notice.Id.Split(',');
                            List<RequestParameter> list = new List<RequestParameter>();

                            list.Add(new RequestParameter { Name = "improvementId", Value = ids[1] });
                            list.Add(new RequestParameter { Name = "impResultId", Value = ids[0] });
                            list.Add(new RequestParameter { Name = "tPId", Value = ids[2] });
                            list.Add(new RequestParameter { Name = "itemId", Value = ids[3] });
                            list.Add(new RequestParameter { Name = "ResultApproalYN", Value = ids[5] });
                            list.Add(new RequestParameter { Name = "ResultStatus", Value = notice.Status });
                            list.Add(new RequestParameter { Name = "AllocateYN", Value = ids[6] });
                            ImprovementMngDto paramDto = new ImprovementMngDto();
                            paramDto.ImprovementId = Convert.ToInt32(ids[1]);
                            paramDto.ImpResultId = Convert.ToInt32(ids[0]);
                            paramDto.TPId = Convert.ToInt32(ids[2]);
                            paramDto.ItemId = Convert.ToInt32(ids[3]);
                            paramDto.PlanApproalYN = Convert.ToBoolean(ids[4]);
                            paramDto.PlanStatus = "G";
                            paramDto.AllocateYN= Convert.ToBoolean(ids[6]);
                            await Navigation.PushAsync(ViewFactory.CreatePage<ImpResultCommitViewModel, ImpResultCommitPage>((vm, v) => vm.Init(paramDto, list)) as Page, true);
                        }
                        break;

                    default:
                        //未读通知
                        if (notice.Id.Split(',').Length == 5)
                        {
                            string[] ids = notice.Id.Split(',');
                            if (ids[4] == "Y")// 需要反馈的通知，直接跳转到通知反馈页面
                            {
                                await Navigation.PushAsync(ViewFactory.CreatePage<NotifiFeedbackViewModel, NotifiFeedbackPage>((vm, v) =>
                                vm.Init(Convert.ToInt32(ids[0]), int.Parse(ids[1]), int.Parse(ids[2]), notice.Status)) as Page, true);
                            }
                            else
                            {
                                await Navigation.PushAsync(ViewFactory.CreatePage<NotifiMngViewModel, NotifiMngPage>((vm, v) =>
                                vm.Init(ids[0].ToString(), Convert.ToInt32(ids[1]), Convert.ToInt32(ids[2]), noticeStatus: notice.Status)) as Page, true);
                            }
                        }
                        break;
                }
            }
            catch (System.Exception)
            {
            }
        }

        private async Task<List<NoticeListInfoDto>> GetMessageData()
        {
            try
            {
                var result = await _noticeMngService.SearchMadeNotifiList("19000101",
                                                   "21001231",
                                                   "",
                                                   "U",
                                                   "1",
                                                   "",
                                                   "",
                                                   CommonContext.Account.UserId);
                if (result.ResultCode == Module.ResultType.Success)
                {

                    List<NoticeListInfoDto> noticeList = CommonHelper.DecodeString<List<NoticeListInfoDto>>(result.Body);
                    int i = 1;
                    noticeList.Select(c => { c.SeqNo = i++; return c; }).ToList();
                    return noticeList;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        private async Task<List<MessageDto>> GetMessageData2()
        {
            try
            {
                var result = await _noticeMngService.AllItemsForMobile(CommonContext.Account.UserId);
                if (result.ResultCode == Module.ResultType.Success)
                {

                    ItemResultDto dto = CommonHelper.DecodeString<ItemResultDto>(result.Body);
                    List<MessageDto> list = new List<MessageDto>();
                    if (dto != null)
                    {
                        foreach (var item in dto.SecondItemList)
                        {
                            if (!string.IsNullOrEmpty(item.Title))
                            {
                                list.Add(new MessageDto() { MessageContent = "[" + item.TypeName + "]" + item.Title, Id = item.Id, Status = item.Status, DataType = item.DataType });
                            }
                        }
                        foreach (var item in dto.ThirdItemList)
                        {
                            if (!string.IsNullOrEmpty(item.Title))
                            {
                                list.Add(new MessageDto() { MessageContent = "[未读通知]" + item.Title.Substring(item.Title.IndexOf(',') + 1), Id = item.Id, Status = item.Status, DataType = item.DataType });
                            }
                        }
                    }

                    int i = 1;
                    list.Select(c => { c.SeqNo = i++; return c; }).ToList();
                    return list;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SetData();
        }
    }
}
