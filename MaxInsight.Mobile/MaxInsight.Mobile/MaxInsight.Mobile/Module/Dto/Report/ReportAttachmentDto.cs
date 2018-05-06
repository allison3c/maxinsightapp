using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Module.Dto.Report
{
    public class ReportAttachmentDto
    {
        public int Id { get; set; }
        public string AttachName { get; set; }
        public string Code { get; set; }
        public string SourceType { get; set; }
        public string DisName { get; set; }
        public string DownloadCnt { get; set; }
        public string Url { get; set; }
        public string InDateTime { get; set; }
        public string DisCode { get; set; }
        public string GRUD { get; set; }
    }
}
