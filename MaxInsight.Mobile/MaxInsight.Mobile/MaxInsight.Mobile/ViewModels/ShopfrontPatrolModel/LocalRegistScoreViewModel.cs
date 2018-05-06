using MaxInsight.Mobile.Domain;
using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Services.RemoteService;
using MaxInsight.Mobile.Services.Tour;
using PCLStorage;
using Plugin.FilePicker;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.ShopfrontPatrolModel
{
    public class LocalRegistScoreViewModel : ViewModel
    {
        private readonly ITourService _tourService;
        private readonly ICommonFun _commonFun;
        private readonly ILocalScoreService _localScoreService;
        private readonly IRemoteService _remoteService;
        CommonHelper _commonHelper;
        //ScoreCheckResultParam saveScore;
        //ObservableCollection<StandardPic> _deleteList = null;
        public bool _previewTimer { get; set; } = true;
        public bool _initTimer { get; set; } = true;

        public LocalRegistScoreViewModel()
        {
            _tourService = Resolver.Resolve<ITourService>();
            _commonFun = Resolver.Resolve<ICommonFun>();
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _localScoreService = Resolver.Resolve<ILocalScoreService>();
            _remoteService = Resolver.Resolve<IRemoteService>();

            _tapCommand = new Command(ImageTaped);

            MessagingCenter.Subscribe<LocalRegistScorePage>(this, "CheckBoxChanged", (obj) =>
            {
                if (CSList.Any(p => p.IsCheck == true))
                {
                    CurrentScore = "0";
                    CurrentSystem.Score = 0;
                }
                else
                {
                    CurrentScore = "1";
                    CurrentSystem.Score = 1;
                }
            });

            MessagingCenter.Subscribe<StandardPic>(this, "DeleteLossImage", (obj) =>
            {
                //删除失分照片
                DeleteImage(obj);
            });

            MessagingCenter.Subscribe<PictureStandard>(this, "RegistScoreItemTapped", (obj) =>
            {
                UploadStandPic(obj.StandardPicId);
            });

            MessagingCenter.Subscribe<PictureStandard>(this, "PreviewPlanAttechment", (obj) =>
            {
                if (!string.IsNullOrEmpty(obj.Url))
                {
                    PreviewStanderImage(obj.Url);
                }
            });

            MessagingCenter.Subscribe<PictureStandard>(this, "DeletePlanAttechment", (obj) =>
            {
                if (!string.IsNullOrEmpty(obj.Url))
                {
                    DeleteStanderImage(obj);
                }
            });
        }

        #region useless
        int taps = 0;
        private ICommand _tapCommand;

        public ICommand TapCommand
        {
            get { return _tapCommand; }
        }

        private void ImageTaped(object s)
        {
            taps++;
            Debug.WriteLine("parameter: " + s);
        }
        #endregion

        #region properties
        private LoaclItemOfTaskDto _currentSystem;
        public LoaclItemOfTaskDto CurrentSystem
        {
            get
            {
                return _currentSystem;
            }
            set
            {
                SetProperty(ref _currentSystem, value);
            }
        }

        private PictureStandard _currentPictureStand;
        public PictureStandard CurrentPictureStand
        {
            get
            {
                return _currentPictureStand;
            }
            set
            {
                SetProperty(ref _currentPictureStand, value);
            }
        }

        private List<LoaclItemOfTaskDto> _systemList;
        public List<LoaclItemOfTaskDto> SystemList
        {
            get { return _systemList; }
            set { SetProperty(ref _systemList, value); }
        }

        private ObservableCollection<StandardPic> _images;
        public ObservableCollection<StandardPic> Images
        {
            get
            {
                return _images;
            }
            set
            {
                SetProperty(ref _images, value);
            }
        }

        StandardPic _lossPicAttachItem;
        public StandardPic LossPicAttachItem
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

        private bool _isPreview;
        public bool IsPreview
        {
            get
            {
                return _isPreview;
            }
            set { SetProperty(ref _isPreview, value); }
        }

        private int _rowHeight;
        public int RowHeight
        {
            get { return _rowHeight; }
            set { SetProperty(ref _rowHeight, value); }
        }

        private int _picRowHeight;
        public int PicRowHeight
        {
            get { return _picRowHeight; }
            set { SetProperty(ref _picRowHeight, value); }
        }

        private int _lossImageList;
        public int LossImageList
        {
            get { return _lossImageList; }
            set { SetProperty(ref _lossImageList, value); }
        }

        private int _currentPos;
        public int CurrentPos
        {
            get { return _currentPos; }
            set { SetProperty(ref _currentPos, value); }
        }

        private int _currentPage;
        public int CurrentPage
        {
            get { return _currentPage; }
            set { SetProperty(ref _currentPage, value); }
        }

        private string _currentScore;
        public string CurrentScore
        {
            get { return _currentScore; }
            set
            {
                SetProperty(ref _currentScore, value);
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        private bool _isPreEnable;
        public bool IsPreEnable
        {
            get { return _isPreEnable; }
            set { SetProperty(ref _isPreEnable, value); }
        }

        private bool _isNextEnable;
        public bool IsNextEnable
        {
            get { return _isNextEnable; }
            set { SetProperty(ref _isNextEnable, value); }
        }

        private bool _isJumpEnable;
        public bool IsJumpEnable
        {
            get { return _isJumpEnable; }
            set { SetProperty(ref _isJumpEnable, value); }
        }

        private int _jumpPage;
        public int JumpPage
        {
            get { return _jumpPage; }
            set { SetProperty(ref _jumpPage, value); }
        }

        private bool _partOne;
        public bool PartOne
        {
            get { return _partOne; }
            set { SetProperty(ref _partOne, value); }
        }

        private bool _partTwo;
        public bool PartTwo
        {
            get { return _partTwo; }
            set { SetProperty(ref _partTwo, value); }
        }

        private bool _partThree;
        public bool PartThree
        {
            get { return _partThree; }
            set { SetProperty(ref _partThree, value); }
        }
        private ObservableCollection<CheckStandard> _cSList;
        public ObservableCollection<CheckStandard> CSList
        {
            get { return _cSList; }
            set { SetProperty(ref _cSList, value); }
        }
        private ObservableCollection<StandardPic> _sPicList;
        public ObservableCollection<StandardPic> SPicList
        {
            get { return _sPicList; }
            set { SetProperty(ref _sPicList, value); }
        }

        private ObservableCollection<PictureStandard> _pStandardList;
        public ObservableCollection<PictureStandard> PStandardList
        {
            get { return _pStandardList; }
            set { SetProperty(ref _pStandardList, value); }
        }
        #endregion

        #region page init
        public async void Init(List<LoaclItemOfTaskDto> list, LoaclItemOfTaskDto dto)
        {
            try
            {
                SystemList = list;
                CurrentSystem = new LoaclItemOfTaskDto();
                CurrentSystem = dto;

                List<CheckStandard> cSList = await _localScoreService.SearchCheckStandard(CurrentSystem.TPId, CurrentSystem.TIId.ToString());
                CSList = new ObservableCollection<CheckStandard>();
                foreach (var item in cSList)
                {
                    CSList.Add(item);
                }

                List<StandardPic> sPicList = await _localScoreService.SearchStandardPic(CurrentSystem.TPId, CurrentSystem.TIId);
                SPicList = new ObservableCollection<StandardPic>();
                foreach (var item in sPicList)
                {
                    SPicList.Add(item);
                }
                PStandardList = new ObservableCollection<PictureStandard>();
                List<PictureStandard> pictureStandardLst = await _localScoreService.SearchPictureStandard(CurrentSystem.TPId, CurrentSystem.TIId);
                foreach (var item in pictureStandardLst)
                {
                    PStandardList.Add(item);
                }

                IsPreview = true;
                int index = list.IndexOf(list.FirstOrDefault(p => p.SeqNo == dto.SeqNo));// list.IndexOf(dto) + 1;
                CurrentPage = index + 1;
                //CurrentPage = dto.SeqNo;
                JumpPage = 1;
                RowHeight = CSList.Count * 50;
                PicRowHeight = PStandardList.Count * 50;
                LossImageList = SPicList.Count * 50;
                IsLoading = false;
                CurrentScore = CurrentSystem.Score == -1 ? "" : CurrentSystem.Score.ToString();
                if (SystemList != null && SystemList.Count > 1)
                {
                    if (index == 0)
                    {
                        IsPreEnable = false;
                    }
                    else
                    {
                        IsPreEnable = true;
                    }
                }
                else
                {
                    IsPreEnable = false;
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region command
        private RelayCommand _uploadImagCommand;
        public RelayCommand UploadImagCommand
        {
            get
            {
                return _uploadImagCommand ?? (_uploadImagCommand = new RelayCommand(UploadImage));
            }
        }

        private RelayCommand<StandardPic> _previewImageCommand;
        public RelayCommand<StandardPic> PreviewImageCommand
        {
            get { return _previewImageCommand ?? (_previewImageCommand = new RelayCommand<StandardPic>(Preview)); }
        }

        private RelayCommand<StandardPic> _deleteImageCommand;
        public RelayCommand<StandardPic> DeleteImageCommand
        {
            get { return _deleteImageCommand ?? (_deleteImageCommand = new RelayCommand<StandardPic>(DeleteImage)); }
        }

        private RelayCommand _preCommand;
        public RelayCommand PreCommand
        {
            get { return _preCommand ?? (_preCommand = new RelayCommand(PrePage)); }
        }

        private RelayCommand _nextCommand;
        public RelayCommand NextCommand
        {
            get { return _nextCommand ?? (_nextCommand = new RelayCommand(NextPage)); }
        }

        private RelayCommand _jumpCommand;
        public RelayCommand JumpCommand
        {
            get { return _jumpCommand ?? (_jumpCommand = new RelayCommand(JumpToPage)); }
        }

        private RelayCommand _saveRegistCoreCommand;
        public RelayCommand SaveRegistCoreCommand
        {
            get { return _saveRegistCoreCommand ?? (_saveRegistCoreCommand = new RelayCommand(SaveRegistCore)); }
        }

        private RelayCommand _loadImageSuccess;
        public RelayCommand LoadImageSuccess
        {
            get { return _loadImageSuccess ?? (_loadImageSuccess = new RelayCommand(LoadImageS)); }
        }

        private RelayCommand _showTowOrHideCommand;
        public RelayCommand ShowTowOrHideCommand
        {
            get { return _showTowOrHideCommand ?? (_showTowOrHideCommand = new RelayCommand(ShowTowOrHide)); }
        }

        private void ShowTowOrHide()
        {
            if (PartTwo)
            {
                PartTwo = false;
            }
            else
            {
                PartTwo = true;
            }
        }

        private RelayCommand _ingnorCommand;
        public RelayCommand IngnorCommand
        {
            get { return _ingnorCommand ?? (_ingnorCommand = new RelayCommand(Ingnor)); }
        }

        #endregion

        #region action
        private async void UploadImage()
        {
            var imageUrl = "";
            var imageType = "L";
            var imageName = "";
            var filePath = "";
            MediaFile file = null;

            await CrossMedia.Current.Initialize();

            var action = await _commonFun.ShowActionSheet("从相册", "拍照");

            IsLoading = true;
            if (action == "从相册")
            {
                MessagingCenter.Send<CommonMessage>(new CommonMessage() { TaskID = "-1" }, "LocalResetTaskID");
                file = await CrossMedia.Current.PickPhotoAsync();
            }
            else if (action == "拍照")
            {
                MessagingCenter.Send<CommonMessage>(new CommonMessage() { TaskID = "-1" }, "LocalResetTaskID");
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    return;
                }
                file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "RMMT",
                    Name = DateTime.Now.ToString("yy-MM-dd").Replace("-", "")
                                   + DateTime.Now.ToString("HH:mm:ss").Replace(":", "")
                });
            }

            if (file == null)
            {
                MessagingCenter.Send<CommonMessage>(new CommonMessage() { TaskID = "0" }, "LocalResetTaskID");
                IsLoading = false;
                return;
            }

            imageName = file.Path.Substring(file.Path.LastIndexOf('/') + 1);
            filePath = file.Path;

            //upload file to server

            try
            {
                string result = _commonFun.SaveAttachLocal(file.GetStream(), file.Path);

                file.Dispose();

                if (!string.IsNullOrWhiteSpace(result))
                {
                    imageUrl = result;

                    var addDto = new StandardPic()
                    {
                        StandardPicId = 0,
                        PicName = imageName,
                        PicType = imageType, //L,G 失分照片／得分照片
                        TIId = CurrentSystem.TIId,
                        Url = imageUrl,
                        FilePath = filePath,
                        PicId = SPicList.Count == 0 ? 1 : SPicList.Max(p => p.PicId) + 1
                    };

                    SPicList.Add(addDto);
                    //ParamSPicList.Add(addDto);

                    LossImageList = SPicList.Count * 50;
                }
                else
                {
                    _commonFun.AlertLongText("上传失败");
                    return;
                }


            }
            catch (OperationCanceledException)
            {
                _commonFun.AlertLongText("上传超时,请重试");
                return;
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("上传异常,请重试");
                return;
            }
            finally
            {
                IsLoading = false;
                MessagingCenter.Send<CommonMessage>(new CommonMessage() { TaskID = "0" }, "LocalResetTaskID");
            }

        }

        void Preview(StandardPic dto)
        {

            //List<ImagePreviewDto> list = new List<ImagePreviewDto>() { new ImagePreviewDto() {
            //        Url = LossPicAttachItem.Url
            //    }};


            //Device.BeginInvokeOnMainThread(async () =>
            //{
            //    if (_previewTimer)
            //    {
            //        _previewTimer = false;
            //        await PopupNavigation.PushAsync(new PreviewImagePage(list, 0), true);
            //        Device.StartTimer(TimeSpan.FromSeconds(3), () =>
            //        {
            //            _previewTimer = true;
            //            return false;
            //        });
            //    }
            //});

            try
            {
                string url = LossPicAttachItem.Url;

                string filename = url.LastIndexOf("/") > 0 ? url.Substring(url.LastIndexOf("/") + 1) : "";
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
                    if (App.SysOS == "IOS")
                    {
                        _commonFun.DownLoadFileFromOss(url, filename, "RMMTIMAGEVIEW");
                    }
                    else
                    {
                        if (_commonHelper.IsNetWorkConnected() == true)
                        {
                            _commonFun.DownLoadFileFromOss(url, filename, "RMMTIMAGEVIEW");
                        }
                        else
                        {
                            _commonFun.AlertLongText("请在连网时预览");
                            return;
                        }
                    }
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("加载异常，请重试");
            }

        }

        private void DeleteImage(StandardPic item)
        {
            var index = SPicList.IndexOf(item);

            if (SPicList != null && SPicList.Count > 0
                && index > -1)
            {
                SPicList.Remove(item);
                //ParamSPicList.FirstOrDefault(p => p.PicId == item.PicId).Url = string.Empty;

                LossImageList = SPicList.Count * 50;
            }
        }

        private void DeleteStanderImage(PictureStandard item)
        {
            var index = PStandardList.IndexOf(item);

            if (index > -1)
            {
                PStandardList.Insert(index, new PictureStandard()
                {
                    StandardPicId = item.StandardPicId,
                    StandardPicName = item.StandardPicName,
                    TIId = item.TIId,
                    TPId = item.TPId,
                    SeqNo = item.SeqNo,
                    Url = "",
                    SuccessImage = "",
                    Id = item.StrPicId
                });
                PStandardList.Remove(item);
            }
        }

        private void PreviewStanderImage(string url)
        {
            //List<ImagePreviewDto> list = new List<ImagePreviewDto>();
            //ImagePreviewDto dto;

            //dto = new ImagePreviewDto();
            //dto.Url = url;
            //list.Add(dto);

            //Device.BeginInvokeOnMainThread(async () =>
            //{
            //    await PopupNavigation.PushAsync(new PreviewImagePage(list, CurrentPos), true);
            //});

            try
            {

                string filename = url.LastIndexOf("/") > 0 ? url.Substring(url.LastIndexOf("/") + 1) : "";
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
                    if (App.SysOS == "IOS")
                    {
                        _commonFun.DownLoadFileFromOss(url, filename, "RMMTIMAGEVIEW");
                    }
                    else
                    {
                        if (_commonHelper.IsNetWorkConnected() == true)
                        {
                            _commonFun.DownLoadFileFromOss(url, filename, "RMMTIMAGEVIEW");
                        }
                        else
                        {
                            _commonFun.AlertLongText("请在连网时预览");
                            return;
                        }
                    }
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("加载异常，请重试");
            }

        }

        private void PrePage()
        {
            if (CurrentPage <= 1)
            {
                return;
            }

            int _store = -1;
            if (!int.TryParse(CurrentScore, out _store))
            {
                _commonFun.AlertLongText("分数请输入数字");
                return;
            }
            CurrentSystem.Score = _store;
            CurrentPage--;
            UpdateBtnState();
        }

        private void NextPage()
        {
            if (CurrentPage > SystemList.Count)
            {
                return;
            }
            if (CurrentSystem.TIId.Length == 36)
            {
                CurrentSystem.GRUD = "N";
            }
            else
            {
                CurrentSystem.GRUD = "I";
            }

            int _store = -1;
            if (!int.TryParse(CurrentScore, out _store))
            {
                _commonFun.AlertLongText("分数请输入数字");
                return;
            }
            //DateTime st;
            //DateTime et;
            //if (DateTime.TryParse(Convert.ToDateTime(CurrentSystem.PlanFinishDate).ToString("yyyy-MM-dd") + " 23:59:59", out st) && st < DateTime.Now)
            //{
            //    _commonFun.AlertLongText("计划完成日期不能小于今天");
            //    return;
            //}
            //if (DateTime.TryParse(Convert.ToDateTime(CurrentSystem.ResultFinishDate).ToString("yyyy-MM-dd") + " 23:59:59", out et) && et < DateTime.Now)
            //{
            //    _commonFun.AlertLongText("结果完成日期不能小于今天");
            //    return;
            //}
            CurrentSystem.Score = _store;
            SaveDataToLocal();// 数据保存到本地DB
            if (CurrentPage < SystemList.Count)
            {
                CurrentPage++;
                UpdateBtnState();
            }
            else
            {
                _commonFun.AlertLongText("已经是最后一条");
            }

        }

        private void JumpToPage()
        {
            int _store = -1;
            if (!int.TryParse(CurrentScore, out _store))
            {
                _commonFun.AlertLongText("分数请输入数字");
                return;
            }
            CurrentSystem.Score = _store;

            if (JumpPage == 0 || JumpPage > SystemList.Count)
            {
                string message = "请输入1~" + SystemList.Count.ToString();
                _commonFun.AlertLongText(message);
                return;
            }
            CurrentPage = JumpPage;
            UpdateBtnState();
        }

        private async void SaveRegistCore()
        {
            if (_commonHelper.IsNetWorkConnected() == true)
            {
                try
                {
                    string size = _commonFun.GetFilesSizeOfUpload();
                    if (await _commonFun.Confirm("上传文件大小为:" + size + "以上, 建议在Wifi环境下进行。确定要上传吗？"))
                    {
                        try
                        {
                            //_commonFun.ShowLoading("上传中...");
                            await _commonFun.UploadLocalFileToServer();
                            await _remoteService.UploadScoreInfo(0);
                            _commonFun.HideLoading();
                            _commonFun.AlertLongText("上传完毕");
                        }
                        catch (Exception)
                        {
                            _commonFun.AlertLongText("上传异常，请重试");
                            _commonFun.HideLoading();
                        }
                        finally
                        {
                            _commonFun.HideLoading();
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    _commonFun.AlertLongText("保存超时,请重试");
                }
                catch (Exception)
                {
                    _commonFun.AlertLongText("上传异常，请重试");
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

        private void LoadImageS()
        {
            CurrentPos = SPicList.Count - 1;
        }

        private void Ingnor()
        {
            CurrentSystem.Score = 99;
            CurrentScore = "99";
        }
        #endregion

        #region pravite method
        public async Task SaveToLocal()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder folder = await rootFolder.CreateFolderAsync("RMMT",
                CreationCollisionOption.OpenIfExists);
            IFile file = await folder.CreateFileAsync("", CreationCollisionOption.ReplaceExisting);
            await file.WriteAllTextAsync("42");
        }

        private async void UpdateBtnState()
        {

            CurrentSystem = SystemList[CurrentPage - 1];

            List<CheckStandard> cSList = await _localScoreService.SearchCheckStandard(CurrentSystem.TPId, CurrentSystem.TIId);
            CSList = new ObservableCollection<CheckStandard>();
            foreach (var item in cSList)
            {
                CSList.Add(item);
            }

            List<StandardPic> sPicList = await _localScoreService.SearchStandardPic(CurrentSystem.TPId, CurrentSystem.TIId);
            SPicList = new ObservableCollection<StandardPic>();
            foreach (var item in sPicList)
            {
                SPicList.Add(item);
            }
            //ParamSPicList = new ObservableCollection<StandardPic>();
            //foreach (var spi in SPicList)
            //{
            //    ParamSPicList.Add(spi);
            //}
            PStandardList = new ObservableCollection<PictureStandard>();
            List<PictureStandard> pictureStandardLst = await _localScoreService.SearchPictureStandard(CurrentSystem.TPId, CurrentSystem.TIId);
            foreach (var item in pictureStandardLst)
            {
                PStandardList.Add(item);
            }

            CurrentScore = CurrentSystem.Score == -1 ? "" : CurrentSystem.Score.ToString();
            RowHeight = CSList.Count * 50;
            PicRowHeight = PStandardList.Count * 50;
            LossImageList = SPicList.Count * 50;

            if (CurrentPage == 1)
            {
                IsPreEnable = false;
            }
            else
            {
                IsPreEnable = true;
            }
        }

        private async void UploadStandPic(int picId)
        {
            var imageUrl = "";
            var imageType = "G";
            var imageName = "";
            var filePath = "";
            MediaFile file = null;

            await CrossMedia.Current.Initialize();

            var action = await _commonFun.ShowActionSheet("从相册", "拍照");

            IsLoading = true;
            if (action == "从相册")
            {
                MessagingCenter.Send<CommonMessage>(new CommonMessage() { TaskID = "-1" }, "LocalResetTaskID");
                file = await CrossMedia.Current.PickPhotoAsync();
            }
            else if (action == "拍照")
            {
                MessagingCenter.Send<CommonMessage>(new CommonMessage() { TaskID = "-1" }, "LocalResetTaskID");
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    return;
                }
                file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "RMMT",
                    Name = DateTime.Now.ToString("yy-MM-dd").Replace("-", "")
                                   + DateTime.Now.ToString("HH:mm:ss").Replace(":", "")
                });
            }

            if (file == null)
            {
                MessagingCenter.Send<CommonMessage>(new CommonMessage() { TaskID = "0" }, "LocalResetTaskID");
                IsLoading = false;
                return;
            }

            imageName = file.Path.Substring(file.Path.LastIndexOf('/') + 1);
            filePath = file.Path;

            //upload file to server

            try
            {
                string result = _commonFun.SaveAttachLocal(file.GetStream(), file.Path);

                file.Dispose();

                if (!string.IsNullOrWhiteSpace(result))
                {
                    imageUrl = result;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var item = PStandardList.FirstOrDefault(p => p.StandardPicId == picId);
                        var index = PStandardList.IndexOf(item);

                        if (index > -1)
                        {
                            PStandardList.Insert(index, new PictureStandard()
                            {
                                StandardPicId = item.StandardPicId,
                                StandardPicName = item.StandardPicName,
                                TIId = item.TIId,
                                TPId = item.TPId,
                                SeqNo = item.SeqNo,
                                Url = imageUrl,
                                SuccessImage = "icon_success"

                            });
                            PStandardList.Remove(item);
                        }
                    });
                }
                else
                {
                    _commonFun.AlertLongText("上传失败");
                    return;
                }

            }
            catch (OperationCanceledException)
            {
                _commonFun.AlertLongText("上传超时,请重试");
                return;
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("上传异常,请重试");
                return;
            }
            finally
            {
                IsLoading = false;
                MessagingCenter.Send<CommonMessage>(new CommonMessage() { TaskID = "0" }, "LocalResetTaskID");
            }
        }

        private void SaveDataToLocal()
        {
            string passYN = "";
            if (CurrentSystem.Score == 1)
            {
                passYN = "1";
            }
            else if (CurrentSystem.Score == 99)
            {
                passYN = "Q";
            }
            else
            {
                passYN = "0";
            }
            Score score = new Score()
            {
                TPId = CurrentSystem.TPId.ToString(),
                ItemId = CurrentSystem.TIId.ToString(),
                Scoreval = CurrentSystem.Score,
                PlanApproalYN = CurrentSystem.PlanApproalYN,
                PlanFinishDate = CurrentSystem.PlanFinishDate,
                ResultApproalYN = CurrentSystem.ResultApproalYN,
                ResultFinishDate = CurrentSystem.ResultFinishDate,
                PassYN = passYN,
                Remarks = CurrentSystem.Remarks,
                InUserId = Convert.ToInt32(CommonContext.Account.UserId),
                InDateTime = DateTime.Now,
                GRUD = CurrentSystem.GRUD,
                Id = string.IsNullOrEmpty(CurrentSystem.StrScoreId) ? Guid.NewGuid().ToString() : CurrentSystem.StrScoreId
            };

            List<CheckResult> csList = new List<CheckResult>();
            foreach (var item in CSList)
            {
                CheckResult checkDto = new CheckResult();
                checkDto.CSId = item.CSID;
                checkDto.Result = item.IsCheck;
                checkDto.TIId = item.TIId;
                checkDto.TPId = item.TPId;
                checkDto.GRUD = CurrentSystem.GRUD;
                checkDto.Id = string.IsNullOrEmpty(item.StrCRId) ? Guid.NewGuid().ToString() : item.StrCRId;
                csList.Add(checkDto);
            }
            List<Domain.StandardPic> sPicList = new List<Domain.StandardPic>();
            foreach (var item2 in SPicList)
            {
                Domain.StandardPic standDto = new Domain.StandardPic();
                standDto.TIId = item2.TIId;
                standDto.TPId = CurrentSystem.TPId;
                standDto.Url = item2.Url;
                standDto.PicName = item2.PicName;
                standDto.Type = "L";
                standDto.GRUD = CurrentSystem.GRUD;
                standDto.Id = string.IsNullOrEmpty(item2.StrPicId) ? Guid.NewGuid().ToString() : item2.StrPicId;
                sPicList.Add(standDto);
            }

            List<Domain.StandardPic> pStandardList = new List<Domain.StandardPic>();
            foreach (var item3 in PStandardList)
            {
                Domain.StandardPic standPicDto = new Domain.StandardPic();
                standPicDto.TIId = item3.TIId;
                standPicDto.TPId = item3.TPId;
                standPicDto.Url = item3.Url;
                standPicDto.Type = "G";
                standPicDto.PSId = item3.StandardPicId;
                standPicDto.PicName = item3.StandardPicName;
                standPicDto.Id = string.IsNullOrEmpty(item3.StrPicId) ? Guid.NewGuid().ToString() : item3.StrPicId;
                standPicDto.GRUD = CurrentSystem.GRUD;

                pStandardList.Add(standPicDto);
            }

            _localScoreService.SaveDataToLocal(score, csList, sPicList, pStandardList);

            RowHeight = CSList.Count * 50;
            PicRowHeight = PStandardList.Count * 50;
            LossImageList = SPicList.Count * 50;
        }

        #endregion
    }
}
