using FreelanceAPI.Models;
using FreelanceAPI.Models.Enum;
using FreelanceMarketplace.API.Enums;

namespace FreelanceAPI.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<User>> GetUsersPageAsync(int page = 1, int pageSize = 10, CancellationToken ct = default);
        public Task<int> GetUsersCountAsync(CancellationToken ct = default);
        public Task<User?> GetUserByIdAsync(string userId, CancellationToken ct = default);
        public Task<bool> AddUserAsync(User user, CancellationToken ct = default);
        public Task<bool> UpdateUserAsync(User user, CancellationToken ct = default);
        public Task<bool> DeleteUserAsync(string userId, CancellationToken ct = default);
        public Task<bool> ExistsByIdAsync(string userId, CancellationToken ct = default);
        public Task<List<Service>> GetUserServicesAsync(string userId, CancellationToken ct = default);
        public Task<List<Order>> GetUserOrdersAsync(string userId, CancellationToken ct = default);
        Task<List<User>> SearchUsersAsync(string keyword, CancellationToken ct = default);
   


    }
}
