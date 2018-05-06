using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto.Improve;
using MaxInsight.Mobile.Services.ImproveService;
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

namespace MaxInsight.Mobile.ViewModels.Improve
{
    public class ImpResultCommitViewModel : ViewModel
    {
        private readonly ITourService _tourService;
        IImproveService _improveService;
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        private MediaFile _mediaFile;
        private ImprovementMngDto _improvementMng;
        public ImpResultCommitViewModel()
        {
            _improveService = Resolver.Resolve<IImproveService>();
            _tourService = Resolver.Resolve<ITourService>();
            _commonFun = Resolver.Resolve<ICommonFun>();
            _commonHelper = Resolver.Resolve<CommonHelper>();

            //MessagingCenter.Unsubscribe<List<RequestParameter>>(this, MessageConst.IMPROVE_PLANORRESULTDETAIL_GETR);
            MessagingCenter.Unsubscribe<List<RequestParameter>>(this, MessageConst.IMPROVE_RESULTATTACH_DELETE);
            //MessagingCenter.Subscribe<List<RequestParameter>>(
            //    this,
            //    MessageConst.IMPROVE_PLANORRESULTDETAIL_GETR,
            //    (param) =>
            //    {
            //        GetImpResultOrResultDetail(param);
            //    });
            MessagingCenter.Subscribe<string>(
            this,
            MessageConst.IMPROVE_RESULTATTACH_DELETE,
            (SeqNo) =>
            {
                DeleteResultAttach(SeqNo);
            });
            MessagingCenter.Subscribe<List<RequestParameter>>(
            this,
            MessageConst.IMPROVE_RESULTAPPLYYN_CHANGE,
            (ApplyYN) =>
            {
                SetApplyYN(ApplyYN);
            });
        }

        #region properties
        string improvementId = "";
        string impResultId = "";
        string tPId = "";
        string itemId = "";
        string resultApproalYN = "";
        string resultStatus = "";
        string allocateYN = "";

        string impServerApplyYNStr = "";
        string impAreaApplyYNStr = "";
        string resultContent = "";

        string _impResultContent;
        public string ImpResultContent
        {
            get { return _impResultContent; }
            set { SetProperty(ref _impResultContent, value); }
        }
        int _serverApplyYN;
        public int ServerApplyYN
        {
            get { return _serverApplyYN; }
            set { SetProperty(ref _serverApplyYN, value); }
        }
        string _serverApplyYNName;
        public string ServerApplyYNName
        {
            get { return _serverApplyYNName; }
            set { SetProperty(ref _serverApplyYNName, value); }
        }
        string _serverApplyMemo;
        public string ServerApplyMemo
        {
            get { return _serverApplyMemo; }
            set { SetProperty(ref _serverApplyMemo, value); }
        }
        int _areaApplyYN;
        public int AreaApplyYN
        {
            get { return _areaApplyYN; }
            set { SetProperty(ref _areaApplyYN, value); }
        }
        string _areaApplyYNName;
        public string AreaApplyYNName
        {
            get { return _areaApplyYNName; }
            set { SetProperty(ref _areaApplyYNName, value); }
        }
        string _areaApplyMemo;
        public string AreaApplyMemo
        {
            get { return _areaApplyMemo; }
            set { SetProperty(ref _areaApplyMemo, value); }
        }
        string _resultStatus;
        public string ResultStatus
        {
            get { return _resultStatus; }
            set { SetProperty(ref _resultStatus, value); }
        }
        string _sApprovalDate;
        public string SApprovalDate
        {
            get { return _sApprovalDate; }
            set { SetProperty(ref _sApprovalDate, value); }
        }
        string _zApprovalDate;
        public string ZApprovalDate
        {
            get { return _zApprovalDate; }
            set { SetProperty(ref _zApprovalDate, value); }
        }
        List<AttachDto> _impResultAttachList;
        public List<AttachDto> ImpResultAttachList
        {
            get { return _impResultAttachList; }
            set { SetProperty(ref _impResultAttachList, value); }
        }
        AttachDto _impResultAttachItem;
        public AttachDto ImpResultAttachItem
        {
            get { return _impResultAttachItem; }
            set { SetProperty(ref _impResultAttachItem, value); }
        }
        List<AttachDto> oldImpResultAttachList = new List<AttachDto>();

        double _lstRowHeight = 32;
        double _lstHeight;
        public double LstHeight
        {
            get { return _lstHeight; }
            set { SetProperty(ref _lstHeight, value); }
        }
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }
        private string _impResultCommitPageTitle;
        public string ImpResultCommitPageTitle
        {
            get { return _impResultCommitPageTitle; }
            set { SetProperty(ref _impResultCommitPageTitle, value); }
        }
        private RelayCommand _allocateDetailCommand;
        public RelayCommand AllocateDetailCommand
        {
            get
            {
                return _allocateDetailCommand ??

                (_allocateDetailCommand = new RelayCommand(GotoImproveDistributionPage));
            }
        }
        private RelayCommand _impPlanCommand;
        public RelayCommand ImpPlanCommand
        {
            get
            {
                return _impPlanCommand ??

                (_impPlanCommand = new RelayCommand(GoImpPlanCommitPage));
            }
        }
        #endregion

