using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Pages.ShopfrontPatrol;
using MaxInsight.Mobile.Services.RemoteService;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.ShopfrontPatrolModel
{
    public class LocalSystemListViewModel : ViewModel
    {
        private readonly ILocalScoreService _localScoreService;
        private bool _refresh { get; set; } = true;
        public RelayCommand<LoaclItemOfTaskDto> ItemTappedCommand { get; set; }
        private List<LoaclItemOfTaskDto> _systemList;
        public List<LoaclItemOfTaskDto> SystemList
        {
            get { return _systemList; }
            set { SetProperty(ref _systemList, value); }
        }
        private LoaclItemOfTaskDto _systemDto;
        private ICommonFun _commonFun;

        private string _taskId;// { get; set; }
        public string TaskId
        {
            get { return _taskId; }
            set { SetProperty(ref _taskId, value); }
        }
        public LoaclItemOfTaskDto SystemDto
        {
            get { return _systemDto; }
            set { SetProperty(ref _systemDto, value); }
        }
        public LocalSystemListViewModel()
        {
            _localScoreService = Resolver.Resolve<ILocalScoreService>();
            _commonFun = Resolver.Resolve<ICommonFun>();


            MessagingCenter.Subscribe<CommonMessage>(this, "LocalResetTaskID", (obj) =>
            {
                if (obj.TaskID =="-1")
                {
                    _refresh = false;
                }
                else
                {
                    _refresh = true;
                }
            });

            MessagingCenter.Subscribe<List<LoaclItemOfTaskDto>>(this, "LocalSendSystemList", (obj) =>
            {
                _taskId = obj.FirstOrDefault().TPId.ToString();
                Init(obj);
            });

            MessagingCenter.Subscribe<LocalSystemListPage>(this, "LocalRefreshSystem", (obj) =>
            {
                if (_refresh)
                {
                    RefreshPage(TaskId.ToString());
                }
            });

            ItemTappedCommand = new RelayCommand<LoaclItemOfTaskDto>(TappedCommand);
        }
        public void Init(List<LoaclItemOfTaskDto> list)
        {
            SystemList = list;
        }
        public async void TappedCommand(LoaclItemOfTaskDto dto)
        {
            try
            {
                if (SystemList == null || SystemList.Count == 0)
                {
                    return;
                }
                if (null != SystemDto)
                {
                    int seq = SystemDto.SeqNo;
                    LoaclItemOfTaskDto currentDto = SystemList.Where(p => p.SeqNo == seq
                                                                && p.TPId == SystemDto.TPId
                                                                && p.TIId == SystemDto.TIId).FirstOrDefault();
                    try
                    {
                        await Navigation.PushAsync<LocalRegistScoreViewModel>((vm, v) => vm.Init(SystemList, currentDto), true);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private async void RefreshPage(string taskId)
        {
            try
            {
                _commonFun.ShowLoading("查询中...");
                var taskList = await _localScoreService.SearchTaskItem(taskId, "");


                if (taskList != null && taskList.Count > 0)
                {
                    Init(taskList);
                }
                else
                {
                    _commonFun.AlertLongText("没有数据");
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
    }
}
