using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module.Dto.Case;
using MaxInsight.Mobile.Services.CaseService;
using MaxInsight.Mobile.Services.ImproveService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.Common
{
    public class ComSinglePopViewModel : ViewModel
    {
        ICaseService _caseService;
        IImproveService _improveService;
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        public ComSinglePopViewModel()
        {
            try
            {
                _caseService = Resolver.Resolve<ICaseService>();
                _improveService = Resolver.Resolve<IImproveService>();
                _commonFun = Resolver.Resolve<ICommonFun>();
                _commonHelper = Resolver.Resolve<CommonHelper>();
                MessagingCenter.Unsubscribe<string>(this, MessageConst.COMMON_SOURCE_GET);
                MessagingCenter.Subscribe<string>(
             this,
             MessageConst.COMMON_SOURCE_GET,
             (dataType) =>
             {
                 SetListViewSource(dataType);
             });
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ComSinglePopViewModel");
            }
        }

        #region Property(s)
        List<NameValueObject> _sourceList;
        public List<NameValueObject> SourceList
        {
            get { return _sourceList; }
            set { SetProperty(ref _sourceList, value, "SourceList"); }
        }
        double _lstRowHeight = 32;
        double _lstHeight;
        public double LstHeight
        {
            get { return _lstHeight; }
            set { SetProperty(ref _lstHeight, value); }
        }
        private string _closeBtnColor = "#F39801";
        public string CloseBtnColor
        {
            get { return _closeBtnColor; }
            set { SetProperty(ref _closeBtnColor, value); }
        }
        #endregion

        #region GetData
        private void SetListViewSource(string dataType)
        {
            switch (dataType)
            {
                case "NoticeStatus":
                    if (CommonContext.Account.UserType == "A" ||
                            CommonContext.Account.UserType == "R" ||
                              CommonContext.Account.UserType == "Z")
                    {
                        List<NameValueObject> statusList = new List<NameValueObject>() { new NameValueObject() { Name = "全部", Value = "" },
                                                                                   new NameValueObject() { Name = "暂存", Value = "T" },
                                                                                  new NameValueObject() { Name = "提交", Value = "S" },
                                                                                 new NameValueObject() { Name = "其它", Value = "1" }};
                        SourceList = statusList;
                        LstHeight = _lstRowHeight * 4;
                    }
                    else
                    {
                        GetNoticeStatusList();
                    }
                    CloseBtnColor = "#F39801";
                    break;
                case "ImpAllTaskOfPlan":
                    GetImpAllTaskOfPlan();
                    CloseBtnColor = "#398FC0";
                    break;
                case "ImpAllSourceType":
                    GetImpAllSourceType();
                    CloseBtnColor = "#398FC0";
                    break;
                default: break;
            }
        }
        #endregion

        #region Method
        private async void GetNoticeStatusList()
        {
            try
            {
                _commonFun.ShowLoading("查询中...");
                //TO-DO
                var result = await _caseService.GetTypeFromHiddenCode("02");
                if (result != null && result.ResultCode == Module.ResultType.Success)
                {

                    var statusList = JsonConvert.DeserializeObject<List<NameValueObject>>(result.Body);
                    if (statusList != null && statusList.Count > 0)
                    {
                        _commonFun.HideLoading();
                        statusList.Insert(0, new NameValueObject() { Name = "全部", Value = "" });
                        SourceList = statusList;
                        LstHeight = SourceList.Count * _lstRowHeight;
                    }
                    else
                    {
                        _commonFun.HideLoading();
                        SourceList = new List<NameValueObject>();
                        _commonFun.ShowToast("没有数据");
                    }
                }
                else
                {
                    _commonFun.HideLoading();
                    SourceList = new List<NameValueObject>();
                    _commonFun.AlertLongText("查询失败，请重试。 " + result.Msg);
                }
            }
            catch (OperationCanceledException)
            {
                _commonFun.HideLoading();
                SourceList = new List<NameValueObject>();
                _commonFun.AlertLongText("请求超时。");
            }
            catch (Exception)
            {
                _commonFun.HideLoading();
                SourceList = new List<NameValueObject>();
                _commonFun.AlertLongText("查询失败，请重试。");
            }
            finally
            {
                _commonFun.HideLoading();
            }
        }

        private async void GetImpAllTaskOfPlan()
        {
            try
            {
                _commonFun.ShowLoading("查询中...");
                //TO-DO
                var result = await _improveService.GetAllTaskOfPlanForImp(CommonContext.Account.UserId);
                if (result != null && result.ResultCode == Module.ResultType.Success)
                {
                    var tpList = JsonConvert.DeserializeObject<List<NameValueObject>>(result.Body);
                    if (tpList != null && tpList.Count > 0)
                    {
                        tpList.Insert(0, new NameValueObject() { Name = "全部", Value = "0" });
                        SourceList = tpList;
                        LstHeight = SourceList.Count * _lstRowHeight;
                    }
                    else
                    {
                        SourceList = new List<NameValueObject>();
                        _commonFun.ShowToast("没有数据");
                    }
                }
                else
                {
                    SourceList = new List<NameValueObject>();
                    _commonFun.AlertLongText("查询失败，请重试。 " + result.Msg);
                }
            }
            catch (OperationCanceledException)
            {
                SourceList = new List<NameValueObject>();
                _commonFun.AlertLongText("请求超时。");
            }
            catch (Exception)
            {
                SourceList = new List<NameValueObject>();
                _commonFun.AlertLongText("查询失败，请重试。");
            }
            finally
            {
                _commonFun.HideLoading();
            }
        }


        private async void GetImpAllSourceType()
        {
            try
            {
                _commonFun.ShowLoading("查询中...");
                //TO-DO
                var result = await _caseService.GetTypeFromHiddenCode("15");
                if (result != null && result.ResultCode == Module.ResultType.Success)
                {
                    var stList = JsonConvert.DeserializeObject<List<NameValueObject>>(result.Body);
                    if (stList != null && stList.Count > 0)
                    {
                        stList.Insert(0, new NameValueObject() { Name = "全部", Value = "" });
                        SourceList = stList;
                        LstHeight = SourceList.Count * _lstRowHeight;
                    }
                    else
                    {
                        SourceList = new List<NameValueObject>();
                        _commonFun.ShowToast("没有数据");
                    }
                }
                else
                {
                    SourceList = new List<NameValueObject>();
                    _commonFun.AlertLongText("查询失败，请重试。 " + result.Msg);
                }
            }
            catch (OperationCanceledException)
            {
                SourceList = new List<NameValueObject>();
                _commonFun.AlertLongText("请求超时。");
            }
            catch (Exception)
            {
                SourceList = new List<NameValueObject>();
                _commonFun.AlertLongText("查询失败，请重试。");
            }
            finally
            {
                _commonFun.HideLoading();
            }
        }
        #endregion
    }
}
