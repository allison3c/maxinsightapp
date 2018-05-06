using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module.Dto.Case;
using MaxInsight.Mobile.Services.ReviewPlansService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.Cases
{
    public class PushInfoViewModel: ViewModel
    {
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        IReviewPlansService _reviewPlansService;

        public PushInfoViewModel()
        {
            _commonFun = Resolver.Resolve<ICommonFun>();
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _reviewPlansService = Resolver.Resolve<IReviewPlansService>();

        }

        public string _pushContent;
        public string PushContent
        {
            get
            {
                return _pushContent;
            }
            set
            {
                SetProperty(ref _pushContent, value);
            }
        }
        public string _pushTitle;
        public string PushTitle
        {
            get
            {
                return _pushTitle;
            }
            set
            {
                SetProperty(ref _pushTitle, value);
            }
        }
        public void Init(string Id)
        {
            PushTitle = "";
            PushContent = "";
            GetPushInfo(Id);
        }

        public async void GetPushInfo(string Id)
        {
            if (_commonHelper.IsNetWorkConnected() == true)
            {
                try
                {
                    _commonFun.ShowLoading("查询中...");
                    // TO-DO
                    var result = await _reviewPlansService.GetPushInfo(Id);
                    if (result.ResultCode == Module.ResultType.Success)
                    {
                        var List = JsonConvert.DeserializeObject<List<PushInfoDto>>(result.Body);
                        //CasesInfoDto caseInfoDto = CommonHelper.DecodeString<CasesInfoDto>(result.Body);
                        if (List != null)
                        {
                            PushTitle = List[0].Title;
                            PushContent = List[0].Content;
                            _commonFun.HideLoading();
                        }
                        else
                        {
                            
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
                catch (Exception e)
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
    }
}
