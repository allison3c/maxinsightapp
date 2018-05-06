using System;

namespace MaxInsight.Mobile.Module.Dto.Notifi
{
    //通知查询列表显示用
    public class NoticeListInfoDto
    {
        public int SeqNo { get; set; }
        public int NoticeId { get; set; }
        public string Status { get; set; }//状态
        public string Title { get; set; }
        public string NoticeNo { get; set; }
        public string StatusName { get; set; }
        public string NeedReply { get; set; }
        public string NeedReplyName { get; set; }
        public string MadeUserName { get; set; }
        public string MadeDate { get; set; }
        public string ReplyDate { get; set; }
        public string FeedbackDate { get; set; }
        public string FeedbackYN { get; set; }
        public int NoticeReaderId { get; set; }
        public DateTime InDateTime { get; set; }
        public int DisId { get; set; }
        public int DepartId { get; set; }
        public bool ShowFirstGrid
        {
            get
            {
                if (FeedbackYN == "Y")
                    return true;
                else
                    return false;
            }
        }
        public bool ShowSecondGrid
        {
            get
            {
                if (FeedbackYN == "Y")
                    return false;
                else
                    return true;
            }
        }
    }
}
