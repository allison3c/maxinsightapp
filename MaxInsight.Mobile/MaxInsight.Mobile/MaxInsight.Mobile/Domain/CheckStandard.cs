using System;

namespace MaxInsight.Mobile.Domain
{
    public class CheckStandard : BaseEntity
    {
        public string CContent { get; set; }
        public int? TIId { get; set; }
        public int? InUserId { get; set; }
        public DateTime? InDateTime { get; set; }
        public int? ModiUserId { get; set; }
        public DateTime? ModiDateTime { get; set; }
    }
}
