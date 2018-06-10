using System;
using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Services.Tour;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using MaxInsight.Mobile.Services.CaseService;
using MaxInsight.Mobile.Module.Dto.Case;

namespace MaxInsight.Mobile
{
    public class ViewRecordViewModel : ViewModel
    {
        private readonly ICommonFun _commonFun;
        private readonly ITourService _tourService;
        ICaseService _caseService;
        private Dictionary<int, string> _dIcDistributor = new Dictionary<int, string>();
        private Dictionary<string, string> _dIcSourceType = new Dictionary<string, string>();

        public ViewRecordViewModel()
        {
            try
            {
                _commonFun = Resolver.Resolve<ICommonFun>();
                _tourService = Resolver.Resolve<ITourService>();
                _caseService = Resolver.Resolve<ICaseService>();

                DistributorName = "请选择";
                SourceTypeName = "全部";
                // 页面初始化，清空
                MessagingCenter.Subscribe<string>(this, "InitParameter", (obj) =>
                {
                    DateTime now = DateTime.Now;
                    DateTime d1 = new DateTime(now.Year, now.Month, 1);
                    StartDate = d1;
                    EndDate = now;
                    DistributorName = "请选择";
                    DisId = 0;
                    SourceTypeName = "全部";
                    SourceTypeCode = "0";
                });

                #region useless
                //Device.BeginInvokeOnMainThread(async () =>
                //{
                //    try
                //    {
                //        if (!(CommonContext.Account.UserType == "D" || CommonContext.Account.UserType == "S"))
                //        {
                //            var result = await _tourService.GetShops(Convert.ToInt32(CommonContext.Account.UserId));
                //            if (null != result && result.ResultCode == Module.ResultType.Success)
                //            {
                //                List<TourDistributorDto> disList = JsonConvert.DeserializeObject<List<TourDistributorDto>>(result.Body);
                //                if (disList != null && disList.Count > 0)
                //                {
                //                    foreach (var item in disList)
                //                    {
                //                        _dIcDistributor.Add(item.DisId, item.DisName);
                //                    }
                //                }
                //                else
                //                {

                //                }
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //    }
                //});
                #endregion

                var zionId = CommonContext.Account.OrgZionId;
                var areaId = CommonContext.Account.OrgAreaId;
                var serverLst = CommonContext.Account.ZionList.Find(z => z.QId == zionId).AreaList.Find(a => a.AId == areaId).ServerList;

                foreach (var item in serverLst)
                {
                    _dIcDistributor.Add(Int32.Parse(item.SId), item.SName);
                }

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
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ViewRecordViewModel");
                return;
            }
        }

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

        private string _distributorName;
        public string DistributorName
        {
            get { return _distributorName; }
            set { SetProperty(ref _distributorName, value); }
        }
        private int _disId;
        public int DisId
        {
            get { return _disId; }
            set { SetProperty(ref _disId, value); }
        }
        private string _sourceTypeName;
        public string SourceTypeName
        {
            get { return _sourceTypeName; }
            set { SetProperty(ref _sourceTypeName, value); }
        }
        private string _sourceTypeCode;
        public string SourceTypeCode
        {
            get { return _sourceTypeCode; }
            set { SetProperty(ref _sourceTypeCode, value); }
        }

        #region Command
        private RelayCommand _search;
        public RelayCommand Search
        {
            get { return _search ?? (_search = new RelayCommand(SearchTask)); }
        }

        public Command OpenDistributorListCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        var action = await _commonFun.ShowActionSheetAny(_dIcDistributor.Values.ToArray<string>());
                        if (_dIcDistributor.ContainsValue(action))
                        {
                            DistributorName = action;
                            DisId = _dIcDistributor.FirstOrDefault(q => q.Value == action).Key;
                        }
                    }
                    catch (Exception)
                    {
                    }
                });
            }


        }

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
                            SourceTypeName = action;
                            SourceTypeCode = _dIcSourceType.FirstOrDefault(q => q.Value == action).Key;
                        }
                    }
                    catch (Exception)
                    {
                    }
                });
            }


        }

        #endregion

        private void SearchTask()
        {
            try
            {
                int disId = 0;
                if (CommonContext.Account.UserType == "D" || CommonContext.Account.UserType == "S")
                {
                    int.TryParse(CommonContext.Account.OrgServerId, out disId);
                }
                else
                {
                    disId = DisId;
                }
                if (disId == 0)
                {
                    _commonFun.AlertLongText("请选择经销商");
                    return;
                }
                else if (StartDate > EndDate)
                {
                    _commonFun.AlertLongText("开始日期不能大于结束日期");
                    return;
                }
                Device.BeginInvokeOnMainThread(async () =>
                {

                    await Navigation.PushAsync<TaskListViewModel>(
                        (vm, v) => vm.Init(disId, StartDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd"), false, SourceTypeCode), true);

                });
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ViewRecordViewModel");
                return;
            }
        }
    }
}
