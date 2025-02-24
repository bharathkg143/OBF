using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderBookingFormApp.Models
{
    [Table("enquiry_api_response")]
    public class EnquiryAPIResponse
    {
        [Key]
        public long Id { get; set; }
        public long? CustomerId { get; set; }
        public long? VehicleId { get; set; }
        public string? CustomerName { get; set; }
        public string? ResponseMsg { get; set; }
        public bool IsEnquiryDataPosted { get; set; }
        public DateTime? DateTimeStamp { get; set; } = DateTime.Now;
    }
}
