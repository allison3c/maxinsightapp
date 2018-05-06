using Android.App;
using Android.Content.PM;
using Android.OS;
using Autofac;
using CN.Jpush.Android.Api;
using MaxInsight.Mobile.Droid.Helper;
using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module.Dto;
using MaxInsight.Mobile.Services;
using MaxInsight.Mobile.Services.ImproveService;
using MaxInsight.Mobile.Services.NotifiService;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XLabs.Ioc;
using XLabs.Ioc.Autofac;
using MaxInsight.Mobile.Services.Tour;
using Acr.UserDialogs;
using FFImageLoading.Forms.Droid;
using Plugin.SecureStorage;
using Com.Pgyersdk.Crash;
using MaxInsight.Mobile.Services.CaseService;
using MaxInsight.Mobile.Services.CalendarService;
using Plugin.FilePicker.Abstractions;
using Plugin.FilePicker;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Services.ReviewPlansService;
using MaxInsight.Mobile.Data;
using Android.Widget;
using MaxInsight.Mobile.Services.RemoteService;
using MaxInsight.Mobile.Domain;
using MaxInsight.Mobile.Module.Dto.Shops;
using MaxInsight.Mobile.Services.ReportService;
using MaxInsight.Mobile.Module.Dto.Case;

namespace MaxInsight.Mobile.Droid
{
    [Activity(Label = "MaxInsight.Mobile", Icon = "@drawable/icon", Theme = "@style/AppTheme",
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
              ScreenOrientation = ScreenOrientation.Portrait,
              WindowSoftInputMode = Android.Views.SoftInput.StateVisible | Android.Views.SoftInput.AdjustResize)]
    public class MainActivity : FormsAppCompatActivity
    {
        CommonFun_Droid _common;
        CommonHelper commonhelper;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            JPushInterface.SetDebugMode(true);
            JPushInterface.Init(this);
            _common = new CommonFun_Droid();
            CopyLocalDB();
            //ToolbarResource = Resource.Layout.toolbar_layout;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
            }

            if (!Resolver.IsSet)
            {
                this.SetIoc();
            }
            //else
            //{
            //    var app = Resolver.Resolve<IXFormsApp>() as IXFormsApp<XFormsApplicationDroid>;
            //    if (app != null) app.AppContext = this;
            //}

            global::Xamarin.Forms.Forms.Init(this, bundle);

            commonhelper = Resolver.Resolve<CommonHelper>();
            if (commonhelper.IsNetWorkConnected())
            {
                //PgySdk
                PgyCrashManager.Register(this);
            }
            UserDialogs.Init(this);
            CachedImageRenderer.Init();
            SecureStorageImplementation.StoragePassword = "ELANDRMMT";
            RegistUpdate();

