using MaxInsight.Mobile.ViewModels.Improve;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace MaxInsight.Mobile.Pages.Improve
{
    public partial class ImproveDistributionPage : ContentPage
    {
        ToolbarItem tbCommint;
        ToolbarItem tbCancel;
        public ImproveDistributionPage()
        {
            InitializeComponent();
            try
            {
                image.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(ShowOrHideImage),
                    NumberOfTapsRequired = 1
                });
                standard.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(ShowOrHideStandard),
                    NumberOfTapsRequired = 1
                });
            }
            catch (Exception)
            {
            }
            //previewImage.GestureRecognizers.Add(new TapGestureRecognizer()
            //{
            //    Command = new Command(PreviewImage),
            //    NumberOfTapsRequired = 1
            //});
        }

        private void PreviewImage()
        {
            try
            {
                MessagingCenter.Send<ImproveDistributionPage>(this, "PreviewImage");
            }
            catch (Exception)
            {
            }
        }

        private void ShowOrHideStandard()
        {
            try
            {
                if (stackStandard.IsVisible)
                {
                    stackStandard.IsVisible = false;
                    twoImage.Source = ImageSource.FromFile("icon_hide");
                }
                else
                {
                    stackStandard.IsVisible = true;
                    twoImage.Source = ImageSource.FromFile("icon_show");
                }
            }
            catch (Exception)
            {
            }
        }

        private void ShowOrHideImage()
        {
            try
            {
                if (stackImage.IsVisible)
                {
                    stackImage.IsVisible = false;
                    oneImage.Source = ImageSource.FromFile("icon_hide");
                }
                else
                {
                    stackImage.IsVisible = true;
                    oneImage.Source = ImageSource.FromFile("icon_show");
                }
            }
            catch (Exception)
            {
            }
        }

        private async void OnOpenDepartmentPopupPage(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushPopupAsync(new DepartmentPopupPage(""));
            }
            catch (Exception)
            {
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                stackImage.IsVisible = false;
                stackStandard.IsVisible = false;
                oneImage.Source = ImageSource.FromFile("icon_hide");
                twoImage.Source = ImageSource.FromFile("icon_hide");
                if (CommonContext.Account.UserType == "S" && CommonContext.ImpPlanStatus == "A")
                {
                    if (tbCommint == null)
                    {
                        tbCommint = new ToolbarItem();
                        tbCommint.Text = "提交";
                        tbCommint.Command = (this.BindingContext as ImproveDistributionViewModel).CommitCommand;
                    }
                    if (tbCancel == null)
                    {
                        tbCancel = new ToolbarItem();
                        tbCancel.Text = "取消";
                        tbCancel.Command = (this.BindingContext as ImproveDistributionViewModel).CancelCommand;
                    }
                    if (!this.ToolbarItems.Contains(tbCancel))
                    {
                        this.ToolbarItems.Add(tbCancel);
                    }
                    if (!this.ToolbarItems.Contains(tbCommint))
                    {
                        this.ToolbarItems.Add(tbCommint);
                    }
                }
                else
                {
                    if (tbCancel != null && this.ToolbarItems.Contains(tbCancel))
                    {
                        this.ToolbarItems.Remove(tbCancel);
                    }
                    if (tbCommint != null && this.ToolbarItems.Contains(tbCommint))
                    {
                        this.ToolbarItems.Remove(tbCommint);
                    }
                }
                //departmentBtn.IsVisible = false;
                sldepartment.IsVisible = false;
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
                if (mi.Checked)
                    //departmentBtn.IsVisible = true;
                    sldepartment.IsVisible = true;
                else
                    // departmentBtn.IsVisible = false;
                    sldepartment.IsVisible = false;
            }
            catch (Exception)
            {
            }
        }
    }
}
