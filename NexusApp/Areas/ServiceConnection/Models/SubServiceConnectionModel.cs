using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexusApp.Areas.ServiceConnection.Models
{
    public class SubServiceConnectionModel
    {
        public int SubServiceConnectionId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? CreatedDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? UpdatedDate { get; set; }
        [ForeignKey(nameof(ServiceConnections))]
        public int ServiceConnectionRefId { get; set; }
        public ServiceConnectionModel? ServiceConnections { get; set; }
        public virtual ICollection<ServiceModel>? ServiceModels { get; set; }
    }
}
