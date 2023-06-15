using Microsoft.AspNetCore.Mvc.Rendering;
using NexusApp.Areas.Customer.Models;
using NexusApp.Areas.ServiceConnection.Models;

namespace NexusApp.Areas.Financial.Models.ModelsViews
{
    public class CustomerRegister
    {
        public List<ServiceModel>? Service { get; set; }
    /*    public List<SubServiceConnectionModel>? SubService { get; set; }
        public List<ServiceConnectionModel>? SconService { get; set; }*/

        public int? ServiceRefId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? message { get; set; }


    }
}
