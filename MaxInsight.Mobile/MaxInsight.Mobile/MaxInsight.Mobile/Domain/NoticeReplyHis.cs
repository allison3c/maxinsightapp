using System;

namespace MaxInsight.Mobile.Domain
{
    public class NoticeReplyHis : BaseEntity
    {
        public  int? NoticeReaderId { get; set; }
        public  string ReplyContent { get; set; }
        public  int? ReplyUserId { get; set; }
        public  DateTime? ReplyDateTime { get; set; }
        public  bool? PassYN { get; set; }
        public  string FeedbackContent { get; set; }
        public  int? FeedbackUserId { get; set; }
        public  DateTime? FeedbackDateTime { get; set; }
        public  string Status { get; set; }
    }
}
