using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Services.Tour;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;
using MaxInsight.Mobile.Services.RemoteService;
using MaxInsight.Mobile.Module.Dto.Improve;
using MaxInsight.Mobile.Services.ImproveService;
using MaxInsight.Mobile.ViewModels.Improve;

namespace MaxInsight.Mobile
{
    public class ShopfrontMainPageViewModel : ViewModel
    {
        private readonly ITourService _tourService;
        private readonly ICommonFun _commonFun;
        private readonly CommonHelper _commonHelper;
        private readonly ILocalScoreService _localScoreService;
        private readonly IImproveService _improveService;
        public RelayCommand<TourDistributorDto> ItemTappedCommand { get; set; }
        private bool canGo = false;

        public ShopfrontMainPageViewModel()
        {
            _tourService = Resolver.Resolve<ITourService>();
            _localScoreService = Resolver.Resolve<ILocalScoreService>();
            _commonFun = Resolver.Resolve<ICommonFun>();
            _commonHelper = Resolver.Resolve<CommonHelper>();
            ItemTappedCommand = new RelayCommand<TourDistributorDto>(TappedCommand);
            _improveService = Resolver.Resolve<IImproveService>();

            MessagingCenter.Subscribe<ShopfrontMainPage>(this, "GetShops", (sender) =>
            {
                canGo = false;
                Task.Run(async () =>
                {
                    await GetShops();
                });
            });
            MessagingCenter.Subscribe<string>(this, "GetImproveDitstriLst", async (arg) =>
            {
                await GetImproveDitstriLst(arg);
            });

            Device.BeginInvokeOnMainThread(async () =>
            {
                if (CommonContext.Account.UserType == "S")
                {
                    await GetImproveDitstriLst("");
                }
                else
                {
                    await GetShops();
                }
            });

        }

        private async Task GetShops()
        {
            try
            {
                _commonFun.ShowLoading("查询中...");
                //if (_commonHelper.IsNetWorkConnected() == true)
                //{
                //    var result = await _tourService.GetShops(Convert.ToInt32(CommonContext.Account.UserId));
                //    if (null != result && result.ResultCode == Module.ResultType.Success)
                //    {
                //        Shops = JsonConvert.DeserializeObject<List<TourDistributorDto>>(result.Body);
                //        canGo = true;
                //    }
                //}
                //else
                //{
                var distributor = await _localScoreService.SearchDistributors();
                Shops = new List<TourDistributorDto>();
                foreach (var item in distributor)
                {
                    Shops.Add(new TourDistributorDto { DisId = Convert.ToInt32(item.Id), DisCode = item.Code, DisName = item.Name });
                }
                canGo = true;
                //}
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

        private async Task GetImproveDitstriLst(string arg)
        {
            if (_commonHelper.IsNetWorkConnected() == true)
            {
                try
                {
                    if (arg != "R")
                    {
                        _commonFun.ShowLoading("查询中...");
                    }
                    int userId = Int32.Parse(CommonContext.Account.UserId);
                    // TO-DO
                    var result = await _improveService.GetResult("", "19000101", "99991231", userId, "A", "A", 0, 0, 0, "");
                    if (result.ResultCode == Module.ResultType.Success)
                    {

                        List<ImprovementMngDto> improvePlanInfo = CommonHelper.DecodeString<List<ImprovementMngDto>>(result.Body);
                        if (improvePlanInfo != null && improvePlanInfo.Count > 0)
                        {
                            ImproveDistriLst = improvePlanInfo;
                            _commonFun.HideLoading();
                        }
                        else
                        {
                            _commonFun.HideLoading();
                            ImproveDistriLst = new List<ImprovementMngDto>();
                            _commonFun.ShowToast("没有数据");
                        }
                    }
                    else
                    {
                        _commonFun.HideLoading();
                        ImproveDistriLst = new List<ImprovementMngDto>();
                        _commonFun.AlertLongText("查询失败，请重试。 " + result.Msg);
                    }
                }
                catch (OperationCanceledException)
                {
                    _commonFun.HideLoading();
                    ImproveDistriLst = new List<ImprovementMngDto>();
                    _commonFun.AlertLongText("请求超时。");
                }
                catch (Exception)
                {
                    _commonFun.HideLoading();
                    ImproveDistriLst = new List<ImprovementMngDto>();
                    _commonFun.AlertLongText("查询异常，请重试。");
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

        #region Propertys
        private List<TourDistributorDto> _shops;
        public List<TourDistributorDto> Shops
        {
            get { return _shops; }
            set { SetProperty(ref _shops, value); }
        }

        private TourDistributorDto _shop;
        public TourDistributorDto Shop
        {
            get { return _shop; }
            set { SetProperty(ref _shop, value); }
        }

        private List<ImprovementMngDto> _improveDistriLst;
        public List<ImprovementMngDto> ImproveDistriLst
        {
            get { return _improveDistriLst; }
            set { SetProperty(ref _improveDistriLst, value); }
        }

        private ImprovementMngDto _selectedImproveDistri;
        public ImprovementMngDto SelectedImproveDistri
        {
            get { return _selectedImproveDistri; }
            set { SetProperty(ref _selectedImproveDistri, value); }
        }
        #endregion

        public void TappedCommand(TourDistributorDto dto)
        {
            try
            {
                if (canGo)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await Navigation.PushAsync<TaskListViewModel>((vm, v) => vm.Init(dto == null ? Shop.DisId : dto.DisId), true);

                    });
                }
            }
            catch (Exception)
            {
            }
        }

        private RelayCommand _downLoadTaskCommand;
        public RelayCommand DownLoadTaskCommand
        {
            get
            {
                return _downLoadTaskCommand
                    ?? (_downLoadTaskCommand = new RelayCommand(DownLoadTask));
            }
        }

        private RelayCommand _imroveItemTappedCommand;
        public RelayCommand ImroveItemTappedCommand
        {
            get
            {
                return _imroveItemTappedCommand ?? (_imroveItemTappedCommand = new RelayCommand(ImproveItemTapped));
            }
        }
        private async void ImproveItemTapped()
        {
            try
            {
                CommonContext.ImpPlanStatus = SelectedImproveDistri.PlanStatus;
                await Navigation.PushAsync<ImproveDistributionViewModel>((vm, v) => vm.Init(SelectedImproveDistri), true);
            }
            catch (Exception)
            {
            }
        }

        private async void DownLoadTask()
        {
            await MasterDataDownloadHelper.DownloadData();
        }
    }
}
