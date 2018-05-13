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
    //DEV
    //[MetaData("JPUSH_APPKEY", Value = "5055c3255b11e1bbe83be12b")]
    //PRD
    [MetaData("JPUSH_APPKEY", Value = "8872d28cb15c10e67add04e0")]
    public class Application : Android.App.Application
    {
        public const string JPUSH_MESSAGE_PERMISSION = "com.maxinsight.toyota.droid.permission.JPUSH_MESSAGE";

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