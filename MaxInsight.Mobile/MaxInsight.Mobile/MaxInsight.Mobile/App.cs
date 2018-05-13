using Autofac;
using MaxInsight.Mobile.Module.Dto;
using MaxInsight.Mobile.Pages;
using MaxInsight.Mobile.Pages.Notifi;
using MaxInsight.Mobile.Services;
using MaxInsight.Mobile.ViewModels;
using MaxInsight.Mobile.Pages.Improve;
using MaxInsight.Mobile.ViewModels.Improve;
using MaxInsight.Mobile.ViewModels.Notifi;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;
using XLabs.Ioc.Autofac;
using System;
using Acr.UserDialogs;
using MaxInsight.Mobile.Pages.Cases;
using MaxInsight.Mobile.ViewModels.Cases;
using MaxInsight.Mobile.Pages.Common;
using MaxInsight.Mobile.ViewModels.Common;
using MaxInsight.Mobile.Pages.Calendar;
using MaxInsight.Mobile.ViewModels.User;
using MaxInsight.Mobile.ViewModels.Calendar;
using MaxInsight.Mobile.Pages.User;
using MaxInsight.Mobile.Module;
using System.Threading.Tasks;
using MaxInsight.Mobile.Helpers;
using System.Collections.Generic;
using MaxInsight.Mobile.ViewModels.ShopfrontPatrolModel;
using MaxInsight.Mobile.Pages.ShopfrontPatrol;
using MaxInsight.Mobile.Pages.ReviewPlans;
using MaxInsight.Mobile.ViewModels.ReviewPlans;
using MaxInsight.Mobile.Pages.Report;
using MaxInsight.Mobile.ViewModels.Report;

namespace MaxInsight.Mobile
{
    public class App : Application
    {
        public static int ScreenWidth { get; set; }
        public static int ScreenHeight { get; set; }
        public static string SysOS { get; set; }
        public App()
        {
            // The root page of your application
            //if (!Resolver.IsSet)
            //{
            //    SetIoc(this);
            //}

            var _commonFun = Resolver.Resolve<ICommonFun>();
            SetViewFactory();
            InitPlatform();
            if (Device.OS == TargetPlatform.iOS)
            {
                var firstInstall = _commonFun.GetCach(CommonContext.FIRSTINSTALLYN);
                if (string.IsNullOrEmpty(firstInstall))
                {
                    MainPage = ViewFactory.CreatePage<IOSWelcomViewModel, IOSWelcomView>() as Page;
                    _commonFun.SetCach(CommonContext.FIRSTINSTALLYN, "1");
                }
                else
                {
                    if (!CommonContext.IsUserLoggedIn)
                    {
                        MainPage = ViewFactory.CreatePage<LoginViewModel, LoginPage>() as Page;
                    }
                    else
                    {
                        MainPage = new NavigationPage(ViewFactory.CreatePage<MainViewModel, MainPage>() as Page);
                    }
                }
            }
            else
            {
                if (!CommonContext.IsUserLoggedIn)
                {
                    MainPage = ViewFactory.CreatePage<LoginViewModel, LoginPage>() as Page;
                }
                else
                {
                    MainPage = new NavigationPage(ViewFactory.CreatePage<MainViewModel, MainPage>() as Page);
                }
            }

            MainPage.BackgroundColor = Color.FromHex("#ECF0F1");
        }

