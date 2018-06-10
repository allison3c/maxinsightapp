using XLabs.Forms.Mvvm;
using MaxInsight.Mobile.Module.Dto;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using XLabs.Ioc;
using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module.Dto.Notifi;
using MaxInsight.Mobile.Services.NotifiService;
using MaxInsight.Mobile.Services.Tour;
using System.Linq;
using System.IO;
using Plugin.FilePicker.Abstractions;
using Plugin.FilePicker;
using Aliyun.OSS;
using Newtonsoft.Json;

namespace MaxInsight.Mobile.ViewModels.Notifi
{
    public class NotifiMngViewModel : ViewModel
    {
        private readonly ITourService _tourService;
        private MediaFile _mediaFile;
        INotifiMngService _notifiMngService;
        CommonHelper _commonHelper;
        ICommonFun _commonFun;
        int _noticeId = 0;//本通知Id

        #region constructor
        public NotifiMngViewModel()
        {
            try
            {
                _tourService = Resolver.Resolve<ITourService>();
                _notifiMngService = Resolver.Resolve<INotifiMngService>();
                _commonHelper = Resolver.Resolve<CommonHelper>();
                _commonFun = Resolver.Resolve<ICommonFun>();

                #region MultiSelectDistributor
                MessagingCenter.Unsubscribe<List<MultiSelectDto>>(this, MessageConst.NOTICE_DISTRIBUTOR_SET);
                MessagingCenter.Subscribe<List<MultiSelectDto>>(
                    this,
                    MessageConst.NOTICE_DISTRIBUTOR_SET,
                    (paramList) =>
                    {
                        SetNoticeDistributor(paramList);
                    });
                #endregion

                #region MultiSelectDepartment
                MessagingCenter.Unsubscribe<List<MultiSelectDto>>(this, MessageConst.NOTICE_DEPARTMENT_SET);
                MessagingCenter.Subscribe<List<MultiSelectDto>>(
                this,
                MessageConst.NOTICE_DEPARTMENT_SET,
                (paramList) =>
                {
                    SetNoticeDepartment(paramList);
                });
                #endregion

                #region Attachment
                MessagingCenter.Subscribe<string>(
                this,
                MessageConst.NOTICE_ATTECHMENT_DELETE,
                (seqNo) =>
                {
                    DeleteAttechmentNotice(seqNo);
                });
                #endregion
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->NotifiMngViewModel");
                return;
            }
        }
        #endregion

        #region Property(s)
        #region 通知标题
        private string _noticeTitle;
        public string NoticeTitle
        {
            get { return _noticeTitle; }
            set { SetProperty(ref _noticeTitle, value); }
        }
        #endregion
        #region 通知编号
        private string _noticeNo;
        public string NoticeNo
        {
            get { return _noticeNo; }
            set { SetProperty(ref _noticeNo, value); }
        }
        #endregion
        #region 通知有效期
        private DateTime _startNoticeDate = DateTime.Now;
        public DateTime StartNoticeDate
        {
            get { return _startNoticeDate; }
            set { SetProperty(ref _startNoticeDate, value); }
        }
        private string _startDateStr;
        public string StartDateStr
        {
            get { return _startDateStr; }
            set { SetProperty(ref _startDateStr, value); }
        }
        private DateTime _endNoticeDate = DateTime.Now;
        public DateTime EndNoticeDate
        {
            get { return _endNoticeDate; }
            set { SetProperty(ref _endNoticeDate, value); }
        }
        private string _endDateStr;
        public string EndDateStr
        {
            get { return _endDateStr; }
            set { SetProperty(ref _endDateStr, value); }
        }
        #endregion        
        #region 通知对象

        private string _noticeReadersSelect = "选择经销商";
        public string NoticeReadersSelect
        {
            get { return _noticeReadersSelect; }
            set { SetProperty(ref _noticeReadersSelect, value, "NoticeReadersSelect"); }
        }
        private Color _noticeReadersColor = StaticColor.ContentFontColor;
        public Color NoticeReadersColor
        {
            get { return _noticeReadersColor; }
            set { SetProperty(ref _noticeReadersColor, value, "NoticeReadersColor"); }
        }


