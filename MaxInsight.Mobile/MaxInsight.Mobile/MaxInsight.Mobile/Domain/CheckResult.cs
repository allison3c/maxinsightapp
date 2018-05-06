namespace MaxInsight.Mobile.Domain
{
    public class CheckResult : BaseEntity
    {
        public int? TPId { get; set; }
        public int? TIId { get; set; }
        public int? CSId { get; set; }
        public bool? Result { get; set; }
        public string GRUD { get; set; }
    }
}
