using FluentValidation;
using FreelanceAPI.Requests;

namespace FreelanceAPI.Validators
{
    public class CreateServicerequest:AbstractValidator<CreateServiceRequest>
    {
        public CreateServicerequest()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Service title is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Service description is required.")
                .Length(10, 1000).WithMessage("Service description must be between 10 and 1000 characters.");   
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Service price must be greater than 0.")
            .LessThan(1000000).WithMessage("Service price must be less than 1000000.");
            RuleFor(x => x.DeliveryTimeInDays).GreaterThan(0).WithMessage("Delivery time must be greater than 0.")
               .LessThanOrEqualTo(10).WithMessage("Delivery time must be less than or equal to 10 days.");
            
            

        }
    }
}
