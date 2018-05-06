using System;

namespace MaxInsight.Mobile.Domain
{
    public class NoticeDepart : BaseEntity
    {
        public  int NoticeId { get; set; }
        public  int DisId { get; set; }
        public  int DepartId { get; set; }
        public  int? InUserId { get; set; }
        public  DateTime? InDateTime { get; set; }
    }
}
