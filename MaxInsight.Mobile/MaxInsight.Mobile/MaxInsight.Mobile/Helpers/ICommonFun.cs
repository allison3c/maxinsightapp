using MaxInsight.Mobile.Module;
using System.IO;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Helpers
{
    public interface ICommonFun
    {
        /// <summary>
        /// show loading
        /// userdialog
        /// https://github.com/aritchie/userdialogs/blob/master/src/Samples/Samples/MainPage.cs
        /// </summary>
        void ShowLoading(string msg);

        /// <summary>
        /// hide loading
        /// userdialog
        /// https://github.com/aritchie/userdialogs/blob/master/src/Samples/Samples/MainPage.cs
        /// </summary>
        void HideLoading();

        /// <summary>
        /// alert
        /// https://github.com/aritchie/userdialogs/blob/master/src/Samples/Samples/MainPage.cs
        /// </summary>
        void Alert(string msg, string title);

        /// <summary>
        /// alert
        /// https://github.com/aritchie/userdialogs/blob/master/src/Samples/Samples/MainPage.cs
        /// </summary>
        /// <param name="msg"></param>
        void AlertLongText(string msg);

        /// <summary>
        /// alert
        /// https://github.com/aritchie/userdialogs/blob/master/src/Samples/Samples/MainPage.cs
        /// </summary>
        /// <param name="msg"></param>
        void ShowToast(string msg);

        /// <summary>
        /// 输入文本框
        /// </summary>
        /// <returns></returns>
        Task<string> ShowPromt(string title);

        /// <summary>
        /// 确认框
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task<bool> Confirm(string msg);

        Task<bool> NoCancelConfirm(string msg);

        Task<string> ShowActionSheetAny(params string[] buttons);

        Task<string> ShowActionSheet(string action1, string action2, string actions = "");
        Stream GetFileStream(byte[] imageData, int width, int height);

        void SetCach(string key, string value);

        string GetCach(string key);

        void DeleteCach(string key);

        //Stream GetFileStream(string filePath);

        void JPushSetAlias(string aliasName);

        void ExistSystem();

        #region DownLoadFile
        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="url">下载文件的URL地址</param>
        /// <param name="exterName">保存到本地的文件扩展名</param>
        /// <returns></returns>
        void DownLoadFile(string url, string exterName);
        void DownLoadFileFromOss(string url, string fileName, string localFile);
        #endregion

        int GetVersionNo();

        Task<APIResult> UploadFileToOSS(Stream stream, string path);
        string SaveAttachLocal(Stream stream, string filename);
        Task UploadLocalFileToServer();
        string GetFilesSizeOfUpload();
        Stream ResizeImage(Stream stream);
        Stream GetAttachLocal(string path);
        string GetTempPathForApiToOss(string localpath, string temppath, string localfilename);
        void DeleteFileForApiToOss(string tempPath);
        void DownDB();
        void DownLoadFileFromOssForReport(string url, string fileName, string localFile);
    }
}
