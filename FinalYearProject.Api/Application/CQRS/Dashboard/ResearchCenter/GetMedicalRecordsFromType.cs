using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinalYearProject.Api.Application.CQRS.Dashboard.ResearchCenter;

public class GetMedicalRecordsFromType : IRequest<BaseResponse<IEnumerable<MedicalDataRecordsResponse>>>
{
    internal long UserID { get; set; }
    public MedicalRecordTypeEnum MedicalRecordType { get; set; }    
}

public class GetMedicalRecordsFromTypeHandler : IRequestHandler<GetMedicalRecordsFromType, BaseResponse<IEnumerable<MedicalDataRecordsResponse>>>
{
    private readonly FinalYearDBContext _context;

    public GetMedicalRecordsFromTypeHandler(FinalYearDBContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse<IEnumerable<MedicalDataRecordsResponse>>> Handle(GetMedicalRecordsFromType request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.UserID, cancellationToken);
            if (user is null)
            {
                return new BaseResponse<IEnumerable<MedicalDataRecordsResponse>>(false, "The User Tied to this operation was not found");
            }

            var records = await _context.MedicalDataRecords.Where(x => x.RecordType == request.MedicalRecordType).ToListAsync(cancellationToken);
            var response = new List<MedicalDataRecordsResponse>();
            foreach (var record in records)
            {
                var hospital = await _context.Users.AsNoTracking().Select(x => new {x.Id, x.FirstName}).FirstOrDefaultAsync(x => x.Id == record.HospitalId, cancellationToken);
                var tt = new MedicalDataRecordsResponse
                {
                    Id = record.Id,
                    HospitalName = hospital!.FirstName!,
                    RecordType = record.RecordType,
                    TimeCreated = record.TimeCreated,
                    TimeUpdated = record.TimeUpdated
                };
                response.Add(tt);
            }

            return new BaseResponse<IEnumerable<MedicalDataRecordsResponse>>(true, "Records Fetched Successfully", response);

        }
        catch (Exception)
        {
            return new BaseResponse<IEnumerable<MedicalDataRecordsResponse>>(false, "An Error Occured WHile trying to fetch Records");
        }
    }
}
