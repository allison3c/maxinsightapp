using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto.Improve;
using MaxInsight.Mobile.Services.ImproveService;
using MaxInsight.Mobile.Services.Tour;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;
using System.IO;
using Plugin.FilePicker.Abstractions;
using Plugin.FilePicker;
using Newtonsoft.Json;
using XLabs;

namespace MaxInsight.Mobile.ViewModels.Improve
{
    public class ImpPlanCommitViewModel : ViewModel
    {
        private readonly ITourService _tourService;
        IImproveService _improveService;
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        private MediaFile _mediaFile;
        private ImprovementMngDto _improvementMng;
        public ImpPlanCommitViewModel()
        {
            _improveService = Resolver.Resolve<IImproveService>();
            _tourService = Resolver.Resolve<ITourService>();
            _commonFun = Resolver.Resolve<ICommonFun>();
            _commonHelper = Resolver.Resolve<CommonHelper>();

            NowDate = DateTime.Now;
            ImpPlanCompleteDate = NowDate;

            //MessagingCenter.Unsubscribe<List<RequestParameter>>(this, MessageConst.IMPROVE_PLANORRESULTDETAIL_GET);
            MessagingCenter.Unsubscribe<List<RequestParameter>>(this, MessageConst.IMPROVE_PLANATTACH_DELETE);
            //MessagingCenter.Subscribe<List<RequestParameter>>(
            //    this,
            //    MessageConst.IMPROVE_PLANORRESULTDETAIL_GET,
            //    (param) =>
            //    {
            //        GetImpPlanOrResultDetail(param);
            //    });
            MessagingCenter.Subscribe<string>(
            this,
            MessageConst.IMPROVE_PLANATTACH_DELETE,
            (SeqNo) =>
            {
                DeletePlanAttach(SeqNo);
            });
            MessagingCenter.Subscribe<List<RequestParameter>>(
            this,
            MessageConst.IMPROVE_PLANAPPLYYN_CHANGE,
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
        string planApproalYN = "";
        string planContent = "";
        string planStatus = "";
        string allocateYN = "";

        string impServerApplyYNStr = "";
        string impAreaApplyYNStr = "";

        string _impPlanContent;
        public string ImpPlanContent
        {
            get { return _impPlanContent; }
            set { SetProperty(ref _impPlanContent, value); }
        }
        DateTime _impPlanCompleteDate;
        public DateTime ImpPlanCompleteDate
        {
            get { return _impPlanCompleteDate; }
            set { SetProperty(ref _impPlanCompleteDate, value); }
        }
        string _impPlanCompleteDateStr;
        public string ImpPlanCompleteDateStr
        {
            get { return _impPlanCompleteDateStr; }
            set { SetProperty(ref _impPlanCompleteDateStr, value); }
        }
        DateTime _nowDate;
        public DateTime NowDate
        {
            get { return _nowDate; }
            set { SetProperty(ref _nowDate, value); }
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
        string _planStatus;
        public string PlanStatus
        {
            get { return _planStatus; }
            set { SetProperty(ref _planStatus, value); }
        }
        string _feedbackTime;
        public string FeedbackTime
        {
            get { return _feedbackTime; }
            set { SetProperty(ref _feedbackTime, value); }
        }
        string _feedbackRegionTime;
        public string FeedbackRegionTime
        {
            get { return _feedbackRegionTime; }
            set { SetProperty(ref _feedbackRegionTime, value); }
        }
        List<AttachDto> _impPlanAttachList;
        public List<AttachDto> ImpPlanAttachList
        {
            get { return _impPlanAttachList; }
            set { SetProperty(ref _impPlanAttachList, value); }
        }
        AttachDto _impPlanAttachItem;
        public AttachDto ImpPlanAttachItem
        {
            get { return _impPlanAttachItem; }
            set { SetProperty(ref _impPlanAttachItem, value); }
        }
        List<AttachDto> oldImpPlanAttachList = new List<AttachDto>();

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
        private string _impPlanCommitPageTitle;
        public string ImpPlanCommitPageTitle
        {
            get { return _impPlanCommitPageTitle; }
            set { SetProperty(ref _impPlanCommitPageTitle, value); }
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
        #endregion

        #region searchData
        private async void GetImpPlanOrResultDetail(List<RequestParameter> param)
        {
            if (param != null)
            {
                foreach (var item in param)
                {
                    if (item.Name.ToUpper() == "IMPROVEMENTID") improvementId = item.Value;
                    if (item.Name.ToUpper() == "IMPRESULTID") impResultId = item.Value;
                    if (item.Name.ToUpper() == "TPID") tPId = item.Value;
                    if (item.Name.ToUpper() == "ITEMID") itemId = item.Value;
                    if (item.Name.ToUpper() == "PLANAPPROALYN") planApproalYN = item.Value;
                    if (item.Name.ToUpper() == "PLANSTATUS") planStatus = item.Value;
                    if (item.Name.ToUpper() == "ALLOCATEYN") allocateYN = item.Value;
                }
                MessagingCenter.Send<string>(planStatus + "§" + planApproalYN+ "§" + allocateYN, MessageConst.IMPROVE_PLANCOMMIT_SETCONTROLROLE);
                if ((allocateYN.ToUpper() == "FALSE" && CommonContext.Account.UserType == "S") || CommonContext.Account.UserType == "D")
                    ImpPlanCommitPageTitle = "改善计划提交";
                else
                    ImpPlanCommitPageTitle = "改善计划审批";
                if (_commonHelper.IsNetWorkConnected() == true)
                {
                    try
                    {
                        _commonFun.ShowLoading("查询中...");
                        var result = await _improveService.GetImpPlanOrResultDetail(improvementId, "A", impResultId, tPId, itemId);// ("14", "A", "3", "21", "7");
                        if (result.ResultCode == Module.ResultType.Success)
                        {
                            _commonFun.HideLoading();
                            var impPlanDetailData = CommonHelper.DecodeString<ImpPlanDetailDto>(result.Body);
                            if (impPlanDetailData == null)
                            {
                                _commonFun.AlertLongText("查询失败，请重试。");
                                return;
                            }

                            ImpPlanContent = impPlanDetailData.ImprovementPlan;
                            if (!string.IsNullOrWhiteSpace(impPlanDetailData.ExpectedTime))
                            {
                                ImpPlanCompleteDate = DateTime.Parse(impPlanDetailData.ExpectedTime);
                                ImpPlanCompleteDateStr = ImpPlanCompleteDate.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                ImpPlanCompleteDateStr = "";
                            }
                            if (CommonContext.Account.UserType == "S")
                            {
                                if (impPlanDetailData.SPlanStatus == "D" || impPlanDetailData.SPlanStatus == "E")
                                {
                                    ServerApplyYN = impPlanDetailData.SPlanStatus == "D" ? 1 : 0;
                                }
                            }
                            ServerApplyYNName = impPlanDetailData.SPlanStatusName;
                            ServerApplyMemo = impPlanDetailData.DisApprovalPlan;
                            if (CommonContext.Account.UserType == "Z")
                            {
                                if (impPlanDetailData.RPlanStatus == "G" || impPlanDetailData.RPlanStatus == "F")
                                {
                                    AreaApplyYN = impPlanDetailData.RPlanStatus == "G" ? 0 : 1;
                                }
                            }
                            AreaApplyYNName = impPlanDetailData.RPlanStatusName;
                            AreaApplyMemo = impPlanDetailData.RegionApprovalPlan;
                            ImpPlanAttachList = impPlanDetailData.AttachList;
                            PlanStatus = impPlanDetailData.PlanStatus;
                            FeedbackTime = impPlanDetailData.FeedbackTime;
                            FeedbackRegionTime = impPlanDetailData.FeedbackRegionTime;
                            oldImpPlanAttachList = new List<AttachDto>();
                            oldImpPlanAttachList.AddRange(ImpPlanAttachList);
                            LstHeight = ImpPlanAttachList.Count * _lstRowHeight;
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
                        _commonFun.AlertLongText("查询异常，请重试。");
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
        public Command SaveImpPlanCommand
        {
            get
            {
                return new Command(() =>
                {
                    SaveImprovementItem();
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
        public void DeletePlanAttach(string SeqNo)
        {
            oldImpPlanAttachList.Remove(oldImpPlanAttachList.Find(item => item.SeqNo == int.Parse(SeqNo)));
            List<AttachDto> newList = new List<AttachDto>();
            newList.AddRange(oldImpPlanAttachList);
            int i = 1;
            newList.Select(c => { c.SeqNo = i++; return c; }).ToList();
            ImpPlanAttachList = newList;
            LstHeight = ImpPlanAttachList.Count * _lstRowHeight;
        }
        public async void SaveImprovementItem()
        {
            if (PlanStatus == "B" || PlanStatus == "D" || (allocateYN.ToUpper() == "FALSE" && planApproalYN.ToUpper() == "TRUE" && PlanStatus == "F"))
            {
                //改善计划内容
                if (string.IsNullOrWhiteSpace(ImpPlanContent))
                {
                    _commonFun.AlertLongText("请输入改善计划内容");
                    return;
                }
            }
            else if ((PlanStatus == "C" || PlanStatus == "F")&&allocateYN.ToUpper()=="TRUE")//Modify by dong.limin 2017-2-23 13:36:29
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
            else if (PlanStatus == "E"&&planApproalYN.ToUpper()=="TRUE")//Modify by dong.limin 2017-2-23 13:36:29
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
                            if (PlanStatus == "D")
                            {
                                saveStatus = "C";
                                planContent = ImpPlanContent;
                            }
                            else
                            {
                                saveStatus = "D";
                                planContent = ServerApplyMemo;
                            }
                        }
                        else if (impAreaApplyYNStr == "1")
                        {
                            if (PlanStatus == "F")
                            {
                                saveStatus = "E";
                                planContent = ServerApplyMemo;
                            }
                            else
                            {
                                saveStatus = "F";
                                planContent = AreaApplyMemo;
                            }
                        }
                        else if (impAreaApplyYNStr == "0")
                        {
                            saveStatus = "G";
                            planContent = AreaApplyMemo;
                        }
                        else if (impServerApplyYNStr == "0")
                        {
                            saveStatus = "E";
                            planContent = ServerApplyMemo;
                        }
                        else
                        {
                            saveStatus = "C";
                            planContent = ImpPlanContent;
                        }
                    }
                    else
                    {
                        if (impAreaApplyYNStr == "1")
                        {
                            if (PlanStatus == "F")
                            {
                                saveStatus = "E";
                                planContent = ImpPlanContent;
                            }
                            else
                            {
                                saveStatus = "F";
                                planContent = AreaApplyMemo;
                            }
                        }
                        else if (impAreaApplyYNStr == "0")
                        {
                            saveStatus = "G";
                            planContent = AreaApplyMemo;
                        }
                        else
                        {
                            if (planApproalYN.ToUpper() == "TRUE")
                                saveStatus = "E";
                            else
                                saveStatus = "G";
                            planContent = ImpPlanContent;
                        }
                    }
                    
                    var result = await _improveService.SaveImprovementItem(planContent, ImpPlanCompleteDate.ToString("yyyyMMdd HH:mm:ss"), improvementId,
                        CommonContext.Account.UserId, ImpPlanAttachList, saveStatus);
                    if (result.ResultCode == Module.ResultType.Success)
                    {
                        //_commonFun.HideLoading();
                        //_commonFun.AlertLongText("提交完毕。 ");
                        //Device.BeginInvokeOnMainThread(async () => { 
                        await Navigation.PopAsync();
                        //});
                        //MessagingCenter.Send<ImpPlanCommitPage>(new ImpPlanCommitPage(), "PlanCommitPopBack");
                        MessagingCenter.Send<string>("A", MessageConst.IMPROVE_PLANLSTDATA_GET);
                        MessagingCenter.Send<String>("", MessageConst.IMPROVE_IMPPLANORRESULTDATA_GET);
                        MessagingCenter.Send<string>("", "MessagePageReSearch");// 给消息页发消息
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
                    return;
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
                    var result = await _tourService.UploadFiletoOss(_stream, filename,_path);

                    if (result != null)
                    {
                        oldImpPlanAttachList.Add((JsonConvert.DeserializeObject<AttachDto>(result.Body)));
                        List<AttachDto> resultList = new List<AttachDto>();
                        resultList.AddRange(oldImpPlanAttachList);
                        int i = 1;
                        resultList.Select(c => { c.SeqNo = i++; return c; }).ToList();
                        ImpPlanAttachList = resultList;
                        LstHeight = ImpPlanAttachList.Count * _lstRowHeight;
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
                string url = ImpPlanAttachItem.Url.ToUpper();
                if (url.EndsWith(".JPG") || url.EndsWith(".BMP") || url.EndsWith(".GIF") ||
                    url.EndsWith(".JPEG") || url.EndsWith(".PNG") || url.EndsWith(".SVG"))
                {
                    //list.Add(new ImagePreviewDto { Url = ImpPlanAttachItem.Url });
                    //Device.BeginInvokeOnMainThread(async () =>
                    //{
                    //    await PopupNavigation.PushAsync(new PreviewImagePage(list, 0), true);
                    //});
                    string filename = ImpPlanAttachItem.Url.LastIndexOf("/") > 0 ? ImpPlanAttachItem.Url.Substring(ImpPlanAttachItem.Url.LastIndexOf("/") + 1) : "";
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
                        _commonFun.DownLoadFileFromOss(ImpPlanAttachItem.Url, filename, "RMMTIMAGEVIEW");
                        //已从服务器上缓存的图片。
                        CrossFilePicker.Current.OpenFile(filename, "RMMTIMAGEVIEW");
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
            oldImpPlanAttachList = new List<AttachDto>();
            LstHeight = 0;
            _improvementMng = improvementMng;
            GetImpPlanOrResultDetail(param);
        }
        #endregion
    }
}
