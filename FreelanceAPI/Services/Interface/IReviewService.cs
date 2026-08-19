using FreelanceAPI.Models;
using FreelanceAPI.Responses;
using M03.RepositoryPattern.Requests;

namespace FreelanceAPI.Services.Interface
{
    public interface IReviewService
    {
        Task<ReviewResponse?> GetByIdAsync(int id);
        Task<ReviewResponse?> GetByOrderIdAsync(int orderId);
        Task<bool> ExistsForOrderAsync(int orderId);
        Task<ReviewResponse?> AddAsync(CreateOrderReviewRequest review);
       
    }
}
