
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using Npgsql.Replication.PgOutput.Messages;

namespace FinalYearProject.Infrastructure.Data.Entities;

public class Dataset
{
}

public class SDTMDataset
{
    public int USUBJID { get; set; }
    public string? SEX { get; set; }
    public int AGE { get; set; }
    public string? TRTGROUP { get; set; }
    public int HEIGHT { get; set; }
    public int WEIGHT { get; set; }
}

public class MedicalDataRecords : BaseEntity
{
    public long HospitalId {  get; set; }
    
    public MedicalRecordTypeEnum RecordType { get; set; }

    public byte[] SDTMRecordBytes { get; set; } = default!;
    public byte[] ICDRecordBytes { get; set; } = default!;

}

public class ICDDataset
{
    public int Patient_ID { get; set; }
    public string Diagnosis_Code { get; set; } = default!;
    public string Diagnosis_Description { get; set; } = default!;
}
public class MedicalDataRecordsResponse : BaseEntity
{
    public string HospitalName { get; set; } = string.Empty;
    public MedicalRecordTypeEnum RecordType { get;  set; }

}
public class ResearchCenter
{
    public int ResearchCenterId { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
}
public class Request : BaseEntity
{
    public long UserID { get; set; }
    public DateTimeOffset DateRequested { get; set; }
    public bool IsApproved { get; set; }
    public UserType UserType { get; set; }
    public string? Documents { get; set; }

}
public class Document
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Path { get; set; }
    public long UserID { get; set; }
}
