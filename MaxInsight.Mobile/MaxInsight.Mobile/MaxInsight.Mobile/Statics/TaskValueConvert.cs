using System;
using System.Globalization;
using Xamarin.Forms;

namespace MaxInsight.Mobile
{
	public class TaskValueConvert : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null && value.ToString() == "E")
			{
				return "检查完成";
			}
			else 
			{
				return "未结束";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
