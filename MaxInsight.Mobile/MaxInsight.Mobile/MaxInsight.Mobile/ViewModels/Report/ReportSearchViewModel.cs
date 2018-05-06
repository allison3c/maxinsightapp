using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module.Dto.Case;
using MaxInsight.Mobile.Services.CaseService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.Report
{
    public class ReportSearchViewModel : ViewModel
    {
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        ICaseService _caseService;
        private Dictionary<string, string> _dIcSourceType = new Dictionary<string, string>();
        public ReportSearchViewModel()
        {
            _commonFun = Resolver.Resolve<ICommonFun>();
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _caseService = Resolver.Resolve<ICaseService>();
            _dIcSourceType.Add("0", "全部");

            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await _caseService.GetTypeFromHiddenCode("15");
                if (null != result && result.ResultCode == Module.ResultType.Success)
                {
                    var caseTypeList = JsonConvert.DeserializeObject<List<NameValueObject>>(result.Body);

                    if (caseTypeList != null && caseTypeList.Count > 0)
                    {
                        foreach (var item in caseTypeList)
                        {
                            _dIcSourceType.Add(item.Value, item.Name);
                        }
                    }
                    else
                    {

                    }
                }

            });
        }

        #region Property
        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }
        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set { SetProperty(ref _endDate, value); }
        }
        public string _sourceType;
        public string SourceType
        {
            get
            {
                return _sourceType == null || _sourceType == "" ? "全部" : _sourceType;
            }
            set
            {
                SetProperty(ref _sourceType, value);
            }
        }

        public string _sourceTypeCode;
        public string SourceTypeCode
        {
            get
            {
                return _sourceTypeCode == null || _sourceTypeCode == "" ? "0" : _sourceTypeCode;
            }
            set
            {
                SetProperty(ref _sourceTypeCode, value);
            }
        }
        #endregion

        #region

        public Command OpenSourceTypeCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        var action = await _commonFun.ShowActionSheetAny(_dIcSourceType.Values.ToArray<string>());
                        if (_dIcSourceType.ContainsValue(action))
                        {
                            SourceType = action;
                            SourceTypeCode = _dIcSourceType.FirstOrDefault(q => q.Value == action).Key;
                        }
                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }
        public Command SearchReportListCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        Dictionary<string, string> param = new Dictionary<string, string>();
                        param.Add("UserId", CommonContext.Account.UserId);
                        param.Add("StartDate", StartDate.ToString("yyyy-MM-dd"));
                        param.Add("EndDate", EndDate.ToString("yyyy-MM-dd"));
                        param.Add("SourceTypeCode", SourceTypeCode);
                        param.Add("SourceType", SourceType);
                        if (StartDate > EndDate)
                        {
                            _commonFun.AlertLongText("结束日期不能小于开始日期");
                            return;
                        }
                        await Navigation.PopAsync(true);
                        MessagingCenter.Send<Dictionary<string, string>>(param, "ReportSearchResult");
                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }
        #endregion
        #region method
        public void Init()
        {
            DateTime now = DateTime.Now;
            DateTime d1 = new DateTime(now.Year, now.Month, 1);
            StartDate = d1;
            EndDate = now;
            SourceType = "全部";
            SourceTypeCode = "0";

        }

        #endregion
    }
}
