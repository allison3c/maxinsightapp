using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MaxInsight.Mobile.Module.Dto.Calender
{
    public class CalenderListAllDto
    {
        public string Id { get; set; }
        public string EachDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string SDate { get; set; }
        public string EDate { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
        public string UserId { get; set; }
        public string DisId { get; set; }
        public string DepartId { get; set; }
        public string Period { get; set; }
        public Color BgColor { get; set; }
        public Color ForeColor { get; set; }
    }
}
