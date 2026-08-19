using FreelanceAPI.Models;

namespace FreelanceAPI.Repositories.Interfaces
{
    public interface IServiceRepository
    {
        public Task<List<Service>> GetServicesPageAsync(int page = 1, int pageSize = 10, CancellationToken ct = default);
        public Task<int> GetServicesCountAsync(CancellationToken ct = default   );
        public Task<Service?> GetServiceByIdAsync(int serviceId, CancellationToken ct = default);
        public Task<bool> AddServiceAsync(Service service, CancellationToken ct = default);
        public Task<bool> UpdateServiceAsync(Service service, CancellationToken ct = default);
        public Task<bool> DeleteServiceAsync(int serviceId, CancellationToken ct = default);
        public Task<bool> ExistsByIdAsync(int serviceId, CancellationToken ct = default);
        Task<List<Service>> SearchServicesAsync(string keyword, CancellationToken ct = default);
        Task<List<Service>> GetServicesBySellerIdAsync(string sellerId, CancellationToken ct = default);
        

    }
}
