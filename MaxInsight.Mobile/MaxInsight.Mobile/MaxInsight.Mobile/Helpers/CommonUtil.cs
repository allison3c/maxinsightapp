using MaxInsight.Mobile.Module.Dto.Notifi;
using System.Collections.Generic;

namespace MaxInsight.Mobile.Helpers
{
    public class CommonUtil
    { 
        #region 服务商  部门
        public static string DisAndDepToString(List<MultiSelectDto> disList, List<MultiSelectDto> depList)
        {
            string readerList = "";
            foreach (MultiSelectDto disDto in disList)
            {
                foreach (MultiSelectDto depDto in depList)
                {
                    readerList = readerList + disDto.DisCode + "|" + depDto.DisCode + ",";
                }
            }
            return readerList.Remove(readerList.Length - 1);
        }
        #endregion
    }
}
