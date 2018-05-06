using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MaxInsight.Mobile.Pages.ShopfrontPatrol
{
    public partial class CustomizedTaskPage : ContentPage
    {
        public CustomizedTaskPage()
        {
            InitializeComponent();
            Title = "自定义任务";

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (lblVisible.Text == "Y")
            {
                stackSave.IsVisible = true;
                txtRemarks.IsEnabled = true;
            }
            else
            {
                stackSave.IsVisible = false;
                txtRemarks.IsEnabled = false;
            }
        }
    }
}
