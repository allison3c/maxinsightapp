using System;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;

namespace MaxInsight.Mobile.ViewModels.Notifi
{
    public class MultiSelectDepViewModel : ViewModel
    {
        public MultiSelectDepViewModel()
        {
            try
            {
                MessagingCenter.Subscribe<string>(
             this,
             MessageConst.NOTICE_DISTRIBUTOR_ONE,
             (allcheckornot) =>
             {
                 OneNoticeDistributor(allcheckornot);
             });
            }
            catch (Exception)
            {
            }
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
