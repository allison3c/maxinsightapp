namespace MaxInsight.Mobile.Domain
{
    public class BaseEntity
    {
        public BaseEntity() { }
        [SQLite.Net.Attributes.PrimaryKey]
        public string Id { get; set; }
    }
}
