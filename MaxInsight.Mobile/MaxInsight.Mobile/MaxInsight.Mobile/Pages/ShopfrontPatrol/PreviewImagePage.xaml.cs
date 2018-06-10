using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using PCLStorage;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
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
            try
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
            catch (Exception)
            {
            }
        }

        private async Task<byte[]> GetStream(string path)
        {
            try
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
            catch (Exception)
            {
                return null;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            // Prevent hide popup
            //return base.OnBackButtonPressed();
            return true;
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                await PopupNavigation.PopAsync();
            }
            catch (Exception)
            {
            }
        }
    }

    public class VM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            try
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                        new PropertyChangedEventArgs(propertyName));
                }
            }
            catch (Exception)
            {
            }
        }

        private List<ImagePreviewDto> _images;
        public List<ImagePreviewDto> Images
        {
            get { return _images; }
            set
            {
                try
                {
                    _images = value;
                    OnPropertyChanged("Images");
                }
                catch (Exception)
                {
                }
            }
        }

        private int _screenHeight;
        public int ScreenHeight
        {
            get { return _screenHeight; }
            set
            {
                try
                {
                    _screenHeight = value;
                    OnPropertyChanged("ScreenHeight");
                }
                catch (Exception)
                {
                }
            }
        }

        private int _currentPos;
        public int CurrentPos
        {
            get { return _currentPos; }
            set
            {
                try
                {
                    _currentPos = value;
                    OnPropertyChanged("CurrentPos");
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
