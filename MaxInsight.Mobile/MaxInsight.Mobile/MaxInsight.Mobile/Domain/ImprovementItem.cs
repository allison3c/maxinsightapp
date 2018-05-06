using System;

namespace MaxInsight.Mobile.Domain
{
    public class ImprovementItem : BaseEntity
    {
        public  int TPId { get; set; }
        public  int ItemId { get; set; }
        public  string ImprovementNo { get; set; }
        public  int? ExecDepartment { get; set; }
        public  int? PlanApproval { get; set; }
        public  string ImprovementCaption { get; set; }
        public  string LostDescription { get; set; }
        public  string PlanStatus { get; set; }
        public  int? FeedbackDisid { get; set; }
        public  DateTime? FeedbackTime { get; set; }
        public  int? FeedbackRegionid { get; set; }
        public  DateTime? FeedbackrRgionTime { get; set; }
        public  string RegionApprovalPlan { get; set; }
        public  string DisApprovalPlan { get; set; }
        public  string ResultStatus { get; set; }
        public  string Date { get; set; }
        public  string ImprovementPlan { get; set; }
        public  DateTime? ExpectedTime { get; set; }
        public  DateTime? PlanTime { get; set; }
        public  int? ProcessId { get; set; }
        public  int? InUserId { get; set; }
        public  DateTime? InDateTime { get; set; }
        public  int? ModiUserId { get; set; }
        public  DateTime? ModiDateTime { get; set; }
    }
}
