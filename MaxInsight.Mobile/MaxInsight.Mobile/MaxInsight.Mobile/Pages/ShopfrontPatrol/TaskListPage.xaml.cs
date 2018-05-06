using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MaxInsight.Mobile
{
    public partial class TaskListPage : ContentPage
    {
        ToolbarItem tbUploadData;
        ToolbarItem tbCustImprove;
        string fromSeachYN = string.Empty; //标记是从查询条件页面跳转过来的，还是从经销商列表中跳转过来的。
        public TaskListPage()
        {
            InitializeComponent();
            Title = "任务列表";
            MessagingCenter.Subscribe<string>(this, "ComeFromSeachYN", (param) =>
            {
                fromSeachYN = param;
                if (CommonContext.Account.UserType == "Z" && string.IsNullOrWhiteSpace(fromSeachYN))
                {
                    if (tbUploadData == null)
                    {
                        tbUploadData = new ToolbarItem();
                        tbUploadData.Text = "数据上传";
                        tbUploadData.Command = (this.BindingContext as TaskListViewModel).UploadScoreImageCommand;
                    }
                    if (!this.ToolbarItems.Contains(tbUploadData))
                    {
                        this.ToolbarItems.Add(tbUploadData);
                    }
                    if (tbCustImprove == null)
                    {
                        tbCustImprove = new ToolbarItem();
                        tbCustImprove.Text = "改善登记";
                        tbCustImprove.Command = (this.BindingContext as TaskListViewModel).AddCustImproveCommand;
                    }
                    if (!this.ToolbarItems.Contains(tbCustImprove))
                    {
                        this.ToolbarItems.Add(tbCustImprove);
                    }
                }
                else
                {
                    if (this.ToolbarItems.Contains(tbUploadData))
                    {
                        this.ToolbarItems.Remove(tbUploadData);
                    }
                    if (this.ToolbarItems.Contains(tbCustImprove))
                    {
                        this.ToolbarItems.Remove(tbCustImprove);
                    }
                }
            });
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event
                                   //Debug.WriteLine("Tapped: " + e.Item);
                                   //((ListView)sender).SelectedItem = null; // de-select the row
            var item = e.Item as TaskOfPlanDto;

            MessagingCenter.Send<TaskOfPlanDto>(item, "CheckTask");
            if (fromSeachYN == "")
            {
                MessagingCenter.Send<string>("", "TaskSearchParam");
            }
            else
            {
                MessagingCenter.Send<string>("search", "TaskSearchParam");
            }

        }

        protected override void OnAppearing()
        {
            //MessagingCenter.Send<TaskListPage>(this, "RefreshTask");
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            //MessagingCenter.Send<CommonMessage>(new CommonMessage() { TaskID = 0 }, "ResetTaskID");
            base.OnDisappearing();
        }
    }
}
