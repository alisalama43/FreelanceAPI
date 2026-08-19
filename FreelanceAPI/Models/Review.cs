namespace FreelanceAPI.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }= null!;
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
