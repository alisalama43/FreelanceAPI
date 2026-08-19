namespace FreelanceAPI.Responses
{
    public class ServiceResponse
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DeliveryTimeInDays { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
