using System;
using Android;

namespace MaxInsight.Mobile.Droid
{
	public class Runnable : Java.Lang.Object, Java.Lang.IRunnable
	{
		Action action;
		public Runnable(Action action)
		{
			this.action = action;
		}

		public void Run()
		{
			action();
		}
	}
}
