using FreelanceAPI.Models;

namespace M03.RepositoryPattern.Responses;

public partial class ProductReviewResponse
{
    public int ReviewId { get; set; }
    public int ProductId { get; set; }
    public int Stars { get; set; }

    private ProductReviewResponse() { }

    public static ProductReviewResponse FromModel(Review? review)
    {
        if (review == null)
            throw new ArgumentNullException(nameof(review), "Cannot create a response from a null review");

        return new ProductReviewResponse
        {
       
            Stars = review.Rating


        };
    }


    public static IEnumerable<ProductReviewResponse> FromModels(IEnumerable<Review> reviews)
    {
        if (reviews == null)
            throw new ArgumentNullException(nameof(reviews));

        return reviews.Select(FromModel);
    }
}
