using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace MaxInsight.Mobile
{
	public partial class SystemListPage : ContentPage
	{
		//private string _id { get; set; }

		//private bool _canExcute { get; set; } = true;
		//private bool _canGo { get; set; } = true;
		//SystemListViewModel _context = new SystemListViewModel();
		//RegistScorePage _registPage = null;

		public SystemListPage()
		{
			InitializeComponent();
			//_context.TaskId = Convert.ToInt32(id);
			//BindingContext = _context;

			Title = "体系列表";
		}

		protected override void OnAppearing()
		{
			//if (_canExcute)
			//{
			//	MessagingCenter.Send<SystemListPage>(this, "RefreshSystem");
			//	_canExcute = false;
			//}
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			//_canExcute = true;
			//_canGo = true;
			base.OnDisappearing();
		}

		async void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
			if (e == null) return; // has been set to null, do not 'process' tapped event
								   //Debug.WriteLine("Tapped: " + e.Item);



			var item = e.Item as ItemOfTaskDto;
			MessagingCenter.Send<ItemOfTaskDto>(item, "GoRegisterPage");

			//if (_canGo)
			//{
			//	_canGo = false;
			//	if (_context != null)
			//	{
			//		//if (_registPage == null)
			//		//{
			//		//	_registPage
			//		//}

			//		if (!Navigation.NavigationStack.Contains(_registPage))
			//		{
			//			await Navigation.PushAsync(_registPage = new RegistScorePage(_context.SystemList, item), true);
			//		}
			//	}
			//}
		}
	}
}
