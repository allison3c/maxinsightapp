using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto.Notifi;
using MaxInsight.Mobile.Services.NotifiService;
using Newtonsoft.Json;
using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.Notifi
{
    class NoticeApproalViewModel : ViewModel
    {
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        INotifiMngService _notifiMngService;

        #region Constructor
        public NoticeApproalViewModel()
        {
            _commonFun = Resolver.Resolve<ICommonFun>();
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _notifiMngService = Resolver.Resolve<INotifiMngService>();


            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    GetNoticeApproalDetail(NotifiContentItem.NoticeReaderId);

            //});
        }
        #endregion

        #region properties

        private int _noticeReaderId;
        public int NoticeReaderId
        {
            get
            {
                return _noticeReaderId;
            }
            set
            {
                SetProperty(ref _noticeReaderId, value);
            }
        }
        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                SetProperty(ref _status, value);
            }
        }
        private int _noticeId;
        public int NoticeId
        {
            get
            {
                return _noticeId;
            }
            set
            {
                SetProperty(ref _noticeId, value);
            }
        }

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                SetProperty(ref _title, value);
            }
        }
        private string _content;
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                SetProperty(ref _content, value);
            }
        }
        private string _feedbackContent;
        public string FeedbackContent
        {
            get
            {
                return _feedbackContent;
            }
            set
            {
                SetProperty(ref _feedbackContent, value);
            }
        }

        private string _replyContent;

        public string ReplyContent
        {
            get
            {
                return _replyContent;
            }
            set
            {
                SetProperty(ref _replyContent, value);
            }
        }

        private List<AttachDto> _attachList;
        public List<AttachDto> AttachList
        {
            get
            {
                return _attachList;
            }
            set
            {
                SetProperty(ref _attachList, value);
            }
        }

        private NeedApprovalDto _notifiContentItem;
        public NeedApprovalDto NotifiContentItem
        {
            get
            {
                return _notifiContentItem;
            }
            set
            {
                SetProperty(ref _notifiContentItem, value);
            }
        }

        private AttachDto _feedBackAttachItem;
        public AttachDto FeedBackAttachItem
        {
            get
            {
                return _feedBackAttachItem;
            }
            set
            {
                SetProperty(ref _feedBackAttachItem, value);
            }
        }

        double _lstRowHeight = 32;
        double _lstHeight;
        public double LstHeight
        {
            get { return _lstHeight; }
            set { SetProperty(ref _lstHeight, value); }
        }
        private bool _isEditable;
        public bool IsEditable
        {
            get { return _isEditable; }
            set { SetProperty(ref _isEditable, value); }
        }

        #endregion
        #region command

        private RelayCommand _approalCommand;
        public RelayCommand ApproalCommand
        {
            get
            {
                return _approalCommand ??

                (_approalCommand = new RelayCommand(Approal));
            }
        }
        private RelayCommand _rejectCommand;
        public RelayCommand RejectCommand
        {
            get
            {
                return _rejectCommand ??

                (_rejectCommand = new RelayCommand(Reject));
            }
        }
        public Command ItemTappedCommand
        {
            get
            {
                return new Command(() =>
                {
                    PreviewImage();
                });
            }
        }
        private RelayCommand _approalLogCommand;
        public RelayCommand ApproalLogCommand
        {
            get
            {
                return _approalLogCommand ??

                (_approalLogCommand = new RelayCommand(GotoApproalLogPage));
            }
        }
        private RelayCommand _noticeDetailCommand;
        public RelayCommand NoticeDetailCommand
        {
            get
            {
                return _noticeDetailCommand ??

                (_noticeDetailCommand = new RelayCommand(GotoNoticeDetailPage));
            }
        }
        #endregion
        #region method

        private void Approal()
        {
            ApproalSave(true);
        }

        private void Reject()
        {
            ApproalSave(false);
        }

        private void PreviewImage()
        {
            try
            {
                List<ImagePreviewDto> list = new List<ImagePreviewDto>();
                string url = FeedBackAttachItem.Url.ToUpper();
                if (url.EndsWith(".JPG") || url.EndsWith(".BMP") || url.EndsWith(".GIF") ||
                    url.EndsWith(".JPEG") || url.EndsWith(".PNG") || url.EndsWith(".SVG"))
                {
                    string filename = FeedBackAttachItem.Url.LastIndexOf("/") > 0 ? FeedBackAttachItem.Url.Substring(FeedBackAttachItem.Url.LastIndexOf("/") + 1) : "";
                    if (String.IsNullOrEmpty(filename)) return;
                    if (CrossFilePicker.Current.IsExistFile(filename, "RMMTIMAGEVIEW"))
                    {
                        //已从服务器上缓存的图片。
                        CrossFilePicker.Current.OpenFile(filename, "RMMTIMAGEVIEW");
                    }
                    else
                    {
                        _commonFun.DownLoadFileFromOss(FeedBackAttachItem.Url, filename, "RMMTIMAGEVIEW");
                    }
                }
                else
                {
                    _commonFun.AlertLongText("请在电脑端阅览");
                    return;
                }

            }
            catch (Exception)
            {
                _commonFun.AlertLongText("加载异常，请重试");
            }
        }
        #endregion
        #region
        private async void ApproalSave(bool type)
        {
            try
            {

                if (ReplyContent == null || (ReplyContent != null && string.IsNullOrEmpty(ReplyContent.Trim())))
                {
                    _commonFun.AlertLongText("请填写审核意见");
                    return;
                }

                NeedApproalParams dto = new NeedApproalParams()
                {
                    NoticeReaderId = NoticeReaderId
                ,
                    PassYN = type
                ,
                    ReplyContent = ReplyContent
                ,
                    UserId = Convert.ToInt32(CommonContext.Account.UserId)
                };

                _commonFun.ShowLoading("保存中...");
                var result = await _notifiMngService.NoticeApprovalS(dto);
                if (result != null)
                {
                    if (result.ResultCode == ResultType.Success)
                    {
                        MessagingCenter.Send<string>("", MessageConst.NOTICE_APPROALDATA_GET);
                        MessagingCenter.Send<string>("", MessageConst.NOTICE_READERLIST_REFRESH);
                        await Navigation.PopAsync(true);
                        _commonFun.AlertLongText("保存成功");
                    }
                    else
                    {
                        _commonFun.AlertLongText(result.Msg);
                    }
                }
                else
                {
                    _commonFun.AlertLongText("保存时服务出现错误,,请重试");
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
                _commonFun.AlertLongText("保存异常,请重试");
            }
            finally
            {
                _commonFun.HideLoading();
            }
        }

        private async void GetNoticeApproalDetail(int NoticeReaderId)
        {
            try
            {

                _commonFun.ShowLoading("查询中...");
                var result = await _notifiMngService.GetNoticeApprovalDetail(NoticeReaderId);
                if (result != null)
                {
                    if (result.ResultCode == ResultType.Success)
                    {

                        var dto = JsonConvert.DeserializeObject<NoticeApprovalDetailDto>(result.Body);

                        if (dto != null)
                        {
                            NoticeId = dto.NoticeId;
                            Status = dto.Status;
                            Title = dto.Title;
                            Content = dto.Content;
                            ReplyContent = dto.ReplyContent;
                            FeedbackContent = dto.FeedbackContent;
                            AttachList = dto.AttachList;
                            LstHeight = AttachList.Count * _lstRowHeight;
                        }
                        else
                        {
                            _commonFun.AlertLongText(result.Msg);
                        }
                    }
                    else
                    {
                        _commonFun.AlertLongText(result.Msg);
                    }
                }
                else
                {
                    _commonFun.AlertLongText("查询时服务出现错误,,请重试");
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
        public void Init(int noticeReaderId, string status)
        {
            NoticeReaderId = noticeReaderId;
            GetNoticeApproalDetail(noticeReaderId);
            //"W"待审核，"N" 待重新提交
            if (status == "W")
            {
                IsEditable = true;
            }
            else
            {
                IsEditable = false;
            }
        }

        private async void GotoApproalLogPage()
        {
            try
            {
                _commonFun.ShowLoading("查询中...");
                var result = await _notifiMngService.GetNoticeApprovalLog(NoticeReaderId);
                if (result != null)
                {
                    if (result.ResultCode == ResultType.Success)
                    {

                        var List = JsonConvert.DeserializeObject<List<ApproalLogDto>>(result.Body);
                        List<ApproalLogDto> list = null;
                        if (List != null && List.Count > 0)
                        {
                            list = (from n in List where n.ReplyContent != null select n).ToList();
                            if (list != null && list.Count > 0)
                            {
                                await Navigation.PushAsync<NoticeApproalLogViewModel>((vm, v) => vm.Init(list, "审核记录"), true);
                            }
                            else
                            {
                                _commonFun.AlertLongText("没有审核记录");
                            }
                        }
                        else
                        {
                            _commonFun.AlertLongText(result.Msg);
                        }
                    }
                    else
                    {
                        _commonFun.AlertLongText(result.Msg);
                    }
                }
                else
                {
                    _commonFun.AlertLongText("查询时服务出现错误,,请重试");
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

        private async void GotoNoticeDetailPage()
        {
            await Navigation.PushAsync<NotifiMngViewModel>((vm, v) => vm.Init(NoticeId.ToString(), 0, 0, Status), true);


        }

        #endregion
    }
}
