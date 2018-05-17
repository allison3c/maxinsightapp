using MaxInsight.Mobile.Module;
using ModernHttpClient;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Aliyun.OSS;

namespace MaxInsight.Mobile.Helpers
{
    public class CommonHelper
    {
        #region Constructor
        static Config.Config _config;
        public string _bucket;
        public string _domain;
        public string _dir;
        public CommonHelper(Config.Config config)
        {
            _config = config;
            _bucket = _config.Get<string>(Config.Config.AliyunOss_Bucket);
            _domain = _config.Get<string>(Config.Config.AliyunOss_Domain);
            _dir = _config.Get<string>(Config.Config.AliyunOss_Dir);
        }
        #endregion

        #region Net work
        /// <summary>
        /// 判断网络是否连接
        /// </summary>
        /// <returns></returns>
        public bool IsNetWorkConnected()
        {
            return CrossConnectivity.Current.IsConnected;
        }

        /// <summary>
        /// 判断网络类型
        /// </summary>
        /// <returns></returns>
        public string GetConnectionTypes()
        {
            string netType = "";
            IEnumerable<ConnectionType> types = CrossConnectivity.Current.ConnectionTypes;

            foreach (var item in types)
            {
                netType += item.ToString() + ";";
            }

            return netType;
        }

        /// <summary>
        /// 获取带宽 数值越小，网络越不稳定
        /// </summary>
        /// <returns></returns>
        public int GetNewWorkBandWiths()
        {
            int band = 0;

            IEnumerable<UInt64> bands = CrossConnectivity.Current.Bandwidths;

            foreach (var item in bands)
            {
                band += Convert.ToInt32(item);
            }

            return band;
        }

        /// <summary>
        /// 判断能否连接服务器
        /// </summary>
        /// <param name="host"></param>
        /// <param name="msTimeout"></param>
        /// <returns></returns>
        public async Task<bool> IsReachable(string host, int msTimeout = 5000)
        {
            return await CrossConnectivity.Current.IsReachable(host, msTimeout);
        }

        /// <summary>
        /// 判断是否能连接服务器上某个服务
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="msTimeout"></param>
        /// <returns></returns>
        public async Task<bool> IsRemoteReachable(string host, int port = 80, int msTimeout = 5000)
        {
            return await CrossConnectivity.Current.IsRemoteReachable(host, port, msTimeout);
        }

        /// <summary>
        /// 判断网络是否发生改变
        /// </summary>
        public bool IsNetworkChanged()
        {
            bool changed = false;
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                changed = args.IsConnected;
            };

            return changed;
        }
        #endregion

