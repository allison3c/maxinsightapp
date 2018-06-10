using MaxInsight.Mobile.Module.Dto;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MaxInsight.Mobile.Pages.Improve
{
    public partial class ServicerPopupPage : PopupPage
    {
        public ServicerPopupPage()
        {
            InitializeComponent();
            try
            {
                List<ServerDto> serverList = new List<ServerDto>();
                serverList.Add(new ServerDto { SId = "0", SName = "全部" });
                foreach (var item in CommonContext.Account.ZionList[0].AreaList)
                {
                    serverList.AddRange(item.ServerList);
                }
                servicerLst.ItemsSource = serverList;
                servicerLst.HeightRequest = serverList.Count * 45;
            }
            catch (Exception)
            {
            }
        }
        private void Cancel(object sender, EventArgs e)
        {
            try
            {
                Navigation.PopPopupAsync();
            }
            catch (Exception)
            {
            }
        }
        public void PassServicer(object sender, ItemTappedEventArgs e)
        {
            try
            {
                Navigation.PopPopupAsync();
                MessagingCenter.Send<ServerDto>((servicerLst.SelectedItem as ServerDto), MessageConst.SERVICERSLIST_SEND);
            }
            catch (Exception)
            {
            }
        }
    }
}
