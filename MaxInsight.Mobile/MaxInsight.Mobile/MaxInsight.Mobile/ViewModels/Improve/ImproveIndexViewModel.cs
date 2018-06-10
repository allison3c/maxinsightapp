using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto.Improve;
using MaxInsight.Mobile.Services.ImproveService;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.Improve
{
    public class ImproveIndexViewModel : ViewModel
    {
        IImproveService improveService;
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        public ImproveIndexViewModel()
        {
            try
            {
                improveService = Resolver.Resolve<IImproveService>();
                _commonFun = Resolver.Resolve<ICommonFun>();
                _commonHelper = Resolver.Resolve<CommonHelper>();
                //MessagingCenter.Unsubscribe<ImproveIndexPage>(this, MessageConst.IMPROVE_RESULT_GET);
                //MessagingCenter.Subscribe<ImproveIndexPage>(this, MessageConst.IMPROVE_RESULT_GET, (c) =>
                // {
                //     GetImproveResultOrResultApproval();
                // });
                MessagingCenter.Unsubscribe<string>(this, MessageConst.IMPROVE_PLANLSTDATA_GET);
                MessagingCenter.Subscribe<string>(this, MessageConst.IMPROVE_PLANLSTDATA_GET, (c) =>
                {
                    if (c == "A")
                    {
                        GetImprovePlanOrPlanApproval();
                    }
                    else
                    {
                        GetImproveResultOrResultApproval();
                    }
                });
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ImproveIndexViewModel");
                return;
            }
        }
        #region Property
        List<ImprovementMngDto> improvePlans;
        public List<ImprovementMngDto> ImprovePlans
        {
            get { return improvePlans; }
            set { SetProperty(ref improvePlans, value); }
        }
        ImprovementMngDto selectedImprovePlan;
        public ImprovementMngDto SelectedImprovePlan
        {
            get { return selectedImprovePlan; }
            set { SetProperty(ref selectedImprovePlan, value); }
        }
        List<ImprovementMngDto> improveResults;
        public List<ImprovementMngDto> ImproveResults
        {
            get { return improveResults; }
            set { SetProperty(ref improveResults, value); }
        }
        ImprovementMngDto selectedImproveResult;
        public ImprovementMngDto SelectedImproveResult
        {
            get { return selectedImproveResult; }
            set { SetProperty(ref selectedImproveResult, value); }
        }
        List<ImprovementMngDto> improvePlansOrResults;
        public List<ImprovementMngDto> ImprovePlansOrResults
        {
            get { return improvePlansOrResults; }
            set { SetProperty(ref improvePlansOrResults, value); }
        }
        ImprovementMngDto selectedImprovePlanOrResult;
        public ImprovementMngDto SelectedImprovePlanOrResult
        {
            get { return selectedImprovePlanOrResult; }
            set { SetProperty(ref selectedImprovePlanOrResult, value); }
        }
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }
        #endregion
        #region Command
        public Command GoImproveSearchPageCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        await Navigation.PushAsync<ImproveSearchViewModel>(true);
                    }
                    catch (Exception)
                    {

                    }

                });
            }
        }
        private RelayCommand<ImprovementMngDto> goImpPlanCommitPageCommand;
        public RelayCommand<ImprovementMngDto> GoImpPlanCommitPageCommand
        {
            get
            {
                return goImpPlanCommitPageCommand
                       ?? (goImpPlanCommitPageCommand = new RelayCommand<ImprovementMngDto>(I => GoImpPlanCommitPage(I)));
            }
        }
        private RelayCommand<ImprovementMngDto> goImpResultCommitPageCommand;
        public RelayCommand<ImprovementMngDto> GoImpResultCommitPageCommand
        {
            get
            {
                return goImpResultCommitPageCommand
                       ?? (goImpResultCommitPageCommand = new RelayCommand<ImprovementMngDto>(I => GoImpResultCommitPage(I)));
            }
        }
        private RelayCommand<ImprovementMngDto> goImproveDistributionPageCommand;
        public RelayCommand<ImprovementMngDto> GoImproveDistributionPageCommand
        {
            get
            {
                return goImproveDistributionPageCommand
                       ?? (goImproveDistributionPageCommand = new RelayCommand<ImprovementMngDto>(I => GoImproveDistributionPage(I)));
            }
        }
        public Command ImprovePlanOrPlanApprovalSearchCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (_commonHelper.IsNetWorkConnected() == true)
                    {
                        GetImprovePlanOrPlanApproval();
                        //MessagingCenter.Send<string>("", MessageConst.IMPROVE_PLANLST_SHOW);
                    }
                    else
                    {
                        _commonFun.AlertLongText("网络连接异常。");
                    }
                });
            }
        }
        public Command ImproveResultOrResultApprovalSearchCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (_commonHelper.IsNetWorkConnected() == true)
                    {
                        GetImproveResultOrResultApproval();
                        //MessagingCenter.Send<string>("", MessageConst.IMPROVE_RESULTLST_SHOW);
                    }
                    else
                    {
                        _commonFun.AlertLongText("网络连接异常。");
                    }

                });
            }
        }
        private RelayCommand _itemTappedCommand;
        public RelayCommand ItemTappedCommand
        {

            get
            {
                return _itemTappedCommand ?? (_itemTappedCommand = new RelayCommand(ItemTapped));
            }
        }
        #endregion
        #region Event
        private async void ItemTapped()
        {
            try
            {
                //var action = await _commonFun.ShowActionSheet("详细/分配", "计划", "结果");
                //IsLoading = true;
                //if (action == "详细/分配")
                //{
                //    GoImproveDistributionPage(SelectedImprovePlanOrResult);
                //    IsLoading = false;
                //}
                //else if (action == "计划")
                //{
                //    GoImpPlanCommitPage(SelectedImprovePlanOrResult);
                //    IsLoading = false;
                //}
                //else if (action == "结果")
                //{
                //    GoImpResultCommitPage(SelectedImprovePlanOrResult);
                //    IsLoading = false;
                //}
                //else
                //{
                //    IsLoading = false;
                //}
                if (SelectedImprovePlanOrResult.PlanStatus == "A")
                {
                    GoImproveDistributionPage(SelectedImprovePlanOrResult);
                }
                else if (SelectedImprovePlanOrResult.PlanStatus == "G")
                {
                    GoImpResultCommitPage(SelectedImprovePlanOrResult);
                }
                else
                {
                    GoImpPlanCommitPage(SelectedImprovePlanOrResult);
                }
                IsLoading = false;
            }
            catch (Exception)
            {
            }
        }
        private async void GoImpPlanCommitPage(ImprovementMngDto improvementMng)
        {
            try
            {
                List<RequestParameter> list = new List<RequestParameter>();
                list.Add(new RequestParameter { Name = "improvementId", Value = improvementMng.ImprovementId.ToString() });
                list.Add(new RequestParameter { Name = "impResultId", Value = improvementMng.ImpResultId.ToString() });
                list.Add(new RequestParameter { Name = "tPId", Value = improvementMng.TPId.ToString() });
                list.Add(new RequestParameter { Name = "itemId", Value = improvementMng.ItemId.ToString() });
                list.Add(new RequestParameter { Name = "planApproalYN", Value = improvementMng.PlanApproalYN.ToString() });
                list.Add(new RequestParameter { Name = "PlanStatus", Value = improvementMng.PlanStatus });
                list.Add(new RequestParameter { Name = "AllocateYN", Value = improvementMng.AllocateYN.ToString() });
                await Navigation.PushAsync<ImpPlanCommitViewModel>((vm, v) => vm.Init(improvementMng, list), true);
            }
            catch (Exception ex)
            {
            }
        }
        private async void GoImpResultCommitPage(ImprovementMngDto improvementMng)
        {
            try
            {
                List<RequestParameter> list = new List<RequestParameter>();
                list.Add(new RequestParameter { Name = "improvementId", Value = improvementMng.ImprovementId.ToString() });
                list.Add(new RequestParameter { Name = "impResultId", Value = improvementMng.ImpResultId.ToString() });
                list.Add(new RequestParameter { Name = "tPId", Value = improvementMng.TPId.ToString() });
                list.Add(new RequestParameter { Name = "itemId", Value = improvementMng.ItemId.ToString() });
                list.Add(new RequestParameter { Name = "ResultApproalYN", Value = improvementMng.ResultApproalYN.ToString() });
                list.Add(new RequestParameter { Name = "ResultStatus", Value = improvementMng.ResultStatus });
                list.Add(new RequestParameter { Name = "AllocateYN", Value = improvementMng.AllocateYN.ToString() });
                await Navigation.PushAsync<ImpResultCommitViewModel>((vm, v) => vm.Init(improvementMng, list), true);
            }
            catch (Exception)
            {
            }
        }
        private async void GoImproveDistributionPage(ImprovementMngDto improvementMng)
        {
            try
            {
                CommonContext.ImpPlanStatus = improvementMng.PlanStatus;
                if (CommonContext.Account.UserType == "S")
                {
                    await Navigation.PushAsync<ImproveDistributionViewModel>((vm, v) => vm.Init(improvementMng), true);
                }
                else
                {
                    if (improvementMng.PlanStatus == "A")
                    {
                        _commonFun.AlertLongText("未分配，没有分配详细");
                    }
                    else
                    {
                        await Navigation.PushAsync<ImproveDistributionViewModel>((vm, v) => vm.Init(improvementMng), true);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        public async void GetImproveResultOrResultApproval()
        {
            try
            {
                if (CommonContext.Account.UserType == "A" || CommonContext.Account.UserType == "R")
                {
                    return;
                }
                _commonFun.ShowLoading("查询中...");
                //TO-DO
                APIResult result = new APIResult();
                if (CommonContext.Account.UserType == "D")
                {
                    result = await improveService.GetResult("", "19000101", "99991231", Convert.ToInt32(CommonContext.Account.UserId), "R", "A,D", 0, 0, 0);
                }
                else if (CommonContext.Account.UserType == "S")
                {
                    result = await improveService.GetResult("", "19000101", "99991231", Convert.ToInt32(CommonContext.Account.UserId), "R", "C,F", 0, 0, 0);
                }
                else if (CommonContext.Account.UserType == "Z")
                {
                    result = await improveService.GetResult("", "19000101", "99991231", Convert.ToInt32(CommonContext.Account.UserId), "R", "E", 0, 0, 0);
                }
                if (result.ResultCode == Module.ResultType.Success)
                {

                    List<ImprovementMngDto> improveResultInfo = CommonHelper.DecodeString<List<ImprovementMngDto>>(result.Body);
                    if (improveResultInfo != null && improveResultInfo.Count > 0)
                    {
                        _commonFun.HideLoading();
                        //ImproveResults = improveResultInfo;
                        ImprovePlansOrResults = improveResultInfo;
                    }
                    else
                    {
                        _commonFun.HideLoading();
                        ImprovePlansOrResults = new List<ImprovementMngDto>();
                        _commonFun.ShowToast("没有数据");
                    }
                }
                else
                {
                    _commonFun.HideLoading();
                    ImprovePlansOrResults = new List<ImprovementMngDto>();
                    _commonFun.AlertLongText("查询失败，请重试。 " + result.Msg);
                }
            }
            catch (OperationCanceledException)
            {
                _commonFun.HideLoading();
                ImprovePlansOrResults = new List<ImprovementMngDto>();
                _commonFun.AlertLongText("请求超时。");
            }
            catch (Exception)
            {
                _commonFun.HideLoading();
                ImprovePlansOrResults = new List<ImprovementMngDto>();
                _commonFun.AlertLongText("查询异常，请重试。");
            }
            finally
            {
                _commonFun.HideLoading();
            }
        }

        public async void GetImprovePlanOrPlanApproval()
        {
            try
            {
                if (CommonContext.Account.UserType == "A" || CommonContext.Account.UserType == "R")
                {
                    return;
                }
                _commonFun.ShowLoading("查询中...");
                //TO-DO
                APIResult result = new APIResult();
                if (CommonContext.Account.UserType == "D")
                {
                    result = await improveService.GetResult("", "19000101", "99991231", Convert.ToInt32(CommonContext.Account.UserId), "A", "B,D", 0, 0, 0);
                }
                else if (CommonContext.Account.UserType == "S")
                {
                    result = await improveService.GetResult("", "19000101", "99991231", Convert.ToInt32(CommonContext.Account.UserId), "A", "C,F", 0, 0, 0);
                }
                else if (CommonContext.Account.UserType == "Z")
                {
                    result = await improveService.GetResult("", "19000101", "99991231", Convert.ToInt32(CommonContext.Account.UserId), "A", "E", 0, 0, 0);
                }
                if (result.ResultCode == Module.ResultType.Success)
                {

                    List<ImprovementMngDto> improvePlanInfo = CommonHelper.DecodeString<List<ImprovementMngDto>>(result.Body);
                    if (improvePlanInfo != null && improvePlanInfo.Count > 0)
                    {
                        _commonFun.HideLoading();
                        //ImprovePlans = improvePlanInfo;
                        ImprovePlansOrResults = improvePlanInfo;
                    }
                    else
                    {
                        _commonFun.HideLoading();
                        ImprovePlansOrResults = new List<ImprovementMngDto>();
                        _commonFun.ShowToast("没有数据");
                    }
                }
                else
                {
                    _commonFun.HideLoading();
                    ImprovePlansOrResults = new List<ImprovementMngDto>();
                    _commonFun.AlertLongText("查询失败，请重试。 " + result.Msg);
                }
            }
            catch (OperationCanceledException)
            {
                _commonFun.HideLoading();
                ImprovePlansOrResults = new List<ImprovementMngDto>();
                _commonFun.AlertLongText("请求超时。");
            }
            catch (Exception)
            {
                _commonFun.HideLoading();
                ImprovePlansOrResults = new List<ImprovementMngDto>();
                _commonFun.AlertLongText("查询异常，请重试。");
            }
            finally
            {
                _commonFun.HideLoading();
            }
        }
        #endregion
    }
}