        private void SetViewFactory()
        {
            ViewFactory.EnableCache = true;
            ViewFactory.Register<LoginPage, LoginViewModel>();
            ViewFactory.Register<MainPage, MainViewModel>();
            ViewFactory.Register<NotifiIndexPage, NotifiIndexViewModel>();
            ViewFactory.Register<NotifiMngPage, NotifiMngViewModel>();
            ViewFactory.Register<UserPage, UserViewModel>();
            ViewFactory.Register<IOSWelcomPage, IOSWelcomViewModel>();
            ViewFactory.Register<ImpPlanCommitPage, ImpPlanCommitViewModel>();
            ViewFactory.Register<ImproveIndexPage, ImproveIndexViewModel>();
            ViewFactory.Register<ImproveSearchPage, ImproveSearchViewModel>();
            ViewFactory.Register<ImproveSearchConditionPage, ImproveSearchConditionViewModel>();
            ViewFactory.Register<NotifiMngSearchPage, NotifiMngSearchViewModel>();
            ViewFactory.Register<NotifiMngSearchConditionPage, NotifiMngSearchConditionViewModel>();
            ViewFactory.Register<ShopfrontMainPage, ShopfrontMainPageViewModel>();
            ViewFactory.Register<TaskListPage, TaskListViewModel>();
            ViewFactory.Register<SystemListPage, SystemListViewModel>();
            ViewFactory.Register<RegistScorePage, RegistScoreViewModel>();
            ViewFactory.Register<ImpResultCommitPage, ImpResultCommitViewModel>();
            ViewFactory.Register<RegistScorePage, RegistScoreViewModel>();
            ViewFactory.Register<ImproveDistributionPage, ImproveDistributionViewModel>();
            ViewFactory.Register<UpdatePopPage, UpdatePopViewModel>();
            ViewFactory.Register<CaseRegPage, CaseRegViewModel>();
            ViewFactory.Register<CaseSearchPage, CaseSearchViewModel>();
            ViewFactory.Register<CasesIndexPage, CaseIndexViewModel>();
            ViewFactory.Register<CaseSearchResultPage, CaseSearchResultViewModel>();
            ViewFactory.Register<ViewRecordPage, ViewRecordViewModel>();
            ViewFactory.Register<ViewRegistScorePage, ViewRegistScoreViewModel>();
            ViewFactory.Register<ComSinglePopPage, ComSinglePopViewModel>();
            ViewFactory.Register<NotifiMngReaderListPage, NotifiMngReaderListViewModel>();
            ViewFactory.Register<NoticeApproalPage, NoticeApproalViewModel>();
            ViewFactory.Register<NoticeApproalLogPage, NoticeApproalLogViewModel>();
            ViewFactory.Register<NotifiFeedbackPage, NotifiFeedbackViewModel>();
            ViewFactory.Register<ChangePasswordPage, ChangePasswordViewModel>();
            ViewFactory.Register<CalendarIndexPage, CalendarIndexViewModel>();
            ViewFactory.Register<CalendarRegPage, CalendarRegViewModel>();
            ViewFactory.Register<CustomizedTaskPage, CustomizedTaskViewModel>();
            ViewFactory.Register<ReviewPlansIndexPage, ReviewPlansIndexViewModel>();
            ViewFactory.Register<ReviewPlansDtlPage, ReviewPlansViewModel>();
            ViewFactory.Register<LocalSystemListPage, LocalSystemListViewModel>();
            ViewFactory.Register<LocalRegistScorePage, LocalRegistScoreViewModel>();
            ViewFactory.Register<ReportSearchIndexPage, ReportSearchIndexViewModel>();
            ViewFactory.Register<ReportSearchPage, ReportSearchViewModel>();
            ViewFactory.Register<CustImprovePage, CustImproveViewModel>();
            ViewFactory.Register<PushInfoPage, PushInfoViewModel>();
            //ViewFactory.Register<PreviewImagePage, PreviewImageViewModel>();
        }


        //private void SetMvvMIoc() { 
        //	var resolverContainer = new SimpleContainer();
        //	resolverContainer.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);
        //}

        private void SetIoc(App instance)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AccountService>().As<IAccountService>();
            //builder.RegisterType<CommonFun_Droid>().As<ICommonFun>();
            builder.RegisterInstance(new Config.Config());
            builder.Register(ctx =>
            {
                return new AccountInfo();
            });

            var container = builder.Build();
            var resolverContainer = new AutofacContainer(container);
            Resolver.SetResolver(resolverContainer.GetResolver());
        }


