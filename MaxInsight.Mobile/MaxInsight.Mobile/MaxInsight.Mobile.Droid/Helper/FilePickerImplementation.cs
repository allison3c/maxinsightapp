using System;
using Android.Content;
using System.Threading.Tasks;
using System.Threading;

using Plugin.FilePicker.Abstractions;
using Android.App;
using Java.IO;

namespace Plugin.FilePicker
{
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    /// 
    [Android.Runtime.Preserve(AllMembers = true)]
	public class FilePickerImplementation : IFilePicker
	{
		private Context context;
		private int requestId;
		private TaskCompletionSource<FileData> completionSource;

		public FilePickerImplementation()
		{
			this.context = Android.App.Application.Context;
		}

		public async Task<FileData> PickFile()
		{
			//var media = await TakeMediaAsync("file/*", Intent.ActionGetContent);三星不能获取到它的其他文件
            var media = await TakeMediaAsync("*/*", Intent.ActionGetContent);

            return media;
		}

		private Task<FileData> TakeMediaAsync(string type, string action)
		{
			int id = GetRequestId();

			var ntcs = new TaskCompletionSource<FileData>(id);
			if (Interlocked.CompareExchange(ref this.completionSource, ntcs, null) != null)
				throw new InvalidOperationException("Only one operation can be active at a time");

			try
			{
				Intent pickerIntent = new Intent(this.context, typeof(FilePickerActivity));
				pickerIntent.SetFlags(ActivityFlags.NewTask);

				this.context.StartActivity(pickerIntent);

				EventHandler<FilePickerEventArgs> handler = null;
				EventHandler<EventArgs> cancelledHandler = null;

				handler = (s, e) =>
				{
					var tcs = Interlocked.Exchange(ref this.completionSource, null);

					FilePickerActivity.FilePicked -= handler;

					tcs.SetResult(new FileData()
					{
						DataArray = e.FileByte,
						FileName = e.FileName
					});
				};

				cancelledHandler = (s, e) =>
				{
					var tcs = Interlocked.Exchange(ref this.completionSource, null);
                    if(tcs!=null)
					tcs.SetResult(null);
					FilePickerActivity.FilePickCancelled -= cancelledHandler;
				};

				FilePickerActivity.FilePickCancelled += cancelledHandler;
				FilePickerActivity.FilePicked += handler;
			}
			catch (Exception exAct)
			{
				System.Diagnostics.Debug.Write(exAct);
			}

			return completionSource.Task;
		}

		private int GetRequestId()
		{
			int id = this.requestId;
			if (this.requestId == Int32.MaxValue)
				this.requestId = 0;
			else
				this.requestId++;

			return id;
		}
        public bool IsExistFile(string fileName,string viewLocation)
        {
            File file = new File(new File(Android.OS.Environment.GetExternalStoragePublicDirectory(""), Application.Context.PackageName), "cach");
            File newFile = new File(file, viewLocation);
            File myFile = new File(newFile, fileName);
            if (!myFile.Exists()) return false;
            return true;
        }
        public Task<bool> SaveFile(FileData fileToSave) { throw new NotImplementedException(); }
        public bool SaveFile(byte[] fileData, string path)
		{
            FileOutputStream fos = null;
            try
            {
                fos = new FileOutputStream(new File(path));
                fos.Write(fileData, 0, fileData.Length);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                fos.Close();
            }
            return true;
        }

        public void OpenFile(string fileToOpen)
        {

        }
        public void OpenFile(File fileToOpen)
		{
            var uri = Android.Net.Uri.FromFile(fileToOpen);
            var intent = new Intent();
			var mime = IOUtil.GetMimeType(uri.ToString());
			intent.SetAction(Intent.ActionView);
			intent.SetDataAndType(uri, mime);
			intent.SetFlags(ActivityFlags.NewTask);

			context.StartActivity(intent);
		}
        private string GetImagePath(string newpath)
        {
            Java.IO.File file = new Java.IO.File(new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(""), Application.Context.PackageName), "cach");
            Java.IO.File newFile = new Java.IO.File(file, newpath);

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
        //图片已缓存到本地，直接传图片名字，本地缓存在RMMT_IMAGE中找，服务器缓存在RMMT_IMAGE_VIEW中找
        public void OpenFile(string fileName,string viewLocation)
		{
            File file = new File(new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(""), Application.Context.PackageName), "cach");
            File newFile = new File(file, viewLocation);
            File myFile = new File(newFile, fileName);
            OpenFile(myFile);
        }

        //服务器获取图片流，保存到本地文件夹RMMT_IMAGE_VIEW中，再预览
        public void OpenFile(FileData fileToOpen)
		{
            string documentsPath = GetImagePath("RMMT_IMAGE_VIEW");
            string filepath = System.IO.Path.Combine(documentsPath, fileToOpen.FileName);
            File myFile = new File(filepath);

            if (!myFile.Exists())
                SaveFile(fileToOpen.DataArray, filepath);

            OpenFile(myFile);
		}
    }
}
