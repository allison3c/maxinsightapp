using System;

namespace MaxInsight.Mobile.Domain
{
    public class CasesInfo : BaseEntity
    {
        public string CaseNo { get; set; }
        public string CaseType { get; set; }
        public string CaseTitle { get; set; }
        public string CasePoint { get; set; }
        public string LossRemark { get; set; }
        public string ImproveRemark { get; set; }
        public int? DisId { get; set; }
        public DateTime? CaseRegDate { get; set; }
        public int CaseRegUserId { get; set; }
        public bool? UseYN { get; set; }
        public DateTime? InDateTime { get; set; }
        public int? InUserId { get; set; }
        public DateTime? ModiDateTime { get; set; }
        public int? ModiUserId { get; set; }
    }
}
