using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using XLabs.Forms.Controls;


namespace MaxInsight.Mobile.Pages.Calendar
{
    public partial class CalendarIndexPage : ContentPage
    {

        public CalendarIndexPage()
        {
            InitializeComponent();
            Title = "日历管理";
            //calendar.DisplayedMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            calendar.MinDate = new DateTime(2015, 11, 1, 0, 0, 0);
            calendar.MaxDate = DateTime.Now.AddYears(10);
            calendar.TodayBackgroundStyle = ECalendarView.BackgroundStyle.CircleOutline;
            calendar.SelectionBackgroundStyle = ECalendarView.BackgroundStyle.Fill;
            calendar.ShouldHighlightDaysOfWeekLabels = false;
            //calendar.ShowNavigationArrows = true;
            //calendar.NavigationArrowsColor = Color.FromRgb(2,160,208);

            calendar.HighlightedDateBackgroundColor = Color.FromRgb(0, 183, 238);
            calendar.TodayDateForegroundColor = Color.FromRgb(255, 255, 255);
            calendar.TodayDateBackgroundColor = Color.FromRgb(253, 97, 0);
            calendar.DateBackgroundColor = Color.FromRgb(255, 255, 255);
            calendar.DateSeparatorColor = Color.FromRgb(204, 204, 204);
            calendar.SelectedDateBackgroundColor = Color.FromRgb(109, 178, 234);
            //日历字体
            calendar.DateLabelFont = Font.SystemFontOfSize(10, FontAttributes.None);

            calendar.DateSelected += Calendar_DateSelected;
            calendar.MonthChanged += Calendar_MonthChanged;

            relativeLayout.HeightRequest = App.ScreenHeight * 0.5;

            listViewRelativeLayout.HeightRequest = App.ScreenHeight * 0.4;

            if (Device.OS == TargetPlatform.iOS)
            {
                Calendar_MonthChanged(null, DateTime.Now);
            }
        }

        private void Calendar_DateSelected(object sender, DateTime e)
        {
            MessagingCenter.Send<string>(e.ToString("yyyy-MM-dd"), "SearchDataByDate");
        }

        private void Calendar_MonthChanged(object sender, DateTime e)
        {
            MessagingCenter.Send<string>(e.ToString("yyyy-MM-dd"), "SearchCalendarData");
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
        private void OnSetToday(object sender, EventArgs e)
        {
            calendar.DisplayedMonth = DateTime.Now;
            MessagingCenter.Send<string>(DateTime.Now.ToString("yyyy-MM-dd"), "SearchCalendarData");
        }

        //public CalendarIndexPage()
        //{
        //    InitializeComponent();
        //    Title = "日历管理";



        //    _relativeLayout = new RelativeLayout()
        //    {
        //        HorizontalOptions = LayoutOptions.FillAndExpand,
        //        VerticalOptions = LayoutOptions.FillAndExpand
        //    };
        //    Content = _relativeLayout;


        //    _calendarView = new CalendarView()
        //    {
        //        //BackgroundColor = Color.Blue
        //        MinDate = new DateTime(2010, 1, 1, 15, 36, 05),
        //        MaxDate = new DateTime(2020, 12, 31, 15, 36, 05),
        //        HighlightedDateBackgroundColor = Color.FromRgb(227, 227, 227),
        //        ShouldHighlightDaysOfWeekLabels = false,
        //        SelectionBackgroundStyle = ECalendarView.BackgroundStyle.CircleFill,
        //        TodayBackgroundStyle = ECalendarView.BackgroundStyle.CircleOutline,
        //        HighlightedDaysOfWeek = new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Sunday },
        //        //ShowNavigationArrows = true,
        //        MonthTitleFont = Font.OfSize("Open 24 Display St", NamedSize.Medium),
        //        HighlightedDays = new List<DateTime>()

        //    };

        //    _relativeLayout.Children.Add(_calendarView,
        //        Constraint.Constant(0),
        //        Constraint.Constant(0),
        //        Constraint.RelativeToParent(p => p.Width),
        //        Constraint.RelativeToParent(p => p.Height * 2 / 3));

        //    _stacker = new StackLayout()
        //    {
        //        HorizontalOptions = LayoutOptions.FillAndExpand,
        //        VerticalOptions = LayoutOptions.StartAndExpand
        //    };
        //    _relativeLayout.Children.Add(_stacker,
        //        Constraint.Constant(0),
        //        Constraint.RelativeToParent(p => p.Height * 2 / 3),
        //        Constraint.RelativeToParent(p => p.Width),
        //        Constraint.RelativeToParent(p => p.Height * 1 / 3)
        //    );
        //    _calendarView.DateSelected += (object sender, DateTime e) =>
        //    {
        //        _stacker.Children.Add(new Label()
        //        {
        //            Text = "Date Was Selected" + e.ToString("d"),
        //            VerticalOptions = LayoutOptions.Start,
        //            HorizontalOptions = LayoutOptions.CenterAndExpand,
        //        });
        //    };
        //}

    }
}
