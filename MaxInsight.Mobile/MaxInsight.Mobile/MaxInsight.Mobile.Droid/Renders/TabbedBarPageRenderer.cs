﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using BottomNavigationBar;
using BottomNavigationBar.Listeners;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using MaxInsight.Mobile;
using MaxInsight.Mobile.Droid.Renders;

[assembly: ExportRenderer(typeof(TabbedBarPage), typeof(TabbedBarPageRenderer))]
namespace MaxInsight.Mobile.Droid.Renders
{
	public class TabbedBarPageRenderer : VisualElementRenderer<TabbedBarPage>, IOnTabClickListener
	{
		bool _disposed;
		BottomNavigationBar.BottomBar _bottomBar;
		FrameLayout _frameLayout;
		IPageController _pageController;

		public TabbedBarPageRenderer()
		{
			AutoPackage = false;
		}

		#region IOnTabClickListener
		public void OnTabSelected(int position)
		{
			//Do we need this call? It's also done in OnElementPropertyChanged
			SwitchContent(Element.Children[position]);
			var bottomBarPage = Element as TabbedBarPage;
			bottomBarPage.CurrentPage = Element.Children[position];
			//bottomBarPage.RaiseCurrentPageChanged();
		}

		public void OnTabReSelected(int position)
		{
		}
		#endregion

