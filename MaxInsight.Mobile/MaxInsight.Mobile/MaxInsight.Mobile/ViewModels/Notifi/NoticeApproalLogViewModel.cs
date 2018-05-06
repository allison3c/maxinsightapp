using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto.Notifi;
using MaxInsight.Mobile.Services.NotifiService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.Notifi
{
    public class NoticeApproalLogViewModel : ViewModel
    {
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        INotifiMngService _notifiMngService;

        public NoticeApproalLogViewModel()
        {
            _commonFun = Resolver.Resolve<ICommonFun>();
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _notifiMngService = Resolver.Resolve<INotifiMngService>();
        }

        #region properties
        private List<ApproalLogDto> _approalLogDtoList;
        public List<ApproalLogDto> ApproalLogDtoList
        {
            get
            {
                return _approalLogDtoList;
            }
            set
            {
                SetProperty(ref _approalLogDtoList, value);
            }
        }
        private string _pageTitle;
        public string PageTitle
        {
            get
            {
                return _pageTitle;
            }
            set
            {
                SetProperty(ref _pageTitle, value);
            }
        }
        #endregion

        #region method
        public void Init(List<ApproalLogDto> dtolist,string pageTitle)
        {
            ApproalLogDtoList = dtolist;
            PageTitle = pageTitle;
        }

        #endregion
    }
}
