using FreelanceAPI.Models;
using FreelanceAPI.Repositories.Interfaces;
using FreelanceAPI.Requests;
using FreelanceAPI.Responses;
using FreelanceAPI.Services.Interface;
using FreelanceMarketplace.API.Common.Exceptions;
using Microsoft.Extensions.Caching.Memory;


namespace FreelanceAPI.Services.implementation
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMemoryCache memoryCache;
        public ServiceService(IServiceRepository serviceRepository, IMemoryCache memoryCache)
        {
            _serviceRepository = serviceRepository;
            this.memoryCache = memoryCache;
        }

        public async Task<ServiceResponse> AddServiceAsync(CreateServiceRequest service, CancellationToken ct = default)
        {
           
            var Service = new Service
            {
                Title = service.Title,
                Description = service.Description,
                Price = service.Price,
                DeliveryTimeInDays = service.DeliveryTimeInDays,
                UserId = service.UserId

            };
            memoryCache.Remove("ServicesBySellerId");

            await _serviceRepository.AddServiceAsync(Service, ct);
            return new ServiceResponse
            {
                
                Title = Service.Title,
                Description = Service.Description,
                Price = Service.Price,
                DeliveryTimeInDays = Service.DeliveryTimeInDays,
                UserId = Service.UserId,
                IsActive=Service.IsActive
            };
        }

        public async Task<bool> DeleteServiceAsync(int serviceId, CancellationToken ct = default)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(serviceId, ct)
                ?? throw new NotFoundException($"Service with id '{serviceId}' was not found.");

            service.IsActive = false;

            await _serviceRepository.UpdateServiceAsync(service, ct);
            memoryCache.Remove("ServicesBySellerId");

            return true;
        }

        public Task<bool> ExistsByIdAsync(int serviceId, CancellationToken ct = default)
        {
            var exists = _serviceRepository.ExistsByIdAsync(serviceId, ct).Result;
            if (!exists)
            {
                throw new NotFoundException($"Service with id '{serviceId}' was not found.");
            }
            return Task.FromResult(exists);
        }

        public async Task<ServiceResponse?> GetServiceByIdAsync(int serviceId, CancellationToken ct = default)
        {
            var result = await _serviceRepository.GetServiceByIdAsync(serviceId, ct);

            if (result == null)
                return null;

            return new ServiceResponse
            {
                
                Title = result.Title,
                Description = result.Description,
                Price = result.Price,
                DeliveryTimeInDays = result.DeliveryTimeInDays,
                UserId = result.UserId,
                IsActive = result.IsActive
            };
        }

        public async Task<List<ServiceResponse>> GetServicesBySellerIdAsync(string sellerId, CancellationToken ct = default)
        {

            return await memoryCache.GetOrCreate("ServicesBySellerId", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                entry.Size = 5;
                var services = await _serviceRepository.GetServicesBySellerIdAsync(sellerId, ct);
                if (services == null)
                {
                    throw new Exception("Not found");
                }
                var serviceResponses = services.Select(s => new ServiceResponse
                {
                    Title = s.Title,
                    Description = s.Description,
                    Price = s.Price,
                    DeliveryTimeInDays = s.DeliveryTimeInDays,
                    UserId = s.UserId,
                    IsActive = s.IsActive
                }).ToList() ?? [];
                return serviceResponses!;


            })!;
         
          
        }

   

        public async Task<int> GetServicesCountAsync(CancellationToken ct = default)
        {
            return await _serviceRepository.GetServicesCountAsync(ct);
            
        }

    public async Task<List<ServiceResponse>> GetServicesPageAsync(int page = 1, int pageSize = 10,  CancellationToken ct = default)
        {
           var cacheKey = $"Services";
            if (memoryCache.TryGetValue(cacheKey,out List<ServiceResponse>? ServicesCache))
            {
                return ServicesCache!;
            }
           
            var services = await _serviceRepository.GetServicesPageAsync(page, pageSize, ct);
            ServicesCache = services.Select(s => new ServiceResponse
            {

                Title = s.Title,
                Description = s.Description,
                Price = s.Price,
                DeliveryTimeInDays = s.DeliveryTimeInDays,
                UserId = s.UserId,
                IsActive = s.IsActive
            }).ToList() ?? [];
            memoryCache.Set(cacheKey, ServicesCache, new MemoryCacheEntryOptions 
            {
               AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                Size = 5
            });

            if (services==null)
                throw new Exception("Not found");

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

        public async Task<List<ServiceResponse>> SearchServicesAsync(string keyword, CancellationToken ct = default)
        {
            var services = await _serviceRepository.SearchServicesAsync(keyword, ct);
          

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

        public async Task<ServiceResponse> UpdateServiceAsync(UpdateServiceRequest request, CancellationToken ct = default)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(request.Id, ct)
                ?? throw new NotFoundException($"Service with id '{request.Id}' was not found.");

            service.Title = request.Title;
            service.Description = request.Description;
            service.Price = request.Price;
            service.DeliveryTimeInDays = request.DeliveryTimeInDays;

            await _serviceRepository.UpdateServiceAsync(service, ct);
            memoryCache.Remove("ServicesBySellerId");

            return new ServiceResponse
            {
                
                Title = service.Title,
                Description = service.Description,
                Price = service.Price,
                DeliveryTimeInDays = service.DeliveryTimeInDays,
                UserId = service.UserId,
                IsActive = service.IsActive
            };
        }
       
    }
}
