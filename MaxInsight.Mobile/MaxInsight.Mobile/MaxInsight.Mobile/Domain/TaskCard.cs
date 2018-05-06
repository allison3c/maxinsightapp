using System;

namespace MaxInsight.Mobile.Domain
{
    public class TaskCard : BaseEntity
    {
        public string TCCode { get; set; }
        public string TCType { get; set; }
        public string TCRange { get; set; }
        public string TCTitle { get; set; }
        public string TCDescription { get; set; }
        public int? DisId { get; set; }
        public int? UseYN { get; set; }
        public int? InUserId { get; set; }
        public DateTime? InDateTime { get; set; }
        public int? ModiUserId { get; set; }
        public DateTime? ModiDateTime { get; set; }
        public string SourceType { get; set; }
        public string TCKind { get; set; }
        public string GRUD { get; set; }

    }
}
