using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace MaxInsight.Mobile.Droid
{
    [Activity(Label = "大众进口汽车区域管理工具包", Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar.Fullscreen", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class WelcomActivity : Activity
    {
        private List<string> guidImage = new List<string>() { "guide_0.jpg", "guide_1.jpg", "guide_2.jpg" };
        HorizontalScrollView scroll;
        LinearLayout linear;
        LinearLayout linearDto;
        DisplayMetrics dm = new DisplayMetrics();
        int screenWidth, screenHeigth;
        int downX;
        Handler handler;
        Runnable runnable;
        Runnable goMainPageRunnable;
        int currentPage, imageCount;
        AssetManager assets;
        Task startupWork;
        Button btnMain;
        bool isMoveRight;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var preferences = GetSharedPreferences("FIRSTSTART", FileCreationMode.WorldReadable);
            if (preferences.GetString(CommonContext.FIRSTINSTALLYN, "") == "")
            {
                SetContentView(Resource.Layout.welcome_activity);


                WindowManager.DefaultDisplay.GetMetrics(dm);
                screenWidth = GetScreenWidth();
                screenHeigth = GetScreenHeigth();

                handler = new Handler();
                runnable = new Runnable(Run);
                goMainPageRunnable = new Runnable(GoLaunchActivity);

                imageCount = guidImage.Count;

                assets = this.Assets;

                InitView();
                UpdateDtoImageView();

                startupWork = new Task(() =>
                {
                });

                startupWork.ContinueWith(t =>
                {
                    new Handler().Post(() =>
                    {
                        GoLaunchActivity();
                    });
                }, TaskScheduler.FromCurrentSynchronizationContext());

                var editor = preferences.Edit();
                editor.PutString(CommonContext.FIRSTINSTALLYN, "1");
                editor.Commit();
            }
            else
            {
                GoLaunchActivity();
            }
        }

        void InitView()
        {
            scroll = (HorizontalScrollView)FindViewById(Resource.Id.hscroll);
            linear = (LinearLayout)FindViewById(Resource.Id.linear);
            linearDto = (LinearLayout)FindViewById(Resource.Id.linear_dot);
            btnMain = (Button)FindViewById(Resource.Id.goMain);

            scroll.HorizontalScrollBarEnabled = false;

            scroll.Touch += ScrollView_OnTouch;

            foreach (var item in guidImage)
            {
                var imageView = new ImageView(this);
                imageView.LayoutParameters = new Android.Views.ViewGroup.LayoutParams(screenWidth, screenHeigth);

                Bitmap bitmap = Getbitmap(item);
                if (null != bitmap)
                {
                    imageView.SetImageBitmap(Getbitmap(item));
                    bitmap.Recycle();
                }
                linear.AddView(imageView);

                var dtImageView = new ImageView(this);
                dtImageView.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                                                                          ViewGroup.LayoutParams.WrapContent);
                dtImageView.SetImageResource(Resource.Drawable.icon_dark);
                linearDto.AddView(dtImageView);

                if (guidImage.IndexOf(item) != imageCount - 1)
                {
                    var spaceView = new View(this);
                    spaceView.LayoutParameters = new ViewGroup.LayoutParams(5, ViewGroup.LayoutParams.MatchParent);
                    linearDto.AddView(spaceView);
                }
            }

            btnMain.Click += BtnMain_GoMainPage;
        }

        private void ScrollView_OnTouch(object sender, Android.Views.View.TouchEventArgs e)
        {
            switch (e.Event.Action)
            {
                case Android.Views.MotionEventActions.Down:
                    downX = (int)e.Event.GetX();
                    break;
                case Android.Views.MotionEventActions.Up:
                case Android.Views.MotionEventActions.Cancel:
                    if (Java.Lang.Math.Abs(e.Event.GetX() - downX) > screenWidth / 6)
                    {
                        if (e.Event.GetX() - downX > 0)
                        {
                            isMoveRight = false;
                            SmoothScrollToPrePage();
                        }
                        else
                        {
                            isMoveRight = true;
                            SmoothScrollToNextPage();
                        }
                    }
                    else
                    {
                        SmoothScrollToCurrent();
                    }
                    break;

            }
        }

        private void BtnMain_GoMainPage(object sender, EventArgs e)
        {
            //handler.Post(goMainPageRunnable);
            //GoMainActivity();
            GoLaunchActivity();
        }

        void SmoothScrollToPrePage()
        {
            if (currentPage > 0)
            {
                currentPage--;
                handler.Post(runnable);
            }
        }

        void SmoothScrollToNextPage()
        {
            if (currentPage < imageCount - 1)
            {
                currentPage++;
                handler.Post(runnable);
                //if (currentPage == imageCount - 1)
                //{
                //	startupWork.Start();
                //}
            }
            //else
            //{
            //	//startupWork.Start();
            //  if (!startupWork.IsCompleted)
            //	{
            //		startupWork.Dispose();
            //	}
            //	handler.Post(goMainPageRunnable);
            //}
        }

        void SmoothScrollToCurrent()
        {
            handler.Post(runnable);
        }

        void Run()
        {
            scroll.SmoothScrollTo(screenWidth * currentPage, 0);
            UpdateDtoImageView();
        }

        void UpdateDtoImageView()
        {
            ImageView preImage;
            ImageView nextImage;
            ImageView currentImage = (ImageView)linearDto.GetChildAt(currentPage * 2);
            if (isMoveRight)
            {
                preImage = (ImageView)linearDto.GetChildAt((currentPage - 1) * 2);
                preImage.SetImageResource(Resource.Drawable.icon_dark);
            }
            else
            {
                nextImage = (ImageView)linearDto.GetChildAt((currentPage + 1) * 2);
                nextImage.SetImageResource(Resource.Drawable.icon_dark);
            }
            currentImage.SetImageResource(Resource.Drawable.icon_white);
            //linearDto.chi
            if (currentPage == imageCount - 1)
            {
                btnMain.Visibility = ViewStates.Visible;
            }
            else
            {
                btnMain.Visibility = ViewStates.Gone;
            }
        }

        private void GoMainActivity()
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            //AnimationUtil.finishActivityAnimation(this);
            this.Finish();
        }
        private void GoLaunchActivity()
        {
            Intent intent = new Intent(this, typeof(LaunchActivity));
            StartActivity(intent);
            this.Finish();
        }


        private Bitmap Getbitmap(string fileName)
        {
            using (var input = assets.Open(fileName))
            {
                BitmapFactory.Options options = new BitmapFactory.Options();
                Bitmap bitmap = BitmapFactory.DecodeStream(input, null, options);
                if (bitmap != null) // && bitmap.Width > 4000)
                {
                    bitmap = Bitmap.CreateScaledBitmap(bitmap, screenWidth, screenHeigth, false);
                }

                return bitmap;
            }
        }

        public int GetImageResourceId(string name)
        {
            //Resource.Drawable drawables = new Resource.Drawable();
            //Drawable drawables = new Drawable();
            //默认的id  
            int resId = 0x7f02000b;
            try
            {
                //根据字符串字段名，取字段//根据资源的ID的变量名获得Field的对象,使用反射机制来实现的  
                //Java.Lang.Reflect.Field field = typeof(Resource.Drawable).GetField(name);
                //取值  
                //resId = fieldInfo.
            }
            catch (Java.Lang.Exception e)
            {
                // TODO Auto-generated catch block  
                e.PrintStackTrace();
            }
            return resId;
        }

        private int GetScreenWidth()
        {
            return dm.WidthPixels;
        }

        private int GetScreenHeigth()
        {
            return dm.HeightPixels;
        }
    }
}
