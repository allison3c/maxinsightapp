using System;
using MaxInsight.Mobile.Helpers;
using Acr.UserDialogs;
using System.Threading.Tasks;
using System.IO;
using Android.Graphics;
using Plugin.SecureStorage;
using CN.Jpush.Android.Api;
using System.Collections.Generic;
using Android.App;
using Android.Widget;
using System.Net;
using Android.Content;
using Android.Content.PM;
using MaxInsight.Mobile.Module;
using XLabs.Ioc;
using Plugin.FilePicker;

namespace MaxInsight.Mobile.Droid.Helper
{
    public class CommonFun_Droid : ICommonFun
    {
        string fileLocalPath = string.Empty;
        Notification.Builder builder;
        // Build the notification:
        Notification notification;
        RemoteViews contentView;
        PendingIntent p;
        // Get the notification manager:
        NotificationManager notificationManager;
        CommonHelper _commonHelper;
        Config.Config _config;

        /// <summary>
        /// show loading
        /// userdialog
        /// https://github.com/aritchie/userdialogs/blob/master/src/Samples/Samples/MainPage.cs
        /// </summary>
        public void ShowLoading(string msg)
        {
            try
            {
                UserDialogs.Instance.ShowLoading(msg);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// hide loading
        /// userdialog
        /// https://github.com/aritchie/userdialogs/blob/master/src/Samples/Samples/MainPage.cs
        /// </summary>
        public void HideLoading()
        {
            try
            {
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// alert
        /// https://github.com/aritchie/userdialogs/blob/master/src/Samples/Samples/MainPage.cs
        /// </summary>
        public async void Alert(string msg, string title)
        {
            await UserDialogs.Instance.AlertAsync(msg, null, "关闭");
        }

        /// <summary>
        /// alert
        /// https://github.com/aritchie/userdialogs/blob/master/src/Samples/Samples/MainPage.cs
        /// </summary>
        /// <param name="content"></param>
        public void AlertLongText(string msg)
        {
            UserDialogs.Instance.Alert(msg, null, "关闭");
        }

        public void ShowToast(string msg)
        {
            UserDialogs.Instance.Toast(new ToastConfig(msg).SetDuration(TimeSpan.FromSeconds(3)).SetAction(x => x.SetText("确定")));
        }

        public async Task<bool> Confirm(string msg)
        {
            var result = await UserDialogs.Instance.ConfirmAsync(msg, "温馨提示", "确定", "取消");

            return result;
        }

        public async Task<bool> NoCancelConfirm(string msg)
        {
            var result = await UserDialogs.Instance.ConfirmAsync(msg, "温馨提示", "确定", string.Empty);

            return result;
        }

        public async Task<string> ShowPromt(string title)
        {
            PromptConfig config = new PromptConfig()
            {
                CancelText = "取消",
                InputType = InputType.Default,
                IsCancellable = true,
                OkText = "确定",
                Title = title,
                Text = ""
            };
            var result = await UserDialogs.Instance.PromptAsync(config);
            if (result.Ok)
            {
                return result.Text;
            }
            return "Cancel";
        }

        public async Task<string> ShowActionSheetAny(params string[] buttons)
        {
            var result = await UserDialogs.Instance.ActionSheetAsync("请选择", "取消", null, null, buttons);

            if (result != null)
            {
                return result;
            }

            return string.Empty;
        }

        public async Task<string> ShowActionSheet(string action1, string action2, string action3 = "")
        {

            if (!string.IsNullOrEmpty(action3))
            {
                var result = await UserDialogs.Instance.ActionSheetAsync("请选择", "取消", null, null, action1, action2, action3);

                if (result != null)
                {
                    return result;
                }
            }
            else
            {
                var result = await UserDialogs.Instance.ActionSheetAsync("请选择", "取消", null, null, action1, action2);

                if (result != null)
                {
                    return result;
                }
            }

            return string.Empty;
        }
        public Stream GetFileStream(byte[] imageData, int width, int height)
        {
            try
            {
                byte[] resizedImage = ResizeImageAndroid(imageData, width, height);
                return new MemoryStream(resizedImage);
                //this._photo.Source = ImageSource.FromStream(() => new MemoryStream(resizedImage));
            }
            catch (Exception)
            {
            }
            return null;
        }

        //public Stream GetFileStream(string filePath)
        //{
        //	string path = System.IO.Path.Combine(filePath);
        //	//Java.IO.File file = new Java.IO.File(path);
        //	//System.IO.File.Open(
        //	byte[] imageData;

        //	if (System.IO.File.Exists(path))
        //	{
        //		try
        //		{
        //			using (var stream = System.IO.File.Open(path, FileMode.Create, FileAccess.ReadWrite))
        //			{
        //				using (MemoryStream ms = new MemoryStream())
        //				{
        //					stream.CopyTo(ms);
        //					imageData = ms.ToArray();
        //				}

        //				byte[] resizedImage = ResizeImageAndroid(imageData, 400, 400);
        //				return new MemoryStream(resizedImage);
        //				//this._photo.Source = ImageSource.FromStream(() => new MemoryStream(resizedImage));
        //			}
        //		}
        //		catch (Exception ex)
        //		{
        //		}
        //	}

        //	return null;
        //}

        private static byte[] ResizeImageAndroid(byte[] imageData, float width, float height)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, originalImage.Width, originalImage.Height, false);

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 50, ms);
                return ms.ToArray();
            }
        }

