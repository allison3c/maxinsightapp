using XLabs.Forms.Mvvm;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto;
using MaxInsight.Mobile.Services;
using ModernHttpClient;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using Xamarin.Forms;
using XLabs.Ioc;
using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module.Dto.Notifi;
using MaxInsight.Mobile.Pages.Notifi;
using MaxInsight.Mobile.Services.NotifiService;

namespace MaxInsight.Mobile.ViewModels.Notifi
{
    public class NotifiMngViewModel : ViewModel
    {
        private MediaFile _mediaFile;
        INotifiMngService _notifiMngService;
        CommonHelper _commonHelper;
        ICommonFun _commonFun;
        int _noticeId = 0;//本通知Id

        #region constructor
        public NotifiMngViewModel()
        {
            _notifiMngService = Resolver.Resolve<INotifiMngService>();
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _commonFun = Resolver.Resolve<ICommonFun>();

            #region MultiSelectDistributor
                MessagingCenter.Subscribe<List<DistributorDto>>(
                this,
                MessageConst.NOTICE_DISTRIBUTOR_SET,
                (paramList) =>
                {
                    SetNoticeDistributor(paramList);
                });
            #endregion

            #region MultiSelectDepartment
            MessagingCenter.Subscribe<List<DistributorDto>>(
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
            (pathToFile) =>
            {
                DeleteAttechmentNotice(pathToFile);
            });
            #endregion
        }
        #endregion

        #region Property(s)
        #region 通知标题
        public string NoticeTitleLabel { get; set; } = "通知标题";
        public string NoticeTitle { get; set; }
        #endregion
        #region 通知有效期
        public string NoticeAvailableDate { get; set; } = "通知有效期";
        public DateTime StartNoticeDate { get; set; } = DateTime.Now;
        public DateTime EndNoticeDate { get; set; } = DateTime.Now;
        #endregion        
        #region 通知对象
        public string NoticeReadersLabel { get; set; } = "通知对象";

        public string _noticeReadersSelect = "选择经销商";
        public string NoticeReadersSelect
        {
            get { return _noticeReadersSelect; }
            set { SetProperty(ref _noticeReadersSelect, value, "NoticeReadersSelect"); }
        }
        public string _noticeReadersColor = "#CFCFCF";
        public string NoticeReadersColor
        {
            get { return _noticeReadersColor; }
            set { SetProperty(ref _noticeReadersColor, value, "NoticeReadersColor"); }
        }


        public string _noticeSelectDep = "选择角色";
        public string NoticeSelectDep
        {
            get { return _noticeSelectDep; }
            set { SetProperty(ref _noticeSelectDep, value, "NoticeSelectDep"); }
        }

        public string _noticeColorDep = "#CFCFCF";
        public string NoticeColorDep
        {
            get { return _noticeColorDep; }
            set { SetProperty(ref _noticeColorDep, value, "NoticeColorDep"); }
        }
        #endregion
        #region MultiSelect
        public bool DisHasSelect { get; set; } = false;
        List<DistributorDto> _allDisItems = new List<DistributorDto>();
        public List<DistributorDto> AllDisItems
        {
            get { return _allDisItems; }
            set { SetProperty(ref _allDisItems, value, "AllDisItems"); }

        }
        public bool DepHasSelect { get; set; } = false;
        List<DistributorDto> _allDepItems = new List<DistributorDto>();
        public List<DistributorDto> AllDepItems
        {
            get { return _allDepItems; }
            set { SetProperty(ref _allDepItems, value, "AllDepItems"); }

        }

        #endregion
        #region 通知内容
        public string NoticeContentLabel { get; set; } = "通知内容";
        public string NoticeContent { get; set; }

        #endregion
        #region RadioSelect
        public string NoticeReplyLabel { get; set; } = "结果反馈";
        public List<string> NotifiReplyList { get; set; } = new List<string>
        {
            "否","是"
        };

        public int ReplySelected { get; set; } = 0;
        #endregion
        #region 通知附件
        public string NoticeAttachmentLabel { get; set; } = "通知附件";
        #region AttachmentList

