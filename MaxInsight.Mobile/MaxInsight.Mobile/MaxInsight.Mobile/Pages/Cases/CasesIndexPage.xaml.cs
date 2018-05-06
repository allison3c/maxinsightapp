using MaxInsight.Mobile.ViewModels.Cases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using XLabs.Forms.Mvvm;

namespace MaxInsight.Mobile.Pages.Cases
{
    public partial class CasesIndexPage : ContentPage
    {
        public CasesIndexPage()
        {
            InitializeComponent();
            Title = "案例分享";
            SetTopStackVisible();

            stackCaseReg.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                NumberOfTapsRequired = 1,
                Command = new Command(CaseRegCommand)
            });

            stackCaseSearch.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                NumberOfTapsRequired = 1,
                Command = new Command(CaseSearchCommand)
            });
        }
        void SetTopStackVisible()
        {
            //管理员,大区，区域可以登记案例
            if (CommonContext.Account.UserType == "R" || CommonContext.Account.UserType == "Z" || CommonContext.Account.UserType == "A")
            {
                stackCaseReg.IsVisible = true;
            }
            else
            {
                stackCaseReg.IsVisible = false;
                stackCaseSearch.HorizontalOptions = LayoutOptions.CenterAndExpand;
            }
        }
        private async void CaseSearchCommand()
        {
            //var page = ViewFactory.CreatePage<CaseSearchViewModel, CaseSearchPage>() as Page;

            //if (!Navigation.NavigationStack.Contains(page))
            //{
            //    await Navigation.PushAsync(page, true);
            //    MessagingCenter.Send<string>("", "InitCaseSearchPage");
            //}

            var page = ViewFactory.CreatePage<CaseSearchResultViewModel, CaseSearchResultPage>() as Page;

            if (!Navigation.NavigationStack.Contains(page))
            {
                await Navigation.PushAsync(page, true);
                //MessagingCenter.Send<string>("", "InitCaseSearchPage");
            }
        }

        private async void CaseRegCommand()
        {
            var page = ViewFactory.CreatePage<CaseRegViewModel, CaseRegPage>() as Page;

            if (!Navigation.NavigationStack.Contains(page))
            {
                await Navigation.PushAsync(page, true);
                MessagingCenter.Send<string>("", "InitCaseRegPage");
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send<string>("", MessageConst.SEARCHTOPNCASELIST);
        }
    }
}
