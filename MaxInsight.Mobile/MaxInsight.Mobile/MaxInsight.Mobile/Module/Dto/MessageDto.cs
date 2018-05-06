using System.Collections.Generic;

namespace MaxInsight.Mobile.Module.Dto
{
    public class MessageDto
    {
        public int SeqNo { get; set; }
        public string MessageContent { get; set; }
        public string Status { get; set; }
        public string Id { get; set; }
        public string DataType { get; set; }
    }

    public class ItemResultDto
    {
        public ItemResultDto()
        {
            SecondItemList = new List<DoItemDto>();
            ThirdItemList = new List<DoItemDto>();
        }
        public string Id { get; set; }
        public List<DoItemDto> SecondItemList { get; set; }
        public List<DoItemDto> ThirdItemList { get; set; }
    }
    public class DoItemDto
    {
        public int SeqNo { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Id { get; set; }
        public string DataType { get; set; }
        public string TypeName { get; set; }
    }
}
