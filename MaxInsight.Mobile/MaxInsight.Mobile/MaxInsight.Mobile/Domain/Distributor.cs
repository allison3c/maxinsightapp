using System;

namespace MaxInsight.Mobile.Domain
{
    public class Distributor : BaseEntity
    {
        public  int? ParentId { get; set; }
        public  string Type { get; set; }
        public  string Code { get; set; }
        public  string Name { get; set; }
        public  bool? UseYN { get; set; }
        public  int? InUserid { get; set; }
        public  DateTime? InDateTime { get; set; }
    }
}
