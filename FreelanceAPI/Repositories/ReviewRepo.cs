using FreelanceAPI.Data;
using FreelanceAPI.Models;
using FreelanceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FreelanceAPI.Repositories
{
    public class ReviewRepo(AppDbContext _context) : IReviewRepo
    {
        public async Task<Review?> GetByIdAsync(int id) =>
           await _context.Reviews
               .Include(r => r.Order).ThenInclude(o => o.Service)
               .Include(r => r.Order).ThenInclude(o => o.Buyer)
               .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<Review?> GetByOrderIdAsync(int orderId) =>
            await _context.Reviews.FirstOrDefaultAsync(r => r.OrderId == orderId);

        public async Task<bool> ExistsForOrderAsync(int orderId) =>
            await _context.Reviews.AnyAsync(r => r.OrderId == orderId);

        public async Task AddAsync(Review review) => await _context.Reviews.AddAsync(review);

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
