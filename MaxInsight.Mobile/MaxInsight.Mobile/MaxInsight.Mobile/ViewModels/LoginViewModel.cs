using MaxInsight.Mobile.Data;
using MaxInsight.Mobile.Domain;
using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module.Dto;
using MaxInsight.Mobile.Pages;
using MaxInsight.Mobile.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels
{
    public class LoginViewModel : ViewModel
    {
        IAccountService _accountService;
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        public LoginViewModel()
        {
            _accountService = Resolver.Resolve<IAccountService>();
            _commonFun = Resolver.Resolve<ICommonFun>();
            _commonHelper = Resolver.Resolve<CommonHelper>();

            //#if DEBUG
            //			UserName = "quyu";
            //			Password = "1234";
            //#endif
        }

        public string UserName { get; set; } //= "bumen";
        public string Password { get; set; } //= "1234";
        private bool IsLogining = true;
        public Command LoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Login();
                });
            }
        }
        private async Task Login()
        {
            if (IsLogining)
            {
                try
                {
                    if (string.IsNullOrEmpty(UserName))
                    {
                        _commonFun.AlertLongText("请输入用户名。");
                        return;
                    }
                    else if (string.IsNullOrEmpty(Password))
                    {
                        _commonFun.AlertLongText("请输入密码。");
                        return;
                    }

                    if (_commonHelper.IsNetWorkConnected() == true)
                    {
                        IsLogining = false;
                        _commonFun.ShowLoading("登录中...");
                        var result = await _accountService.Login(UserName, Password);
                        if (result.ResultCode == Module.ResultType.Success)
                        {

                            _commonFun.JPushSetAlias(UserName.ToLower().Trim());
                            MessagingCenter.Send<string>(UserName.ToLower().Trim(), "RegistPushTags");
                            AccountInfo accountInfo = CommonHelper.DecodeString<AccountInfo>(result.Body);
                            if (accountInfo != null)
                            {
                                accountInfo.LoggedInAt = DateTime.Now;
                                CommonContext.Account = accountInfo;
                                _commonFun.SetCach(CommonContext.USERNAMEKEY, UserName);

                                #region Create Table
                                var conn = Resolver.Resolve<ISQLite>().GetConnection();
                                conn.CreateTable<Attachment>();
                                conn.CreateTable<CalenderPlans>();
                                conn.CreateTable<CasesInfo>();
                                conn.CreateTable<CheckResult>();
                                conn.CreateTable<Domain.CheckStandard>();
                                conn.CreateTable<CodeHidden>();
                                conn.CreateTable<Department>();
                                conn.CreateTable<Distributor>();
                                conn.CreateTable<Employee>();
                                conn.CreateTable<ImprovementApprovalHis>();
                                conn.CreateTable<ImprovementItem>();
                                conn.CreateTable<ImprovementItemResult>();
                                conn.CreateTable<Notice>();
                                conn.CreateTable<NoticeDepart>();
                                conn.CreateTable<NoticeReader>();
                                conn.CreateTable<NoticeReplyHis>();
                                conn.CreateTable<Domain.PictureStandard>();
                                conn.CreateTable<Plans>();
                                conn.CreateTable<ProcessDtl>();
                                conn.CreateTable<ProcessMst>();
                                conn.CreateTable<PushSend>();
                                conn.CreateTable<Score>();
                                conn.CreateTable<Domain.StandardPic>();
                                conn.CreateTable<TaskCard>();
                                conn.CreateTable<TaskItem>();
                                conn.CreateTable<TaskOfPlan>();
                                #endregion

                                #region Bottom Menu
                                TabbedBarPage _bottomPage = new TabbedBarPage() { Title = "服务" };

                                var _messagePage = new MessagePage()
                                {
                                    Title = "消息",
                                    Icon = (FileImageSource)FileImageSource.FromFile("message")
                                };

                                _bottomPage.Children.Add(_messagePage);

                                var _mainPage = new MainPage()
                                {
                                    Title = "服务",
                                    Icon = (FileImageSource)FileImageSource.FromFile("server")
                                };

                                _bottomPage.Children.Add(_mainPage);

                                var _userPage = new UserPage()
                                {
                                    Title = "我的",
                                    Icon = (FileImageSource)FileImageSource.FromFile("me")
                                };

                                _bottomPage.Children.Add(_userPage);


                                _bottomPage.CurrentPageChanged += (sender, e) =>
                                {
                                    _bottomPage.Title = "全景经营能力PCM评估改善平台";//_bottomPage.CurrentPage.Title;
                                    if (Device.OS == TargetPlatform.Android)
                                    {
                                        _bottomPage.BarTextColor = Color.FromHex("#6281AB"); //底端Menu，当前菜单的颜色
                                    }
                                };

                                _bottomPage.CurrentPage = _bottomPage.Children[1];
                                #endregion

                                #region DownLoadData
                                _commonFun.HideLoading();
                                if (accountInfo.UserType == "Z")
                                {
                                    //if (await _commonFun.Confirm("登录成功，是否同步数据？"))
                                    //{
                                    await MasterDataDownloadHelper.DownloadData();
                                    //}
                                }

                                #endregion

                                //BarTextColor 导航栏字体颜色
                                //BarBackgroundColor 导航栏背景颜色
                                Application.Current.MainPage = new NavigationPage(_bottomPage)
                                {
                                    BarTextColor = Color.White,
                                    BarBackgroundColor = Color.FromHex("#6281AB"),
                                    BackgroundColor = Color.FromHex("#6281AB")
                                };
                            }
                            else
                            {
                                _commonFun.AlertLongText("用户名或者密码不正确。");
                                IsLogining = true;
                            }
                        }
                        else
                        {
                            _commonFun.HideLoading();
                            _commonFun.AlertLongText("登录失败，请重试。 " + result.Msg);
                            IsLogining = true;
                        }
                    }
                    else
                    {
                        _commonFun.AlertLongText("网络连接异常。");
                        IsLogining = true;
                    }
                }
                catch (OperationCanceledException)
                {
                    _commonFun.HideLoading();
                    _commonFun.AlertLongText("请求超时。");
                    IsLogining = true;
                }
                catch (Exception ex)
                {
                    _commonFun.HideLoading();
                    _commonFun.AlertLongText("登录异常，请重试。");
                    IsLogining = true;
                }
                finally
                {
                    _commonFun.HideLoading();
                }
            }
        }
    }
}
