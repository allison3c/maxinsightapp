using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Module.Dto.Calender
{
    public class SaveCalenderMngParams
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public string SDate { get; set; }
        public string EDate { get; set; }
        public string UserID { get; set; }
    }
}
