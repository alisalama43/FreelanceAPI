using FreelanceAPI.Data;
using FreelanceAPI.Interfaces;
using FreelanceAPI.Models;
using FreelanceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FreelanceAPI.Repositories
{
    public class ServiceRepo(AppDbContext context) : IServiceRepository
    {
        public async Task<bool> AddServiceAsync(Service service, CancellationToken ct = default)
        {
            context.Services.Add(service);
            return await context.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> DeleteServiceAsync(int serviceId, CancellationToken ct = default)
        {
            var service = await context.Services.FirstOrDefaultAsync(s => s.Id == serviceId, ct);
            context.Services.Remove(service);
            return await context.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> ExistsByIdAsync(int serviceId, CancellationToken ct = default) =>
            await context.Services.AnyAsync(s => s.Id == serviceId, ct);
        public async Task<Service?> GetServiceByIdAsync(int serviceId, CancellationToken ct = default)
        {
            var service= await context.Services.FirstOrDefaultAsync(s => s.Id == serviceId, ct);
            return service;
        }

        public async Task<int> GetServicesCountAsync(CancellationToken ct = default)
        {
            return await context.Services.CountAsync(ct);
        }

        public async Task<List<Service>> GetServicesPageAsync(int page = 1, int pageSize = 10, CancellationToken ct = default)
        {
            var services = await context.Services.Skip((page - 1) * pageSize)
                           .Take(pageSize)
                           .ToListAsync(ct);

            return services;
        }

        public async Task<bool> UpdateServiceAsync(Service service, CancellationToken ct = default)
        {

            return await context.SaveChangesAsync(ct) > 0;
        }
        public async Task<List<Service>> SearchServicesAsync(string keyword, CancellationToken ct = default)
        {
            return await context.Services
                .Where(s => s.Title.Contains(keyword) ||
                            s.Description.Contains(keyword))
                .ToListAsync(ct);
        }
        public async Task<List<Service>> GetServicesBySellerIdAsync(string sellerId, CancellationToken ct = default)
        {
            return await context.Services
                .Where(s => s.UserId == sellerId)
                .ToListAsync(ct);
        }
    }
}
