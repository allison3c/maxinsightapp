
using MaxInsight.Mobile.ViewModels.Improve;
using System;
using System.Linq;
using Xamarin.Forms;
using XLabs.Enums;
using XLabs.Forms.Controls;
using XLabs.Forms.Mvvm;

namespace MaxInsight.Mobile.Pages.Improve
{
    public partial class ImproveIndexPage : ContentPage
    {
        public ImproveIndexPage()
        {
            InitializeComponent();
            try
            {
                CreateGrid();
                //Init();
                MessagingCenter.Send<string>("R", MessageConst.IMPROVE_PLANLSTDATA_GET);
            }
            catch (Exception)
            {
            }
        }

        //private void Init()
        //{
        //    //improvePlanLst.IsVisible = false;
        //    //improveResultLst.IsVisible = true;
        //}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //MessagingCenter.Subscribe<string>(
            //   this,
            //   MessageConst.IMPROVE_PLANLST_SHOW,
            //   (c) =>
            //   {
            //       //improvePlanLst.IsVisible = true;
            //       //improveResultLst.IsVisible = false;
            //   });
            //MessagingCenter.Subscribe<string>(
            //    this,
            //    MessageConst.IMPROVE_RESULTLST_SHOW,
            //    (c) =>
            //    {
            //        //improvePlanLst.IsVisible = false;
            //        //improveResultLst.IsVisible = true;
            //    });
        }

        protected override void OnDisappearing()
        {

            base.OnDisappearing();
            //MessagingCenter.Unsubscribe<string>(this, MessageConst.IMPROVE_PLANLST_SHOW);
            //MessagingCenter.Unsubscribe<string>(this, MessageConst.IMPROVE_RESULTLST_SHOW);
        }

