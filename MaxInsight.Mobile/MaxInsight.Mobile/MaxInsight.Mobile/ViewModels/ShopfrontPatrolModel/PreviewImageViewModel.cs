using System;
using System.Collections.ObjectModel;
using XLabs;
using XLabs.Forms.Mvvm;

namespace MaxInsight.Mobile
{
	public class PreviewImageViewModel : ViewModel
	{
		public PreviewImageViewModel()
		{
		}


		private ObservableCollection<StandardPicRegDto> _images;
		public ObservableCollection<StandardPicRegDto> Images
		{
			get { return _images; }
			set { SetProperty(ref _images, value); }
		}

		public void Init(ObservableCollection<StandardPicRegDto> images, StandardPicRegDto currentImage) {
			Images = images;
		}
	}
}
