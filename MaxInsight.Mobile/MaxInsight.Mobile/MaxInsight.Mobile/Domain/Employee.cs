namespace MaxInsight.Mobile.Domain
{
    public class Employee : BaseEntity
    {
        public  string UserType { get; set; }
        public  int? DisId { get; set; }
        public  int? DepartId { get; set; }
        public  string UserName { get; set; }
        public  string CellNo { get; set; }
        public  int? UserId { get; set; }
        public  bool? UserYN { get; set; }
    }
}
