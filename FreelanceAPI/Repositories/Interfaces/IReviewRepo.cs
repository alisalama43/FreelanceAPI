using FreelanceAPI.Models;

namespace FreelanceAPI.Repositories.Interfaces
{
    public interface IReviewRepo
    {
        Task<Review?> GetByIdAsync(int id);
        Task<Review?> GetByOrderIdAsync(int orderId);
        Task<bool> ExistsForOrderAsync(int orderId);
        Task AddAsync(Review review);
        Task<bool> SaveChangesAsync();
    }
}
