using System;

namespace MaxInsight.Mobile.Domain
{
    public class TaskItem : BaseEntity
    {
        public int? SeqNo { get; set; }
        public string Title { get; set; }
        public string ExeMode { get; set; }
        public string Tcid { get; set; }
        public string ScoreStandard { get; set; }
        public int? InUserId { get; set; }
        public DateTime? InDateTime { get; set; }
        public int? ModiUserId { get; set; }
        public DateTime? ModiDateTime { get; set; }
        public string GRUD { get; set; }
    }
}
