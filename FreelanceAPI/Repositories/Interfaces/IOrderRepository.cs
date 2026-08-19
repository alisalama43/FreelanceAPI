using FreelanceAPI.Models;
using FreelanceAPI.Models.Enum;

namespace FreelanceAPI.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<bool> AddOrderAsync(Order order, CancellationToken ct = default);
    Task<bool> AddOrderReviewAsync(Review review, CancellationToken ct = default);
    Task<bool> DeleteOrderAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct = default);
    Task<Order?> GetOrderByIdAsync(int orderId, CancellationToken ct = default);
    Task<List<Review>> GetOrderReviewsAsync(int orderId, CancellationToken ct = default);
    Task<int> GetOrdersCountAsync( CancellationToken ct = default);
    Task<List<Order>> GetOrdersPageAsync(int page = 1, int pageSize = 10, CancellationToken ct = default);
    Task<Review?> GetReviewAsync(int orderId, int reviewId, CancellationToken ct = default);
    Task<bool> UpdateOrderAsync(Order updatedOrder, CancellationToken ct = default);
    Task<List<Order>> GetOrdersByBuyerIdAsync(string buyerId, CancellationToken ct = default);
    Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status, CancellationToken ct = default);
}