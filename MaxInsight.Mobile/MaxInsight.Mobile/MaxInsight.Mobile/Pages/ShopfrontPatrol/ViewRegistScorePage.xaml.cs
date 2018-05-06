using System;
using System.Collections.Generic;

using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace MaxInsight.Mobile
{
	public partial class ViewRegistScorePage : ContentPage
	{
		public ViewRegistScorePage()
		{
			InitializeComponent();

			Title = "打分结果查询";

			picImage.GestureRecognizers.Add(new TapGestureRecognizer()
			{
				Command = new Command(ShowOrHidePartTwo),
				NumberOfTapsRequired = 1
			});

			checkImage.GestureRecognizers.Add(new TapGestureRecognizer()
			{
				Command = new Command(ShowOrHidePartThree),
				NumberOfTapsRequired = 1
			});

			stackPic.IsVisible = false;
			stackCheck.IsVisible = false;

			if (Device.OS == TargetPlatform.Android)
			{
				pre.WidthRequest = 70;
				next.WidthRequest = 70;
				jump.WidthRequest = 70;

				pre.HeightRequest = 38;
				next.HeightRequest = 38;
				jump.HeightRequest = 38;
			}
			else
			{
				pre.WidthRequest = 50;
				next.WidthRequest = 50;
				jump.WidthRequest = 50;

				pre.HeightRequest = 30;
				next.HeightRequest = 30;
				jump.HeightRequest = 30;
			}
		}

		void ShowOrHidePartTwo()
		{
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

		void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
			if (e == null) return;
			MessagingCenter.Send<PictureStandard>(e.Item as PictureStandard, "ViewRegistScoreItemTapped");
		}

		public void OnPreviewPlanAttechment(object sender, EventArgs e)
		{
            ImageButton img = sender as ImageButton;
            //Button img = sender as Button;
            MessagingCenter.Send<PictureStandard>(img.CommandParameter as PictureStandard, "ViewPreviewPlanAttechment");
		}
	}
}