        private string _noticeSelectDep = "选择角色";
        public string NoticeSelectDep
        {
            get { return _noticeSelectDep; }
            set { SetProperty(ref _noticeSelectDep, value, "NoticeSelectDep"); }
        }

        private Color _noticeColorDep = StaticColor.ContentFontColor;
        public Color NoticeColorDep
        {
            get { return _noticeColorDep; }
            set { SetProperty(ref _noticeColorDep, value, "NoticeColorDep"); }
        }
        #endregion
        #region MultiSelect
        public bool DisHasSelect { get; set; } = false;
        private List<MultiSelectDto> _allDisItems = new List<MultiSelectDto>();
        public List<MultiSelectDto> AllDisItems
        {
            get { return _allDisItems; }
            set { SetProperty(ref _allDisItems, value, "AllDisItems"); }

        }
        public bool DepHasSelect { get; set; } = false;
        private List<MultiSelectDto> _allDepItems = new List<MultiSelectDto>();
        public List<MultiSelectDto> AllDepItems
        {
            get { return _allDepItems; }
            set { SetProperty(ref _allDepItems, value, "AllDepItems"); }

        }

        #endregion
        #region 通知内容
        private string _noticeContent;
        public string NoticeContent
        {
            get { return _noticeContent; }
            set { SetProperty(ref _noticeContent, value); }
        }
        #endregion
        #region RadioSelect
        private int _replySelected = 0;
        public int ReplySelected
        {
            get { return _replySelected; }
            set { SetProperty(ref _replySelected, value); }
        }
        private string _replySelectedName;
        public string ReplySelectedName
        {
            get { return _replySelectedName; }
            set { SetProperty(ref _replySelectedName, value); }
        }
        #endregion
        #region AttachmentList

        List<AttachDto> oldAttachmentList = new List<AttachDto>();
        List<AttachDto> _notifiAttachmentList;
        public List<AttachDto> NotifiAttachmentList
        {
            get { return _notifiAttachmentList; }
            set { SetProperty(ref _notifiAttachmentList, value); }

        }
        public AttachDto NotifiAttachItem { get; set; }
        double _lstRowHeight = 32;
        double _lstHeight;
        public double LstHeight
        {
            get { return _lstHeight; }
            set { SetProperty(ref _lstHeight, value); }
        }
        #endregion
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }
        public NoticeDetailDto ResultDto { get; set; }

        #region Page Title
        private string _noticeMngPageTitle;
        public string NoticeMngPageTitle
        {
            get { return _noticeMngPageTitle; }
            set { SetProperty(ref _noticeMngPageTitle, value); }
        }
        #endregion
        #endregion Property(s)

