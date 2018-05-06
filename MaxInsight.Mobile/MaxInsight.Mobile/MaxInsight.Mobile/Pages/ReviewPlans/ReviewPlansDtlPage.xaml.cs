using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MaxInsight.Mobile.Pages.ReviewPlans
{
    public partial class ReviewPlansDtlPage : ContentPage
    {
        public ReviewPlansDtlPage()
        {
            Title = "计划任务详细";
            InitializeComponent();
            image.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(ShowOrHideImage),
                NumberOfTapsRequired = 1
            });
        }
        private void ShowOrHideImage()
        {
            if (stackImage.IsVisible)
            {
                stackImage.IsVisible = false;
                oneImage.Source = ImageSource.FromFile("icon_hide");
            }
            else
            {
                stackImage.IsVisible = true;
                oneImage.Source = ImageSource.FromFile("icon_show");
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            stackImage.IsVisible = false;          
            oneImage.Source = ImageSource.FromFile("icon_hide");          
        }
    }
}
