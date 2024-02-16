using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinalYearProject.Api.Application.CQRS.Dashboard.Hospital;

public class ChangeHospitalRequestStatus : IRequest<BaseResponse>
{
    internal long HospitalId { get; set; }
    internal bool IsApproved { get; set; }
    public int RequestId { get; set; }
}

public class ChangeHospitalRequestStatusHandler : IRequestHandler<ChangeHospitalRequestStatus, BaseResponse>
{
    private readonly FinalYearDBContext _context;

    public ChangeHospitalRequestStatusHandler(FinalYearDBContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse> Handle(ChangeHospitalRequestStatus request, CancellationToken cancellationToken)
    {
        try
        {
            var Hospital = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.HospitalId && x.UserType == UserType.Hospital);
            if (Hospital is null)
            {
                return new BaseResponse(false, "The Hospital Tied to this operation was not found");
            }
            var requestee = await _context.HospitalRequests.FirstOrDefaultAsync(x => x.Id == request.RequestId);
            if (requestee is null)
            {
                return new BaseResponse(false, "The Request Tied to this operation was not found");
            }
            requestee.IsApproved = request.IsApproved;
            //TODO send email
            return new BaseResponse(true, "Request Changed Successfully");
        }
        catch (Exception ex)
        {

            return new BaseResponse(false, "Something went wrong while trying to update the request , please try again");
        }

    }
}
