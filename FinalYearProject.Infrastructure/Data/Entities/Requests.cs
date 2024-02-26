using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using System.Runtime.CompilerServices;

namespace FinalYearProject.Infrastructure.Data.Entities;

public class DataRequests : BaseEntity
{
    public string Description { get; set; } = default!;
    public bool IsApproved { get; set; }
    public int IrbProposalId { get; set; }
    public long HospitalId { get; set; }
    public long ResearchCenterId { get; set; }
    public DataRequestSatusEnum status { get; set; }
    public long MedicalRecordID { get; set; }

   

}


