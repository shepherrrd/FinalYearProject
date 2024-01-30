namespace FinalYearProject.Infrastructure.Data.Entities;

public class Requests : BaseEntity
{
    public string Description { get; set; } = default!;
    public bool IsApproved { get; set; }
    public int IrbProposalId { get; set; }
    public long HospitalId { get; set; }
    public long ResearchCenterId { get; set; }

}
