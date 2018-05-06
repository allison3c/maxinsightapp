using System;

namespace MaxInsight.Mobile.Domain
{
    public class Notice : BaseEntity
    {
        public  string NoticeNo { get; set; }
        public  string Title { get; set; }
        public  string Content { get; set; }
        public  string SDate { get; set; }
        public  string EDate { get; set; }
        public  bool? NeedReply { get; set; }
        public  string Status { get; set; }
        public  DateTime? InDate { get; set; }
        public  int? InUserId { get; set; }
        public  DateTime? InDateTime { get; set; }
        public  int? ModiUserId { get; set; }
        public  DateTime? ModiDateTime { get; set; }
    }
}