        public void SetCach(string key, string value)
        {
            CrossSecureStorage.Current.SetValue(key, value);
        }

        public string GetCach(string key)
        {
            if (CrossSecureStorage.Current.HasKey(key))
            {
                return CrossSecureStorage.Current.GetValue(key);
            }

            return string.Empty;
        }

        public void DeleteCach(string key)
        {
            if (CrossSecureStorage.Current.HasKey(key))
            {
                CrossSecureStorage.Current.DeleteKey(key);
            }
        }

        public void JPushSetAlias(string aliasName)
        {
            JPushInterface.SetAlias(Application.Context, aliasName, new TagAliasCallback());
        }

        public void ExistSystem()
        {
            try
            {
                Java.Lang.JavaSystem.Exit(0);
                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            }
            catch (Exception)
            {
                Java.Lang.JavaSystem.Exit(1);
            }
        }

        #region DownLoadFile
        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="url">下载文件的URL地址</param>
        /// <param name="exterName">保存到本地的文件扩展名</param>
        /// <returns></returns>
        public void DownLoadFile(string url, string exterName)
        {
            // Instantiate the builder and set notification elements:
            builder = new Notification.Builder(Application.Context.ApplicationContext);
            // Build the notification:
            notification = builder.Build();
            contentView = new RemoteViews(Application.Context.PackageName, Resource.Layout.progress_notify);
            // Get the notification manager:
            notificationManager =
                Application.Context.ApplicationContext.GetSystemService(Context.NotificationService) as NotificationManager;


            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string localFilename = DateTime.Now.ToString("yyyyMMddHHmmss") + exterName;

            fileLocalPath = System.IO.Path.Combine(GetPath().Path, localFilename);
            WebClient webClient = new WebClient();

            try
            {
                webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
                Task.Factory.StartNew(() => { webClient.DownloadFileAsync(new Uri(url), fileLocalPath); });

            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (System.Exception)
            {
                return;
            }
        }
        public void DownLoadFileFromOss(string url, string fileName, string localFile)
        {
            string localFilename = fileName;

            fileLocalPath = System.IO.Path.Combine(GetImagePath(localFile), localFilename);
            WebClient webClient = new WebClient();

            try
            {
                ShowLoading("");
                webClient.DownloadFileCompleted += WebClient_DownloadImageCompleted;
                Task t = Task.Factory.StartNew(() =>
                {
                    webClient.DownloadFileAsync(new Uri(url), fileLocalPath);
                });
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (System.Exception)
            {
                return;
            }
        }
        private void WebClient_DownloadImageCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                //install app
                if (e.Cancelled)
                {
                    Toast.MakeText(Application.Context.ApplicationContext, e.Error.Message, ToastLength.Long).Show();
                    return;
                }
                HideLoading();
                Java.IO.File fileToOpen = new Java.IO.File(fileLocalPath);
                var uri = Android.Net.Uri.FromFile(fileToOpen);
                var intent = new Intent();
                var mime = IOUtil.GetMimeType(uri.ToString());
                intent.SetAction(Intent.ActionView);
                intent.SetDataAndType(uri, mime);
                intent.SetFlags(ActivityFlags.NewTask);

                Android.App.Application.Context.StartActivity(intent);
            }
            catch (Exception)
            {
                AlertLongText("文件格式不支持预览，请到下载目录查看." + fileLocalPath.Replace("emulated/0/", ""));
            }

        }
        private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //install app
            if (e.Cancelled)
            {
                Toast.MakeText(Application.Context.ApplicationContext, e.Error.Message, ToastLength.Long).Show();
                return;
            }

