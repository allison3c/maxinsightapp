using System;
using System.IO;
using Autofac;
using FFImageLoading.Forms.Touch;
using FFImageLoading.Transformations;
using Foundation;
using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.iOS.Helper;
using MaxInsight.Mobile.Module.Dto;
using MaxInsight.Mobile.Pages;
using MaxInsight.Mobile.Services;
using MaxInsight.Mobile.Services.ImproveService;
using MaxInsight.Mobile.Services.NotifiService;
using MaxInsight.Mobile.Services.Tour;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Forms;
using XLabs.Ioc;
using XLabs.Ioc.Autofac;
using MaxInsight.Mobile.Services.CaseService;
using com.eland.ios.JPush;
using UserNotifications;
using ObjCRuntime;
using Plugin.LocalNotifications;
using MaxInsight.Mobile.Services.CalendarService;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Services.ReviewPlansService;
using MaxInsight.Mobile.Data;
using MaxInsight.Mobile.Services.RemoteService;
using MaxInsight.Mobile.Domain;
using Plugin.FilePicker.Abstractions;
using Plugin.FilePicker;
using MaxInsight.Mobile.Module.Dto.Shops;
using MaxInsight.Mobile.Services.ReportService;
using WindowsAzure.Messaging;
using MaxInsight.Mobile.Module.Dto.Case;