        List<DocumentDto> oldAttachmentList = new List<DocumentDto>();
        List<DocumentDto> _notifiAttachmentList;
        public List<DocumentDto> NotifiAttachmentList
        {
            get { return _notifiAttachmentList; }
            set { SetProperty(ref _notifiAttachmentList, value, "NotifiAttachmentList"); }

        }
        private DocumentDto SelectedRow { get; set; }
        //public DocumentDto SelectedRow
        //{
        //    get { return _selectedRow; }
        //    set { this.SetProperty(ref _selectedRow, value); }
        //}
        #endregion
        #endregion
        #endregion Property(s)

        #region Command
        public Command AddAttechmentCommand
        {
            get
            {
                return new Command(async(saveType) =>
                {
                    //通知标题
                    if (string.IsNullOrEmpty(NoticeTitle)|| NoticeTitle.Trim()=="")
                    {
                        _commonFun.AlertLongText("请输入通知标题");
                        return;
                    }
                    //通知有效期
                    if (StartNoticeDate> EndNoticeDate)
                    {
                        _commonFun.AlertLongText("开始日期需小于结束日期");
                        return;
                    }
                    //通知对象
                    if (!DisHasSelect)
                    {
                        _commonFun.AlertLongText("请选择经销商");
                        return;
                    }
                    if (!DepHasSelect)
                    {
                        _commonFun.AlertLongText("请选择部门");
                        return;
                    }
                    //结果反馈
                    //通知内容
                    if (string.IsNullOrEmpty(NoticeContent) || NoticeContent.Trim() == "")
                    {
                        _commonFun.AlertLongText("请输入通知内容");
                        return;
                    }
                    
                    if (_commonHelper.IsNetWorkConnected() == true)
                    {
                        try
                        {
                            if (saveType.Equals("T"))
                                _commonFun.ShowLoading("暂存中...");
                            else if (saveType.Equals("S"))
                                _commonFun.ShowLoading("提交中...");
                            else if (saveType.Equals("C"))
                                return;
                            string disdepList = CommonUtil.DisAndDepToString(AllDisItems,AllDepItems);
                            string attachmentList = NotifiAttachmentList == null || NotifiAttachmentList.Count == 0 ?
                            "" : CommonUtil.CreateXML(NotifiAttachmentList);
                            var result = await _notifiMngService.SaveMadeNotifi(NoticeTitle,
                                                    StartNoticeDate.ToString("yyyyMMdd"),
                                                    EndNoticeDate.ToString("yyyyMMdd"),
                                                    disdepList,
                                                    ReplySelected.ToString(),
                                                    NoticeContent,
                                                    attachmentList,
                                                    saveType.ToString(),
                                                    _noticeId,
                                                    //Convert.ToInt32(CommonContext.Account.UserId)
                                                    6);
                            if (result.ResultCode == Module.ResultType.Success)
                            {
                                _commonFun.HideLoading();
                                //AccountInfo accountInfo = CommonHelper.DecodeString<AccountInfo>(result.Body.Replace("[", "").Replace("]", "").Replace("\r\n", ""));
                                //if (accountInfo != null)
                                //{
                                //    accountInfo.LoggedInAt = DateTime.Now;
                                //    CommonContext.Account = accountInfo;
                                    
                                //}
                                //else
                                //{
                                //    _commonFun.AlertLongText("查询不到用户信息。");
                                //}
                                //await Navigation.PushAsync<MainViewModel>();
                            }
                            else
                            {
                                _commonFun.HideLoading();
                                _commonFun.AlertLongText("保存失败，请重试。 " + result.Msg);
                                //await Navigation.PushAsync(new NavigationPage(ViewFactory.CreatePage<MainViewModel, Page>() as Page));
                                //await CoreMethods.DisplayAlert("Login Failed", "Login failed. Check username or password.", "OK");
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            _commonFun.HideLoading();
                            _commonFun.AlertLongText("请求超时。");
                        }
                        catch (Exception ex)
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
                });
            }
        }
        public Command PickPhotoCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CrossMedia.Current.Initialize(); 

                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        //await DisplayAlert("No PickPhoto", ":( No PickPhoto available.", "OK");
                        return;
                    }

                    _mediaFile = await CrossMedia.Current.PickPhotoAsync();

                    if (_mediaFile == null)
                        return;

                    if (_commonHelper.IsNetWorkConnected() == true)
                    {
                        try
                        {
                            _commonFun.ShowLoading("上传中...");
                            var content = new MultipartFormDataContent();

                            content.Add(new StreamContent(_mediaFile.GetStream()),
                                "\"file\"",
                                $"\"{_mediaFile.Path}\"");
                            var _httpClient = new HttpClient(new NativeMessageHandler());
                            _httpClient.Timeout = new TimeSpan(0, 1, 30);
                            var uploadServiceBaseAddress = "http://10.202.101.44:6505//rmmt/api/v1/uploadfile";
                            var httpResponseMessage = await _httpClient.PostAsync(uploadServiceBaseAddress, content);
                            if (httpResponseMessage.IsSuccessStatusCode)
                            {
                                var contentee = await httpResponseMessage.Content.ReadAsStringAsync();
                                APIResult dto = JsonConvert.DeserializeObject<APIResult>(contentee.ToString());
                                oldAttachmentList.AddRange((JsonConvert.DeserializeObject<List<DocumentDto>>(dto.Body)));
                                List<DocumentDto> resultList = new List<DocumentDto>();
                                foreach (DocumentDto item in oldAttachmentList)
                                {
                                    resultList.Add((DocumentDto)item.Clone());
                                }
                                NotifiAttachmentList = resultList;
                            }
                            else
                            {
                                _commonFun.HideLoading();
                                var contentee = await httpResponseMessage.Content.ReadAsStringAsync();
                                APIResult dto = JsonConvert.DeserializeObject<APIResult>(contentee.ToString());
                                _commonFun.AlertLongText("上传失败，请重试。 " + dto.Msg);
                                //await Navigation.PushAsync(new NavigationPage(ViewFactory.CreatePage<MainViewModel, Page>() as Page));
                                //await CoreMethods.DisplayAlert("Login Failed", "Login failed. Check username or password.", "OK");
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            _commonFun.HideLoading();
                            _commonFun.AlertLongText("请求超时。");
                        }
                        catch (Exception ex)
                        {
                            _commonFun.HideLoading();
                            _commonFun.AlertLongText("上传失败，请重试。");
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
                });
            }
        }

