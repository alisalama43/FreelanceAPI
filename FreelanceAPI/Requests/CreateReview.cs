namespace FreelanceAPI.Requests
{
    public class CreateReview
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
            public int Rating { get; set; }
            public string Comment { get; set; } = string.Empty;
       
    }
}
