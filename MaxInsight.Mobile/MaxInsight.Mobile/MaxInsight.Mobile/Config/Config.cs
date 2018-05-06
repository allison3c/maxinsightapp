using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace MaxInsight.Mobile.Config
{
    public class Config
    {
        public const string Version = "Version";
        public const string AppId = "AppId";
        public const string AppName = "AppName";
        public const string AppMode = "AppMode";
        public const string SessionLifetimeMinute = "SessionLifetimeMinute";
        public const string Endpoints = "Endpoints";
        public const string Endpoints_BaseUrl = "Endpoints.BaseUrl";
        public const string Endpoints_UploadOSSBaseUrl = "Endpoints.UploadOSSBaseUrl";
        public const string Endpoints_Paths = "Endpoints.Paths";
        public const string AliyunOss_AccessKeyId = "AliyunOss.AccessKeyId";
        public const string AliyunOss_AccessKeySecret = "AliyunOss.AccessKeySecret";
        public const string AliyunOss_Endpoint = "AliyunOss.Endpoint";
        public const string AliyunOss_Bucket = "AliyunOss.Bucket";
        public const string AliyunOss_Domain = "AliyunOss.Domain";
        public const string AliyunOss_Dir = "AliyunOss.Dir";

        private readonly JObject _jobj;

        public Config(string environmentName = null)
        {
            var assembly = typeof(Config).GetTypeInfo().Assembly;

            Stream stream = assembly.GetManifestResourceStream("MaxInsight.Mobile.Config.config.base.json");
            using (var reader = new System.IO.StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                _jobj = JObject.Parse(json);
            }

            if (string.IsNullOrWhiteSpace(environmentName) == false)
            {
                stream = assembly.GetManifestResourceStream($"MaxInsight.Mobile.Config.config.{environmentName}.json");
                using (var reader = new System.IO.StreamReader(stream))
                {
                    string json = reader.ReadToEnd();

                    JObject jobj = JObject.Parse(json);
                    _jobj.Merge(jobj);
                }
            }
        }

        public T Get<T>(string key)
        {
            return _jobj.SelectToken(key, true).Value<T>();
        }
    }
}
