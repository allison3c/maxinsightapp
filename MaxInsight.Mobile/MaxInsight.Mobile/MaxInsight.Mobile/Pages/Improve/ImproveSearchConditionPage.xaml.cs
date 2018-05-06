using MaxInsight.Mobile.Module.Dto;
using MaxInsight.Mobile.Pages.Common;
using MaxInsight.Mobile.Pages.Notifi;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MaxInsight.Mobile.Pages.Improve
{
    public partial class ImproveSearchConditionPage : ContentPage
    {
        public ImproveSearchConditionPage()
        {
            InitializeComponent();
            SetControl();
            //statusRdo.ItemsSource = new List<string> { "计划","结果"};
            //statusRdo.Items[0].Checked = true;
            //itemNameLbl.WidthRequest = App.ScreenWidth - 80;
        }

        private void SetControl()
        {
            if (CommonContext.Account.UserType=="D")
            {
                departmentLbl.IsVisible = true;
                servicerLbl.IsVisible = true;
                departmentLbl.Text = CommonContext.Account.OrgDepartmentName;
                servicerLbl.Text = CommonContext.Account.OrgServerName;
                servicerBtn.IsVisible = false;
                departmentBtn.IsVisible = false;
            }
            else if (CommonContext.Account.UserType=="S")
            {
                departmentLbl.IsVisible = false;
                servicerLbl.IsVisible = true;
                servicerLbl.Text = CommonContext.Account.OrgServerName;
                servicerBtn.IsVisible = false;
                departmentBtn.IsVisible = true;
            }
            else
            {
                departmentLbl.IsVisible = false;
                servicerLbl.IsVisible = false;
                servicerBtn.IsVisible = true;
                departmentBtn.IsVisible = true;
            }
        }
        private ComSinglePopPage comSinglePopPage;
        public ComSinglePopPage ComSinglePopPage
        {
            get
            {
                if (comSinglePopPage == null)
                {
                    comSinglePopPage = new ComSinglePopPage();
                }
                return comSinglePopPage;
            }
        }
        private DepartmentPopupPage departmentPopupPage;
        public DepartmentPopupPage DepartmentPopupPage
        {
            get
            {
                if (departmentPopupPage == null)
                {
                    departmentPopupPage = new DepartmentPopupPage("A");
                }
                return departmentPopupPage;
            }
        }
        private ServicerPopupPage servicerPopupPage;
        public ServicerPopupPage ServicerPopupPage
        {
            get
            {
                if (servicerPopupPage == null)
                {
                    servicerPopupPage = new ServicerPopupPage();
                }
                return servicerPopupPage;
            }
        }
        private StatusPopupPage statusPopupPage;
        public StatusPopupPage StatusPopupPage
        {
            get
            {
                if (statusPopupPage == null)
                {
                    //statusPopupPage = new StatusPopupPage(statusRdo.SelectedIndex);
                    statusPopupPage = new StatusPopupPage();
                }
                return statusPopupPage;
            }
        }
        private async void OnOpenStatusPopupPage(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushPopupAsync(StatusPopupPage);
            }
            catch
            {

            }
        }
        private async void OnOpenServicerPopupPage(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushPopupAsync(ServicerPopupPage);
            }
            catch
            {
                //Method `PushPopupAsync' not found in type `Xamarin.Forms.INavigation'.
            }
        }
        private async void OnOpenDepartmentPopupPage(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushPopupAsync(DepartmentPopupPage);
            }
            catch
            {

            }
        }
        private async void OnOpenPlanPopupPage(object sender, EventArgs e)
        {
            try
            {
                ComSinglePopPage.DataType = "ImpAllTaskOfPlan";
                await Navigation.PushPopupAsync(ComSinglePopPage);
            }
            catch
            {

            }
        }
        private async void OnOpenSourceTypePopupPage(object sender, EventArgs e)
        {
            try
            {
                ComSinglePopPage.DataType = "ImpAllSourceType";
                await Navigation.PushPopupAsync(ComSinglePopPage);
            }
            catch
            {

            }
        }
    }
}
