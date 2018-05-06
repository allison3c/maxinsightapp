using System;
using System.Globalization;
using Xamarin.Forms;

namespace MaxInsight.Mobile
{
	public class FinisCheckBool : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null && !string.IsNullOrEmpty(value.ToString()))
			{
				if (value.ToString() == "S")
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return false;
		}
	}
}
