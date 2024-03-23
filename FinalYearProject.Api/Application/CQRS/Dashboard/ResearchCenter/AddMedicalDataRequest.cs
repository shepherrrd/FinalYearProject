using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using FinalYearProject.Infrastructure.Services.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinalYearProject.Api.Application.CQRS.Dashboard.ResearchCenter;

public class AddMedicalDataRequest : IRequest<BaseResponse>
{
    internal long UserID { get; set; }

    public int MedicalRecordID { get; set; }

    public IFormFile IRBApproval { get; set; } = default!;
    public IFormFile Reason { get; set; } = default!;

    public IFormFile Proposal { get; set; } = default!;
    public string Description { get; set; } = string.Empty;
}


public class AddMedicalDataRequestValidator : AbstractValidator<AddMedicalDataRequest>
{
    public AddMedicalDataRequestValidator()
    {
        RuleFor(x => x.Description).NotNull().NotEmpty();
        RuleFor(x => x.IRBApproval).NotEmpty().NotNull().Must(BeValidFileType).WithMessage("This is not a valid Type");
        RuleFor(x => x.Reason).NotEmpty().NotNull().Must(BeValidFileType).WithMessage("This is not a valid Type");
        RuleFor(x => x.Proposal).NotEmpty().NotNull().Must(BeValidFileType).WithMessage("This is not a valid Type");
    }

    private bool BeValidFileType(IFormFile file)
    {
        if (file == null) return true; // Assuming files are optional. If mandatory, return false.

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".pdf" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return allowedExtensions.Contains(fileExtension);
    }
}


public class AddMedicalDataRequestHandler : IRequestHandler<AddMedicalDataRequest, BaseResponse>
{
    private readonly FinalYearDBContext _context;
    private readonly ICloudinaryService _cloudinary;
    public AddMedicalDataRequestHandler(FinalYearDBContext context,
        ICloudinaryService cloudinary
        )
    {
        _context = context;
        _cloudinary = cloudinary;
    }

    public async Task<BaseResponse> Handle(AddMedicalDataRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.UserID, cancellationToken);
            if (user is null)
            {
                return new BaseResponse(false, "The User Tied to this operation was not found");
            }
            var record = await _context.MedicalDataRecords.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.MedicalRecordID,cancellationToken);
            if (record is null)
                return new BaseResponse(false, "Medical Record not found");
            var reasonstream = new MemoryStream();

            request.Reason.OpenReadStream().CopyTo(reasonstream);
            reasonstream.Position = 0;
            var reasoned = await _cloudinary.UploadFilesAsync(reasonstream, Guid.NewGuid().ToString());

            var IrbApproval = new MemoryStream();
            request.IRBApproval.OpenReadStream().CopyTo(IrbApproval);
            IrbApproval.Position = 0;
            var irb = await _cloudinary.UploadFilesAsync(IrbApproval, Guid.NewGuid().ToString());

            var researchmemoryStream = new MemoryStream();
            request.Proposal.OpenReadStream().CopyTo(researchmemoryStream);
            researchmemoryStream.Position = 0;
            var research = await _cloudinary.UploadFilesAsync(researchmemoryStream, Guid.NewGuid().ToString());

            if (!reasoned.Status || !irb.Status || !research.Status)
            {
                return new BaseResponse(false, "An Error Occured WHile Processing Documents Please Contact Support After Many Tries");
            }

            var reason = new Document
            {
                Path = reasoned.Message,
                Title = request.Reason.FileName,

            };

            var irbinfo = new Document
            {
                Path = irb.Message,
                Title = request.IRBApproval.FileName,

            };

            var researchproposalinfo = new Document
            {
                Path = research.Message,
                Title = request.Proposal.FileName,

            };

            await _context.AddRangeAsync(reason, irbinfo, researchproposalinfo);
            await _context.SaveChangesAsync(cancellationToken);

            var requested = new DataRequests
            {
                IrbProposalId = irbinfo.Id,
                MedicalRecordID = record.Id,
                ResearchCenterId = user.Id,
                IsApproved = false,
                Description = request.Description,
                HospitalId = record.HospitalId,
                status = DataRequestSatusEnum.PENDING,
                TimeCreated = DateTimeOffset.UtcNow,
                TimeUpdated = DateTimeOffset.UtcNow,

            };
            await _context.AddAsync(requested, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new BaseResponse(true, "Request Created Successfully");

        }
        catch (Exception)
        {
            return new BaseResponse(false, "An Error Occured WHile Creating Your Requests");
        }
    }
}