        #region Command
        public Command SaveNoticeCommand
        {
            get
            {
                return new Command((saveType) =>
                {
                    SaveNoticeInfo(saveType.ToString());
                });
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

        #endregion

        #region GetData

        #region MultiSelectDistributor
        private void SetNoticeDistributor(List<MultiSelectDto> paramList)
        {
            try
            {
                AllDisItems = paramList;
                List<MultiSelectDto> selectItems = paramList.FindAll(item => item.IsChecked);
                if (selectItems != null && selectItems.Count > 0)
                {
                    NoticeReadersSelect = string.Format("共选择{0}项", selectItems.Count);
                    NoticeReadersColor = Color.FromHex("3998C0");//"#3998c0"
                    DisHasSelect = true;
                }
                else
                {
                    NoticeReadersSelect = "选择经销商";
                    NoticeReadersColor = StaticColor.ContentFontColor;
                    DisHasSelect = false;
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->NotifiMngViewModel");
                return;
            }
        }
        #endregion

        #region MultiSelectDepartment

        private void SetNoticeDepartment(List<MultiSelectDto> paramList)
        {
            try
            {
                AllDepItems = paramList;
                List<MultiSelectDto> selectItems = paramList.FindAll(item => item.IsChecked);
                if (selectItems != null && selectItems.Count > 0)
                {
                    NoticeSelectDep = string.Format("共选择{0}项", selectItems.Count);
                    NoticeColorDep = Color.FromHex("3998C0");
                    DepHasSelect = true;
                }
                else
                {
                    NoticeSelectDep = "选择角色";
                    NoticeColorDep = StaticColor.ContentFontColor;
                    DepHasSelect = false;
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->NotifiMngViewModel");
                return;
            }
        }
        #endregion

        #region Attachment
        private void DeleteAttechmentNotice(string SeqNo)
        {
            try
            {
                oldAttachmentList.Remove(oldAttachmentList.Find(item => item.SeqNo == int.Parse(SeqNo)));
                List<AttachDto> newList = new List<AttachDto>();
                newList.AddRange(oldAttachmentList);
                int i = 1;
                newList.Select(c => { c.SeqNo = i++; return c; }).ToList();
                NotifiAttachmentList = newList;
                LstHeight = NotifiAttachmentList.Count * _lstRowHeight;
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->NotifiMngViewModel");
                return;
            }
        }
        #endregion

        #region NoticeDetail
        private async void SetNoticeDetail(string noticeId)
        {
            try
            {
                _commonFun.ShowLoading("查询中...");
                //TO-DO
                var result = await _notifiMngService.SearchMadeNoticeDetailInfo(noticeId);
                if (result.ResultCode == Module.ResultType.Success)
                {

                    NoticeDetailDto noticeList = CommonHelper.DecodeString<NoticeDetailDto>(result.Body);
                    if (noticeList != null)
                    {
                        _commonFun.HideLoading();
                        ResultDto = noticeList;
                        NoticeTitle = ResultDto.Title;
                        NoticeNo = ResultDto.NoticeNo;
                        StartNoticeDate = ResultDto.SDate;
                        StartDateStr = ResultDto.SDate.ToString("yyyy-MM-dd");
                        EndNoticeDate = ResultDto.EDate;
                        EndDateStr = ResultDto.EDate.ToString("yyyy-MM-dd");
                        ReplySelected = ResultDto.NeedReply;
                        ReplySelectedName = ResultDto.NeedReplyName;
                        AllDisItems = ResultDto.NoticeDisList;
                        AllDepItems = ResultDto.NoticeDepList;
                        NoticeContent = ResultDto.Content;
                        oldAttachmentList = new List<AttachDto>();
                        oldAttachmentList.AddRange(ResultDto.AttachList);
                        NotifiAttachmentList = oldAttachmentList;
                        LstHeight = oldAttachmentList.Count * _lstRowHeight;
                        List<MultiSelectDto> disList = new List<MultiSelectDto>();
                        List<MultiSelectDto> depList = new List<MultiSelectDto>();
                        if ((CommonContext.Account.UserType == "A" || CommonContext.Account.UserType == "R" || CommonContext.Account.UserType == "Z"))
                        {
                            if (ResultDto.Status == "T")
                            {
                                List<ServerDto> serverList = new List<ServerDto>();
                                foreach (var item in CommonContext.Account.ZionList[0].AreaList)
                                {
                                    serverList.AddRange(item.ServerList);
                                }
                                foreach (ServerDto item in serverList)
                                    if (AllDisItems.Exists(a => a.DisCode == item.SId))
                                        disList.Add(new MultiSelectDto { DisCode = item.SId, DisName = item.SName, IsChecked = true });
                                    else
                                        disList.Add(new MultiSelectDto { DisCode = item.SId, DisName = item.SName, IsChecked = false });

                                List<DepartmentDto> departmentList = new List<DepartmentDto>();
                                departmentList.AddRange(CommonContext.Account.DepartmentList);
                                foreach (DepartmentDto item in departmentList)
                                    if (AllDepItems.Exists(a => a.DisCode == item.DId))
                                        depList.Add(new MultiSelectDto { DisCode = item.DId, DisName = item.DName, IsChecked = true });
                                    else
                                        depList.Add(new MultiSelectDto { DisCode = item.DId, DisName = item.DName, IsChecked = false });
                            }
                            else
                            {
                                disList = AllDisItems;
                                depList = AllDepItems;
                            }
                        }
                        else if (CommonContext.Account.UserType == "S")
                        {
                            disList.Add(new MultiSelectDto { DisCode = CommonContext.Account.OrgServerId, DisName = CommonContext.Account.OrgServerName, IsChecked = true });
                            List<DepartmentDto> departmentList = new List<DepartmentDto>();
                            departmentList.AddRange(CommonContext.Account.DepartmentList);
                            foreach (DepartmentDto item in departmentList)
                                if (AllDepItems.Exists(a => a.DisCode == item.DId))
                                    depList.Add(new MultiSelectDto { DisCode = item.DId, DisName = item.DName, IsChecked = true });
                                else
                                    depList.Add(new MultiSelectDto { DisCode = item.DId, DisName = item.DName, IsChecked = false });
                        }
                        else
                        {
                            disList.Add(new MultiSelectDto { DisCode = CommonContext.Account.OrgServerId, DisName = CommonContext.Account.OrgServerName, IsChecked = true });
                            depList.Add(new MultiSelectDto { DisCode = CommonContext.Account.OrgDepartmentId, DisName = CommonContext.Account.OrgDepartmentName, IsChecked = true });
                        }
                        MessagingCenter.Send<List<MultiSelectDto>>(disList, MessageConst.NOTICE_DISTRIBUTOR_SHOW);
                        MessagingCenter.Send<List<MultiSelectDto>>(depList, MessageConst.NOTICE_DEPARTMENT_SHOW);
                        if (AllDisItems != null && AllDisItems.Count > 0)
                        {
                            NoticeReadersSelect = string.Format("共选择{0}项", AllDisItems.Count);
                            NoticeReadersColor = Color.FromHex("3998C0");
                            DisHasSelect = true;
                        }
                        else
                        {
                            NoticeReadersSelect = "选择经销商";
                            NoticeReadersColor = StaticColor.ContentFontColor;
                            DisHasSelect = false;
                        }
                        if (AllDepItems != null && AllDepItems.Count > 0)
                        {
                            NoticeSelectDep = string.Format("共选择{0}项", AllDepItems.Count);
                            NoticeColorDep = Color.FromHex("3998C0");//
                            DepHasSelect = true;
                        }
                        else
                        {
                            NoticeSelectDep = "选择角色";
                            NoticeColorDep = StaticColor.ContentFontColor;
                            DepHasSelect = false;
                        }
                        _noticeId = ResultDto.NoticeId;
                    }
                    else
                    {
                        _commonFun.HideLoading();
                        ResultDto = new NoticeDetailDto();
                        _commonFun.ShowToast("没有数据");
                    }
                }
                else
                {
                    _commonFun.HideLoading();
                    ResultDto = new NoticeDetailDto();
                    _commonFun.AlertLongText("查询失败，请重试。 " + result.Msg);
                }
            }
            catch (OperationCanceledException)
            {
                _commonFun.HideLoading();
                ResultDto = new NoticeDetailDto();
                _commonFun.AlertLongText("请求超时。");
            }
            catch (Exception)
            {
                _commonFun.HideLoading();
                ResultDto = new NoticeDetailDto();
                _commonFun.AlertLongText("查询异常，请重试。");
            }
            finally
            {
                _commonFun.HideLoading();
            }
        }
        #endregion

        #region SaveNoticeInfo
        private async void SaveNoticeInfo(string saveType)
        {
            try
            {
                if (_noticeId == 0 && saveType.Equals("C"))
                {
                    await Navigation.PopAsync();
                    return;
                }
                //通知标题
                if (string.IsNullOrEmpty(NoticeTitle) || NoticeTitle.Trim() == "")
                {
                    _commonFun.AlertLongText("请输入通知标题");
                    return;
                }
                //通知有效期
                if (Convert.ToDateTime(StartNoticeDate.ToString("yyyy-MM-dd")) > Convert.ToDateTime(EndNoticeDate.ToString("yyyy-MM-dd")))
                {
                    _commonFun.AlertLongText("开始日期不能大于结束日期");
                    return;
                }
                //通知对象
                if (!DisHasSelect)
                {
                    _commonFun.AlertLongText("请选择经销商");
                    return;
                }
                //if (!DepHasSelect)
                //{
                //    _commonFun.AlertLongText("请选择部门");
                //    return;
                //}
                //结果反馈
                //通知内容
                if (string.IsNullOrEmpty(NoticeContent) || NoticeContent.Trim() == "")
                {
                    _commonFun.AlertLongText("请输入通知内容");
                    return;
                }
                _commonFun.ShowLoading("保存中...");
                if (_commonHelper.IsNetWorkConnected() == true)
                {
                    try
                    {
                        List<MultiSelectDto> saveDepList = new List<MultiSelectDto>();
                        if (AllDepItems == null || AllDepItems.Count == 0)
                            saveDepList.Add(new MultiSelectDto() { DisCode = "0", DisName = "" });
                        else
                            saveDepList = AllDepItems;
                        string disdepList = CommonUtil.DisAndDepToString(AllDisItems, saveDepList);
                        var result = await _notifiMngService.SaveMadeNotifi(NoticeTitle,
                                                StartNoticeDate.ToString("yyyyMMdd"),
                                                EndNoticeDate.ToString("yyyyMMdd"),
                                                disdepList,
                                                ReplySelected == 0 ? "0" : "1",
                                                NoticeContent,
                                                NotifiAttachmentList,
                                                saveType.ToString(),
                                                _noticeId,
                                                Convert.ToInt32(CommonContext.Account.UserId));
                        if (result.ResultCode == Module.ResultType.Success)
                        {
                            _noticeId = 0;
                            //发条消息让 审批查询，通知结果查询查询
                            MessagingCenter.Send<string>("", MessageConst.NOTIFI_SAVEREFRESH_GO);
                            await Navigation.PopAsync(true);
                            _commonFun.AlertLongText("保存成功");
                        }
                        else
                        {
                            _commonFun.HideLoading();
                            _commonFun.AlertLongText("保存失败，请重试。 " + result.Msg);
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
                        _commonFun.AlertLongText("保存失败，请重试。");
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
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->NotifiMngViewModel");
                return;
            }
        }
        #endregion

        #region SetReadStatus
        private async void SetReadStatus(NoticeReadStatusDto readStatusDto)
        {
            if (_commonHelper.IsNetWorkConnected() == true)
            {
                try
                {
                    var result = await _notifiMngService.UpdateReaderReadStatus(readStatusDto);
                    if (result.ResultCode == Module.ResultType.Success)
                    {
                        _commonFun.HideLoading();
                        MessagingCenter.Send<string>("", MessageConst.NOTIFI_SAVEREFRESH_GO);
                    }
                    else
                    {
                        _commonFun.HideLoading();
                        _commonFun.AlertLongText("保存失败，请重试。 " + result.Msg);
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
                    _commonFun.AlertLongText("保存异常，请重试。");
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
        #endregion
        #endregion

        #region Method
        private async void UploadFile(string fileType)
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
                ////_mediaFile = await CrossMedia.Current.PickVideoAsync();
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
                ////_mediaFile = await CrossMedia.Current.PickPhotoAsync();
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
                    string filenameselected = _path.Substring(_path.LastIndexOf("/") + 1);
                    var result = await _tourService.UploadFiletoOss(_stream, filenameselected, _path);

                    if (result != null && result.ResultCode == 0)
                    {
                        oldAttachmentList.Add((JsonConvert.DeserializeObject<AttachDto>(result.Body)));
                        List<AttachDto> resultList = new List<AttachDto>();
                        resultList.AddRange(oldAttachmentList);
                        int i = 1;
                        resultList.Select(c => { c.SeqNo = i++; return c; }).ToList();
                        NotifiAttachmentList = resultList;
                        LstHeight = NotifiAttachmentList.Count * _lstRowHeight;
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
                    //_commonFun.HideLoading();
                    _commonFun.AlertLongText("上传异常，请重试。");
                }
                finally
                {
                    //_mediaFile.Dispose();
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
        private void PreviewImage()
        {
            try
            {
                //List<ImagePreviewDto> list = new List<ImagePreviewDto>();
                //string url = NotifiAttachItem.Url.ToUpper();
                //if (url.EndsWith(".JPG") || url.EndsWith(".BMP") || url.EndsWith(".GIF") ||
                //    url.EndsWith(".JPEG") || url.EndsWith(".PNG") || url.EndsWith(".SVG"))
                //{
                //    list.Add(new ImagePreviewDto { Url = NotifiAttachItem.Url });
                //    Device.BeginInvokeOnMainThread(async () =>
                //    {
                //        await PopupNavigation.PushAsync(new PreviewImagePage(list, 0), true);
                //    });
                //}
                //else
                //{
                //_commonFun.AlertLongText("请在电脑端阅览");
                //return;
                //}
                string url = NotifiAttachItem.Url.ToUpper();
                if (url.EndsWith(".JPG") || url.EndsWith(".BMP") || url.EndsWith(".GIF") ||
                    url.EndsWith(".JPEG") || url.EndsWith(".PNG") || url.EndsWith(".SVG"))
                {
                    string filename = NotifiAttachItem.Url.LastIndexOf("/") > 0 ? NotifiAttachItem.Url.Substring(NotifiAttachItem.Url.LastIndexOf("/") + 1).ToLower() : "";
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
                        _commonFun.DownLoadFileFromOss(NotifiAttachItem.Url, filename, "RMMTIMAGEVIEW");
                        //已从服务器上缓存的图片。
                        //CrossFilePicker.Current.OpenFile(filename, "RMMTIMAGEVIEW");
                        //OssClient client = CommonHelper.CreateOssClient();
                        //var bucketName = _commonHelper._bucket;
                        //var key = "AreaTool/" + filename;

                        //var result = client.GetObject(bucketName, key);
                        //FileData fileToSave = new FileData();
                        //using (Stream stream = result.Content)
                        //{
                        //    int length = Convert.ToInt32(stream.Length);
                        //    byte[] bytes = new byte[length];
                        //    //stream.Position = 0;
                        //    //stream.Read(bytes, 0, bytes.Length);
                        //    stream.Read(bytes, 0, length);
                        //    // 设置当前流的位置为流的开始
                        //    //stream.Seek(0, SeekOrigin.Begin);
                        //    fileToSave.DataArray = bytes;
                        //    fileToSave.FileName = filename;

                        //}
                        ////MemoryStream resultstream = GetObjectPartly(bucketName, key);
                        ////FileData fileToSave = new FileData();
                        ////using (Stream stream = resultstream)
                        ////{
                        ////    int length = Convert.ToInt32(stream.Length);
                        ////    byte[] bytes = new byte[length];
                        ////    stream.Read(bytes, 0, length);
                        ////    // 设置当前流的位置为流的开始
                        ////    //stream.Seek(0, SeekOrigin.Begin);
                        ////    fileToSave.DataArray = bytes;
                        ////    fileToSave.FileName = filename;
                        ////}
                        //CrossFilePicker.Current.OpenFile(fileToSave);
                        ////resultstream.Flush();
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
        #region multiple download from oss
        public MemoryStream GetObjectPartly(string bucketName, string key)
        {
            try
            {
                OssClient client = CommonHelper.CreateOssClient();
                var memoryStream = new MemoryStream();
                var objectMetadata = client.GetObjectMetadata(bucketName, key);
                var fileLength = objectMetadata.ContentLength;
                const int partSize = 1024 * 1024 * 10;

                var partCount = CalPartCount(fileLength, partSize);

                for (var i = 0; i < partCount; i++)
                {
                    var startPos = partSize * i;
                    var endPos = partSize * i + (partSize < (fileLength - startPos) ? partSize : (fileLength - startPos)) - 1;
                    Download(memoryStream, startPos, endPos, bucketName, key, client);
                }
                return memoryStream;
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->NotifiMngViewModel");
                return null;
            }
        }
        private int CalPartCount(long fileLength, long partSize)
        {
            try
            {
                var partCount = (int)(fileLength / partSize);
                if (fileLength % partSize != 0)
                {
                    partCount++;
                }
                return partCount;
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->NotifiMngViewModel");
                return 0;
            }
        }
        private void Download(MemoryStream memoryStream, long startPos, long endPos, String bucketName, String fileKey, OssClient client)
        {
            System.IO.Stream contentStream = null;
            try
            {
                var getObjectRequest = new GetObjectRequest(bucketName, fileKey);
                getObjectRequest.SetRange(startPos, endPos);
                var ossObject = client.GetObject(getObjectRequest);
                byte[] buffer = new byte[1024 * 1024];
                var bytesRead = 0;
                memoryStream.Seek(startPos, SeekOrigin.Begin);
                contentStream = ossObject.Content;
                while ((bytesRead = contentStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    memoryStream.Write(buffer, 0, bytesRead);
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->NotifiMngViewModel");
                return;
            }
            finally
            {
                if (contentStream != null)
                {
                    contentStream.Dispose();
                }
            }
        }
        #endregion
        #region page init
        public void Init(string noticeId, int disId = 0, int departId = 0, string noticeStatus = "")
        {
            try
            {
                MessagingCenter.Send<string>(noticeStatus, MessageConst.NOTICE_MADE_SETCONTROLROLE);
                IsLoading = false;
                if (noticeId == "0")
                {
                    NoticeTitle = string.Empty;
                    NoticeNo = string.Empty;
                    StartNoticeDate = DateTime.Now;
                    EndNoticeDate = DateTime.Now;
                    NoticeReadersSelect = "选择经销商";
                    NoticeReadersColor = StaticColor.ContentFontColor;
                    DisHasSelect = false;
                    NoticeSelectDep = "选择角色";
                    NoticeColorDep = StaticColor.ContentFontColor;
                    DepHasSelect = false;
                    MessagingCenter.Send<List<MultiSelectDto>>(new List<MultiSelectDto>(), MessageConst.NOTICE_DISTRIBUTOR_SHOW);
                    MessagingCenter.Send<List<MultiSelectDto>>(new List<MultiSelectDto>(), MessageConst.NOTICE_DEPARTMENT_SHOW);
                    NoticeContent = string.Empty;
                    ReplySelected = 0;
                    oldAttachmentList = new List<AttachDto>();
                    NotifiAttachmentList = new List<AttachDto>();
                    LstHeight = 0;
                    NoticeMngPageTitle = "通知拟定";
                    _noticeId = 0;
                    AllDepItems = new List<MultiSelectDto>();
                    AllDisItems = new List<MultiSelectDto>();
                }
                else
                {
                    AllDisItems = new List<MultiSelectDto>();
                    AllDepItems = new List<MultiSelectDto>();
                    SetNoticeDetail(noticeId);
                    if ((CommonContext.Account.UserType == "S" || CommonContext.Account.UserType == "D") && noticeStatus == "U")

                    {
                        NoticeReadStatusDto readStatusDto = new NoticeReadStatusDto();
                        readStatusDto.NoticeId = Convert.ToInt32(noticeId);
                        readStatusDto.DisId = disId;
                        readStatusDto.DepartId = departId;
                        readStatusDto.InUserId = Convert.ToInt32(CommonContext.Account.UserId);
                        SetReadStatus(readStatusDto);
                    }
                    if (noticeStatus == "" || noticeStatus == "T")
                    {
                        NoticeMngPageTitle = "通知拟定";
                    }
                    else
                    {
                        NoticeMngPageTitle = "通知详细";
                    }
                    _noticeId = Convert.ToInt32(noticeId);
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->NotifiMngViewModel");
                return;
            }
        }

        #endregion
    }
}
