using System;

namespace MaxInsight.Mobile.Domain
{
    public class TaskOfPlan : BaseEntity
    {
        public string TPCode { get; set; }
        public string TPTitle { get; set; }
        public string TPDescription { get; set; }
        public string Status { get; set; }
        public DateTime? SDateTime { get; set; }
        public DateTime? EDateTime { get; set; }
        public int? PId { get; set; }
        public string TCId { get; set; }
        public int? InUserId { get; set; }
        public DateTime? InDateTime { get; set; }
        public string GRUD { get; set; }
    }
}
