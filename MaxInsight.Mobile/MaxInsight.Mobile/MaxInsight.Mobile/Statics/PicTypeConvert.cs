using System;
using System.Globalization;
using Xamarin.Forms;

namespace MaxInsight.Mobile
{
	public class PicTypeConvert : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null && !string.IsNullOrEmpty(value.ToString()))
			{
				if (value.ToString() == "G")
				{
					return "现场照片";
				}
				else if (value.ToString() == "L")
				{
					return (string)"失分照片";
				}
			}

			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
