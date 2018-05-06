using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using XLabs.Enums;
using XLabs.Forms.Controls;

namespace MaxInsight.Mobile.Pages.Cases
{
    public partial class CaseRegPage : ContentPage
    {
        public CaseRegPage()
        {
            InitializeComponent();
            //AddTopButtons();
            //Title = "案例登记";
            SetControlsStatus();
        }
        public void OnDeleteCaseAttechment(object sender, EventArgs e)
        {
            ImageButton img = sender as ImageButton;
            MessagingCenter.Send<string>(img.CommandParameter.ToString(), MessageConst.CASEATTACH_DELETE);
        }
        void SetControlsStatus()
        {
            //管理员,大区，区域可以登记案例
            if (CommonContext.Account.UserType == "R" || CommonContext.Account.UserType == "Z" || CommonContext.Account.UserType == "A")
            {
                statusBtn.IsVisible = true;
                lblCaseType.IsVisible = false;
                lstCaseInofAttachWithDelete.IsVisible = true;
                lstCaseInofAttach.IsVisible = false;
                Title = "案例登记";
            }
            else
            {
                statusBtn.IsVisible = false;
                lblCaseType.IsVisible = true;
                lstCaseInofAttachWithDelete.IsVisible = false;
                lstCaseInofAttach.IsVisible = true;
                Title = "案例详细";
            }
            //文件上传在IOS上不显示
            if (Device.OS == TargetPlatform.iOS)
            {
                btnAttachFileUpload.IsVisible = false;
            }
            else
            {
                btnAttachFileUpload.IsVisible = true;
            }
        }
        //private void AddTopButtons()
        //{
        //    var caseRegStack = new StackLayout()
        //    {
        //        Orientation = StackOrientation.Vertical,
        //        HorizontalOptions = LayoutOptions.StartAndExpand,
        //        VerticalOptions = LayoutOptions.Center,
        //        Padding = new Thickness(0, 5, 0, 0),
        //        Margin = new Thickness(30, 0, 0, 0)
        //    };

        //    var caseRegButton = new ImageButton()
        //    {
        //        Source = ImageSource.FromFile("Improveplanapply"),
        //        Orientation = ImageOrientation.ImageCentered,
        //        BackgroundColor = Color.Transparent,
        //        HorizontalOptions = LayoutOptions.Center,
        //        TextColor = Color.FromHex("#FFFFFF"),
        //        WidthRequest = 60,
        //        HeightRequest = 60
        //    };
        //    caseRegButton.SetBinding(Button.CommandProperty, "ImproveResultOrResultApprovalSearchCommand");

        //    var caseRegLabel = new Label()
        //    {
        //        Text = "案例登记",
        //        HorizontalOptions = LayoutOptions.Center,
        //        TextColor = Color.White,
        //        FontSize = 15
        //    };

        //    caseRegStack.Children.Add(caseRegButton);
        //    caseRegStack.Children.Add(caseRegLabel);

        //    topStack.Children.Add(caseRegStack);


        //    var caseSearchStack = new StackLayout()
        //    {
        //        Orientation = StackOrientation.Vertical,
        //        HorizontalOptions = LayoutOptions.StartAndExpand,
        //        VerticalOptions = LayoutOptions.Center,
        //        Padding = new Thickness(0, 5, 0, 0),
        //        Margin = new Thickness(30, 0, 0, 0)
        //    };

        //    var caseSearchButton = new ImageButton()
        //    {
        //        Source = ImageSource.FromFile("Improveplanapply"),
        //        Orientation = ImageOrientation.ImageCentered,
        //        BackgroundColor = Color.Transparent,
        //        HorizontalOptions = LayoutOptions.Center,
        //        TextColor = Color.FromHex("#FFFFFF"),
        //        WidthRequest = 60,
        //        HeightRequest = 60
        //    };
        //    caseRegButton.SetBinding(Button.CommandProperty, "ImproveResultOrResultApprovalSearchCommand");

        //    var caseSearchLabel = new Label()
        //    {
        //        Text = "案例登记",
        //        HorizontalOptions = LayoutOptions.Center,
        //        TextColor = Color.White,
        //        FontSize = 15
        //    };

        //    caseSearchStack.Children.Add(caseSearchButton);
        //    caseSearchStack.Children.Add(caseSearchLabel);

        //    topStack.Children.Add(caseSearchStack);
        //}
    }
}
