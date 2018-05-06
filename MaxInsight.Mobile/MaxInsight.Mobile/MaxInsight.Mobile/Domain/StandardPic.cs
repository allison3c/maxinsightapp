namespace MaxInsight.Mobile.Domain
{
    public class StandardPic : BaseEntity
    {
        public string TPId { get; set; }
        public string TIId { get; set; }
        public int? PSId { get; set; }
        public string Url { get; set; }
        public string PicName { get; set; }
        public string Type { get; set; }
        public string GRUD { get; set; }
        public string DelChk { get; set; }
    }
}
