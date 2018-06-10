using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto.Shops;
using MaxInsight.Mobile.Services.RemoteService;
using MaxInsight.Mobile.Services.Tour;
using System;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.ShopfrontPatrolModel
{
    public class CustomizedTaskViewModel : ViewModel
    {
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        ITourService _tourService;
        ILocalScoreService _localScoreService;
        string fromSeachYN = "search";

        public CustomizedTaskViewModel()
        {
            try
            {
                _commonFun = Resolver.Resolve<ICommonFun>();
                _commonHelper = Resolver.Resolve<CommonHelper>();
                _tourService = Resolver.Resolve<ITourService>();
                _localScoreService = Resolver.Resolve<ILocalScoreService>();


                MessagingCenter.Subscribe<string>(this, "TaskSearchParam", (param) =>
                {
                    fromSeachYN = param;
                });
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->CustImproveViewModel");
                return;
            }
        }

        #region properties
        private CustomizedTaskDto _customizedTask;
        public CustomizedTaskDto CustomizedTask
        {
            get
            {
                return _customizedTask;
            }
            set
            {
                SetProperty(ref _customizedTask, value);
            }
        }
        private string _visibleYN;
        public string VisibleYN
        {
            get
            {
                return _visibleYN;
            }
            set
            {
                SetProperty(ref _visibleYN, value);
            }
        }

        #endregion

        #region command
        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ??

                (_saveCommand = new RelayCommand(LocalSaveMethod));
            }
        }
        #endregion

        #region methods
        public void Init(CustomizedTaskDto dto, string tPStatus)
        {
            try
            {
                CustomizedTask = dto;
                if (tPStatus == "E" && fromSeachYN == "search")//结束
                {
                    VisibleYN = "N";
                }
                else
                {
                    VisibleYN = "Y";
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->CustomizedTaskViewModel");
                return;
            }
        }

        private async void SaveMethod()
        {
            try
            {
                if (CustomizedTask == null)
                {
                    return;
                }
                else if (string.IsNullOrEmpty(CustomizedTask.Remarks))
                {
                    _commonFun.AlertLongText("请填写备注信息");
                    return;
                }
                CustomizedTaskDto dto = new CustomizedTaskDto()
                {
                    ScoreId = CustomizedTask.ScoreId,
                    TPId = CustomizedTask.TPId,
                    UserId = int.Parse(CommonContext.Account.UserId),
                    Remarks = CustomizedTask.Remarks

                };

                _commonFun.ShowLoading("保存中...");
                var result = await _tourService.CustomizedTaskCheck(dto);
                if (result != null)
                {
                    if (result.ResultCode == ResultType.Success)
                    {
                        await Navigation.PopAsync();
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

        private async void LocalSaveMethod()
        {
            try
            {
                if (CustomizedTask == null)
                {
                    return;
                }
                else if (string.IsNullOrEmpty(CustomizedTask.Remarks))
                {
                    _commonFun.AlertLongText("请填写备注信息");
                    return;
                }
                CustomizedTaskDto dto = new CustomizedTaskDto()
                {
                    ScoreId = CustomizedTask.ScoreId,
                    TPId = CustomizedTask.TPId,
                    UserId = int.Parse(CommonContext.Account.UserId),
                    Remarks = CustomizedTask.Remarks

                };

                _commonFun.ShowLoading("保存中...");
                int result = _localScoreService.LocalCustomizedTaskCheck(dto);
                if (result > 0)
                {

                    await Navigation.PopAsync();
                    _commonFun.AlertLongText("保存成功");
                }

                else
                {
                    _commonFun.AlertLongText("保存失败");
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

        #endregion
    }
}
