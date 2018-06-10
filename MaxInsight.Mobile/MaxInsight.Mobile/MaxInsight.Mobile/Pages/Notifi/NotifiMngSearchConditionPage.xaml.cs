using MaxInsight.Mobile.Module.Dto.Notifi;
using MaxInsight.Mobile.Pages.Common;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MaxInsight.Mobile.Pages.Notifi
{
    public partial class NotifiMngSearchConditionPage : ContentPage
    {
        List<MultiSelectDto> disSource = new List<MultiSelectDto>();
        List<MultiSelectDto> depSource = new List<MultiSelectDto>();
        public NotifiMngSearchConditionPage()
        {
            InitializeComponent();
            try
            {
                MessagingCenter.Unsubscribe<List<MultiSelectDto>>(this, MessageConst.NOTICE_DISTRIBUTOR_SHOW_LIST);
                MessagingCenter.Subscribe<List<MultiSelectDto>>(
                this,
                MessageConst.NOTICE_DISTRIBUTOR_SHOW_LIST,
                (paramList) =>
                {
                    disSource = paramList;
                });
                MessagingCenter.Unsubscribe<List<MultiSelectDto>>(this, MessageConst.NOTICE_DEPARTMENT_SHOW_LIST);
                MessagingCenter.Subscribe<List<MultiSelectDto>>(
                this,
                MessageConst.NOTICE_DEPARTMENT_SHOW_LIST,
                (paramList) =>
                {
                    depSource = paramList;
                });
                replyRdo.ItemsSource = new List<string> { "是", "否" };
                replyRdo.Items[0].Checked = true;
                SetControlWithRole();
            }
            catch (Exception)
            {
            }
        }
        private ComSinglePopPage _comPopPage;
        public ComSinglePopPage ComPopPage
        {
            get
            {
                if (_comPopPage == null)
                {
                    _comPopPage = new ComSinglePopPage();
                }
                return _comPopPage;
            }
        }
        private MultiSelectPopPage _disPopPage;
        public MultiSelectPopPage DisPopPage
        {
            get
            {
                if (_disPopPage == null)
                {
                    _disPopPage = new MultiSelectPopPage();
                }
                return _disPopPage;
            }
        }

        private MultiSelectDepPage _depPopPage;
        public MultiSelectDepPage DepPopPage
        {
            get
            {
                if (_depPopPage == null)
                {
                    _depPopPage = new MultiSelectDepPage();
                }
                return _depPopPage;
            }
        }
        private async void OnOpenStatusPopupPage(object sender, EventArgs e)
        {
            try
            {
                ComPopPage.DataType = "NoticeStatus";
                await Navigation.PushPopupAsync(ComPopPage);
            }
            catch
            {

            }
        }
        private async void OnOpenDisPopupPage(object sender, EventArgs e)
        {
            try
            {
                DisPopPage.ParamData = disSource;
                DisPopPage.ParamType = "NoticeList";
                await Navigation.PushPopupAsync(DisPopPage);
            }
            catch
            {

            }
        }
        private async void OnOpenDepPopupPage(object sender, EventArgs e)
        {
            try
            {
                DepPopPage.ParamData = depSource;
                DepPopPage.ParamType = "NoticeList";
                await Navigation.PushPopupAsync(DepPopPage);
            }
            catch
            {

            }
        }
        private void SetControlWithRole()
        {
            try
            {
                //服务商登陆，只显示服务商的名字
                if (CommonContext.Account.UserType == "S")
                {
                    distributorLbl.IsVisible = true;
                    distributorLbl.Text = CommonContext.Account.OrgServerName;
                    distributorBtn.IsVisible = false;
                }
                //部门登陆，显示服务商的名字和部门的名字
                else if (CommonContext.Account.UserType == "D")
                {
                    distributorLbl.IsVisible = true;
                    distributorLbl.Text = CommonContext.Account.OrgServerName;
                    distributorBtn.IsVisible = false;

                    departmentLbl.IsVisible = true;
                    departmentLbl.Text = CommonContext.Account.OrgDepartmentName;
                    departmentBtn.IsVisible = false;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
