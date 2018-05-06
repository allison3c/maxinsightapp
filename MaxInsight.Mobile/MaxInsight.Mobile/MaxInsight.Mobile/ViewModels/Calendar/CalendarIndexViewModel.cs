using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module.Dto.Calender;
using MaxInsight.Mobile.Services.CalendarService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.Calendar
{
    public class CalendarIndexViewModel : ViewModel
    {
        CommonHelper _commonHelper;
        ICalendarService _calendarService;
        ICommonFun _commonFun;
        public List<DateTime> _eventDates;
        public List<DateTime> EventDates
        {
            get
            {
                return _eventDates;
            }
            set
            {
                SetProperty(ref _eventDates, value);
            }
        }
        double _lstRowHeight = 52;
        double _lstHeight;
        public double LstHeight
        {
            get { return _lstHeight; }
            set { SetProperty(ref _lstHeight, value); }
        }
        private string _selectedDate;
        public string SelectedDate
        {
            get { return _selectedDate; }
            set { SetProperty(ref _selectedDate, value); }
        }
        public CalendarIndexViewModel()
        {
            _commonFun = Resolver.Resolve<ICommonFun>();
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _calendarService = Resolver.Resolve<ICalendarService>();
            EventDates = new List<DateTime>();

            CalendarBydateList = new ObservableCollection<CalenderListAllDto>();

            MessagingCenter.Subscribe<string>(
            this,
            "SearchCalendarData",
            (date) =>
            {
                SearchCalendarData(date);
            });

            MessagingCenter.Subscribe<string>(
            this,
            "SearchDataByDate",
            (date) =>
            {
                SearchDataByDate(date);
            });

        }
        #region Property

        private List<CalenderListAllDto> _calenderList;
        public List<CalenderListAllDto> CalenderList
        {
            get
            {
                return _calenderList;
            }
            set
            {
                SetProperty(ref _calenderList, value);
            }
        }

        public ObservableCollection<CalenderListAllDto> _calendarBydateList;
        public ObservableCollection<CalenderListAllDto> CalendarBydateList
        {
            get
            {
                return _calendarBydateList;
            }
            set
            {
                SetProperty(ref _calendarBydateList, value);
            }
        }
        private CalenderListAllDto _calendarItem;
        public CalenderListAllDto CalendarItem
        {
            get
            {
                return _calendarItem;
            }
            set
            {
                SetProperty(ref _calendarItem, value);
            }
        }
        private DateTime _displayedMonth;
        public DateTime DisplayedMonth
        {
            get
            {
                return _displayedMonth;
            }
            set
            {
                SetProperty(ref _displayedMonth, value);
            }
        }
        #endregion

        #region Command
        public Command CalendarRegCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        //await Navigation.PushAsync<CalendarRegViewModel>();
                        if (SelectedDate == null || SelectedDate == "")
                        {
                            SelectedDate = DateTime.Now.ToString("yyyy-MM-dd");
                        }
                        Navigation.PushAsync<CalendarRegViewModel>((vm, v) => vm.Init(null, SelectedDate), true);
                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }
        public Command ItemTappedCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        Navigation.PushAsync<CalendarRegViewModel>((vm, v) => vm.Init(CalendarItem, SelectedDate), true);
                    }
                    catch (Exception ex)
                    {

                    }

                });
            }
        }
        #endregion

        #region Method
        private async void SearchCalendarData(string date)
        {
            try
            {
                if (date == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    SelectedDate = date;
                }
                var result = await _calendarService.GetCalenderListAll(CommonContext.Account.UserId, date);
                if (null != result && result.ResultCode == Module.ResultType.Success)
                {
                    var list = JsonConvert.DeserializeObject<List<CalenderListAllDto>>(result.Body);
                    List<string> strLst = new List<string>();
                    if (list != null && list.Count > 0)
                    {
                        CalenderList = list;
                        strLst = (
                                    from c in list
                                    select c.EachDate)
                                    .Distinct().ToList();
                        List<DateTime> listD = new List<DateTime>();
                        foreach (var item in strLst)
                        {
                            listD.Add(DateTime.Parse(item));
                        }
                        EventDates = listD;
                    }
                    else
                    {
                        CalenderList = new List<CalenderListAllDto>();
                        EventDates = new List<DateTime>();
                    }
                    CalendarBydateList.Clear();
                    LstHeight = CalendarBydateList.Count * _lstRowHeight + CalendarBydateList.Count * 10;

                }
            }

            catch (OperationCanceledException)
            {
            }
            catch (Exception)
            {
            }
            finally
            {
            }
        }

        private void SearchDataByDate(string date)
        {
            try
            {
                SelectedDate = date;
                CalendarBydateList.Clear();
                foreach (var item in CalenderList)
                {
                    if (item.EachDate == date)
                    {
                        item.Period = item.SDate + "~" + item.EDate;
                        if (item.Type == "S")
                        {
                            item.BgColor = Color.FromRgb(249, 217, 0);
                            item.ForeColor = Color.Black;
                        }
                        else
                        {
                            item.BgColor = Color.FromRgb(109, 178, 234);
                            item.ForeColor = Color.White;
                        }
                        CalendarBydateList.Add(item);

                    }

                }
                LstHeight = CalendarBydateList.Count * _lstRowHeight + CalendarBydateList.Count * 10;

            }
            catch (Exception)
            {
            }
        }
        public void Init()
        {
            DateTime now = DateTime.Now;
            DisplayedMonth = now;
            SelectedDate = now.ToString("yyyy-MM-dd");
            if (Device.OS == TargetPlatform.iOS)
            {
                SearchCalendarData(now.ToString("yyyy-MM-dd"));
            }
        }
        #endregion
    }
}
