using System;

namespace MaxInsight.Mobile.Domain
{
    public class ImprovementApprovalHis : BaseEntity
    {
        public  int SeqNo { get; set; }
        public  int? ImpResultId { get; set; }
        public  int? ProcessId { get; set; }
        public  int? OrderNo { get; set; }
        public  string ResultStatus { get; set; }
        public  string ApprovalContent { get; set; }
        public  DateTime? ApprovalDateTime { get; set; }
        public  int? ApprovalUserId { get; set; }
    }
}
