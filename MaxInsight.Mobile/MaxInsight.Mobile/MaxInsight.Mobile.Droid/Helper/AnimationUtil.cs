using System;
using Android.App;
using Android.Content;
using Android.Views.Animations;

namespace MaxInsight.Mobile.Droid
{
    public class AnimationUtil
    {
        public static LayoutAnimationController getListAnimTranslate()
        {
            try
            {
                AnimationSet set = new AnimationSet(true);
                Animation animation = new AlphaAnimation(0.0f, 1.0f);
                animation.Duration = 500;//(500);
                set.AddAnimation(animation);

                animation = new TranslateAnimation(Dimension.RelativeToSelf, 0.0f,
                        Dimension.RelativeToSelf, 0.0f, Dimension.RelativeToSelf,
                        -1.0f, Dimension.RelativeToSelf, 0.0f);
                animation.Duration = 800; //(800);
                set.AddAnimation(animation);
                LayoutAnimationController controller = new LayoutAnimationController(
                        set, 0.5f);

                controller.Order = DelayOrder.Random;
                return controller;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /**
		 * 退出Activity的动画 : zoom 动画
		 * 
		 * @param context
		 */
        public static void finishActivityAnimation(Context context)
        {
            try
            {
                ((Activity)context).Finish();
                ((Activity)context).OverridePendingTransition(Resource.Animation.zoom_enter,
                        Resource.Animation.zoom_exit);
            }
            catch (Exception)
            {
            }
        }

        /***
		 * zoom 动画s
		 * 
		 * @param context
		 */
        public static void activityZoomAnimation(Context context)
        {
            try
            {
                ((Activity)context).OverridePendingTransition(Resource.Animation.zoom_enter,
                        Resource.Animation.zoom_exit);
            }
            catch (Exception)
            {
            }
        }
    }
}
