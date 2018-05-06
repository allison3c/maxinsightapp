using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MaxInsight.Mobile.Helpers;
using PCLStorage;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using XLabs.Ioc;
using static PCLStorage.FileSystem;

namespace MaxInsight.Mobile
{
	public partial class PreviewImagePage : PopupPage
	{
        public PreviewImagePage()
		{
			InitializeComponent();
        }

        public PreviewImagePage(List<ImagePreviewDto> images, int position)
		{
			var bc = new VM();
			BindingContext = bc;
			InitializeComponent();

            List<ImagePreviewDto> list = new List<ImagePreviewDto>();
            if (images != null && images.Count > 0)
            {
                foreach (ImagePreviewDto imageDto in images)
                {
                    string url = imageDto.Url.ToUpper();
                    if (url.EndsWith(".JPG") || url.EndsWith(".BMP") || url.EndsWith(".GIF") ||
                        url.EndsWith(".JPEG") || url.EndsWith(".PNG") || url.EndsWith(".SVG"))
                    {
                        list.Add(new ImagePreviewDto { Url = imageDto.Url.Replace("oss", "img") + "@50q" });
                    }
                }

            }
           
            Device.StartTimer(TimeSpan.FromMilliseconds(2000), () =>
			{
				bc.Images = list;
				bc.CurrentPos = position;

				return false;
			});
		}

		private async Task<byte[]> GetStream(string path)
		{
			var f = await Current.LocalStorage.GetFileAsync(path);

			Stream s = await f.OpenAsync(FileAccess.Read);

			byte[] imageData;

			using (MemoryStream ms = new MemoryStream())
			{
				await s.CopyToAsync(ms);
				imageData = ms.ToArray();
			}

			return imageData;
		}

		protected override bool OnBackButtonPressed()
		{
			// Prevent hide popup
			//return base.OnBackButtonPressed();
			return true;
		}

		async void Handle_Clicked(object sender, System.EventArgs e)
		{
			await PopupNavigation.PopAsync();
		}
	}

	public class VM : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this,
					new PropertyChangedEventArgs(propertyName));
			}
		}

		private List<ImagePreviewDto> _images;
		public List<ImagePreviewDto> Images
		{
			get { return _images; }
			set {
				_images = value;
				OnPropertyChanged("Images");
			}
		}

		private int _screenHeight;
		public int ScreenHeight { 
			get { return _screenHeight; }
			set
			{
				_screenHeight = value;
				OnPropertyChanged("ScreenHeight");
			}
		}

		private int _currentPos;
		public int CurrentPos
		{
			get { return _currentPos; }
			set { 
			
				_currentPos = value;
				OnPropertyChanged("CurrentPos");
			}
		}
	}
}
