using FreelanceAPI.Models;
using FreelanceAPI.Requests;
using FreelanceAPI.Responses;

namespace FreelanceAPI.Services.Interface
{
    public interface IServiceService
    {
        public Task<List<ServiceResponse>> GetServicesPageAsync(int page = 1, int pageSize = 10, CancellationToken ct = default);
        public Task<int> GetServicesCountAsync(CancellationToken ct = default);
        public Task<ServiceResponse?> GetServiceByIdAsync(int serviceId, CancellationToken ct = default);
        public Task<ServiceResponse> AddServiceAsync(CreateServiceRequest service, CancellationToken ct = default);
        public Task<ServiceResponse> UpdateServiceAsync(UpdateServiceRequest service, CancellationToken ct = default);
        public Task<bool> DeleteServiceAsync(int serviceId, CancellationToken ct = default);
        public Task<bool> ExistsByIdAsync(int serviceId, CancellationToken ct = default);
        Task<List<ServiceResponse>> SearchServicesAsync(string keyword, CancellationToken ct = default);
        Task<List<ServiceResponse>> GetServicesBySellerIdAsync(string  sellerId, CancellationToken ct = default);
    }
}