        Label resultLabel;
        Label planLabel;
        Label resultCommitLable;
        Label planCommitLable;
        private void CreateGrid()
        {
            try
            {
                if (CommonContext.Account.UserType == "S" || CommonContext.Account.UserType == "Z" || CommonContext.Account.UserType == "R")
                {
                    var resultStack = new StackLayout()
                    {
                        Orientation = StackOrientation.Vertical,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.Center
                        //,
                        //Padding = new Thickness(0, 5, 0, 0),
                        //Margin = new Thickness(30, 0, 0, 0)
                    };

                    var improveResultApprovalSearchButton = new Image()
                    {
                        Source = ImageSource.FromFile("Improveresultcommit"),
                        BackgroundColor = Color.Transparent,
                        HorizontalOptions = LayoutOptions.Center,
                        //TextColor = Color.FromHex("#FFFFFF"),
                        WidthRequest = 50,
                        HeightRequest = 50
                    };
                    resultLabel = new Label()
                    {
                        Text = "结果审批",
                        HorizontalOptions = LayoutOptions.Center,
                        TextColor = StaticColor.ContentWhite,
                        FontSize = 17,
                        FontAttributes = FontAttributes.Bold
                    };

                    resultStack.Children.Add(improveResultApprovalSearchButton);
                    resultStack.Children.Add(resultLabel);

                    resultStack.GestureRecognizers.Add(new TapGestureRecognizer()
                    {
                        NumberOfTapsRequired = 1,
                        Command = new Command(ImpResultCommand)
                    });

                    topStack.Children.Add(resultStack);


                    var planStack = new StackLayout()
                    {
                        Orientation = StackOrientation.Vertical,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.Center
                        //,
                        //Padding = new Thickness(0, 5, 0, 0)
                    };

                    var improvePlanApprovalSearchButton = new Image()
                    {
                        Source = ImageSource.FromFile("Improveplanapply"),
                        BackgroundColor = Color.Transparent,
                        HorizontalOptions = LayoutOptions.Center,
                        //TextColor = Color.FromHex("#FFFFFF"),
                        WidthRequest = 50,
                        HeightRequest = 50
                    };
                    planLabel = new Label()
                    {
                        Text = "计划审批",
                        HorizontalOptions = LayoutOptions.Center,
                        TextColor = StaticColor.ContentWhite,
                        FontSize = 15
                    };

                    planStack.Children.Add(improvePlanApprovalSearchButton);
                    planStack.Children.Add(planLabel);

                    planStack.GestureRecognizers.Add(new TapGestureRecognizer()
                    {
                        NumberOfTapsRequired = 1,
                        Command = new Command(ImpPlanCommand)
                    });
                    topStack.Children.Add(planStack);

                    var searchStack = new StackLayout()
                    {
                        Orientation = StackOrientation.Vertical,
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        VerticalOptions = LayoutOptions.Center
                        //,
                        //Padding = new Thickness(0, 5, 0, 0),
                        //Margin = new Thickness(0, 0, 30, 0)
                    };
                    var improveSearchButton = new Image()
                    {
                        Source = ImageSource.FromFile("ImproveSearch"),
                        BackgroundColor = Color.Transparent,
                        //TextColor = Color.FromHex("#FFFFFF"),
                        HorizontalOptions = LayoutOptions.Center,
                        WidthRequest = 50,
                        HeightRequest = 50
                    };
                    var searchLabel = new Label()
                    {
                        Text = "进度查看",
                        HorizontalOptions = LayoutOptions.Center,
                        TextColor = StaticColor.ContentWhite,
                        FontSize = 15
                    };

                    searchStack.Children.Add(improveSearchButton);
                    searchStack.Children.Add(searchLabel);
                    searchStack.GestureRecognizers.Add(new TapGestureRecognizer()
                    {
                        NumberOfTapsRequired = 1,
                        Command = new Command(ImpSearchCommand)
                    });

                    topStack.Children.Add(searchStack);

                    //gridImprove.Children.Add(improveSearchButton,2,0);
                    //gridImprove.Children.Add(improvePlanApprovalSearchButton, 1, 0);
                    //gridImprove.Children.Add(improveResultApprovalSearchButton, 0, 0);
                }
                else
                {
                    var resultStack = new StackLayout()
                    {
                        Orientation = StackOrientation.Vertical,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.Center
                        //,
                        //Padding = new Thickness(0, 5, 0, 0),
                        //Margin = new Thickness(30, 0, 0, 0)
                    };
                    resultCommitLable = new Label()
                    {
                        Text = "结果提交",
                        HorizontalOptions = LayoutOptions.Center,
                        TextColor = StaticColor.ContentWhite,
                        FontSize = 17,
                        FontAttributes = FontAttributes.Bold
                    };
                    var improveResultSearchButton = new Image()
                    {
                        Source = ImageSource.FromFile("ImproveDispatch"),
                        BackgroundColor = Color.Transparent,
                        //TextColor = Color.FromHex("#FFFFFF"),
                        HorizontalOptions = LayoutOptions.Center,
                        WidthRequest = 50,
                        HeightRequest = 50
                    };
                    resultStack.Children.Add(improveResultSearchButton);
                    resultStack.Children.Add(resultCommitLable);
                    resultStack.GestureRecognizers.Add(new TapGestureRecognizer()
                    {
                        NumberOfTapsRequired = 1,
                        Command = new Command(ImpResultCommand)
                    });

                    topStack.Children.Add(resultStack);


                    var planStack = new StackLayout()
                    {
                        Orientation = StackOrientation.Vertical,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.Center
                        //,
                        //Padding = new Thickness(0, 5, 0, 0)
                    };
                    planCommitLable = new Label()
                    {
                        Text = "计划提交",
                        HorizontalOptions = LayoutOptions.Center,
                        TextColor = StaticColor.ContentWhite,
                        FontSize = 15
                    };

                    var improvePlanSearchButton = new Image()
                    {
                        Source = ImageSource.FromFile("Improveplanmake"),
                        BackgroundColor = Color.Transparent,
                        //TextColor = Color.FromHex("#FFFFFF"),
                        HorizontalOptions = LayoutOptions.Center,
                        WidthRequest = 50,
                        HeightRequest = 50
                    };
                    planStack.Children.Add(improvePlanSearchButton);
                    planStack.Children.Add(planCommitLable);

                    planStack.GestureRecognizers.Add(new TapGestureRecognizer()
                    {
                        NumberOfTapsRequired = 1,
                        Command = new Command(ImpPlanCommand)
                    });

                    topStack.Children.Add(planStack);

                    var searchStack = new StackLayout()
                    {
                        Orientation = StackOrientation.Vertical,
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        VerticalOptions = LayoutOptions.Center
                        //,
                        //Padding = new Thickness(0, 5, 0, 0),
                        //Margin = new Thickness(0, 0, 30, 0)
                    };
                    var searchLable = new Label()
                    {
                        Text = "进度查看",
                        HorizontalOptions = LayoutOptions.Center,
                        TextColor = StaticColor.ContentWhite,
                        FontSize = 15
                    };

                    var improveSearchButton = new Image()
                    {
                        Source = ImageSource.FromFile("ImproveSearch"),
                        BackgroundColor = Color.Transparent,
                        //TextColor = Color.FromHex("#FFFFFF"),
                        HorizontalOptions = LayoutOptions.Center,
                        WidthRequest = 50,
                        HeightRequest = 50
                    };
                    searchStack.Children.Add(improveSearchButton);
                    searchStack.Children.Add(searchLable);

                    searchStack.GestureRecognizers.Add(new TapGestureRecognizer()
                    {
                        NumberOfTapsRequired = 1,
                        Command = new Command(ImpSearchCommand)
                    });


                    topStack.Children.Add(searchStack);

                    //gridImprove.Children.Add(improveResultSearchButton, 0, 0);
                    //gridImprove.Children.Add(improvePlanSearchButton, 1, 0);
                    //gridImprove.Children.Add(improveSearchButton, 2, 0);
                }
            }
            catch (Exception)
            {
            }
        }

