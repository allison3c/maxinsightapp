using System;
using System.Collections.Generic;
using MaxInsight.Mobile.Pages;
using MaxInsight.Mobile.ViewModels;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;

namespace MaxInsight.Mobile
{
	public partial class IOSWelcomPage : ContentPage
	{
		public IOSWelcomPage()
		{
			InitializeComponent();

			welcomScroll.OnLastSwiper += (sender, e) => {
				Device.BeginInvokeOnMainThread(() =>
				{
					Application.Current.MainPage = ViewFactory.CreatePage<LoginViewModel, LoginPage>() as Page;
				});
			};
		}
	}
}
