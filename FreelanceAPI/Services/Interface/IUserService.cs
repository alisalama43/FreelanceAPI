using FreelanceAPI.Models;
using FreelanceAPI.Requests;
using FreelanceAPI.Responses;
using FreelanceMarketplace.API.Enums;
using M03.RepositoryPattern.Responses;

namespace FreelanceAPI.Services.Interface
{
    public interface IUserService
    {
        public Task<List<UserResponse>> GetUsersPageAsync(int page = 1, int pageSize = 10, CancellationToken ct = default);
        public Task<int> GetUsersCountAsync(CancellationToken ct = default);
        public Task<UserResponse?> GetUserByIdAsync(string id, CancellationToken ct = default);
        public Task<bool> UpdateUserAsync(string id, UpdateUser user, CancellationToken ct = default);
        public Task<bool> DeleteUserAsync(string id, CancellationToken ct = default);
        public Task<bool> ExistsByIdAsync(string id, CancellationToken ct = default);
        public Task<List<ServiceResponse>> GetUserServicesAsync(string id, CancellationToken ct = default);
        public Task<List<OrderResponse>> GetUserOrdersAsync(string id, CancellationToken ct = default);
        Task<List<UserResponse>> SearchUsersAsync(string keyword, CancellationToken ct = default);
        

    }
}
