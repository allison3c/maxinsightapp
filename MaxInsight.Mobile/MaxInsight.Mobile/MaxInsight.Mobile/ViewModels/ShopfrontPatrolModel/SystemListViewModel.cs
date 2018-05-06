using System;
using System.Collections.Generic;
using System.Linq;
using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Services.Tour;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile
{
    public class SystemListViewModel : ViewModel
	{
		private readonly ITourService _tourService;
		private readonly ICommonFun _commonFun;

		private bool _refresh { get; set; } = true;

		private string _taskId;// { get; set; }
		public string TaskId { 
			get { return _taskId; }
			set { SetProperty(ref _taskId, value); }
		}

		private string _Id;// { get; set; }
		public string Id
		{
			get { return _Id; }
			set { SetProperty(ref _Id, value); }
		}

		private bool _isSetVisible;
		public bool IsSetVisible { 
			get { return _isSetVisible; }
			set { SetProperty(ref _isSetVisible, value); }
		}

		public SystemListViewModel()
		{
			_tourService = Resolver.Resolve<ITourService>();
			_commonFun = Resolver.Resolve<ICommonFun>();

            if (CommonContext.Account.UserType == "S" || CommonContext.Account.UserType == "D")
            {
                //cannot display the set button
                IsSetVisible = false;
            }
            else {
                IsSetVisible = true;
            }

			MessagingCenter.Subscribe<List<ItemOfTaskDto>>(this, "SendSystemList", (obj) =>
			{
                _taskId = obj.FirstOrDefault().TPId.ToString();
				Init(obj);
			});

			MessagingCenter.Subscribe<CommonMessage>(this, "ResetTaskID", (obj) =>
			{
				if (obj.TaskID == "-1")
				{
					_refresh = false;
				}
				else
				{
					_refresh = true;
				}
			});

			MessagingCenter.Subscribe<SystemListPage>(this, "RefreshSystem", (obj) => { 
				if (_refresh)
				{
					RefreshPage(TaskId);
				}
			});

			MessagingCenter.Subscribe<ItemOfTaskDto>(this, "GoRegisterPage", (obj) => {
				GoRegistScore(obj);
			});
		}

		//private RelayCommand<ItemOfTaskDto> _itemTappedCommand;
		//public RelayCommand<ItemOfTaskDto> ItemTappedCommand
		//{
		//	get
		//	{
		//		return _itemTappedCommand ?? (_itemTappedCommand = new RelayCommand<ItemOfTaskDto>(GoRegistScore));
		//	}
		//}

		private RelayCommand<int> _updateCommand;
		public RelayCommand<int> UpdateCommand
		{
			get
			{
				return _updateCommand ?? (_updateCommand = new RelayCommand<int>(OpenUpdatePop));
			}
		}


		private ItemOfTaskDto _systemDto;
		public ItemOfTaskDto SystemDto
		{
			get { return _systemDto; }
			set { SetProperty(ref _systemDto, value); }
		}

		private List<ItemOfTaskDto> _systemList;
		public List<ItemOfTaskDto> SystemList
		{
			get { return _systemList; }
			set { SetProperty(ref _systemList, value); }
		}

		private bool _isClicked;
		public bool IsClicked 
		{ 
			get { return _isClicked; }
			set { SetProperty(ref _isClicked, value); }
		}

		private void OpenUpdatePop(int seqNo) { 
			if (!IsClicked)
			{
				_commonFun.AlertLongText("已结束的任务不能再设置");
				return;
			}

			Device.BeginInvokeOnMainThread(async () =>
			{
				await PopupNavigation.PushAsync(new UpdatePopPage(SystemList.FirstOrDefault(p => p.SeqNo == seqNo)), true);
			});
		}

		private void GoRegistScore(ItemOfTaskDto dto)
		{
            try
            {
                if (SystemList == null || SystemList.Count == 0)
                {
                    return;
                }

                if (null != dto)
                {
                    int seq = dto.SeqNo;
                    ItemOfTaskDto currentDto = SystemList.Where(p => p.SeqNo == seq
                                                                && p.TPId == dto.TPId
                                                                && p.TIId == dto.TIId).FirstOrDefault();
                    //int index = SystemList.IndexOf(currentDto);

                    //if (index == -1)
                    //{
                    //    _commonFun.AlertLongText("获取索引失败");
                    //    return;
                    //}

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await Navigation.PushAsync<RegistScoreViewModel>(true);
                        //SystemList.FirstOrDefault().CurrentIndex = index;
                        //MessagingCenter.Send<List<ItemOfTaskDto>>(SystemList, "PassSystemList");
                        try
                        {
                            if (currentDto.IsClicked)
                            {
                                await Navigation.PushAsync<RegistScoreViewModel>((vm, v) => vm.Init(SystemList, currentDto), true);
                            }
                            else
                            {
                                await Navigation.PushAsync<ViewRegistScoreViewModel>((vm, v) => vm.Init(SystemList, currentDto), true);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    });
                }
            }
            catch (Exception)
            {
            }
		}

		public void Init(List<ItemOfTaskDto> list)
		{
			SystemList = list;
			if (SystemList.FirstOrDefault().IsClicked)
			{
				IsClicked = true;
			}
			else
			{
				IsClicked = false;
			}
		}

		public void PopBack() { 
		
		}

		private async void RefreshPage(string taskId)
		{
			try
			{
				_commonFun.ShowLoading("查询中...");
				var result = await _tourService.CheckPlan(taskId, "");
				if (null != result && result.ResultCode == Module.ResultType.Success)
				{
					var taskList = JsonConvert.DeserializeObject<List<ItemOfTaskDto>>(result.Body);

					if (taskList != null && taskList.Count > 0)
					{
						Init(taskList);
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
				_commonFun.AlertLongText("查询异常,请重试");
			}
			finally
			{
				_commonFun.HideLoading();
			}
		}
	}
}
