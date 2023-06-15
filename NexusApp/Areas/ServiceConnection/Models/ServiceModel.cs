using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NexusApp.Areas.Customer.Models;
using NexusApp.Areas.Financial.Models;
using NexusApp.Areas.ServiceConnection.Models;

namespace NexusApp.Areas.ServiceConnection.Models
{
    public class ServiceModel
    {
         public int ServiceId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal ServicePrice { get; set; }

        public int? Duration { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? CreatedDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? UpdatedDate { get; set; }
        [ForeignKey(nameof(SubServiceConnections))]
        public int SubServiceConnectionRefId { get; set; }
        public SubServiceConnectionModel? SubServiceConnections { get; set; }
        public virtual ICollection<CustomerModel> Customers { get; set; }
        public virtual ICollection<ConnectionModel> Connections{ get; set; }
    }
}
