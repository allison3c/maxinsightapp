using System;
using System.ComponentModel;
using MaxInsight.Mobile;
using MaxInsight.Mobile.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MListView), typeof(MListViewRenderer))]
namespace MaxInsight.Mobile.iOS
{
	public class MListViewRenderer : ListViewRenderer
	{
		private UIColor color = Color.FromHex("ecf0f1").ToUIColor();

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (Control == null)
			{
				return;
			}

			this.Control.TableFooterView = new UIView();

			if (e.PropertyName == "ItemsSource")
			{
				foreach (var cell in Control.VisibleCells)
				{
					cell.SelectionStyle = UITableViewCellSelectionStyle.Gray;
					//cell.BackgroundView = new UIView(cell.Frame);
					//cell.BackgroundView.BackgroundColor = color;
				}
			}
		}
	}
}
