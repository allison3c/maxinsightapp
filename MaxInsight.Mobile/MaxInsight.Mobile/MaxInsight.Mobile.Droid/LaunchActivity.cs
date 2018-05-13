using Android.App;
using Android.OS;
using Android.Content;
using System.Threading.Tasks;
using Android.Content.PM;

namespace MaxInsight.Mobile.Droid
{
    [Activity(Label = "PCMÆÀ¹À¸ÄÉÆÆ½Ì¨", Icon ="@drawable/icon", Theme = "@android:style/Theme.NoTitleBar.Fullscreen", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class LaunchActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.launch_activity);

            Task startupWork = new Task(() =>
            {
                //UserDialogs.Init(this);
            });

            startupWork.ContinueWith(t =>
            {
                new Handler().PostDelayed(() =>
                {
                    GoMainActivity();
                }, 2000);
            }, TaskScheduler.FromCurrentSynchronizationContext());

            startupWork.Start();
        }

        private void GoMainActivity()
        {
            Intent intent = new Intent(this, typeof(MainActivity));
			//Intent intent = new Intent(this, typeof(WelcomActivity));
            StartActivity(intent);

			AnimationUtil.activityZoomAnimation(this);

            this.Finish();
        }
    }
}