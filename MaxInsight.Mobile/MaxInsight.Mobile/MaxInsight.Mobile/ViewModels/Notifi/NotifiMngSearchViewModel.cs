using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto.Notifi;
using MaxInsight.Mobile.Services.NotifiService;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.Notifi
{
    public class NotifiMngSearchViewModel : ViewModel
    {
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        INotifiMngService _noticeMngService;
        List<RequestParameter> paramList = new List<RequestParameter>();
        #region Constructor
        public NotifiMngSearchViewModel()
        {
            _commonFun = Resolver.Resolve<ICommonFun>();
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _noticeMngService= Resolver.Resolve<INotifiMngService>();
            MessagingCenter.Unsubscribe<List<RequestParameter>>(this, MessageConst.NOTIFI_SEARCHCONDITION_PASS);
            MessagingCenter.Subscribe<List<RequestParameter>>(this, MessageConst.NOTIFI_SEARCHCONDITION_PASS, (param) =>
            {
                if (param != null && param.Count > 0)
                {
                    paramList = param;
                    StatusSelectName = param.Find(p => p.Name == "StatusSelectName").Value;
                    ReplySelectedName = param.Find(p => p.Name == "ReplySelectedName").Value;
                    StartDateAndEndDate = param.Find(p => p.Name == "StartDate").Value+"~"+param.Find(p => p.Name == "EndDate").Value;
                    NoticeReaderDes = param.Find(p => p.Name == "NoticeReaderDes").Value;
                    NoticeNo = param.Find(p => p.Name == "NoticeNo").Value;
                    NoticeTitle = param.Find(p => p.Name == "NoticeTitle").Value;
                    GetNoticeList(param);
                }

            });
            MessagingCenter.Unsubscribe<string>(this, MessageConst.NOTIFI_SAVEREFRESH_GO);
            MessagingCenter.Subscribe<string>(this, MessageConst.NOTIFI_SAVEREFRESH_GO, (c) =>
            {
                if(paramList!=null&&paramList.Count>0)
                    GetNoticeList(paramList);
            });
        }
        #endregion
        #region Property(s)
        private string _statusSelectName;
        public string StatusSelectName
        {
            get { return _statusSelectName; }
            set { SetProperty(ref _statusSelectName, value); }
        }
        private string _replySelectedName;
        public string ReplySelectedName
        {
            get { return _replySelectedName; }
            set { SetProperty(ref _replySelectedName, value); }
        }
        private string _startDateAndEndDate;
        public string StartDateAndEndDate
        {
            get { return _startDateAndEndDate; }
            set { SetProperty(ref _startDateAndEndDate, value); }
        }

        private string _noticeReaderDes;
        public string NoticeReaderDes
        {
            get { return _noticeReaderDes; }
            set { SetProperty(ref _noticeReaderDes, value); }
        }
        private string _noticeNo;
        public string NoticeNo
        {
            get { return _noticeNo; }
            set { SetProperty(ref _noticeNo, value); }
        }
        private string _noticeTitle;
        public string NoticeTitle
        {
            get { return _noticeTitle; }
            set { SetProperty(ref _noticeTitle, value); }
        }
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }
        private RelayCommand _itemTappedCommand;
        public RelayCommand ItemTappedCommand
        {

            get
            {
                return _itemTappedCommand ?? (_itemTappedCommand = new RelayCommand(ItemTapped));
            }
        }
        private List<NoticeListInfoDto> _noticeList;
        public List<NoticeListInfoDto> NoticeList
        {
            get { return _noticeList; }
            set { SetProperty(ref _noticeList, value); }
        }
       private NoticeListInfoDto _selectedNoticeItem;
        public NoticeListInfoDto SelectedNoticeItem
        {
            get { return _selectedNoticeItem; }
            set { SetProperty(ref _selectedNoticeItem, value); }
        }
        #endregion
        #region Commmand
        public Command GoNotifiMngConditionPageCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        await Navigation.PushAsync<NotifiMngSearchConditionViewModel>((vm, v) => vm.Init(),true);
                    }
                    catch (Exception)
                    {
                    }

                });
            }
        }
        #endregion

        #region Event
        private void ItemTapped()
        {
            IsLoading = true;
            if(SelectedNoticeItem.Status=="T")
            {
                GoNoticeMngMadePage(SelectedNoticeItem);
            }
            //经销商登陆
            else if(CommonContext.Account.UserType == "S")
            {
                if (SelectedNoticeItem.FeedbackYN == "Y" && SelectedNoticeItem.NeedReply == "1"&& SelectedNoticeItem.Status != "U")
                {
                        GoNoticeFeedbackPage(SelectedNoticeItem);
                }
                else if(SelectedNoticeItem.Status== "U"||SelectedNoticeItem.Status == "R")
                {
                    GoNoticeMngMadePage(SelectedNoticeItem);
                }
                else
                {
                    GoNotifiMngReaderListPage(SelectedNoticeItem);
                }
            }
            //部门登陆  
            else if (CommonContext.Account.UserType == "D")
            {
                
                if (SelectedNoticeItem.NeedReply == "1"&& SelectedNoticeItem.Status != "U")
                {
                    GoNoticeFeedbackPage(SelectedNoticeItem);
                }
                else 
                {
                    GoNoticeMngMadePage(SelectedNoticeItem);
                }
            }
            else//跳转至通知对象列表
            {
                GoNotifiMngReaderListPage(SelectedNoticeItem);
            }
            IsLoading = false;
        }
        private async void GoNoticeMngMadePage(NoticeListInfoDto noticeListInfoDto)
        {
            try
            {
                await Navigation.PushAsync<NotifiMngViewModel>((vm, v) => vm.Init(noticeListInfoDto.NoticeId.ToString(), noticeListInfoDto.DisId, noticeListInfoDto.DepartId,  noticeListInfoDto.Status), true);            
            }
            catch (Exception)
            {
            }
        }
        private async void GoNotifiMngReaderListPage(NoticeListInfoDto noticeListInfoDto)
        {
            try
            {
                //CommonContext.ImpPlanStatus = noticeListInfoDto.Status;
                await Navigation.PushAsync<NotifiMngReaderListViewModel>((vm,v)=>vm.Init(noticeListInfoDto.NoticeId.ToString()),true);
            }
            catch (Exception)
            {
            }
        }
        private async void GoNoticeFeedbackPage(NoticeListInfoDto noticeListInfoDto)
        {
            try
            {
                await Navigation.PushAsync<NotifiFeedbackViewModel>((vm, v) => vm.Init(noticeListInfoDto.NoticeId, noticeListInfoDto.DisId, noticeListInfoDto.DepartId, noticeListInfoDto.Status, CommonContext.Account.UserId), true);

            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region GetData
        private async void GetNoticeList(List<RequestParameter> param)
        {
            try
            {
                _commonFun.ShowLoading("查询中...");
                string fromDate = param.Find(p => p.Name == "StartDate").Value.Replace("-", "");
                string toDate = param.Find(p => p.Name == "EndDate").Value.Replace("-", "");
                string noticeReaders = param.Find(p => p.Name == "NoticeReaders").Value;
                string status = param.Find(p => p.Name == "StatusSelectIndex").Value;
                string needReply = param.Find(p => p.Name == "ReplySelected").Value;
                string title = param.Find(p => p.Name == "NoticeTitle").Value;
                string noticeNo = param.Find(p => p.Name == "NoticeNo").Value;
                string inUserId = CommonContext.Account.UserId;
                //TO-DO
                var result = await _noticeMngService.SearchMadeNotifiList(fromDate,
                                                   toDate,
                                                   noticeReaders,
                                                   status,
                                                   needReply,
                                                   title,
                                                   noticeNo,
                                                   inUserId);
                if (result.ResultCode == Module.ResultType.Success)
                {

                    List<NoticeListInfoDto> noticeList = CommonHelper.DecodeString<List<NoticeListInfoDto>>(result.Body);
                    if (noticeList != null && noticeList.Count > 0)
                    {
                        _commonFun.HideLoading();
                        NoticeList = noticeList;
                    }
                    else
                    {
                        _commonFun.HideLoading();
                        NoticeList = new List<NoticeListInfoDto>();
                        _commonFun.ShowToast("没有数据");
                    }
                }
                else
                {
                    _commonFun.HideLoading();
                    NoticeList = new List<NoticeListInfoDto>();
                    _commonFun.AlertLongText("查询失败，请重试。 " + result.Msg);
                }
            }
            catch (OperationCanceledException)
            {
                _commonFun.HideLoading();
                NoticeList = new List<NoticeListInfoDto>();
                _commonFun.AlertLongText("请求超时。");
            }
            catch (Exception)
            {
                _commonFun.HideLoading();
                NoticeList = new List<NoticeListInfoDto>();
                _commonFun.AlertLongText("查询异常，请重试。");
            }
            finally
            {
                _commonFun.HideLoading();
            }
        }

        #endregion
    }
}
