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
    public class MultiSelectDepViewModel:ViewModel
    {
       
       
        public MultiSelectDepViewModel()
        {
            MessagingCenter.Subscribe<string>(
         this,
         MessageConst.NOTICE_DISTRIBUTOR_ONE,
         (allcheckornot) =>
         {
             OneNoticeDistributor(allcheckornot);
         });
        }

        #region Property(s)
        bool _isAllChecked;
        public bool IsAllChecked
        {
            get { return _isAllChecked; }
            set { SetProperty(ref _isAllChecked, value, "IsAllChecked"); }

        }
        #endregion

        #region GetData
        public void OneNoticeDistributor(string allcheckornot)
        {
            IsAllChecked = allcheckornot == "Y" ? true : false;
        }
        #endregion
    }
}
