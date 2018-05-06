using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using MaxInsight.Mobile;
using MaxInsight.Mobile.Droid.Renders;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ImageGallery), typeof(ImageGalleryRenderer))]
namespace MaxInsight.Mobile.Droid.Renders
{
	public class ImageGalleryRenderer : ViewRenderer<ImageGallery, ViewGroup>
	{

		HorizontalScrollView hScrollView;
		int screenWidth;
		int screenHeight;
		int downX;
		LinearLayout linearLayout;
		public ObservableCollection<string> Images { get; set; }

		int currentPage = 0;
		Handler handler;
		Runnable runnable;

		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<ImageGallery> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement == null)
			{
				//SetNativeControl(_gallery);
				screenWidth = (int)Resources.DisplayMetrics.WidthPixels + 30;
				screenHeight = (int)Resources.DisplayMetrics.HeightPixels / 4;
				//if (Element.HeightRequest != 0)
				//{
				//	screenHeight = (int)Element.HeightRequest;
				//}
				//else {
				//	screenHeight = (int)Resources.DisplayMetrics.HeightPixels;
				//}

				Images = e.NewElement.ItemsSource as ObservableCollection<string> ?? new ObservableCollection<string>();

				handler = new Handler();
				runnable = new Runnable(Run);

				InitHorizotalScrollView();
			}
			//Bind(e.NewElement);
		}

		private void InitHorizotalScrollView() { 
			hScrollView = new HorizontalScrollView(this.Context);
			hScrollView.LayoutParameters = new LayoutParams(screenWidth, screenHeight);
			hScrollView.HorizontalScrollBarEnabled = false;

			hScrollView.Touch += (object sender, TouchEventArgs e) =>
			{
				switch (e.Event.Action)
				{
					case Android.Views.MotionEventActions.Down:
						downX = (int)e.Event.GetX();
						break;
					case Android.Views.MotionEventActions.Up:
					case Android.Views.MotionEventActions.Cancel:
						if (Java.Lang.Math.Abs(e.Event.GetX() - downX) > screenWidth / 4)
						{
							if (e.Event.GetX() - downX > 0)
							{
								SmoothScrollToPrePage();
							}
							else {
								SmoothScrollToNextPage();
							}
						}
						else {
							SmoothScrollToCurrent();
						}
						break;
				}
			};


			linearLayout = new LinearLayout(this.Context);
			linearLayout.LayoutParameters = new LayoutParams(screenWidth, screenHeight);

			foreach (var item in Images)
			{
				linearLayout.AddView(SetImageView(item));
			}

			hScrollView.AddView(linearLayout);

			SetNativeControl(hScrollView);

			//set timer
			Device.StartTimer(TimeSpan.FromSeconds(3), () => {
				ExcuteTimer();
				return true;
			});
		}

		private void ExcuteTimer() {
			//currentPage = 0;
			SmoothScrollToNextPage();
		}

		private ImageView SetImageView(string item) {
			ImageView imageView = new ImageView(this.Context);
			imageView.LayoutParameters = new LayoutParams(screenWidth, screenHeight);

			if (IsValidUrl(item))
				imageView.SetImageBitmap(GetBitmapFromUrl(item));
			else
				imageView.SetImageResource(Resources.GetIdentifier(System.IO.Path.GetFileNameWithoutExtension(item), "drawable", Context.PackageName));

			imageView.SetScaleType(ImageView.ScaleType.FitXy);

			return imageView;
		} 


		void SmoothScrollToPrePage()
		{
			if (currentPage > 0)
			{
				currentPage--;
				handler.Post(runnable);
			}
		}

		void SmoothScrollToNextPage()
		{
			if (currentPage < Images.Count - 1)
			{
				currentPage++;
			}
			else
			{
				currentPage = 0;
			}
			handler.Post(runnable);
		}

		void SmoothScrollToCurrent()
		{
			handler.Post(runnable);
		}

		private void Run()
		{
			hScrollView.SmoothScrollTo((int)linearLayout.GetChildAt(currentPage).GetX(), 0);
		}

		class Runnable : Java.Lang.Object, Java.Lang.IRunnable
		{
			Action action;
			public Runnable(Action action)
			{
				this.action = action;
			}
			public void Run()
			{
				action();
			}
		}

		/// <summary>
		/// Gets the view.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="convertView">The convert view.</param>
		/// <param name="parent">The parent.</param>
		/// <param name="position">The position.</param>
		/// <returns>Android.Views.View.</returns>
		protected virtual Android.Views.View GetView(string item, Android.Views.View convertView, Android.Views.ViewGroup parent, int position)
		{
			var imageView = convertView as ImageView ?? new ImageView(parent.Context);

			if (IsValidUrl(item))
				imageView.SetImageBitmap(GetBitmapFromUrl(item));
			else
				imageView.SetImageResource(Resources.GetIdentifier(System.IO.Path.GetFileNameWithoutExtension(item), "drawable", Context.PackageName));

			imageView.SetScaleType(ImageView.ScaleType.FitXy);
			return imageView;
		}//////////////////////////

		/// <summary>
		/// Binds the specified new element.
		/// </summary>
		/// <param name="newElement">The new element.</param>
		private void Bind(ImageGallery newElement)
		{
			if (newElement != null)
			{
				newElement.PropertyChanging += ElementPropertyChanging;
				newElement.PropertyChanged += ElementPropertyChanged;
				((INotifyCollectionChanged)newElement.ItemsSource).CollectionChanged += DataCollectionChanged;
			}
		}

		/// <summary>
		/// Elements the property changing.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PropertyChangingEventArgs"/> instance containing the event data.</param>
		private void ElementPropertyChanging(object sender, PropertyChangingEventArgs e)
		{
			if (e.PropertyName == "ItemsSource")
				((INotifyCollectionChanged)Element.ItemsSource).CollectionChanged -= DataCollectionChanged;
		}

		/// <summary>
		/// Datas the collection changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
		private void DataCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			//this.Source.NotifyDataSetChanged();
		}

		/// <summary>
		/// Elements the property changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
		private void ElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "ItemsSource")
				((INotifyCollectionChanged)Element.ItemsSource).CollectionChanged += DataCollectionChanged;
		}

		/// <summary>
		/// Determines whether [is valid URL] [the specified URL string].
		/// </summary>
		/// <param name="urlString">The URL string.</param>
		/// <returns><c>true</c> if [is valid URL] [the specified URL string]; otherwise, <c>false</c>.</returns>
		private bool IsValidUrl(string urlString)
		{
			return URLUtil.IsValidUrl(urlString);
		}

		/// <summary>
		/// Gets the bitmap from URL.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns>Bitmap.</returns>
		private Bitmap GetBitmapFromUrl(string url)
		{
			Bitmap imageBitmap = null;

			using (var webClient = new WebClient())
			{
				var imageBytes = webClient.DownloadData(url);
				if (imageBytes != null && imageBytes.Length > 0)
					imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
			}

			return imageBitmap;
		}

		private class DataSource : BaseAdapter
		{
			/// <summary>
			/// The gallery renderer
			/// </summary>
			ImageGalleryRenderer _galleryRenderer;

			/// <summary>
			/// Initializes a new instance of the <see cref="DataSource"/> class.
			/// </summary>
			/// <param name="galleryRenderer">The gallery renderer.</param>
			public DataSource(ImageGalleryRenderer galleryRenderer)
			{
				this._galleryRenderer = galleryRenderer;
			}

			#region abstract members of BaseAdapter
			/// <summary>
			/// To be added.
			/// </summary>
			/// <param name="position">To be added.</param>
			/// <returns>To be added.</returns>
			/// <remarks>To be added.</remarks>
			public override Java.Lang.Object GetItem(int position)
			{
				return position;
			}

			/// <summary>
			/// To be added.
			/// </summary>
			/// <param name="position">To be added.</param>
			/// <returns>To be added.</returns>
			/// <remarks>To be added.</remarks>
			public override long GetItemId(int position)
			{
				return position;
			}

			/// <summary>
			/// To be added.
			/// </summary>
			/// <param name="position">To be added.</param>
			/// <param name="convertView">To be added.</param>
			/// <param name="parent">To be added.</param>
			/// <returns>To be added.</returns>
			/// <remarks>To be added.</remarks>
			public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
			{
				return _galleryRenderer.GetView(_galleryRenderer.Element.ItemsSource.Cast<string>().ToArray()[position], convertView, parent, position);
			}

			/// <summary>
			/// To be added.
			/// </summary>
			/// <value>To be added.</value>
			/// <remarks>To be added.</remarks>
			public override int Count
			{
				get
				{
					return _galleryRenderer.Element.ItemsSource.Cast<string>().Count();
				}
			}
			#endregion
		}
	}
}
