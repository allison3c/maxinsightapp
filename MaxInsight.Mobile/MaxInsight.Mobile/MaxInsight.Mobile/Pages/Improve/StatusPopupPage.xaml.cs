using MaxInsight.Mobile.Module.Dto;
using MaxInsight.Mobile.ViewModels.Improve;
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
    public partial class StatusPopupPage : PopupPage
    {
        public StatusPopupPage()
        {
            InitializeComponent();
            List<ImpStatusDto> impStatusLst = new List<ImpStatusDto>();
            impStatusLst.Add(new ImpStatusDto { ImpStatusCode = "", ImpStatusName = "全部",StatusKind=0 });
            impStatusLst.AddRange(CommonContext.Account.ImpStatusList);
            impStatusLstView.ItemsSource = impStatusLst;
            impStatusLstView.HeightRequest = impStatusLst.Count * 45;
        }
        //public StatusPopupPage(int statusType)
        //{
        //    InitializeComponent();
        //    if (statusType==0)
        //    {
        //        List<ImpPlanStatusDto> impPlanStatusLst = new List<ImpPlanStatusDto>();
        //        impPlanStatusLst.Add(new ImpPlanStatusDto {  PCode="", PName="全部"});
        //        impPlanStatusLst.AddRange(CommonContext.Account.ImpPlanStatusList);
        //        planStatusLst.ItemsSource = impPlanStatusLst;
        //        planStatusLst.HeightRequest = impPlanStatusLst.Count * 45;
        //        planStatusLst.IsVisible = true;
        //        resultStatusLst.IsVisible = false;
        //    }
        //    else
        //    {
        //        List<ImpResultStatusDto> impResultStatusLst = new List<ImpResultStatusDto>();
        //        impResultStatusLst.Add(new ImpResultStatusDto { RCode="", RName="全部"});
        //        impResultStatusLst.AddRange(CommonContext.Account.ImpResultStatusList);
        //        resultStatusLst.ItemsSource = impResultStatusLst;
        //        resultStatusLst.HeightRequest = impResultStatusLst.Count * 45; 
        //        resultStatusLst.IsVisible = true;
        //        planStatusLst.IsVisible = false;
        //    }
        //}
        //public void PassPlanStatus(object sender, ItemTappedEventArgs e)
        //{
        //    Navigation.PopPopupAsync();
        //    MessagingCenter.Send<ImpPlanStatusDto>((planStatusLst.SelectedItem as ImpPlanStatusDto),MessageConst.PLANSTATUSLIST_SEND);
        //}
        //public void PassResultStatus(object sender, ItemTappedEventArgs e)
        //{
        //    Navigation.PopPopupAsync();
        //    MessagingCenter.Send<ImpResultStatusDto>((resultStatusLst.SelectedItem as ImpResultStatusDto), MessageConst.RESULTSTATUSLIST_SEND);
        //}
        public void PassImpStatus(object sender, ItemTappedEventArgs e)
        {
            Navigation.PopPopupAsync();
            MessagingCenter.Send<ImpStatusDto>((impStatusLstView.SelectedItem as ImpStatusDto), MessageConst.IMPSTATUSLIST_SEND);
        }
        private void Cancel(object sender, EventArgs e)
        {
            Navigation.PopPopupAsync();
        }
    }
}
