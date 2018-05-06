using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MaxInsight.Mobile.Pages.Calendar
{
    public partial class CalendarRegPage : ContentPage
    {
        public CalendarRegPage()
        {
            InitializeComponent();
            Title = "任务登记";

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (lblCtype.Text == "P")
            {
                btnDelete.IsVisible = false;
            }
            else
            {
                btnDelete.IsVisible = true;
            }
        }
    }
}
