using System;

namespace MaxInsight.Mobile.Domain
{
    public class ProcessMst:BaseEntity
    {
        public  int ProcessId { get; set; }
        public  int? InUserId { get; set; }
        public  DateTime? InDateTime { get; set; }
    }
}
