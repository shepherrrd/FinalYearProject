using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Data.Models;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinalYearProject.Api.Application.CQRS.Identity;


public class VIewResearchCenterDashboard : IRequest<BaseResponse<List<DataReuqestDto>>>
{
    internal long UserID { get; set; }
}


public class VIewResearchCenterDashboardHandler : IRequestHandler<VIewResearchCenterDashboard, BaseResponse<List<DataReuqestDto>>>
{
    private readonly FinalYearDBContext _context;
    private readonly ILogger<VIewResearchCenterDashboardHandler> _logger;

    public VIewResearchCenterDashboardHandler(FinalYearDBContext context, ILogger<VIewResearchCenterDashboardHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<BaseResponse<List<DataReuqestDto>>> Handle(VIewResearchCenterDashboard request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.UserID,cancellationToken);
            if (user is null)
            {
                _logger.LogInformation($"VIEW_REQUEST_RESEARCH => USER WITH ID {request.UserID} was null");
                return new BaseResponse<List<DataReuqestDto>>(false, "The User tied to this operation was not found");
            }
            var requests = await _context.HospitalRequests.AsNoTracking().Where(x => x.ResearchCenterId == user.Id).ToListAsync();
            var response = new List<DataReuqestDto>();
            requests.ForEach(async x =>
            {
            var p = new DataReuqestDto
            {
                Description = x.Description,
                status = x.status,
                IsApproved = x.IsApproved,
                TimeCreated = x.TimeCreated,
                TimeUpdated = x.TimeUpdated,
                Id = x.Id,
                IrbProposalId = x.IrbProposalId 
            }; 
               var f = await _context.HospitalInfos.Select(x => new {x.ID,x.HospitalName}).FirstOrDefaultAsync(t => t.ID == x.Id,cancellationToken);
                p.Name = f.HospitalName!;
                response.Add(p);
            });

            return new BaseResponse<List<DataReuqestDto>>(true, "Requests Fetched Successfully", response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An Error Occured ");
            return new BaseResponse<List<DataReuqestDto>>(false, "An Error Occured while trying to fetch Reuqests");
        }
    }
}
