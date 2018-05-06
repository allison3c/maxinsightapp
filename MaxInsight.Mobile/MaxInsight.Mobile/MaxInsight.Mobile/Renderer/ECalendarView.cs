using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MaxInsight.Mobile
{
    public class ECalendarView : View
    {
        /// <summary>
        /// Enum BackgroundStyle
        /// </summary>
        public enum BackgroundStyle
        {
            /// <summary>
            /// The fill
            /// </summary>
            Fill,
            /// <summary>
            /// The circle fill
            /// </summary>
            CircleFill,
            /// <summary>
            /// The circle outline
            /// </summary>
            CircleOutline
        }

        /**
		 * SelectedDate property
		 */
        /// <summary>
        /// The minimum date property
        /// </summary>
        public static readonly BindableProperty MinDateProperty =
            BindableProperty.Create(
                "MinDate",
                typeof(DateTime),
                typeof(ECalendarView),
                FirstDayOfMonth(DateTime.Today),
                BindingMode.OneWay,
                null, null, null, null);


        /// <summary>
        /// Gets or sets the minimum date.
        /// </summary>
        /// <value>The minimum date.</value>
        public DateTime MinDate
        {
            get
            {
                return (DateTime)base.GetValue(ECalendarView.MinDateProperty);
            }
            set
            {

                base.SetValue(ECalendarView.MinDateProperty, value);
            }
        }

        /// <summary>
        /// The maximum date property
        /// </summary>
        public static readonly BindableProperty MaxDateProperty =
            BindableProperty.Create(
                "MaxDate",
                typeof(DateTime),
                typeof(ECalendarView),
                LastDayOfMonth(DateTime.Today),
                BindingMode.OneWay,
                null, null, null, null);


        /// <summary>
        /// Gets or sets the maximum date.
        /// </summary>
        /// <value>The maximum date.</value>
        public DateTime MaxDate
        {
            get
            {
                return (DateTime)base.GetValue(ECalendarView.MaxDateProperty);
            }
            set
            {
                base.SetValue(ECalendarView.MaxDateProperty, value);
            }
        }
        //Helper method
        /// <summary>
        /// Firsts the day of month.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>DateTime.</returns>
        public static DateTime FirstDayOfMonth(DateTime date)
        {
            return date.AddDays(1 - date.Day);
        }
        //Helper method
        /// <summary>
        /// Lasts the day of month.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>DateTime.</returns>
        public static DateTime LastDayOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }



        /**
		 * SelectedDate property
		 */
        /// <summary>
        /// The selected date property
        /// </summary>
        public static readonly BindableProperty SelectedDateProperty =
            BindableProperty.Create(
                "SelectedDate",
                typeof(DateTime?),
                typeof(ECalendarView),
                null,
                BindingMode.TwoWay,
                null, null, null, null);


        /// <summary>
        /// Gets or sets the selected date.
        /// </summary>
        /// <value>The selected date.</value>
        public DateTime? SelectedDate
        {
            get
            {
                return (DateTime?)base.GetValue(ECalendarView.SelectedDateProperty);
            }
            set
            {
                base.SetValue(ECalendarView.SelectedDateProperty, value);
            }
        }

        /**
		 * Displayed date property
		 */
        /// <summary>
        /// The displayed month property
        /// </summary>
        public static readonly BindableProperty DisplayedMonthProperty =
            BindableProperty.Create(
                "DisplayedMonth",
                typeof(DateTime),
                typeof(ECalendarView),
                DateTime.Now,
                BindingMode.TwoWay,
                null, null, null, null);


        /// <summary>
        /// Gets or sets the displayed month.
        /// </summary>
        /// <value>The displayed month.</value>
        public DateTime DisplayedMonth
        {
            get
            {
                return (DateTime)base.GetValue(ECalendarView.DisplayedMonthProperty);
            }
            set
            {
                base.SetValue(ECalendarView.DisplayedMonthProperty, value);
            }
        }


        /**
		 * DateLabelFont property
		 */
        /// <summary>
        /// The date label font property
        /// </summary>
        public static readonly BindableProperty DateLabelFontProperty = BindableProperty.Create("DateLabelFont", typeof(Font), typeof(ECalendarView), Font.Default, BindingMode.OneWay, null, null, null, null);

        /**
		 * Font used by the calendar dates and day labels
		 */
        /// <summary>
        /// Gets or sets the date label font.
        /// </summary>
        /// <value>The date label font.</value>
        public Font DateLabelFont
        {
            get
            {
                return (Font)base.GetValue(ECalendarView.DateLabelFontProperty);
            }
            set
            {
                base.SetValue(ECalendarView.DateLabelFontProperty, value);
            }
        }


        /**
		 * Font property
		 */
        /// <summary>
        /// The month title font property
        /// </summary>
        public static readonly BindableProperty MonthTitleFontProperty = BindableProperty.Create("MonthTitleFont", typeof(Font), typeof(ECalendarView), Font.Default, BindingMode.OneWay, null, null, null, null);

        /**
		 * Font used by the month title
		 */
        /// <summary>
        /// Gets or sets the month title font.
        /// </summary>
        /// <value>The month title font.</value>
        public Font MonthTitleFont
        {
            get
            {
                return (Font)base.GetValue(ECalendarView.MonthTitleFontProperty);
            }
            set
            {
                base.SetValue(ECalendarView.MonthTitleFontProperty, value);
            }
        }




        /**
		 * TextColorProperty property
		 */
        /// <summary>
        /// The text color property
        /// </summary>
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Color), typeof(ECalendarView), Color.Default, BindingMode.OneWay, null, null, null, null);

        /**
		 * Overall text color property. Default color is platform specific.
		 */
        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        /// <value>The color of the text.</value>
        public Color TextColor
        {
            get
            {
                return (Color)base.GetValue(ECalendarView.TextColorProperty);
            }
            set
            {
                base.SetValue(ECalendarView.TextColorProperty, value);
            }
        }

        /**
		 * TodayDateForegroundColorProperty property
		 */
        /// <summary>
        /// The today date foreground color property
        /// </summary>
        public static readonly BindableProperty TodayDateForegroundColorProperty = BindableProperty.Create("TodayDateForegroundColor", typeof(Color), typeof(ECalendarView), Color.Default, BindingMode.OneWay, null, null, null, null);

        /**
		 * Foreground color of today date. Default color is platform specific.
		 */
        /// <summary>
        /// Gets or sets the color of the today date foreground.
        /// </summary>
        /// <value>The color of the today date foreground.</value>
        public Color TodayDateForegroundColor
        {
            get
            {
                return (Color)base.GetValue(ECalendarView.TodayDateForegroundColorProperty);
            }
            set
            {
                base.SetValue(ECalendarView.TodayDateForegroundColorProperty, value);
            }
        }

        /**
		 * TodayDateBackgroundColorProperty property
		 */
        /// <summary>
        /// The today date background color property
        /// </summary>
        public static readonly BindableProperty TodayDateBackgroundColorProperty = BindableProperty.Create("TodayDateBackgroundColor", typeof(Color), typeof(ECalendarView), Color.Default, BindingMode.OneWay, null, null, null, null);

        /**
		 * Background color of today date. Default color is platform specific.
		 */
        /// <summary>
        /// Gets or sets the color of the today date background.
        /// </summary>
        /// <value>The color of the today date background.</value>
        public Color TodayDateBackgroundColor
        {
            get
            {
                return (Color)base.GetValue(ECalendarView.TodayDateBackgroundColorProperty);
            }
            set
            {
                base.SetValue(ECalendarView.TodayDateBackgroundColorProperty, value);
            }
        }

        /**
		 * DateForegroundColorProperty property
		 */
        /// <summary>
        /// The date foreground color property
        /// </summary>
        public static readonly BindableProperty DateForegroundColorProperty = BindableProperty.Create("DateForegroundColor", typeof(Color), typeof(ECalendarView), Color.Default, BindingMode.OneWay, null, null, null, null);

        /**
		 * Foreground color of date in the calendar. Default color is platform specific.
		 */
        /// <summary>
        /// Gets or sets the color of the date foreground.
        /// </summary>
        /// <value>The color of the date foreground.</value>
        public Color DateForegroundColor
        {
            get
            {
                return (Color)base.GetValue(ECalendarView.DateForegroundColorProperty);
            }
            set
            {
                base.SetValue(ECalendarView.DateForegroundColorProperty, value);
            }
        }

        /**
		 * DateBackgroundColorProperty property
		 */
        /// <summary>
        /// The date background color property
        /// </summary>
        public static readonly BindableProperty DateBackgroundColorProperty = BindableProperty.Create("DateBackgroundColor", typeof(Color), typeof(ECalendarView), Color.Default, BindingMode.OneWay, null, null, null, null);

        /**
		 * Background color of date in the calendar. Default color is platform specific.
		 */
        /// <summary>
        /// Gets or sets the color of the date background.
        /// </summary>
        /// <value>The color of the date background.</value>
        public Color DateBackgroundColor
        {
            get
            {
                return (Color)base.GetValue(ECalendarView.DateBackgroundColorProperty);
            }
            set
            {
                base.SetValue(ECalendarView.DateBackgroundColorProperty, value);
            }
        }


        /**
		 * InactiveDateForegroundColorProperty property
		 */
        /// <summary>
        /// The inactive date foreground color property
        /// </summary>
        public static readonly BindableProperty InactiveDateForegroundColorProperty = BindableProperty.Create("InactiveDateForegroundColor", typeof(Color), typeof(ECalendarView), Color.Default, BindingMode.OneWay, null, null, null, null);

        /**
		 * Foreground color of date in the calendar which is outside of the current month. Default color is platform specific.
		 */
        /// <summary>
        /// Gets or sets the color of the inactive date foreground.
        /// </summary>
        /// <value>The color of the inactive date foreground.</value>
        public Color InactiveDateForegroundColor
        {
            get
            {
                return (Color)base.GetValue(ECalendarView.InactiveDateForegroundColorProperty);
            }
            set
            {
                base.SetValue(ECalendarView.InactiveDateForegroundColorProperty, value);
            }
        }

        /**
		 * InactiveDateBackgroundColorProperty property
		 */
        /// <summary>
        /// The inactive date background color property
        /// </summary>
        public static readonly BindableProperty InactiveDateBackgroundColorProperty = BindableProperty.Create("InactiveDateBackgroundColor", typeof(Color), typeof(ECalendarView), Color.Default, BindingMode.OneWay, null, null, null, null);

        /**
		 * Background color of date in the calendar  which is outside of the current month. Default color is platform specific.
		 */
        /// <summary>
        /// Gets or sets the color of the inactive date background.
        /// </summary>
        /// <value>The color of the inactive date background.</value>
        public Color InactiveDateBackgroundColor
        {
            get
            {
                return (Color)base.GetValue(ECalendarView.InactiveDateBackgroundColorProperty);
            }
            set
            {
                base.SetValue(ECalendarView.InactiveDateBackgroundColorProperty, value);
            }
        }


        /**
		 * HighlightedDateForegroundColorProperty property
		 */
        /// <summary>
        /// The highlighted date foreground color property
        /// </summary>
        public static readonly BindableProperty HighlightedDateForegroundColorProperty = BindableProperty.Create("HighlightedDateForegroundColor", typeof(Color), typeof(ECalendarView), Color.Default, BindingMode.OneWay, null, null, null, null);

        /**
		 * Foreground color of highlighted date in the calendar. Default color is platform specific.
		 */
        /// <summary>
        /// Gets or sets the color of the highlighted date foreground.
        /// </summary>
        /// <value>The color of the highlighted date foreground.</value>
        public Color HighlightedDateForegroundColor
        {
            get
            {
                return (Color)base.GetValue(ECalendarView.HighlightedDateForegroundColorProperty);
            }
            set
            {
                base.SetValue(ECalendarView.HighlightedDateForegroundColorProperty, value);
            }
        }
        /**
		 * HighlightedDateBackgroundColor property
		 */
        /// <summary>
        /// The highlighted date background color property
        /// </summary>
        public static readonly BindableProperty HighlightedDateBackgroundColorProperty = BindableProperty.Create("HighlightedDateBackgroundColor", typeof(Color), typeof(ECalendarView), Color.Default, BindingMode.OneWay, null, null, null, null);

        /**
		 * Background color of selected date in the calendar. Default color is platform specific.
		 */
        /// <summary>
        /// Gets or sets the color of the highlighted date background.
        /// </summary>
        /// <value>The color of the highlighted date background.</value>
        public Color HighlightedDateBackgroundColor
        {
            get
            {
                return (Color)base.GetValue(ECalendarView.HighlightedDateBackgroundColorProperty);
            }
            set
            {
                base.SetValue(ECalendarView.HighlightedDateBackgroundColorProperty, value);
            }
        }


        /**
		 * TodayBackgroundStyle property
		 */
        /// <summary>
        /// The today background style property
        /// </summary>
        public static readonly BindableProperty TodayBackgroundStyleProperty = BindableProperty.Create("TodayBackgroundStyle", typeof(BackgroundStyle), typeof(ECalendarView), BackgroundStyle.Fill, BindingMode.OneWay, null, null, null, null);

        /**
		 * Background style for today cell. It is only respected on iOS for now.
		 */
        /// <summary>
        /// Gets or sets the today background style.
        /// </summary>
        /// <value>The today background style.</value>
        public BackgroundStyle TodayBackgroundStyle
        {
            get
            {
                return (BackgroundStyle)base.GetValue(ECalendarView.TodayBackgroundStyleProperty);
            }
            set
            {
                base.SetValue(ECalendarView.TodayBackgroundStyleProperty, value);
            }
        }


        /**
		 * SelectionBackgroundStyle property
		 */
        /// <summary>
        /// The selection background style property
        /// </summary>
        public static readonly BindableProperty SelectionBackgroundStyleProperty = BindableProperty.Create("SelectionBackgroundStyle", typeof(BackgroundStyle), typeof(ECalendarView), BackgroundStyle.Fill, BindingMode.OneWay, null, null, null, null);

        /**
		 * Background style for selecting the cells. It is only respected on iOS for now.
		 */
        /// <summary>
        /// Gets or sets the selection background style.
        /// </summary>
        /// <value>The selection background style.</value>
        public BackgroundStyle SelectionBackgroundStyle
        {
            get
            {
                return (BackgroundStyle)base.GetValue(ECalendarView.SelectionBackgroundStyleProperty);
            }
            set
            {
                base.SetValue(ECalendarView.SelectionBackgroundStyleProperty, value);
            }
        }


        /**
		 * SelectedDateForegroundColorProperty property
		 */
        /// <summary>
        /// The selected date foreground color property
        /// </summary>
        public static readonly BindableProperty SelectedDateForegroundColorProperty = BindableProperty.Create("SelectedDateForegroundColor", typeof(Color), typeof(ECalendarView), Color.Default, BindingMode.OneWay, null, null, null, null);

        /**
		 * Foreground color of selected date in the calendar. Default color is platform specific.
		 */
        /// <summary>
        /// Gets or sets the color of the selected date foreground.
        /// </summary>
        /// <value>The color of the selected date foreground.</value>
        public Color SelectedDateForegroundColor
        {
            get
            {
                return (Color)base.GetValue(ECalendarView.SelectedDateForegroundColorProperty);
            }
            set
            {
                base.SetValue(ECalendarView.SelectedDateForegroundColorProperty, value);
            }
        }

        /**
		 * DateBackgroundColorProperty property
		 */
        /// <summary>
        /// The selected date background color property
        /// </summary>
        public static readonly BindableProperty SelectedDateBackgroundColorProperty = BindableProperty.Create("SelectedDateBackgroundColor", typeof(Color), typeof(ECalendarView), Color.Default, BindingMode.OneWay, null, null, null, null);

        /**
		 * Background color of selected date in the calendar. Default color is platform specific.
		 */
        /// <summary>
        /// Gets or sets the color of the selected date background.
        /// </summary>
        /// <value>The color of the selected date background.</value>
        public Color SelectedDateBackgroundColor
        {
            get
            {
                return (Color)base.GetValue(ECalendarView.SelectedDateBackgroundColorProperty);
            }
            set
            {
                base.SetValue(ECalendarView.SelectedDateBackgroundColorProperty, value);
            }
        }



        /**
		 * DayOfWeekLabelForegroundColorProperty property
		 */
        /// <summary>
        /// The day of week label foreground color property
        /// </summary>
        public static readonly BindableProperty DayOfWeekLabelForegroundColorProperty = BindableProperty.Create("DayOfWeekLabelForegroundColor", typeof(Color), typeof(ECalendarView), Color.Default, BindingMode.OneWay, null, null, null, null);

        /**
		 * Foreground color of week day labels in the month header. Default color is platform specific.
		 */
        /// <summary>
        /// Gets or sets the color of the day of week label foreground.
        /// </summary>
        /// <value>The color of the day of week label foreground.</value>
        public Color DayOfWeekLabelForegroundColor
        {
            get
            {
                return (Color)base.GetValue(ECalendarView.DayOfWeekLabelForegroundColorProperty);
            }
            set
            {
                base.SetValue(ECalendarView.DayOfWeekLabelForegroundColorProperty, value);
            }
        }
        /**
		 * DayOfWeekLabelForegroundColorProperty property
		 */
        /// <summary>
        /// The day of week label background color property
        /// </summary>
        public static readonly BindableProperty DayOfWeekLabelBackgroundColorProperty = BindableProperty.Create("DayOfWeekLabelBackgroundColor", typeof(Color), typeof(ECalendarView), Color.Default, BindingMode.OneWay, null, null, null, null);

        /**
		 * Background color of week day labels in the month header. Default color is platform specific.
		 */
        /// <summary>
        /// Gets or sets the color of the day of week label background.
        /// </summary>
        /// <value>The color of the day of week label background.</value>
        public Color DayOfWeekLabelBackgroundColor
        {
            get
            {
                return (Color)base.GetValue(ECalendarView.DayOfWeekLabelBackgroundColorProperty);
            }
            set
            {
                base.SetValue(ECalendarView.DayOfWeekLabelBackgroundColorProperty, value);
            }
        }



        /**
		 * DayOfWeekLabelForegroundColorProperty property
		 */
        /// <summary>
        /// The month title foreground color property
        /// </summary>
        public static readonly BindableProperty MonthTitleForegroundColorProperty = BindableProperty.Create("MonthTitleForegroundColor", typeof(Color), typeof(ECalendarView), Color.Default, BindingMode.OneWay, null, null, null, null);

        /**
		 * Foreground color of week day labels in the month header. Default color is platform specific.
		 */
        /// <summary>
        /// Gets or sets the color of the month title foreground.
        /// </summary>
        /// <value>The color of the month title foreground.</value>
        public Color MonthTitleForegroundColor
        {
            get
            {
                return (Color)base.GetValue(ECalendarView.MonthTitleForegroundColorProperty);
            }
            set
            {
                base.SetValue(ECalendarView.MonthTitleForegroundColorProperty, value);
            }
        }


        /**
		 * DayOfWeekLabelForegroundColorProperty property
		 */
        /// <summary>
        /// The month title background color property
        /// </summary>
        public static readonly BindableProperty MonthTitleBackgroundColorProperty = BindableProperty.Create("MonthTitleBackgroundColor", typeof(Color), typeof(ECalendarView), Color.Default, BindingMode.OneWay, null, null, null, null);

        /**
		 * Background color of week day labels in the month header. Default color is platform specific.
		 */
        /// <summary>
        /// Gets or sets the color of the month title background.
        /// </summary>
        /// <value>The color of the month title background.</value>
        public Color MonthTitleBackgroundColor
        {
            get
            {
                return (Color)base.GetValue(ECalendarView.MonthTitleBackgroundColorProperty);
            }
            set
            {
                base.SetValue(ECalendarView.MonthTitleBackgroundColorProperty, value);
            }
        }

        /**
		 * DateSeparatorColorProperty property
		 */
        /// <summary>
        /// The date separator color property
        /// </summary>
        public static readonly BindableProperty DateSeparatorColorProperty = BindableProperty.Create("DateSeparatorColor", typeof(Color), typeof(ECalendarView), Color.Default, BindingMode.OneWay, null, null, null, null);

        /**
		 * Color of separator between dates. Default color is platform specific.
		 */
        /// <summary>
        /// Gets or sets the color of the date separator.
        /// </summary>
        /// <value>The color of the date separator.</value>
        public Color DateSeparatorColor
        {
            get
            {
                return (Color)base.GetValue(ECalendarView.DateSeparatorColorProperty);
            }
            set
            {
                base.SetValue(ECalendarView.DateSeparatorColorProperty, value);
            }
        }



        /**
		 * ShowNavigationArrowsProperty property
		 */
        /// <summary>
        /// The show navigation arrows property
        /// </summary>
        public static readonly BindableProperty ShowNavigationArrowsProperty = BindableProperty.Create("ShowNavigationArrows", typeof(bool), typeof(ECalendarView), false, BindingMode.OneWay, null, null, null, null);

        /**
		 * Whether to show navigation arrows for going through months. The navigation arrows 
		 */
        /// <summary>
        /// Gets or sets a value indicating whether [show navigation arrows].
        /// </summary>
        /// <value><c>true</c> if [show navigation arrows]; otherwise, <c>false</c>.</value>
        public bool ShowNavigationArrows
        {
            get
            {
                return (bool)base.GetValue(ECalendarView.ShowNavigationArrowsProperty);
            }
            set
            {
                base.SetValue(ECalendarView.ShowNavigationArrowsProperty, value);
            }
        }

        /**
		 * NavigationArrowsColorProperty property
		 */
        /// <summary>
        /// The navigation arrows color property
        /// </summary>
        public static readonly BindableProperty NavigationArrowsColorProperty = BindableProperty.Create("NavigationArrowsColorProperty", typeof(Color), typeof(ECalendarView), Color.Default, BindingMode.OneWay, null, null, null, null);

        /**
		 * Color of the navigation colors (if shown). Default color is platform specific
		 */
        /// <summary>
        /// Gets or sets the color of the navigation arrows.
        /// </summary>
        /// <value>The color of the navigation arrows.</value>
        public Color NavigationArrowsColor
        {
            get
            {
                return (Color)base.GetValue(ECalendarView.NavigationArrowsColorProperty);
            }
            set
            {
                base.SetValue(ECalendarView.NavigationArrowsColorProperty, value);
            }
        }


        /**
		 * ShouldHighlightDaysOfWeekLabelsProperty property
		 */
        /// <summary>
        /// The should highlight days of week labels property
        /// </summary>
        public static readonly BindableProperty ShouldHighlightDaysOfWeekLabelsProperty = BindableProperty.Create("ShouldHighlightDaysOfWeekLabels", typeof(bool), typeof(ECalendarView), false, BindingMode.OneWay, null, null, null, null);

        /**
		 * Whether to highlight also the labels of week days when the entire column is highlighted.
		 */
        /// <summary>
        /// Gets or sets a value indicating whether [should highlight days of week labels].
        /// </summary>
        /// <value><c>true</c> if [should highlight days of week labels]; otherwise, <c>false</c>.</value>
        public bool ShouldHighlightDaysOfWeekLabels
        {
            get
            {
                return (bool)base.GetValue(ECalendarView.ShouldHighlightDaysOfWeekLabelsProperty);
            }
            set
            {
                base.SetValue(ECalendarView.ShouldHighlightDaysOfWeekLabelsProperty, value);
            }
        }



        /**
		 * HighlightedDaysOfWeekProperty property
		 */
        /// <summary>
        /// The highlighted days of week property
        /// </summary>
        public static readonly BindableProperty HighlightedDaysOfWeekProperty = BindableProperty.Create("HighlightedDaysOfWeek", typeof(DayOfWeek[]), typeof(ECalendarView), new DayOfWeek[] { }, BindingMode.OneWay, null, null, null, null);

        /**
		 * Background color of selected date in the calendar. Default color is platform specific.
		 */
        /// <summary>
        /// Gets or sets the highlighted days of week.
        /// </summary>
        /// <value>The highlighted days of week.</value>
        public DayOfWeek[] HighlightedDaysOfWeek
        {
            get
            {
                return (DayOfWeek[])base.GetValue(ECalendarView.HighlightedDaysOfWeekProperty);
            }
            set
            {
                base.SetValue(ECalendarView.HighlightedDaysOfWeekProperty, value);
            }
        }
        public static readonly BindableProperty HighlightedDaysProperty = BindableProperty.Create("HighlightedDays", typeof(List<DateTime>), typeof(ECalendarView));

        public List<DateTime> HighlightedDays
        {
            get { return (List<DateTime>)GetValue(HighlightedDaysProperty); }
            set { SetValue(HighlightedDaysProperty, value); }
        }





        #region ColorHelperProperties

        /// <summary>
        /// Gets the actual color of the date background.
        /// </summary>
        /// <value>The actual color of the date background.</value>
        public Color ActualDateBackgroundColor
        {
            get
            {
                return this.DateBackgroundColor;
            }

        }

        /// <summary>
        /// Gets the actual color of the date foreground.
        /// </summary>
        /// <value>The actual color of the date foreground.</value>
        public Color ActualDateForegroundColor
        {
            get
            {
                if (this.DateForegroundColor != Color.Default)
                {
                    return this.DateForegroundColor;
                }
                return this.TextColor;
            }
        }

        /// <summary>
        /// Gets the actual color of the inactive date background.
        /// </summary>
        /// <value>The actual color of the inactive date background.</value>
        public Color ActualInactiveDateBackgroundColor
        {
            get
            {
                if (this.InactiveDateBackgroundColor != Color.Default)
                {
                    return this.InactiveDateBackgroundColor;
                }
                return this.ActualDateBackgroundColor;
            }

        }

        /// <summary>
        /// Gets the actual color of the inactive date foreground.
        /// </summary>
        /// <value>The actual color of the inactive date foreground.</value>
        public Color ActualInactiveDateForegroundColor
        {
            get
            {
                if (this.InactiveDateForegroundColor != Color.Default)
                {
                    return this.InactiveDateForegroundColor;
                }
                return this.ActualDateForegroundColor;
            }
        }

        /// <summary>
        /// Gets the actual color of the today date foreground.
        /// </summary>
        /// <value>The actual color of the today date foreground.</value>
        public Color ActualTodayDateForegroundColor
        {
            get
            {
                if (this.TodayDateForegroundColor != Color.Default)
                {
                    return this.TodayDateForegroundColor;
                }
                return this.ActualDateForegroundColor;
            }
        }
        /// <summary>
        /// Gets the actual color of the today date background.
        /// </summary>
        /// <value>The actual color of the today date background.</value>
        public Color ActualTodayDateBackgroundColor
        {
            get
            {
                if (this.TodayDateBackgroundColor != Color.Default)
                {
                    return this.TodayDateBackgroundColor;
                }
                return this.ActualDateBackgroundColor;
            }
        }

        /// <summary>
        /// Gets the actual color of the selected date foreground.
        /// </summary>
        /// <value>The actual color of the selected date foreground.</value>
        public Color ActualSelectedDateForegroundColor
        {
            get
            {
                if (this.SelectedDateForegroundColor != Color.Default)
                {
                    return this.SelectedDateForegroundColor;
                }
                return this.ActualDateForegroundColor;
            }
        }

        /// <summary>
        /// Gets the actual color of the selected date background.
        /// </summary>
        /// <value>The actual color of the selected date background.</value>
        public Color ActualSelectedDateBackgroundColor
        {
            get
            {
                if (this.SelectedDateBackgroundColor != Color.Default)
                {
                    return this.SelectedDateBackgroundColor;
                }
                return this.ActualDateBackgroundColor;
            }
        }

        /// <summary>
        /// Gets the actual color of the month title foreground.
        /// </summary>
        /// <value>The actual color of the month title foreground.</value>
        public Color ActualMonthTitleForegroundColor
        {
            get
            {
                if (this.MonthTitleForegroundColor != Color.Default)
                {
                    return MonthTitleForegroundColor;
                }
                return this.TextColor;
            }
        }

        /// <summary>
        /// Gets the actual color of the month title background.
        /// </summary>
        /// <value>The actual color of the month title background.</value>
        public Color ActualMonthTitleBackgroundColor
        {
            get
            {
                if (this.MonthTitleBackgroundColor != Color.Default)
                {
                    return MonthTitleBackgroundColor;
                }
                return this.BackgroundColor;
            }
        }

        /// <summary>
        /// Gets the actual color of the day of week label foreground.
        /// </summary>
        /// <value>The actual color of the day of week label foreground.</value>
        public Color ActualDayOfWeekLabelForegroundColor
        {
            get
            {
                if (this.DayOfWeekLabelForegroundColor != Color.Default)
                {
                    return DayOfWeekLabelForegroundColor;
                }
                return this.TextColor;
            }
        }

        /// <summary>
        /// Gets the actual color of the day of week label backround.
        /// </summary>
        /// <value>The actual color of the day of week label backround.</value>
        public Color ActualDayOfWeekLabelBackroundColor
        {
            get
            {
                if (this.DayOfWeekLabelBackgroundColor != Color.Default)
                {
                    return DayOfWeekLabelBackgroundColor;
                }
                return this.BackgroundColor;
            }
        }

        /// <summary>
        /// Gets the actual color of the navigation arrows.
        /// </summary>
        /// <value>The actual color of the navigation arrows.</value>
        public Color ActualNavigationArrowsColor
        {
            get
            {
                if (this.NavigationArrowsColor != Color.Default)
                {
                    return NavigationArrowsColor;
                }
                return this.ActualMonthTitleForegroundColor;
            }
        }

        /// <summary>
        /// Gets the actual color of the highlighted date foreground.
        /// </summary>
        /// <value>The actual color of the highlighted date foreground.</value>
        public Color ActualHighlightedDateForegroundColor
        {
            get
            {
                return HighlightedDateForegroundColor;
            }
        }

        /// <summary>
        /// Gets the actual color of the highlighted date background.
        /// </summary>
        /// <value>The actual color of the highlighted date background.</value>
        public Color ActualHighlightedDateBackgroundColor
        {
            get
            {
                return HighlightedDateBackgroundColor;
            }
        }
        #endregion



        /// <summary>
        /// Initializes a new instance of the <see cref="ECalendarView"/> class.
        /// </summary>
        public ECalendarView()
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                HeightRequest = 198 + 20; //This is the size of the original iOS calendar
            }
            else if (Device.OS == TargetPlatform.Android)
            {
                HeightRequest = 300; //This is the size in which Android calendar renders comfortably on most devices
            }

        }

        /// <summary>
        /// Notifies the displayed month changed.
        /// </summary>
        /// <param name="date">The date.</param>
        public void NotifyDisplayedMonthChanged(DateTime date)
        {
            DisplayedMonth = date;
            if (MonthChanged != null)
                MonthChanged(this, date);
        }
        /// <summary>
        /// Occurs when [month changed].
        /// </summary>
        public event EventHandler<DateTime> MonthChanged;


        /// <summary>
        /// Notifies the date selected.
        /// </summary>
        /// <param name="dateSelected">The date selected.</param>
        public void NotifyDateSelected(DateTime dateSelected)
        {
            SelectedDate = dateSelected;
            if (DateSelected != null)
                DateSelected(this, dateSelected);
        }

        /// <summary>
        /// Occurs when [date selected].
        /// </summary>
        public event EventHandler<DateTime> DateSelected;



    }
}
