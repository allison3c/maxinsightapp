using System;
using System.Collections.Generic;
using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Services.Tour;
using Newtonsoft.Json;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;
using MaxInsight.Mobile.ViewModels.ShopfrontPatrolModel;
using MaxInsight.Mobile.Module.Dto.Shops;
using System.Linq;
using MaxInsight.Mobile.Services.RemoteService;
using MaxInsight.Mobile.Pages.ShopfrontPatrol;

namespace MaxInsight.Mobile
{
    public class TaskListViewModel : ViewModel
    {
        private int Id { get; set; }

        private readonly ITourService _tourService;
        private readonly ILocalScoreService _localScoreService;
        private readonly ICommonFun _commonFun;
        private readonly CommonHelper _commonHelper;
        private readonly IRemoteService _remoteService;
        private int _disId { get; set; }
        private bool _isChecking { get; set; } = true;
        private string _statu { get; set; } = string.Empty;

        public TaskListViewModel()
        {
            _tourService = Resolver.Resolve<ITourService>();
            _localScoreService = Resolver.Resolve<ILocalScoreService>();
            _commonFun = Resolver.Resolve<ICommonFun>();
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _remoteService = Resolver.Resolve<IRemoteService>();


            MessagingCenter.Subscribe<TourDistributorDto>(this, "SendShopItem", (obj) =>
            {
                _disId = obj.DisId;
                _isChecking = true;
                _statu = "N";
                //if (_commonHelper.IsNetWorkConnected() == true)
                //{
                //    GetPlans(_disId, "", "", _statu);
                //}
                //else
                //{
                GetLocalPlans(_disId);
                //}
            });


            //MessagingCenter.Subscribe<TaskListPage>(this, "RefreshTask", (obj) =>
            //{
            //	if (_disId != 0)
            //	{
            //		GetPlans(_disId);
            //	}
            //});

            MessagingCenter.Subscribe<TaskOfPlanDto>(this, "CheckTask", async (obj) =>
            {
                if (_isChecking)
                {
                    //if (_commonHelper.IsNetWorkConnected() == true)
                    //{
                    //    if (obj.TPStatus == "E")
                    //    {
                    //        if (obj.TPType == "C")
                    //        {
                    //            CheckCustomizedTask(obj.TPId, "A", obj.TPStatus);
                    //        }
                    //        else
                    //        {
                    //            CheckPlan(obj.TPId, "A");
                    //        }

                    //    }
                    //    else
                    //    {
                    //        var action = await _commonFun.ShowActionSheet("开始检查", "结束检查");

                    //        if (action == "开始检查")
                    //        {
                    //            if (obj.TPType == "C")// 自定义任务
                    //            {
                    //                CheckCustomizedTask(obj.TPId, "S", obj.TPStatus);
                    //            }
                    //            else
                    //            {
                    //                CheckStartPlan(obj.TPId);
                    //            }
                    //        }
                    //        else if (action == "结束检查")
                    //        {
                    //            if (obj.TPType == "C")// 自定义任务
                    //            {
                    //                CheckCustomizedTask(obj.TPId, "E", obj.TPStatus);
                    //            }
                    //            else
                    //            {
                    //                CheckEndPlan(obj.TPId);
                    //            }

                    //        }
                    //    }
                    //}

                    // 没有网络的状态下
                    if (obj.TPStatus == "E")
                    {
                        if (obj.TPType == "C")
                        {
                            //CheckCustomizedTask(obj.TPId, "A", obj.TPStatus);
                            LocalCheckCustomizedTask(obj.TPId, "A", obj.TPStatus);
                        }
                        else
                        {
                            LocalCheckPlan(obj.TPId, "A");
                        }

                    }
                    else
                    {
                        var action = await _commonFun.ShowActionSheet("开始检查", "结束检查");//, "取消任务");

                        if (action == "开始检查")
                        {
                            if (obj.TPType == "C")// 自定义任务
                            {
                                //CheckCustomizedTask(obj.TPId, "S", obj.TPStatus);
                                LocalCheckCustomizedTask(obj.TPId, "S", obj.TPStatus);
                            }
                            else
                            {
                                LocalCheckStartPlan(obj.TPId);
                            }
                        }
                        else if (action == "结束检查")
                        {
                            if (obj.TPType == "C")// 自定义任务
                            {
                                //CheckCustomizedTask(obj.TPId, "E", obj.TPStatus);
                                LocalCheckCustomizedTask(obj.TPId, "E", obj.TPStatus);
                            }
                            else
                            {
                                LocalCheckEndPlan(obj.TPId);
                            }

                        }
                        else if (action == "取消任务")
                        {
                            if (await _commonFun.Confirm("确定取消该任务吗?"))
                            {
                                if (obj.TPType == "C")// 自定义任务
                                {
                                    LocalCheckCustomizedTask(obj.TPId, "C", obj.TPStatus);
                                }
                                else
                                {
                                    LocalClosePlanTask(obj.TPId);
                                }
                            }
                        }
                    }

                }
                else
                {
                    if (obj.TPType == "C")
                    {
                        CheckCustomizedTask(obj.TPId, "A", obj.TPStatus);
                    }
                    else
                    {
                        CheckPlan(obj.TPId, "");
                    }
                }
            });

            MessagingCenter.Subscribe<string>(this, "SearchTaskList", (disId) =>
            {
                _isChecking = true;
                _statu = "N";

                GetLocalPlans(Convert.ToInt32(disId));
            });
        }

