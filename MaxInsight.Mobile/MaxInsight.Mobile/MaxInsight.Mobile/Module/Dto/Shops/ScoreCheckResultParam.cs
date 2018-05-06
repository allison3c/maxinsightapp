using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace MaxInsight.Mobile
{
    public class ScoreCheckResultParam
    {
        public List<ScoreRegDto> ScoreLst { get; set; }
        public List<CheckResultRegDto> CheckResultLst { get; set; }
        public List<StandardPicRegDto> StandardPicLst { get; set; }
        public List<PictureStandard> PicStandLst { get; set; }
        public int UserId { get; set; }
    }
    public class ScoreRegDto
    {
        public string TPId { get; set; }
        public string TIId { get; set; }
        public int Score { get; set; }
        public string PassYN { get; set; }
        public string Remarks { get; set; }
        public string GRUD { get; set; }
        public bool PlanApproalYN { get; set; }
        public bool ResultApproalYN { get; set; }
        public DateTime? PlanFinishDate { get; set; }
        public DateTime? ResultFinishDate { get; set; }

    }
    public class CheckResultRegDto
    {
        public int TPId { get; set; }
        public int TIId { get; set; }
        public int CSId { get; set; }
        public bool Result { get; set; }
        public string GRUD { get; set; }
    }
    public class StandardPicRegDto
    {
        public string TPId { get; set; }
        public string TIId { get; set; }
        public string Url { get; set; }
        public string PicName { get; set; }
        public string PicType { get; set; }
        public string GRUD { get; set; }

        //add by me
        public string FilePath { get; set; }
        public ImageSource ImageStream { get; set; }
    }
}
