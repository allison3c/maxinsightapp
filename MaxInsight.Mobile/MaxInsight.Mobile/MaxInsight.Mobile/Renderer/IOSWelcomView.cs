using System;
using Xamarin.Forms;

namespace MaxInsight.Mobile
{
	public class IOSWelcomView : View
	{
		public event EventHandler OnLastSwiper;

		public static readonly BindableProperty ImagesProperty =
			BindableProperty.Create("Images", typeof(string[]), typeof(IOSWelcomView), new string[] { });

		public string[] Images
		{
			get { return (string[])GetValue(ImagesProperty); }
			set { SetValue(ImagesProperty, value); }
		}

		//public static readonly BindableProperty BannerHeightPageProperty =
		//	BindableProperty.Create("BannerHeight", typeof(int), typeof(IOSWelcomView), 0);
		////自定义height
		//public int BannerHeight
		//{
		//	get { return (int)GetValue(BannerHeightPageProperty); }
		//	set { SetValue(BannerHeightPageProperty, value); }
		//}

		//public static readonly BindableProperty IsAutoScrollProperty =
		//	BindableProperty.Create("IsAutoScroll", typeof(bool), typeof(IOSWelcomView), false);
		////是否自动滚动
		//public bool IsAutoScroll
		//{
		//	get { return (bool)GetValue(IsAutoScrollProperty); }
		//	set { SetValue(IsAutoScrollProperty, value); }
		//}

		public void LastSwiper()
		{
			var lastSwiper = OnLastSwiper;
			if (lastSwiper != null)
				lastSwiper(this, EventArgs.Empty);
		}
	}
}
