using System.ComponentModel.DataAnnotations;
using NexusApp.Areas.ServiceConnection.Models;

namespace NexusApp.Areas.Financial.Models
{
    public class ConnectionModel
    {
        public int ConnectionID { get; set; }
        [Required]
        public string ConnectionName { get; set; }
        [Required]
        public string ConnectionType { get; set; } = string.Empty;
     
        public string? Status { get; set; } = string.Empty;
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime OrderDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? CreatedDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? UpdatedDate { get; set; }
         public int ServiceRefId { get; set; }
        public ServiceModel? Services { get; set; }

        public virtual ICollection<OrderDetailModel>? OrderDetails { get; set; }
    }
}
