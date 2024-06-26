﻿using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using FinalYearProject.Infrastructure.Infrastructure.Services.Implementations;
using FinalYearProject.Infrastructure.Infrastructure.Services.Interfaces;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Payultra.Infrastructure.Services.Implementations;
using System.Security.Cryptography;

namespace FinalYearProject.Api.Application.CQRS.Dashboard.Hospital;

public class UploadMedicalDataRequest : IRequest<BaseResponse>
{
#nullable disable
    internal long HospitalId { get; set; }
    public IFormFile SDTMDATA { get; set; }
    public IFormFile ICDDATA { get; set; }
    public MedicalRecordTypeEnum medicalRecordTypes { get; set; }
    public string PublicKey { get; set; }
    public string ExponentKey { get; set; }
}

public class UploadMedicalDataRequestValidator : AbstractValidator<UploadMedicalDataRequest>
{
    public UploadMedicalDataRequestValidator()
    {
        RuleFor(x => x.SDTMDATA).NotEmpty().NotNull();

        RuleFor(x => x.ICDDATA).NotEmpty().NotNull();
    }
}

public class UploadMedicalDataRequestHandler : IRequestHandler<UploadMedicalDataRequest, BaseResponse>
{
    private readonly IUtilityService _utility;
    private readonly FinalYearDBContext _context;
    private readonly IEncryptionService _encryption;
    private readonly IAccountService _accountService;
    private readonly IHybridEncryption _hybrid;
    public UploadMedicalDataRequestHandler(IUtilityService utility,
        FinalYearDBContext context,
        IEncryptionService encryption,
        IAccountService accountService,
        IHybridEncryption hybrid
        )
    {
        _utility = utility;
        _context = context;
        _encryption = encryption;
        _accountService = accountService;
        _hybrid = hybrid;
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
            if (!validateSDTM.Status || !validateICD.Status)
            {
                return new BaseResponse(false, "Invalid Dataset Format");
            }
            //if (user.AccountStatus == AccountStatusEnum.Active && user.UserType == UserType.Hospital)
            //{
            //    var keys = _encryption.GenerateEncryptionKey();
            //    var userrsa = new UserRSA
            //    {
            //        PrivateRSAParameters = JsonConvert.SerializeObject( keys.Data!.privatersa),
            //        PublicRSAParameters = JsonConvert.SerializeObject( keys.Data.publicrsa),
            //        UserID = user.Id,
            //    };
            //    await _context.AddAsync(userrsa, cancellationToken);
            //    await _context.SaveChangesAsync(cancellationToken);
            //    var sendkeys = await _accountService.SendHospitalWelcomeEmailAsync(new SendHospitalKeysRequest
            //    {
            //        Email = user.Email,
            //        FirstName = user.FirstName,
            //        PrivateKeyExponent = keys.Data!.PrivateKeyExponent,
            //        PublicKeyExponent = keys.Data.PublicKeyExponent,
            //        PrivateKeyModulus = keys.Data.PrivateKeyModulus,
            //        PublicKeyModulus = keys.Data.PublicKeyModulus,
            //        UserId = user.Id

            //    }, cancellationToken);
            //}
            var publicKey = new RSAParameters
            {
                Modulus = Convert.FromBase64String(request.PublicKey),
                Exponent = Convert.FromBase64String(request.ExponentKey)
            };

            var ICDBytes = _hybrid.EncryptData(validateICD.Data, publicKey);
            var SDTMBYTES = _hybrid.EncryptData(validateSDTM.Data, publicKey);
            var medicalRecord = new MedicalDataRecords
            {
                HospitalId = user.Id,
                ICDRecordBytes = ICDBytes.Data,
                SDTMRecordBytes = SDTMBYTES.Data,
                RecordType = request.medicalRecordTypes,
                TimeCreated = DateTimeOffset.UtcNow,
                TimeUpdated = DateTimeOffset.UtcNow,
            };
            await _context.AddAsync(medicalRecord, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new BaseResponse(true, "Records Added Successfully");
        }
        catch (Exception)
        {
            return new BaseResponse(false, "An Error Occured While trying to upload new records");
        }

    }
}
