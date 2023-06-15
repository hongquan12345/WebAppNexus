using System.ComponentModel.DataAnnotations;

namespace NexusApp.Areas.ServiceConnection.Models
{
    public class ServiceConnectionModel
    {
        public int ServiceConnectionId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal Deposit { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? CreatedDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? UpdatedDate { get; set; }
        public virtual ICollection<SubServiceConnectionModel>? SubServiceConnectionModels { get; set; }
    }
}
