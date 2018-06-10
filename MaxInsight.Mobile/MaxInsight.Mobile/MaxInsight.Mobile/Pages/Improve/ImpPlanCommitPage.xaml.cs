using MaxInsight.Mobile.Module;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace MaxInsight.Mobile.Pages.Improve
{
    public partial class ImpPlanCommitPage : ContentPage
    {
        public ImpPlanCommitPage()
        {
            InitializeComponent();

            //if (CommonContext.Account.UserType == "D")
            //{
            //   Title = "改善计划提交";
            //}
            // else
            // {
            //   Title = "改善计划审批";
            //}
            try
            {
                ServerApplyYN.ItemsSource = new[] { "通过", "拒绝" };
                AreaApplyYN.ItemsSource = new[] { "通过", "拒绝" };

                MessagingCenter.Unsubscribe<string>(this, MessageConst.IMPROVE_PLANCOMMIT_SETCONTROLROLE);
                MessagingCenter.Subscribe<string>(
                    this,
                    MessageConst.IMPROVE_PLANCOMMIT_SETCONTROLROLE,
                    (arg) =>
                    {
                        var array = arg.Split('§');
                        string planStatus = array[0];
                        string planApproalYN = array[1];
                        string allocateYN = array[2];

                        if (allocateYN.ToUpper() == "FALSE")
                            SetControlRoleByServer(planStatus);
                        else
                            SetControlRole(planStatus);

                        if (planApproalYN.ToUpper() == "FALSE")
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

        protected override void OnAppearing()
        {
            try
            {
                MessagingCenter.Subscribe<ImpPlanCommitPage>(this, "PlanCommitPopBack", (obj) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Navigation.PopAsync(true);
                    });
                });
            }
            catch (Exception)
            {
            }
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            try
            {
                MessagingCenter.Unsubscribe<RegistScorePage>(this, "PlanCommitPopBack");
            }
            catch (Exception)
            {
            }
            base.OnDisappearing();
        }

        #region event

        public void OnDeletePlanAttechment(object sender, EventArgs e)
        {
            try
            {
                ImageButton img = sender as ImageButton;
                MessagingCenter.Send<string>(img.CommandParameter.ToString(), MessageConst.IMPROVE_PLANATTACH_DELETE);
            }
            catch (Exception)
            {
            }
        }

        private void OnServerCheckedChanged(object sender, int e)
        {
            try
            {
                MessagingCenter.Send<List<RequestParameter>>(new List<RequestParameter> { new RequestParameter { Name = "Server", Value = e.ToString() } }, MessageConst.IMPROVE_PLANAPPLYYN_CHANGE);
            }
            catch (Exception)
            {
            }
        }
        private void OnAreaCheckedChanged(object sender, int e)
        {
            try
            {
                MessagingCenter.Send<List<RequestParameter>>(new List<RequestParameter> { new RequestParameter { Name = "Area", Value = e.ToString() } }, MessageConst.IMPROVE_PLANAPPLYYN_CHANGE);
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region private
        private void SetControlRole(string planStatus)
        {
            try
            {
                switch (CommonContext.Account.UserType)
                {
                    case "D":
                        SetControlRoleForDepartment(planStatus);
                        break;
                    case "S":
                        SetControlRoleForServer(planStatus);
                        break;
                    case "Z":
                        SetControlRoleForArea(planStatus);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        private void SetControlRoleByServer(string planStatus)
        {
            try
            {
                switch (CommonContext.Account.UserType)
                {
                    case "S":
                        SetControlRoleForServerByServer(planStatus);
                        break;
                    case "Z":
                        SetControlRoleForArea(planStatus);
                        break;
                    default:
                        SetControlDefault(planStatus);
                        break;
                }
                layServeApply.IsVisible = false;
            }
            catch (Exception)
            {
            }
        }

        private void SetControlRoleForDepartment(string planStatus)
        {
            try
            {
                if (planStatus == "B")
                {
                    layApply.IsVisible = false;
                    lblImpPlanContent.IsVisible = false;
                    txtImpPlanContent.IsVisible = true;
                    lblCompletDate.IsVisible = false;
                    dateCompletDate.IsVisible = true;
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
                    lstImpPlanAttach.IsVisible = true;
                    lstImpPlanAttachNoDelete.IsVisible = false;
                    //btnSaveImpPlan.IsVisible = true;
                    sltSaveImpPlan.IsVisible = true;

                }
                else if (planStatus == "D")
                {
                    layApply.IsVisible = true;
                    layServeApply.IsVisible = true;
                    lblImpPlanContent.IsVisible = false;
                    txtImpPlanContent.IsVisible = true;
                    lblCompletDate.IsVisible = false;
                    dateCompletDate.IsVisible = true;
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
                    lstImpPlanAttach.IsVisible = true;
                    lstImpPlanAttachNoDelete.IsVisible = false;
                    //btnSaveImpPlan.IsVisible = true;
                    sltSaveImpPlan.IsVisible = true;
                }
                else
                {
                    SetControlDefault(planStatus);
                }
            }
            catch (Exception)
            {
            }
        }

        private void SetControlRoleForServer(string planStatus)
        {
            try
            {
                if (planStatus == "C" || planStatus == "F")
                {
                    SetControlDefault("");
                    lblServerApplyYN.IsVisible = false;
                    ServerApplyYN.IsVisible = true;
                    lblServerApplyMemo.IsVisible = false;
                    txtServerApplyMemo.IsVisible = true;
                    //btnSaveImpPlan.IsVisible = true;
                    sltSaveImpPlan.IsVisible = true;
                    lstImpPlanAttachNoDelete.IsVisible = true;
                }
                else
                {
                    SetControlDefault(planStatus);
                }
            }
            catch (Exception)
            {
            }
        }

        private void SetControlRoleForArea(string planStatus)
        {
            try
            {
                if (planStatus == "E")
                {
                    SetControlDefault("");
                    lblArearApplyYN.IsVisible = false;
                    AreaApplyYN.IsVisible = true;
                    lblAreaApplyMemo.IsVisible = false;
                    txtAreaApplyMemo.IsVisible = true;
                    //btnSaveImpPlan.IsVisible = true;
                    sltSaveImpPlan.IsVisible = true;
                    lstImpPlanAttachNoDelete.IsVisible = true;
                }
                else
                {
                    SetControlDefault(planStatus);
                }
            }
            catch (Exception)
            {
            }
        }

        private void SetControlDefault(string planStatus)
        {
            try
            {
                lblImpPlanContent.IsVisible = true;
                txtImpPlanContent.IsVisible = false;
                lblCompletDate.IsVisible = true;
                dateCompletDate.IsVisible = false;
                lstImpPlanAttachNoDelete.IsVisible = true;
                lstImpPlanAttach.IsVisible = false;
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
                //btnSaveImpPlan.IsVisible = false;
                sltSaveImpPlan.IsVisible = false;
            }
            catch (Exception)
            {
            }
        }

        private void SetControlRoleForServerByServer(string planStatus)
        {
            try
            {
                if (planStatus == "B")
                {
                    layApply.IsVisible = false;
                    lblImpPlanContent.IsVisible = false;
                    txtImpPlanContent.IsVisible = true;
                    lblCompletDate.IsVisible = false;
                    dateCompletDate.IsVisible = true;
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
                    lstImpPlanAttach.IsVisible = true;
                    lstImpPlanAttachNoDelete.IsVisible = false;
                    //btnSaveImpPlan.IsVisible = true;
                    sltSaveImpPlan.IsVisible = true;

                }
                else if (planStatus == "F")
                {
                    layApply.IsVisible = true;
                    layServeApply.IsVisible = true;
                    lblImpPlanContent.IsVisible = false;
                    txtImpPlanContent.IsVisible = true;
                    lblCompletDate.IsVisible = false;
                    dateCompletDate.IsVisible = true;
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
                    lstImpPlanAttach.IsVisible = true;
                    lstImpPlanAttachNoDelete.IsVisible = false;
                    //btnSaveImpPlan.IsVisible = true;
                    sltSaveImpPlan.IsVisible = true;
                }
                else
                {
                    SetControlDefault(planStatus);
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}
