﻿using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinalYearProject.Api.Application.CQRS.Dashboard.Admin;

public class UpdateRegistrationStatusRequest : IRequest<BaseResponse>
{
    internal long AdminID { get; set; }
    public int RegistrationID { get; set; }

    public bool IsApproved { get; set; }
}


public class UpdateRegistrationStatusRequestHandler : IRequestHandler<UpdateRegistrationStatusRequest, BaseResponse>
{
    private readonly FinalYearDBContext _context;

    public UpdateRegistrationStatusRequestHandler(FinalYearDBContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse> Handle(UpdateRegistrationStatusRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.AdminID,cancellationToken);
            if (user is null)
            {
                return new BaseResponse(false, "The User Tied to this operation was not found");
            }

            var registration = await _context.RegistrationRequests.FirstOrDefaultAsync(x => x.Id == request.RegistrationID, cancellationToken);
            if (registration is null)
                return new BaseResponse(false, "The registration request tied to this operation was not found");
            if (request.IsApproved)
            {
                registration.IsApproved = true; 
                var registrationed = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.RegistrationID, cancellationToken);
                registrationed!.AccountStatus =  AccountStatusEnum.Active;
                await _context.SaveChangesAsync(cancellationToken);
            }

            return new BaseResponse(true, request.IsApproved ?  "Approved Successfully " : "Rejected successfully");
        }
        catch (Exception )
        {
            return new BaseResponse(false, "An Error occured while trying to change request status");
        }
    }
}
