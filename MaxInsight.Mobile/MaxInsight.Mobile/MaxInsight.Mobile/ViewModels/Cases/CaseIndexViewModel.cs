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
    public class CaseIndexViewModel : ViewModel
    {
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        ICaseService _caseService;
        public RelayCommand<CasesListDto> ItemTappedCommand { get; set; }

        #region Constructor
        public CaseIndexViewModel()
        {
            _commonFun = Resolver.Resolve<ICommonFun>();
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _caseService = Resolver.Resolve<ICaseService>();
            ItemTappedCommand = new RelayCommand<CasesListDto>(TappedCommand);

            MessagingCenter.Unsubscribe<string>(this, MessageConst.CASESAVESUCCESS);
            MessagingCenter.Subscribe<string>(this, MessageConst.CASESAVESUCCESS, (c) =>
            {
                SearchTopNCaseList();
            });

            MessagingCenter.Subscribe<string>(this, MessageConst.SEARCHTOPNCASELIST, (c) =>
            {
                SearchTopNCaseList();
            });
        }
        #endregion

        #region Property
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

        #region Method
        private async void SearchTopNCaseList()
        {
            try
            {
                _commonFun.ShowLoading("查询中...");
                var now = DateTime.Now.ToString();
                var result = await _caseService.GetCaseList(Convert.ToInt32(CommonContext.Account.UserId), "", "", "0", "");
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
            }
            finally
            {
                _commonFun.HideLoading();
            }
        }
        #endregion
    }
}
