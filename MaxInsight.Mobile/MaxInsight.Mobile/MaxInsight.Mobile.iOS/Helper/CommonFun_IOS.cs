using Acr.UserDialogs;
using MaxInsight.Mobile.Helpers;
using System;
using System.Threading.Tasks;
using System.IO;
using Plugin.SecureStorage;
using com.eland.ios.JPush;
using Foundation;
using MaxInsight.Mobile.Module;
using Aliyun.OSS;
using UIKit;
using XLabs.Ioc;
using System.Net;

namespace MaxInsight.Mobile.iOS.Helper
{
    public class CommonFun_IOS : ICommonFun
    {
        string fileLocalPath = string.Empty;
        CommonHelper _commonHelper;

        public async void Alert(string msg, string title)
        {
            try
            {
                await UserDialogs.Instance.AlertAsync(msg, null, "关闭");
            }
            catch (Exception)
            {

            }
        }

        public void AlertLongText(string msg)
        {
            try
            {
                UserDialogs.Instance.Alert(msg, null, "关闭");
            }
            catch (Exception ex)
            {

            }
        }

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

        public void ShowLoading(string msg)
        {
            try
            {
                UserDialogs.Instance.ShowLoading(msg);
            }
            catch (Exception ex)
            {
            }
        }

        public void ShowToast(string msg)
        {
            try
            {
                UserDialogs.Instance.Toast(new ToastConfig(msg).SetDuration(TimeSpan.FromSeconds(3)).SetAction(x => x.SetText("确定")));
            }
            catch (Exception)
            {

            }
        }

        public Task<string> ShowPromt(string title)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Confirm(string msg)
        {
            return UserDialogs.Instance.ConfirmAsync(msg, "温馨提示", "确定", "取消");
            //throw new NotImplementedException();
        }

