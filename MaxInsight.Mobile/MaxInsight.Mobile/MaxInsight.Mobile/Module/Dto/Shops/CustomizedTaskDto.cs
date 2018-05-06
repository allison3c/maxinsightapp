using MaxInsight.Mobile.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Module.Dto.Shops
{
    public class CustomizedTaskDto: BaseEntity
    {
        public int TPId { get; set; }
        public int TCId { get; set; }
        public int ScoreId { get; set; }
        public string Remarks { get; set; }
        public int UserId { get; set; }
    }
}
