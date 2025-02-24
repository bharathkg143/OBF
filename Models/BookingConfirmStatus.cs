using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderBookingFormApp.Models
{
    [Table("booking_confirm_status")]
    public class BookingConfirmStatus
    {
        [Key]
        public long Id { get; set; }
        public long? CustomerId { get; set; }
        public long? VehicleId { get; set; }
        public string? OrderBookingId { get; set; }
        public string? ConfirmedBookingToken { get; set; }
        public DateTime? ConfirmedBookingAt { get; set; } = DateTime.Now;
    }
}
