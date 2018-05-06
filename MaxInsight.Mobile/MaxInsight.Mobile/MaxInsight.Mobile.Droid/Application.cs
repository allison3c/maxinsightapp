using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using System.Reflection;


[assembly: Permission(Name = MaxInsight.Mobile.Droid.Application.JPUSH_MESSAGE_PERMISSION, ProtectionLevel = Protection.Signature)]
[assembly: UsesPermission(Name = MaxInsight.Mobile.Droid.Application.JPUSH_MESSAGE_PERMISSION)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.WakeLock)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.Internet)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.Vibrate)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.ReadPhoneState)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.WriteExternalStorage)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.ReadExternalStorage)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.MountUnmountFilesystems)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.AccessNetworkState)]
[assembly: UsesPermission(Name = "android.permission.RECEIVE_USER_PRESENT")]
[assembly: UsesPermission(Name = Android.Manifest.Permission.WriteSettings)]

namespace MaxInsight.Mobile.Droid
{
    //[Application]
    [MetaData("JPUSH_CHANNEL", Value = "developer-default")]
    [MetaData("JPUSH_APPKEY", Value = "7ea762fe7e192e824514c1c2")]
    //[MetaData("JPUSH_APPKEY", Value = "5af3416249f70699720994dd")]
    public class Application : Android.App.Application
    {
        public const string JPUSH_MESSAGE_PERMISSION = "com.maxinsight.mobile.droid.permission.JPUSH_MESSAGE";

        public Application(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
        }
    }
}