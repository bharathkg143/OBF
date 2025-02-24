using Elfie.Serialization;
using iText.Barcodes.Dmcode;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OrderBookingFormApp.Data;
using OrderBookingFormApp.Models;
using OrderBookingFormApp.Services.IServices;

namespace OrderBookingFormApp.Services
{
    public class BookingInfoAPIService : IBookingInfoAPIService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;

        private readonly string? _apiUrl;
        private readonly string? _channel;
        private readonly string? _apiKey;
        private readonly string? _source;
        private readonly string? _sourceName;

        public BookingInfoAPIService(IConfiguration configuration,IHttpContextAccessor httpContextAccessor, ApplicationDbContext context, HttpClient httpClient)
        {
            this._configuration = configuration;
            this._httpContextAccessor = httpContextAccessor;
            this._context = context;
            this._httpClient = httpClient;
            _apiUrl = _configuration["BookingInfoAPISettings:base_url"];
            _channel = _configuration["BookingInfoAPISettings:channel"];
            _apiKey = _configuration["BookingInfoAPISettings:api-key"];
            _source = _configuration["BookingInfoAPISettings:source"];
            _sourceName = _configuration["BookingInfoAPISettings:source_name"];
        }

        public async Task<string> SendBookingInfoThroughBusinessWhatsAppAsync(ConfirmSalesBooking? confirmedSaleBookingData)
        {
            string? customerName = string.Empty;
            string? mobileNumber = string.Empty;
            string? attachFile = string.Empty;
            string? model = string.Empty;
            string? brand = string.Empty;
            string? company = string.Empty;
            string? templateId = string.Empty;
            string? respContent = string.Empty;
            try
            {
                string? loggedMobileNumber = _httpContextAccessor.HttpContext?.Session.GetString("MobileNumber");

                SMSTemplates? SMSTemplateDetails = await _context.SMSTemplates.Where(x => x.SMSTemplateName == "booking_info" && x.IsActive).FirstOrDefaultAsync();

                if (confirmedSaleBookingData != null && SMSTemplateDetails != null && !string.IsNullOrEmpty(_channel) && !string.IsNullOrEmpty(_apiKey) && !string.IsNullOrEmpty(_source) && !string.IsNullOrEmpty(_sourceName))
                {
                    customerName = confirmedSaleBookingData.CustomerName;
                    model = confirmedSaleBookingData.Model;
                    brand = "Maruti Suzuki";
                    company = "Popular vehicles and service";
                    templateId = SMSTemplateDetails.SMSTemplateId;

                    if (!string.IsNullOrEmpty(loggedMobileNumber)) 
                    {
                        mobileNumber = loggedMobileNumber.Length == 10 ? $"91{loggedMobileNumber}" : loggedMobileNumber;
                    }
                    else
                    {
                        mobileNumber = confirmedSaleBookingData?.MobileNo?.Length == 10 ? $"91{confirmedSaleBookingData.MobileNo}" : confirmedSaleBookingData?.MobileNo;
                    }

                    if (!string.IsNullOrEmpty(confirmedSaleBookingData?.ObfPDF_FilePath))
                    {
                        attachFile = Path.GetFileName(confirmedSaleBookingData.ObfPDF_FilePath);
                    }

                    string jsonStringTemplate = JsonConvert.SerializeObject(new
                    {
                        id = templateId,
                        @params = new[] { customerName, brand, model, company, attachFile }
                    });

                    Dictionary<string, string> formUrlEncodeData = new Dictionary<string, string>
                    {
                        { "channel", _channel },
                        { "source", _source},
                        { "destination", mobileNumber },
                        { "src.name", _sourceName },
                        { "template", jsonStringTemplate }
                    };

                    FormUrlEncodedContent formUrlEncodedContent = new(formUrlEncodeData);

                    _httpClient.DefaultRequestHeaders.Add("apikey",_apiKey);
                    _httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");

                    HttpResponseMessage response = await _httpClient.PostAsync(_apiUrl, formUrlEncodedContent);

                    respContent = await response.Content.ReadAsStringAsync();

                    return $"{response.StatusCode}, {respContent}";
                }
                return "SMS Template or User OBF Data is null or not found";

            }
            catch (Exception ex)
            {
                string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;
                await LogExceptionAsync(ex, "Error occurred while sending erp api request");
                return $"{respContent}, Exception while sending request:{exception}";
            }
        }

        /// <summary>
        /// Logs error messages if any, for more information while sending api request to send OBF Submission details and file
        /// ErrorFilePath:c:\PopularSales-OBFDocuments\ApiErrorlogs\BookingInfoThroughBusinessWhatsAppApiErrorLogs.txt
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="additionalInfo"></param>
        /// <returns></returns>
        private async Task LogExceptionAsync(Exception ex, string additionalInfo = "")
        {
            string drivePath = @"C:\PopularSales-OBFDocuments";
            string? logFilePath = Path.Combine(drivePath, "ApiErrorlogs", "BookingInfoThroughBusinessWhatsappApiErrorLogs.txt");

            string? logDirectory = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                await writer.WriteLineAsync($"Timestamp: {DateTime.Now}");
                await writer.WriteLineAsync($"Message: {ex.Message}");
                await writer.WriteLineAsync($"Additional Info: {additionalInfo}");

                if (ex.InnerException != null)
                {
                    await writer.WriteLineAsync($"Inner Exception: {ex.InnerException.Message}");
                    if (ex.InnerException.InnerException != null)
                    {
                        await writer.WriteLineAsync($"Inner Inner Exception: {ex.InnerException.InnerException.Message}");
                    }
                }

                await writer.WriteLineAsync($"Stack Trace: {ex.StackTrace}");
                await writer.WriteLineAsync(new string('-', 50));
            }
        }
    }
}
