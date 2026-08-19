using Azure.Core;
using FreelanceAPI.Models;
using FreelanceAPI.Models.Enum;
using FreelanceAPI.Repositories.Interfaces;
using FreelanceAPI.Requests;
using FreelanceAPI.Responses;
using FreelanceAPI.Services.Interface;
using FreelanceMarketplace.API.Common.Exceptions;
using M03.RepositoryPattern.Requests;
using M03.RepositoryPattern.Responses;

namespace FreelanceAPI.Services.implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<bool> AddOrderAsync(CreateOrderRequest order, CancellationToken ct = default)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(order.Id, ct);
            if (existingOrder != null)
              throw new InvalidOperationException($"Order with ID {order.Id} already exists.");
            var newOrder = new Order
            {
                BuyerId = order.BuyerId,
                ServiceId = order.ServiceId,
                Status = order.Status,
                OrderDate = order.OrderDate
            };
            return await _orderRepository.AddOrderAsync(newOrder);
        }

        public async Task<bool> AddOrderReviewAsync(CreateOrderReviewRequest review, CancellationToken ct = default)
        {
           
            var newReview = new Review
            {
                
                Rating = review.Rating,
                Comment = review.Comment,
                
            };
            return await _orderRepository.AddOrderReviewAsync(newReview);
        }

        public async Task DeleteOrderAsync(int id, CancellationToken ct = default)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(id, ct)
                ?? throw new NotFoundException($"Order with ID '{id}' was not found.");

            await _orderRepository.DeleteOrderAsync(id, ct);
        }

        public async Task<bool> ExistsByIdAsync(int id, CancellationToken ct = default)
        {

            return await _orderRepository.ExistsByIdAsync(id, ct);
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(int orderId, CancellationToken ct = default)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId, ct);
            if (order == null)
            {
                return null;
            }
           var response = new OrderResponse
            { Id = order.Id,
                BuyerId = order.BuyerId,
                ServiceId = order.ServiceId, Status = order.Status, 
                OrderDate = order.OrderDate 
            };
            return response;
        }

        public async Task<List<ReviewResponse>> GetOrderReviewsAsync(int orderId, CancellationToken ct = default)
        {
            var reviews = await _orderRepository.GetOrderReviewsAsync(orderId, ct);
            if (reviews == null)
                throw new InvalidOperationException($"No reviews found for Order ID {orderId}.");
            return reviews.Select(r => new ReviewResponse
            {
                Id = r.Id,
                OrderId = r.OrderId,
                Rating = r.Rating,
                Comment = r.Comment
            }).ToList();
        }

        public async Task<List<OrderResponse>> GetOrdersByBuyerIdAsync(string buyerId, CancellationToken ct = default)
        {
            var orders = await _orderRepository.GetOrdersByBuyerIdAsync(buyerId, ct);
            if (orders == null)
                throw new InvalidOperationException($"No orders found for Buyer ID {buyerId}.");
            return orders.Select(o => new OrderResponse
            {
                Id = o.Id,
                BuyerId = o.BuyerId,
                ServiceId = o.ServiceId,
                Status = o.Status,
                OrderDate = o.OrderDate
            }).ToList();
        }

        public async Task<List<OrderResponse>> GetOrdersByStatusAsync(OrderStatus status, CancellationToken ct = default)
        {
            var orders = await _orderRepository.GetOrdersByStatusAsync(status, ct);
            if (orders == null)
                throw new InvalidOperationException($"No orders found with status {status}.");
            return orders.Select(o => new OrderResponse
            {
                Id = o.Id,
                BuyerId = o.BuyerId,
                ServiceId = o.ServiceId,
                Status = o.Status,
                OrderDate = o.OrderDate
            }).ToList();
        }

        public async Task<int> GetOrdersCountAsync(CancellationToken ct = default)
        {
           return await _orderRepository.GetOrdersCountAsync(ct);

        }

        public async Task<List<OrderResponse>> GetOrdersPageAsync(int page = 1, int pageSize = 10, CancellationToken ct = default)
        {
            var orders = await _orderRepository.GetOrdersPageAsync(page, pageSize, ct);
            if (orders == null)
                throw new InvalidOperationException($"No orders found for page {page} with page size {pageSize}.");
            return orders.Select(o => new OrderResponse
            {
                Id = o.Id,
                BuyerId = o.BuyerId,
                ServiceId = o.ServiceId,
                Status = o.Status,
                OrderDate = o.OrderDate
            }).ToList();
        }

        public async Task<ReviewResponse?> GetReviewAsync(int orderId, int reviewId, CancellationToken ct = default)
        {
            var review = await _orderRepository.GetReviewAsync(orderId, reviewId, ct);
            if (review == null)
                return null;
            return new ReviewResponse
            {
                Id = review.Id,
                OrderId = review.OrderId,
                Rating = review.Rating,
                Comment = review.Comment
            };
        }

        public async Task UpdateOrderAsync(
       int id,
       UpdateOrderRequest request,
       CancellationToken ct = default)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id, ct)
                ?? throw new NotFoundException($"Order with ID '{id}' was not found.");

            order.Status = request.Status;

            await _orderRepository.UpdateOrderAsync(order, ct);
        }

    }
}