		protected override void Dispose(bool disposing)
		{
			if (disposing && !_disposed)
			{
				_disposed = true;

				RemoveAllViews();

				foreach (Page pageToRemove in Element.Children)
				{
					IVisualElementRenderer pageRenderer = Platform.GetRenderer(pageToRemove);

					if (pageRenderer != null)
					{
						pageRenderer.ViewGroup.RemoveFromParent();
						pageRenderer.Dispose();
					}

					// pageToRemove.ClearValue (Platform.RendererProperty);
				}

				if (_bottomBar != null)
				{
					_bottomBar.SetOnTabClickListener(null);
					_bottomBar.Dispose();
					_bottomBar = null;
				}

				if (_frameLayout != null)
				{
					_frameLayout.Dispose();
					_frameLayout = null;
				}

				/*if (Element != null) {
					PageController.InternalChildren.CollectionChanged -= OnChildrenCollectionChanged;
				}*/
			}

			base.Dispose(disposing);
		}

		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();
			_pageController.SendAppearing();
		}

		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow();
			_pageController.SendDisappearing();
		}


		protected override void OnElementChanged(ElementChangedEventArgs<TabbedBarPage> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{

				TabbedBarPage bottomBarPage = e.NewElement;

				if (_bottomBar == null)
				{
					_pageController = PageController.Create(bottomBarPage);

					// create a view which will act as container for Page's
					_frameLayout = new FrameLayout(Forms.Context);
					_frameLayout.LayoutParameters = new FrameLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent, GravityFlags.Fill);
					AddView(_frameLayout, 0);

					// create bottomBar control
					_bottomBar = BottomNavigationBar.BottomBar.Attach(_frameLayout, null);
					_bottomBar.NoTabletGoodness();
					if (bottomBarPage.FixedMode)
					{
						_bottomBar.UseFixedMode();
					}

					switch (bottomBarPage.BarTheme)
					{
						case TabbedBarPage.BarThemeTypes.Light:
							break;
						case TabbedBarPage.BarThemeTypes.DarkWithAlpha:
							_bottomBar.UseDarkThemeWithAlpha(true);
							break;
						case TabbedBarPage.BarThemeTypes.DarkWithoutAlpha:
							_bottomBar.UseDarkThemeWithAlpha(false);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					_bottomBar.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
					_bottomBar.SetOnTabClickListener(this);

					UpdateTabs();
					UpdateBarBackgroundColor();
					UpdateBarTextColor();
					_bottomBar.SelectTabAtPosition(e.NewElement.Children.IndexOf(e.NewElement.CurrentPage), true);
				}

				if (bottomBarPage.CurrentPage != null)
				{
					SwitchContent(bottomBarPage.CurrentPage);
				}
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == nameof(TabbedPage.CurrentPage))
			{
				SwitchContent(Element.CurrentPage);
			}
			else if (e.PropertyName == NavigationPage.BarBackgroundColorProperty.PropertyName)
			{
				UpdateBarBackgroundColor();
			}
			else if (e.PropertyName == NavigationPage.BarTextColorProperty.PropertyName)
			{
				UpdateBarTextColor();
			}
		}

		protected virtual void SwitchContent(Page view)
		{
			Context.HideKeyboard(this);

			_frameLayout.RemoveAllViews();

			if (view == null)
			{
				return;
			}

			if (Platform.GetRenderer(view) == null)
			{
				Platform.SetRenderer(view, Platform.CreateRenderer(view));
			}

			_frameLayout.AddView(Platform.GetRenderer(view).ViewGroup);
		}

		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			int width = r - l;
			int height = b - t;

			var context = Context;

			_bottomBar.Measure(MeasureSpecFactory.MakeMeasureSpec(width, MeasureSpecMode.Exactly), MeasureSpecFactory.MakeMeasureSpec(height, MeasureSpecMode.AtMost));
			int tabsHeight = Math.Min(height, Math.Max(_bottomBar.MeasuredHeight, _bottomBar.MinimumHeight));

			if (width > 0 && height > 0)
			{
				_pageController.ContainerArea = new Rectangle(0, 0, context.FromPixels(width), context.FromPixels(_frameLayout.Height));
				ObservableCollection<Element> internalChildren = _pageController.InternalChildren;

				for (var i = 0; i < internalChildren.Count; i++)
				{
					var child = internalChildren[i] as VisualElement;

					if (child == null)
					{
						continue;
					}

					IVisualElementRenderer renderer = Platform.GetRenderer(child);
					var navigationRenderer = renderer as NavigationPageRenderer;
					if (navigationRenderer != null)
					{
						// navigationRenderer.ContainerPadding = tabsHeight;
					}
				}

				_bottomBar.Measure(MeasureSpecFactory.MakeMeasureSpec(width, MeasureSpecMode.Exactly), MeasureSpecFactory.MakeMeasureSpec(tabsHeight, MeasureSpecMode.Exactly));
				_bottomBar.Layout(0, 0, width, tabsHeight);
			}

			base.OnLayout(changed, l, t, r, b);
		}

		void UpdateBarBackgroundColor()
		{
			if (_disposed || _bottomBar == null)
			{
				return;
			}

			_bottomBar.SetBackgroundColor(Element.BarBackgroundColor.ToAndroid());
		}

		void UpdateBarTextColor()
		{
			if (_disposed || _bottomBar == null)
			{
				return;
			}

			_bottomBar.SetActiveTabColor(Element.BarTextColor.ToAndroid());
			// The problem SetActiveTabColor does only work in fiexed mode // haven't found yet how to set text color for tab items on_bottomBar, doesn't seem to have a direct way
		}

		void UpdateTabs()
		{
			// create tab items
			SetTabItems();

			// set tab colors
			SetTabColors();
		}

		void SetTabItems()
		{
			BottomBarTab[] tabs = Element.Children.Select(page =>
			{
				var tabIconId = ResourceManagerEx.IdFromTitle(page.Icon, ResourceManager.DrawableClass);
				return new BottomBarTab(tabIconId, page.Title);
			}).ToArray();

			_bottomBar.SetItems(tabs);
		}

		void SetTabColors()
		{
			for (int i = 0; i < Element.Children.Count; ++i)
			{
				Page page = Element.Children[i];

				Color? tabColor = page.GetTabColor();

				if (tabColor != null)
				{
					_bottomBar.MapColorForTab(i, tabColor.Value.ToAndroid());
				}
			}
		}
	}
}
