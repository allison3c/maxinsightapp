using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto.Calender;
using MaxInsight.Mobile.Services.CalendarService;
using System;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.Calendar
{
    public class CalendarRegViewModel : ViewModel
    {
        ICalendarService _calendarService;
        ICommonFun _commonFun;
        #region Constructor
        public CalendarRegViewModel()
        {
            _calendarService = Resolver.Resolve<ICalendarService>();
            _commonFun = Resolver.Resolve<ICommonFun>();
        }
        #endregion

        #region properties

        private string _startDate;
        public string StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                SetProperty(ref _startDate, value);
            }
        }
        private TimeSpan _startTime;
        public TimeSpan StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                SetProperty(ref _startTime, value);
            }
        }

        private string _endDate;
        public string EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                SetProperty(ref _endDate, value);
            }
        }
        private TimeSpan _endTime;
        public TimeSpan EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                SetProperty(ref _endTime, value);
            }
        }

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                SetProperty(ref _title, value);
            }
        }
        private string _content;
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                SetProperty(ref _content, value);
            }
        }

        private string _id;
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                SetProperty(ref _id, value);
            }
        }

        private bool _visibleYN;
        public bool VisibleYN
        {
            get
            {
                return _visibleYN;
            }
            set
            {
                SetProperty(ref _visibleYN, value);
            }
        }
        public string _ctype;
        public string CType
        {
            get
            {
                return _ctype;
            }
            set
            {
                SetProperty(ref _ctype, value);
            }
        }

        #endregion

        #region Command
        public Command CalendarSaveCommand
        {
            get
            {
                return new Command(() =>
               {
                   try
                   {
                       SaveCalenderEvent();
                   }
                   catch (Exception)
                   {
                   }
               });
            }
        }
        public Command CalendarDeleteCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        DeleteCalendarEvent();
                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }

        #endregion

        #region methods
        public void Init(CalenderListAllDto dto, string selectedDate)
        {
            try
            {
                if (dto != null)
                {
                    Id = dto.Id;
                    Title = dto.Title;
                    Content = dto.Content;
                    StartDate = dto.SDate;
                    StartTime = dto.SDate != null ? Convert.ToDateTime(dto.SDate).TimeOfDay : DateTime.Now.TimeOfDay;
                    EndDate = dto.EDate;
                    EndTime = dto.EDate != null ? Convert.ToDateTime(dto.EDate).TimeOfDay : DateTime.Now.TimeOfDay;
                    if (dto.Type == "T")
                    {
                        VisibleYN = true;
                        CType = "T";
                    }
                    else
                    {
                        VisibleYN = false;
                        CType = "P";
                    }
                }
                else
                {
                    Id = "0";
                    Title = "";
                    Content = "";
                    StartDate = selectedDate;
                    EndDate = selectedDate;
                    StartTime = DateTime.Now.TimeOfDay;
                    EndTime = DateTime.Now.TimeOfDay;
                    VisibleYN = true;
                    CType = "P";
                }
            }
            catch (Exception)
            {
            }
        }


        private async void SaveCalenderEvent()
        {
            try
            {
                string _sdt = Convert.ToDateTime(StartDate).Date.ToString("yyyy-MM-dd") + " " + TwoString(StartTime.Hours) + ":" + TwoString(StartTime.Minutes);
                DateTime st;
                string _set = Convert.ToDateTime(EndDate).Date.ToString("yyyy-MM-dd") + " " + TwoString(EndTime.Hours) + ":" + TwoString(EndTime.Minutes);
                DateTime et;
                if (Title == "")
                {
                    _commonFun.AlertLongText("请填写标题");
                    return;
                }
                else if (DateTime.TryParse(_sdt, out st) && DateTime.TryParse(_set, out et))
                {
                    if (st > et)
                    {
                        _commonFun.AlertLongText("开始时间不能大于结束时间");
                        return;
                    }
                }
                else if (string.IsNullOrEmpty(Content))
                {
                    _commonFun.AlertLongText("请填内容");
                    return;
                }

                SaveCalenderMngParams dto = new SaveCalenderMngParams()
                {
                    Id = Id,
                    Title = Title,
                    Content = Content,
                    Type = "T",
                    SDate = _sdt,
                    EDate = _set,
                    UserID = CommonContext.Account.UserId
                };

                _commonFun.ShowLoading("保存中...");
                var result = await _calendarService.CreateCalenderPlans(dto);
                if (result != null)
                {
                    if (result.ResultCode == ResultType.Success)
                    {
                        //_commonFun.AlertLongText("保存成功");                        
                        await Navigation.PopAsync();
                        MessagingCenter.Send<string>(Convert.ToDateTime(StartDate).ToString("yyyy-MM-dd"), "SearchCalendarData");
                    }
                    else
                    {
                        _commonFun.AlertLongText("保存失败" + result.Msg);
                    }
                }
                else
                {
                    _commonFun.AlertLongText("保存时服务出现错误,,请重试");
                }
            }
            catch (OperationCanceledException)
            {
                _commonFun.HideLoading();
                _commonFun.AlertLongText("请求超时,请重试");
            }
            catch (Exception)
            {
                _commonFun.HideLoading();
            }
            finally
            {
                _commonFun.HideLoading();
            }
        }

        private async void DeleteCalendarEvent()
        {
            try
            {
                if (!await _commonFun.Confirm("确定删除吗？"))
                {
                    return;
                }
                var result = await _calendarService.DeleteCalenderPlans(Id.ToString());
                if (result != null)
                {
                    if (result.ResultCode == ResultType.Success)
                    {
                        //_commonFun.AlertLongText("删除成功");                        
                        await Navigation.PopAsync();
                        MessagingCenter.Send<string>(Convert.ToDateTime(StartDate).ToString("yyyy-MM-dd"), "SearchCalendarData");
                    }
                    else
                    {
                        _commonFun.AlertLongText("保存失败,请重试");
                    }
                }
                else
                {
                    _commonFun.AlertLongText("保存时服务出现错误,,请重试");
                }
            }
            catch (OperationCanceledException)
            {
                _commonFun.HideLoading();
                _commonFun.AlertLongText("请求超时,请重试");
            }
            catch (Exception)
            {
                _commonFun.HideLoading();
            }
            finally
            {
                _commonFun.HideLoading();
            }
        }

        private string TwoString(int t)
        {
            if (t < 10)
            {
                return "0" + t.ToString();
            }
            else
            {
                return t.ToString();
            }

        }
        #endregion
    }
}
