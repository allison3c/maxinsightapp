using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module.Dto.ReviewPlans;
using MaxInsight.Mobile.Services.ReviewPlansService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.ReviewPlans
{
    public class ReviewPlansIndexViewModel : ViewModel
    {
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        IReviewPlansService _reviewPlansService;
        public RelayCommand<ReviewPlansListDto> ItemTappedCommand { get; set; }
        public ReviewPlansIndexViewModel()
        {
            _commonFun = Resolver.Resolve<ICommonFun>();
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _reviewPlansService = Resolver.Resolve<IReviewPlansService>();
            ItemTappedCommand = new RelayCommand<ReviewPlansListDto>(TappedCommand);
            MessagingCenter.Subscribe<string>(this, MessageConst.GETREVIEWPLANSLIST, (c) =>
            {
                SearchReviewPlansList();
            });
        }
        #region Property
        public List<ReviewPlansListDto> _reviewPlansList;
        public List<ReviewPlansListDto> ReviewPlansList
        {
            get
            {
                return _reviewPlansList;
            }
            set
            {
                SetProperty(ref _reviewPlansList, value);
            }
        }
        public ReviewPlansListDto _selectedPlansItem;
        public ReviewPlansListDto SelectedPlansItem
        {
            get
            {
                return _selectedPlansItem;
            }
            set
            {
                SetProperty(ref _selectedPlansItem, value);
            }
        }
        #endregion
        #region Command
        public async void TappedCommand(ReviewPlansListDto dto)
        {
            try
            {
                await Navigation.PushAsync<ReviewPlansViewModel>((vm, v) => vm.Init(SelectedPlansItem.Id), true);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Method
        public async void SearchReviewPlansList()
        {
            try
            {
                _commonFun.ShowLoading("查询中...");              
                var result = await _reviewPlansService.GetReviewPlansList_Mobile(CommonContext.Account.UserId);
                if (null != result && result.ResultCode == Module.ResultType.Success)
                {
                    var reviewPlansList = JsonConvert.DeserializeObject<List<ReviewPlansListDto>>(result.Body);

                    if (reviewPlansList != null && reviewPlansList.Count > 0)
                    {
                        ReviewPlansList = reviewPlansList;
                    }
                    else
                    {
                        ReviewPlansList = new List<ReviewPlansListDto>();
                        //_commonFun.AlertLongText("没有数据.");
                    }
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
        #endregion
    }
}
