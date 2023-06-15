using System.ComponentModel.DataAnnotations;


namespace NexusApp.Areas.Survey.Models
{
    public class InstallModel
    {
        public int InstallId { get; set; }
        [Required]
        public string Status { get; set; } //Processing - Completed

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime? InstalledDate { get; set; } 
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime? FinishedDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime CreatedDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? UpdatedDate { get; set; }
        public int SurveyRefId { get; set; }
        public SurveyModel Surveys { get; set; }
    }
}
