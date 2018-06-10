using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MaxInsight.Mobile
{
    public partial class ViewRecordPage : ContentPage
    {
        public ViewRecordPage()
        {
            InitializeComponent();
            Title = "得分查看";
            SetControls();
        }
        private void SetControls()
        {
            try
            {
                if (CommonContext.Account.UserType == "D" || CommonContext.Account.UserType == "S")
                {
                    stackDis.IsVisible = false;
                }
                else
                {
                    stackDis.IsVisible = true;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
