namespace FreelanceAPI.Responses
{
    public class ReviewResponse
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string ServiceTitle { get; set; } = string.Empty;
        public string BuyerName { get; set; } = string.Empty;
    }
}
