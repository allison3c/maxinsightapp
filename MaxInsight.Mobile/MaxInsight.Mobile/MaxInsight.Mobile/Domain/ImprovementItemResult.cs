using System;

namespace MaxInsight.Mobile.Domain
{
    public class ImprovementItemResult : BaseEntity
    {
        public virtual int? ImprovementId { get; set; }
        public virtual string ReplyContent { get; set; }
        public virtual DateTime? CommitDateTime { get; set; }
        public virtual int? InUserId { get; set; }
        public virtual DateTime? InDateTime { get; set; }
    }
}
