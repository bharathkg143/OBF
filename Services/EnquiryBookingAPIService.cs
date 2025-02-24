using OrderBookingFormApp.Data;
using OrderBookingFormApp.Models.ViewModels;
using OrderBookingFormApp.Models;
using OrderBookingFormApp.Services.IServices;
using OrderBookingFormApp.Models.ApiModels;
using static OrderBookingFormApp.Models.ApiModels.EnquiryAM;
using System.Text;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using Microsoft.CodeAnalysis.Elfie.Extensions;

namespace OrderBookingFormApp.Services
{
    public class EnquiryBookingAPIService : IEnquiryBookingAPIService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private readonly string? _apiUrl;
        private readonly string? _authUsername;
        private readonly string? _authPassword;

        public EnquiryBookingAPIService(ApplicationDbContext context, HttpClient httpClient, IConfiguration configuration)
        {
            this._context = context;
            this._httpClient = httpClient;
            this._configuration = configuration;
            _apiUrl = _configuration["EnquiryAPISettings:BaseUrl"];
            _authUsername = _configuration["EnquiryAPISettings:Username"];
            _authPassword = _configuration["EnquiryAPISettings:Password"];
        }

        public async Task<string> PostEnquiryDataAsync(EnquiryDataVM enquiryData, SalesBookedData salesBookedData, DateTime? bookingConfirmedDateTime)
        {
            string respContent = string.Empty;
            try
            {
                DateTime expectBuyDate = Convert.ToDateTime(salesBookedData.ExpectedDateForEnquiryApi);
                DateTime curDate = DateTime.Now;
                var diff = (expectBuyDate - curDate).TotalDays;
                var likelyPurchaseWithin = Math.Max(0, Math.Round(diff)) + 1;

                string enquiryType = string.Empty;
                enquiryType = likelyPurchaseWithin <= 10 ? "Hot" : (likelyPurchaseWithin > 10 && likelyPurchaseWithin <= 30 ? "Warm" : "Cold");

                var enquiryAPI = new EnquiryAM
                {
                    enquiryData = new List<EnquiryData>
                    {
                        new EnquiryData
                        {
                            Status = "Pending",
                            Bool = "1",
                            EnquiryType = salesBookedData.SourceOfEnquiry ?? "Null",
                            CustomerCode = "Null",
                            Title = enquiryData.title ?? "Null",
                            CustomerName = salesBookedData.CustomerName ?? "Null",
                            AddressArea = salesBookedData.Address ?? "Null",
                            City = salesBookedData.City ?? "Null",
                            MobileNumber = salesBookedData.MobileNo ?? "Null",
                            ResidenceNumber = "0000000000",
                            OfficeNumber = "0000000000",
                            AlternateMobileNumber = "0000000000",
                            CommunicateTo = "Null",
                            Email = salesBookedData.Email ?? "Null",
                            Organization = "OTHERS",
                            OrganizationDetails = "OTHERS",
                            PrefferedPaymentMode = "Null",
                            Comments = "Null",
                            VoucherType = "Null",
                            Date = bookingConfirmedDateTime.HasValue ? bookingConfirmedDateTime.Value.Date.ToString("yyyy-MM-dd") : "0000000000",
                            Time = bookingConfirmedDateTime.HasValue ? bookingConfirmedDateTime.Value.ToString("HH:mm:ss") : "00:00",
                            Branch = enquiryData.erpworkshopcode,
                            BuyerType = enquiryData.purchasetype ?? "Null",
                            Executive =  enquiryData.firstName ?? "",
                            Evaluator = enquiryData.evaluator ?? "Null",
                            SourceOfEnquiry = salesBookedData.SourceDetails ?? "Null",
                            EnquiryNumber = enquiryData.AssignedInteractionid.ToString(),
                            CustomerRequests = "Null",
                            PrincipalBranch = enquiryData.PrincipalBranch ?? "Null",
                            MakerCategory = enquiryData.oldbrand ?? "Null",
                            OldVehicleModel = enquiryData.oldmodel ?? "Null",
                            OldVehicleVariant =  enquiryData.oldvariant ?? "Null",
                            OldVehicleColour = "Null",
                            OldVehicleYearOfModel =  enquiryData.yom.ToString() ?? "Null",
                            ExpectedPrice =  enquiryData.ExpectedPrice.ToString() ?? "Null",
                            EvaluatedPrice = "0",
                            ActivityPlan = "Null",
                            Scheduled = "0000-00-00",
                            AssignedTo = "Null",
                            InstructionForFollowUp = "Null",
                            VehicleComments = "Null",
                            VehicleModel = salesBookedData.Model ?? "Null",
                            Vehicle =  enquiryData.variantCode ?? "Null",
                            Colour =  salesBookedData.Color ?? "Null",
                            Description =   salesBookedData.Variant ?? "Null",
                            Quantity = "1",
                            LikelyToPurchaseWithin = likelyPurchaseWithin.ToString(),
                            ExpectedDate = salesBookedData.ExpectedDateForEnquiryApi.ToString() ?? "0000000000",
                            EnquiryStatus = enquiryType ?? "Null",
                            NextFollowupDate =  enquiryData.NextFollowupDate.ToString() ?? "0000000000",
                            Day = "Null",
                            PreOwnedActionPlan = "Null",
                            PreOwnedAssignedTo = "Null",
                            PreOwnedActivityPlan = "Null",
                            PreOwnedComments = "Null",
                            PreOwnedInstructionForFollowUp = "Null",
                            OpenReason = "Interested",
                            ConvertedToBooking = "True",
                            PermitSales = "Null",
                            EnquiryLost = "Null",
                            EnquiryLostReason = "Null",
                            CoDealer = "Null",
                            PurchasedOtherVehicleMake = "Null",
                            PurchasedOtherVehicleModel = "Null",
                            Remarks = "Null"
                        }
                    }
                };

                var encodedByteAuth = Encoding.ASCII.GetBytes($"{_authUsername}:{_authPassword}");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(encodedByteAuth));

                var jsonContent = JsonConvert.SerializeObject(enquiryAPI);

                StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(_apiUrl, httpContent);

                respContent = await response.Content.ReadAsStringAsync();

                return $"{response.StatusCode}, {respContent}";

            }
            catch (Exception ex)
            {
                string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;
                await LogExceptionAsync(ex, "Error occurred while sending erp api request");
                return $"{respContent}, Exception while sending request:{exception}";
            }
        }

        /// <summary>
        /// Logs error messages  if any for more information while sending post api request to send user data to ERP
        /// ErrorFilePath:c:\PopularSales-OBFDocuments\ApiErrorlogs\PostApiErrorLogs.txt
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="additionalInfo"></param>
        /// <returns></returns>
        private async Task LogExceptionAsync(Exception ex, string additionalInfo = "")
        {
            string drivePath = @"C:\PopularSales-OBFDocuments";
            string? logFilePath = Path.Combine(drivePath, "ApiErrorlogs", "PostApiErrorLogs.txt");

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
