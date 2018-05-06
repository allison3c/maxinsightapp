using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using XLabs.Forms.Controls;
using XLabs.Ioc;
using XLabs.Platform.Device;

namespace MaxInsight.Mobile
{
	public partial class RegistScorePage : ContentPage
	{
		public RegistScorePage()
		{
			InitializeComponent();

			Title = "得分登记";

			picImage.GestureRecognizers.Add(new TapGestureRecognizer() { 
				Command = new Command(ShowOrHidePartTwo),
				NumberOfTapsRequired = 1
			});

			checkImage.GestureRecognizers.Add(new TapGestureRecognizer()
			{
				Command = new Command(ShowOrHidePartThree),
				NumberOfTapsRequired = 1
			});

			//previewImage.GestureRecognizers.Add(new TapGestureRecognizer()
			//{
			//	Command = new Command(PreviewImage),
			//	NumberOfTapsRequired = 1
			//});

			//deleteImage.GestureRecognizers.Add(new TapGestureRecognizer()
			//{
			//	Command = new Command(DeleteImage),
			//	NumberOfTapsRequired = 1
			//});
			stackPic.IsVisible = false;
			stackCheck.IsVisible = false;

			//var device = Resolver.Resolve<IDevice>();

			//var width = device.Display.Width / device.Display.Scale;
			//var height = device.Display.Height / device.Display.Scale;

			if (Device.OS == TargetPlatform.Android)
			{
				pre.WidthRequest = 70;
				next.WidthRequest = 70;
				ingnor.WidthRequest = 70;
				jump.WidthRequest = 70;

				pre.HeightRequest = 38;
				next.HeightRequest = 38;
				ingnor.HeightRequest = 38;
				jump.HeightRequest = 38;
			}
			else
			{
				pre.WidthRequest = 50;
				next.WidthRequest = 50;
				ingnor.WidthRequest = 50;
				jump.WidthRequest = 50;

				pre.HeightRequest = 30;
				next.HeightRequest = 30;
				ingnor.HeightRequest = 30;
				jump.HeightRequest = 30;
			}
		}

		//void PreviewImage() {
		//	MessagingCenter.Send<RegistScorePage>(this, "PreviewImage");
		//}

		//void DeleteImage()
		//{
		//	MessagingCenter.Send<RegistScorePage>(this, "DeleteImage");
		//}

		void ShowOrHidePartTwo() {
			if (stackPic.IsVisible)
			{
				stackPic.IsVisible = false;
				oneImage.Source = ImageSource.FromFile("icon_hide");
			}
			else
			{
				stackPic.IsVisible = true;
				oneImage.Source = ImageSource.FromFile("icon_show");
			}
		}

		void ShowOrHidePartThree()
		{
			if (stackCheck.IsVisible)
			{
				stackCheck.IsVisible = false;
				twoImage.Source = ImageSource.FromFile("icon_hide");
			}
			else
			{
				stackCheck.IsVisible = true;
				twoImage.Source = ImageSource.FromFile("icon_show");
			}
		}

		void Handle_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
		{
			//告知viewmodel checkbox发生了改变
			MessagingCenter.Send<RegistScorePage>(this, "CheckBoxChanged");
		}

		protected override void OnAppearing()
		{
			//MessagingCenter.Subscribe<RegistScorePage>(this, "PopBack", (obj) => { 
			//	Device.BeginInvokeOnMainThread(() =>
			//	{
			//		Navigation.PopAsync(true);
			//	});
			//});
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			//MessagingCenter.Unsubscribe<RegistScorePage>(this, "PopBack");
			//stackPic.IsVisible = false;
			//stackCheck.IsVisible = false;
			MessagingCenter.Send<SystemListPage>(new SystemListPage(), "RefreshSystem");
			base.OnDisappearing();
		}

		//void Handle_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		//{
		//	//告知viewmodel checkbox发生了改变
		//	//MessagingCenter.Send<RegistScorePage>(this, "CheckBoxChanged");
		//}

		void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
			if (e == null) return;
			MessagingCenter.Send<PictureStandard>(e.Item as PictureStandard, "RegistScoreItemTapped");
		}

		//public void OnDelete(object sender, EventArgs e)
		//{
		//	var mi = ((MenuItem)sender);
		//	var picStander = mi.CommandParameter as PictureStandard;

		//	if (picStander == null)
		//	{
		//		//DisplayAlert("More Context Action", "", "OK");
		//		return;
		//	}

		//	MessagingCenter.Send<PictureStandard>(picStander, "DeleteStanderImage");
		//}

		protected override bool OnBackButtonPressed()
		{
			return false;
		}

		public void OnPreviewPlanAttechment(object sender, EventArgs e)
		{
            ImageButton img = sender as ImageButton;
            //Button img = sender as Button;
            MessagingCenter.Send<PictureStandard>(img.CommandParameter as PictureStandard, "PreviewPlanAttechment");
		}

		public void OnDeletePlanAttechment(object sender, EventArgs e)
		{
            ImageButton img = sender as ImageButton;
            //Button img = sender as Button;
            MessagingCenter.Send<PictureStandard>(img.CommandParameter as PictureStandard, "DeletePlanAttechment");
		}

		public void OnPreviewLossImage(object sender, EventArgs e)
		{
			ImageButton img = sender as ImageButton;
			MessagingCenter.Send<StandardPic>(img.CommandParameter as StandardPic, "PreviewLossImage");
		}

		public void OnDeleteLossImage(object sender, EventArgs e)
		{
            ImageButton img = sender as ImageButton;
            //Button img = sender as Button;
            MessagingCenter.Send<StandardPic>(img.CommandParameter as StandardPic, "DeleteLossImage");
		}

		//public void OnPreviewImage(object sender, EventArgs e)
		//{
		//	PreviewImage();
		//}

		//public void OnDeleteImage(object sender, EventArgs e)
		//{
		//	DeleteImage();
		//}
	}
}
