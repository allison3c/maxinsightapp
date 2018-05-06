using System;
using XLabs.Forms.Mvvm;

namespace MaxInsight.Mobile
{
	public class IOSWelcomViewModel : ViewModel
	{
		public IOSWelcomViewModel()
		{
			Images = new string[] { "guide_0.jpg", "guide_1.jpg", "guide_2.jpg" };
		}

		private string[] images;
		public string[] Images
		{
			get { return images; }
			set { SetProperty(ref images, value); }
		}


	}
}
