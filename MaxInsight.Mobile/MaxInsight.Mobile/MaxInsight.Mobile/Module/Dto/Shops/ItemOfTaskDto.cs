using MaxInsight.Mobile.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace MaxInsight.Mobile
{
    public class ItemOfTaskDto : BaseEntity
    {
        public string TPId { get; set; }
        public string TIId { get; set; }
        public int SeqNo { get; set; }
        public string Title { get; set; }
        public string PassYN { get; set; }
        public int Score { get; set; }
        public bool PlanApproalYN { get; set; }
        public string PlanApproalYNText
        {
            get { if (PlanApproalYN) { return "需要"; } else { return "不需要"; } }
            set { }
        }
        public bool ResultApproalYN { get; set; }
        public string ResultApproalYNText
        {
            get { if (ResultApproalYN) { return "需要"; } else { return "不需要"; } }
            set { }
        }

        public string ExeMode { get; set; }
        public string ScoreStandard { get; set; }
        public bool IsClicked { get; set; }
        public string Remarks { get; set; }
        public string GRUD { get; set; }
        public bool VisibleYN
        {
            get { return !IsClicked; }
            set { }
        }
        public DateTime? PlanFinishDate { get; set; }
        public DateTime? ResultFinishDate { get; set; }

        public string StrPlanFinishDate
        {
            get { if (PlanFinishDate != null) { return Convert.ToDateTime(PlanFinishDate).ToString("yyyy-MM-dd"); } else { return ""; } }
            set { }
        }
        public string StrResultFinishDate
        {
            get { if (ResultFinishDate != null) { return Convert.ToDateTime(ResultFinishDate).ToString("yyyy-MM-dd"); } else { return ""; } }
            set { }
        }

        public ObservableCollection<CheckStandard> CSList { get; set; }
        public ObservableCollection<StandardPic> SPicList { get; set; }
        public ObservableCollection<PictureStandard> PStandardList { get; set; }

        public ObservableCollection<StandardPic> ParamSPicList { get; set; } = new ObservableCollection<StandardPic>();

        public int CurrentIndex { get; set; } = 0;

        //public ObservableCollection<StandardPic> Images { get; set; } = new ObservableCollection<StandardPic>();
    }

    public class CheckStandard : BaseEntity
    {
        public int TPId { get; set; }
        public int TIId { get; set; }
        public int SeqNo { get; set; }
        public int CSID { get; set; }
        public string CContent { get; set; }
        public bool IsCheck { get; set; }
        public string StrCRId { get; set; }
    }

    public class StandardPic : BaseEntity
    {
        public int PicId { get; set; }
        public string TPId { get; set; }
        public string TIId { get; set; }
        public int SeqNo { get; set; }
        public string PicName { get; set; }
        public string Url { get; set; }
        public string PicType { get; set; }
        public string FilePath { get; set; }
        public ImageSource ImageStream { get; set; }
        public int StandardPicId { get; set; }
        public string StrPicId { get; set; }
    }

    public class PictureStandard : BaseEntity
    {
        public int StandardPicId { get; set; }
        public string TPId { get; set; }
        public string TIId { get; set; }
        public int SeqNo { get; set; }
        public string StandardPicName { get; set; }
        public string Url { get; set; }
        public string SuccessImage { get; set; }
        public string GRUD { get; set; }
        public string StrPicId { get; set; }
    }

    public class LoaclItemOfTaskDto : BaseEntity
    {
        public string TPId { get; set; }
        public string LCTIId { get; set; }
        public string TIId { get; set; }
        public int SeqNo { get; set; }
        public string Title { get; set; }
        public string PassYN { get; set; }
        public string StrPassYN
        {
            get
            {
                if (PassYN == "0")
                {
                    return "否";
                }
                else if (PassYN == "1")
                {
                    return "是";
                }
                else if (PassYN == "Q")
                {
                    return "不涉及";
                }
                else { return ""; }
            }
            set { }
        }
        public int Score { get; set; }
        public bool PlanApproalYN { get; set; }
        public string PlanApproalYNText
        {
            get { if (PlanApproalYN) { return "需要"; } else { return "不需要"; } }
            set { }
        }
        public bool ResultApproalYN { get; set; }
        public string ResultApproalYNText
        {
            get { if (ResultApproalYN) { return "需要"; } else { return "不需要"; } }
            set { }
        }

        public string ExeMode { get; set; }
        public string ScoreStandard { get; set; }
        public bool IsClicked { get; set; }
        public string Remarks { get; set; }
        public string GRUD { get; set; }
        public bool VisibleYN
        {
            get { return !IsClicked; }
            set { }
        }
        public DateTime PlanFinishDate { get; set; }
        public DateTime ResultFinishDate { get; set; }
        public string StrPlanFinishDate
        {
            get { if (PlanFinishDate != null) { return Convert.ToDateTime(PlanFinishDate).ToString("yyyy-MM-dd"); } else { return ""; } }
            set { }
        }
        public string StrResultFinishDate
        {
            get { if (ResultFinishDate != null) { return Convert.ToDateTime(ResultFinishDate).ToString("yyyy-MM-dd"); } else { return ""; } }
            set { }
        }
        public string StrScoreId { get; set; }
    }
}
