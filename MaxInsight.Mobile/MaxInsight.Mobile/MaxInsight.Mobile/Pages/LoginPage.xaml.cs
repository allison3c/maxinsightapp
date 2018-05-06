using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using Xamarin.Forms;
using XLabs.Ioc;

namespace MaxInsight.Mobile.Pages
{
    public partial class LoginPage : ContentPage
    {
		ICommonFun _commonFun;
        IOSVersionInfoDto info = null;
        public LoginPage()
        {
            InitializeComponent();
			_commonFun = Resolver.Resolve<ICommonFun>();
        }

		protected async override void OnAppearing()
		{
			if (_commonFun != null)
			{
				var name = _commonFun.GetCach(CommonContext.USERNAMEKEY);

				if (!string.IsNullOrEmpty(name))
				{
					userName.Text = name;
				}
            }

            info = await App.CheckUpdate();
            if (info != null && !string.IsNullOrEmpty(info.appKey))
            {
                UpdateNewVersion();
            }

            base.OnAppearing();
        }
        private async void UpdateNewVersion()
        {
            if (await _commonFun.Confirm("新的版本已发布，是否更新?"))
            {
                MessagingCenter.Send<IOSVersionInfoDto>(info, "UpdateApp");
            }
        }
    }
}
