using System;

namespace MaxInsight.Mobile.Domain
{
    public class PushSend:BaseEntity
    {
        public  string Title { get; set; }
        public  string Content { get; set; }
        public  string TargetUser { get; set; }
        public  bool? PushYN { get; set; }
        public  DateTime? SendDate { get; set; }
        public  DateTime? InDate { get; set; }
        public  int? InUserId { get; set; }
    }
}
