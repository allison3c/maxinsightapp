using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto.Notifi;
using MaxInsight.Mobile.Services.NotifiService;
using MaxInsight.Mobile.Services.Tour;
using Newtonsoft.Json;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.Notifi
{
    public class NotifiFeedbackViewModel : ViewModel
    {
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        INotifiMngService _notifiMngService;
        private MediaFile _mediaFile;
        private readonly ITourService _tourService;
        #region Constructor
        public NotifiFeedbackViewModel()
        {
            try
            {
                _commonFun = Resolver.Resolve<ICommonFun>();
                _commonHelper = Resolver.Resolve<CommonHelper>();
                _notifiMngService = Resolver.Resolve<INotifiMngService>();
                _tourService = Resolver.Resolve<ITourService>();

                //Device.BeginInvokeOnMainThread(() =>
                //{
                //    GetNoticeApproalDetail(NotifiContentItem.NoticeReaderId);

                //});


                MessagingCenter.Unsubscribe<List<RequestParameter>>(this, MessageConst.CASEATTACH_DELETE);

                MessagingCenter.Subscribe<string>(
                this,
                MessageConst.CASEATTACH_DELETE,
                (SeqNo) =>
                {
                    DeleteCaseAttach(SeqNo);
                });
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->NotifiFeedbackViewModel");
                return;
            }
        }
        #endregion
        public List<AttachDto> AttachList = new List<AttachDto>();
        public List<AttachDto> _caseAttachList;
        public List<AttachDto> CaseAttachList
        {
            get { return _caseAttachList; }
            set { SetProperty(ref _caseAttachList, value); }
        }
        private FeedBackListDto _noticeListInfoDto;
        public FeedBackListDto NoticeListInfoDto
        {
            get
            {
                return _noticeListInfoDto;
            }
            set
            {
                SetProperty(ref _noticeListInfoDto, value);
            }
        }
        double _lstHeight;
        double _lstRowHeight = 32;
        public double LstHeight
        {
            get { return _lstHeight; }
            set { SetProperty(ref _lstHeight, value); }
        }
        public AttachDto _caseAttachItem;
        public AttachDto CaseAttachItem
        {
            get { return _caseAttachItem; }
            set { SetProperty(ref _caseAttachItem, value); }
        }
        public string feedbackContent;
        public string FeedbackContent
        {
            get
            {
                return feedbackContent;
            }
            set
            {
                SetProperty(ref feedbackContent, value);
            }
        }

        public string replyContent;
        public string ReplyContent
        {
            get
            {
                return replyContent;
            }
            set
            {
                SetProperty(ref replyContent, value);
            }
        }
        private bool _visibleYN;
        public bool VisibleYN
        {
            get
            {
                return _visibleYN;
            }
            set { SetProperty(ref _visibleYN, value); }
        }
        private bool _visibleNY;
        public bool VisibleNY
        {
            get
            {
                return _visibleNY;
            }
            set { SetProperty(ref _visibleNY, value); }
        }

        private bool _iSshowYN;
        public bool ISshowYN
        {
            get
            {
                return _iSshowYN;
            }
            set { SetProperty(ref _iSshowYN, value); }
        }
        public int _fbId;
        public int FBkId
        {
            get
            {
                return _fbId;
            }
            set { SetProperty(ref _fbId, value); }
        }
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        public void Init(int NoticeId, int disId, int departId, string Status, string FeedBackId = "")
        {
            IsLoading = false;
            NoticeListInfoDto = new FeedBackListDto { NoticeId = NoticeId, DisId = disId.ToString(), DepartId = departId.ToString(), Status = Status };
            AttachList = new List<AttachDto>();
            ISshowYN = false;
            FeedbackContent = "";
            CaseAttachList = new List<AttachDto>();
            ReplyContent = "";
            LstHeight = 0;
            FBkId = int.Parse(CommonContext.Account.UserId);
            searchFeedBackDtl();

        }
        private RelayCommand _noticeDetailCommand;
        private RelayCommand _replyLogCommand;
        public RelayCommand NoticeDetailCommand
        {

            get
            {
                return _noticeDetailCommand ??

                (_noticeDetailCommand = new RelayCommand(GotoNoticeDetailPage));
            }
        }
        public RelayCommand ReplyLogCommand
        {

            get
            {
                return _replyLogCommand ??

                (_replyLogCommand = new RelayCommand(GotoReplyLogPage));
            }
        }
        public Command PickPhotoCommand
        {
            get
            {
                return new Command((arg) =>
                {
                    UploadFile(arg.ToString());
                });
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
        private RelayCommand _saveFeedBackNoticeCommand;
        private RelayCommand _cancelFeedBackNoticeCommand;
        public RelayCommand SaveFeedBackNoticeCommand
        {
            get
            {
                return _saveFeedBackNoticeCommand ??

                (_saveFeedBackNoticeCommand = new RelayCommand(SaveFeedBackNotice));
            }
        }
        public RelayCommand CancelFeedBackNoticeCommand
        {
            get
            {
                return _cancelFeedBackNoticeCommand ??

                (_cancelFeedBackNoticeCommand = new RelayCommand(CancelFeedBackNotice));
            }
        }
        public async void UploadFile(string fileType)
        {
            try
            {
                await CrossMedia.Current.Initialize();
                _mediaFile = null;
                string _path = "";
                Stream _stream = null;

                if (fileType == "V")
                {
                    if (!CrossMedia.Current.IsPickVideoSupported)
                    {
                        _commonFun.AlertLongText("此设备不支持视频上传");
                        return;
                    }
                    var action = await _commonFun.ShowActionSheet("从相册", "录制");

                    IsLoading = true;
                    if (action == "从相册")
                    {
                        _mediaFile = await CrossMedia.Current.PickVideoAsync();
                    }
                    else if (action == "录制")
                    {
                        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
                        {
                            return;
                        }
                        _mediaFile = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
                        {
                            Directory = "RMMT",
                            Name = DateTime.Now.ToString("yy-MM-dd").Replace("-", "")
                                           + DateTime.Now.ToString("HH:mm:ss").Replace(":", "")
                        });
                    }
                }
                else if (fileType == "P")
                {
                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        _commonFun.AlertLongText("此设备不支持图片上传");
                        return;
                    }
                    var action = await _commonFun.ShowActionSheet("从相册", "拍照");
                    IsLoading = true;

                    if (action == "从相册")
                    {
                        _mediaFile = await CrossMedia.Current.PickPhotoAsync();
                    }
                    else if (action == "拍照")
                    {
                        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                        {
                            return;
                        }
                        _mediaFile = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                        {
                            Directory = "RMMT",
                            Name = DateTime.Now.ToString("yy-MM-dd").Replace("-", "")
                                           + DateTime.Now.ToString("HH:mm:ss").Replace(":", "")
                        });
                    }
                }
                else
                {
                    try
                    {
                        IsLoading = true;
                        FileData filedata = new FileData();
                        filedata = await CrossFilePicker.Current.PickFile();
                        if (filedata == null)
                        {
                            IsLoading = false;
                            return;
                        }
                        string filename = filedata.FileName;
                        byte[] file = filedata.DataArray;
                        _stream = new MemoryStream(file);
                        _path = filename;
                    }
                    catch (Exception)
                    {
                        IsLoading = false;
                    }
                }

                if (_mediaFile != null)
                {
                    _stream = _mediaFile.GetStream();
                    _path = _mediaFile.Path;
                }
                if (_stream == null)
                {
                    IsLoading = false;
                    return;
                }
                if (_commonHelper.IsNetWorkConnected() == true)
                {
                    try
                    {
                        string filename = _path.Substring(_path.LastIndexOf("/") + 1);
                        var result = await _tourService.UploadFiletoOss(_stream, filename, _path);

                        if (result != null)
                        {
                            AttachList.Add((JsonConvert.DeserializeObject<AttachDto>(result.Body)));
                            List<AttachDto> resultList = new List<AttachDto>();
                            resultList.AddRange(AttachList);
                            int i = 1;
                            resultList.Select(c => { c.SeqNo = i++; return c; }).ToList();
                            CaseAttachList = resultList;
                            LstHeight = CaseAttachList.Count * _lstRowHeight;
                        }
                        else
                        {
                            _commonFun.AlertLongText("上传失败，请重试。 " + result.Msg);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        _commonFun.AlertLongText("请求超时。");
                    }
                    catch (Exception)
                    {
                        _commonFun.AlertLongText("上传异常，请重试。");
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                }
                else
                {
                    _mediaFile.Dispose();
                    _commonFun.AlertLongText("网络连接异常。");
                    IsLoading = false;
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->NotifiFeedbackViewModel");
                return;
            }
        }
        public void DeleteCaseAttach(string SeqNo)
        {
            try
            {
                AttachList.Remove(AttachList.Find(item => item.SeqNo == int.Parse(SeqNo)));
                List<AttachDto> newList = new List<AttachDto>();
                newList.AddRange(AttachList);
                int i = 1;
                newList.Select(c => { c.SeqNo = i++; return c; }).ToList();
                CaseAttachList = newList;
                LstHeight = CaseAttachList.Count * _lstRowHeight;
            }
            catch (Exception)
            {
            }
        }
        private void PreviewImage()
        {
            try
            {
                List<ImagePreviewDto> list = new List<ImagePreviewDto>();
                string url = CaseAttachItem.Url.ToUpper();
                if (url.EndsWith(".JPG") || url.EndsWith(".BMP") || url.EndsWith(".GIF") ||
                    url.EndsWith(".JPEG") || url.EndsWith(".PNG") || url.EndsWith(".SVG"))
                {
                    //list.Add(new ImagePreviewDto { Url = CaseAttachItem.Url });
                    //Device.BeginInvokeOnMainThread(async () =>
                    //{
                    //    await PopupNavigation.PushAsync(new PreviewImagePage(list, 0), true);
                    //});
                    string filename = CaseAttachItem.Url.LastIndexOf("/") > 0 ? CaseAttachItem.Url.Substring(CaseAttachItem.Url.LastIndexOf("/") + 1) : "";
                    if (String.IsNullOrEmpty(filename)) return;
                    if (CrossFilePicker.Current.IsExistFile(filename, "RMMT_IMAGE"))
                    {
                        //本地预览
                        CrossFilePicker.Current.OpenFile(filename, "RMMT_IMAGE");
                    }
                    else if (CrossFilePicker.Current.IsExistFile(filename, "RMMTIMAGEVIEW"))
                    {
                        //已从服务器上缓存的图片。
                        CrossFilePicker.Current.OpenFile(filename, "RMMTIMAGEVIEW");
                    }
                    else
                    {
                        _commonFun.DownLoadFileFromOss(CaseAttachItem.Url, filename, "RMMTIMAGEVIEW");
                        //已从服务器上缓存的图片。
                        //CrossFilePicker.Current.OpenFile(filename, "RMMTIMAGEVIEW");
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
        public async void SaveFeedBackNotice()
        {
            try
            {
                if (FeedbackContent == "")
                {
                    _commonFun.AlertLongText("请填写反馈内容");
                    return;
                }
                FeedBackInfoDto dto = new FeedBackInfoDto()
                {
                    NoticeId = NoticeListInfoDto.NoticeId,
                    ReplyContent = FeedbackContent,
                    Status = "S",
                    UserId = int.Parse(CommonContext.Account.UserId),
                    list = CaseAttachList
                };

                _commonFun.ShowLoading("保存中...");
                var result = await _notifiMngService.SaveFeedBackNotice(dto);
                if (result != null)
                {
                    if (result.ResultCode == ResultType.Success)
                    {
                        //跳转或刷新CaseIndexViewModel
                        MessagingCenter.Send<string>("", MessageConst.NOTICE_FEEDBACKDATA_GET);
                        MessagingCenter.Send<string>("", MessageConst.NOTIFI_SAVEREFRESH_GO);
                        MessagingCenter.Send<string>("", "MessagePageReSearch");// 给消息页发消息
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
            }
            finally
            {
                _commonFun.HideLoading();
            }
        }
        public async void CancelFeedBackNotice()
        {
            try
            {
                MessagingCenter.Send<string>("", MessageConst.NOTIFI_SAVEFEEDBACK_GO);
                await Navigation.PopAsync(true);
            }
            catch (Exception)
            {

            }
            finally
            {
                _commonFun.HideLoading();
            }
        }
        private async void GotoNoticeDetailPage()
        {
            try
            {
                await Navigation.PushAsync<NotifiMngViewModel>((vm, v) => vm.Init(NoticeListInfoDto.NoticeId.ToString(), 0, 0, NoticeListInfoDto.Status), true);
            }
            catch (Exception)
            {
            }
        }
        private async void GotoReplyLogPage()
        {
            try
            {
                _commonFun.ShowLoading("查询中...");
                if (NoticeListInfoDto.NoticeReaderId == null)
                {
                    _commonFun.HideLoading();
                    _commonFun.AlertLongText("没有反馈记录.");
                    return;
                }
                var result = await _notifiMngService.GetNoticeApprovalLog(int.Parse(NoticeListInfoDto.NoticeReaderId));
                if (result != null)
                {
                    if (result.ResultCode == ResultType.Success)
                    {

                        var List = JsonConvert.DeserializeObject<List<ApproalLogDto>>(result.Body);
                        if (List != null && List.Count > 0)
                        {
                            await Navigation.PushAsync<NoticeApproalLogViewModel>((vm, v) => vm.Init(List, "反馈记录"), true);
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
        private async void searchFeedBackDtl()
        {
            try
            {
                _commonFun.ShowLoading("查询中...");
                var result = await _notifiMngService.SearchNoticeFeedBackDtl(NoticeListInfoDto.NoticeId.ToString(), CommonContext.Account.UserId, NoticeListInfoDto.DisId.ToString(), NoticeListInfoDto.DepartId.ToString());
                if (result != null)
                {
                    if (result.ResultCode == ResultType.Success)
                    {

                        var List = JsonConvert.DeserializeObject<List<NoticeFeedBackDtlDto>>(result.Body);
                        if (List != null && List.Count > 0)
                        {
                            if ((NoticeListInfoDto.Status == "N" || NoticeListInfoDto.Status == "R" || NoticeListInfoDto.Status == "U") && List[0].EditYN == "Y")
                            {
                                VisibleYN = true;
                                VisibleNY = false;
                            }
                            else
                            {
                                if (NoticeListInfoDto.Status != "N" && NoticeListInfoDto.Status != "R" && NoticeListInfoDto.Status != "U")
                                {
                                    ISshowYN = true;
                                }
                                VisibleYN = false;
                                VisibleNY = true;
                            }

                            NoticeListInfoDto.NoticeReaderId = List[0].NoticeReaderId;
                            ReplyContent = List[0].ReplyContent;
                            if ((NoticeListInfoDto.Status == "N" || NoticeListInfoDto.Status == "R" || NoticeListInfoDto.Status == "U") && List[0].EditYN == "Y")
                            {
                                FeedbackContent = "";
                                CaseAttachList = null;
                                LstHeight = 0;
                                ISshowYN = false;
                            }
                            else
                            {
                                FeedbackContent = List[0].FeedbackContent;
                                CaseAttachList = List[0].AttachList;
                                LstHeight = List[0].AttachList.Count * _lstRowHeight;
                                ISshowYN = true;
                            }


                            //await Navigation.PushAsync<NoticeApproalLogViewModel>((vm, v) => vm.Init(List), true);
                        }
                        else
                        {
                            ISshowYN = true;
                            VisibleYN = false;
                            VisibleNY = true;
                            _commonFun.AlertLongText("没有查询到数据.");
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
    }
}
