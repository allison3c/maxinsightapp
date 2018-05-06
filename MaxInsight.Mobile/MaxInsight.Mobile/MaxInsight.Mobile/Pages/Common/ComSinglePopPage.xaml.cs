using MaxInsight.Mobile.Module.Dto.Case;
using MaxInsight.Mobile.ViewModels.Common;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MaxInsight.Mobile.Pages.Common
{
    public partial class ComSinglePopPage : PopupPage
    {
        public ComSinglePopPage()
        {
            InitializeComponent();
            this.BindingContext = new ComSinglePopViewModel();
        }
        #region Property
        public string DataType { get; set; }
        #endregion
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send<string>(DataType, MessageConst.COMMON_SOURCE_GET);
        }
        public void PassSelectItem(object sender, ItemTappedEventArgs e)
        {
            Navigation.PopPopupAsync();
            switch (DataType)
            {
                case "NoticeStatus":
                    MessagingCenter.Send<NameValueObject>((statusLst.SelectedItem as NameValueObject), MessageConst.NOTICE_STATUSLIST_SELECT);
                    break;
                case "ImpAllTaskOfPlan":
                    MessagingCenter.Send<NameValueObject>((statusLst.SelectedItem as NameValueObject), MessageConst.IMP_TASKOFPLANLIST_SELECT);
                    break;
                case "ImpAllSourceType":
                    MessagingCenter.Send<NameValueObject>((statusLst.SelectedItem as NameValueObject), MessageConst.IMP_SOURCETYPELIST_SELECT);
                    break;
                default:
                    break;
            }
        }
        private void Cancel(object sender, EventArgs e)
        {
            Navigation.PopPopupAsync();
        }
    }
}
