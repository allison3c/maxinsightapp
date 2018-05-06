using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLabs.Forms.Mvvm;

namespace MaxInsight.Mobile.Module.Dto.Notifi
{
    public class MultiSelectDto:ViewModel
    {
        public bool _isChecked;
        public string DisCode { get; set; }
        public string DisName { get; set; }
        public bool IsChecked
        {
            get { return _isChecked;}
            set { SetProperty(ref _isChecked, value, "IsChecked"); }
        }
    }
}
