using System;
using Xamarin.Forms;
using System.Linq;
using MaxInsight.Mobile.Module.Dto.Notifi;
using XLabs.Forms.Mvvm;
using MaxInsight.Mobile.ViewModels.Notifi;

namespace MaxInsight.Mobile.Pages.Notifi
{
    public partial class NotifiIndexPage : ContentPage
    {
        private NotifiMngPage notifiMngPage;
        //private NotifiFeedbackPage notifiFeedbackPage;
        public NotifiIndexPage()
        {
            InitializeComponent();
            SetControlRole();

            noticeReg.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                NumberOfTapsRequired = 1,
                Command = new Command(NoticeRegCommand)
            });

            noticeApproal.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                NumberOfTapsRequired = 1,
                Command = new Command(NoticeApproalCommand)
            });

            noticeSearch.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                NumberOfTapsRequired = 1,
                Command = new Command(NoticeSearchCommand)
            });

            noticeFeed.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                NumberOfTapsRequired = 1,
                Command = new Command(NoticeFeedCommand)
            });

            noticeSearch2.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                NumberOfTapsRequired = 1,
                Command = new Command(NoticeSearchCommand)
            });
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (CommonContext.Account.UserType == "S" || CommonContext.Account.UserType == "D")
            {
                MessagingCenter.Send<NotifiIndexPage>(this, MessageConst.NOTICE_FEEDBDATA_GET);
            }
            else
            {
                //MessagingCenter.Send<string>("", MessageConst.NOTICE_APPROALDATA_GET);
            }
        }

        #region events
        public void GoNotifiContentTapped(object sender, EventArgs args)
        {
            if (notifiMngPage == null)
            {
                notifiMngPage = new NotifiMngPage((args as ItemTappedEventArgs).Item as NoticeDto);
            }
            if (!Navigation.NavigationStack.Contains(notifiMngPage))
            {
                Navigation.PushAsync(notifiMngPage);
            }
        }

        /*
        public void GoNotifiFeedBTapped(object sender, EventArgs args)
        {
            if (notifiFeedbackPage == null)
            {
                notifiFeedbackPage = new NotifiFeedbackPage((args as ItemTappedEventArgs).Item as NoticeListInfoDto);
            }
            if (!Navigation.NavigationStack.Contains(notifiFeedbackPage))
            {
                Navigation.PushAsync(notifiFeedbackPage);
            }
        }
        */
        #endregion

        #region private method

        private void SetControlRole()
        {
            if (CommonContext.Account.UserType == "S" || CommonContext.Account.UserType == "D")
            {
                lstNotifiFeedB.IsVisible = true;
                lstNotifiContent.IsVisible = false;
                topMenu.IsVisible = false;
                topMenu2.IsVisible = true;
            }
            else
            {
                lstNotifiFeedB.IsVisible = false;
                lstNotifiContent.IsVisible = true;
                topMenu.IsVisible = true;
                topMenu2.IsVisible = false;

            }
        }

        private async void NoticeRegCommand()
        {
            var page = ViewFactory.CreatePage<NotifiMngViewModel, NotifiMngPage>((vm, v) => vm.Init("0", noticeStatus: "Reg")) as Page;

            if (!Navigation.NavigationStack.Contains(page))
            {
                await Navigation.PushAsync(page, true);
            }
        }
        private void NoticeApproalCommand()
        {
            MessagingCenter.Send<string>("", "noticeApproalSearch1");
        }
        private async void NoticeSearchCommand()
        {
            var page = ViewFactory.CreatePage<NotifiMngSearchViewModel, NotifiMngSearchPage>() as Page;

            if (!Navigation.NavigationStack.Contains(page))
            {
                await Navigation.PushAsync(page, true);
            }
        }
        private void NoticeFeedCommand()
        {
            MessagingCenter.Send<string>("", "noticeFeedBackList");
        }

        #endregion
    }
}
