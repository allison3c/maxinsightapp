namespace MaxInsight.Mobile.Domain
{
    public class Attachment : BaseEntity
    {
        public int  RefId { get; set; }
        public string Type { get; set; }
        public string AttachName { get; set; }
        public string Url { get; set; }
    }
}
