using OrderBookingFormApp.Models;
using OrderBookingFormApp.Models.ViewModels;

namespace OrderBookingFormApp.Services.IServices
{
    public interface IEnquiryBookingAPIService 
    {
         Task<string> PostEnquiryDataAsync(EnquiryDataVM enquiryData, SalesBookedData salesBookedData, DateTime? bookingConfirmedDateTime);
    }
}
