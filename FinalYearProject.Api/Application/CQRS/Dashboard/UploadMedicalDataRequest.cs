using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using FinalYearProject.Infrastructure.Infrastructure.Services.Interfaces;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinalYearProject.Api.Application.CQRS.Dashboard;

public class UploadMedicalDataRequest : IRequest<BaseResponse>
{
#nullable disable
    internal long HospitalId { get; set; }
    public IFormFile SDTMDATA {  get; set; } 
    public IFormFile ICDDATA {  get; set; }
    public List<MedicalRecordTypeEnum> medicalRecordTypes { get; set; }
}

public class UploadMedicalDataRequestValidator : AbstractValidator<UploadMedicalDataRequest>
{
    public UploadMedicalDataRequestValidator()
    {
        RuleFor(x => x.SDTMDATA).NotEmpty().NotNull();
       RuleFor(x => x.SDTMDATA).Must(x => x.FileName.EndsWith(".csv"))
            .WithMessage("The file must be a CSV file.");
        RuleFor(x => x.SDTMDATA.ContentType)
            .Equal("text/csv")
            .WithMessage("The file content type must be 'text/csv'.");

        RuleFor(x => x.ICDDATA).NotEmpty().NotNull();
       RuleFor(x => x.ICDDATA).Must(x => x.FileName.EndsWith(".csv"))
            .WithMessage("The file must be a CSV file.");
        RuleFor(x => x.ICDDATA.ContentType)
            .Equal("text/csv")
            .WithMessage("The file content type must be 'text/csv'.");
    }
}

public class UploadMedicalDataRequestHandler : IRequestHandler<UploadMedicalDataRequest, BaseResponse>
{
    private readonly IUtilityService _utility;
    private readonly FinalYearDBContext _context;

    public UploadMedicalDataRequestHandler(IUtilityService utility,
        FinalYearDBContext context)
    {
        _utility = utility;
        _context = context;
    }

    public async Task<BaseResponse> Handle(UploadMedicalDataRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.HospitalId);
            if (user == null)
            {
                return new BaseResponse(false, "The User tied to this operation was not found");
            }
            var validateSDTM = _utility.VerifySdtmDatasetFormat(request.SDTMDATA);
            var validateICD = _utility.VerifyICDDatasetFormat(request.ICDDATA);
            if (!validateSDTM.Status || validateICD.Status)
            {
                return new BaseResponse(false, "Invalid Dataset Format");
            }

            var medicalRecord = new MedicalDataRecords
            {
                HospitalId = user.Id,
                ICDRecord = validateICD.Data,
                SDTMRecord = validateSDTM.Data,
                RecordType = request.medicalRecordTypes,
                TimeCreated = DateTimeOffset.UtcNow,
                TimeUpdated = DateTimeOffset.UtcNow,
            };
            await _context.AddAsync(medicalRecord, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new BaseResponse(true, "Records Added Successfully");
        }
        catch (Exception )
        {
            return new BaseResponse(false, "An Error Occured While trying to upload new records");
        }
        
    }
}
