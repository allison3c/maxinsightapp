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
    public partial class DepartmentPopupPage : PopupPage
    {
        string type;
        public DepartmentPopupPage()
        {
            
        }
        public DepartmentPopupPage(string type)
        {
            InitializeComponent();
            this.type = type;
            List<DepartmentDto> departmentList = new List<DepartmentDto>();
            if (type=="A")
            {
                departmentList.Add(new DepartmentDto { DId = "0", DName = "全部" });
            }
            departmentList.AddRange(CommonContext.Account.DepartmentList);
            departmentLst.ItemsSource = departmentList;
            departmentLst.HeightRequest = departmentList.Count * 45;
        }

        private void Cancel(object sender, EventArgs e)
        {
            Navigation.PopPopupAsync();
        }

        public void PassDepartment(object sender, ItemTappedEventArgs e)
        {
            Navigation.PopPopupAsync();
            if (type=="A")
            {
                MessagingCenter.Send<DepartmentDto>((departmentLst.SelectedItem as DepartmentDto), MessageConst.DEPARTMENTLIST_SEND);
            }
            else
            {
                MessagingCenter.Send<DepartmentDto>((departmentLst.SelectedItem as DepartmentDto), MessageConst.RESPONSIBLEDEPARTMENT_SEND);
            }
        }
    }
}