            notificationManager.CancelAll();
            notificationManager.Dispose();

            Intent intent = new Intent(Intent.ActionView);
            intent.SetFlags(ActivityFlags.NewTask);
            intent.SetDataAndType(Android.Net.Uri.Parse(@"file://" + fileLocalPath), "application/vnd.android.package-archive");
            Application.Context.ApplicationContext.StartActivity(intent);
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage % 10 == 0)
            {
                Notify(e.ProgressPercentage);
            }
        }

        public void DownLoadFileFromOssForReport(string url, string fileName, string localFile)
        {
            string localFilename = fileName;

            fileLocalPath = System.IO.Path.Combine(GetImagePath(localFile), localFilename);
            WebClient webClient = new WebClient();

            try
            {
                ShowLoading("");
                Task t = Task.Factory.StartNew(() =>
                {
                    webClient.DownloadFileAsync(new Uri(url), fileLocalPath);
                });
                AlertLongText("请到如下路径查看文件 " + fileLocalPath.Replace("emulated/0/", ""));
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (System.Exception)
            {
                return;
            }
        }

        public Java.IO.File GetPath()
        {
            Java.IO.File file = new Java.IO.File(new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(""), Application.Context.PackageName), "cach");
            Java.IO.File newFile = new Java.IO.File(file, "RMMT");

            if (!newFile.Exists())
            {
                if (!newFile.Mkdirs())
                {
                    return null;
                }
                try
                {
                    new Java.IO.File(newFile, ".nomedia").CreateNewFile();
                }
                catch (Java.IO.IOException)
                {
                }
            }
            return newFile;
        }
        public void Notify(int rate)
        {
            try
            {
                contentView.SetProgressBar(Resource.Id.progress, 100, rate, false);
                contentView.SetTextViewText(Resource.Id.notificationTitle, "正在下载更新");
                contentView.SetTextViewText(Resource.Id.notificationPercent, rate + "%");

                notification.ContentView = contentView;
                notification.Flags = NotificationFlags.ForegroundService;
                notification.Icon = Resource.Drawable.icon;
                p = PendingIntent.GetActivity(Application.Context.ApplicationContext, 0, new Intent(Intent.ActionView), 0);
                notification.ContentIntent = p;

                // Publish the notification:
                const int notificationId = 0;
                notificationManager.Notify(notificationId, notification);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        public int GetVersionNo()
        {
            try
            {
                //PackageManager manager = Application.Context.PackageManager.GetPackageInfo.GetVersionNo();
                PackageInfo info = Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0);
                int version = info.VersionCode;
                return version;
            }
            catch (Java.Lang.Exception e)
            {
                return 0;
            }
        }

        public Task<APIResult> UploadFileToOSS(Stream stream, string path)
        {
            return null;
        }

        #region
        public string SaveAttachLocal(Stream stream, string filename)
        {
            _config = Resolver.Resolve<Config.Config>();
            string basePath = _config.Get<string>(Config.Config.AliyunOss_Domain);
            string dir = _config.Get<string>(Config.Config.AliyunOss_Dir);
            string filepath = string.Empty;
            try
            {
                string documentsPath = GetImagePath("RMMT_ORGIMAGE"); //CreateDirectoryForPictures().Path;
                string localFilename = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + filename.Substring(filename.LastIndexOf("."));
                string newFile = System.IO.Path.Combine(documentsPath, localFilename);

                if (File.Exists(filename))
                {
                    File.Copy(filename, newFile);
                    filepath = ResizeImage(newFile);
                    File.Delete(newFile);
                }
            }
            catch (System.Exception)
            {
                filepath = "";
            }
            if (filepath.LastIndexOf("/") != -1)
            {
                filepath = basePath + dir + filepath.Substring(filepath.LastIndexOf("/") + 1);
            }
            else
            {
                filepath = "";
            }
            return filepath;
        }

        public string GetFilesSizeOfUpload()
        {
            string documentsPath = GetImagePath("RMMT_IMAGE");
            long size = 0;
            foreach (var item in Directory.GetFiles(documentsPath))
            {
                FileInfo fi = new FileInfo(item);
                size += fi.Length;
            }
            if (size < 1024 * 1024)
            {
                return (int)(size / 1024) + "KB";
            }
            else
            {
                return (int)(size / 1024 / 1024) + "M";
            }

        }

        public async Task UploadLocalFileToServer()
        {
            _commonHelper = Resolver.Resolve<CommonHelper>();
            string documentsPath = GetImagePath("RMMT_IMAGE");
            int fileCnt = Directory.GetFiles(documentsPath).Length;
            int t = 100;
            if (fileCnt != 0)
            {
                t = (int)Math.Floor(((double)1.0) / fileCnt * 100);
            }

            using (var dialog = UserDialogs.Instance.Progress())
            {
                if (fileCnt > 0)
                {
                    foreach (var item in Directory.GetFiles(documentsPath))
                    {
                        APIResult result = await _commonHelper.UploadFileToOSS(GetAttachLocal(item), item.Substring(item.LastIndexOf("/") + 1), "L");
                        if (result.ResultCode == ResultType.Success)
                        {
                            File.Delete(item);
                        }
                        dialog.PercentComplete += t;
                    }
                    if (dialog.PercentComplete < 100)
                    {
                        dialog.PercentComplete += (100 - dialog.PercentComplete);
                    }
                }
                else
                {
                    dialog.PercentComplete += 100;
                }
                if (dialog.PercentComplete == 100)
                {
                    dialog.Hide();
                }
            }
        }

        public Stream GetAttachLocal(string path)
        {
            if (File.Exists(path))
            {
                Stream stream = new MemoryStream(File.ReadAllBytes(path));
                return stream;
            }
            else
            {
                return null;
            }
        }

        private string GetImagePath(string localFile)
        {
            Java.IO.File file = new Java.IO.File(new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(""), Application.Context.PackageName), "cach");
            Java.IO.File newFile = new Java.IO.File(file, localFile);//

            if (!newFile.Exists())
            {
                if (!newFile.Mkdirs())
                {
                    return null;
                }
                try
                {
                    new Java.IO.File(newFile, ".nomedia").CreateNewFile();
                }
                catch (Java.IO.IOException)
                {
                }
            }
            return newFile.Path;
        }

        private string ResizeImage(string sourceFilePath)
        {
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = false;
            options.InPreferredConfig = Bitmap.Config.Rgb565;
            options.InDither = true;

            Bitmap bmp = BitmapFactory.DecodeFile(sourceFilePath, options);

            string documentsPath = GetImagePath("RMMT_IMAGE"); //CreateDirectoryForPictures().Path;
            string localFilename = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + sourceFilePath.Substring(sourceFilePath.LastIndexOf("."));
            string newPath = System.IO.Path.Combine(documentsPath, localFilename);

            using (var fs = new FileStream(newPath, FileMode.OpenOrCreate,
                                       FileAccess.ReadWrite,
                                       FileShare.None))
            {
                bmp.Compress(Bitmap.CompressFormat.Jpeg, 50, fs);
            }

            return newPath;
        }

        public string GetTempPathForApiToOss(string localpath, string temppath, string localfilename)
        {
            return "";
        }
        public void DeleteFileForApiToOss(string tempPath)
        {
        }
        #endregion

        public Stream ResizeImage(Stream stream)
        {
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = false;
            options.InPreferredConfig = Bitmap.Config.Rgb565;
            options.InDither = true;

            Bitmap bmp = BitmapFactory.DecodeStream(stream, null, options);

            MemoryStream newStream = new MemoryStream();
            bmp.Compress(Bitmap.CompressFormat.Jpeg, 50, newStream);

            return newStream;
        }

        public void DownDB()
        {
            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MaxInsight.db3");
            string localPath = System.IO.Path.Combine(GetPath().Path, "MaxInsight.db3");

            try
            {
                if (System.IO.File.Exists(dbPath))
                {
                    if (System.IO.File.Exists(localPath))
                    {
                        System.IO.File.Delete(localPath);
                        string newLocalPath = System.IO.Path.Combine(GetPath().Path, "MaxInsight.db3");
                        System.IO.File.Copy(dbPath, newLocalPath);
                    }
                    else
                    {
                        System.IO.File.Copy(dbPath, localPath);
                    }
                }
            }
            catch (System.Exception)
            {
            }
        }
    }
    public class TagAliasCallback : ITagAliasCallback
    {
        public IntPtr Handle
        {
            get { return IntPtr.Zero; }
        }

        public void Dispose()
        {
        }

        public void GotResult(int p0, string p1, ICollection<string> p2)
        {
            if (p0 == 0)
            {

            }
            else
            {

            }
        }
    }
}