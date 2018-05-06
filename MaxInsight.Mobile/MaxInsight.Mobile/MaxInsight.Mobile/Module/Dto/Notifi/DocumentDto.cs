
using System;

namespace MaxInsight.Mobile.Module.Dto.Notifi
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PathToFile { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