            Xamarin.Forms.Forms.ViewInitialized += (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.View.StyleId))
                {
                    e.NativeView.ContentDescription = e.View.StyleId;
                }
            };
            LoadApplication(new App());
            BackPressed += MainActivity_BackPressed;

            string regID = JPushInterface.GetRegistrationID(ApplicationContext);
            App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
            App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
        }

        public void CopyLocalDB()
        {
            string dbName = "MaxInsight.db3";
            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbName);
            try
            {
                if (!System.IO.File.Exists(dbPath))
                {
                    using (var asset = Assets.Open(dbName))
                    using (var dest = System.IO.File.Create(dbPath))
                    {
                        asset.CopyTo(dest);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Long).Show();
            }
        }

        private void SetIoc()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AccountService>().As<IAccountService>();
            builder.RegisterType<NotifiMngService>().As<INotifiMngService>();
            builder.RegisterType<ImproveService>().As<IImproveService>();
            builder.RegisterType<TourService>().As<ITourService>();
            builder.RegisterType<CaseService>().As<ICaseService>();
            builder.RegisterType<ReviewPlansService>().As<IReviewPlansService>();
            builder.RegisterType<CalendarService>().As<ICalendarService>();
            builder.RegisterType<RemoteService>().As<IRemoteService>();
            builder.RegisterType<LocalScoreService>().As<ILocalScoreService>();
            builder.RegisterType<ReportService>().As<IReportService>();
            #region sqlite
            builder.RegisterType<SqliteRepository<TaskOfPlanDto>>().As<IRepository<TaskOfPlanDto>>();
            builder.RegisterType<SqliteRepository<Distributor>>().As<IRepository<Distributor>>();
            builder.RegisterType<SqliteRepository<LoaclItemOfTaskDto>>().As<IRepository<LoaclItemOfTaskDto>>();
            builder.RegisterType<SqliteRepository<CheckStandard>>().As<IRepository<CheckStandard>>();
            builder.RegisterType<SqliteRepository<StandardPic>>().As<IRepository<StandardPic>>();
            builder.RegisterType<SqliteRepository<PictureStandard>>().As<IRepository<PictureStandard>>();
            builder.RegisterType<SqliteRepository<Score>>().As<IRepository<Score>>();
            builder.RegisterType<SqliteRepository<TaskOfPlan>>().As<IRepository<TaskOfPlan>>();
            builder.RegisterType<SqliteRepository<CheckResult>>().As<IRepository<CheckResult>>();
            builder.RegisterType<SqliteRepository<Domain.StandardPic>>().As<IRepository<Domain.StandardPic>>();
            builder.RegisterType<SqliteRepository<CustomizedTaskDto>>().As<IRepository<CustomizedTaskDto>>();
            builder.RegisterType<SqliteRepository<PlanDto>>().As<IRepository<PlanDto>>();
            builder.RegisterType<SqliteRepository<CustImproveItemDB>>().As<IRepository<CustImproveItemDB>>();
            builder.RegisterType<SqliteRepository<TaskCard>>().As<IRepository<TaskCard>>();
            builder.RegisterType<SqliteRepository<TaskItem>>().As<IRepository<TaskItem>>();
            builder.RegisterType<SqliteRepository<NameValueObject>>().As<IRepository<NameValueObject>>();

            #endregion
            builder.RegisterType<CommonFun_Droid>().As<ICommonFun>();
            builder.RegisterType<FilePickerImplementation>().As<IFilePicker>();
            builder.RegisterInstance(new CommonHelper(new Config.Config()));
            builder.RegisterInstance(new Config.Config());
            builder.RegisterType<SQLite_Android>().As<ISQLite>();
            builder.Register(ctx =>
            {
                return new AccountInfo();
            });

            var container = builder.Build();
            var resolverContainer = new AutofacContainer(container);
            Resolver.SetResolver(resolverContainer.GetResolver());
        }
        //bool doubleBackToExitPressedOnce = false;

        private bool MainActivity_BackPressed(object sender, System.EventArgs e)
        {
            var page = Xamarin.Forms.Application.Current.MainPage;
            return true;
            //if ((page is NavigationPage) && (page as NavigationPage).CurrentPage.ToString() != "MaxInsight.Mobile.Pages.IndexPage")
            //{
            //    return true;
            //}

            //if (!(page is NavigationPage) && (page.ToString() != "MaxInsight.Mobile.Pages.LoginPage"))
            //{
            //    return true;
            //}

            //if (doubleBackToExitPressedOnce)
            //{
            //    NotificationManager notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
            //    notificationManager.Cancel(10001000);
            //    notificationManager.Dispose();

            //    try
            //    {
            //        this.Finish();
            //        Java.Lang.JavaSystem.Exit(0);
            //        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            //    }
            //    catch (Exception)
            //    {
            //        Java.Lang.JavaSystem.Exit(1);
            //    }
            //    return true;
            //}

            //this.doubleBackToExitPressedOnce = true;
            ////Toast.MakeText(this, "再按一次退出程序", ToastLength.Short).Show();

            //new Handler().PostDelayed(() =>
            //{
            //    doubleBackToExitPressedOnce = false;
            //}, 2000);

            //return true;
        }

        private void RegistUpdate()
        {
            MessagingCenter.Subscribe<IOSVersionInfoDto>(this, "UpdateApp", m =>
            {
                //update apk start
                _common.DownLoadFile("https://www.pgyer.com/apiv1/app/install?_api_key=9e59ce985118440f3c0b753d2fa8f923&aKey=" + m.appKey + "& password=", ".apk");
                //_common.DownLoadFile("https://www.pgyer.com/apiv1/app/install?_api_key=b2b9d7af1d81a3a596be546390fc7d22&aKey=" + m.appKey + "& password=", ".apk");
            });
        }
    }
}

