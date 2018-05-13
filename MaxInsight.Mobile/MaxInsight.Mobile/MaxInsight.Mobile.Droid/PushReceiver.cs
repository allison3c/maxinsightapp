
using Android.App;
using Android.Content;
using Android.OS;
using CN.Jpush.Android.Api;

namespace MaxInsight.Mobile.Droid
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new string[] {
        "cn.jpush.android.intent.REGISTRATION",
        "cn.jpush.android.intent.UNREGISTRATION" ,
        "cn.jpush.android.intent.MESSAGE_RECEIVED",
        "cn.jpush.android.intent.NOTIFICATION_RECEIVED",
        "cn.jpush.android.intent.NOTIFICATION_OPENED",
        "cn.jpush.android.intent.ACTION_RICHPUSH_CALLBACK",
        "cn.jpush.android.intent.CONNECTION"
    }, Categories = new string[] { "com.maxinsight.toyota.droid" })]
    public class MaxInsightPushReceiver : BroadcastReceiver
    {
        Notification notification;

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == "cn.jpush.android.intent.REGISTRATION")
            {

            }
            else if (intent.Action == "cn.jpush.android.intent.NOTIFICATION_RECEIVED")
            {
                //var builder = new Notification.Builder(context)
                //                  .SetAutoCancel(true)
                //                .SetDefaults(NotificationDefaults.Sound)
                //    .SetContentTitle("Eland Sales")
                //    .SetSmallIcon(Resource.Drawable.icon)
                //                .SetContentText(intent.Extras.GetString(JPushInterface.ExtraAlert));
                //const int notificationId = 10001000;
                //var notificationManager =
                //    context.GetSystemService(Context.NotificationService) as NotificationManager;
                //notification = builder.Build();

                //notification.Defaults |= NotificationDefaults.Vibrate;

                //notificationManager.Notify(notificationId, notification);
            }
            else if (intent.Action == "cn.jpush.android.intent.MESSAGE_RECEIVED")
            {
                int notificationId = int.Parse(System.DateTime.Now.ToString("HHmmss")); //10001000;
                PendingIntent pendIntent = PendingIntent.GetActivity(context, notificationId, intent, PendingIntentFlags.UpdateCurrent);

                var builder = new Notification.Builder(context)
                                .SetAutoCancel(true)
                                .SetDefaults(NotificationDefaults.Sound)
                                .SetContentIntent(pendIntent)
                                .SetContentTitle(intent.Extras.GetString(JPushInterface.ExtraTitle))
                                .SetSmallIcon(Resource.Drawable.icon)
                                .SetContentText(intent.Extras.GetString(JPushInterface.ExtraMessage));
                //receivingNotification(context, intent.Extras);

                var notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
                notification = builder.Build();

                notification.Defaults |= NotificationDefaults.Vibrate;

                notificationManager.Notify(notificationId, notification);
            }
        }

        private void receivingNotification(Context context, Bundle bundle)
        {
            string title = bundle.GetString(JPushInterface.ExtraTitle);
            string message = bundle.GetString(JPushInterface.ExtraMessage);
            string extras = bundle.GetString(JPushInterface.ExtraExtra);
        }

    }
}