using System;

namespace MaxInsight.Mobile.Domain
{
    public class NoticeReader : BaseEntity
    {
        public  int? NoticeId { get; set; }
        public  int? EmployeId { get; set; }
        public  string FeedbackYN { get; set; }
        public  string ReplyContent { get; set; }
        public  DateTime? ReplyDateTime { get; set; }
        public  string FeedbackContent { get; set; }
        public  DateTime? FeedbackDateTime { get; set; }
        public  bool? PassYN { get; set; }
        public  string Status { get; set; }
        public  int? InUserId { get; set; }
        public  DateTime? InDateTime { get; set; }
        public  int? ModiUserId { get; set; }
        public  DateTime? ModiDateTime { get; set; }
        public  int? DisId { get; set; }
        public  int? DepartId { get; set; }
    }
}
