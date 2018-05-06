using System;
using Xamarin.Forms;

namespace MaxInsight.Mobile
{
	public class TabbedBarPage : TabbedPage
	{
		public enum BarThemeTypes { Light, DarkWithAlpha, DarkWithoutAlpha }

		public bool FixedMode { get; set; }
		public BarThemeTypes BarTheme { get; set; }

		public void RaiseCurrentPageChanged()
		{
			OnCurrentPageChanged();
		}
	}
}
