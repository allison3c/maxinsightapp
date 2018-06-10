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
            try
            {
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
            catch (Exception)
            {
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                if (CommonContext.Account.UserType == "S" || CommonContext.Account.UserType == "D")
                {
                    MessagingCenter.Send<NotifiIndexPage>(this, MessageConst.NOTICE_FEEDBDATA_GET);
                }
                else
                {
                    //MessagingCenter.Send<string>("", MessageConst.NOTICE_APPROALDATA_GET);
                }
            }
            catch (Exception)
            {
            }
        }

        #region events
        public void GoNotifiContentTapped(object sender, EventArgs args)
        {
            try
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
            catch (Exception)
            {
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
            try
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
            catch (Exception)
            {
            }
        }

        private async void NoticeRegCommand()
        {
            try
            {
                var page = ViewFactory.CreatePage<NotifiMngViewModel, NotifiMngPage>((vm, v) => vm.Init("0", noticeStatus: "Reg")) as Page;

                if (!Navigation.NavigationStack.Contains(page))
                {
                    await Navigation.PushAsync(page, true);
                }
            }
            catch (Exception)
            {
            }
        }
        private void NoticeApproalCommand()
        {
            try
            {
                MessagingCenter.Send<string>("", "noticeApproalSearch1");
            }
            catch (Exception)
            {
            }
        }
        private async void NoticeSearchCommand()
        {
            try
            {
                var page = ViewFactory.CreatePage<NotifiMngSearchViewModel, NotifiMngSearchPage>() as Page;

                if (!Navigation.NavigationStack.Contains(page))
                {
                    await Navigation.PushAsync(page, true);
                }
            }
            catch (Exception)
            {
            }
        }
        private void NoticeFeedCommand()
        {
            try
            {
                MessagingCenter.Send<string>("", "noticeFeedBackList");
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}