        /// <summary>
        /// Shows the loading indicator.
        /// </summary>
        /// <param name="isRunning">If set to <c>true</c> is running.</param>
        /// <param name = "isCancel">If set to <c>true</c> user can cancel the loading event (just uses PopModalAync here)</param>
        public static void ShowLoading(bool isRunning, bool isCancel = false)
        {
            if (isRunning == true)
            {
                if (isCancel == true)
                {
                    UserDialogs.Instance.Loading("Loading", new Action(async () =>
                    {
                        if (Application.Current.MainPage.Navigation.ModalStack.Count > 1)
                        {
                            await Application.Current.MainPage.Navigation.PopModalAsync();
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Navigation: Can't pop modal without any modals pushed");
                        }
                        UserDialogs.Instance.Loading().Hide();
                    }));
                }
                else
                {
                    UserDialogs.Instance.Loading("Loading");
                }
            }
            else
            {
                UserDialogs.Instance.Loading().Hide();
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public async static Task<IOSVersionInfoDto> CheckUpdate()
        {
            IOSVersionInfoDto info = new IOSVersionInfoDto();
            var commonHelper = Resolver.Resolve<CommonHelper>();
            var commonFun = Resolver.Resolve<ICommonFun>();
            string shortcutName = "";
            if (Device.OS == TargetPlatform.iOS)
            {
                shortcutName = "pcm_android";
                //shortcutName = "maxinsight_droid";
            }
            else
            {
                shortcutName = "pcm_ios";
                //shortcutName = "maxinsight_ios";
            }

            List<RequestParameter> shortcutList = new List<RequestParameter>()
            {
                new RequestParameter()
                {
                    Name = "shortcut",
                    Value = shortcutName
                },
                new RequestParameter()
                {
                    Name = "_api_key",
                    //Value = "9e59ce985118440f3c0b753d2fa8f923"
                    Value = "b2b9d7af1d81a3a596be546390fc7d22"
                }
            };
            PgyAppInfoShortcutInfo shortcutDto = null;
            try
            {
                shortcutDto = await commonHelper.HttpGetPOST<PgyAppInfoShortcutInfo>("https://www.pgyer.com", "/apiv1/app/getAppKeyByShortcut", shortcutList);
            }
            catch (Exception)
            {
                return info;
            }

            if (shortcutDto == null || shortcutDto.data == null)
            {
                return info;
            }
            List<RequestParameter> appInfoList = new List<RequestParameter>()
            {
                new RequestParameter()
                {
                    Name = "aKey",
                    Value = shortcutDto.data.appKey
                },
                new RequestParameter()
                {
                    Name = "uKey",
                    //Value = "690728932ff4cabc4a3b2e0ec96f0e24"
                    Value = "dc9a543b1b03cb81e077ddaa07640898"
                },
                new RequestParameter()
                {
                    Name = "_api_key",
                    //Value = "9e59ce985118440f3c0b753d2fa8f923"
                    Value = "b2b9d7af1d81a3a596be546390fc7d22"
                }
            };

            PgyAppInfoDetail appInfoDto = null;
            try
            {
                appInfoDto = await commonHelper.HttpGetPOST<PgyAppInfoDetail>("https://www.pgyer.com", "/apiv1/app/view", appInfoList);
            }
            catch (Exception)
            {
                return info;
            }

            if (appInfoDto == null || appInfoDto.data == null)
            {
                return info;
            }
            int version = commonFun.GetVersionNo();

            if (Convert.ToInt32(appInfoDto.data.appBuildVersion) > version)
            {
                //return appInfoDto.data.appUpdateDescription;
                info.updateMsg = appInfoDto.data.appUpdateDescription;
                info.appKey = appInfoDto.data.appKey;
                info.newVersion = Convert.ToInt32(appInfoDto.data.appVersionNo);
                info.nowVersion = version;
            }
            return info;
        }

        void InitPlatform()
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                SysOS = "IOS";
            }
            else if (Device.OS == TargetPlatform.Android)
            {
                SysOS = "Android";
            }
            else
            {
                SysOS = "";
            }
        }
		public static void GoPreviewImageGesturePage(string url) {
			MessagingCenter.Send(url, "GoPreviewImageGesturePage");
		}
    }
}
