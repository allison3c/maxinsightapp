using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto;
using MaxInsight.Mobile.Module.Dto.Case;
using MaxInsight.Mobile.Module.Dto.Notifi;
using MaxInsight.Mobile.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.Notifi
{
    public class NotifiMngSearchConditionViewModel : ViewModel
    {
        ICommonFun _commonFun;
        public NotifiMngSearchConditionViewModel()
        {
            StatusSelectName = "全部";
            StatusSelectIndex = "";
            ReplySelected = 0;
            DateTime now = DateTime.Now;
            StartDate = new DateTime(now.Year,now.Month,1);
            EndDate = DateTime.Now;
            NoticeSelectDis = "全部";
            NoticeColorDis = StaticColor.ContentFontColor;
            DisSelectList = new List<MultiSelectDto>();
            NoticeSelectDep = "请选择";
            NoticeColorDep = StaticColor.ContentFontColor;
            DepSelectList = new List<MultiSelectDto>();
            NoticeTitle = "";
            NoticeNo = "";
            _commonFun = Resolver.Resolve<ICommonFun>();
            MessagingCenter.Unsubscribe<NameValueObject>(this, MessageConst.NOTICE_STATUSLIST_SELECT);
            MessagingCenter.Subscribe<NameValueObject>(this, MessageConst.NOTICE_STATUSLIST_SELECT, statusItem =>
            {
                StatusSelectName= statusItem.Name;
                StatusSelectIndex = statusItem.Value;
            });
            MessagingCenter.Unsubscribe <List<MultiSelectDto>> (this, MessageConst.NOTICE_DISTRIBUTOR_SET_LIST);
            MessagingCenter.Subscribe<List<MultiSelectDto>>(
             this,
             MessageConst.NOTICE_DISTRIBUTOR_SET_LIST,
             (paramList) =>
             {
                 SetDisSelectList(paramList);
             });
            MessagingCenter.Unsubscribe<List<MultiSelectDto>>(this, MessageConst.NOTICE_DEPARTMENT_SET_LIST);
            MessagingCenter.Subscribe<List<MultiSelectDto>>(this, MessageConst.NOTICE_DEPARTMENT_SET_LIST, paramList =>
            {
                SetDepSelectList(paramList);
            });
        }
        #region Property
        #region 通知状态
        private string _statusSelectName;
        public string StatusSelectName
        {
            get { return _statusSelectName; }
            set { SetProperty(ref _statusSelectName, value); }
        }
        private string _statusSelectIndex;
        public string StatusSelectIndex
        {
            get { return _statusSelectIndex; }
            set { SetProperty(ref _statusSelectIndex, value); }
        }
        #endregion
        #region 反馈与否
        public int ReplySelected { get; set; } = 0;
        #endregion
        #region 查询期间
        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }
        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set { SetProperty(ref _endDate, value); }
        }
        #endregion
        #region 通知对象
        private string _noticeSelectDis;
        public string NoticeSelectDis
        {
            get { return _noticeSelectDis; }
            set { SetProperty(ref _noticeSelectDis, value); }
        }

        private Color _noticeColorDis;
        public Color NoticeColorDis
        {
            get { return _noticeColorDis; }
            set { SetProperty(ref _noticeColorDis, value); }
        }
        private List<MultiSelectDto> _disSelectList;
        public List<MultiSelectDto> DisSelectList
        {
            get { return _disSelectList; }
            set { SetProperty(ref _disSelectList, value); }
        }

        private string _noticeSelectDep;
        public string NoticeSelectDep
        {
            get { return _noticeSelectDep; }
            set { SetProperty(ref _noticeSelectDep, value); }
        }
        private Color _noticeColorDep;
        public Color NoticeColorDep
        {
            get { return _noticeColorDep; }
            set { SetProperty(ref _noticeColorDep, value); }
        }
        private List<MultiSelectDto> _depSelectList;
        public List<MultiSelectDto> DepSelectList
        {
            get { return _depSelectList; }
            set { SetProperty(ref _depSelectList, value); }
        }
        #endregion
        #region 通知标题
        private string _noticeTitle;
        public string NoticeTitle
        {
            get { return _noticeTitle; }
            set { SetProperty(ref _noticeTitle, value); }
        }
        #endregion
        #region 通知编号
        private string _noticeNo;
        public string NoticeNo
        {
            get { return _noticeNo; }
            set { SetProperty(ref _noticeNo, value); }
        }
        #endregion
        #endregion
        #region Command
        private RelayCommand _passNoticeConditionCommand;
        public RelayCommand PassNoticeConditionCommand
        {
            get
            {
                return _passNoticeConditionCommand
                       ?? (_passNoticeConditionCommand = new RelayCommand(PassNoticeCondition));
            }
        }
        #endregion
        #region Event
        private async void PassNoticeCondition()
        {
            if (Convert.ToDateTime(StartDate.ToString("yyyy-MM-dd")) > Convert.ToDateTime(EndDate.ToString("yyyy-MM-dd")))
            {
                _commonFun.AlertLongText("开始日期不能大于结束日期");
                return;
            }
            List<RequestParameter> parameterLst = new List<RequestParameter>();
            parameterLst.Add(new RequestParameter { Name = "StatusSelectIndex", Value = StatusSelectIndex});
            parameterLst.Add(new RequestParameter { Name = "ReplySelected", Value = ReplySelected==0?"1":"0" });
            parameterLst.Add(new RequestParameter { Name = "ReplySelectedName", Value = ReplySelected==0?"是":"否" });
            parameterLst.Add(new RequestParameter { Name = "StartDate", Value = StartDate.ToString("yyyy-MM-dd") });
            parameterLst.Add(new RequestParameter { Name = "EndDate", Value = EndDate.ToString("yyyy-MM-dd") });
            parameterLst.Add(new RequestParameter { Name = "NoticeTitle", Value = NoticeTitle});
            parameterLst.Add(new RequestParameter { Name = "NoticeNo", Value = NoticeNo});
            parameterLst.Add(new RequestParameter { Name = "StatusSelectName", Value = StatusSelectName });
            if (CommonContext.Account.UserType == "D")
            {
                parameterLst.Add(new RequestParameter { Name = "NoticeReaders", Value = CommonContext.Account.OrgServerId + "|"+ CommonContext.Account.OrgDepartmentId });
                parameterLst.Add(new RequestParameter { Name = "NoticeReaderDes", Value = CommonContext.Account.OrgServerName +">"+ CommonContext.Account.OrgDepartmentName });
            }
            else if (CommonContext.Account.UserType == "S")
            {
                parameterLst.Add(new RequestParameter { Name = "ServiceId", Value = CommonContext.Account.OrgServerId });
                parameterLst.Add(new RequestParameter { Name = "ServiceName", Value = CommonContext.Account.OrgServerName });
                string noticeReaders ="";
                if (DepSelectList == null || DepSelectList.Count == 0)
                {
                    parameterLst.Add(new RequestParameter { Name = "NoticeReaders", Value = CommonContext.Account.OrgServerId + "|0" });
                    parameterLst.Add(new RequestParameter { Name = "NoticeReaderDes", Value = CommonContext.Account.OrgServerName});
                }
                else
                {
                    foreach (MultiSelectDto item in DepSelectList)
                        noticeReaders = noticeReaders+ CommonContext.Account.OrgServerId + "|" + item.DisCode + ",";
                    parameterLst.Add(new RequestParameter { Name = "NoticeReaders", Value = noticeReaders.Remove(noticeReaders.Length - 1) });
                    parameterLst.Add(new RequestParameter { Name = "NoticeReaderDes", Value = CommonContext.Account.OrgServerName + ">" + "共选择" + DepSelectList.Count + "个部门" });
                }
            }
            else
            {
                if ((DisSelectList == null || DisSelectList.Count == 0) && (DepSelectList == null || DepSelectList.Count == 0))
                {
                    parameterLst.Add(new RequestParameter { Name = "NoticeReaders", Value = "" });
                    parameterLst.Add(new RequestParameter { Name = "NoticeReaderDes", Value = "选择了所有经销商" });

                }
                else if ((DisSelectList == null || DisSelectList.Count == 0) && DepSelectList != null && DepSelectList.Count > 0)
                {
                    List<ServerDto> serverList = new List<ServerDto>();
                    foreach (var item in CommonContext.Account.ZionList[0].AreaList)
                    {
                        serverList.AddRange(item.ServerList);
                    }
                    foreach (ServerDto item in serverList)
                        DisSelectList.Add(new MultiSelectDto { DisCode = item.SId, DisName = item.SName, IsChecked = true });
                    parameterLst.Add(new RequestParameter { Name = "NoticeReaders", Value = CommonUtil.DisAndDepToString(DisSelectList, DepSelectList) });
                    parameterLst.Add(new RequestParameter { Name = "NoticeReaderDes", Value = "选择了所有经销商" + ">" + "共选择" + DisSelectList.Count * DepSelectList.Count + "个部门" });
                }
                else if (DisSelectList != null && DisSelectList.Count >0 && (DepSelectList == null || DepSelectList.Count ==0))
                {
                    string readerList = "";
                    foreach (MultiSelectDto disDto in DisSelectList)
                        readerList = readerList + disDto.DisCode + "|0,";
                    parameterLst.Add(new RequestParameter { Name = "NoticeReaders", Value = readerList.Remove(readerList.Length - 1) });
                    parameterLst.Add(new RequestParameter { Name = "NoticeReaderDes", Value = "共选择" + DisSelectList.Count + "个经销商"});
                }
                else
                {
                    parameterLst.Add(new RequestParameter { Name = "NoticeReaders", Value = CommonUtil.DisAndDepToString(DisSelectList, DepSelectList) });
                    parameterLst.Add(new RequestParameter { Name = "NoticeReaderDes", Value = "共选择" + DisSelectList.Count + "个经销商" + ">" + "共选择" + DisSelectList.Count * DepSelectList.Count + "个部门" });
                }
            }
            await Navigation.PopAsync();
            MessagingCenter.Send<List<RequestParameter>>(parameterLst, MessageConst.NOTIFI_SEARCHCONDITION_PASS);
        }
        #endregion

        #region Method
        private void SetDisSelectList(List<MultiSelectDto> paramList)
        {
            DisSelectList = paramList;
            List<MultiSelectDto> selectItems = paramList.FindAll(item => item.IsChecked);
            if (selectItems != null && selectItems.Count > 0)
            {
                NoticeSelectDis = string.Format("共选择{0}项", selectItems.Count);
                NoticeColorDis = Color.FromHex("3998C0");//"#3998c0"
            }
            else
            {
                NoticeSelectDis = "全部";
                NoticeColorDis = StaticColor.ContentFontColor;
            }
        }
        private void SetDepSelectList(List<MultiSelectDto> paramList)
        {
            DepSelectList = paramList;
            List<MultiSelectDto> selectItems = paramList.FindAll(item => item.IsChecked);
            if (selectItems != null && selectItems.Count > 0)
            {
                NoticeSelectDep = string.Format("共选择{0}项", selectItems.Count);
                NoticeColorDep = Color.FromHex("3998C0");//"#3998c0"
            }
            else
            {
                NoticeSelectDep = "请选择";
                NoticeColorDep = StaticColor.ContentFontColor;
            }
        }
        #endregion

        #region page init
        public void Init()
        {
            StatusSelectName = "全部";
            StatusSelectIndex = "";
            ReplySelected = 0;
            DateTime now = DateTime.Now;
            StartDate = new DateTime(now.Year, now.Month, 1);
            EndDate = DateTime.Now;
            NoticeSelectDis = "全部";
            NoticeColorDis = StaticColor.ContentFontColor;
            DisSelectList = new List<MultiSelectDto>();
            NoticeSelectDep = "请选择";
            NoticeColorDep = StaticColor.ContentFontColor;
            DepSelectList = new List<MultiSelectDto>();
            NoticeTitle = "";
            NoticeNo = "";
            MessagingCenter.Send<List<MultiSelectDto>>(new List<MultiSelectDto>(), MessageConst.NOTICE_DISTRIBUTOR_SHOW_LIST);
            MessagingCenter.Send<List<MultiSelectDto>>(new List<MultiSelectDto>(), MessageConst.NOTICE_DEPARTMENT_SHOW_LIST);
        }

        #endregion
    }
}
