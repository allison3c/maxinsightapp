﻿using MaxInsight.Mobile.Module.Dto.Notifi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace MaxInsight.Mobile.Pages.Notifi
{
    public partial class NotifiFeedbackPage : ContentPage
    {
        public NotifiFeedbackPage()
        {
            InitializeComponent();
            //文件上传在IOS上不显示
            if (Device.OS == TargetPlatform.iOS)
            {
                btnAttachUploadFile.IsVisible = false;
            }
        }
        public NotifiFeedbackPage(NoticeListInfoDto noticeDto)
        {
            InitializeComponent();
        }
        public void OnDeleteCaseAttechment(object sender, EventArgs e)
        {
            ImageButton img = sender as ImageButton;
            MessagingCenter.Send<string>(img.CommandParameter.ToString(), MessageConst.CASEATTACH_DELETE);
        }
    }
}
