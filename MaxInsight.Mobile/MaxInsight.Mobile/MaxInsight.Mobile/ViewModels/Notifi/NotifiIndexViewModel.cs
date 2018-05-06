using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module.Dto.Notifi;
using MaxInsight.Mobile.Pages.Notifi;
using MaxInsight.Mobile.Services.NotifiService;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.Notifi
{
    public class NotifiIndexViewModel : ViewModel
    {
        INotifiMngService _notifiMngService;
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        public RelayCommand<NeedApprovalDto> ItemTappedCommand { get; set; }
        public RelayCommand<FeedBackListDto> FeedItemTappedCommand { get; set; }
        public NotifiIndexViewModel()
        {
            _notifiMngService = Resolver.Resolve<INotifiMngService>();
            _commonFun = Resolver.Resolve<ICommonFun>();
            _commonHelper = Resolver.Resolve<CommonHelper>();
            ItemTappedCommand = new RelayCommand<NeedApprovalDto>(TappedCommand);
            FeedItemTappedCommand = new RelayCommand<FeedBackListDto>(FeedTappedCommand);
            MessagingCenter.Subscribe<NotifiIndexPage>(
                this,
                MessageConst.NOTICE_MAKEDATA_GET,
                (c) =>
                {
                    //GetNofifiListOfMake();
                });

            if (CommonContext.Account.UserType == "S" || CommonContext.Account.UserType == "D")
            {
                MessagingCenter.Subscribe<NotifiIndexPage>(
                    this,
                    MessageConst.NOTICE_FEEDBDATA_GET,
                    (c) =>
                    {
                        GetNofifiListOfFeedB();
                    });
                MessagingCenter.Subscribe<string>(
                    this,
                    MessageConst.NOTICE_FEEDBACKDATA_GET,
                    (c) =>
                    {
                        GetNofifiListOfFeedB();
                    });
            }
            else
            {
                //获取待审核的通知列表
                MessagingCenter.Subscribe<string>(
                this,
                MessageConst.NOTICE_APPROALDATA_GET,
                (c) =>
                {
                    GetNofifiListOfApproal();
                });

                GetNofifiListOfApproal();
            }

            MessagingCenter.Subscribe<string>(
            this,
            "noticeApproalSearch1",
            (c) =>
            {
                GetNofifiListOfApproal();
            });

          MessagingCenter.Subscribe<string>(
           this,
           "noticeFeedBackList",
           (c) =>
           {
               GetNofifiListOfFeedB();
           });
        }
        #region Properties

        List<NoticeDto> _notifiListOfMakeData;
        List<FeedBackListDto> _feedbackinfolist;
        public List<NoticeDto> NotifiListOfMakeData
        {
            get { return _notifiListOfMakeData; }
            set { SetProperty(ref _notifiListOfMakeData, value); }
        }

        public List<FeedBackListDto> Feedbackinfolist
        {
            get { return _feedbackinfolist; }
            set { SetProperty(ref _feedbackinfolist, value); }
        }
        NoticeDto _notifiContentItem;
        public NoticeDto NotifiContentItem
        {
            get { return _notifiContentItem; }
            set { SetProperty(ref _notifiContentItem, value); }
        }

        List<NoticeDto> _notifiListOfFeedBData;
        public List<NoticeDto> NotifiListOfFeedBData
        {
            get { return _notifiListOfFeedBData; }
            set { SetProperty(ref _notifiListOfFeedBData, value); }
        }
        NoticeDto _notifiFeedBItem;
        public NoticeDto NotifiFeedBItem
        {
            get { return _notifiFeedBItem; }
            set { SetProperty(ref _notifiFeedBItem, value); }
        }
        List<NeedApprovalDto> _notifiListOfApproalData;
        public List<NeedApprovalDto> NotifiListOfApproalData
        {
            get { return _notifiListOfApproalData; }
            set { SetProperty(ref _notifiListOfApproalData, value); }
        }
        private NeedApprovalDto _notifiListOfApproalItem;
        public NeedApprovalDto NotifiListOfApproalItem
        {
            get { return _notifiListOfApproalItem; }
            set { SetProperty(ref _notifiListOfApproalItem, value); }
        }
        private FeedBackListDto _notifiFeedBackItem;
        public FeedBackListDto NotifiFeedBackItem
        {
            get { return _notifiFeedBackItem; }
            set { SetProperty(ref _notifiFeedBackItem, value); }
        }
        #endregion

        #region searchData
        public async void GetNofifiListOfMake()
        {
            if (_commonHelper.IsNetWorkConnected() == true)
            {
                try
                {
                    _commonFun.ShowLoading("查询中...");
                    var result = await _notifiMngService.GetNotifiListOfMake("NoticeList", "20160901", "20160930", CommonContext.Account.UserId, "1", "", "T");
                    if (result.ResultCode == Module.ResultType.Success)
                    {
                        _commonFun.HideLoading();
                        NotifiListOfMakeData = CommonHelper.DecodeString<List<NoticeDto>>(result.Body.Replace("\r\n", ""));
                        if (NotifiListOfMakeData == null || NotifiListOfMakeData.Count < 1)
                        {
                            _commonFun.AlertLongText("无制作中的任务通知。");
                        }
                    }
                    else
                    {
                        _commonFun.HideLoading();
                        _commonFun.AlertLongText("查询失败，请重试。 " + result.Msg);
                    }
                }
                catch (OperationCanceledException)
                {
                    _commonFun.HideLoading();
                    _commonFun.AlertLongText("请求超时。");
                }
                catch (Exception)
                {
                    _commonFun.HideLoading();
                    _commonFun.AlertLongText("查询异常，请重试。");
                }
                finally
                {
                    _commonFun.HideLoading();
                }
            }
            else
            {
                _commonFun.AlertLongText("网络连接异常。");
            }
        }

        public async void GetNofifiListOfFeedB()
        {
            if (_commonHelper.IsNetWorkConnected() == true)
            {
                try
                {
                    _commonFun.ShowLoading("查询中...");
                    var result = await _notifiMngService.searchNotiFeedBMstListByUserId(CommonContext.Account.UserId);
                    if (result.ResultCode == Module.ResultType.Success)
                    {
                        _commonFun.HideLoading();
                        Feedbackinfolist = CommonHelper.DecodeString<List<FeedBackListDto>>(result.Body.Replace("\r\n", ""));
                        if (Feedbackinfolist == null || Feedbackinfolist.Count < 1)
                        {
                            // _commonFun.AlertLongText("没有需要反馈的任务通知。");
                            _commonFun.HideLoading();
                        }
                    }
                    else
                    {
                        _commonFun.HideLoading();
                        _commonFun.AlertLongText("查询失败，请重试。 " + result.Msg);
                    }
                }
                catch (OperationCanceledException)
                {
                    _commonFun.HideLoading();
                    _commonFun.AlertLongText("请求超时。");
                }
                catch (Exception)
                {
                    _commonFun.HideLoading();
                    _commonFun.AlertLongText("查询异常，请重试。");
                }
                finally
                {
                    _commonFun.HideLoading();
                }
            }
            else
            {
                _commonFun.AlertLongText("网络连接异常。");
            }
        }
        public async void GetNofifiListOfApproal()
        {
            if (_commonHelper.IsNetWorkConnected() == true)
            {
                try
                {
                    _commonFun.ShowLoading("查询中...");
                    var result = await _notifiMngService.GetNeedApprovalDtoList(Convert.ToInt32(CommonContext.Account.UserId));
                    if (result.ResultCode == Module.ResultType.Success)
                    {
                        _commonFun.HideLoading();
                        NotifiListOfApproalData = CommonHelper.DecodeString<List<NeedApprovalDto>>(result.Body);
                        if (NotifiListOfApproalData == null || NotifiListOfApproalData.Count < 1)
                        {
                            //_commonFun.AlertLongText("没有待审核的通知");
                        }
                    }
                    else
                    {
                        _commonFun.HideLoading();
                        _commonFun.AlertLongText("查询失败，请重试。 " + result.Msg);
                    }
                }
                catch (OperationCanceledException)
                {
                    _commonFun.HideLoading();
                    _commonFun.AlertLongText("请求超时。");
                }
                catch (Exception)
                {
                    _commonFun.HideLoading();
                    _commonFun.AlertLongText("查询异常，请重试。");
                }
                finally
                {
                    _commonFun.HideLoading();
                }
            }
            else
            {
                _commonFun.AlertLongText("网络连接异常。");
            }
        }

        public async void TappedCommand(NeedApprovalDto dto)
        {
            try
            {
                await Navigation.PushAsync<NoticeApproalViewModel>((vm, v) => vm.Init(NotifiListOfApproalItem.NoticeReaderId, "W"), true);
            }
            catch (Exception)
            {
            }
        }

        public async void FeedTappedCommand(FeedBackListDto dto)
        {
            try
            {
                /*if (NotifiFeedBackItem.Status == "U")
                {
                    await Navigation.PushAsync<NotifiMngViewModel>((vm, v) => vm.Init(NotifiFeedBackItem.NoticeId.ToString(),int.Parse(NotifiFeedBackItem.DisId), int.Parse(NotifiFeedBackItem.DepartId), "Y", NotifiFeedBackItem.Status), true);
                    MessagingCenter.Send<string>(NotifiFeedBackItem.Status, MessageConst.NOTICE_MADE_SETCONTROLROLE);
                }
                else
                {
                    await Navigation.PushAsync<NotifiFeedbackViewModel>((vm, v) => vm.Init(NotifiFeedBackItem.NoticeId,int.Parse(NotifiFeedBackItem.DisId),int.Parse(NotifiFeedBackItem.DepartId), NotifiFeedBackItem.Status, CommonContext.Account.UserId.ToString()), true);
                }*/
                /* if (NotifiFeedBackItem.Status == "U")
                 {
                     await Navigation.PushAsync<NotifiMngViewModel>((vm, v) => vm.Init(NotifiFeedBackItem.NoticeId.ToString(),int.Parse(NotifiFeedBackItem.DisId), "Y", NotifiFeedBackItem.Status), true);
                     MessagingCenter.Send<string>(NotifiFeedBackItem.Status, MessageConst.NOTICE_MADE_SETCONTROLROLE);
                 }
                 else
                 {
                     await Navigation.PushAsync<NotifiFeedbackViewModel>((vm, v) => vm.Init(NotifiFeedBackItem.NoticeId,int.Parse(NotifiFeedBackItem.DisId),int.Parse(NotifiFeedBackItem.DepartId), NotifiFeedBackItem.Status, CommonContext.Account.UserId.ToString()), true);
                 }*/
                //await Navigation.PopAsync();
                //List<RequestParameter> parameterLst = new List<RequestParameter>();
                //parameterLst.Add(new RequestParameter { Name = "caseId", Value = SelectedCaseItem.Id.ToString() });
                //MessagingCenter.Send<List<RequestParameter>>(parameterLst, "GetCaseInfoDetail");
                await Navigation.PushAsync<NotifiFeedbackViewModel>((vm, v) => vm.Init(NotifiFeedBackItem.NoticeId, int.Parse(NotifiFeedBackItem.DisId), int.Parse(NotifiFeedBackItem.DepartId), NotifiFeedBackItem.Status, CommonContext.Account.UserId.ToString()), true);

            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Command
        public Command GoNotifiMakeListPageCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        await Navigation.PushAsync<NotifiMngSearchViewModel>();
                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }
        #endregion
    }
}
