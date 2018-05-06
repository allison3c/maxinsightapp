using MaxInsight.Mobile.Module.Dto;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MaxInsight.Mobile.Pages.Improve
{
    public partial class ServicerPopupPage : PopupPage
    {
        public ServicerPopupPage()
        {
            InitializeComponent();
            List<ServerDto> serverList = new List<ServerDto>();
            serverList.Add(new ServerDto { SId="0",SName="全部"});
            foreach (var item in CommonContext.Account.ZionList[0].AreaList)
            {
                serverList.AddRange(item.ServerList);
            }
            servicerLst.ItemsSource = serverList;
            servicerLst.HeightRequest = serverList.Count * 45;
        }
        private void Cancel(object sender, EventArgs e)
        {
            Navigation.PopPopupAsync();
        }
        public void PassServicer(object sender, ItemTappedEventArgs e)
        {
            Navigation.PopPopupAsync();
            MessagingCenter.Send<ServerDto>((servicerLst.SelectedItem as ServerDto), MessageConst.SERVICERSLIST_SEND);
        }
    }
}
