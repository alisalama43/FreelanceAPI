using FreelanceAPI.Models;
using FreelanceAPI.Repositories.Interfaces;
using FreelanceAPI.Responses;
using FreelanceAPI.Services.Interface;
using M03.RepositoryPattern.Requests;

namespace FreelanceAPI.Services.implementation
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepo _reviewRepo;

        // تم تعديل اسم المتغير إلى _reviewRepo ليكون أوضح
        public ReviewService(IReviewRepo reviewRepo)
        {
            _reviewRepo = reviewRepo;
        }

        public async Task<ReviewResponse?> AddAsync(CreateOrderReviewRequest request)
        {
            if (request == null)
                return null;

            // 1. تحويل الـ Request (DTO) إلى Model
            var reviewModel = new Review
            {
             // تأكد من اسم الخاصية لديك في الـ Request
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            // 2. إضافة الكائن وحفظ التغيرات في قاعدة البيانات
            await _reviewRepo.AddAsync(reviewModel);
            await _reviewRepo.SaveChangesAsync();

            // 3. إرجاع الـ Response
            return new ReviewResponse
            {
                Id = reviewModel.Id,
                Rating = reviewModel.Rating,
                Comment = reviewModel.Comment
            };
        }

        public async Task<bool> ExistsForOrderAsync(int orderId)
        {
            return await _reviewRepo.ExistsForOrderAsync(orderId);
        }

        public async Task<ReviewResponse?> GetByIdAsync(int id)
        {
            // 1. انتظار جلب البيانات من الـ Repo
            var review = await _reviewRepo.GetByIdAsync(id);

            if (review == null)
                return null;

            // 2. تحويل الـ Review (Model) إلى ReviewResponse (DTO)
            return new ReviewResponse
            {
                Id = review.Id,
                Rating = review.Rating,
                Comment = review.Comment,
                // أضف بقية الخصائص الموجودة في ReviewResponse هنا
            };
        }

        public async Task<ReviewResponse?> GetByOrderIdAsync(int orderId)
        {
            var review = await _reviewRepo.GetByOrderIdAsync(orderId);

            if (review == null)
                return null;

            return new ReviewResponse
            {
                Id = review.Id,
                Rating = review.Rating,
                Comment = review.Comment,
                // أضف بقية الخصائص الموجودة في ReviewResponse هنا
            };
        }
    }
}