using System;
using System.Globalization;
using Xamarin.Forms;

namespace MaxInsight.Mobile
{
	public class ScoreValueConvert : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null && (int)value == -1)
			{
				return "";
			}
			else
			{
				return value.ToString();
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
