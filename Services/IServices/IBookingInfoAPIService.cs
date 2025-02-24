using OrderBookingFormApp.Models;

namespace OrderBookingFormApp.Services.IServices
{
    public interface IBookingInfoAPIService
    {
        Task<string> SendBookingInfoThroughBusinessWhatsAppAsync(ConfirmSalesBooking? confirmedSaleBookingData);
    }
}
