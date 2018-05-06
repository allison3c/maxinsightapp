using Rg.Plugins.Popup.Pages;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Services;

namespace MaxInsight.Mobile.Pages.ShopfrontPatrol
{
    public partial class PreviewImageGesturePage : PopupPage
    {
        public PreviewImageGesturePage()
        {
            InitializeComponent();
        }
        public PreviewImageGesturePage(string url)
        {
            InitializeComponent();
            imageGesture.ItemsSource = url;
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await PopupNavigation.PopAsync();
        }
    }
}
