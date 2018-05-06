using System;
using System.Threading.Tasks;
using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Services.Tour;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile
{
    public class UpdatePopViewModel : ViewModel
	{
		private readonly ITourService _tourService;
		private readonly ICommonFun _commonFun;

		public UpdatePopViewModel()
		{
			_tourService = Resolver.Resolve<ITourService>();
			_commonFun = Resolver.Resolve<ICommonFun>();
		}

		private RelayCommand _closePopCommand;
		public RelayCommand ClosePopCommand { 
			get { return _closePopCommand ?? (_closePopCommand = new RelayCommand(ClosePop));}
		}

		private async void ClosePop()
		{
			await PopupNavigation.PopAsync();
		}

		private RelayCommand _saveCommand;
		public RelayCommand SaveCommand
		{

			get
			{
				return _saveCommand ?? (_saveCommand = new RelayCommand(Save));
			}
		}

		private ItemOfTaskDto _saveDto;
		public ItemOfTaskDto SaveDto { 
			get { return _saveDto; }
			set { SetProperty(ref _saveDto, value); }
		}

		private async void Save()
		{
			await Task.Run(() =>
			{
				SaveSystem(SaveDto);
			});
		}

		private async void SaveSystem(ItemOfTaskDto dto)
		{
			try
			{
				_commonFun.ShowLoading("保存中...");

				var result = await _tourService.SaveSystemList(dto);
				if (result != null && result.ResultCode == Module.ResultType.Success)
				{
					Device.BeginInvokeOnMainThread(() =>
					{
						_commonFun.Alert("保存成功", "");
						ClosePop();
						
					});
				}
			}
			catch (OperationCanceledException)
			{
				_commonFun.HideLoading();
				_commonFun.AlertLongText("保存超时,请重试");
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
	}
}
