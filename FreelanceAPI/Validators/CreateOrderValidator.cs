using FluentValidation;
using M03.RepositoryPattern.Requests;

namespace FreelanceAPI.Validators
{
    public class CreateOrderValidator: AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderValidator() 
        {
            RuleFor(x => x.ServiceId)
               
                .NotEmpty().WithMessage("ServiceId is required.")
                .GreaterThan(0).WithMessage("ServiceId must be greater than 0.");
           
        }
    }
}
