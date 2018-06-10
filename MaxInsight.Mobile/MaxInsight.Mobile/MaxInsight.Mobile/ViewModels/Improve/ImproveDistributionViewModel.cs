using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto;
using MaxInsight.Mobile.Module.Dto.Improve;
using MaxInsight.Mobile.Pages.Improve;
using MaxInsight.Mobile.Services.ImproveService;
using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.Improve
{
    public class ImproveDistributionViewModel : ViewModel
    {
        IImproveService improveService;
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        ImprovementMngDto improvementMng;
        List<RequestParameter> paramList = new List<RequestParameter>();
        public ImproveDistributionViewModel()
        {
            try
            {
                improveService = Resolver.Resolve<IImproveService>();
                _commonFun = Resolver.Resolve<ICommonFun>();
                _commonHelper = Resolver.Resolve<CommonHelper>();
                //MessagingCenter.Unsubscribe<ImprovementMngDto>(this, MessageConst.IMPROVEDISTRIBUTION_SEND);
                //MessagingCenter.Subscribe<ImprovementMngDto>(this, MessageConst.IMPROVEDISTRIBUTION_SEND, m =>
                //  {
                //      DepartmentSelect = "选择";
                //      Department = null;
                //      if (m != null)
                //      {
                //          if (CommonContext.Account.UserType == "S")
                //          {
                //              if (m.PlanStatus == "A")
                //              {
                //                  IsEdit = true;
                //                  IsShow = false;
                //              }
                //              else
                //              {
                //                  IsEdit = false;
                //                  IsShow = true;
                //              }
                //          }
                //          else
                //          {
                //              IsEdit = false;
                //              IsShow = true;
                //          }
                //          improvementMng = m;
                //          ExecDepartmentName = m.ExecDepartmentName;
                //          PlanApproal = m.PlanApproalYN == true ? "区域" : "服务经理";
                //          ResultApproal = m.ResultApproalYN == true ? "区域" : "服务经理";
                //          GetImproveDistributionDetail(m);
                //      }
                //  });
                MessagingCenter.Unsubscribe<DepartmentDto>(this, MessageConst.RESPONSIBLEDEPARTMENT_SEND);
                MessagingCenter.Subscribe<DepartmentDto>(this, MessageConst.RESPONSIBLEDEPARTMENT_SEND, l =>
                {
                    DepartmentSelect = l.DName;
                    Department = l;
                });
                //MessagingCenter.Subscribe<List<RequestParameter>>(this, MessageConst.SEARCHCONDITION_PASS, (param) =>
                //{
                //    if (param != null && param.Count > 0)
                //    {
                //        paramList = param;
                //    }

                //});
                MessagingCenter.Unsubscribe<ImproveDistributionPage>(this, "PreviewImage");
                MessagingCenter.Subscribe<ImproveDistributionPage>(this, "PreviewImage", n =>
                {
                    Preview();
                });
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ImproveDistributionViewModel");
                return;
            }
        }
        #region Property
        private string execDepartmentName;
        public string ExecDepartmentName
        {
            get { return execDepartmentName; }
            set { SetProperty(ref execDepartmentName, value); }
        }
        private string planApproal;
        public string PlanApproal
        {
            get { return planApproal; }
            set { SetProperty(ref planApproal, value); }
        }
        private string resultApproal;
        public string ResultApproal
        {
            get { return resultApproal; }
            set { SetProperty(ref resultApproal, value); }
        }
        private string improvementCaption;
        public string ImprovementCaption
        {
            get { return improvementCaption; }
            set { SetProperty(ref improvementCaption, value); }
        }
        private string lostDescription;
        public string LostDescription
        {
            get { return lostDescription; }
            set { SetProperty(ref lostDescription, value); }
        }
        private string score;
        public string Score
        {
            get { return score; }
            set { SetProperty(ref score, value); }
        }
        private string departmentSelect;
        public string DepartmentSelect
        {
            get { return departmentSelect; }
            set { SetProperty(ref departmentSelect, value); }
        }
        private DepartmentDto department;
        public DepartmentDto Department
        {
            get { return department; }
            set { SetProperty(ref department, value); }
        }
        List<PicDto> picList;
        public List<PicDto> PicList
        {
            get { return picList; }
            set { SetProperty(ref picList, value); }
        }
        PicDescDto selectedPicDesc;
        public PicDescDto SelectedPicDesc
        {
            get { return selectedPicDesc; }
            set { SetProperty(ref selectedPicDesc, value); }
        }
        List<StandardDto> standardList;
        public List<StandardDto> StandardList
        {
            get { return standardList; }
            set { SetProperty(ref standardList, value); }
        }
        List<PicDescDto> picDescList;
        public List<PicDescDto> PicDescList
        {
            get { return picDescList; }
            set { SetProperty(ref picDescList, value); }
        }
        private bool isEdit;
        public bool IsEdit
        {
            get { return isEdit; }
            set { SetProperty(ref isEdit, value); }
        }
        private bool isShow;
        public bool IsShow
        {
            get { return isShow; }
            set { SetProperty(ref isShow, value); }
        }
        private int currentPos;
        public int CurrentPos
        {
            get { return currentPos; }
            set { SetProperty(ref currentPos, value); }
        }
        double picDescLstHeight;
        public double PicDescLstHeight
        {
            get { return picDescLstHeight; }
            set { SetProperty(ref picDescLstHeight, value); }
        }
        double standardLstHeight;
        public double StandardLstHeight
        {
            get { return standardLstHeight; }
            set { SetProperty(ref standardLstHeight, value); }
        }
        private string _planFinishDate;
        public string PlanFinishDate
        {
            get { return _planFinishDate; }
            set { SetProperty(ref _planFinishDate, value); }
        }
        private string _resultFinishDate;
        public string ResultFinishDate
        {
            get { return _resultFinishDate; }
            set { SetProperty(ref _resultFinishDate, value); }
        }
        private int _lossImageList;
        public int LossImageList
        {
            get { return _lossImageList; }
            set { SetProperty(ref _lossImageList, value); }
        }
        private PicDto _lossPicAttachItem;
        public PicDto LossPicAttachItem
        {
            get
            {
                return _lossPicAttachItem;
            }
            set
            {
                SetProperty(ref _lossPicAttachItem, value);
            }
        }
        private bool _allocateYN;
        public bool AllocateYN
        {
            get { return _allocateYN; }
            set { SetProperty(ref _allocateYN, value); }
        }
        #endregion
        #region Command
        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return cancelCommand
                       ?? (cancelCommand = new RelayCommand(Cancel));
            }
        }
        private RelayCommand commitCommand;
        public RelayCommand CommitCommand
        {
            get
            {
                return commitCommand
                       ?? (commitCommand = new RelayCommand(Commit));
            }
        }
        private RelayCommand _itemTappedCommand;
        public RelayCommand ItemTappedCommand
        {

            get
            {
                return _itemTappedCommand ?? (_itemTappedCommand = new RelayCommand(PreviewImage));
            }
        }
        private RelayCommand _loadImageSuccess;
        public RelayCommand LoadImageSuccess
        {
            get { return _loadImageSuccess ?? (_loadImageSuccess = new RelayCommand(LoadImageS)); }
        }
        private RelayCommand previewCommand;
        public RelayCommand PreviewCommand
        {
            get
            {
                return previewCommand
                       ?? (previewCommand = new RelayCommand(Preview));
            }
        }
        private RelayCommand<PicDto> _previewImageCommand;
        public RelayCommand<PicDto> PreviewImageCommand
        {
            get { return _previewImageCommand ?? (_previewImageCommand = new RelayCommand<PicDto>(Preview)); }
        }
        #endregion
        #region Event
        private void Preview()
        {
            try
            {
                if (PicList != null && PicList.Count > 0)
                {
                    //List<ImagePreviewDto> list = new List<ImagePreviewDto>();
                    //ImagePreviewDto dto;
                    foreach (var item in PicList)
                    {
                        string filename = item.Url.LastIndexOf("/") > 0 ? item.Url.Substring(item.Url.LastIndexOf("/") + 1) : "";
                        if (String.IsNullOrEmpty(filename)) return;
                        if (CrossFilePicker.Current.IsExistFile(filename, "RMMTIMAGEVIEW"))
                        {
                            //已从服务器上缓存的图片。
                            CrossFilePicker.Current.OpenFile(filename, "RMMTIMAGEVIEW");
                        }
                        else
                        {
                            _commonFun.DownLoadFileFromOss(item.Url, filename, "RMMTIMAGEVIEW");
                        }
                    }

                    //Device.BeginInvokeOnMainThread(async () =>
                    //{
                    //    await PopupNavigation.PushAsync(new PreviewImagePage(list, CurrentPos), true);
                    //});
                }
                else
                {
                    _commonFun.AlertLongText("没有可预览的照片");
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ImproveDistributionViewModel");
                return;
            }
        }
        private void Preview(PicDto picDto)
        {
            try
            {
                string url = LossPicAttachItem.Url;

                string filename = url.LastIndexOf("/") > 0 ? url.Substring(url.LastIndexOf("/") + 1) : "";
                if (String.IsNullOrEmpty(filename)) return;
                if (CrossFilePicker.Current.IsExistFile(filename, "RMMTIMAGEVIEW"))
                {
                    //已从服务器上缓存的图片。
                    CrossFilePicker.Current.OpenFile(filename, "RMMTIMAGEVIEW");
                }
                else
                {
                    _commonFun.DownLoadFileFromOss(url, filename, "RMMTIMAGEVIEW");

                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("加载异常，请重试");
            }
        }
        private void LoadImageS()
        {
            CurrentPos = PicDescList.Count - 1;
        }
        private void PreviewImage()
        {
            try
            {
                if (SelectedPicDesc.IsPreview == false)
                {
                    _commonFun.AlertLongText("没有可预览的照片");
                    return;
                }
                //List<ImagePreviewDto> list = new List<ImagePreviewDto>();
                //list.Add(new ImagePreviewDto { Url = SelectedPicDesc.Url });
                //Device.BeginInvokeOnMainThread(async () =>
                //{
                //    await PopupNavigation.PushAsync(new PreviewImagePage(list, 0), true);
                //});
                string filename = SelectedPicDesc.Url.LastIndexOf("/") > 0 ? SelectedPicDesc.Url.Substring(SelectedPicDesc.Url.LastIndexOf("/") + 1) : "";
                if (String.IsNullOrEmpty(filename)) return;
                if (CrossFilePicker.Current.IsExistFile(filename, "RMMTIMAGEVIEW"))
                {
                    //已从服务器上缓存的图片。
                    CrossFilePicker.Current.OpenFile(filename, "RMMTIMAGEVIEW");
                }
                else
                {
                    _commonFun.DownLoadFileFromOss(SelectedPicDesc.Url, filename, "RMMTIMAGEVIEW");
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ImproveDistributionViewModel");
                return;
            }
        }
        private async void GetImproveDistributionDetail(ImprovementMngDto improvementMng)
        {
            try
            {
                string improvementId = improvementMng.ImprovementId.ToString();
                string impResultId = improvementMng.ImpResultId.ToString();
                string tPId = improvementMng.TPId.ToString();
                string itemId = improvementMng.ItemId.ToString();
                if (_commonHelper.IsNetWorkConnected() == true)
                {
                    try
                    {
                        _commonFun.ShowLoading("查询中...");
                        var result = await improveService.GetImpPlanOrResultDetail(improvementId, "0", impResultId, tPId, itemId);
                        if (result.ResultCode == Module.ResultType.Success)
                        {
                            _commonFun.HideLoading();
                            var impAllocateData = CommonHelper.DecodeString<ImpAllocateDto>(result.Body);
                            if (impAllocateData != null)
                            {

                                if (CommonContext.Account.UserType == "S")
                                {
                                    if (impAllocateData.PlanStatus == "A")
                                    {
                                        IsEdit = true;
                                        IsShow = false;
                                    }
                                    else
                                    {
                                        IsEdit = false;
                                        IsShow = true;
                                    }
                                }
                                else
                                {
                                    IsEdit = false;
                                    IsShow = true;
                                }
                                ExecDepartmentName = impAllocateData.ExecDepartmentName;
                                AllocateYN = impAllocateData.AllocateYN;
                                PlanApproal = impAllocateData.PlanApproalYN == true ? "评估师" : "总经理";
                                ResultApproal = impAllocateData.ResultApproalYN == true ? "评估师" : "总经理";
                                StandardList = impAllocateData.StandardList;
                                PicList = impAllocateData.PicList;
                                LossImageList = impAllocateData.PicList.Count * 40;
                                PicDescList = impAllocateData.PicDescList;
                                if (PicDescList != null)
                                {
                                    foreach (var item in PicDescList)
                                    {
                                        item.IsPreview = string.IsNullOrEmpty(item.Url) ? false : true;
                                    }
                                }
                                ImprovementCaption = impAllocateData.ImprovementCaption;
                                LostDescription = impAllocateData.LostDescription;
                                Score = impAllocateData.Score.ToString();
                                PlanFinishDate = impAllocateData.PlanFinishDate;
                                ResultFinishDate = impAllocateData.ResultFinishDate;
                                PicDescLstHeight = 40 * PicDescList.Count + 45;
                                StandardLstHeight = 40 * StandardList.Count + 45;
                            }
                            else
                            {
                                _commonFun.HideLoading();
                                _commonFun.ShowToast("查无数据");
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
                    _commonFun.AlertLongText("网络连接异常");
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ImproveDistributionViewModel");
                return;
            }
        }
        private async void Commit()
        {
            try
            {
                if (AllocateYN == true && Department == null)
                {
                    _commonFun.AlertLongText("请选择责任部门");
                    return;
                }
                if (string.IsNullOrEmpty(ImprovementCaption))
                {
                    _commonFun.AlertLongText("总经理指示为必填");
                    return;
                }
                string tpId = improvementMng.TPId.ToString();
                string itemId = improvementMng.ItemId.ToString();
                string departmentId = Department == null ? null : Department.DId;
                string improvementCaption = ImprovementCaption;
                string lostDescription = LostDescription;
                string inUserId = CommonContext.Account.UserId;
                if (_commonHelper.IsNetWorkConnected() == true)
                {
                    _commonFun.ShowLoading("提交中...");
                    var result = await improveService.SaveImproveDistribution(tpId, itemId, departmentId, improvementCaption, lostDescription, inUserId, AllocateYN);
                    if (result.ResultCode == ResultType.Success)
                    {
                        await Navigation.PopAsync();
                        ImprovementCaption = string.Empty;
                        LostDescription = string.Empty;
                        MessagingCenter.Send<List<RequestParameter>>(paramList, MessageConst.PASS_IMPROVESEARCHCONDITION);
                        MessagingCenter.Send<string>("R", "GetImproveDitstriLst");
                        _commonFun.AlertLongText("提交完毕。 ");
                    }
                    else
                    {
                        _commonFun.HideLoading();
                        _commonFun.AlertLongText("提交失败，请重试。 " + result.Msg);
                    }
                }
                else
                {
                    _commonFun.AlertLongText("网络连接异常。");
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
            }
            finally
            {
                _commonFun.HideLoading();
            }
        }
        private async void Cancel()
        {
            try
            {
                await Navigation.PopAsync();
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ImproveDistributionViewModel");
                return;
            }
        }
        #endregion
        #region Init
        public void Init(ImprovementMngDto m, List<RequestParameter> param = null)
        {
            try
            {
                DepartmentSelect = "选择";
                Department = null;
                if (m != null)
                {
                    improvementMng = m;
                    GetImproveDistributionDetail(m);
                }
                if (param != null && param.Count > 0)
                {
                    paramList = param;
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ImproveDistributionViewModel");
                return;
            }
        }
        #endregion
    }
}
