using FreelanceAPI.Interfaces;
using FreelanceAPI.Models;
using FreelanceAPI.Requests;
using FreelanceAPI.Responses;
using FreelanceAPI.Services.Interface;
using FreelanceMarketplace.API.Common.Exceptions;
using FreelanceMarketplace.API.Enums;
using M03.RepositoryPattern.Responses;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace FreelanceAPI.Services.implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        public UserService(IUserRepository userRepository, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }
        public async Task<bool> DeleteUserAsync(string userId, CancellationToken ct = default)
        {
            var exists = await ExistsByIdAsync(userId, ct);
            if (!exists)
            {
                return false;
            }
            await _userRepository.DeleteUserAsync(userId, ct);

            return true;
        }

        public async Task<bool> ExistsByIdAsync(string userId, CancellationToken ct = default)
        {
            return await _userRepository.ExistsByIdAsync(userId, ct);
        }

        public async Task<UserResponse?> GetUserByIdAsync(string     userId, CancellationToken ct = default)
        {
            var exists = await _userRepository.GetUserByIdAsync(userId, ct);
            var UserServices = await _userRepository.GetUserServicesAsync(userId, ct);
            if (exists == null)
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            return new UserResponse()
            {
                Id = exists.Id,
                UserName = exists.Name,
                UserEmail = exists.Name,
               
                Services = UserServices.Select(s => new ServiceResponse
                {
                    Description = s.Description,
                    Price = s.Price
                }).ToList()


            };
        }

        public async Task<List<OrderResponse>>GetUserOrdersAsync(string userId, CancellationToken ct = default)
        {
            var orders = await _userRepository.GetUserOrdersAsync(userId, ct);

            if (orders == null || !orders.Any())
                throw new KeyNotFoundException($"No orders found for user with ID {userId}.");

            return orders.Select(order => new OrderResponse
            {
                Id = order.Id,
                BuyerId = order.BuyerId,
                ServiceId = order.ServiceId,
                Status = order.Status,
                OrderDate = order.OrderDate
            }).ToList();
        }
        
        public async Task<int> GetUsersCountAsync(CancellationToken ct = default)
        {
            return await _userRepository.GetUsersCountAsync(ct);
        }

        public async Task<List<ServiceResponse>> GetUserServicesAsync(string userId, CancellationToken ct = default)
        {
            var services = await _userRepository.GetUserServicesAsync(userId, ct);
            if (services == null || !services.Any())
                throw new KeyNotFoundException($"No services found for user with ID {userId}.");
            return services.Select(s => new ServiceResponse
            {
                Title = s.Title,
                Description = s.Description,
                Price = s.Price,
                DeliveryTimeInDays = s.DeliveryTimeInDays,
                UserId = s.UserId,
                IsActive = s.IsActive
            }).ToList();
        }

        public async Task<List<UserResponse>> GetUsersPageAsync(int page = 1, int pageSize = 10, CancellationToken ct = default)
        {
            var users = await _userRepository.GetUsersPageAsync(page, pageSize, ct);
            if (users == null || !users.Any())
                throw new KeyNotFoundException($"No users found for page {page} with page size {pageSize}.");
            return users.Select(user => new UserResponse
            {
                Id = user.Id,
                UserName = user.Name,
                UserEmail = user.Email,
                
                Services = user.Services.Select(s => new ServiceResponse
                {
                    Title = s.Title,
                    Description = s.Description,
                    Price = s.Price,
                    DeliveryTimeInDays = s.DeliveryTimeInDays,
                    UserId = s.UserId,
                    IsActive = s.IsActive
                }).ToList()
            }).ToList();
        }

        public async Task<List<UserResponse>> SearchUsersAsync(string keyword, CancellationToken ct = default)
        {
            var users = await _userRepository.SearchUsersAsync(keyword, ct);
            if (users == null || !users.Any())
                throw new KeyNotFoundException($"No users found for keyword '{keyword}'.");
            return users.Select(user => new UserResponse
            {
                Id = user.Id,
                UserName = user.Name,
                UserEmail = user.Email,
               
                Services = user.Services.Select(s => new ServiceResponse
                {
                    Title = s.Title,
                    Description = s.Description,
                    Price = s.Price,
                    DeliveryTimeInDays = s.DeliveryTimeInDays,
                    UserId = s.UserId,
                    IsActive = s.IsActive
                }).ToList()
            }).ToList();
        }

        public async Task<bool> UpdateUserAsync(
          string userId,
          UpdateUser request,
          CancellationToken ct = default)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId, ct)
                ?? throw new NotFoundException($"User with ID '{userId}' was not found.");

            existingUser.Name = request.Name;
            existingUser.Email = request.Email;

            return await _userRepository.UpdateUserAsync(existingUser, ct);
        }

    }
}
