using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto.Case;
using MaxInsight.Mobile.Services.CaseService;
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

namespace MaxInsight.Mobile.ViewModels.Cases
{
    public class CaseRegViewModel : ViewModel
    {
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        private MediaFile _mediaFile;
        private readonly ITourService _tourService;
        ICaseService _caseService;

        private Dictionary<string, string> _dIcCaseType = new Dictionary<string, string>();
        List<RequestParameter> paramList = new List<RequestParameter>();

        #region Constructor
        public CaseRegViewModel()
        {
            _tourService = Resolver.Resolve<ITourService>();
            _commonFun = Resolver.Resolve<ICommonFun>();
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _caseService = Resolver.Resolve<ICaseService>();
            IsLoading = false;
            CaseType = "请选择";

            MessagingCenter.Unsubscribe<List<RequestParameter>>(this, MessageConst.CASEATTACH_DELETE);

            MessagingCenter.Subscribe<string>(
            this,
            MessageConst.CASEATTACH_DELETE,
            (SeqNo) =>
            {
                DeleteCaseAttach(SeqNo);
            });

            MessagingCenter.Subscribe<List<RequestParameter>>(this, "GetCaseInfoDetail", (param) =>
            {
                if (param != null && param.Count > 0)
                {
                    paramList = param;
                    string caseId = param.Find(p => p.Name == "caseId").Value;
                    GetCaseInfoDetail(caseId);
                }

            });

            MessagingCenter.Subscribe<string>(this, "InitCaseRegPage", (param) =>
            {
                Init(0);

            });

            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await _caseService.GetTypeFromHiddenCode("12");
                if (null != result && result.ResultCode == Module.ResultType.Success)
                {
                    var caseTypeList = JsonConvert.DeserializeObject<List<NameValueObject>>(result.Body);

                    if (caseTypeList != null && caseTypeList.Count > 0)
                    {
                        foreach (var item in caseTypeList)
                        {
                            _dIcCaseType.Add(item.Value, item.Name);
                        }
                    }
                    else
                    {

                    }
                }

            });
        }
        #endregion

        #region properties
        public int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                SetProperty(ref _id, value);
            }
        }


        public string _caseNo;
        public string CaseNo
        {
            get
            {
                return _caseNo;
            }
            set
            {
                SetProperty(ref _caseNo, value);
            }
        }

        public string _caseType;
        public string CaseType
        {
            get
            {
                return _caseType;
            }
            set
            {
                SetProperty(ref _caseType, value);
            }
        }

        public string _caseTypeCode;
        public string CaseTypeCode
        {
            get
            {
                return _caseTypeCode == null || _caseTypeCode == "" ? "0" : _caseTypeCode;
            }
            set
            {
                SetProperty(ref _caseTypeCode, value);
            }
        }

        public string _caseTitle;
        public string CaseTitle
        {
            get
            {
                return _caseTitle;
            }
            set
            {
                SetProperty(ref _caseTitle, value);
            }
        }

        public string _casePoint;
        public string CasePoint
        {
            get
            {
                return _casePoint;
            }
            set
            {
                SetProperty(ref _casePoint, value);
            }
        }

        public string _lossRemark;
        public string LossRemark
        {
            get
            {
                return _lossRemark;
            }
            set
            {
                SetProperty(ref _lossRemark, value);
            }
        }

        public string _improveRemark;
        public string ImproveRemark
        {
            get
            {
                return _improveRemark;
            }
            set
            {
                SetProperty(ref _improveRemark, value);
            }
        }

        public List<AttachDto> AttachList = new List<AttachDto>();


        public List<AttachDto> _caseAttachList;
        public List<AttachDto> CaseAttachList
        {
            get { return _caseAttachList; }
            set { SetProperty(ref _caseAttachList, value); }
        }

        public AttachDto _caseAttachItem;
        public AttachDto CaseAttachItem
        {
            get { return _caseAttachItem; }
            set { SetProperty(ref _caseAttachItem, value); }
        }

        private bool _visibleYN;
        public bool VisibleYN
        {
            get
            {
                if (CommonContext.Account.UserType == "R" || CommonContext.Account.UserType == "Z" || CommonContext.Account.UserType == "A")
                {
                    _visibleYN = true;
                }
                else
                {
                    _visibleYN = false;
                }
                return _visibleYN;
            }
            set { SetProperty(ref _visibleYN, value); }
        }

        double _lstRowHeight = 32;
        double _lstHeight;
        public double LstHeight
        {
            get { return _lstHeight; }
            set { SetProperty(ref _lstHeight, value); }
        }

        private bool _deleteBtnVisible;
        public bool DeleteBtnVisible
        {
            get { return _deleteBtnVisible; }
            set { SetProperty(ref _deleteBtnVisible, value); }
        }
        public bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }
        #endregion

        #region command
        private RelayCommand _saveCasesCommand;
        public RelayCommand SaveCasesCommand
        {
            get
            {
                return _saveCasesCommand ??

                (_saveCasesCommand = new RelayCommand(SaveCases));
            }
        }
        private RelayCommand _deleteCasesCommand;
        public RelayCommand DeleteCasesCommand
        {
            get
            {
                return _deleteCasesCommand ??
                (_deleteCasesCommand = new RelayCommand(DeleteCase));
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

        public Command GotoCaseSearchPageCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        await Navigation.PushAsync<CaseSearchViewModel>();
                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }

        public Command OpenCaseTypeCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        var action = await _commonFun.ShowActionSheetAny(_dIcCaseType.Values.ToArray<string>());
                        if (_dIcCaseType.ContainsValue(action))
                        {
                            CaseType = action;
                            CaseTypeCode = _dIcCaseType.FirstOrDefault(q => q.Value == action).Key;
                        }
                    }
                    catch (Exception)
                    {
                    }
                });
            }


        }

        #endregion

        #region method
        private async void SaveCases()
        {
            try
            {


                if (CaseTypeCode == "0")
                {
                    _commonFun.AlertLongText("请选择案例类型");
                    return;
                }
                else if (string.IsNullOrEmpty(CaseTitle.Trim()))
                {
                    _commonFun.AlertLongText("请填写案例标题");
                    return;
                }
                else if (string.IsNullOrEmpty(CasePoint.Trim()))
                {
                    _commonFun.AlertLongText("请填写案例要点");
                    return;
                }
                else if (string.IsNullOrEmpty(LossRemark.Trim()))
                {
                    _commonFun.AlertLongText("请填写失分说明");
                    return;
                }
                else if (string.IsNullOrEmpty(ImproveRemark.Trim()))
                {
                    _commonFun.AlertLongText("请填写改善措施");
                    return;
                }
                CasesParamDto caseDto = new CasesParamDto()
                {
                    Id = Id,
                    CaseType = CaseTypeCode,
                    CasePoint = CasePoint,
                    LossRemark = LossRemark,
                    ImproveRemark = ImproveRemark,
                    InUserId = int.Parse(CommonContext.Account.UserId),
                    CaseTitle = CaseTitle,
                    AttachList = CaseAttachList,
                };

                _commonFun.ShowLoading("保存中...");
                var result = await _caseService.InsertOrUpdateCasesInfo(caseDto);
                if (result != null)
                {
                    if (result.ResultCode == ResultType.Success)
                    {
                        //跳转或刷新CaseIndexViewModel
                        await Navigation.PopAsync();
                        MessagingCenter.Send<string>("", MessageConst.CASESAVESUCCESS);
                        _commonFun.AlertLongText("保存成功");
                    }
                    else
                    {
                        _commonFun.AlertLongText("保存失败,请重试");
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
                _commonFun.AlertLongText("保存失败,请重试");
            }
            finally
            {
                _commonFun.HideLoading();
            }
        }

        private async void DeleteCase()
        {
            try
            {
                CasesDelParamDto delDto = new CasesDelParamDto();
                delDto.InUserId = int.Parse(CommonContext.Account.UserId);
                delDto.IdList = new List<IdParamDto>();
                delDto.IdList.Add(new IdParamDto() { Id = Id });

                _commonFun.ShowLoading("删除中...");
                var result = await _caseService.DeleteCasesInfo(delDto);
                if (result != null)
                {
                    if (result.ResultCode == ResultType.Success)
                    {
                        //跳转或刷新CaseIndexViewModel                        
                        await Navigation.PopAsync();
                        MessagingCenter.Send<string>("", MessageConst.CASESAVESUCCESS);
                        MessagingCenter.Send<string>("", "CaseSearchResultList");
                        _commonFun.AlertLongText("保存成功");
                    }
                    else
                    {
                        //_commonFun.AlertLongText(result.Msg);
                    }
                }
                else
                {
                    _commonFun.AlertLongText("删除时服务出现错误,,请重试");
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
                _commonFun.AlertLongText("删除失败,请重试");
            }
            finally
            {
                _commonFun.HideLoading();
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
            else if (fileType == "F")
            {
                try
                {

                    FileData filedata = new FileData();
                    filedata = await CrossFilePicker.Current.PickFile();
                    if (filedata == null)
                    {
                        return;
                    }
                    string filename = filedata.FileName;
                    byte[] file = filedata.DataArray;
                    _stream = new MemoryStream(file);
                    _path = filename;
                }
                catch (Exception e)
                {
                }
            }
            if (_mediaFile != null)
            {
                _stream = _mediaFile.GetStream();
                _path = _mediaFile.Path;
            }
            if (_stream == null)
            {
                _commonFun.HideLoading();
                return;
            }

            if (_commonHelper.IsNetWorkConnected() == true)
            {
                try
                {
                    IsLoading = true;
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
                        return;
                    }
                }
                catch (OperationCanceledException)
                {
                    _commonFun.AlertLongText("请求超时。");
                    return;
                }
                catch (Exception)
                {
                    _commonFun.AlertLongText("上传失败，请重试。");
                    return;
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
                return;
            }
        }

        private async void GetCaseInfoDetail(string caseId)
        {
            if (_commonHelper.IsNetWorkConnected() == true)
            {
                try
                {
                    _commonFun.ShowLoading("查询中...");
                    // TO-DO
                    var result = await _caseService.GetCaseDetailInfo(caseId);
                    if (result.ResultCode == Module.ResultType.Success)
                    {

                        CasesInfoDto caseInfoDto = CommonHelper.DecodeString<CasesInfoDto>(result.Body);
                        if (caseInfoDto != null)
                        {
                            Id = caseInfoDto.Id;
                            CaseNo = caseInfoDto.CaseNo;
                            CaseTypeCode = caseInfoDto.CaseType;
                            CaseType = caseInfoDto.CaseTypeName;
                            CaseTitle = caseInfoDto.CaseTitle;
                            CasePoint = caseInfoDto.CasePoint;
                            LossRemark = caseInfoDto.LossRemark;
                            ImproveRemark = caseInfoDto.ImproveRemark;
                            AttachList = caseInfoDto.AttachList;
                            CaseAttachList = caseInfoDto.AttachList;
                            LstHeight = CaseAttachList.Count * _lstRowHeight;
                            _commonFun.HideLoading();
                        }
                        else
                        {
                            _commonFun.HideLoading();
                            _commonFun.ShowToast("没有数据");
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
                    _commonFun.AlertLongText("查询失败，请重试。");
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

        public void Init(int caseId)
        {
            if (caseId == 0)
            {
                Id = 0;
                CaseNo = string.Empty;
                CaseTypeCode = string.Empty;
                CaseType = "请选择";
                CaseTitle = string.Empty;
                CasePoint = string.Empty;
                LossRemark = string.Empty;
                ImproveRemark = string.Empty;
                AttachList = new List<AttachDto>();
                CaseAttachList = new List<AttachDto>();
                LstHeight = CaseAttachList.Count * _lstRowHeight;
                DeleteBtnVisible = false;//登记时，删除按钮不显示

            }
            else
            {
                if (VisibleYN)
                {
                    DeleteBtnVisible = true;
                }
                GetCaseInfoDetail(caseId.ToString());
            }
        }



        #endregion

        #region event
        public void DeleteCaseAttach(string SeqNo)
        {
            AttachList.Remove(AttachList.Find(item => item.SeqNo == int.Parse(SeqNo)));
            List<AttachDto> newList = new List<AttachDto>();
            newList.AddRange(AttachList);
            int i = 1;
            newList.Select(c => { c.SeqNo = i++; return c; }).ToList();
            CaseAttachList = newList;
            LstHeight = CaseAttachList.Count * _lstRowHeight;
        }
        private void PreviewImage()
        {
            try
            {
                string url = CaseAttachItem.Url.ToUpper();
                if (url.EndsWith(".JPG") || url.EndsWith(".BMP") || url.EndsWith(".GIF") ||
                    url.EndsWith(".JPEG") || url.EndsWith(".PNG") || url.EndsWith(".SVG"))
                {
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
                _commonFun.AlertLongText("加载失败，请重试");
            }
        }
        #endregion
    }
}
