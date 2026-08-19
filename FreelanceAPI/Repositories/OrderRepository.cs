
using FreelanceAPI.Data;
using FreelanceAPI.Models;
using FreelanceAPI.Models.Enum;
using FreelanceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
public class OrderRepository(AppDbContext context) : IOrderRepository
{
    public async Task<int> GetOrdersCountAsync(CancellationToken ct = default) =>
        await context.Orders.CountAsync(ct);

    public async Task<List<Order>> GetOrdersPageAsync(int page = 1, int pageSize = 10, CancellationToken ct = default)
    {
        var orders       = await context.Orders.Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync(ct);

        return orders;
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId, CancellationToken ct = default)=>
     await  context.Orders.FirstOrDefaultAsync(o => o.Id == orderId, ct);

    public async Task<List<Review>> GetOrderReviewsAsync(int orderId, CancellationToken ct = default)
    {
        return await context.Reviews.Where(r => r.Id == orderId).ToListAsync(ct);
    }

    public async Task<Review?> GetReviewAsync(int orderId, int reviewId, CancellationToken ct = default)
    {
        return await context.Reviews.FirstOrDefaultAsync(r => r.Id == orderId && r.Id == reviewId, ct);
    }

    public async Task<bool> AddOrderAsync(Order order, CancellationToken ct = default)
    {
        context.Orders.Add(order );

        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> AddOrderReviewAsync(Review review, CancellationToken ct = default)
    {
        context.Reviews.Add(review);

        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateOrderAsync(Order updatedOrder, CancellationToken ct = default)
    {
        context.Orders.Update(updatedOrder);
        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> DeleteOrderAsync(int id, CancellationToken ct = default)
    {
        var order = await context.Orders.FirstOrDefaultAsync(p => p.Id == id, ct);

        context.Orders.Remove(order);

        return await context.SaveChangesAsync(ct) > 0;
    }
    public async Task<bool> ExistsByIdAsync(int id, CancellationToken ct = default)
        => await context.Orders.AnyAsync(p => p.Id == id, ct    );
    public async Task<List<Order>> GetOrdersByBuyerIdAsync(string buyerId, CancellationToken ct = default)
    {
        return await context.Orders
            .Where(o => o.BuyerId == buyerId)
            .ToListAsync(ct);
    }
    public async Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status, CancellationToken ct = default)
    {
        return await context.Orders
            .Where(o => o.Status == status)
            .ToListAsync(ct);
    }

}