using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module.Dto.Report;
using MaxInsight.Mobile.Services.ReportService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.Report
{
    public class ReportSearchIndexViewModel : ViewModel
    {
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        IReportService _reportService;
        public RelayCommand<ReportAttachmentDto> ItemTappedCommand { get; set; }
        int userid = 0;
        string startDate = "";
        string endDate = "";
        string sourceTypeCode = "";
        public ReportSearchIndexViewModel()
        {
            _commonFun = Resolver.Resolve<ICommonFun>();
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _reportService = Resolver.Resolve<IReportService>();
            ItemTappedCommand = new RelayCommand<ReportAttachmentDto>(TappedCommand);

            MessagingCenter.Subscribe<Dictionary<string, string>>(this, "ReportSearchResult", (c) =>
            {
                InitParam(c);
                SearchReportLst();
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

        private string _sourceType;
        public string SourceType
        {
            get
            {
                return _sourceType;
            }
            set
            {
                SetProperty(ref _sourceType, value);
            }
        }
        public List<ReportAttachmentDto> _reportAttachmentLst;

        public List<ReportAttachmentDto> ReportAttachmentLst
        {
            get
            {
                return _reportAttachmentLst;
            }
            set
            {
                SetProperty(ref _reportAttachmentLst, value);
            }
        }

        public ReportAttachmentDto _selectedItem;
        public ReportAttachmentDto SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                SetProperty(ref _selectedItem, value);
            }
        }

        public Command GotoSearchPageCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Navigation.PushAsync<ReportSearchViewModel>((vm, v) => vm.Init(), true);
                });
            }
        }
        public async void TappedCommand(ReportAttachmentDto dto)
        {
            try
            {
                _commonFun.DownLoadFileFromOssForReport(SelectedItem.Url, SelectedItem.AttachName, "RMMT");
                await _reportService.UpdateAttachmentDownloadCnt(SelectedItem.Id);
                _commonFun.HideLoading();
            }
            catch (Exception)
            {
                _commonFun.ShowLoading("下载异常，请重试");
                _commonFun.HideLoading();
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

            if (param.ContainsKey("SourceTypeCode"))
            {
                sourceTypeCode = param["SourceTypeCode"].ToString();
            }
            if (param.ContainsKey("SourceType"))
            {
                SourceType = param["SourceType"].ToString();
            }
            SearchPeriod = startDate + "~" + endDate;

        }

        private async void SearchReportLst()
        {
            try
            {
                _commonFun.ShowLoading("查询中...");
                var result = await _reportService.GetAttachmentByUserId(userid, sourceTypeCode, startDate.Replace("-", ""), endDate.Replace("-", ""));
                if (null != result && result.ResultCode == Module.ResultType.Success)
                {
                    var casesList = JsonConvert.DeserializeObject<List<ReportAttachmentDto>>(result.Body);

                    if (casesList != null && casesList.Count > 0)
                    {
                        ReportAttachmentLst = casesList;
                    }
                    else
                    {
                        ReportAttachmentLst = new List<ReportAttachmentDto>();
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
                _commonFun.AlertLongText("查询异常,请重试");
            }
            finally
            {
                _commonFun.HideLoading();
            }
        }
        #endregion
    }
}
