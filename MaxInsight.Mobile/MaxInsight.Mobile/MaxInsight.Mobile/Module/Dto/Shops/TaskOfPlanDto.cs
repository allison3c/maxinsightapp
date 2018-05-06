using MaxInsight.Mobile.Domain;
using System;
namespace MaxInsight.Mobile
{
    public class TaskOfPlanDto : BaseEntity
    {
        public string TPId { get; set; }
        public string TCCode { get; set; }
        public string TCTitle { get; set; }
        public string TPStatus { get; set; }
        public string TPType { get; set; }
        public string SourceType { get; set; }
        public string PTitle { get; set; }
        public string UserName { get; set; }
    }
}
