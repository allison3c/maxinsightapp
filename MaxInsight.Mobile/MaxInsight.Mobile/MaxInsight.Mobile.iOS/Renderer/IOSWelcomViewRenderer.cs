using System;
using CoreGraphics;
using Foundation;
using MaxInsight.Mobile;
using MaxInsight.Mobile.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(IOSWelcomView), typeof(IOSWelcomViewRenderer))]
namespace MaxInsight.Mobile.iOS
{
	public class IOSWelcomViewRenderer : ViewRenderer<IOSWelcomView, UIView>
	{
		UIScrollView scrollView;
		UIPageControl pageControl;
		nfloat screenWidth;
		nfloat screenHeight;
		nint currentIndex;
		int imageCount;// = 3;
		string[] images;
		UIButton btnGoMain;
		
		protected override void OnElementChanged(ElementChangedEventArgs<IOSWelcomView> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
			{
				if (e.OldElement == null)
				{
					screenWidth = UIScreen.MainScreen.Bounds.Width;
					screenHeight = UIScreen.MainScreen.Bounds.Height;

					InitImages();
					AddScrollView();
					AddImageViews();
					AddPageControl();
					AddGoMainButton();
					SetDefaultImage();
				}
				else
				{
					scrollView.DecelerationEnded -= ScrollView_DecelerationEnded; ;
					scrollView.DraggingStarted -= ScrollView_DraggingStarted; ;
					scrollView.DraggingEnded -= ScrollView_DraggingEnded;

					scrollView.Dispose();
					pageControl.Dispose();
					this.Dispose();
				}
			}
		}

		void InitImages()
		{
			images = Element != null ? Element.Images : new string[] { "guide_0.jpg", "guide_1.jpg", "guide_2.jpg" };
			imageCount = images.Length;
		}

		void AddScrollView()
		{
			scrollView = new UIScrollView(UIScreen.MainScreen.Bounds);
			//去掉滚动条
			scrollView.ShowsHorizontalScrollIndicator = false;
			scrollView.ShowsVerticalScrollIndicator = false;
			//设置分页
			scrollView.PagingEnabled = true;
			//设置contentSize 
			scrollView.ContentSize = new CGSize(screenWidth * imageCount, screenHeight);

			scrollView.DecelerationEnded += ScrollView_DecelerationEnded; ;
			scrollView.DraggingStarted += ScrollView_DraggingStarted; ;
			scrollView.DraggingEnded += ScrollView_DraggingEnded;

			AddSubview(scrollView);
		}

		void AddImageViews()
		{
			for (int i = 0; i < imageCount; i++)
			{
				var imageView = new UIImageView(new CGRect(screenWidth * i, 0, screenWidth, screenHeight));
				imageView.Image = UIImage.FromFile(images[i]);
				imageView.ContentMode = UIViewContentMode.ScaleToFill;
				scrollView.AddSubview(imageView);
			}
		}

		void AddPageControl()
		{
			pageControl = new UIPageControl() { 
				AutoresizingMask = UIViewAutoresizing.All,
				ContentMode = UIViewContentMode.ScaleToFill
			};
			//CGSize size = pageControl.SizeForNumberOfPages(imageCount);

			//pageControl.Frame = new CGRect(screenWidth / 2, Frame.Size.Height - 30, size.Width, size.Height);

			//pageControl.Bounds = new CGRect(0, 0, size.Width, size.Height);
			//pageControl.Center = new CGPoint(screenWidth / 2, screenHeight - 30);
			//pageControl.CurrentPageIndicatorTintColor = Color.FromHex("#ecf0f1").ToUIColor();
			//pageControl.PageIndicatorTintColor = Color.FromHex("#6281AB").ToUIColor();// UIColor.Gray;
			//pageControl.Pages = imageCount;

			AddSubview(pageControl);
		}

		public override void Draw(CGRect rect)
		{
			pageControl.Frame = new CGRect(rect.Left, rect.Height - 40, rect.Width, 40);
			pageControl.CurrentPageIndicatorTintColor = Color.FromHex("#ecf0f1").ToUIColor();
			pageControl.PageIndicatorTintColor = Color.FromHex("#6281AB").ToUIColor();// UIColor.Gray;
			pageControl.Pages = imageCount;
			base.Draw(rect);
		}

		void AddGoMainButton() {
			btnGoMain = new UIButton();
			btnGoMain.Bounds = new CGRect(0, 0, screenWidth / 2, 40);
			btnGoMain.Center = new CGPoint(screenWidth / 2, screenHeight - 80);
			//btnGoMain.BackgroundColor = UIColor.Red;
			btnGoMain.Layer.MasksToBounds = true;
			btnGoMain.Layer.CornerRadius = 5;
			btnGoMain.Layer.BorderWidth = 1;
			btnGoMain.Layer.BorderColor = Color.FromHex("#ecf0f1").ToCGColor();

			btnGoMain.SetTitle("立刻进入", UIControlState.Normal);
			btnGoMain.SetTitleColor(Color.FromHex("#ecf0f1").ToUIColor(), UIControlState.Normal);

			btnGoMain.Hidden = true;

			btnGoMain.TouchUpInside += BtnGoMain_TouchUp;

			AddSubview(btnGoMain);
		}

		void BtnGoMain_TouchUp(object sender, EventArgs e) {
			Element.LastSwiper();
		}

		void SetDefaultImage()
		{
			currentIndex = 0;
			pageControl.CurrentPage = currentIndex;
		}

		void ScrollView_DecelerationEnded(object sender, EventArgs e)
		{
			CGPoint offset = scrollView.ContentOffset;
			currentIndex = (int)(offset.X / screenWidth);
			if (currentIndex == imageCount - 1)
			{
				btnGoMain.Hidden = false;
			}
			else
			{
				btnGoMain.Hidden = true;
			}
			scrollView.SetContentOffset(new CGPoint(offset.X, 0), true);
			pageControl.CurrentPage = currentIndex;
		}

		void ScrollView_DraggingStarted(object sender, EventArgs e)
		{
		}

		void ScrollView_DraggingEnded(object sender, DraggingEventArgs e)
		{
		}
	}
}