namespace MaxInsight.Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : XFormsApplicationDelegate //global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private const string ConnectionString = "Endpoint=sb://rmmtpush-ns.servicebus.chinacloudapi.cn/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=cb0VONj0cM89rHJdgM2Whd4a/PHpg++0lDSHF4+MhWc=";
        private const string NotificationHubPath = "rmmtpush";
        private SBNotificationHub Hub { get; set; }
        private NSData _deviceToken;

        //XFormsApplicationDelegate
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            this.SetIoc();
            CachedImageRenderer.Init();

            var ignore = new GrayscaleTransformation();
            Rg.Plugins.Popup.IOS.Popup.Init();

            //JPUSHService.SetDebugMode();

            Xamarin.Behaviors.Infrastructure.Init();

            RegistUpdateNavigationBar();

            global::Xamarin.Forms.Forms.Init();
            CopyLocalDB();
            LoadApplication(new App());

            Xamarin.Forms.Forms.ViewInitialized += (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.View.StyleId))
                {
                    e.NativeView.AccessibilityIdentifier = e.View.StyleId;
                }
            };

            /*****
            //jpush init start
            //string advertisingId = ASIdentifierManager.SharedManager.AdvertisingIdentifier.ToString();
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                //JPUSHRegisterEntity pushEntity = new JPUSHRegisterEntity();
                //pushEntity.Types = 4 | 1 | 2;
                //JPushNotifyEvent notifyEvent = new JPushNotifyEvent();
                //JPUSHService.RegisterForRemoteNotificationConfig(pushEntity, notifyEvent);
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                JPUSHService.RegisterForRemoteNotificationTypes(4 | 1 | 2, new NSSet());
            }
            else
            {
                JPUSHService.RegisterForRemoteNotificationTypes(4 | 1 | 2, new NSSet());
            }

            try
            {
				if (options == null)
				{
					options = new NSDictionary("_j_msgid", "-1", "aps", 
					                           new NSDictionary("alert", "12345", "badge", "0", "sound", "default"));
                    //options.SetValueForKey(new NSString("-1"), new NSString("_j_msgid"));
                    //NSDictionary subDic = new NSDictionary();
                    //subDic.SetValueForKey(new NSString("alert"), new NSString("123456"));
                    //subDic.SetValueForKey(new NSString("badge"), new NSString("1"));
                    //subDic.SetValueForKey(new NSString("sound"), new NSString("default"));
                    //options.SetValueForKey(subDic, new NSString("aps"));

                    JPUSHService.SetupWithOption(options, "35d7e27236095f67b93966b3", "", false, "");
                    //JPUSHService.SetupWithOption(options, "5af3416249f70699720994dd", "", true, "");
                }
                else
				{
					JPUSHService.SetupWithOption(options, "35d7e27236095f67b93966b3", "", false, "");
                    //JPUSHService.SetupWithOption(options, "5af3416249f70699720994dd", "", true, "");
                }
            }
            catch (Exception)
            {

            }
            ******/

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // Request notification permissions from the user
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert
                                                                      | UNAuthorizationOptions.Sound
                                                                      | UNAuthorizationOptions.Badge,
                                                                      (approved, err) =>
                                                                      {
                                                                          // Handle approval
                                                                          if (approved)
                                                                          {
                                                                              UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
                                                                              {
                                                                                  UIApplication.SharedApplication.RegisterForRemoteNotifications();
                                                                                  UNUserNotificationCenter.Current.Delegate = new NotificationDelegate();
                                                                              });
                                                                          }
                                                                      });
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                       UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                       new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }

            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
            //jpush init end
            //app.ApplicationIconBadgeNumber = 0;
            //JPUSHService.ResetBadge();
            ConfigureApplicationTheming();
            App.ScreenHeight = int.Parse(UIScreen.MainScreen.Bounds.Height.ToString());
            App.ScreenWidth = int.Parse(UIScreen.MainScreen.Bounds.Width.ToString());
            return base.FinishedLaunching(app, options);
        }

        private void CopyLocalDB()
        {
            var dbName = "MaxInsight";
            var document = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var filePath = Path.Combine(document, dbName);
            var sourcePath = NSBundle.MainBundle.PathForResource(dbName, "db3");

            if (!File.Exists(filePath))
            {
                File.Copy(sourcePath, filePath);
            }
        }

        private void RegistUpdateNavigationBar()
        {
            MessagingCenter.Subscribe<MainPage>(this, "ChangeNavigation", (obj) =>
            {
                UINavigationBar.Appearance.BackgroundColor = Color.Red.ToUIColor();
            });
        }

        void ConfigureApplicationTheming()
        {
            UINavigationBar.Appearance.TintColor = UIColor.White;
            UINavigationBar.Appearance.BarTintColor = Color.FromHex("#ECF0F1").ToUIColor();
            UINavigationBar.Appearance.TitleTextAttributes = new UIStringAttributes { ForegroundColor = UIColor.White };
            UIBarButtonItem.Appearance.SetTitleTextAttributes(new UITextAttributes { TextColor = UIColor.White }, UIControlState.Normal);

            UITabBar.Appearance.TintColor = UIColor.White;
            UITabBar.Appearance.BarTintColor = UIColor.White;
            UITabBar.Appearance.SelectedImageTintColor = Color.FromHex("#6281AB").ToUIColor();
            UITabBarItem.Appearance.SetTitleTextAttributes(new UITextAttributes { TextColor = Color.FromHex("#6281AB").ToUIColor() }, UIControlState.Selected);

            //UIProgressView.Appearance.ProgressTintColor = UIColor.Red;
        }

        /// <summary>
        /// Sets the IoC.
        /// </summary>
        private void SetIoc()
        {
            var app = new XFormsAppiOS();
            app.Init(this);

            var documents = app.AppDataDirectory;
            var pathToDatabase = Path.Combine(documents, "RMMT.db");

            var builder = new ContainerBuilder();
            builder.RegisterType<AccountService>().As<IAccountService>();
            builder.RegisterType<NotifiMngService>().As<INotifiMngService>();
            builder.RegisterType<ImproveService>().As<IImproveService>();
            builder.RegisterType<TourService>().As<ITourService>();
            builder.RegisterType<CaseService>().As<ICaseService>();
            builder.RegisterType<CalendarService>().As<ICalendarService>();
            builder.RegisterType<CommonFun_IOS>().As<ICommonFun>();
            builder.RegisterType<FilePickerImplementation>().As<IFilePicker>();
            builder.RegisterType<ReviewPlansService>().As<IReviewPlansService>();
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
            builder.RegisterInstance(new CommonHelper(new Config.Config()));
            builder.RegisterInstance(new Config.Config());
            builder.RegisterType<SQLite_iOS>().As<ISQLite>();
            builder.Register(ctx =>
            {
                return new AccountInfo();
            });
            //builder.Register<IDevice>(t => AppleDevice.CurrentDevice);

            var container = builder.Build();
            var resolverContainer = new AutofacContainer(container);

            Resolver.SetResolver(resolverContainer.GetResolver());
        }

        public override void OnActivated(UIApplication application)
        {
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
            MessagingCenter.Subscribe<IOSVersionInfoDto>(this, "UpdateApp", m =>
            {
                UpdateApp(m);
            });
            MessagingCenter.Subscribe<string>(this, "RegistPushTags", (param) =>
            {
                if (Hub != null)
                {
                    Hub.UnregisterAllAsync(_deviceToken, (error) =>
                    {
                        if (error != null)
                        {
                            Console.WriteLine("Error calling Unregister: {0}", error.ToString());
                            return;
                        }

                        NSSet tags = new NSSet(param); // create tags if you want
                        Hub.RegisterNativeAsync(_deviceToken, tags, (errorCallback) =>
                        {
                            if (errorCallback != null)
                                Console.WriteLine("RegisterNativeAsync error: " + errorCallback.ToString());
                        });
                    });
                }
            });
        }

        public override void DidRegisterUserNotificationSettings(UIApplication application, UIUserNotificationSettings notificationSettings)
        {
            UIApplication.SharedApplication.RegisterForRemoteNotifications();
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            //base.ReceivedRemoteNotification(application, userInfo);
            // show an alert
            //UIAlertController okayAlertController = UIAlertController.Create(notification.AlertAction, 
            //                                                                 notification.AlertBody, 
            //                                                                 UIAlertControllerStyle.Alert);
            //okayAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

            //Window.RootViewController.PresentViewController(okayAlertController, true, null);

            //// reset our badge
            //UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
            //JPUSHService.HandleRemoteNotification(userInfo);
            ProcessNotification(userInfo, false);
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            //base.DidReceiveRemoteNotification(application, userInfo, completionHandler);
            //JPUSHService.HandleRemoteNotification(userInfo);
            ProcessNotification(userInfo, false);
            completionHandler(UIBackgroundFetchResult.NewData);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            //JPUSHService.RegisterDeviceToken(deviceToken);
            try
            {
                _deviceToken = deviceToken;
                Hub = new SBNotificationHub(ConnectionString, NotificationHubPath);

                Hub.UnregisterAllAsync(deviceToken, (error) =>
                {
                    if (error != null)
                    {
                        Console.WriteLine("Error calling Unregister: {0}", error.ToString());
                        return;
                    }

                    NSSet tags = null; // create tags if you want
                    Hub.RegisterNativeAsync(deviceToken, tags, (errorCallback) =>
                    {
                        if (errorCallback != null)
                            Console.WriteLine("RegisterNativeAsync error: " + errorCallback.ToString());
                    });
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("RegisterNativeAsync exception:" + ex.Message.ToString());
            }
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            new UIAlertView("Error registering push notifications", error.LocalizedDescription, null, "OK", null).Show();
        }

        public void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            // Check to see if the dictionary has the aps key.  This is the notification payload you would have sent
            if (null != options && options.ContainsKey(new NSString("aps")))
            {
                //Get the aps dictionary
                NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

                string alertString = string.Empty;
                string paramString = string.Empty;
                string alertTitle = string.Empty;
                string alertContent = string.Empty;

                if (aps.ContainsKey(new NSString("alert")))
                    alertString = (aps[new NSString("alert")] as NSString).ToString();
                if (!string.IsNullOrWhiteSpace(alertString) && alertString.Split('§').Length > 1)
                {
                    alertTitle = alertString.Split('§')[0];
                    alertContent = alertString.Split('§')[1];
                }
                else
                {
                    alertTitle = "RMMT";
                    alertContent = alertString;
                }

                if (aps.ContainsKey(new NSString("param")))
                    paramString = (aps[new NSString("param")] as NSString).ToString();

                if (!fromFinishedLaunching)
                {
                    //Manually show an alert
                    if (!string.IsNullOrWhiteSpace(alertString))
                    {
                        UIApplication.SharedApplication.BeginInvokeOnMainThread(delegate
                        {
                            UIApplication.SharedApplication.CancelAllLocalNotifications();
                            CrossLocalNotifications.Current.Cancel(10001000);
                            try
                            {
                                CrossLocalNotifications.Current.Show(alertTitle, alertContent, 10001000);
                            }
                            catch (Exception)
                            {
                            }
                        });
                    }
                }
            }
        }

        void UpdateApp(IOSVersionInfoDto info)
        {
            NSUrl downloadURL = NSUrl.FromString("itms-services://?action=download-manifest&url=https://www.pgyer.com/app/plist/" + info.appKey);
            UIApplication.SharedApplication.OpenUrl(downloadURL);
        }
    }

    public class NotificationDelegate : UNUserNotificationCenterDelegate
    {
        #region Constructors
        public NotificationDelegate()
        {
        }
        #endregion

        #region Override Methods
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            // Do something with the notification
            //Console.WriteLine("Active Notification: {0}", notification);

            // Tell system to display the notification anyway or use
            // `None` to say we have handled the display locally.
            var userInfo = notification.Request.Content.UserInfo;
            ProcessNotification(userInfo, false);
            completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.Badge);
        }

        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            var userInfo = response.Notification.Request.Content.UserInfo;
            ProcessNotification(userInfo, false);
            completionHandler();
        }
        #endregion

        public void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            // Check to see if the dictionary has the aps key.  This is the notification payload you would have sent
            if (null != options && options.ContainsKey(new NSString("aps")))
            {
                //Get the aps dictionary
                NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

                string alertString = string.Empty;
                string paramString = string.Empty;
                string alertTitle = string.Empty;
                string alertContent = string.Empty;

                if (aps.ContainsKey(new NSString("alert")))
                    alertString = (aps[new NSString("alert")] as NSString).ToString();
                if (!string.IsNullOrWhiteSpace(alertString) && alertString.Split('§').Length > 1)
                {
                    alertTitle = alertString.Split('§')[0];
                    alertContent = alertString.Split('§')[1];
                }
                else
                {
                    alertTitle = "RMMT";
                    alertContent = alertString;
                }

                if (aps.ContainsKey(new NSString("param")))
                    paramString = (aps[new NSString("param")] as NSString).ToString();

                if (!fromFinishedLaunching)
                {
                    //Manually show an alert
                    if (!string.IsNullOrWhiteSpace(alertString))
                    {
                        UIApplication.SharedApplication.BeginInvokeOnMainThread(delegate
                        {
                            //UIApplication.SharedApplication.CancelAllLocalNotifications();
                            //CrossLocalNotifications.Current.Cancel(10001000);
                            try
                            {
                                CrossLocalNotifications.Current.Show(alertTitle, alertContent, 10001000);
                            }
                            catch (Exception)
                            {
                            }
                        });
                    }
                }
            }
        }
    }

    /***
    class JPushNotifyEvent : JPUSHRegisterDelegate
    {
        public JPushNotifyEvent() { }

        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center,
                                                            UNNotificationResponse response,
                                                            [BlockProxy(typeof(Action))]Action completionHandler)
        {
            // Required
            NSDictionary userInfo = response.Notification.Request.Content.UserInfo;
            //if ([response.notification.request.trigger isKindOfClass:[UNPushNotificationTrigger class]]) {
            //				[JPUSHService handleRemoteNotification:userInfo];
            //		}
            //completionHandler();  // 系统要求执行这个方法


            //UNPushNotificationTrigger trigger = new UNPushNotificationTrigger(new NSCoder());
            //Type _triigerType = trigger.GetType();
            //ObjCRuntime.Class _class = new Class(_triigerType);

            //if (response.Notification.Request.Trigger.IsKindOfClass(_class))
            //{
            //ProcessNotification(userInfo, false);
            //JPUSHService.HandleRemoteNotification(userInfo);
            //}
            completionHandler();
        }

        public override void WillPresentNotification(UNUserNotificationCenter center,
                                                     UNNotification notification,
                                                     [BlockProxy(typeof(nint))] Action<nint> completionHandler)
        {
            try
            {
                NSDictionary userInfo = notification.Request.Content.UserInfo;

                //if (notification.Request.Trigger.GetType() == typeof(UNPushNotificationTrigger))
                //{
                //	JPUSHService.HandleRemoteNotification(userInfo);
                //}
                //UNPushNotificationTrigger trigger = new UNPushNotificationTrigger(new NSCoder());
                //Type _triigerType = trigger.GetType();
                //ObjCRuntime.Class _class = new Class(_triigerType);

                //if (notification.Request.Trigger.IsKindOfClass(_class))
                //{
                //ProcessNotification(userInfo, false);
                //}
                completionHandler(4);
            }
            catch (Exception)
            {

            }
        }

        public void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            // Check to see if the dictionary has the aps key.  This is the notification payload you would have sent
            if (null != options && options.ContainsKey(new NSString("aps")))
            {
                //Get the aps dictionary
                NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

                string alertString = string.Empty;
                string paramString = string.Empty;
                string alertTitle = string.Empty;
                string alertContent = string.Empty;

                if (aps.ContainsKey(new NSString("alert")))
                    alertString = (aps[new NSString("alert")] as NSString).ToString();
                if (string.IsNullOrWhiteSpace(alertString) && alertString.Split('§').Length > 1)
                {
                    alertTitle = alertString.Split('§')[0];
                    alertContent = alertString.Split('§')[1];
                }
                else
                {
                    alertTitle = "RMMT";
                    alertContent = alertString;
                }

                if (aps.ContainsKey(new NSString("param")))
                    paramString = (aps[new NSString("param")] as NSString).ToString();

                if (!fromFinishedLaunching)
                {
                    //Manually show an alert
                    if (!string.IsNullOrEmpty(alertString))
                    {
                        UIApplication.SharedApplication.BeginInvokeOnMainThread(delegate
                        {
                            UIApplication.SharedApplication.CancelAllLocalNotifications();
                            CrossLocalNotifications.Current.Cancel(10001000);
                            try
                            {
                                CrossLocalNotifications.Current.Show(alertTitle, alertContent, 10001000);
                            }
                            catch (Exception)
                            {
                            }
                        });
                    }
                }
            }
        }
    }
    **/
}
