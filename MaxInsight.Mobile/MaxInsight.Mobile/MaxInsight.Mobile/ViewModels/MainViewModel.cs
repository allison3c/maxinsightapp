using System.Collections.ObjectModel;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;

namespace MaxInsight.Mobile.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
			Images = new ObservableCollection<string>();
			for (var i = 0; i < 10; i++)
			{
				Images.Add("icon_main@3x.jpg");
			}
        }

		private ObservableCollection<string> _images;

		public ObservableCollection<string> Images
		{
			get
			{
				return _images;
			}
			set
			{
				SetProperty(ref _images, value);
			}
		}


        public Command GoTaskPageCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        await Navigation.PushAsync<MainViewModel>();
                    }
                    catch (System.Exception)
                    {
                    }
                });
            }
        }
    }
}
