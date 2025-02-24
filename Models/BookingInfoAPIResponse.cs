using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderBookingFormApp.Models
{
    [Table("bookinginfo_api_response")]
    public class BookingInfoAPIResponse
    {
        [Key]
        public long Id { get; set; }
        public long? CustomerId { get; set; }
        public long? VehicleId { get; set; }
        public string? CustomerName { get; set; }
        public string? MobileNumber { get; set; }
        public string? ResponseMsg { get; set; }
        public bool IsBookingSMSSent { get; set; }
        public DateTime? DateTimeStamp { get; set; } = DateTime.Now;
    }
}