        #region command
        private void ImpResultCommand()
        {
            try
            {
                if (CommonContext.Account.UserType == "S" || CommonContext.Account.UserType == "Z" || CommonContext.Account.UserType == "R")
                {
                    //resultLabel.TextColor = Color.FromHex("#A0A0A0");
                    //planLabel.TextColor = StaticColor.ContentWhite;
                    resultLabel.FontAttributes = FontAttributes.Bold;
                    planLabel.FontAttributes = FontAttributes.None;
                    resultLabel.FontSize = 17;
                    planLabel.FontSize = 15;
                }
                else
                {
                    //planCommitLable.TextColor = StaticColor.ContentWhite;
                    //resultCommitLable.TextColor = Color.FromHex("#A0A0A0");
                    planCommitLable.FontAttributes = FontAttributes.None;
                    resultCommitLable.FontAttributes = FontAttributes.Bold;
                    planCommitLable.FontSize = 15;
                    resultCommitLable.FontSize = 17;
                }
                MessagingCenter.Send<string>("R", MessageConst.IMPROVE_PLANLSTDATA_GET);
            }
            catch (Exception)
            {
            }
        }
        private void ImpPlanCommand()
        {
            try
            {
                if (CommonContext.Account.UserType == "S" || CommonContext.Account.UserType == "Z" || CommonContext.Account.UserType == "R")
                {
                    //planLabel.TextColor = Color.FromHex("#A0A0A0");
                    //resultLabel.TextColor = StaticColor.ContentWhite;
                    planLabel.FontAttributes = FontAttributes.Bold;
                    resultLabel.FontAttributes = FontAttributes.None;
                    planLabel.FontSize = 17;
                    resultLabel.FontSize = 15;
                }
                else
                {
                    //planCommitLable.TextColor = Color.FromHex("#A0A0A0");
                    //resultCommitLable.TextColor = StaticColor.ContentWhite;
                    planCommitLable.FontAttributes = FontAttributes.Bold;
                    resultCommitLable.FontAttributes = FontAttributes.None;
                    planCommitLable.FontSize = 17;
                    resultCommitLable.FontSize = 15;
                }
                MessagingCenter.Send<string>("A", MessageConst.IMPROVE_PLANLSTDATA_GET);
            }
            catch (Exception)
            {
            }
        }
        private async void ImpSearchCommand()
        {
            try
            {
                var page = ViewFactory.CreatePage<ImproveSearchViewModel, ImproveSearchPage>() as Page;
                if (!Navigation.NavigationStack.Contains(page))
                {
                    await Navigation.PushAsync(page, true);
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}
