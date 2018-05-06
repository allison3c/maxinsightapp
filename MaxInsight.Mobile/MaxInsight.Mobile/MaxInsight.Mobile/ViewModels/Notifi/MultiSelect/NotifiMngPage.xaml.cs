using MaxInsight.Mobile.Module.Dto.Notifi;
using MaxInsight.Mobile.ViewModels.Notifi;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace MaxInsight.Mobile.Pages.Notifi
{
    public partial class NotifiMngPage : ContentPage
    {
        public NotifiMngPage()
        {
            InitializeComponent();
            ////this.FindByName<ListView>("_AttachmentList").BindingContext = new NotifiMngViewModel().NoticeAttachmentList;
            //_AttachmentList.ItemSelected += (o, e) => { this._AttachmentList.SelectedItem = null; };
        }
        public NotifiMngPage(NoticeDto noticeDto)
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ansPicker.Items[0].Checked = true;
        }
        private async void OnOpenDisPopupPage(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();
            var page = new MultiSelectPopPage("NotifiMngPage");
            await Navigation.PushPopupAsync(page);
        }
        private async void OnOpenDepPopupPage(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();
            var page = new MultiSelectDepPage("NotifiMngPage");
            await Navigation.PushPopupAsync(page);
            //var page = new TestPopPage();
            //await Navigation.PushPopupAsync(page);
        }

        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return; // has been set to null, do not 'process' tapped event
            ((ListView)sender).SelectedItem = null; // de-select the row
        }
        public void OnDeleteAttechment(object sender,EventArgs e)
        {
            var item = (Button)sender;
            MessagingCenter.Send<string>(item.CommandParameter.ToString(), MessageConst.NOTICE_ATTECHMENT_DELETE);
        }

    }
}
