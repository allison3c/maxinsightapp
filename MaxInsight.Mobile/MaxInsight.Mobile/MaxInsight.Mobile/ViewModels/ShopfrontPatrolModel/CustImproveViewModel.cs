using MaxInsight.Mobile.Domain;
using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module.Dto.Case;
using MaxInsight.Mobile.Module.Dto.Shops;
using MaxInsight.Mobile.Services.RemoteService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.ShopfrontPatrolModel
{
    public class CustImproveViewModel : ViewModel
    {
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        ILocalScoreService _localScoreService;

        private Dictionary<int, string> _dIcPlans = new Dictionary<int, string>();
        private Dictionary<string, string> _dIcCardType = new Dictionary<string, string>();


        #region Constructor
        public CustImproveViewModel()
        {
            _commonFun = Resolver.Resolve<ICommonFun>();
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _localScoreService = Resolver.Resolve<ILocalScoreService>();

            Device.BeginInvokeOnMainThread(async () =>
            {
                var cardType = await _localScoreService.LocalGetTypeFromHiddenCode("16");
                if (cardType != null && cardType.Count > 0)
                {
                    foreach (var item in cardType)
                    {
                        _dIcCardType.Add(item.Value, item.Name);
                    }
                }
                else
                {

                }

            });

        }
        #endregion

        #region properties

        private int _disId;
        public int DisId
        {
            get
            {
                return _disId;
            }
            set
            {
                SetProperty(ref _disId, value);
            }
        }

        private int _pId;
        public int PId
        {
            get
            {
                return _pId;
            }
            set
            {
                SetProperty(ref _pId, value);
            }
        }

        private string _planName;
        public string PlanName
        {
            get
            {
                return _planName;
            }
            set
            {
                SetProperty(ref _planName, value);
            }
        }

        private string _improveTitle;
        public string ImproveTitle
        {
            get
            {
                return _improveTitle;
            }
            set
            {
                SetProperty(ref _improveTitle, value);
            }
        }

        private string _improveDesc;
        public string ImproveDesc
        {
            get
            {
                return _improveDesc;
            }
            set
            {
                SetProperty(ref _improveDesc, value);
            }
        }
        private bool _planApproalYN;
        public bool PlanApproalYN
        {
            get
            {
                return _planApproalYN;
            }
            set
            {
                SetProperty(ref _planApproalYN, value);
            }
        }
        private DateTime? _planFinishDate;
        public DateTime? PlanFinishDate
        {
            get
            {
                return _planFinishDate;
            }
            set
            {
                SetProperty(ref _planFinishDate, value);
            }
        }

        private bool _resultApproalYN;
        public bool ResultApproalYN
        {
            get
            {
                return _resultApproalYN;
            }
            set
            {
                SetProperty(ref _resultApproalYN, value);
            }
        }
        private DateTime? _resultFinishDate;
        public DateTime? ResultFinishDate
        {
            get
            {
                return _resultFinishDate;
            }
            set
            {
                SetProperty(ref _resultFinishDate, value);
            }
        }
        private string _remark;
        public string Remark
        {
            get
            {
                return _remark;
            }
            set
            {
                SetProperty(ref _remark, value);
            }
        }

        private string _cardType;
        public string CardType
        {
            get
            {
                return _cardType;
            }
            set
            {
                SetProperty(ref _cardType, value);
            }
        }

        private string _cardTypeCode;
        public string CardTypeCode
        {
            get
            {
                return _cardTypeCode;
            }
            set
            {
                SetProperty(ref _cardTypeCode, value);
            }
        }

        #endregion


        #region command
        public Command OpenPlanListCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        var action = await _commonFun.ShowActionSheetAny(_dIcPlans.Values.ToArray<string>());
                        if (_dIcPlans.ContainsValue(action))
                        {
                            PlanName = action;
                            PId = _dIcPlans.FirstOrDefault(q => q.Value == action).Key;
                        }
                    }
                    catch (Exception)
                    {
                    }
                });
            }


        }

        public Command OpenCardTypeListCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        var action = await _commonFun.ShowActionSheetAny(_dIcCardType.Values.ToArray<string>());
                        if (_dIcCardType.ContainsValue(action))
                        {
                            CardType = action;
                            CardTypeCode = _dIcCardType.FirstOrDefault(q => q.Value == action).Key;
                        }
                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }

        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
               {
                   try
                   {
                       if (PId == 0)
                       {
                           _commonFun.AlertLongText("请选择计划任务");
                           return;
                       }
                       else if (CardTypeCode == "")
                       {
                           _commonFun.AlertLongText("请选择任务卡类型");
                           return;
                       }
                       else if (ImproveTitle.Trim() == "")
                       {
                           _commonFun.AlertLongText("请填写改善标题");
                           return;
                       }

                       TaskCard tc = new TaskCard()
                       {
                           Id = Guid.NewGuid().ToString(),
                           TCTitle = ImproveTitle,
                           TCDescription = ImproveDesc,
                           UseYN = 2,
                           InUserId = int.Parse(CommonContext.Account.UserId),
                           SourceType = "D",
                           TCKind = CardTypeCode,
                           GRUD = "N"

                       };

                       TaskItem ti = new TaskItem()
                       {
                           Id = Guid.NewGuid().ToString(),
                           SeqNo = 1,
                           Title = ImproveTitle,
                           ScoreStandard = ImproveDesc,
                           InUserId = int.Parse(CommonContext.Account.UserId),
                           Tcid = tc.Id,
                           GRUD = "N"
                       };

                       TaskOfPlan tp = new TaskOfPlan()
                       {
                           Id = Guid.NewGuid().ToString(),
                           Status = "S",
                           PId = PId,
                           TCId = tc.Id,
                           InUserId = int.Parse(CommonContext.Account.UserId),
                           TPTitle = ImproveTitle,
                           GRUD = "N"
                       };
                       Score score = new Score()
                       {
                           TPId = tp.Id,
                           ItemId = ti.Id,
                           Scoreval = 0,
                           PlanApproalYN = PlanApproalYN,
                           PlanFinishDate = PlanFinishDate,
                           ResultApproalYN = ResultApproalYN,
                           ResultFinishDate = ResultFinishDate,
                           PassYN = "0",
                           Remarks = Remark,
                           InUserId = int.Parse(CommonContext.Account.UserId),
                           GRUD = "N",
                           Id = Guid.NewGuid().ToString(),
                       };

                       _localScoreService.SaveCustImprove(tc, ti, tp, score);
                       await Navigation.PopAsync();
                       MessagingCenter.Send<string>(DisId.ToString(), "SearchTaskList");
                   }
                   catch (Exception ex)
                   {
                   }
               });
            }


        }
        #endregion
        public void Init(int disId)
        {
            DisId = disId;
            PId = 0;
            PlanName = "请选择";
            CardType = "请选择";
            CardTypeCode = "";
            ImproveTitle = "";
            ImproveDesc = "";
            PlanApproalYN = false;
            ResultApproalYN = true;
            Remark = "";
            PlanFinishDate = DateTime.Now.AddDays(7);
            ResultFinishDate = DateTime.Now.AddDays(7);

            Device.BeginInvokeOnMainThread(async () =>
            {
                var planList = await _localScoreService.SearchPlanList(DisId);
                _dIcPlans.Clear();
                if (planList != null && planList.Count > 0)
                {
                    foreach (var item in planList)
                    {
                        _dIcPlans.Add(item.PId, item.PTitle);
                    }
                }
                else
                {

                }

            });
        }
    }
}
