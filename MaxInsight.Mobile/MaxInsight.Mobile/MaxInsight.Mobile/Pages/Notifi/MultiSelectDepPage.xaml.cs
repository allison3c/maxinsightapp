
using MaxInsight.Mobile.Module.Dto;
using MaxInsight.Mobile.Module.Dto.Notifi;
using MaxInsight.Mobile.ViewModels.Notifi;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace MaxInsight.Mobile.Pages.Notifi
{
    public partial class MultiSelectDepPage : PopupPage
    {
        public MultiSelectDepPage()
        {
            InitializeComponent();
            this.BindingContext = new MultiSelectDepViewModel();
        }
        #region
        public List<MultiSelectDto> ParamData{ get; set; }
        public string ParamType { get; set; }
        #endregion
        protected override void OnAppearing()
        {
            if (ParamData == null || ParamData.Count == 0)
            {
                List<DepartmentDto> departmentList = new List<DepartmentDto>();
                departmentList.AddRange(CommonContext.Account.DepartmentList);
                List<MultiSelectDto> source = new List<MultiSelectDto>();
                foreach (DepartmentDto item in departmentList)
                    source.Add(new MultiSelectDto { DisCode = item.DId, DisName = item.DName, IsChecked = false });
                _lvMulstiselectSample.ItemsSource = source;
                _lvMulstiselectSample.HeightRequest = source.Count * 45;
            }
            else
            {
                _lvMulstiselectSample.ItemsSource = ParamData;
                _lvMulstiselectSample.HeightRequest = ParamData.Count *45;
            }
            MessagingCenter.Send<string>("F", MessageConst.NOTICE_DISTRIBUTOR_ONE);
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (ParamType == "NoticeMade")
            {
                List<MultiSelectDto> resultList = (List<MultiSelectDto>)_lvMulstiselectSample.ItemsSource == null ? null : ((List<MultiSelectDto>)_lvMulstiselectSample.ItemsSource).FindAll(a => a.IsChecked);
                if (resultList == null) resultList = new List<MultiSelectDto>();
                MessagingCenter.Send<List<MultiSelectDto>>(resultList, MessageConst.NOTICE_DEPARTMENT_SET);
                MessagingCenter.Send<List<MultiSelectDto>>((List<MultiSelectDto>)_lvMulstiselectSample.ItemsSource, MessageConst.NOTICE_DEPARTMENT_SHOW);
            }
            else if(ParamType == "NoticeList")
            {
                List<MultiSelectDto> resultList = (List<MultiSelectDto>)_lvMulstiselectSample.ItemsSource == null ? null : ((List<MultiSelectDto>)_lvMulstiselectSample.ItemsSource).FindAll(a => a.IsChecked);
                if (resultList == null) resultList = new List<MultiSelectDto>();
                MessagingCenter.Send<List<MultiSelectDto>>(resultList, MessageConst.NOTICE_DEPARTMENT_SET_LIST);
                MessagingCenter.Send<List<MultiSelectDto>>((List<MultiSelectDto>)_lvMulstiselectSample.ItemsSource, MessageConst.NOTICE_DEPARTMENT_SHOW_LIST);

            }

        }
        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return; // has been set to null, do not 'process' tapped event
            ((ListView)sender).SelectedItem = null; // de-select the row
        }
        public void OnCheckedChanged(object sender, EventArgs e)
        {
            var mi = ((CheckBox)sender);
            List<MultiSelectDto> checkBoxSource = (List<MultiSelectDto>)_lvMulstiselectSample.ItemsSource;
            foreach (MultiSelectDto item in checkBoxSource)
            {
                item.IsChecked = mi.Checked;
            }
        }
        public void OnOneCheckedChanged(object sender, EventArgs e)
        {
            //var mi = ((CheckBox)_IsAllCheckedName).Checked;
            string checkallornot = "Y";
            List<MultiSelectDto> checkBoxSource = (List<MultiSelectDto>)_lvMulstiselectSample.ItemsSource;
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
        private void ClosePopupPage(object sender, EventArgs e)
        {
            //await Navigation.PopPopupAsync();
            PopupNavigation.PopAsync();
        }
    }
}
