namespace MaxInsight.Mobile.Domain
{
    public class CodeHidden : BaseEntity
    {
        public string GroupCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public int DisSeq { get; set; }
        public bool? UseYN { get; set; }
    }
}
