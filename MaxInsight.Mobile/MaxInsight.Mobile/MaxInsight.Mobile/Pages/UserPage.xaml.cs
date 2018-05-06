using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Pages.User;
using MaxInsight.Mobile.ViewModels.User;
using System;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.Pages
{
    public partial class UserPage : BaseContentPage
    {
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        IOSVersionInfoDto info = null;
        bool IsDoaning = false;
        public UserPage()
        {
            InitializeComponent();
            _commonFun = Resolver.Resolve<ICommonFun>();
            _commonHelper = Resolver.Resolve<CommonHelper>();

            lblDisName.Text = CommonContext.Account.DisName;
            lblUserTypeName.Text = CommonContext.Account.UserTypeName;
            txtTelNo.Text = CommonContext.Account.Phone;
            layPassword.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    try
                    {
                        await Navigation.PushAsync(ViewFactory.CreatePage<ChangePasswordViewModel, ChangePasswordPage>() as Page, true);
                    }
                    catch (System.Exception)
                    {
                    }
                })
            });
            layCheckUpdate.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    try
                    {
                        if (IsDoaning)
                        {
                            string msg = "";
                            if (App.SysOS == "Android")
                            {
                                msg = "安装程序正在后台下载";
                            }
                            else
                            {
                                msg = "按Home键回到桌面即可看到更新进度";
                            }
                            _commonFun.AlertLongText(msg);
                        }
                        else
                        {
                            if (info != null && !string.IsNullOrEmpty(info.appKey))
                            {
                                UpdateNewVersion();
                            }
                            else
                            {
                                _commonFun.ShowLoading("检查中...");
                                info = await App.CheckUpdate();
                                _commonFun.HideLoading();
                                if (info != null && !string.IsNullOrEmpty(info.appKey))
                                {
                                    UpdateNewVersion();
                                }
                                else
                                {
                                    _commonFun.AlertLongText("恭喜你，你已经是最新版本了。");
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                })
            });
        }

        protected async override void OnAppearing()
        {
            info = await App.CheckUpdate();
            if (info != null && !string.IsNullOrEmpty(info.appKey))
            {
                lblCheckUpdate.Text = "有新版本发布";
                lblCheckUpdate.TextColor = Color.Red;
            }
            else
            {
                lblCheckUpdate.Text = "检查更新";
                lblCheckUpdate.TextColor = Color.FromHex("A0A0A0");
            }
            base.OnAppearing();
        }
        public void ExistSystem(object sender, EventArgs e)
        {
            Helpers.ICommonFun commonFun = XLabs.Ioc.Resolver.Resolve<Helpers.ICommonFun>();
            commonFun.ExistSystem();
        }
        private async void UpdateNewVersion()
        {
            if (await _commonFun.Confirm("新的版本已发布，是否更新?"))
            {
                MessagingCenter.Send<IOSVersionInfoDto>(info, "UpdateApp");
                IsDoaning = true;
                info = null;
            }
        }
    }
}
