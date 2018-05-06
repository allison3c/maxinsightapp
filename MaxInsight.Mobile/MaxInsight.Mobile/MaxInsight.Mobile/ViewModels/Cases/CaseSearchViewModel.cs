using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto.Case;
using MaxInsight.Mobile.Services.CaseService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.Cases
{

    public class CaseSearchViewModel : ViewModel
    {
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        ICaseService _caseService;

        private Dictionary<string, string> _dIcCaseType = new Dictionary<string, string>();
        public RelayCommand<CasesListDto> ItemTappedCommand { get; set; }
        public CaseSearchViewModel()
        {
            _commonFun = Resolver.Resolve<ICommonFun>();
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _caseService = Resolver.Resolve<ICaseService>();
            ItemTappedCommand = new RelayCommand<CasesListDto>(TappedCommand);

            _dIcCaseType.Add("0", "全部");
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await _caseService.GetTypeFromHiddenCode("12");
                if (null != result && result.ResultCode == Module.ResultType.Success)
                {
                    var caseTypeList = JsonConvert.DeserializeObject<List<NameValueObject>>(result.Body);

                    if (caseTypeList != null && caseTypeList.Count > 0)
                    {
                        foreach (var item in caseTypeList)
                        {
                            _dIcCaseType.Add(item.Value, item.Name);
                        }
                    }
                    else
                    {

                    }
                }

            });

            // 页面初始化，清空
            MessagingCenter.Subscribe<string>(this, "InitCaseSearchPage", (obj) =>
            {
                Init();
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
        public string _keyWord;
        public string KeyWord
        {
            get { return _keyWord == null ? "" : _keyWord; }
            set { SetProperty(ref _keyWord, value); }
        }
        public string _caseType;
        public string CaseType
        {
            get
            {
                return _caseType == null || _caseType == "" ? "全部" : _caseType;
            }
            set
            {
                SetProperty(ref _caseType, value);
            }
        }

        public string _caseTypeCode;
        public string CaseTypeCode
        {
            get
            {
                return _caseTypeCode == null || _caseTypeCode == "" ? "0" : _caseTypeCode;
            }
            set
            {
                SetProperty(ref _caseTypeCode, value);
            }
        }
        public CasesListDto _selectedCaseItem;
        public CasesListDto SelectedCaseItem
        {
            get
            {
                return _selectedCaseItem;
            }
            set
            {
                SetProperty(ref _selectedCaseItem, value);
            }
        }

        public List<CasesListDto> _caseInfoList;

        public List<CasesListDto> CaseInfoList
        {
            get
            {
                return _caseInfoList;
            }
            set
            {
                SetProperty(ref _caseInfoList, value);
            }
        }


        #endregion

        #region Command
        public Command OpenCaseTypeCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        var action = await _commonFun.ShowActionSheetAny(_dIcCaseType.Values.ToArray<string>());
                        if (_dIcCaseType.ContainsValue(action))
                        {
                            CaseType = action;
                            CaseTypeCode = _dIcCaseType.FirstOrDefault(q => q.Value == action).Key;
                        }
                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }



        public Command SearchCaseListCommand
        {
            get
            {
                return new Command(async () =>
                {
                    //try
                    //{
                    //    _commonFun.ShowLoading("查询中...");
                    //    var result = await _caseService.GetCaseList(4, StartDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd"), CaseTypeCode, KeyWord);
                    //    if (null != result && result.ResultCode == Module.ResultType.Success)
                    //    {
                    //        var casesList = JsonConvert.DeserializeObject<List<CasesListDto>>(result.Body);

                    //        if (casesList != null && casesList.Count > 0)
                    //        {
                    //            CaseInfoList = casesList;
                    //        }
                    //        else
                    //        {
                    //            _commonFun.AlertLongText(result.Msg);
                    //        }
                    //    }
                    //}
                    //catch (OperationCanceledException)
                    //{
                    //    _commonFun.HideLoading();
                    //    _commonFun.AlertLongText("请求超时,请重试");
                    //}
                    //catch (Exception ex)
                    //{
                    //    _commonFun.HideLoading();
                    //    _commonFun.AlertLongText("检查失败,请重试");
                    //}
                    //finally
                    //{
                    //    _commonFun.HideLoading();
                    //}

                    try
                    {
                        Dictionary<string, string> param = new Dictionary<string, string>();
                        param.Add("UserId", CommonContext.Account.UserId);
                        param.Add("StartDate", StartDate.ToString("yyyy-MM-dd"));
                        param.Add("EndDate", EndDate.ToString("yyyy-MM-dd"));
                        param.Add("CaseTypeCode", CaseTypeCode);
                        param.Add("CaseType", CaseType);
                        param.Add("KeyWord", KeyWord);
                        if (StartDate > EndDate)
                        {
                            _commonFun.AlertLongText("结束日期不能小于开始日期");
                            return;
                        }
                        await Navigation.PopAsync(true);
                        MessagingCenter.Send<Dictionary<string, string>>(param, "CaseSearchResult");
                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }

        public async void TappedCommand(CasesListDto dto)
        {
            try
            {
                // await Navigation.PushAsync<CaseRegViewModel>((vm, v) => vm.Init(SelectedCaseItem.Id), true);
                await Navigation.PopAsync();
                List<RequestParameter> parameterLst = new List<RequestParameter>();
                parameterLst.Add(new RequestParameter { Name = "caseId", Value = SelectedCaseItem.Id.ToString() });
                MessagingCenter.Send<List<RequestParameter>>(parameterLst, "GetCaseInfoDetail");

            }
            catch (Exception)
            {
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
            KeyWord = string.Empty;
            CaseType = "全部";
            CaseTypeCode = "0";

        }

        #endregion
    }
}
