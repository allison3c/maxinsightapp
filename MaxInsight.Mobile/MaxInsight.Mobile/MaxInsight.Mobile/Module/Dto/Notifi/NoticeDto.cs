using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Module.Dto.Notifi
{
    public class NoticeDto
    {
        public int Id { get; set; }
        public string NoticeNo { get; set; }
        public string InDate { get; set; }
        public string NeedReplyName { get; set; }
        public string StatusName { get; set; }
        public string UserName { get; set; }
        public string NoticeReaders { get; set; }
        public string Title { get; set; }
        public int NoticeId { get; set; }
        public string SDate { get; set; }
        public string EDate { get; set; }
        public string NeedReply { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public int InUserId { get; set; }
    }
}
