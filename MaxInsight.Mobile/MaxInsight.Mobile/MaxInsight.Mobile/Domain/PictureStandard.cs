using System;

namespace MaxInsight.Mobile.Domain
{
    public class PictureStandard : BaseEntity
    {
        public  int TIId { get; set; }
        public  string StandardPicName { get; set; }
        public  string Url { get; set; }
        public  int? InUserId { get; set; }
        public  DateTime? InDateTime { get; set; }
        public  int? ModiUserId { get; set; }
        public  DateTime? ModiDateTime { get; set; }
    }
}
