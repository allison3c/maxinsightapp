using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Services.Tour;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;
using Plugin.FilePicker;

namespace MaxInsight.Mobile
{
    public class ViewRegistScoreViewModel : ViewModel
    {
        private readonly ITourService _tourService;
        private readonly ICommonFun _commonFun;
        CommonHelper _commonHelper;
        ScoreCheckResultParam saveScore;
        //ObservableCollection<StandardPic> _deleteList = null;
        public bool _previewTimer { get; set; } = true;
        public bool _initTimer { get; set; } = true;

        public ViewRegistScoreViewModel()
        {
            try
            {
                _tourService = Resolver.Resolve<ITourService>();
                _commonFun = Resolver.Resolve<ICommonFun>();
                _commonHelper = Resolver.Resolve<CommonHelper>();

                _tapCommand = new Command(ImageTaped);

                MessagingCenter.Subscribe<List<ItemOfTaskDto>>(this, "PassSystemList", (obj) =>
                {
                    //if (null != obj)
                    //{
                    //	Init(obj, obj.FirstOrDefault().CurrentIndex);
                    //}
                });

                //MessagingCenter.Subscribe<RegistScorePage>(this, "InitPage", (obj) =>
                //{
                //    if (null != SystemList && SystemList.Count > 0 && CurrentSystem != null)
                //    {
                //        if (_initTimer)
                //        {
                //            _initTimer = false;
                //            Init(SystemList, CurrentSystem);
                //        }
                //    }
                //});

                MessagingCenter.Subscribe<RegistScorePage>(this, "CheckBoxChanged", (obj) =>
                {
                    if (CurrentSystem.CSList.Any(p => p.IsCheck == true))
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

                //MessagingCenter.Subscribe<StandardPic>(this, "PreviewLossImage", (obj) =>
                //{
                //	if (_previewTimer)
                //	{
                //		_previewTimer = false;
                //		//浏览失分照片
                //		Preview(obj);

                //		Device.StartTimer(TimeSpan.FromSeconds(2), () => {
                //			_previewTimer = true;
                //			return true;
                //		});
                //	}
                //});

                MessagingCenter.Subscribe<StandardPic>(this, "DeleteLossImage", (obj) =>
                {
                    //删除失分照片
                    DeleteImage(obj);
                });

                MessagingCenter.Subscribe<PictureStandard>(this, "ViewRegistScoreItemTapped", (obj) =>
                {
                    UploadStandPic(obj.StandardPicId);
                });

                MessagingCenter.Subscribe<PictureStandard>(this, "ViewPreviewPlanAttechment", (obj) =>
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
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ViewRegistScoreViewModel");
                return;
            }
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
        }
        #endregion

        #region properties
        private ItemOfTaskDto _currentSystem;
        public ItemOfTaskDto CurrentSystem
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

        private List<ItemOfTaskDto> _systemList;
        public List<ItemOfTaskDto> SystemList
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
                //if (CurrentSystem != null)
                //{
                //	CurrentSystem.Score = value;
                //}
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
        #endregion

        #region page init
        public void Init(List<ItemOfTaskDto> list, ItemOfTaskDto dto)
        {
            try
            {
                SystemList = list;
                CurrentSystem = dto;

                CurrentSystem.ParamSPicList = new ObservableCollection<StandardPic>();

                //_deleteList = new ObservableCollection<StandardPic>();
                foreach (var spi in CurrentSystem.SPicList)
                {
                    CurrentSystem.ParamSPicList.Add(spi);
                }

                IsPreview = true;
                int index = list.IndexOf(list.FirstOrDefault(p => p.SeqNo == dto.SeqNo));// list.IndexOf(dto) + 1;
                CurrentPage = index + 1;
                JumpPage = 1;
                RowHeight = CurrentSystem.CSList.Count * 50;
                PicRowHeight = CurrentSystem.PStandardList.Count * 50;
                LossImageList = CurrentSystem.SPicList.Count * 50;
                IsLoading = false;
                CurrentScore = CurrentSystem.Score == -1 ? "" : CurrentSystem.Score.ToString();
                //IsPreEnable = false;
                if (SystemList != null && SystemList.Count > 1)
                {
                    //if (index == SystemList.Count - 1)
                    //{
                    //    IsNextEnable = false;
                    //}
                    //else
                    //{
                    //    IsNextEnable = true;
                    //}

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
                    //IsNextEnable = false;
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ViewRecordViewModel");
                return;
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

        //DeleteStanderImageCommand

        //private RelayCommand<int> _deleteStanderImageCommand;
        //public RelayCommand<int> DeleteStanderImageCommand
        //{
        //	get { return _deleteStanderImageCommand ?? (_deleteStanderImageCommand = new RelayCommand<int>(DeleteStanderImage)); }
        //}

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
        //private RelayCommand _itemTappedCommand;
        //public RelayCommand ItemTappedCommand 
        //{ 
        //	get { return _itemTappedCommand ?? (_itemTappedCommand = new RelayCommand(PicItemTapped)); }
        //}
        #endregion

        #region action
        private async void UploadImage()
        {
            try
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
                    MessagingCenter.Send<CommonMessage>(new CommonMessage() { TaskID = "-1" }, "ResetTaskID");
                    file = await CrossMedia.Current.PickPhotoAsync();
                }
                else if (action == "拍照")
                {
                    MessagingCenter.Send<CommonMessage>(new CommonMessage() { TaskID = "-1" }, "ResetTaskID");
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
                    MessagingCenter.Send<CommonMessage>(new CommonMessage() { TaskID = "0" }, "ResetTaskID");
                    IsLoading = false;
                    return;
                }

                imageName = file.Path.Substring(file.Path.LastIndexOf('/') + 1);
                filePath = file.Path;

                //upload file to server
                if (_commonHelper.IsNetWorkConnected() == true)
                {
                    try
                    {
                        var result = await _tourService.UploadFile(file.GetStream(), file.Path);

                        file.Dispose();

                        if (result != null)
                        {
                            var resultInfo = JsonConvert.DeserializeObject<List<AttachDto>>(result.Body);
                            if (result.ResultCode == ResultType.Success)
                            {
                                imageUrl = resultInfo.FirstOrDefault().Url;

                                var addDto = new StandardPic()
                                {
                                    //ImageStream = ImageSource.FromStream(() => { return displayStream; }),//resource,
                                    StandardPicId = 0,
                                    PicName = imageName,
                                    PicType = imageType, //L,G 失分照片／得分照片
                                    TIId = CurrentSystem.TIId,
                                    Url = imageUrl,
                                    FilePath = filePath,
                                    PicId = CurrentSystem.SPicList.Count == 0 ? 1 : CurrentSystem.SPicList.Max(p => p.PicId) + 1
                                };

                                CurrentSystem.SPicList.Add(addDto);
                                CurrentSystem.ParamSPicList.Add(addDto);

                                LossImageList = CurrentSystem.SPicList.Count * 50;
                            }
                            else
                            {
                                _commonFun.AlertLongText(result.Msg);
                                return;
                            }
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
                        MessagingCenter.Send<CommonMessage>(new CommonMessage() { TaskID = "0" }, "ResetTaskID");
                    }
                }
                else
                {
                    MessagingCenter.Send<CommonMessage>(new CommonMessage() { TaskID = "0" }, "ResetTaskID");
                    file.Dispose();
                    _commonFun.AlertLongText("网络连接异常");
                    IsLoading = false;
                    return;
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ViewRegistScoreViewModel");
                return;
            }
        }

        void Preview(StandardPic dto)
        {
            try
            {
                //if (CurrentSystem.SPicList != null && CurrentSystem.SPicList.Count > 0)
                //{
                List<ImagePreviewDto> list = new List<ImagePreviewDto>() { new ImagePreviewDto() {
                    Url = LossPicAttachItem.Url
                }};
                //ImagePreviewDto dto;
                //foreach (var item in CurrentSystem.SPicList)
                //{
                //	dto = new ImagePreviewDto();
                //	dto.Url = item.Url;
                //	list.Add(dto);
                //}
                /*
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (_previewTimer)
                    {
                        _previewTimer = false;
                        await PopupNavigation.PushAsync(new PreviewImagePage(list, 0), true);
                        Device.StartTimer(TimeSpan.FromSeconds(3), () =>
                        {
                            _previewTimer = true;
                            return false;
                        });
                    }
                });*/
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
                        _commonFun.DownLoadFileFromOss(url, filename, "RMMTIMAGEVIEW");

                    }
                }
                catch (Exception)
                {
                    _commonFun.AlertLongText("加载异常，请重试");
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ViewRegistScoreViewModel");
                return;
            }
        }

        private void DeleteImage(StandardPic item)
        {
            try
            {
                var index = CurrentSystem.SPicList.IndexOf(item);

                if (CurrentSystem.SPicList != null && CurrentSystem.SPicList.Count > 0
                    && index > -1)
                {

                    //StandardPic dto = new StandardPic();
                    //dto.TIId = item.TIId;
                    //dto.TPId = item.TPId;

                    //CurrentSystem.SPicList.Insert(index, dto);
                    CurrentSystem.SPicList.Remove(item);
                    CurrentSystem.ParamSPicList.FirstOrDefault(p => p.PicId == item.PicId).Url = string.Empty;

                    LossImageList = CurrentSystem.SPicList.Count * 50;
                    //_deleteList.RemoveAt(index);
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ViewRegistScoreViewModel");
                return;
            }
        }

        private void DeleteStanderImage(PictureStandard item)
        {
            try
            {
                var index = CurrentSystem.PStandardList.IndexOf(item);

                if (index > -1)
                {
                    CurrentSystem.PStandardList.Insert(index, new PictureStandard()
                    {
                        StandardPicId = item.StandardPicId,
                        StandardPicName = item.StandardPicName,
                        TIId = item.TIId,
                        TPId = item.TPId,
                        SeqNo = item.SeqNo,
                        Url = "",
                        SuccessImage = ""
                    });
                    CurrentSystem.PStandardList.Remove(item);
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ViewRegistScoreViewModel");
                return;
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
            //	await PopupNavigation.PushAsync(new PreviewImagePage(list, CurrentPos), true);
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
                    _commonFun.DownLoadFileFromOss(url, filename, "RMMTIMAGEVIEW");

                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("加载异常，请重试");
            }
        }

        private void PrePage()
        {
            try
            {
                if (CurrentPage <= 1)
                {
                    return;
                }
                CurrentSystem.Score = !string.IsNullOrEmpty(CurrentScore) ? Convert.ToInt32(CurrentScore) : -1;
                CurrentPage--;
                UpdateBtnState();
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ViewRegistScoreViewModel");
                return;
            }
        }

        private void NextPage()
        {
            try
            {
                if (CurrentPage > SystemList.Count)
                {
                    return;
                }

                CurrentSystem.GRUD = "I";
                CurrentSystem.Score = !string.IsNullOrEmpty(CurrentScore) ? Convert.ToInt32(CurrentScore) : -1;

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
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ViewRegistScoreViewModel");
                return;
            }
        }

        private void JumpToPage()
        {
            try
            {
                CurrentSystem.Score = !string.IsNullOrEmpty(CurrentScore) ? Convert.ToInt32(CurrentScore) : -1;

                if (JumpPage == 0 || JumpPage > SystemList.Count)
                {
                    string message = "请输入1~" + SystemList.Count.ToString();
                    _commonFun.AlertLongText(message);
                    return;
                }
                CurrentPage = JumpPage;
                UpdateBtnState();
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ViewRegistScoreViewModel");
                return;
            }
        }

        private async void SaveRegistCore()
        {
            try
            {
                if (!CurrentSystem.IsClicked)
                {
                    _commonFun.AlertLongText("已结束的任务不能再上传");
                    return;
                }

                if (SystemList == null || SystemList.Count == 0)
                {
                    _commonFun.AlertLongText("没有可保存的数据");
                    return;
                }

                saveScore = new ScoreCheckResultParam();

                List<ScoreRegDto> ScoreLst = new List<ScoreRegDto>();
                List<CheckResultRegDto> CheckResultLst = new List<CheckResultRegDto>();
                List<StandardPicRegDto> StandardPicLst = new List<StandardPicRegDto>();
                List<PictureStandard> PicStandLst = new List<PictureStandard>();// { get; set; }
                                                                                // int UserId { get; set; }

                ScoreRegDto scoreDto;
                CheckResultRegDto checkDto;
                StandardPicRegDto standDto;
                PictureStandard standPicDto;

                foreach (var item in SystemList)
                {
                    scoreDto = new ScoreRegDto();

                    scoreDto.TIId = item.TIId;
                    scoreDto.GRUD = item.GRUD;
                    scoreDto.TPId = item.TPId;
                    scoreDto.Score = item.SeqNo == CurrentSystem.SeqNo ? (!string.IsNullOrEmpty(CurrentScore) ? Convert.ToInt32(CurrentScore) : -1) : item.Score;//update real source
                                                                                                                                                                 //scoreDto.PassYN = item.PassYN;
                    scoreDto.Remarks = item.Remarks;

                    ScoreLst.Add(scoreDto);

                    foreach (var item1 in item.CSList)
                    {
                        checkDto = new CheckResultRegDto();
                        checkDto.CSId = item1.CSID;
                        checkDto.Result = item1.IsCheck;
                        checkDto.TIId = item1.TIId;
                        checkDto.TPId = item1.TPId;
                        checkDto.GRUD = item.GRUD;

                        CheckResultLst.Add(checkDto);
                    }

                    foreach (var item2 in item.ParamSPicList)
                    {
                        if (item2.StandardPicId == 0)
                        {
                            standDto = new StandardPicRegDto();
                            standDto.TIId = item2.TIId;
                            standDto.TPId = CurrentSystem.TPId;
                            standDto.Url = item2.Url;
                            standDto.PicName = item2.PicName;
                            standDto.PicType = item2.PicType;
                            standDto.GRUD = item.GRUD;

                            StandardPicLst.Add(standDto);
                        }
                    }

                    foreach (var item3 in item.PStandardList)
                    {
                        standPicDto = new PictureStandard();
                        standPicDto.TIId = item3.TIId;
                        standPicDto.TPId = item3.TPId;
                        standPicDto.Url = item3.Url;
                        standPicDto.StandardPicId = item3.StandardPicId;
                        standPicDto.GRUD = item.GRUD;

                        PicStandLst.Add(standPicDto);
                    }
                }

                saveScore.CheckResultLst = CheckResultLst;
                saveScore.ScoreLst = ScoreLst;
                saveScore.StandardPicLst = StandardPicLst;
                saveScore.PicStandLst = PicStandLst;

                saveScore.UserId = Convert.ToInt32(CommonContext.Account.UserId);

                if (saveScore != null || saveScore.CheckResultLst.Any()
                    || saveScore.ScoreLst.Any() || saveScore.StandardPicLst.Any())
                {
                    try
                    {
                        _commonFun.ShowLoading("保存中...");
                        //save
                        var result = await _tourService.SaveRegistCore(saveScore);

                        if (result != null)
                        {
                            if (result.ResultCode == ResultType.Success)
                            {

                                _commonFun.AlertLongText("保存成功");
                                //CurrentSystem.SPicList = _deleteList;
                                //CurrentSystem.SPicList = new ObservableCollection<StandardPic>();
                                //foreach (var sp in _deleteList)
                                //{
                                //	CurrentSystem.SPicList.Add(sp);
                                //}

                                //LossImageList = CurrentSystem.SPicList.Count * 50;
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
                        _commonFun.AlertLongText("保存超时,请重试");
                    }
                    catch (Exception)
                    {
                        _commonFun.AlertLongText("保存异常,请重试");
                    }
                    finally
                    {
                        _commonFun.HideLoading();
                    }
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ViewRegistScoreViewModel");
                return;
            }
        }

        private void LoadImageS()
        {
            CurrentPos = CurrentSystem.SPicList.Count - 1;
        }

        private void Ingnor()
        {
            //CurrentSystem.PassYN = "Q";
            CurrentSystem.Score = 99;
            CurrentScore = "99";
        }
        #endregion

        #region pravite method
        //public async Task SaveToLocal()
        //{
        //	IFolder rootFolder = FileSystem.Current.LocalStorage;
        //	IFolder folder = await rootFolder.CreateFolderAsync("RMMT",
        //		CreationCollisionOption.OpenIfExists);
        //	IFile file = await folder.CreateFileAsync("", CreationCollisionOption.ReplaceExisting);
        //	await file.WriteAllTextAsync("42");
        //}

        private void UpdateBtnState()
        {
            try
            {
                CurrentSystem = SystemList[CurrentPage - 1];

                CurrentSystem.ParamSPicList = new ObservableCollection<StandardPic>();
                //_deleteList = new ObservableCollection<StandardPic>();
                foreach (var spi in CurrentSystem.SPicList)
                {
                    CurrentSystem.ParamSPicList.Add(spi);
                }

                CurrentScore = CurrentSystem.Score == -1 ? "" : CurrentSystem.Score.ToString();
                RowHeight = CurrentSystem.CSList.Count * 50;
                PicRowHeight = CurrentSystem.PStandardList.Count * 50;
                LossImageList = CurrentSystem.SPicList.Count * 50;

                if (CurrentPage == 1)
                {
                    IsPreEnable = false;
                }
                else
                {
                    IsPreEnable = true;
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ViewRegistScoreViewModel");
                return;
            }

            //if (SystemList != null && SystemList.Count > 1)
            //{
            //    if (CurrentPage != SystemList.Count)
            //    {
            //        IsNextEnable = true;
            //    }
            //    else
            //    {
            //        IsNextEnable = false;
            //    }
            //}
            //else
            //{
            //    IsNextEnable = false;
            //}
        }

        private async void UploadStandPic(int picId)
        {
            try
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
                    MessagingCenter.Send<CommonMessage>(new CommonMessage() { TaskID = "-1" }, "ResetTaskID");
                    file = await CrossMedia.Current.PickPhotoAsync();
                }
                else if (action == "拍照")
                {
                    MessagingCenter.Send<CommonMessage>(new CommonMessage() { TaskID = "-1" }, "ResetTaskID");
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
                    MessagingCenter.Send<CommonMessage>(new CommonMessage() { TaskID = "0" }, "ResetTaskID");
                    IsLoading = false;
                    return;
                }

                imageName = file.Path.Substring(file.Path.LastIndexOf('/') + 1);
                filePath = file.Path;

                //upload file to server
                if (_commonHelper.IsNetWorkConnected() == true)
                {
                    try
                    {
                        var result = await _tourService.UploadFile(file.GetStream(), file.Path);

                        file.Dispose();

                        if (result != null)
                        {
                            var resultInfo = JsonConvert.DeserializeObject<List<AttachDto>>(result.Body);
                            if (result.ResultCode == ResultType.Success)
                            {
                                imageUrl = resultInfo.FirstOrDefault().Url;

                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    var item = CurrentSystem.PStandardList.FirstOrDefault(p => p.StandardPicId == picId);
                                    var index = CurrentSystem.PStandardList.IndexOf(item);

                                    if (index > -1)
                                    {
                                        CurrentSystem.PStandardList.Insert(index, new PictureStandard()
                                        {
                                            StandardPicId = item.StandardPicId,
                                            StandardPicName = item.StandardPicName,
                                            TIId = item.TIId,
                                            TPId = item.TPId,
                                            SeqNo = item.SeqNo,
                                            Url = imageUrl,
                                            SuccessImage = "icon_success"
                                        });
                                        CurrentSystem.PStandardList.Remove(item);
                                    }

                                //CurrentSystem.PStandardList.Insert(
                                //CurrentSystem.PStandardList.FirstOrDefault(p => p.StandardPicId == picId).Url = imageUrl;
                            });
                            }
                            else
                            {
                                _commonFun.AlertLongText(result.Msg);
                                return;
                            }
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
                        MessagingCenter.Send<CommonMessage>(new CommonMessage() { TaskID = "0" }, "ResetTaskID");
                    }
                }
                else
                {
                    MessagingCenter.Send<CommonMessage>(new CommonMessage() { TaskID = "0" }, "ResetTaskID");
                    file.Dispose();
                    _commonFun.AlertLongText("网络连接异常");
                    IsLoading = false;
                    return;
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ViewRegistScoreViewModel");
                return;
            }
        }
        #endregion
    }
}