        public Command DeleteAttechmentCommand
        {
            get
            {
                return new Command(async (PathToFile) =>
                {
                    await CrossMedia.Current.Initialize();
                    if (SelectedRow != null && SelectedRow.Id == 0)
                    {
                        oldAttachmentList.Remove(SelectedRow);
                        NotifiAttachmentList = oldAttachmentList;

                    }
                });
            }
        }
        #endregion

        #region GetData

        #region MultiSelectDistributor
        public void SetNoticeDistributor(List<DistributorDto> paramList)
        {
            AllDisItems = paramList;
            List<DistributorDto> selectItems= paramList.FindAll(item => item.IsChecked);
            if (selectItems != null&&selectItems.Count>0)
            {
                NoticeReadersSelect = string.Format("共选择{0}项", selectItems.Count);
                NoticeReadersColor = "#3998c0";
                DisHasSelect = true;
            }
            else
            {
                NoticeReadersSelect = "选择经销商";
                NoticeReadersColor = "#CFCFCF";
                DisHasSelect = false;
            }
        }
        #endregion

        #region MultiSelectDepartment

        public void SetNoticeDepartment(List<DistributorDto> paramList)
        {
            AllDepItems = paramList;
            List<DistributorDto> selectItems = paramList.FindAll(item => item.IsChecked);
            if (selectItems != null && selectItems.Count > 0)
            {
                NoticeSelectDep = string.Format("共选择{0}项", selectItems.Count);
                NoticeColorDep = "#3998c0";
                DepHasSelect = true;
            }
            else
            {
                NoticeSelectDep = "选择角色";
                NoticeColorDep = "#CFCFCF";
                DepHasSelect = false;
            }
        }
        #endregion

        #region Attachment
        public void DeleteAttechmentNotice(string pathToFile)
        {
            oldAttachmentList.Remove(oldAttachmentList.Find(item => item.PathToFile == pathToFile));
            List<DocumentDto> resultList = new List<DocumentDto>();
            foreach (DocumentDto item in oldAttachmentList)
            {
                resultList.Add((DocumentDto)item.Clone());
            }
            NotifiAttachmentList = resultList;
        }
        #endregion
        #endregion
    }
}
