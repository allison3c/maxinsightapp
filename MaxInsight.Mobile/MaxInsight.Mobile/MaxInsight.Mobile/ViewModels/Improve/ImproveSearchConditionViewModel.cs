using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using MaxInsight.Mobile.Module.Dto;
using MaxInsight.Mobile.Module.Dto.Case;
using MaxInsight.Mobile.Pages.Improve;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.Improve
{
    public class ImproveSearchConditionViewModel : ViewModel
    {
        ICommonFun _commonFun;
        public ImproveSearchConditionViewModel()
        {
            try
            {
                _commonFun = Resolver.Resolve<ICommonFun>();
                //MessagingCenter.Unsubscribe<ImpPlanStatusDto>(this, MessageConst.PLANSTATUSLIST_SEND);
                //MessagingCenter.Subscribe<ImpPlanStatusDto>(this, MessageConst.PLANSTATUSLIST_SEND, m =>
                //  {
                //      StatusSelect = m.PName;
                //      ImpPlanStatus = m;
                //      if (ImpPlanStatus!=null)
                //      {
                //          if (ImpPlanStatus.PCode=="A")
                //          {
                //              IsClick = false;
                //          }
                //          else
                //          {
                //              IsClick = true;
                //          }
                //      }
                //  });
                //MessagingCenter.Unsubscribe<ImpResultStatusDto>(this, MessageConst.RESULTSTATUSLIST_SEND);
                //MessagingCenter.Subscribe<ImpResultStatusDto>(this, MessageConst.RESULTSTATUSLIST_SEND, k =>
                //{
                //    StatusSelect = k.RName;
                //    ImpResultStatus = k;
                //    if (ImpResultStatus != null)
                //    {
                //        if (ImpResultStatus.RCode == "A")
                //        {
                //            IsClick = false;
                //        }
                //        else
                //        {
                //            IsClick = true;
                //        }
                //    }
                //});

                MessagingCenter.Unsubscribe<ImpStatusDto>(this, MessageConst.IMPSTATUSLIST_SEND);
                MessagingCenter.Subscribe<ImpStatusDto>(this, MessageConst.IMPSTATUSLIST_SEND, k =>
                {
                    StatusSelect = k.ImpStatusName;
                    ItemSelected = k.StatusKind;
                    ImpStatus = k;
                    if (ImpStatus != null && ItemSelected == 1)
                    {
                        if (ImpStatus.ImpStatusCode == "A")
                        {
                            IsClick = false;
                        }
                        else
                        {
                            IsClick = true;
                        }
                    }
                });

                //所属计划
                MessagingCenter.Unsubscribe<NameValueObject>(this, MessageConst.IMP_TASKOFPLANLIST_SELECT);
                MessagingCenter.Subscribe<NameValueObject>(this, MessageConst.IMP_TASKOFPLANLIST_SELECT, n =>
                {
                    PlanSelectName = n.Name;
                    PlanSelect = n.Value;
                });

                MessagingCenter.Unsubscribe<ServerDto>(this, MessageConst.SERVICERSLIST_SEND);
                MessagingCenter.Subscribe<ServerDto>(this, MessageConst.SERVICERSLIST_SEND, n =>
                {
                    ServicerSelect = n.SName;
                    Server = n;
                });
                MessagingCenter.Unsubscribe<DepartmentDto>(this, MessageConst.DEPARTMENTLIST_SEND);
                MessagingCenter.Subscribe<DepartmentDto>(this, MessageConst.DEPARTMENTLIST_SEND, l =>
                {
                    DepartmentSelect = l.DName;
                    Department = l;
                });

                //来源类型
                MessagingCenter.Unsubscribe<NameValueObject>(this, MessageConst.IMP_SOURCETYPELIST_SELECT);
                MessagingCenter.Subscribe<NameValueObject>(this, MessageConst.IMP_SOURCETYPELIST_SELECT, n =>
                {
                    SourceTypeName = n.Name;
                    SourceType = n.Value;
                });
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ImproveSearchConditionViewModel");
                return;
            }
        }
        #region Property
        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set { SetProperty(ref startDate, value); }
        }
        private DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set { SetProperty(ref endDate, value); }
        }
        private int itemSelected;
        public int ItemSelected
        {
            get
            {
                return itemSelected;
            }
            set
            {
                SetProperty(ref itemSelected, value);
            }
        }
        private string statusSelect;
        public string StatusSelect
        {
            get { return statusSelect; }
            set { SetProperty(ref statusSelect, value); }
        }
        private string itemName;
        public string ItemName
        {
            get { return itemName; }
            set { SetProperty(ref itemName, value); }
        }
        private string departmentSelect;
        public string DepartmentSelect
        {
            get { return departmentSelect; }
            set { SetProperty(ref departmentSelect, value); }
        }
        private string servicerSelect;
        public string ServicerSelect
        {
            get { return servicerSelect; }
            set { SetProperty(ref servicerSelect, value); }
        }
        //private ImpResultStatusDto impResultStatus;
        //public ImpResultStatusDto ImpResultStatus
        //{
        //    get { return impResultStatus; }
        //    set { SetProperty(ref impResultStatus, value); }
        //}
        //private ImpPlanStatusDto impPlanStatus;
        //public ImpPlanStatusDto ImpPlanStatus
        //{
        //    get { return impPlanStatus; }
        //    set { SetProperty(ref impPlanStatus, value); }
        //}

        private ImpStatusDto impStatus;
        public ImpStatusDto ImpStatus
        {
            get { return impStatus; }
            set { SetProperty(ref impStatus, value); }
        }
        private DepartmentDto department;
        public DepartmentDto Department
        {
            get { return department; }
            set { SetProperty(ref department, value); }
        }
        private ServerDto server;
        public ServerDto Server
        {
            get { return server; }
            set { SetProperty(ref server, value); }
        }
        private bool isClick;
        public bool IsClick
        {
            get { return isClick; }
            set { SetProperty(ref isClick, value); }
        }

        //所属计划
        private string _planSelectName;
        public string PlanSelectName
        {
            get { return _planSelectName; }
            set { SetProperty(ref _planSelectName, value); }
        }
        public string PlanSelect { get; set; }

        //来源类型
        private string _sourceTypeName;
        public string SourceTypeName
        {
            get { return _sourceTypeName; }
            set { SetProperty(ref _sourceTypeName, value); }
        }
        public string SourceType { get; set; }
        #endregion
        #region Command
        private RelayCommand passImproveConditionCommand;
        public RelayCommand PassImproveConditionCommand
        {
            get
            {
                return passImproveConditionCommand
                       ?? (passImproveConditionCommand = new RelayCommand(PassImproveCondition));
            }
        }
        #endregion
        #region Event
        private async void PassImproveCondition()
        {
            try
            {
                if (StartDate > EndDate)
                {
                    _commonFun.AlertLongText("开始日期不能大于结束日期");
                    return;
                }
                List<RequestParameter> parameterLst = new List<RequestParameter>();
                parameterLst.Add(new RequestParameter { Name = "StatueType", Value = ItemSelected == 0 ? "A" : "R" });
                parameterLst.Add(new RequestParameter { Name = "StatueTypeName", Value = ItemSelected == 0 ? "计划" : "结果" });
                parameterLst.Add(new RequestParameter { Name = "StartDate", Value = StartDate.ToString("yyyy-MM-dd") });
                parameterLst.Add(new RequestParameter { Name = "EndDate", Value = EndDate.ToString("yyyy-MM-dd") });
                //parameterLst.Add(new RequestParameter { Name = "Statue", Value =ItemSelected==0?(ImpPlanStatus==null?"": ImpPlanStatus.PCode):(ImpResultStatus==null?"":ImpResultStatus.RCode) });
                parameterLst.Add(new RequestParameter { Name = "Statue", Value = ImpStatus == null ? "" : ImpStatus.ImpStatusCode });
                parameterLst.Add(new RequestParameter { Name = "StatueName", Value = StatusSelect });
                parameterLst.Add(new RequestParameter { Name = "ItemName", Value = ItemName });
                parameterLst.Add(new RequestParameter { Name = "PlanSelectName", Value = PlanSelectName });
                parameterLst.Add(new RequestParameter { Name = "PlanSelect", Value = PlanSelect });
                parameterLst.Add(new RequestParameter { Name = "SourceTypeName", Value = SourceTypeName });
                parameterLst.Add(new RequestParameter { Name = "SourceType", Value = SourceType });
                if (CommonContext.Account.UserType == "D")
                {
                    parameterLst.Add(new RequestParameter { Name = "DepartmentId", Value = CommonContext.Account.OrgDepartmentId });
                    parameterLst.Add(new RequestParameter { Name = "ServiceId", Value = CommonContext.Account.OrgServerId });
                    parameterLst.Add(new RequestParameter { Name = "DepartmentName", Value = CommonContext.Account.OrgDepartmentName });
                    parameterLst.Add(new RequestParameter { Name = "ServiceName", Value = CommonContext.Account.OrgServerName });
                }
                else if (CommonContext.Account.UserType == "S")
                {
                    parameterLst.Add(new RequestParameter { Name = "DepartmentId", Value = Department == null ? "0" : Department.DId });
                    parameterLst.Add(new RequestParameter { Name = "ServiceId", Value = CommonContext.Account.OrgServerId });
                    parameterLst.Add(new RequestParameter { Name = "DepartmentName", Value = Department == null ? "全部" : Department.DName });
                    parameterLst.Add(new RequestParameter { Name = "ServiceName", Value = CommonContext.Account.OrgServerName });

                }
                else
                {
                    parameterLst.Add(new RequestParameter { Name = "DepartmentId", Value = Department == null ? "0" : Department.DId });
                    parameterLst.Add(new RequestParameter { Name = "ServiceId", Value = Server == null ? "0" : Server.SId });
                    parameterLst.Add(new RequestParameter { Name = "DepartmentName", Value = Department == null ? "全部" : Department.DName });
                    parameterLst.Add(new RequestParameter { Name = "ServiceName", Value = Server == null ? "全部" : Server.SName });
                }
                await Navigation.PopAsync();
                MessagingCenter.Send<List<RequestParameter>>(parameterLst, MessageConst.PASS_IMPROVESEARCHCONDITION);
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ImproveSearchConditionViewModel");
                return;
            }
        }
        #endregion
        #region Init
        public void Init()
        {
            try
            {
                DateTime now = DateTime.Now;
                StartDate = new DateTime(now.Year, now.Month, 1);
                EndDate = DateTime.Now;
                StatusSelect = "全部";
                ServicerSelect = "全部";
                DepartmentSelect = "全部";
                //ImpPlanStatus = null;
                //ImpResultStatus = null;
                ImpStatus = null;
                Department = null;
                Server = null;
                IsClick = true;
                //所属计划
                PlanSelectName = "全部";
                PlanSelect = "0";
                //来源类型
                SourceTypeName = "全部";
                SourceType = "";

                ItemSelected = 0;
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ImproveSearchConditionViewModel");
                return;
            }
        }
        #endregion
    }
}
