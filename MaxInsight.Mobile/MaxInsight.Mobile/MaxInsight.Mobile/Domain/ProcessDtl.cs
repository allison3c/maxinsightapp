using System;

namespace MaxInsight.Mobile.Domain
{
    public class ProcessDtl:BaseEntity
    {
        public  int ProcessId { get; set; }
        public  int OrderNo { get; set; }
        public  string EmpType { get; set; }
        public  int? InUserId { get; set; }
        public  DateTime? InDateTime { get; set; }
    }
}
