using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using XLabs.Ioc;
using MaxInsight.Mobile.Module.Dto.Shops;
using System.Text.RegularExpressions;

namespace MaxInsight.Mobile.Services.Tour
{
    public interface ITourService
    {
        Task<APIResult> GetShops(int id);
        Task<APIResult> GetPlans(int disId, string startTime = "", string endTime = "", string statu = "", string sourceTypeCode = "D");
        Task<APIResult> CheckPlan(string taskId, string operation = "S");
        Task<APIResult> SaveSystemList(ItemOfTaskDto dto);
        Task<APIResult> UploadFile(Stream stream, string path);
        Task<APIResult> UploadFiletoOss(Stream stream, string path, string localpath);
        Task<APIResult> SaveRegistCore(ScoreCheckResultParam scrorDto);
        Task<APIResult> GetCustomizedTaskByTaskId(string taskId, string operation);
        Task<APIResult> CustomizedTaskCheck(CustomizedTaskDto dto);
    }

    public class TourService : ITourService
    {
        CommonHelper _commonHelper;
        ICommonFun _commonFun;
        Config.Config _config;
        string baseUrl = "";
        public TourService(Config.Config config)
        {
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _commonFun = Resolver.Resolve<ICommonFun>();
            _config = config;
            baseUrl = _config.Get<string>(Config.Config.Endpoints_BaseUrl);
        }

        public async Task<APIResult> GetShops(int id)
        {
            List<RequestParameter> param = new List<RequestParameter>() {
                new RequestParameter { Name = "userId", Value = id.ToString() }
            };

            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.Shops"), param);
            return info;
        }

        public async Task<APIResult> GetPlans(int disId, string startTime = "", string endTime = "", string statu = "", string sourceTypeCode = "D")
        {
            List<RequestParameter> param = new List<RequestParameter>() {
                new RequestParameter { Name = "disId", Value = disId.ToString() },
                new RequestParameter { Name = "startTime", Value = startTime},
                new RequestParameter { Name = "endTime", Value = endTime},
                new RequestParameter { Name = "status", Value = statu},
                new RequestParameter { Name = "sourceType", Value = sourceTypeCode},
                new RequestParameter { Name = "InUserId", Value =CommonContext.Account.UserId}
            };

            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.Plans"), param);
            return info;
        }

        public async Task<APIResult> CheckPlan(string taskId, string operation = "S")
        {
            List<RequestParameter> param = new List<RequestParameter>() {
                new RequestParameter { Name = "taskId", Value = taskId.ToString() },
                new RequestParameter { Name = "operation", Value = operation }
            };

            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.CheckPlan"), param);
            return info;
        }

        public async Task<APIResult> SaveSystemList(ItemOfTaskDto dto)
        {
            //ItemApproalParams param = new ItemApproalParams()
            //{
            //	TPId = dto.TPId,
            //	TIId = dto.TIId,
            //	Score = dto.Score,
            //	PlanApproalYN = dto.PlanApproalYN,
            //	ResultApproalYN = dto.ResultApproalYN,
            //	UserId = CommonContext.Account.UserId
            //};

            var param = new
            {
                TPId = dto.TPId,
                TIId = dto.TIId,
                Score = dto.Score,
                PlanApproalYN = dto.PlanApproalYN,
                ResultApproalYN = dto.ResultApproalYN,
                UserId = CommonContext.Account.UserId
            };

            var info = await _commonHelper.HttpPOST<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.SaveSystem"), param);
            return info;
        }

        public async Task<APIResult> UploadFile(Stream stream, string path)
        {
            var info = await _commonHelper.UploadFile<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.UploadFile"),
                                                                 stream, path);
            return info;
        }
        public async Task<APIResult> UploadFiletoOss(Stream stream, string filename, string localpath)
        {
            APIResult info;
            Regex regImg = new Regex(".+(.JPEG|.jpeg|.JPG|.jpg|.GIF|.gif|.BMP|.bmp|.PNG|.png)$");

            if (regImg.IsMatch(filename))
            {
                if (App.SysOS == "Android")
                {
                    info = await _commonHelper.UploadFileToOSS(_commonFun.ResizeImage(stream), filename, "");
                }
                else
                {
                    string newPath = _commonFun.GetTempPathForApiToOss(localpath, "RMMTIMAGEVIEW", filename);
                    Stream newStream = _commonFun.GetAttachLocal(newPath);
                    info = await _commonHelper.UploadFileToOSS(newStream, filename, "");
                    _commonFun.DeleteFileForApiToOss(newPath);
                }
            }
            else
            {
                info = await _commonHelper.UploadFileToOSS(stream, filename, "");
            }

            return info;
        }

        public async Task<APIResult> SaveRegistCore(ScoreCheckResultParam scrorDto)
        {
            var info = await _commonHelper.HttpPOST<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.SaveScore"), scrorDto);

            return info;
        }

        public async Task<APIResult> GetCustomizedTaskByTaskId(string taskId, string operation)
        {
            List<RequestParameter> param = new List<RequestParameter>() {
                new RequestParameter { Name = "taskId", Value = taskId.ToString() },
                new RequestParameter { Name = "operation", Value = operation }
            };

            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.GetCustomizedTaskByTaskId"), param);
            return info;
        }

        public async Task<APIResult> CustomizedTaskCheck(CustomizedTaskDto dto)
        {
            var info = await _commonHelper.HttpPOST<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.CustomizedTaskCheck"), dto);

            return info;
        }
    }
}
