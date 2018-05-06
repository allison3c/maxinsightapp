using System;

namespace MaxInsight.Mobile.Domain
{
    public class Plans:BaseEntity
    {
        public  string Title { get; set; }
        public  int? DistributorId { get; set; }
        public  DateTime? VisitDateTime { get; set; }
        public  string VisitType { get; set; }
        public  string PStatus { get; set; }
        public  string RejectReason { get; set; }
        public  int? Batch { get; set; }
        public  int? InUserId { get; set; }
        public  DateTime? InDateTime { get; set; }
        public  int? ModiUserId { get; set; }
        public  DateTime? ModiDateTime { get; set; }
    }
}
