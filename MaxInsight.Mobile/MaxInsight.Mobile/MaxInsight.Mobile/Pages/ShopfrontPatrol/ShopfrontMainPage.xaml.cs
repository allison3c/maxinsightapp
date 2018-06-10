using System;
using System.Linq;
using MaxInsight.Mobile.Helpers;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile
{
    public partial class ShopfrontMainPage : ContentPage
    {
        //ViewRecordPage viewRecordPage = null;
        CommonHelper _commonHelper;

        ToolbarItem tbDownLoad;
        public ShopfrontMainPage()
        {
            InitializeComponent();
            try
            {
                _commonHelper = Resolver.Resolve<CommonHelper>();

                if (CommonContext.Account.UserType == "S") //区域:Z  服务商:S
                {
                    stackRecord.IsVisible = false;
                    listView.IsVisible = false;
                    stackViewRecord.HorizontalOptions = LayoutOptions.StartAndExpand;
                    stackImproveDistri.IsVisible = true;
                    Title = "详情查看";
                }
                else
                {
                    Title = "巡视检核";
                }

                stackRecord.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    NumberOfTapsRequired = 1,
                    Command = new Command(RecordCommand)
                });

                stackViewRecord.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    NumberOfTapsRequired = 1,
                    Command = new Command(ViewRecordCommand)
                });

                stackImproveDistri.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    NumberOfTapsRequired = 1,
                    Command = new Command(GetImproveDitstriLstCommand)
                });
            }
            catch (Exception)
            {
            }
        }

        private void RecordCommand()
        {
            try
            {
                MessagingCenter.Send<ShopfrontMainPage>(this, "GetShops");
            }
            catch (Exception)
            {
            }
        }

        private async void ViewRecordCommand()
        {
            try
            {
                if (!_commonHelper.IsNetWorkConnected())
                {
                    await DisplayAlert("温馨提示", "请在有网络时访问。", "确定");
                    return;
                }
                var viewRecordPage = ViewFactory.CreatePage<ViewRecordViewModel, ViewRecordPage>() as Page;

                if (!Navigation.NavigationStack.Contains(viewRecordPage))
                {
                    await Navigation.PushAsync(viewRecordPage, true);
                    MessagingCenter.Send<string>("", "InitParameter");
                }
            }
            catch (Exception)
            {
            }
        }

        private void GetImproveDitstriLstCommand()
        {
            try
            {
                MessagingCenter.Send<string>("", "GetImproveDitstriLst");
            }
            catch (Exception)
            {
            }
        }

        protected override void OnAppearing()
        {
            //var page = Xamarin.Forms.Application.Current.MainPage;

            ////只会在进入此页面查询，返回此页面时不再做查询，如果需要，把下面的注释打开即可
            //if ((page is NavigationPage) && ((page as NavigationPage).CurrentPage.ToString() 
            //                                 == "MaxInsight.Mobile.ShopfrontMainPage"))
            //                                 //|| (page as NavigationPage).CurrentPage.ToString()
            //                                 //== "MaxInsight.Mobile.TaskListPage"))
            //{
            //	MessagingCenter.Send<ShopfrontMainPage>(this, "GetShops");
            //}
            base.OnAppearing();
            try
            {
                if (CommonContext.Account.UserType == "Z")
                {
                    if (tbDownLoad == null)
                    {
                        tbDownLoad = new ToolbarItem();
                        tbDownLoad.Text = "刷新";
                        tbDownLoad.Command = (this.BindingContext as ShopfrontMainPageViewModel).DownLoadTaskCommand;
                    }
                    if (!this.ToolbarItems.Contains(tbDownLoad))
                    {
                        this.ToolbarItems.Add(tbDownLoad);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        protected override void OnDisappearing()
        {
            //viewRecordPage = null;
            base.OnDisappearing();
        }

        async void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            try
            {
                if (e == null)
                {
                    return;
                }
                var item = e.Item as TourDistributorDto;

                await Navigation.PushAsync(ViewFactory.CreatePage<TaskListViewModel, TaskListPage>() as Page, true);
                MessagingCenter.Send<string>("", "ComeFromSeachYN");
                MessagingCenter.Send<TourDistributorDto>(item, "SendShopItem");
            }
            catch (Exception)
            {
            }
        }
    }
}
