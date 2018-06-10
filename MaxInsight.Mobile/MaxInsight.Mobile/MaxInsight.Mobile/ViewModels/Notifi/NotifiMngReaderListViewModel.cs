using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module.Dto.Notifi;
using MaxInsight.Mobile.Services.NotifiService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.Notifi
{
    public class NotifiMngReaderListViewModel : ViewModel
    {

        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        INotifiMngService _notifiMngService;
        public RelayCommand ItemTappedCommand { get; set; }
        public string _paramNotice = "";
        public NotifiMngReaderListViewModel()
        {
            try
            {
                _commonFun = Resolver.Resolve<ICommonFun>();
                _commonHelper = Resolver.Resolve<CommonHelper>();
                _notifiMngService = Resolver.Resolve<INotifiMngService>();
                ItemTappedCommand = new RelayCommand(TappedCommand);

                //MessagingCenter.Unsubscribe<string>(this, MessageConst.NOTICE_READERLIST_SEARCH);
                //MessagingCenter.Subscribe<string>(this, MessageConst.NOTICE_READERLIST_SEARCH, (c) =>
                //{
                //    _paramNotice = c;
                //    SearchNoticeReadersList(c);
                //});
                MessagingCenter.Unsubscribe<string>(this, MessageConst.NOTICE_READERLIST_REFRESH);
                MessagingCenter.Subscribe<string>(this, MessageConst.NOTICE_READERLIST_REFRESH, (c) =>
                {
                    if (!string.IsNullOrWhiteSpace(_paramNotice) && _paramNotice != "0")
                        SearchNoticeReadersList(_paramNotice);
                });
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->NotifiMngReaderListViewModel");
                return;
            }
        }
        #region Property
        public List<NotifiReadersDto> _noticeReaderList;

        public List<NotifiReadersDto> NoticeReaderList
        {
            get
            {
                return _noticeReaderList;
            }
            set
            {
                SetProperty(ref _noticeReaderList, value);
            }
        }
        public NotifiReadersDto _selectedReaderItem;
        public NotifiReadersDto SelectedReaderItem
        {
            get
            {
                return _selectedReaderItem;
            }
            set
            {
                SetProperty(ref _selectedReaderItem, value);
            }
        }

        #endregion

        #region Command
        public async void TappedCommand()
        {
            try
            {

                if (SelectedReaderItem.Status != "U" && SelectedReaderItem.FeedbackYN == "Y")
                {
                    if (CommonContext.Account.UserType == "A" || CommonContext.Account.UserType == "R" || CommonContext.Account.UserType == "Z")
                    {
                        await Navigation.PushAsync<NoticeApproalViewModel>((vm, v) => vm.Init(SelectedReaderItem.ReaderId, SelectedReaderItem.Status), true);
                    }
                    else
                    {
                        await Navigation.PushAsync<NotifiFeedbackViewModel>((vm, v) => vm.Init(SelectedReaderItem.NoticeId, SelectedReaderItem.DisId, SelectedReaderItem.DepartId, SelectedReaderItem.Status, CommonContext.Account.UserId), true);
                    }
                }
                else
                {
                    await Navigation.PushAsync<NotifiMngViewModel>((vm, v) => vm.Init(SelectedReaderItem.NoticeId.ToString(), SelectedReaderItem.DisId, SelectedReaderItem.DepartId, SelectedReaderItem.Status), true);
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Method
        private async void SearchNoticeReadersList(string noticeId)
        {
            try
            {
                _commonFun.ShowLoading("查询中...");
                var now = DateTime.Now.ToString();
                var result = await _notifiMngService.SearchNoticeReaders(noticeId, CommonContext.Account.UserId);
                if (null != result && result.ResultCode == Module.ResultType.Success)
                {
                    var noticeReaderList = JsonConvert.DeserializeObject<List<NotifiReadersDto>>(result.Body);

                    if (noticeReaderList != null && noticeReaderList.Count > 0)
                    {
                        NoticeReaderList = noticeReaderList;
                    }
                    else
                    {
                        _commonFun.AlertLongText(result.Msg);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _commonFun.HideLoading();
                _commonFun.AlertLongText("请求超时,请重试");
            }
            catch (Exception)
            {
                _commonFun.HideLoading();
                _commonFun.AlertLongText("查询异常,请重试");
            }
            finally
            {
                _commonFun.HideLoading();
            }
        }
        #endregion

        #region Init
        public void Init(string noticeId = "0")
        {
            try
            {
                SearchNoticeReadersList(noticeId);
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->NotifiMngReaderListViewModel");
                return;
            }
        }
        #endregion
    }
}
