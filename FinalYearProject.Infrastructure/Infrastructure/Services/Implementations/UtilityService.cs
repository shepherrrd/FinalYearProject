
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using FinalYearProject.Infrastructure.Infrastructure.Auth;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.DataExtension;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using FinalYearProject.Infrastructure.Infrastructure.Services.Interfaces;
using FinalYearProject.Infrastructure.Data.Entities;
using CsvHelper;
using System.Globalization;
using Newtonsoft.Json;

namespace Payultra.Infrastructure.Services.Implementations
{
    public class UtilityService : IUtilityService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<UtilityService> _logger;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly FinalYearDBContext _context;

        public UtilityService(
            IHttpContextAccessor accessor,
            ILogger<UtilityService> logger,
            IConfiguration config,
            IWebHostEnvironment webHostEnvironment,
            FinalYearDBContext context
        )
        {
            _accessor = accessor;
            _logger = logger;
            _config = config;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        public string GenerateRandomNumber(int length)
        {
            _logger?.LogInformation($"UTILITY_SERVICE Generate_Random_Number => Proccess started");
            var output = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                output.Append(new Random().Next(10));
            }

            _logger?.LogInformation($"UTILITY_SERVICE Generate_Random_Number => Proccess completed");
            return output.ToString();
        }
       

        public static FileStreamResult GetPDFFileStream(string htmlBody, string fileName)
        {
            var converter = new SelectPdf.HtmlToPdf();
            var doc = converter.ConvertHtmlString(htmlBody);
            var stream = new MemoryStream();
            doc.Save(stream);
            doc.Close();
            stream.Position = 0;
            var fileStreamResult = new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{fileName}.pdf"
            };
            return fileStreamResult;
        }

        public string GetPDFBase64String(string htmlBody)
        {
            var converter = new SelectPdf.HtmlToPdf();
            var doc = converter.ConvertHtmlString(htmlBody);
            var stream = new MemoryStream();
            doc.Save(stream);
            doc.Close();
            stream.Position = 0;
            var bytes = stream.ToArray();
            string fileBase64string = Convert.ToBase64String(bytes);
            stream.Dispose();
            return fileBase64string;
        }

        

        

        public BaseResponse<string> VerifySdtmDatasetFormat(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                var headers = csv.Context.Reader.HeaderRecord;

                var requiredHeaders = new[] { "USUBJID", "SEX", "AGE", "TRTGROUP", "HEIGHT", "WEIGHT" };
                var result = requiredHeaders.All(header => headers.Contains(header));
                var records = "";
                if (result)
                {
                    var sdrecords = csv.GetRecords<SDTMDataset>().ToList();
                    records = JsonConvert.SerializeObject(sdrecords);
                }                     
                return new BaseResponse<string>(result, "Validation done", records);
            }
        }

        public BaseResponse<string> VerifyICDDatasetFormat(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                var headers = csv.Context.Reader.HeaderRecord;

                var requiredHeaders = new[] { "Patient_ID", "Diagnosis_Code", "Diagnosis_Description" };
                var result = requiredHeaders.All(header => headers.Contains(header));
                var records = "";
                if (result)
                {
                    var sdrecords = csv.GetRecords<SDTMDataset>().ToList();
                    records = JsonConvert.SerializeObject(sdrecords);
                }
                return new BaseResponse<string>(result, "Validation done", records);
            }
        }
    }
}
