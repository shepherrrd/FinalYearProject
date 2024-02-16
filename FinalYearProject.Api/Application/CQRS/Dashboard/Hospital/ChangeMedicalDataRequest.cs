using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinalYearProject.Api.Application.CQRS.Dashboard.Hospital;

public class ChangeMedicalDataRequest : IRequest<BaseResponse>
{
    internal long UserId { get; set; }
    public string PrivateKey { get; set; } = default!;
    public int RequestID { get; set; }
    
    internal bool IsApproved { get; set; } 
}

public class AcceptMedicalDataRequestValidator : AbstractValidator<ChangeMedicalDataRequest>
{
    public AcceptMedicalDataRequestValidator()
    {
        RuleFor(x => x.PrivateKey).NotNull().NotEmpty().When(x => x.IsApproved);
        RuleFor(x => x.RequestID).NotNull().NotEmpty();
    }
}

public class AcceptMedicalDataRequestHandler : IRequestHandler<ChangeMedicalDataRequest, BaseResponse>
{
    private readonly FinalYearDBContext _context;

    public AcceptMedicalDataRequestHandler(FinalYearDBContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse> Handle(ChangeMedicalDataRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId && x.UserType == UserType.Hospital, cancellationToken);
            if (user is null)
                return new BaseResponse(false, "THe User Tied to this operation was not found");

            var medicalrequest = await _context.HospitalRequests.FirstOrDefaultAsync(x => x.Id == request.RequestID && x.HospitalId == user.Id, cancellationToken);
            if (medicalrequest is null)
                return new BaseResponse(false, "The request tied to this operation was noto found");
            medicalrequest.IsApproved = request.IsApproved;
            medicalrequest.TimeUpdated = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
            if (medicalrequest.IsApproved)
            {
                //send email for approvalincluding files
            }
            else
            {
                //send rejection email
            }

            return new BaseResponse(true, medicalrequest.IsApproved ? "Request Approved Successfully" : "Request Rejected Successfully");
        }
        catch (Exception)
        {

            throw;
        }
        
    }
}
