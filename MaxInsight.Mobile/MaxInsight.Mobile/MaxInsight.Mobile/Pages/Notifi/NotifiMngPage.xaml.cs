using MaxInsight.Mobile.Module.Dto.Notifi;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MaxInsight.Mobile.Pages.Notifi
{
    public partial class NotifiMngPage : ContentPage
    {
        List<MultiSelectDto> disSource = new List<MultiSelectDto>();
        List<MultiSelectDto> depSource = new List<MultiSelectDto>();
        public NotifiMngPage()
        {
            InitializeComponent();
            ////this.FindByName<ListView>("_AttachmentList").BindingContext = new NotifiMngViewModel().NoticeAttachmentList;
            //_AttachmentList.ItemSelected += (o, e) => { this._AttachmentList.SelectedItem = null; };
            try
            {
                MessagingCenter.Unsubscribe<List<MultiSelectDto>>(this, MessageConst.NOTICE_DISTRIBUTOR_SHOW);
                MessagingCenter.Subscribe<List<MultiSelectDto>>(
                this,
                MessageConst.NOTICE_DISTRIBUTOR_SHOW,
                (paramList) =>
                {
                    disSource = paramList;
                });
                MessagingCenter.Unsubscribe<List<MultiSelectDto>>(this, MessageConst.NOTICE_DEPARTMENT_SHOW);
                MessagingCenter.Subscribe<List<MultiSelectDto>>(
                this,
                MessageConst.NOTICE_DEPARTMENT_SHOW,
                (paramList) =>
                {
                    depSource = paramList;
                });
                MessagingCenter.Unsubscribe<string>(this, MessageConst.NOTICE_MADE_SETCONTROLROLE);
                MessagingCenter.Subscribe<string>(
                    this,
                    MessageConst.NOTICE_MADE_SETCONTROLROLE,
                    (savetype) =>
                    {
                        SetControlRole(savetype);
                    });

                replyRdo.ItemsSource = new List<string> { "否", "是" };
                replyRdo.Items[0].Checked = true;
            }
            catch (Exception)
            {
            }
        }
        public NotifiMngPage(NoticeDto noticeDto)
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
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
        private async void OnOpenDisPopupPage(object sender, EventArgs e)
        {
            try
            {
                DisPopPage.ParamData = disSource;
                DisPopPage.ParamType = "NoticeMade";
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
                DepPopPage.ParamType = "NoticeMade";
                await Navigation.PushPopupAsync(DepPopPage);
            }
            catch
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
        public void OnDeleteAttechment(object sender, EventArgs e)
        {
            try
            {
                var item = (Button)sender;
                MessagingCenter.Send<string>(item.CommandParameter.ToString(), MessageConst.NOTICE_ATTECHMENT_DELETE);
            }
            catch (Exception)
            {
            }
        }
        private void SetControlRole(string savetype)
        {
            try
            {
                noticenoSLayout.IsVisible = true;
                if (savetype == "T" || savetype == "Reg")
                {
                    noticetitleEntry.IsVisible = true; noticetitleLabel.IsVisible = false;//通知标题
                    startDateDP.IsVisible = true; lblstartDateDP.IsVisible = false;//开始日期
                    endDateDP.IsVisible = true; lblendDateDP.IsVisible = false;//结束日期
                    gridReplySelecte.IsVisible = true; lblReplySelecte.IsVisible = false;//结果反馈
                    noticecontentEditor.IsVisible = true; noticecontentLbl.IsVisible = false;//通知内容
                    btnAttachUpload.IsVisible = true; btnAttachUploadVedio.IsVisible = true;
                    if (Device.OS == TargetPlatform.iOS)
                    {
                        btnAttachUploadFile.IsVisible = false;//上传按钮
                    }
                    else
                    {
                        btnAttachUploadFile.IsVisible = true;//上传按钮
                    }

                    lstNotifiAttach.IsVisible = true; lstNotifiAttachNoDelete.IsVisible = false;//附件
                    tempSaveBtn.IsVisible = true; commitBtn.IsVisible = true; cancelBtn.IsVisible = true;
                    if (savetype == "Reg") noticenoSLayout.IsVisible = false;
                }
                else
                {
                    noticetitleEntry.IsVisible = false; noticetitleLabel.IsVisible = true;//通知标题
                    startDateDP.IsVisible = false; lblstartDateDP.IsVisible = true;//开始日期
                    endDateDP.IsVisible = false; lblendDateDP.IsVisible = true;//结束日期
                    gridReplySelecte.IsVisible = false; lblReplySelecte.IsVisible = true;//结果反馈
                    noticecontentEditor.IsVisible = false; noticecontentLbl.IsVisible = true;//通知内容
                    btnAttachUpload.IsVisible = false; btnAttachUploadVedio.IsVisible = false;
                    btnAttachUploadFile.IsVisible = false;//上传按钮
                    lstNotifiAttach.IsVisible = false; lstNotifiAttachNoDelete.IsVisible = true;//附件
                    tempSaveBtn.IsVisible = false; commitBtn.IsVisible = false; cancelBtn.IsVisible = false;
                    if (CommonContext.Account.UserType == "S")
                    {
                        distributorLbl.IsVisible = true;
                        distributorBtn.IsVisible = false;
                        distributorLbl.Text = CommonContext.Account.DisName;

                    }
                    else if (CommonContext.Account.UserType == "D")
                    {
                        distributorLbl.IsVisible = true;
                        distributorBtn.IsVisible = false;
                        distributorLbl.Text = CommonContext.Account.OrgServerName;

                        departmentLbl.IsVisible = true;
                        departmentBtn.IsVisible = false;
                        departmentLbl.Text = CommonContext.Account.OrgDepartmentName;
                    }
                    else
                    {
                        distributorLbl.IsVisible = false;
                        distributorBtn.IsVisible = true;

                        departmentLbl.IsVisible = false;
                        departmentBtn.IsVisible = true;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
