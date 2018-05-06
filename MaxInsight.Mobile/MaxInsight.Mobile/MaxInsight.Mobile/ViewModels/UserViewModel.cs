using XLabs.Forms.Mvvm;

namespace MaxInsight.Mobile.ViewModels
{
    public class UserViewModel : ViewModel
    {
        public UserViewModel()
        {
            DisName = CommonContext.Account.DisName;
        }
        #region Properties

        public string DisName { get; set; }
        public string UserTypeName { get; set; } = CommonContext.Account.UserTypeName;
        public string TelNo { get { return CommonContext.Account.Phone; } }

        #endregion
    }
}
