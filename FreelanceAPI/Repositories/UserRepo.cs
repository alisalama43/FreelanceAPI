using FreelanceAPI.Data;
using FreelanceAPI.Interfaces;
using FreelanceAPI.Models;

using FreelanceMarketplace.API.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FreelanceAPI.Repositories
{
    public class UserRepo(AppDbContext _context) : IUserRepository
    {
        
       
        public async Task<bool> AddUserAsync(User user, CancellationToken ct = default)
        {
            _context.Users.Add(user);
            return await _context.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> DeleteUserAsync(string Id, CancellationToken ct = default)
        {
           var user = await _context.Users.FindAsync(Id);
            _context.Users.Remove(user);
            return await _context.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> ExistsByIdAsync(string userId, CancellationToken ct = default  )=> await _context.Users.AnyAsync(u => u.Id == userId, ct);
        

        public async Task<User?> GetUserByIdAsync(string userId, CancellationToken ct = default)
        {
            return await _context.Users.FindAsync(userId, ct);
        }

        public async Task<List<Order>> GetUserOrdersAsync(string userId, CancellationToken ct = default)
        {

            return await _context.Orders
                .Where(o => o.BuyerId == userId)
                .ToListAsync(ct);
        }

   

        public async Task<int> GetUsersCountAsync(CancellationToken ct = default)
        {
            return await _context.Users.CountAsync(ct);
        }

        public async Task<List<Service>> GetUserServicesAsync(string userId, CancellationToken ct = default)
        {
           return await _context.Services
                .Where(s => s.UserId == userId)
                .ToListAsync(ct);
        }

        public async Task<List<User>> GetUsersPageAsync(int page = 1, int pageSize = 10, CancellationToken ct = default)
        {
            var users = await _context.Users .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync(ct);

            return users;

        }

        public async Task<List<User>> SearchUsersAsync(string keyword, CancellationToken ct = default)
        {
            return await _context.Users
                .Where(u => u.Name.Contains(keyword) ||
                            u.Email.Contains(keyword))
                .ToListAsync(ct);
        }

        public async Task<bool> UpdateUserAsync(User user, CancellationToken ct = default)
        {
            var existingUser = await _context.Users.FindAsync(user.Id, ct       );
            if (existingUser == null)
                return false;
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.PasswordHash = user.PasswordHash;
            return await _context.SaveChangesAsync(ct) > 0;


        }
    }
}


