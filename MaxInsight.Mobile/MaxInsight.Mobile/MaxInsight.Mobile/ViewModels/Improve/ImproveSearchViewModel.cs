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
    public class ImproveSearchViewModel : ViewModel
    {
        IImproveService improveService;
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        List<RequestParameter> paramList = new List<RequestParameter>();
        public ImproveSearchViewModel()
        {
            try
            {
                improveService = Resolver.Resolve<IImproveService>();
                _commonFun = Resolver.Resolve<ICommonFun>();
                _commonHelper = Resolver.Resolve<CommonHelper>();
                MessagingCenter.Unsubscribe<string>(this, MessageConst.IMPROVE_IMPPLANORRESULTDATA_GET);
                MessagingCenter.Subscribe<string>(this, MessageConst.IMPROVE_IMPPLANORRESULTDATA_GET, (c) =>
                {
                    if (paramList != null && paramList.Count > 0)
                        GetImproveResultOrPlan(paramList);
                });
                MessagingCenter.Subscribe<List<RequestParameter>>(this, MessageConst.PASS_IMPROVESEARCHCONDITION, (param) =>
                 {
                     if (param != null && param.Count > 0)
                     {
                         paramList = param;
                         StatueType = param.Find(p => p.Name == "StatueTypeName").Value;
                         StartDate = param.Find(p => p.Name == "StartDate").Value;
                         EndDate = param.Find(p => p.Name == "EndDate").Value;
                         Statue = param.Find(p => p.Name == "StatueName").Value;
                         Service = param.Find(p => p.Name == "ServiceName").Value;
                         Department = param.Find(p => p.Name == "DepartmentName").Value;
                         ItemName = param.Find(p => p.Name == "ItemName").Value;
                         PlanSelectName = param.Find(p => p.Name == "PlanSelectName").Value;
                         SourceTypeName = param.Find(p => p.Name == "SourceTypeName").Value;
                         GetImproveResultOrPlan(param);
                     }
                     else if (paramList != null && paramList.Count > 0)
                     {
                         GetImproveResultOrPlan(paramList);
                     }

                 });
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ImproveSearchViewModel");
                return;
            }
        }
        #region Property
        private string itemName;
        public string ItemName
        {
            get { return itemName; }
            set { SetProperty(ref itemName, value); }
        }
        private string statueType;
        public string StatueType
        {
            get
            {
                return statueType;
            }
            set
            {
                SetProperty(ref statueType, value);
            }
        }
        private string startDate;
        public string StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                SetProperty(ref startDate, value);
            }
        }
        private string endDate;
        public string EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                SetProperty(ref endDate, value);
            }
        }
        private string statue;
        public string Statue
        {
            get
            {
                return statue;
            }
            set
            {
                SetProperty(ref statue, value);
            }
        }
        private string service;
        public string Service
        {
            get
            {
                return service;
            }
            set
            {
                SetProperty(ref service, value);
            }
        }
        private string department;
        public string Department
        {
            get
            {
                return department;
            }
            set
            {
                SetProperty(ref department, value);
            }
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
        private string _planSelectName;
        public string PlanSelectName
        {
            get { return _planSelectName; }
            set { SetProperty(ref _planSelectName, value); }
        }
        private string _sourceTypeName;
        public string SourceTypeName
        {
            get { return _sourceTypeName; }
            set { SetProperty(ref _sourceTypeName, value); }
        }
        #endregion
        #region Commmand
        public Command GoImproveConditionPageCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        await Navigation.PushAsync<ImproveSearchConditionViewModel>((vm, v) => vm.Init(), true);
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
            catch (Exception ex)
            {

            }
        }
        private void GetImproveResultOrPlan(List<RequestParameter> param)
        {
            try
            {
                string startDate = param.Find(p => p.Name == "StartDate").Value.Replace("-", "");
                string endDate = param.Find(p => p.Name == "EndDate").Value.Replace("-", "");
                string statue = param.Find(p => p.Name == "Statue").Value;
                int serviceId = Int32.Parse(param.Find(p => p.Name == "ServiceId").Value);
                int departmentId = Int32.Parse(param.Find(p => p.Name == "DepartmentId").Value);
                int userId = Int32.Parse(CommonContext.Account.UserId);
                string itemName = ItemName;
                int planid = Int32.Parse(param.Find(p => p.Name == "PlanSelect").Value);
                string sourceType = param.Find(p => p.Name == "SourceType").Value;
                if (param.Find(p => p.Name == "StatueType").Value == "A")
                {

                    GetImprovePlan(itemName, startDate, endDate, userId, "A", statue, serviceId, departmentId, planid, sourceType);
                }
                else
                {
                    GetImproveResult(itemName, startDate, endDate, userId, "R", statue, serviceId, departmentId, planid, sourceType);
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ImproveSearchViewModel");
                return;
            }
        }

        private async void GetImproveResult(string itemNae, string startDate, string endDate, int userId, string statueType, string statue, int serviceId, int departmentId, int planid, string sourceType)
        {
            try
            {
                _commonFun.ShowLoading("查询中...");
                //TO-DO
                var result = await improveService.GetResult(itemNae, startDate, endDate, userId, statueType, statue, serviceId, departmentId, planid, sourceType);
                if (result.ResultCode == Module.ResultType.Success)
                {

                    List<ImprovementMngDto> improveResultInfo = CommonHelper.DecodeString<List<ImprovementMngDto>>(result.Body);
                    if (improveResultInfo != null && improveResultInfo.Count > 0)
                    {
                        _commonFun.HideLoading();
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

        private async void GetImprovePlan(string itemNae, string startDate, string endDate, int userId, string statueType, string statue, int serviceId, int departmentId, int planid, string sourceType)
        {
            if (_commonHelper.IsNetWorkConnected() == true)
            {
                try
                {
                    _commonFun.ShowLoading("查询中...");
                    // TO-DO
                    var result = await improveService.GetResult(itemNae, startDate, endDate, userId, statueType, statue, serviceId, departmentId, planid, sourceType);
                    if (result.ResultCode == Module.ResultType.Success)
                    {

                        List<ImprovementMngDto> improvePlanInfo = CommonHelper.DecodeString<List<ImprovementMngDto>>(result.Body);
                        if (improvePlanInfo != null && improvePlanInfo.Count > 0)
                        {
                            ImprovePlansOrResults = improvePlanInfo;
                            _commonFun.HideLoading();
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
            else
            {
                _commonFun.AlertLongText("网络连接异常。");
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
            catch (Exception)
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
                    await Navigation.PushAsync<ImproveDistributionViewModel>((vm, v) => vm.Init(improvementMng, paramList), true);
                }
                else
                {
                    if (improvementMng.PlanStatus == "A")
                    {
                        _commonFun.AlertLongText("未分配，没有分配详细");
                    }
                    else
                    {
                        await Navigation.PushAsync<ImproveDistributionViewModel>((vm, v) => vm.Init(improvementMng, paramList), true);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}