        #region GoTargetPage
        private async void GotoImproveDistributionPage()
        {
            CommonContext.ImpPlanStatus = _improvementMng.PlanStatus;
            if (CommonContext.Account.UserType == "S")
            {
                await Navigation.PushAsync<ImproveDistributionViewModel>((vm, v) => vm.Init(_improvementMng), true);
            }
            else
            {
                if (_improvementMng.PlanStatus == "A")
                {
                    _commonFun.AlertLongText("未分配，没有分配详细");
                }
                else
                {
                    await Navigation.PushAsync<ImproveDistributionViewModel>((vm, v) => vm.Init(_improvementMng), true);
                }
            }

        }
        private async void GoImpPlanCommitPage()
        {
            try
            {
                List<RequestParameter> list = new List<RequestParameter>();
                list.Add(new RequestParameter { Name = "improvementId", Value = _improvementMng.ImprovementId.ToString() });
                list.Add(new RequestParameter { Name = "impResultId", Value = _improvementMng.ImpResultId.ToString() });
                list.Add(new RequestParameter { Name = "tPId", Value = _improvementMng.TPId.ToString() });
                list.Add(new RequestParameter { Name = "itemId", Value = _improvementMng.ItemId.ToString() });
                list.Add(new RequestParameter { Name = "planApproalYN", Value = _improvementMng.PlanApproalYN.ToString() });
                list.Add(new RequestParameter { Name = "PlanStatus", Value = _improvementMng.PlanStatus });
                list.Add(new RequestParameter { Name = "AllocateYN", Value = _improvementMng.AllocateYN.ToString() });
                await Navigation.PushAsync<ImpPlanCommitViewModel>((vm, v) => vm.Init(_improvementMng, list), true);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region searchData
        private async void GetImpResultOrResultDetail(List<RequestParameter> param)
        {
            if (param != null)
            {
                foreach (var item in param)
                {
                    if (item.Name.ToUpper() == "IMPROVEMENTID") improvementId = item.Value;
                    if (item.Name.ToUpper() == "IMPRESULTID") impResultId = item.Value;
                    if (item.Name.ToUpper() == "TPID") tPId = item.Value;
                    if (item.Name.ToUpper() == "ITEMID") itemId = item.Value;
                    if (item.Name.ToUpper() == "RESULTAPPROALYN") resultApproalYN = item.Value;
                    if (item.Name.ToUpper() == "RESULTSTATUS") resultStatus = item.Value;
                    if (item.Name.ToUpper() == "ALLOCATEYN") allocateYN = item.Value;
                }
                MessagingCenter.Send<string>(resultStatus + "§" + resultApproalYN + "§" + allocateYN, MessageConst.IMPROVE_RESULTCOMMIT_SETCONTROLROLE);
                if ((allocateYN.ToUpper() == "FALSE" && CommonContext.Account.UserType == "S")|| CommonContext.Account.UserType == "D")
                    ImpResultCommitPageTitle = "改善结果提交";
                else
                    ImpResultCommitPageTitle = "改善结果审批";
                if (_commonHelper.IsNetWorkConnected() == true)
                {
                    try
                    {
                        _commonFun.ShowLoading("查询中...");
                        var result = await _improveService.GetImpPlanOrResultDetail(improvementId, "R", impResultId, tPId, itemId);// ("14", "A", "3", "21", "7");
                        if (result.ResultCode == Module.ResultType.Success)
                        {
                            _commonFun.HideLoading();
                            var impResultDetailData = CommonHelper.DecodeString<ImpResultDetailDto>(result.Body);
                            if (impResultDetailData == null)
                            {
                                _commonFun.AlertLongText("查询失败，请重试。");
                                return;
                            }

                            ImpResultContent = impResultDetailData.ReplyContent;

                            if (CommonContext.Account.UserType == "S")
                            {
                                if (impResultDetailData.SResultStatus == "D" || impResultDetailData.SResultStatus == "E")
                                {
                                    ServerApplyYN = impResultDetailData.SResultStatus == "D" ? 1 : 0;
                                }
                            }
                            ServerApplyYNName = impResultDetailData.SResultStatusName;
                            ServerApplyMemo = impResultDetailData.ApprovalSContent;
                            if (CommonContext.Account.UserType == "Z")
                            {
                                if (impResultDetailData.ZResultStatus == "G" || impResultDetailData.ZResultStatus == "F")
                                {
                                    AreaApplyYN = impResultDetailData.ZResultStatus == "G" ? 0 : 1;
                                }
                            }
                            AreaApplyYNName = impResultDetailData.ZResultStatusName;
                            AreaApplyMemo = impResultDetailData.ApprovalZContent;
                            ImpResultAttachList = impResultDetailData.AttachList;
                            ResultStatus = impResultDetailData.ResultStatus;
                            SApprovalDate = impResultDetailData.SApprovalDate;
                            ZApprovalDate = impResultDetailData.ZApprovalDate;
                            oldImpResultAttachList = new List<AttachDto>();
                            oldImpResultAttachList.AddRange(ImpResultAttachList);
                            LstHeight = ImpResultAttachList.Count * _lstRowHeight;
                        }
                        else
                        {
                            _commonFun.HideLoading();
                            _commonFun.AlertLongText("查询失败，请重试。 " + result.Msg);
                            return;
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        _commonFun.HideLoading();
                        _commonFun.AlertLongText("请求超时。");
                        return;
                    }
                    catch (Exception)
                    {
                        _commonFun.HideLoading();
                        _commonFun.AlertLongText("查询异常，请重试");
                        return;
                    }
                    finally
                    {
                        _commonFun.HideLoading();
                    }
                }
                else
                {
                    _commonFun.AlertLongText("网络连接异常。");
                    return;
                }
            }
            else
            {
                _commonFun.AlertLongText("查询失败，请重试。");
                return;
            }
        }
        #endregion

        #region Command
        public Command SaveImpResultCommand
        {
            get
            {
                return new Command(() =>
                {
                    SaveImprovementResult();
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

        #region method
        public void DeleteResultAttach(string SeqNo)
        {
            oldImpResultAttachList.Remove(oldImpResultAttachList.Find(item => item.SeqNo == int.Parse(SeqNo)));
            List<AttachDto> newList = new List<AttachDto>();
            newList.AddRange(oldImpResultAttachList);
            int i = 1;
            newList.Select(c => { c.SeqNo = i++; return c; }).ToList();
            ImpResultAttachList = newList;
            LstHeight = ImpResultAttachList.Count * _lstRowHeight;
        }
        public async void SaveImprovementResult()
        {
            if (ResultStatus == "A" || ResultStatus == "D"||(allocateYN.ToUpper() == "FALSE"&& resultApproalYN.ToUpper() == "TRUE" && ResultStatus=="F"))
            {
                //改善结果内容
                if (string.IsNullOrWhiteSpace(ImpResultContent))
                {
                    _commonFun.AlertLongText("请输入改善结果内容");
                    return;
                }
            }
            else if ((ResultStatus == "C" || ResultStatus == "F")&&allocateYN.ToUpper()=="TRUE")
            {
                if (ServerApplyYN.ToString() != impServerApplyYNStr)
                {
                    _commonFun.AlertLongText("请选择审核类型");
                    return;
                }
                if (string.IsNullOrWhiteSpace(ServerApplyMemo))
                {
                    _commonFun.AlertLongText("请输入审核意见内容");
                    return;
                }
            }
            else if (ResultStatus == "E"&&resultApproalYN.ToUpper()=="TRUE")
            {
                if (AreaApplyYN.ToString() != impAreaApplyYNStr)
                {
                    _commonFun.AlertLongText("请选择审核类型");
                    return;
                }
                if (string.IsNullOrWhiteSpace(AreaApplyMemo))
                {
                    _commonFun.AlertLongText("请输入审核意见内容");
                    return;
                }
            }

            if (_commonHelper.IsNetWorkConnected() == true)
            {
                try
                {
                    _commonFun.ShowLoading("提交中...");

                    string saveStatus = "";
                    if(allocateYN.ToUpper() == "TRUE")
                    {
                        if (impServerApplyYNStr == "1")
                        {
                            if (ResultStatus == "D")
                            {
                                saveStatus = "C";
                                resultContent = ImpResultContent;
                            }
                            else
                            {
                                saveStatus = "D";
                                resultContent = ServerApplyMemo;
                            }
                        }
                        else if (impAreaApplyYNStr == "1")
                        {
                            if (ResultStatus == "F")
                            {
                                saveStatus = "E";
                                resultContent = ServerApplyMemo;
                            }
                            else
                            {
                                saveStatus = "F";
                                resultContent = AreaApplyMemo;
                            }
                        }
                        else if (impAreaApplyYNStr == "0")
                        {
                            saveStatus = "G";
                            resultContent = AreaApplyMemo;
                        }
                        else if (impServerApplyYNStr == "0")
                        {
                            saveStatus = "E";
                            resultContent = ServerApplyMemo;
                        }
                        else
                        {
                            saveStatus = "C";
                            resultContent = ImpResultContent;
                        }
                    }
                    else
                    {
                        if (impAreaApplyYNStr == "1")
                        {
                            if (ResultStatus == "F")
                            {
                                saveStatus = "E";
                                resultContent = ImpResultContent;
                            }
                            else
                            {
                                saveStatus = "F";
                                resultContent = AreaApplyMemo;
                            }
                        }
                        else if (impAreaApplyYNStr == "0")
                        {
                            saveStatus = "G";
                            resultContent = AreaApplyMemo;
                        }
                        else
                        {
                            if (resultApproalYN.ToUpper() == "TRUE")
                                saveStatus = "E";
                            else
                                saveStatus = "G";
                            resultContent = ImpResultContent;
                        }
                    }
                    
                    var result = await _improveService.SaveImprovementResult(improvementId, impResultId, saveStatus, resultContent, ImpResultAttachList);
                    if (result.ResultCode == Module.ResultType.Success)
                    {
                        //_commonFun.HideLoading();
                        //_commonFun.AlertLongText("提交完毕。 ");
                        MessagingCenter.Send<string>("R", MessageConst.IMPROVE_PLANLSTDATA_GET);
                        MessagingCenter.Send<String>("", MessageConst.IMPROVE_IMPPLANORRESULTDATA_GET);
                        MessagingCenter.Send<string>("", "MessagePageReSearch");// 给消息页发消息
                        await Navigation.PopAsync();
                        //MessagingCenter.Send(this, "PlanCommitPopBack");
                    }
                    else
                    {
                        _commonFun.HideLoading();
                        _commonFun.AlertLongText("提交失败，请重试。 " + result.Msg);
                        return;
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
                    return;
                }
                finally
                {
                    _commonFun.HideLoading();
                }
            }
            else
            {
                _commonFun.AlertLongText("网络连接异常。");
                return;
            }
        }
        public void SetApplyYN(List<RequestParameter> applyYN)
        {
            foreach (var item in applyYN)
            {
                if (item.Name == "Server")
                {
                    impServerApplyYNStr = item.Value;
                }
                else if (item.Name == "Area")
                {
                    if (item.Value != "1")
                    {
                        impServerApplyYNStr = ServerApplyYN.ToString();
                    }
                    impAreaApplyYNStr = item.Value;
                }
            }
        }
        public async void UploadFile(string fileType)
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
                    var result =await _tourService.UploadFiletoOss(_stream, filename,_path);

                    if (result != null)
                    {
                        oldImpResultAttachList.Add((JsonConvert.DeserializeObject<AttachDto>(result.Body)));
                        List<AttachDto> resultList = new List<AttachDto>();
                        resultList.AddRange(oldImpResultAttachList);
                        int i = 1;
                        resultList.Select(c => { c.SeqNo = i++; return c; }).ToList();
                        ImpResultAttachList = resultList;
                        LstHeight = ImpResultAttachList.Count * _lstRowHeight;
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
                List<ImagePreviewDto> list = new List<ImagePreviewDto>();
                string url = ImpResultAttachItem.Url.ToUpper();
                if (url.EndsWith(".JPG") || url.EndsWith(".BMP") || url.EndsWith(".GIF") ||
                    url.EndsWith(".JPEG") || url.EndsWith(".PNG") || url.EndsWith(".SVG"))
                {
                    //list.Add(new ImagePreviewDto { Url = ImpResultAttachItem.Url });
                    //Device.BeginInvokeOnMainThread(async () =>
                    //{
                    //    await PopupNavigation.PushAsync(new PreviewImagePage(list, 0), true);
                    //});
                    string filename = ImpResultAttachItem.Url.LastIndexOf("/") > 0 ? ImpResultAttachItem.Url.Substring(ImpResultAttachItem.Url.LastIndexOf("/") + 1) : "";
                    if (String.IsNullOrEmpty(filename)) return;
                    if (CrossFilePicker.Current.IsExistFile(filename, "RMMTIMAGEVIEW"))
                    {
                        //已从服务器上缓存的图片。
                        CrossFilePicker.Current.OpenFile(filename, "RMMTIMAGEVIEW");
                    }
                    else
                    {
                        _commonFun.DownLoadFileFromOss(ImpResultAttachItem.Url, filename, "RMMTIMAGEVIEW");
                    }
                }
                else
                {
                    _commonFun.AlertLongText("请在电脑端阅览");
                    return;
                }

            }
            catch
            {
                _commonFun.AlertLongText("加载失败，请重试");
            }
        }
        #endregion

        #region page init
        public void Init(ImprovementMngDto improvementMng,List<RequestParameter> param)
        {
            IsLoading = false;
            oldImpResultAttachList = new List<AttachDto>();
            LstHeight = 0;
            _improvementMng = improvementMng;
            GetImpResultOrResultDetail(param);
        }
        #endregion
    }
}
