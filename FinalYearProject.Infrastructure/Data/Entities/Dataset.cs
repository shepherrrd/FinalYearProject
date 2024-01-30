

namespace FinalYearProject.Infrastructure.Data.Entities;

public class Dataset
{
}

public class SDTMDataset
{
    public long HospitalID { get; set; }
    public int USUBJID { get; set; }
    public string SEX { get; set; }
    public int AGE { get; set; }
    public string TRTGROUP { get; set; }
    public int HEIGHT { get; set; }
    public int WEIGHT { get; set; }
}

public class ICDDataset
{
    public long HospitalID { get; set; }

    public int Patient_ID { get; set; }
    public string Diagnosis_Code { get; set; }
    public string Diagnosis_Description { get; set; }
}
public class ResearchCenter
{
    public int ResearchCenterId { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
}
public class Request
{
    public int Id { get; set; }
    public long UserID { get; set; }
    public DateTimeOffset DateRequested { get; set; }
    public bool IsApproved { get; set; }
    public string? Documents { get; set; }
}
public class Document
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Path { get; set; }
    public long UserID { get; set; }
}
