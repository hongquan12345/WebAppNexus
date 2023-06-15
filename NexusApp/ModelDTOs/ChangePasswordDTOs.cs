using System.ComponentModel.DataAnnotations;

namespace NexusApp.ModelDTOs
{
    public class ChangePasswordDTOs
    {
        public int Id { get; set; }

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }

    }
}