        private List<TaskOfPlanDto> _taskList;
        public List<TaskOfPlanDto> TaskList
        {
            get { return _taskList; }
            set { SetProperty(ref _taskList, value); }
        }

        private TaskOfPlanDto _plan;
        public TaskOfPlanDto Plan
        {
            get { return _plan; }
            set { SetProperty(ref _plan, value); }
        }

        private string _startTime = "";
        private string _endTime = "";

        private async void GetPlans(int id, string startTime = "", string endTime = "", string statu = "", string sourceTypeCode = "D")
        {
            try
            {
                _commonFun.ShowLoading("查询中...");
                var result = await _tourService.GetPlans(id, startTime, endTime, statu, sourceTypeCode);
                if (null != result && result.ResultCode == Module.ResultType.Success)
                {
                    TaskList = JsonConvert.DeserializeObject<List<TaskOfPlanDto>>(result.Body);
                }
                if (TaskList != null && TaskList.Count == 0)
                {
                    _commonFun.ShowToast("没有数据");
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

        private async void GetLocalPlans(int id)
        {
            try
            {
                _commonFun.ShowLoading("查询中...");
                TaskList = await _localScoreService.GetTaskOfPlans(id);
                foreach (var item in TaskList)
                {
                    if (item.SourceType == "" || item.SourceType == null)
                    {
                        item.SourceType = "巡视检核";
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

        public void Init(int disId, string startDate, string endDate, bool isCheck, string sourceTypeCode)
        {
            _startTime = startDate;
            _endTime = endDate;
            // only view the record
            //if (CommonContext.Account.UserType == "S" || CommonContext.Account.UserType == "D")
            //{
            //    //服务商 search done
            //    _statu = "F";
            //    _isChecking = false;
            //}
            ////else if (CommonContext.Account.UserType == "Z")
            //else
            //{
            //    _statu = "F";
            //    _isChecking = false;
            //}

            MessagingCenter.Send<string>("search", "ComeFromSeachYN");

            //只查看结束的任务
            _statu = "F";
            _isChecking = isCheck;
            GetPlans(disId, startDate, endDate, _statu, sourceTypeCode);
        }

        private RelayCommand<string> _startCheckCommand;
        public RelayCommand<string> StartCheckCommand
        {
            get
            {
                return _startCheckCommand
                    ?? (_startCheckCommand = new RelayCommand<string>(CheckStartPlan));
            }
        }

        private void CheckStartPlan(string taskId)
        {
            CheckPlan(taskId, "S");
        }
        private void LocalCheckStartPlan(string taskId)
        {
            LocalCheckPlan(taskId, "S");
        }

        private RelayCommand<string> _finishCheckCommand;
        public RelayCommand<string> FinishCheckCommand
        {
            get
            {
                return _finishCheckCommand
                    ?? (_finishCheckCommand = new RelayCommand<string>(CheckEndPlan));
            }
        }

        private void CheckEndPlan(string taskId)
        {
            CheckPlan(taskId, "E");
        }
        private void LocalCheckEndPlan(string taskId)
        {
            LocalCheckPlan(taskId, "E");
        }
        private void LocalClosePlanTask(string taskId)
        {
            LocalCheckPlan(taskId, "C");
        }

        private async void CheckPlan(string taskId, string operation)
        {
            try
            {
                //_isChecking = false;
                _commonFun.ShowLoading("查询中...");
                var result = await _tourService.CheckPlan(taskId, operation);
                if (null != result)
                {
                    if (result.ResultCode == Module.ResultType.Success)
                    {
                        var taskList = JsonConvert.DeserializeObject<List<ItemOfTaskDto>>(result.Body);

                        if (taskList != null && taskList.Count > 0)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                if (operation != "E")
                                {
                                    await Navigation.PushAsync(ViewFactory.CreatePage<SystemListViewModel, SystemListPage>() as Page, true);
                                    //await Navigation.PushAsync(new SystemListPage(taskId.ToString()), true);
                                    MessagingCenter.Send<List<ItemOfTaskDto>>(taskList, "SendSystemList");
                                }
                                else
                                {
                                    //刷新页面
                                    GetPlans(_disId, _startTime, _endTime, _statu);
                                }
                            });
                        }
                        else
                        {
                            _commonFun.AlertLongText(result.Msg);
                        }
                    }
                    else
                    {
                        _commonFun.AlertLongText(result.Msg);
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
                _commonFun.AlertLongText("检查异常,请重试");
            }
            finally
            {
                //_isChecking = true;
                _commonFun.HideLoading();
            }
        }

        private async void LocalCheckPlan(string taskId, string operation)
        {
            try
            {
                _commonFun.ShowLoading("查询中...");
                List<LoaclItemOfTaskDto> taskList = new List<LoaclItemOfTaskDto>();
                try
                {
                    taskList = await _localScoreService.SearchTaskItem(taskId, operation);
                }
                catch (Exception ex)
                {
                    _commonFun.HideLoading();
                    _commonFun.AlertLongText(ex.Message);
                }

                if (taskList != null && taskList.Count > 0)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (operation != "E" && operation != "C")
                        {
                            await Navigation.PushAsync(ViewFactory.CreatePage<LocalSystemListViewModel, LocalSystemListPage>() as Page, true);
                            MessagingCenter.Send<List<LoaclItemOfTaskDto>>(taskList, "LocalSendSystemList");
                        }
                        else
                        {
                            //刷新页面
                            //GetPlans(_disId, _startTime, _endTime, _statu);
                            GetLocalPlans(_disId);
                        }
                    });
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

        private async void CheckCustomizedTask(string taskId, string operation, string tPStatus)
        {
            if (_commonHelper.IsNetWorkConnected() == true)
            {
                try
                {
                    _commonFun.ShowLoading("查询中...");
                    var result = await _tourService.GetCustomizedTaskByTaskId(taskId, operation);
                    if (result.ResultCode == Module.ResultType.Success)
                    {
                        List<CustomizedTaskDto> dtoList = CommonHelper.DecodeString<List<CustomizedTaskDto>>(result.Body);
                        if (dtoList != null && dtoList.Count > 0)
                        {
                            if (operation != "E")
                            {
                                CustomizedTaskDto dto = dtoList.FirstOrDefault();
                                await Navigation.PushAsync<CustomizedTaskViewModel>((vm, v) => vm.Init(dto, tPStatus), true);
                            }
                            else
                            {
                                GetPlans(_disId, _startTime, _endTime, _statu);
                            }

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

        private async void LocalCheckCustomizedTask(string taskId, string operation, string tPStatus)
        {

            try
            {
                _commonFun.ShowLoading("查询中...");
                List<CustomizedTaskDto> result = new List<CustomizedTaskDto>();
                try
                {
                    result = await _localScoreService.LocalGetCustomizedTaskByTaskId(taskId, operation);
                }
                catch (Exception ex)
                {
                    _commonFun.HideLoading();
                    _commonFun.AlertLongText(ex.Message);
                }
                if (result != null && result.Count > 0)
                {
                    if (operation != "E" && operation != "C")
                    {
                        CustomizedTaskDto dto = result.FirstOrDefault();
                        await Navigation.PushAsync<CustomizedTaskViewModel>((vm, v) => vm.Init(dto, tPStatus), true);
                    }
                    else
                    {
                        GetLocalPlans(_disId);
                    }

                }
                else
                {
                    _commonFun.HideLoading();
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


        #region UploadImage
        private RelayCommand _ploadScoreImageCommand;
        public RelayCommand UploadScoreImageCommand
        {
            get
            {
                return _ploadScoreImageCommand
                    ?? (_ploadScoreImageCommand = new RelayCommand(UploadScoreImage));
            }
        }

        private async void UploadScoreImage()
        {
            if (_commonHelper.IsNetWorkConnected() == true)
            {
                try
                {
                    if (!_localScoreService.CheckTaskStatus(_disId))
                    {
                        _commonFun.AlertLongText("请全部结束检查后，再上传数据");
                        return;
                    }
                    string size = _commonFun.GetFilesSizeOfUpload();
                    if (await _commonFun.Confirm("上传文件大小为:" + size + " 以上, 建议在Wifi环境下进行。确定要上传吗？"))
                    {
                        try
                        {
                            _commonFun.ShowLoading("上传中...");
                            await _commonFun.UploadLocalFileToServer();
                            await _remoteService.UploadScoreInfo(0);
                            await MasterDataDownloadHelper.DownloadData("1");
                            GetLocalPlans(_disId);
                            _commonFun.HideLoading();
                            _commonFun.AlertLongText("上传完毕");
                        }
                        catch (Exception ex)
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
                catch (Exception ex)
                {
                    _commonFun.AlertLongText("上传失败，请重试");
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


        private RelayCommand _addCustImproveCommand;
        public RelayCommand AddCustImproveCommand
        {
            get
            {
                return _addCustImproveCommand
                    ?? (_addCustImproveCommand = new RelayCommand(AddCustImprove));
            }
        }
        private async void AddCustImprove()
        {
            await Navigation.PushAsync<CustImproveViewModel>((vm, v) => vm.Init(_disId), true);
        }

        #endregion
    }
}
