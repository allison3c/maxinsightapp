using MaxInsight.Mobile.ViewModels.Improve;
using MaxInsight.Mobile.ViewModels.Notifi;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XLabs.Enums;
using XLabs.Forms.Controls;
using XLabs.Forms.Mvvm;
using System.Collections.ObjectModel;
using MaxInsight.Mobile.ViewModels.Cases;
using MaxInsight.Mobile.ViewModels.Calendar;
using System.Threading.Tasks;
using MaxInsight.Mobile.Module.Dto.Notifi;
using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Services.NotifiService;
using XLabs.Ioc;
using System.Linq;
using MaxInsight.Mobile.ViewModels.ReviewPlans;
using MaxInsight.Mobile.ViewModels.Report;
using MaxInsight.Mobile.Pages.Calendar;
using Rg.Plugins.Popup.Services;
using MaxInsight.Mobile.Pages.ShopfrontPatrol;
using System.Text.RegularExpressions;

namespace MaxInsight.Mobile.Pages
{
    public partial class MainPage
    {
        CommonHelper _commonHelper;
        ICommonFun _commonFun;
        INotifiMngService _noticeMngService;
        List<NoticeListInfoDto> lstMessage;
        bool FirstYN = true;
        int index = 0;
        public MainPage()
        {
            InitializeComponent();
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _commonFun = Resolver.Resolve<ICommonFun>();
            _noticeMngService = Resolver.Resolve<INotifiMngService>();
            imageGallery.ItemsSource = new ObservableCollection<string>() { "banner1", "banner2", "banner3", "banner4" };

            InitMenuGrid();
            DisplayMessage();

            MessagingCenter.Subscribe<string>(this, "GoPreviewImageGesturePage", (param) =>
            {
                Regex regImg = new Regex(".+(.JPEG|.jpeg|.JPG|.jpg|.GIF|.gif|.BMP|.bmp|.PNG|.png)$");
                if (regImg.IsMatch(param))
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await PopupNavigation.PushAsync(new PreviewImageGesturePage(param), true);
                    });
                }
                else
                {
                    _commonFun.AlertLongText("请在电脑端阅览");
                    return;
                }

            });
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (!FirstYN)
            {
                await ResetData();
            }
        }

        #region InitMenu
        private void InitMenuGrid()
        {
            List<Action> action = new List<Action>(); //commit something
            string[] images;
            string[] menuName;
            if (CommonContext.Account.UserType == "D")// || CommonContext.Account.UserType == "S")
            {
                images = new string[] { "icon_plan", "icon_update", "icon_notifi" };//, "icon_share" };
                menuName = new string[] { "日历管理", "监控互动", "通知公告" };//, "案例分享" };
                action.Add(GoCalendar);
                action.Add(GoUpgrid);
                action.Add(GoNotifi);
                //action.Add(GoCaseShare);
            }
            else if (CommonContext.Account.UserType == "S")
            {
                images = new string[] { "icon_plan", "icon_check", "icon_update", "icon_notifi" };//, "icon_share" };
                menuName = new string[] { "日历管理", "详情查看", "监控互动", "通知公告" };//, "案例分享" };
                action.Add(GoCalendar);
                action.Add(GoCheck);
                action.Add(GoUpgrid);
                action.Add(GoNotifi);
                //action.Add(GoCaseShare);
                //action.Add(GoReport);
            }
            else if (CommonContext.Account.UserType == "Z")
            {
                images = new string[] { "icon_plan", "icon_check", "icon_update", "icon_notifi" };//, "icon_share" };
                menuName = new string[] { "日历管理", "巡视检核", "监控互动", "通知公告" };//, "案例分享" };
                action.Add(GoCalendar);
                action.Add(GoCheck);
                action.Add(GoUpgrid);
                action.Add(GoNotifi);
                //action.Add(GoCaseShare);
                //action.Add(GoReport);
            }
            else
            {
                images = new string[] { "icon_plan", "icon_check", "icon_update", "icon_notifi" };//, "icon_share" };
                menuName = new string[] { "日历管理", "计划任务", "监控互动", "通知公告" };//, "案例分享" };
                action.Add(GoCalendar);
                action.Add(GoPlan);
                action.Add(GoUpgrid);
                action.Add(GoNotifi);
                //action.Add(GoCaseShare);
                //action.Add(GoReport);
            }
            int menuCnt = menuName.Length;

            if (menuCnt % 2 == 0)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(120) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(120) });
            }
            else
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(120) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(120) }); 
            }



            if (Device.OS == TargetPlatform.iOS)
            {
                for (int i = 0; i < menuCnt; i++)
                {
                    var stack = new StackLayout()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Orientation = StackOrientation.Vertical,
                        BackgroundColor = Color.White,
                        Padding = new Thickness(0, 20, 0, 0)
                    };

                    var image = new ImageButton()
                    {
                        ImageHeightRequest = 50,
                        ImageWidthRequest = 50,
                        Source = ImageSource.FromFile(images[i]),
                        //Text = menuName[i],
                        Orientation = ImageOrientation.ImageCentered,
                        Command = new Command(action[i]),
                        TextColor = Color.Black,
                        BorderRadius = 0,
                        Margin = 2,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    };

                    stack.Children.Add(image);

                    var lable = new Label()
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        Text = menuName[i]
                    };

                    stack.Children.Add(lable);

                    stack.GestureRecognizers.Add(new TapGestureRecognizer()
                    {
                        Command = new Command(action[i]),
                        NumberOfTapsRequired = 1
                    });

                    //ChangeImageButtonColor(i, stack);
                    if (menuCnt % 2 == 0)
                    {
                        grid.Children.Add(stack, i % 2, i / 2);
                    }
                    else
                    {
                        grid.Children.Add(stack, i % 3, i / 3); 
                    }
                }
            }
            else
            {
                for (int i = 0; i < menuCnt; i++)
                {
                    var stack = new ImageButton()
                    {
                        ImageHeightRequest = 50,
                        ImageWidthRequest = 50,
                        Source = ImageSource.FromFile(images[i]),
                        Text = menuName[i],
                        Orientation = ImageOrientation.ImageCentered,
                        BackgroundColor = Color.White,
                        Command = new Command(action[i]),
                        TextColor = Color.Black,
                        BorderRadius = 0,
                    };

                    //ChangeImageButtonColor(i, stack);
                    if (menuCnt % 2 == 0)
                    {
                        grid.Children.Add(stack, i % 2, i / 2);
                    }
                    else
                    {
                        grid.Children.Add(stack, i % 3, i / 3); 
                    }
                }
            }
        }

        private async void GoReport()
        {
            try
            {
                //_commonFun.DownDB();
                if (!_commonHelper.IsNetWorkConnected())
                {
                    await DisplayAlert("温馨提示", "请在有网络时访问。", "确定");
                    return;
                }
                var page = ViewFactory.CreatePage<ReportSearchIndexViewModel, Page>() as Page;
                if (!Navigation.NavigationStack.Contains(page))
                {
                    await Navigation.PushAsync(page, true);
                }

                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("UserId", CommonContext.Account.UserId);
                param.Add("StartDate", DateTime.Now.AddMonths(-1).AddDays(1).ToString("yyyy-MM-dd"));
                param.Add("EndDate", DateTime.Now.ToString("yyyy-MM-dd"));
                param.Add("SourceTypeCode", "0");
                param.Add("SourceType", "全部");
                MessagingCenter.Send<Dictionary<string, string>>(param, "ReportSearchResult");
            }
            catch (Exception)
            {
            }
        }
        private async void GoCheck()
        {
            try
            {
                if (!_commonHelper.IsNetWorkConnected() && CommonContext.Account.UserType != "Z")
                {
                    await DisplayAlert("温馨提示", "请在有网络时访问。", "确定");
                    return;
                }
                var page = ViewFactory.CreatePage<ShopfrontMainPageViewModel, Page>() as Page;
                if (!Navigation.NavigationStack.Contains(page))
                {
                    await Navigation.PushAsync(page, true);
                }
            }
            catch (Exception)
            {
            }
        }
        private async void GoUpgrid()
        {
            try
            {
                if (!_commonHelper.IsNetWorkConnected())
                {
                    await DisplayAlert("温馨提示", "请在有网络时访问。", "确定");
                    return;
                }

                var page = ViewFactory.CreatePage<ImproveIndexViewModel, Page>() as Page;
                if (!Navigation.NavigationStack.Contains(page))
                {
                    await Navigation.PushAsync(page, true);
                }
                MessagingCenter.Send<MainPage>(this, "ChangeNavigation");
            }
            catch (Exception)
            {
            }
        }
        private async void GoNotifi()
        {
            try
            {
                if (!_commonHelper.IsNetWorkConnected())
                {
                    await DisplayAlert("温馨提示", "请在有网络时访问。", "确定");
                    return;
                }

                var page = ViewFactory.CreatePage<NotifiIndexViewModel, Page>() as Page;
                if (!Navigation.NavigationStack.Contains(page))
                {
                    await Navigation.PushAsync(page, true);
                }
            }
            catch (Exception)
            {
            }
        }
        private async void GoCaseShare()
        {
            try
            {
                if (!_commonHelper.IsNetWorkConnected())
                {
                    await DisplayAlert("温馨提示", "请在有网络时访问。", "确定");
                    return;
                }
                var page = ViewFactory.CreatePage<CaseIndexViewModel, Page>() as Page;
                if (!Navigation.NavigationStack.Contains(page))
                {
                    await Navigation.PushAsync(page, true);
                }
            }
            catch (Exception)
            {
            }
        }
        private async void GoCalendar()
        {
            try
            {
                if (!_commonHelper.IsNetWorkConnected())
                {
                    await DisplayAlert("温馨提示", "请在有网络时访问。", "确定");
                    return;
                }
                //await Navigation.PushAsync(ViewFactory.CreatePage<CalendarIndexViewModel, Page>() as Page, true);
                var page = ViewFactory.CreatePage<CalendarIndexViewModel, CalendarIndexPage>((vm, v) => vm.Init()) as Page;
                if (!Navigation.NavigationStack.Contains(page))
                {
                    await Navigation.PushAsync(page, true);
                }
            }
            catch (Exception)
            {
            }
        }
        private async void GoPlan()
        {
            try
            {
                if (!_commonHelper.IsNetWorkConnected())
                {
                    await DisplayAlert("温馨提示", "请在有网络时访问。", "确定");
                    return;
                }
                var page = ViewFactory.CreatePage<ReviewPlansIndexViewModel, Page>() as Page;
                if (!Navigation.NavigationStack.Contains(page))
                {
                    await Navigation.PushAsync(page, true);
                }
            }
            catch (Exception)
            {
            }
        }

        private static void ChangeImageButtonColor(int i, ImageButton stack)
        {
            if (i < 3)
            {
                if (i % 3 == 0)
                {
                    stack.BackgroundColor = Color.FromRgb(201, 199, 157);
                }
                else if (i % 3 == 1)
                {
                    stack.BackgroundColor = Color.FromRgb(78, 140, 168);
                }
                else if (i % 3 == 2)
                {
                    stack.BackgroundColor = Color.FromRgb(118, 175, 175);
                }
            }
            else if (i > 3 && i <= 5)
            {
                if (i % 3 == 0)
                {
                    stack.BackgroundColor = Color.FromRgb(78, 140, 168);
                }
                else if (i % 3 == 1)
                {
                    stack.BackgroundColor = Color.FromRgb(222, 206, 189);
                }
                else if (i % 3 == 2)
                {
                    stack.BackgroundColor = Color.FromRgb(189, 148, 142);
                }
            }
            else if (i > 5 && i <= 8)
            {
                if (i % 3 == 0)
                {
                    stack.BackgroundColor = Color.FromRgb(162, 115, 73);
                }
                else if (i % 3 == 1)
                {
                    stack.BackgroundColor = Color.FromRgb(236, 199, 110);
                }
                else if (i % 3 == 2)
                {
                    stack.BackgroundColor = Color.FromRgb(185, 121, 177);
                }
            }
            else if (i > 8 && i <= 11)
            {
                if (i % 3 == 0)
                {
                    stack.BackgroundColor = Color.FromRgb(206, 186, 196);
                }
                else if (i % 3 == 1)
                {
                    stack.BackgroundColor = Color.FromRgb(78, 140, 168);
                }
                else if (i % 3 == 2)
                {
                    stack.BackgroundColor = Color.FromRgb(239, 236, 205);
                }
            }
        }
        #endregion

        #region Message

        private async void DisplayMessage()
        {
            await ResetData();
            FirstYN = false;

            //set timer
            Device.StartTimer(TimeSpan.FromSeconds(3), () =>
            {
                if (lstMessage == null || lstMessage.Count <= 0)
                {
                    lblMessage.Text = "没有最新通知！";
                    return true;
                }
                else
                {
                    string msg = lstMessage[index].Title;

                    if (msg.Length > 15 && !Char.IsPunctuation(msg.PadRight(1).ToCharArray()[0]))
                    {
                        msg = msg.Substring(0, 15) + "...";
                    }
                    lblMessage.Text = msg;
                    index++;
                    if (lstMessage.Count <= index)
                    {
                        index = 0;
                    }
                    return true;
                }
            });
        }

        private async Task ResetData()
        {
            index = 0;
            lstMessage = await GetMessageData();
        }

        private async Task<List<NoticeListInfoDto>> GetMessageData()
        {
            try
            {
                var result = await _noticeMngService.SearchMadeNotifiList("19000101",
                                                   "21001231",
                                                   "",
                                                   "U",
                                                   "",
                                                   "",
                                                   "",
                                                   CommonContext.Account.UserId);
                if (result.ResultCode == Module.ResultType.Success)
                {

                    List<NoticeListInfoDto> noticeList = CommonHelper.DecodeString<List<NoticeListInfoDto>>(result.Body);
                    int i = 1;
                    noticeList.Select(c => { c.SeqNo = i++; return c; }).ToList();
                    return noticeList;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }
        #endregion
    }
}
