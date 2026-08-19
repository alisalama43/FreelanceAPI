using FreelanceAPI.Models;
using FreelanceAPI.Models.Enum;
using FreelanceAPI.Requests;
using FreelanceAPI.Responses;
using M03.RepositoryPattern.Requests;
using M03.RepositoryPattern.Responses;

namespace FreelanceAPI.Services.Interface
{
    public interface IOrderService
    {
        Task<List<OrderResponse>> GetOrdersPageAsync(int page = 1, int pageSize = 10, CancellationToken ct = default);
        Task<bool> AddOrderAsync(CreateOrderRequest order, CancellationToken ct = default);
        Task<bool> AddOrderReviewAsync(CreateOrderReviewRequest review, CancellationToken ct = default);
        Task DeleteOrderAsync(int id, CancellationToken ct = default);
        Task<bool> ExistsByIdAsync(int id, CancellationToken ct = default);
        Task<OrderResponse?> GetOrderByIdAsync(int orderId, CancellationToken ct = default);
        Task<List<ReviewResponse>> GetOrderReviewsAsync(int orderId, CancellationToken ct = default);
        Task<int> GetOrdersCountAsync(CancellationToken ct = default);
        Task<ReviewResponse?> GetReviewAsync(int orderId, int reviewId, CancellationToken ct = default);
        Task UpdateOrderAsync(int id, UpdateOrderRequest updatedOrder, CancellationToken ct = default);
        Task<List<OrderResponse>> GetOrdersByBuyerIdAsync(string buyerId, CancellationToken ct = default);
        Task<List<OrderResponse>> GetOrdersByStatusAsync(OrderStatus status, CancellationToken ct = default);
    }
}
