using System.ComponentModel.DataAnnotations;

namespace M03.RepositoryPattern.Requests;

public class CreateOrderReviewRequest
{
     
    [Required]
   [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    
   public int Rating { get; set; } 
   public string Comment { get; set; } = string.Empty;
}