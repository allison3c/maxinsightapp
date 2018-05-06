namespace MaxInsight.Mobile.Module
{
    public class PgyAppInfoShortcutInfo
    {
        public string code { get; set; }
        public string message { get; set; }
        public ShortcutInfo data { get; set; }
    }
    public class ShortcutInfo
    {
        public string appKey { get; set; }
        public string appType { get; set; }
        public string appFileName { get; set; }
        public string appFileSize { get; set; }
        public string appName { get; set; }
        public string appVersion { get; set; }
        public string appVersionNo { get; set; }
        public string appBuildVersion { get; set; }
        public string appIdentifier { get; set; }
        public string appCreated { get; set; }
    }
    public class PgyAppInfoDetail
    {
        public string code { get; set; }
        public string message { get; set; }
        public InfoDetail data { get; set; }
    }
    public class InfoDetail
    {
        public string appKey { get; set; } //App Key
        public string userKey { get; set; } //User Key
        public int appType { get; set; } //应用类型（1:iOS; 2:Android）
        public int appIsFirst { get; set; } //是否是第一个App（1:是; 2:否）
        public int appIsLastest { get; set; } //是否是最新版（1:是; 2:否）
        public string appFileSize { get; set; } //App 文件大小
        public string appName { get; set; } //App 名称
        public string appVersion { get; set; } //版本号
        public string appVersionNo { get; set; } //适用于Android的版本编号，iOS始终为0
        public string appBuildVersion { get; set; } //蒲公英生成的用于区分历史版本的build号
        public string appIdentifier { get; set; } //应用程序包名，iOS为BundleId，Android为包名
        public string appIcon { get; set; } //应用的Icon图标key，访问地址为 http://o1wh05aeh.qnssl.com/image/view/app_icons/[应用的Icon图标key]
        public string appDescription { get; set; } //应用介绍
        public string appUpdateDescription { get; set; } //应用更新说明
        public string appScreenShots { get; set; } //应用截图的key，获取地址为 http://o1whyeemo.qnssl.com/image/view/app_screenshots/[应用截图的key]
        public string appShortcutUrl { get; set; } //应用短链接
        public string appQRCodeURL { get; set; } //应用二维码地址
        public string appCreated { get; set; } //应用上传时间
        public string appUpdated { get; set; } //应用更新时间
        public string appFollowed { get; set; }
    }
}
