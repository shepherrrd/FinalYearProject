using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinalYearProject.Api.Application.CQRS.Dashboard.Admin;

public class ViewRegistrationRequests : IRequest<BaseResponse<IEnumerable<Request>>>
{
    internal long AdminID { get; set; }
}

public class ViewRegistrationRequestsHandler : IRequestHandler<ViewRegistrationRequests, BaseResponse<IEnumerable<Request>>>
{
    private readonly FinalYearDBContext _context;

    public ViewRegistrationRequestsHandler(FinalYearDBContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse<IEnumerable<Request>>> Handle(ViewRegistrationRequests request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.AdminID, cancellationToken);
            if (user is null)
                return new BaseResponse<IEnumerable<Request>>(false, "The User Tied to this operation was not found");
            var requests = await _context.RegistrationRequests.AsNoTracking().ToListAsync(cancellationToken);

            return new BaseResponse<IEnumerable<Request>>(true, "Requests Fetched", requests);
        }
        catch (Exception)
        {
            return new BaseResponse<IEnumerable<Request>>(false, "An Error Occured while trying to fetch requests");
        }
        
    }
}
