namespace NexusApp.ModelDTOs
{
    public class UpdateCustomerDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Street { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
    }
}
