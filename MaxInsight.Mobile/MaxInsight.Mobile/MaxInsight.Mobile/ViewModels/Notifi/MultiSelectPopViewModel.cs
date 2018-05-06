using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module.Dto.Notifi;
using MaxInsight.Mobile.Pages.Notifi;
using MaxInsight.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.Notifi
{
    public class MultiSelectPopViewModel:ViewModel
    {
       public MultiSelectPopViewModel()
        {
            MessagingCenter.Subscribe<string>(
           this,
           MessageConst.NOTICE_DISTRIBUTOR_ONE,
           (allcheckornot) =>
           {
               OneNoticeDistributor(allcheckornot);
           });
        }

        bool _isAllChecked;
        public bool IsAllChecked {
            get { return _isAllChecked; }
            set { SetProperty(ref _isAllChecked, value, "IsAllChecked"); }

        }
        public void OneNoticeDistributor(string allcheckornot)
        {
            IsAllChecked = allcheckornot=="Y"?true:false;
        }
    }
}
