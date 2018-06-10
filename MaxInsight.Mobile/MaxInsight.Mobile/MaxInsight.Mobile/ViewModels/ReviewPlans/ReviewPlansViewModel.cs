using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto.ReviewPlans;
using MaxInsight.Mobile.Services.ReviewPlansService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.ReviewPlans
{
    public class ReviewPlansViewModel : ViewModel
    {
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        IReviewPlansService _reviewPlansService;

        public ReviewPlansViewModel()
        {
            try
            {
                _commonFun = Resolver.Resolve<ICommonFun>();
                _commonHelper = Resolver.Resolve<CommonHelper>();
                _reviewPlansService = Resolver.Resolve<IReviewPlansService>();
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ReviewPlansViewModel");
                return;
            }
        }
        #region properties
        public string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                SetProperty(ref _name, value);
            }
        }

        public string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                SetProperty(ref _title, value);
            }
        }

        public string _visitDateTime;
        public string VisitDateTime
        {
            get
            {
                return _visitDateTime;
            }
            set
            {
                SetProperty(ref _visitDateTime, value);
            }
        }

        public string _visitTypeName;
        public string VisitTypeName
        {
            get
            {
                return _visitTypeName;
            }
            set
            {
                SetProperty(ref _visitTypeName, value);
            }
        }

        double _lstHeight;
        public double LstHeight
        {
            get { return _lstHeight; }
            set { SetProperty(ref _lstHeight, value); }
        }
        public List<PlansDtlDto> _plansDtlDtoList;
        public List<PlansDtlDto> PlansDtlDtoList
        {
            get { return _plansDtlDtoList; }
            set { SetProperty(ref _plansDtlDtoList, value); }
        }

        public string _reviewPlansContent;
        public string ReviewPlansContent
        {
            get
            {
                return _reviewPlansContent;
            }
            set
            {
                SetProperty(ref _reviewPlansContent, value);
            }
        }
        private RelayCommand _passCommand;
        public RelayCommand PassCommand
        {

            get
            {
                return _passCommand ??

                (_passCommand = new RelayCommand(PassPlans));
            }
        }
        private RelayCommand _refuseCommand;
        public RelayCommand RefuseCommand
        {

            get
            {
                return _refuseCommand ??

                (_refuseCommand = new RelayCommand(RefusePlans));
            }
        }
        #endregion
        #region Mothod
        public void Init(string pid)
        {
            try
            {
                ReviewPlansContent = "";
                GetPlansDtlList(pid);
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ReviewPlansViewModel");
                return;
            }
        }

        public async void GetPlansDtlList(string Id)
        {
            if (_commonHelper.IsNetWorkConnected() == true)
            {
                try
                {
                    _commonFun.ShowLoading("查询中...");
                    // TO-DO
                    var result = await _reviewPlansService.GetPlansDtlList(Id);
                    if (result.ResultCode == Module.ResultType.Success)
                    {
                        var List = JsonConvert.DeserializeObject<List<PlansDtlDto>>(result.Body);
                        //CasesInfoDto caseInfoDto = CommonHelper.DecodeString<CasesInfoDto>(result.Body);
                        if (List != null)
                        {
                            Title = List[0].Title;
                            Name = List[0].Name;
                            VisitDateTime = List[0].VisitDateTime;
                            VisitTypeName = List[0].VisitTypeName;
                            PlansDtlDtoList = List;
                            LstHeight = (32 * List.Count) + 40;
                            _commonFun.HideLoading();
                        }
                        else
                        {
                            LstHeight = 30;
                            _commonFun.HideLoading();
                            _commonFun.ShowToast("没有数据");
                        }
                    }
                    else
                    {
                        _commonFun.HideLoading();
                        _commonFun.AlertLongText("查询失败，请重试。 " + result.Msg);
                    }
                }
                catch (OperationCanceledException)
                {
                    _commonFun.HideLoading();
                    _commonFun.AlertLongText("请求超时。");
                }
                catch (Exception)
                {
                    _commonFun.HideLoading();
                    _commonFun.AlertLongText("查询异常，请重试。");
                }
                finally
                {
                    _commonFun.HideLoading();
                }
            }
            else
            {
                _commonFun.AlertLongText("网络连接异常。");
            }
        }
        public async void PassPlans()
        {
            try
            {
                if (ReviewPlansContent == "")
                {
                    _commonFun.AlertLongText("请填写审核意见");
                    return;
                }
                _commonFun.ShowLoading("保存中...");
                var result = await _reviewPlansService.ReviewPlans(PlansDtlDtoList[0].Id, "P", ReviewPlansContent, CommonContext.Account.UserId);
                if (result != null)
                {
                    if (result.ResultCode == ResultType.Success)
                    {
                        //跳转或刷新CaseIndexViewModel
                        //MessagingCenter.Send<string>("", MessageConst.GETREVIEWPLANSLIST);
                        //MessagingCenter.Send<string>("", MessageConst.NOTIFI_SAVEREFRESH_GO);
                        await Navigation.PopAsync(true);
                        _commonFun.AlertLongText("保存成功");
                    }
                    else
                    {
                        _commonFun.AlertLongText(result.Msg);
                    }
                }
                else
                {
                    _commonFun.AlertLongText("审核时服务出现错误,,请重试");
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
                _commonFun.AlertLongText("保存异常,请重试");
            }
            finally
            {
                _commonFun.HideLoading();
            }
        }
        public async void RefusePlans()
        {
            try
            {
                if (ReviewPlansContent == "")
                {
                    _commonFun.AlertLongText("请填写审核意见");
                    return;
                }
                _commonFun.ShowLoading("保存中...");
                var result = await _reviewPlansService.ReviewPlans(PlansDtlDtoList[0].Id, "F", ReviewPlansContent, CommonContext.Account.UserId);
                if (result != null)
                {
                    if (result.ResultCode == ResultType.Success)
                    {
                        //MessagingCenter.Send<string>("", MessageConst.GETREVIEWPLANSLIST);
                        //MessagingCenter.Send<string>("", MessageConst.NOTIFI_SAVEREFRESH_GO);
                        await Navigation.PopAsync(true);
                        _commonFun.AlertLongText("保存成功");
                    }
                    else
                    {
                        _commonFun.AlertLongText(result.Msg);
                    }
                }
                else
                {
                    _commonFun.AlertLongText("审核时服务出现错误,,请重试");
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
                _commonFun.AlertLongText("保存异常,请重试");
            }
            finally
            {
                _commonFun.HideLoading();
            }
        }

        #endregion
    }
}
