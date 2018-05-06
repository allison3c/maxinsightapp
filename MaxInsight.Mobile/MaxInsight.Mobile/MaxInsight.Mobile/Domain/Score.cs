using System;

namespace MaxInsight.Mobile.Domain
{
    public class Score:BaseEntity
    {
        public  string TPId { get; set; }
        public string ItemId { get; set; }
        public  int? Scoreval { get; set; }
        public  bool? PlanApproalYN { get; set; }
        public  DateTime? PlanFinishDate { get; set; }
        public  bool? ResultApproalYN { get; set; }
        public  DateTime? ResultFinishDate { get; set; }
        public  string PassYN { get; set; }
        public  string Remarks { get; set; }
        public  DateTime? InDateTime { get; set; }
        public  int? InUserId { get; set; }
        public  int? ModiUserId { get; set; }
        public  DateTime? ModiDateTime { get; set; }
        public string GRUD { get; set;}
    }
}
