using MaxInsight.Mobile.Module;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace MaxInsight.Mobile.Pages.Improve
{
    public partial class ImpResultCommitPage : ContentPage
    {
        public ImpResultCommitPage()
        {
            InitializeComponent();

            //if (CommonContext.Account.UserType == "D")
            //{
            //   Title = "改善结果提交";
            // }
            // else
            // {
            // Title = "改善结果审批";
            // }
            try
            {
                ServerApplyYN.ItemsSource = new[] { "通过", "拒绝" };
                AreaApplyYN.ItemsSource = new[] { "通过", "拒绝" };

                MessagingCenter.Unsubscribe<string>(this, MessageConst.IMPROVE_RESULTCOMMIT_SETCONTROLROLE);
                MessagingCenter.Subscribe<string>(
                    this,
                    MessageConst.IMPROVE_RESULTCOMMIT_SETCONTROLROLE,
                    (arg) =>
                    {
                        var array = arg.Split('§');
                        string ResultStatus = array[0];
                        string ResultApproalYN = array[1];
                        string allocateYN = array[2];

                        if (allocateYN.ToUpper() == "FALSE")
                            SetControlRoleByServer(ResultStatus);
                        else
                            SetControlRole(ResultStatus);
                        if (ResultApproalYN.ToUpper() == "FALSE")
                        {
                            layAreaApply.IsVisible = false;
                        }
                        else
                        {
                            layAreaApply.IsVisible = true;
                        }
                    });
            }
            catch (Exception)
            {
            }
        }

        #region event

        public void OnDeleteResultAttechment(object sender, EventArgs e)
        {
            try
            {
                ImageButton img = sender as ImageButton;
                MessagingCenter.Send<string>(img.CommandParameter.ToString(), MessageConst.IMPROVE_RESULTATTACH_DELETE);
            }
            catch (Exception)
            {
            }
        }

        private void OnServerCheckedChanged(object sender, int e)
        {
            try
            {
                MessagingCenter.Send<List<RequestParameter>>(new List<RequestParameter> { new RequestParameter { Name = "Server", Value = e.ToString() } }, MessageConst.IMPROVE_RESULTAPPLYYN_CHANGE);
            }
            catch (Exception)
            {
            }
        }
        private void OnAreaCheckedChanged(object sender, int e)
        {
            try
            {
                MessagingCenter.Send<List<RequestParameter>>(new List<RequestParameter> { new RequestParameter { Name = "Area", Value = e.ToString() } }, MessageConst.IMPROVE_RESULTAPPLYYN_CHANGE);
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region private
        private void SetControlRole(string ResultStatus)
        {
            try
            {
                switch (CommonContext.Account.UserType)
                {
                    case "D":
                        SetControlRoleForDepartment(ResultStatus);
                        break;
                    case "S":
                        SetControlRoleForServer(ResultStatus);
                        break;
                    case "Z":
                        SetControlRoleForArea(ResultStatus);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        private void SetControlRoleByServer(string ResultStatus)
        {
            try
            {
                switch (CommonContext.Account.UserType)
                {
                    case "S":
                        SetControlRoleForServerByServer(ResultStatus);
                        break;
                    case "Z":
                        SetControlRoleForArea(ResultStatus);
                        break;
                    default:
                        SetControlDefault(ResultStatus);
                        break;
                }
                layServeApply.IsVisible = false;
            }
            catch (Exception)
            {
            }
        }


        private void SetControlRoleForDepartment(string ResultStatus)
        {
            try
            {
                if (ResultStatus == "A")
                {
                    layApply.IsVisible = false;
                    lblImpResultContent.IsVisible = false;
                    txtImpResultContent.IsVisible = true;
                    btnAttachUpload.IsVisible = true;
                    btnAttachUploadVedio.IsVisible = true;
                    if (Device.OS == TargetPlatform.iOS)
                    {
                        btnAttachUploadFile.IsVisible = false;
                    }
                    else
                    {
                        btnAttachUploadFile.IsVisible = true;
                    }
                    lstImpResultAttach.IsVisible = true;
                    lstImpResultAttachNoDelete.IsVisible = false;
                    //btnSaveImpResult.IsVisible = true;
                    sltSaveImpResult.IsVisible = true;
                }
                else if (ResultStatus == "D")
                {
                    layApply.IsVisible = true;
                    layServeApply.IsVisible = true;
                    lblImpResultContent.IsVisible = false;
                    txtImpResultContent.IsVisible = true;
                    btnAttachUpload.IsVisible = true;
                    btnAttachUploadVedio.IsVisible = true;
                    if (Device.OS == TargetPlatform.iOS)
                    {
                        btnAttachUploadFile.IsVisible = false;
                    }
                    else
                    {
                        btnAttachUploadFile.IsVisible = true;
                    }
                    lstImpResultAttach.IsVisible = true;
                    lstImpResultAttachNoDelete.IsVisible = false;
                    //btnSaveImpResult.IsVisible = true;
                    sltSaveImpResult.IsVisible = true;
                }
                else
                {
                    SetControlDefault(ResultStatus);
                }
            }
            catch (Exception)
            {
            }
        }

        private void SetControlRoleForServer(string ResultStatus)
        {
            try
            {
                if (ResultStatus == "C" || ResultStatus == "F")
                {
                    SetControlDefault("");
                    lblServerApplyYN.IsVisible = false;
                    ServerApplyYN.IsVisible = true;
                    lblServerApplyMemo.IsVisible = false;
                    txtServerApplyMemo.IsVisible = true;
                    //btnSaveImpResult.IsVisible = true;
                    sltSaveImpResult.IsVisible = true;
                }
                else
                {
                    SetControlDefault(ResultStatus);
                }
            }
            catch (Exception)
            {
            }
        }

        private void SetControlRoleForArea(string ResultStatus)
        {
            try
            {
                if (ResultStatus == "E")
                {
                    SetControlDefault("");
                    lblArearApplyYN.IsVisible = false;
                    AreaApplyYN.IsVisible = true;
                    lblAreaApplyMemo.IsVisible = false;
                    txtAreaApplyMemo.IsVisible = true;
                    //btnSaveImpResult.IsVisible = true;
                    sltSaveImpResult.IsVisible = true;
                }
                else
                {
                    SetControlDefault(ResultStatus);
                }
            }
            catch (Exception)
            {
            }
        }
        private void SetControlDefault(string ResultStatus)
        {
            try
            {
                lblImpResultContent.IsVisible = true;
                txtImpResultContent.IsVisible = false;
                lstImpResultAttachNoDelete.IsVisible = true;
                lstImpResultAttach.IsVisible = false;
                layApply.IsVisible = true;
                layServeApply.IsVisible = true;
                lblServerApplyYN.IsVisible = true;
                ServerApplyYN.IsVisible = false;
                lblServerApplyMemo.IsVisible = true;
                txtServerApplyMemo.IsVisible = false;
                layAreaApply.IsVisible = true;
                lblArearApplyYN.IsVisible = true;
                AreaApplyYN.IsVisible = false;
                lblAreaApplyMemo.IsVisible = true;
                txtAreaApplyMemo.IsVisible = false;

                btnAttachUpload.IsVisible = false;
                btnAttachUploadVedio.IsVisible = false;
                btnAttachUploadFile.IsVisible = false;
                //btnSaveImpResult.IsVisible = false;
                sltSaveImpResult.IsVisible = false;
            }
            catch (Exception)
            {
            }
        }


        private void SetControlRoleForServerByServer(string ResultStatus)
        {
            try
            {
                if (ResultStatus == "A")
                {
                    layApply.IsVisible = false;
                    lblImpResultContent.IsVisible = false;
                    txtImpResultContent.IsVisible = true;
                    btnAttachUpload.IsVisible = true;
                    btnAttachUploadVedio.IsVisible = true;
                    if (Device.OS == TargetPlatform.iOS)
                    {
                        btnAttachUploadFile.IsVisible = false;
                    }
                    else
                    {
                        btnAttachUploadFile.IsVisible = true;
                    }
                    lstImpResultAttach.IsVisible = true;
                    lstImpResultAttachNoDelete.IsVisible = false;
                    //btnSaveImpResult.IsVisible = true;
                    sltSaveImpResult.IsVisible = true;
                }
                else if (ResultStatus == "F")
                {
                    layApply.IsVisible = true;
                    layServeApply.IsVisible = true;
                    lblImpResultContent.IsVisible = false;
                    txtImpResultContent.IsVisible = true;
                    btnAttachUpload.IsVisible = true;
                    btnAttachUploadVedio.IsVisible = true;
                    if (Device.OS == TargetPlatform.iOS)
                    {
                        btnAttachUploadFile.IsVisible = false;
                    }
                    else
                    {
                        btnAttachUploadFile.IsVisible = true;
                    }
                    lstImpResultAttach.IsVisible = true;
                    lstImpResultAttachNoDelete.IsVisible = false;
                    //btnSaveImpResult.IsVisible = true;
                    sltSaveImpResult.IsVisible = true;
                }
                else
                {
                    SetControlDefault(ResultStatus);
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}
