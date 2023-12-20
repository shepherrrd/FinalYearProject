using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using FluentValidation;
using MediatR;

namespace FinalYearProject.Api.Application.CQRS.Registration;

public class RegisterHospitalRequest : IRequest<BaseResponse>
{
    public string? Location { get; set; }
    public string? HospitalName { get; set; }
    public string? HospitalEmail { get; set; }
    public string? WebsiteAddress { get; set; }
    public IFormFile? CacDocument { get; set; }
    public IFormFile? NafdacDocument { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public UserType AccountType { get; set; }
}

public class RegisterHospitalRequestValidator : AbstractValidator<RegisterHospitalRequest>
{
    public RegisterHospitalRequestValidator()
    {
        RuleFor(x => x.Location)
         .NotEmpty()
         .MaximumLength(100);

        RuleFor(x => x.HospitalName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.HospitalEmail)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.WebsiteAddress).NotNull().NotEmpty();
        RuleFor(x => x.WebsiteAddress)
            .Must(BeAValidUrl).When(x => !string.IsNullOrEmpty(x.WebsiteAddress));

       RuleFor(x => x.CacDocument).NotNull().NotEmpty();
        RuleFor(x => x.NafdacDocument).NotNull().NotEmpty();

        RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password field is required.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!#%*?&])[A-Za-z\d@$!#%*?&]{8,}$").WithMessage("Invalid password format");
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("The password and confirmation password do not match.");

        RuleFor(x => x.AccountType)
            .IsInEnum();
    }

    private bool BeAValidUrl(string? url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

}


public class RegisterHospitalRequestHandler : IRequestHandler<RegisterHospitalRequest, BaseResponse>
{
    public async Task<BaseResponse> Handle(RegisterHospitalRequest request, CancellationToken cancellationToken)
    {
        return new BaseResponse(false, "Name");
    }
}