using NexusApp.Areas.Survey.Models;

namespace NexusApp.Areas.Financial.Models.ModelsViews
{
    public class InstallGuaranteeViewModel
    {
        public List<InstallModel>? InstallModelView { get; set; }
        public List<GuaranteeModel>? GuaModelView { get; set; }
    }
}