        public Task<bool> NoCancelConfirm(string msg)
        {
            throw new NotImplementedException();
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

        public Stream GetFileStream(string filePath)
        {
            throw new NotImplementedException();
        }

        public Stream GetFileStream(byte[] imageData, int width, int height)
        {
            throw new NotImplementedException();
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

        public async Task<string> ShowActionSheetAny(params string[] buttons)
        {
            var result = await UserDialogs.Instance.ActionSheetAsync("请选择", "取消", null, null, buttons);

            if (result != null)
            {
                return result;
            }

            return string.Empty;
        }

        public void JPushSetAlias(string aliasName)
        {
            ObjCRuntime.Selector sel = new ObjCRuntime.Selector("tagsAliasCallback");
            NSObject obj = new NSObject();

            JPUSHService.SetAlias(aliasName, sel, obj);


        }

        private void tagsAliasCallback(int iResCode, NSSet tags, NSString alias)
        {

        }
        [System.Runtime.InteropServices.DllImport("__Internal", EntryPoint = "exit")]
        public static extern void exit(int status);
        public void ExistSystem()
        {
            //System.Threading.Thread.CurrentThread.Abort();
            //NSThread.Exit();
            exit(0);
        }

        public void DownLoadFile(string url, string exterName)
        {
        }

        public int GetVersionNo()
        {
            string version = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion").ToString();
            if (!string.IsNullOrEmpty(version))
            {
                return Convert.ToInt32(version);
            }
            return 0;
        }

        static OssClient _ossClient;
        public async Task<APIResult> UploadFileToOSS(Stream stream, string path)
        {
            CreateOssClientOfIOS();
            string key = path.LastIndexOf("/") != -1 ? path.Substring(path.LastIndexOf("/") + 1) : "";
            //_ossClient.PutObject("vgic", key, path);
            //path = path.Substring(1);
            stream.Position = 0;
            var result = await Task<PutObjectResult>.Factory.FromAsync(
                        _ossClient.BeginPutObject,
                        _ossClient.EndPutObject,
                        "vgic", key, stream,
                        null);
            AttachDto attachDto = new AttachDto();
            attachDto.AttachName = path.LastIndexOf("/") != -1 ? path.Substring(path.LastIndexOf("/") + 1) : "";
            //var metadata = _ossClient.GetObjectMetadata("vgic", key);
            //var etag = metadata.ETag;

            //var req = new GeneratePresignedUriRequest("vgic", key, SignHttpMethod.Get);
            attachDto.Url = "http://vgic.oss-cn-beijing.aliyuncs.com/" + attachDto.AttachName;
            return new APIResult { Body = CommonHelper.EncodeDto<AttachDto>(attachDto), ResultCode = ResultType.Success, Msg = "" };
        }

        private static OssClient CreateOssClientOfIOS()
        {
            if (_ossClient == null)
            {
                string accessKeyId = "3JkljJxvXgjLz80X";
                string accessKeySecret = "L2ERHORPk3WkjqfGUb27RlxvT8x5f3";
                string endpoint = "oss-cn-beijing.aliyuncs.com";
                //var conf = new ClientConfiguration();
                ////conf.IsCname = true;/// 配置使用Cname
                ////conf.ConnectionLimit = 512;  //HttpWebRequest最大的并发连接数目
                //conf.MaxErrorRetry = 3;     //设置请求发生错误时最大的重试次数
                //conf.ConnectionTimeout = 300;  //设置连接超时时间
                ////conf.SetCustomEpochTicks(customEpochTicks);        //设置自定义基准时间, CreateCancelTokenSource().Token.ToString()
                _ossClient = new OssClient(endpoint, accessKeyId, accessKeySecret);
                var exist = _ossClient.DoesBucketExist("vgic");
                if (!exist)
                    _ossClient.CreateBucket("vgic");
            }
            return _ossClient;
        }

        #region LocalImage
        public string SaveAttachLocal(Stream stream, string filename)
        {
            Config.Config _config = Resolver.Resolve<Config.Config>();
            string basePath = _config.Get<string>(Config.Config.AliyunOss_Domain);
            string dir = _config.Get<string>(Config.Config.AliyunOss_Dir);
            string filepath = string.Empty;
            try
            {
                string documentsPath = GetImagePath("RMMT_ORGIMAGE"); //CreateDirectoryForPictures().Path;
                string localFilename = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + (filename.LastIndexOf("/") != -1 ? filename.Substring(filename.LastIndexOf("/") + 1) : "");
                string newFile = Path.Combine(documentsPath, localFilename);

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
            string retPath;
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            retPath = Path.Combine(documents, NSBundle.MainBundle.BundleIdentifier, "cach", localFile);
            Directory.CreateDirectory(retPath);
            return retPath;
        }
        private string ResizeImage(string sourceFilePath)
        {
            string documentsPath = GetImagePath("RMMT_IMAGE"); //CreateDirectoryForPictures().Path;
            string localFilename = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + sourceFilePath.Substring(sourceFilePath.LastIndexOf("."));
            string newPath = Path.Combine(documentsPath, localFilename);

            if (File.Exists(sourceFilePath))
            {
                using (UIImage sourceImage = UIImage.FromFile(sourceFilePath))
                {
                    sourceImage.AsJPEG(0.5f).Save(newPath, true);
                }
            }
            return newPath;
        }
        public string GetTempPathForApiToOss(string localpath, string temppath, string localfilename)
        {
            string documentsPath = GetImagePath(temppath); //"RMMTIMAGEVIEW"
            string newPath = Path.Combine(documentsPath, localfilename);

            if (File.Exists(localpath))
            {
                using (UIImage sourceImage = UIImage.FromFile(localpath))
                {
                    sourceImage.AsJPEG(0.5f).Save(newPath, true);
                }
            }
            return newPath;
        }
        public void DeleteFileForApiToOss(string tempPath)
        {
            File.Delete(tempPath);
        }
        #endregion

        #region Resize Image
        public Stream ResizeImage(Stream stream)
        {
            // Load the bitmap
            UIImage originalImage = ImageFromByteArray(ReadFully(stream));
            //
            var bytesImagen = originalImage.AsJPEG(0.5f).ToArray();
            Stream compressStream = new MemoryStream(bytesImagen);
            return compressStream;
        }

        private UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }
            UIImage image;
            try
            {
                image = new UIImage(NSData.FromArray(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }
            return image;
        }

        private byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        #endregion

        public void DownDB() { }

        public void DownLoadFileFromOss(string url, string fileName, string localFile)
        {
            string localFilename = fileName;

            fileLocalPath = Path.Combine(GetImagePath(localFile), localFilename);

            if (NSFileManager.DefaultManager.FileExists(fileLocalPath))
            {
                App.GoPreviewImageGesturePage(fileLocalPath);
                return;
            }

            fileLocalPath = Path.Combine(GetImagePath("RMMT_IMAGE"), localFilename);
            if (NSFileManager.DefaultManager.FileExists(fileLocalPath))
            {
                App.GoPreviewImageGesturePage(fileLocalPath);
                return;
            }

            fileLocalPath = Path.Combine(GetImagePath(localFile), localFilename);
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
            HideLoading();
            App.GoPreviewImageGesturePage(fileLocalPath);
            /**
			var previous = UIApplication.CheckForIllegalCrossThreadCalls;
			UIApplication.CheckForIllegalCrossThreadCalls = false;

			FilePickerImplementation filePick = new FilePickerImplementation();
			filePick.OpenFile(fileLocalPath);

			UIApplication.CheckForIllegalCrossThreadCalls = previous;
			***/
        }
        public void DownLoadFileFromOssForReport(string url, string fileName, string localFile)
        {
            string localFilename = fileName;

            fileLocalPath = Path.Combine(GetImagePath(localFile), localFilename);

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
    }
}