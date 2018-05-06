using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto.Calender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLabs.Ioc;

namespace MaxInsight.Mobile.Services.CalendarService
{
    public interface ICalendarService
    {
        Task<APIResult> GetCalenderListAll(string UserId, string Date);
        Task<APIResult> CreateCalenderPlans(SaveCalenderMngParams dto);
        Task<APIResult> DeleteCalenderPlans(string Id);

    }
    public class CalendarService : ICalendarService
    {
        CommonHelper _commonHelper;
        Config.Config _config;
        string baseUrl = "";
        public CalendarService(Config.Config config)
        {
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _config = config;
            baseUrl = _config.Get<string>(Config.Config.Endpoints_BaseUrl);
        }
        public async Task<APIResult> GetCalenderListAll(string UserId, string Date)
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter { Name = "UserId", Value = UserId.ToString() },
                new RequestParameter {Name = "Date",Value = Date },
            };
            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.GetCalenderListAll"), param);
            return info;
        }

        public async Task<APIResult> CreateCalenderPlans(SaveCalenderMngParams dto)
        {
            var info = await _commonHelper.HttpPOST<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.CreateCalenderPlans"), dto);
            return info;
        }

        public async Task<APIResult> DeleteCalenderPlans(string Id)
        {
            var info = await _commonHelper.HttpPOST<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.DeleteCalenderPlans"), Id);
            return info;
        }
    }
}