        #region HttpGet
        /// <summary>
        /// HTTP GET请求
        /// </summary>
        /// <param name="uri">/api/NewLogin?userId={userId}&password={password}&imei={imei}</param>
        /// <param name="param">传输的参数，以 {key : value}的形式传递</param>
        public async Task<T> HttpGet<T>(string baseUrl, string uri, List<RequestParameter> param) where T : new()
        {
            await CheckRemoteServerConnection(baseUrl);
            var client = CreateHttpClient();

            List<String> paraLists = new List<string>();
            foreach (var item in param)
            {
                paraLists.Add(string.Concat(item.Name, "=", item.Value));
            }
            string endpoint = "";// baseUrl + uri + "?" + string.Join("&", paraLists);

            if (paraLists.Count > 0)
            {
                endpoint = baseUrl + uri + "?" + string.Join("&", paraLists);
            }
            else
            {
                endpoint = baseUrl + uri;
            }

            var response = await client.GetAsync(endpoint, CreateCancelTokenSource().Token).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                //JsonSerializerSettings jsetting = new JsonSerializerSettings();
                //jsetting.NullValueHandling = NullValueHandling.Ignore;
                return JsonConvert.DeserializeObject<T>(content);//此处进行一定操作后会报空引用异常。
            }
            return new T();
        }

        private static HttpClient _httpClient;
        private static HttpClient CreateHttpClient()
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient(new NativeMessageHandler());
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateToken());
            }
            return _httpClient;
        }

        public async Task HttpPutFromRequestParameter(string baseUrl, string uri, List<RequestParameter> param)
        {
            await CheckRemoteServerConnection(baseUrl);
            var client = CreateHttpClient();
            string endpoint = ""; //baseUrl + uri;

            List<String> paraLists = new List<string>();
            foreach (var item in param)
            {
                paraLists.Add(string.Concat(item.Name, "=", item.Value));
            }
            if (paraLists.Count > 0)
            {
                endpoint = baseUrl + uri + "?" + string.Join("&", paraLists);
            }
            else
            {
                endpoint = baseUrl + uri;
            }
            var value = param.Select(p => new KeyValuePair<string, string>(p.Name, p.Value)).ToList();
            var response = await client.PutAsync(endpoint, new FormUrlEncodedContent(value), CreateCancelTokenSource().Token).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                //return JsonConvert.DeserializeObject<T>(content);
            }
        }

        /// <summary>
        /// HTTP GET请求
        /// </summary>
        /// <param name="uri">/api/NewLogin?userId={userId}&password={password}&imei={imei}</param>
        /// <param name="param">传输的参数，以 {key : value}的形式传递</param>
        public async Task<T> HttpGetPOST<T>(string baseUrl, string uri, List<RequestParameter> param) where T : new()
        {
            await CheckRemoteServerConnection(baseUrl);
            var client = CreateHttpClient();
            string endpoint = baseUrl + uri;
            var value = param.Select(p => new KeyValuePair<string, string>(p.Name, p.Value)).ToList();
            var response = await client.PostAsync(endpoint, new FormUrlEncodedContent(value), CreateCancelTokenSource().Token).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            return new T();

        }

        public async Task<T> HttpPut<T>(string baseUrl, string uri, object obj)
        {
            await CheckRemoteServerConnection(baseUrl);
            HttpClient httpClient = CreateHttpClient();
            string endpoint = baseUrl + uri;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, endpoint);
            request.Content = new System.Net.Http.StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8,
                                    "application/json");
            var response = await httpClient.SendAsync(request, CreateCancelTokenSource().Token).ConfigureAwait(false);


            //var response = await httpClient.PostAsync (endpoint, formContent);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            return default(T);
        }

        public async Task<T> HttpGetPOSToCommodity<T>(string baseUrl, string uri, object obj) where T : new()
        {
            await CheckRemoteServerConnection(baseUrl);

            HttpClient httpClient = CreateHttpClient();
            string endpoint = baseUrl + uri;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Content = new System.Net.Http.StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8,
                                    "application/json");
            var response = await httpClient.SendAsync(request, CreateCancelTokenSource().Token).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            return new T();

        }

        private async Task CheckRemoteServerConnection(string baseUrl)
        {
            try
            {
                string host = baseUrl.Substring(0, baseUrl.LastIndexOf(":"));
                string port = baseUrl.Substring(baseUrl.LastIndexOf(":") + 1);
                if (await IsRemoteReachable(baseUrl, Convert.ToInt32(port)) == false)
                {
                    throw new OperationCanceledException();
                };
            }
            catch (Exception)
            {
            }
        }

        private static CancellationTokenSource CreateCancelTokenSource(int cancelTime = 0)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(cancelTime == 0 ? 10000 : cancelTime);
            return cts;
        }

        public async Task<T> HttpPOST<T>(string baseUrl, string uri, object obj) where T : new()
        {
            var client = CreateHttpClient();
            var cts = new CancellationTokenSource();
            cts.CancelAfter(200000);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, baseUrl + uri);
            request.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8,
                                    "application/json");
            var response = await client.SendAsync(request, cts.Token);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            return new T();
        }

        ///// <summary>
        ///// HTTP GET FOR LIST
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="baseUrl"></param>
        ///// <param name="uri"></param>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //public async Task<List<T>> HttpGetPOSTForList<T>(string baseUrl, string uri, List<RequestParameter> param) where T : new()
        //{
        //    try
        //    {
        //        var client = new RestClient(baseUrl);// !string.IsNullOrEmpty(baseUrl) ? baseUrl : "https://www.pgyer.com");
        //        client.Timeout = new TimeSpan(0, 0, 30);
        //        var request = CreateAuthorizationRequest(uri, Method.POST);
        //        foreach (var item in param)
        //        {
        //            request.AddParameter(item.Name, item.Value);
        //        }
        //        var respone = await client.Execute<List<T>>(request);
        //        return respone.Data;
        //    }
        //    catch (Exception)
        //    {
        //        return new List<T>();
        //    }
        //}

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<string> HttpGetString(string baseUrl, string uri, List<RequestParameter> list)
        {
            await CheckRemoteServerConnection(baseUrl);
            var client = CreateHttpClient();
            List<String> paraLists = new List<string>();
            foreach (var item in list)
            {
                paraLists.Add(string.Concat(item.Name, "=", item.Value));
            }
            string endpoint = "";//baseUrl + uri + "?" + string.Join("&", paraLists);

            if (paraLists.Count > 0)
            {
                endpoint = baseUrl + uri + "?" + string.Join("&", paraLists);
            }
            else
            {
                endpoint = baseUrl + uri;
            }

            var response = await client.GetAsync(endpoint, CreateCancelTokenSource().Token).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return string.Empty;

        }
        #endregion

        #region HttpPost
        /// <summary>
        /// HTTP POST请求
        /// </summary>
        /// <typeparam name="T">传输的Domain</typeparam>
        /// <param name="uri">api地址，不包括url</param>
        /// <param name="list">数据</param>
        /// <returns></returns>
        public async Task<string> HttpPost<T>(string baseUrl, string uri, T obj)
        {
            await CheckRemoteServerConnection(baseUrl);
            if (obj == null)
            {
                return string.Empty;
            }

            var client = CreateHttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, baseUrl + uri);
            request.Content = new System.Net.Http.StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8,
                                    "application/json");
            var response = await client.SendAsync(request, CreateCancelTokenSource().Token).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return "OK";
            }
            return string.Empty;
        }

        #endregion

        #region http patch
        /// <summary>
        /// HTTP Patch请求
        /// </summary>
        /// <param name="uri">/api/NewLogin?userId={userId}&password={password}&imei={imei}</param>
        /// <param name="param">传输的参数，以 {key : value}的形式传递</param>
        public async Task<string> HttpPatch(string baseUrl, string uri, object obj)
        {
            var client = CreateHttpClient();
            client.Timeout = new TimeSpan(0, 0, 30);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), baseUrl + uri);
            request.Content = new System.Net.Http.StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8,
                                    "application/json");
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return "OK";
            }
            return "Faile";
        }
        #endregion

        #region post file to serve
        public async Task<T> UploadFile<T>(string baseUrl, string uri, Stream stream, string path)
        {
            if (null == stream)
            {
                return default(T);
            }

            var content = new MultipartFormDataContent();

            content.Add(new StreamContent(stream),
                "\"file\"",
                $"\"{path}\"");
            var httpClient = CreateHttpClient();// new HttpClient(new NativeMessageHandler());
            httpClient.Timeout = new TimeSpan(0, 1, 30);
            var uploadServiceBaseAddress = baseUrl + uri;
            var httpResponseMessage = await httpClient.PostAsync(uploadServiceBaseAddress, content);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentee = await httpResponseMessage.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<T>(contentee.ToString());

                return result;
            }

            return default(T);
        }
        #endregion

        #region post file to oss
        private static OssClient _ossClient;
        public static OssClient CreateOssClient()
        {
            if (_ossClient == null)
            {
                //string accessKeyId = "LTAIf3JaskXqG1bx";
                //string accessKeySecret = "EauvHZ7OXKtmxhDi9RN7YO8MrQo2YP";
                //string endpoint = "oss-cn-shanghai.aliyuncs.com";
                string accessKeyId = _config.Get<string>(Config.Config.AliyunOss_AccessKeyId);
                string accessKeySecret = _config.Get<string>(Config.Config.AliyunOss_AccessKeySecret);
                string endpoint = _config.Get<string>(Config.Config.AliyunOss_Endpoint);
                //var conf = new ClientConfiguration();
                ////conf.IsCname = true;/// 配置使用Cname
                ////conf.ConnectionLimit = 512;  //HttpWebRequest最大的并发连接数目
                //conf.MaxErrorRetry = 3;     //设置请求发生错误时最大的重试次数
                //conf.ConnectionTimeout = 300;  //设置连接超时时间
                ////conf.SetCustomEpochTicks(customEpochTicks);        //设置自定义基准时间, CreateCancelTokenSource().Token.ToString()

                _ossClient = new OssClient(endpoint, accessKeyId, accessKeySecret);
            }
            return _ossClient;
        }

        public async Task<APIResult> UploadFileToOSS(Stream stream, string filename, string extraParam)
        {
            if (null == stream)
            {
                return new APIResult { Body = "", ResultCode = ResultType.Success, Msg = "" };
            }
            try
            {
                //Android上传文件突然报Aliyun.OSS 空引用的错误，所以修改了Android文件上传方法。
                //if (App.SysOS == "Android")
                //{

                //    if (_ossClient == null)
                //          CreateOssClient();
                //    string key = string.Empty;
                //    if (extraParam.ToUpper() == "L")
                //    {
                //        key = _dir + filename;
                //    }
                //    else
                //    {
                //        key = _dir + DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + filename.Substring(filename.LastIndexOf("."));
                //    }
                //    //_ossClient.PutObject("vgic", key, path);
                //    stream.Position = 0;
                //    var result = await Task<PutObjectResult>.Factory.FromAsync(
                //                _ossClient.BeginPutObject,
                //                _ossClient.EndPutObject,
                //                _bucket, key, stream,
                //                null);

                //    //var tcs = new TaskCompletionSource<bool>();
                //    //// 创建上传Object的Metadata
                //    //ObjectMetadata meta = new ObjectMetadata();
                //    //// 上传Object.
                //    //PutObjectResult result = _ossClient.PutObject(_bucket, key, stream, meta);

                //    AttachDto attachDto = new AttachDto();
                //    attachDto.AttachName = filename;
                //    attachDto.Url = _domain +key;
                //    return new APIResult { Body = CommonHelper.EncodeDto<AttachDto>(attachDto), ResultCode = ResultType.Success, Msg = "" };
                //}
                //else
                //{
                HttpClient client = new HttpClient();
                byte[] bytes = new byte[(int)stream.Length];
                stream.Read(bytes, 0, bytes.Length);

                client.BaseAddress = new Uri(_config.Get<string>(Config.Config.Endpoints_UploadOSSBaseUrl));

                StringContent sc = new StringContent(JsonConvert.SerializeObject(new
                {
                    Bytes = bytes,
                    Filename = filename,
                    ExtraParam = extraParam
                }), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("Excel/UploadToOss", sc);

                if (response.IsSuccessStatusCode)
                {
                    AttachDto attachDto = new AttachDto();
                    attachDto.AttachName = filename;
                    attachDto.Url = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result.ToString()).ToString();
                    return new APIResult { Body = CommonHelper.EncodeDto<AttachDto>(attachDto), ResultCode = ResultType.Success, Msg = "" };
                }
                else
                {
                    return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = "" };
                }
                //}
            }
            catch (Exception e)
            {
                return new APIResult { Body = "", ResultCode = ResultType.Failure, Msg = e.StackTrace + "====" + e.ToString() };
            }
        }
        public string GetOssFileUrl(string fileName)
        {
            CreateOssClient();
            Uri uri = _ossClient.GeneratePresignedUri(_bucket, fileName, DateTime.Now.AddDays(1.0));
            return uri.AbsoluteUri;
        }
        #endregion

        public static string EncodeDto<T>(T t)
        {
            string jsonString = string.Empty;

            try
            {
                jsonString = JsonConvert.SerializeObject(t);
            }
            catch (Exception)
            {
            }
            return jsonString;
        }

        public static T DecodeString<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        #region token
        private static string GenerateToken()
        {
            var now = DateTime.UtcNow;
            var payload = new Dictionary<string, object>(){
                { "jti", Guid.NewGuid().ToString() },
                { "iat", ToUnixEpochDate(now).ToString()}};
            var secretKey = "toyotasupersecret_secretkey!123";
            string token = JWT.JsonWebToken.Encode(payload, secretKey, JWT.JwtHashAlgorithm.HS256);
            return token;
        }
        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        #endregion
    }
}
