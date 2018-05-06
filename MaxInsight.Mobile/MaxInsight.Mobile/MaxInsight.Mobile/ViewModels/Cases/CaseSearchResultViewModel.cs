using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module.Dto.Case;
using MaxInsight.Mobile.Services.CaseService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.Cases
{
    public class CaseSearchResultViewModel : ViewModel
    {
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        ICaseService _caseService;
        public RelayCommand<CasesListDto> ItemTappedCommand { get; set; }

        int userid = 0;
        string startDate = "";
        string endDate = "";
        string caseTypeCode = "";
        public CaseSearchResultViewModel()
        {
            _commonFun = Resolver.Resolve<ICommonFun>();
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _caseService = Resolver.Resolve<ICaseService>();
            ItemTappedCommand = new RelayCommand<CasesListDto>(TappedCommand);

            MessagingCenter.Subscribe<Dictionary<string, string>>(this, "CaseSearchResult", (c) =>
            {
                InitParam(c);
                SearchCaseLst();
            });

            MessagingCenter.Subscribe<string>(this, "CaseSearchResultList", (c) =>
            {
                SearchCaseLst();
            });


        }
        #region Property
        private string _searchPeriod;
        public string SearchPeriod
        {
            get
            {
                return _searchPeriod;
            }
            set
            {
                SetProperty(ref _searchPeriod, value);
            }
        }
        private string _keyWord;
        public string KeyWord
        {
            get
            {
                return _keyWord;
            }
            set
            {
                SetProperty(ref _keyWord, value);
            }
        }

        private string _caseType;
        public string CaseType
        {
            get
            {
                return _caseType;
            }
            set
            {
                SetProperty(ref _caseType, value);
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

        public Command GoCaseSearchPageCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Navigation.PushAsync<CaseSearchViewModel>((vm, v) => vm.Init(), true);
                });
            }
        }

        #endregion

        #region Command
        public async void TappedCommand(CasesListDto dto)
        {
            try
            {
                await Navigation.PushAsync<CaseRegViewModel>((vm, v) => vm.Init(SelectedCaseItem.Id), true);

            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region method

        private void InitParam(Dictionary<string, string> param)
        {
            if (param.ContainsKey("UserId"))
            {
                userid = Convert.ToInt32(param["UserId"]);
            }

            if (param.ContainsKey("StartDate"))
            {
                startDate = param["StartDate"].ToString();
            }

            if (param.ContainsKey("EndDate"))
            {
                endDate = param["EndDate"].ToString();
            }

            if (param.ContainsKey("CaseTypeCode"))
            {
                caseTypeCode = param["CaseTypeCode"].ToString();
            }
            if (param.ContainsKey("KeyWord"))
            {
                KeyWord = param["KeyWord"].ToString();
            }
            if (param.ContainsKey("CaseType"))
            {
                CaseType = param["CaseType"].ToString();
            }
            SearchPeriod = startDate + "~" + endDate;

        }

        private async void SearchCaseLst()
        {
            try
            {
                _commonFun.ShowLoading("查询中...");
                var result = await _caseService.GetCaseList(userid, startDate, endDate, caseTypeCode, KeyWord);
                if (null != result && result.ResultCode == Module.ResultType.Success)
                {
                    var casesList = JsonConvert.DeserializeObject<List<CasesListDto>>(result.Body);

                    if (casesList != null && casesList.Count > 0)
                    {
                        CaseInfoList = casesList;
                    }
                    else
                    {
                        CaseInfoList = new List<CasesListDto>();
                        _commonFun.ShowToast("没有数据");
                    }
                }

            }
            catch (OperationCanceledException)
            {
                _commonFun.HideLoading();
                _commonFun.AlertLongText("请求超时,请重试");
            }
            catch (Exception)
            {
                _commonFun.HideLoading();
                _commonFun.AlertLongText("查询失败,请重试");
            }
            finally
            {
                _commonFun.HideLoading();
            }
        }
        #endregion
    }
}


