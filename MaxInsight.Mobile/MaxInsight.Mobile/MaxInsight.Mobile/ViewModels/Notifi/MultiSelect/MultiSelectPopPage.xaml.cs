using MaxInsight.Mobile.Module.Dto.Notifi;
using MaxInsight.Mobile.ViewModels.Notifi;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace MaxInsight.Mobile.Pages.Notifi
{
    public partial class MultiSelectPopPage : PopupPage
    {
        public MultiSelectPopPage() { }
        public MultiSelectPopPage(string parentPage)
        {
            InitializeComponent();
            this.BindingContext = new MultiSelectPopViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send<MultiSelectPopPage>(this, MessageConst.NOTICE_DISTRIBUTOR_GET);
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send<List<DistributorDto>>((List<DistributorDto>)_lvMulstiselectSample.ItemsSource, MessageConst.NOTICE_DISTRIBUTOR_SET);
                    
            
        }
        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return; // has been set to null, do not 'process' tapped event
            ((ListView)sender).SelectedItem = null; // de-select the row
        }
        public void OnCheckedChanged(object sender, EventArgs e)
        {
            var mi = ((CheckBox)sender);
            List<DistributorDto> checkBoxSource = (List<DistributorDto>)_lvMulstiselectSample.ItemsSource;
            foreach(DistributorDto item in checkBoxSource)
            {
                
                item.IsChecked = mi.Checked;
            }
            MessagingCenter.Send<List<DistributorDto>>(checkBoxSource, MessageConst.NOTICE_DISTRIBUTOR_ALL);
        }
        public void OnOneCheckedChanged(object sender, EventArgs e)
        {
            var mi = ((CheckBox)_IsAllCheckedName).Checked;
            string checkallornot = "Y";
            List<DistributorDto> checkBoxSource = (List<DistributorDto>)_lvMulstiselectSample.ItemsSource;
            if (checkBoxSource != null)
            {
                int checkCount = checkBoxSource.FindAll(one => one.IsChecked).Count;
                int allCount = checkBoxSource.Count;
                if (allCount > 0 && checkCount == allCount)
                    checkallornot = "Y";
                else
                    checkallornot = "F";
            }
            MessagingCenter.Send<string>(checkallornot, MessageConst.NOTICE_DISTRIBUTOR_ONE);
        }
        private async void ClosePopupPage(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
    }
}
