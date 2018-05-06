using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Module.Dto.Notifi
{
    public class SearchParamDto
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string NoticeReaders { get; set; }
        public string Status { get; set; }
        public string NeedReply { get; set; }
        public string Title { get; set; }
        public string NoticeNo { get; set; }
        public string InUserId { get; set; }

    }
}
