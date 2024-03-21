using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Data.Models;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinalYearProject.Api.Application.CQRS.Dashboard.Hospital;

public class GetHospitalDataRequests : IRequest<BaseResponse<IEnumerable<RequestResponse>>>
{
    internal long UserID { get; set; }

}

public class GetHospitalDataRequestsHandler : IRequestHandler<GetHospitalDataRequests, BaseResponse<IEnumerable<RequestResponse>>>
{
    private readonly FinalYearDBContext _context;

    public GetHospitalDataRequestsHandler(FinalYearDBContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse<IEnumerable<RequestResponse>>> Handle(GetHospitalDataRequests request, CancellationToken cancellationToken)
    {
        try
        {
            // Fix: Properly use async-await
            var hospital = await _context.Users.AsNoTracking()
                .Where(x => x.Id == request.UserID)
                .FirstOrDefaultAsync(cancellationToken);

            if (hospital is null)
                return new BaseResponse<IEnumerable<RequestResponse>>(false, "The Hospital assigned to this Id was not found");

            var response = new List<RequestResponse>();

            // Fix: Use ToListAsync to perform database query once
            var requestResponses = await _context.HospitalRequests
                .Where(x => x.HospitalId == hospital.Id)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // Fix: Avoid asynchronous operations inside the ForEach loop
            foreach (var x in requestResponses)
            {
                var research = await _context.Users.AsNoTracking()
                    .Where(k => k.Id == x.ResearchCenterId)
                    .FirstOrDefaultAsync(cancellationToken);

                var irbProposal = await _context.Documents.AsNoTracking()
                    .Where(k => k.Id == x.IrbProposalId)
                    .FirstOrDefaultAsync(cancellationToken);

                var passports = await _context.ResearchCenterInfo.AsNoTracking()
                    .Where(researchInfo => researchInfo.ID == research!.ResearchCenterInfo)
                    .Select(joinedItem => joinedItem.PassportID)
                    .FirstOrDefaultAsync(cancellationToken);

                var document = await _context.Documents.AsNoTracking()
                    .Where(doc => doc.Id == passports)
                    .Select(joinedItem => new { DocumentPath = joinedItem.Path })
                    .FirstOrDefaultAsync(cancellationToken);

                var rr = new RequestResponse
                {
                    Description = x.Description,
                    Id = x.Id,
                    IrbProposal = irbProposal?.Path!,
                    Name = $"{research?.FirstName} {research?.LastName}",
                    Passport = document?.DocumentPath!,
                    TimeUpdated = x.TimeUpdated
                };

                response.Add(rr);
            }

            return new BaseResponse<IEnumerable<RequestResponse>>(true, "Request Fetched Successfully", response);
        }
        catch (Exception)
        {

            throw;
        }
       
    }
}
