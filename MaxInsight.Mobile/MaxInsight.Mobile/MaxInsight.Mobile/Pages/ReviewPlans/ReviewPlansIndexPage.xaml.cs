using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MaxInsight.Mobile.Pages.ReviewPlans
{
    public partial class ReviewPlansIndexPage : ContentPage
    {
        public ReviewPlansIndexPage()
        {
            Title = "审核列表";
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                MessagingCenter.Send<string>("", MessageConst.GETREVIEWPLANSLIST);
            }
            catch (Exception)
            {
            }
        }
    }
}
