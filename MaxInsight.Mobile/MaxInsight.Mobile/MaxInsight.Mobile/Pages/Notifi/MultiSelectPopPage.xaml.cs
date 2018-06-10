﻿using MaxInsight.Mobile.Module.Dto;
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

        public MultiSelectPopPage()
        {
            InitializeComponent();
            this.BindingContext = new MultiSelectPopViewModel();
        }

        #region
        public List<MultiSelectDto> ParamData { get; set; }
        public string ParamType { get; set; }
        #endregion
        protected override void OnAppearing()
        {
            try
            {
                if (ParamData == null || ParamData.Count == 0)
                {
                    List<ServerDto> serverList = new List<ServerDto>();
                    foreach (var item in CommonContext.Account.ZionList[0].AreaList)
                    {
                        serverList.AddRange(item.ServerList);
                    }
                    List<MultiSelectDto> source = new List<MultiSelectDto>();
                    foreach (ServerDto item in serverList)
                        source.Add(new MultiSelectDto { DisCode = item.SId, DisName = item.SName, IsChecked = false });
                    _lvMulstiselectSample.ItemsSource = source;
                    _lvMulstiselectSample.HeightRequest = source.Count * 45;

                }
                else
                {
                    _lvMulstiselectSample.ItemsSource = ParamData;
                    _lvMulstiselectSample.HeightRequest = ParamData.Count * 45;
                }
                MessagingCenter.Send<string>("F", MessageConst.NOTICE_DISTRIBUTOR_ONE);
            }
            catch (Exception)
            {
            }
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            try
            {
                if (ParamType == "NoticeMade")
                {
                    List<MultiSelectDto> resultList = (List<MultiSelectDto>)_lvMulstiselectSample.ItemsSource == null ? null : ((List<MultiSelectDto>)_lvMulstiselectSample.ItemsSource).FindAll(a => a.IsChecked);
                    if (resultList == null) resultList = new List<MultiSelectDto>();
                    MessagingCenter.Send<List<MultiSelectDto>>(resultList, MessageConst.NOTICE_DISTRIBUTOR_SET);
                    MessagingCenter.Send<List<MultiSelectDto>>((List<MultiSelectDto>)_lvMulstiselectSample.ItemsSource, MessageConst.NOTICE_DISTRIBUTOR_SHOW);
                }
                else if (ParamType == "NoticeList")
                {
                    List<MultiSelectDto> resultList = (List<MultiSelectDto>)_lvMulstiselectSample.ItemsSource == null ? null : ((List<MultiSelectDto>)_lvMulstiselectSample.ItemsSource).FindAll(a => a.IsChecked);
                    if (resultList == null) resultList = new List<MultiSelectDto>();
                    MessagingCenter.Send<List<MultiSelectDto>>(resultList, MessageConst.NOTICE_DISTRIBUTOR_SET_LIST);
                    MessagingCenter.Send<List<MultiSelectDto>>((List<MultiSelectDto>)_lvMulstiselectSample.ItemsSource, MessageConst.NOTICE_DISTRIBUTOR_SHOW_LIST);
                }
            }
            catch (Exception)
            {
            }
        }
        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem == null) return; // has been set to null, do not 'process' tapped event
                ((ListView)sender).SelectedItem = null; // de-select the row
            }
            catch (Exception)
            {
            }
        }
        public void OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                var mi = ((CheckBox)sender);
                List<MultiSelectDto> checkBoxSource = (List<MultiSelectDto>)_lvMulstiselectSample.ItemsSource;
                foreach (MultiSelectDto item in checkBoxSource)
                {
                    item.IsChecked = mi.Checked;
                }
            }
            catch (Exception)
            {
            }
        }
        public void OnOneCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string checkallornot = "Y";
                List<MultiSelectDto> checkBoxSource = (List<MultiSelectDto>)_lvMulstiselectSample.ItemsSource;
                if (checkBoxSource != null)
                {
                    int checkCount = checkBoxSource.FindAll(one => one.IsChecked).Count;
                    int allCount = checkBoxSource.Count;
                    if (allCount > 0 && checkCount == allCount)
                        checkallornot = "Y";
                    //this.IsAllChecked = true;
                    else
                        checkallornot = "N";
                }
                MessagingCenter.Send<string>(checkallornot, MessageConst.NOTICE_DISTRIBUTOR_ONE);
            }
            catch (Exception)
            {
            }
        }
        private async void ClosePopupPage(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PopPopupAsync();
            }
            catch (Exception)
            {
            }
        }
    }
}
