using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderBookingFormApp.Models
{
    [Table("sms_interaction")]
    public class SMSInteraction
    {
        [Key]
        public int Id { get; set; }
        public long? CustomerId { get; set; }
        public long? MobileNumber { get; set; }

        [StringLength(1500)]
        public string? Message { get; set; }
        public string? SmsType { get; set; }
        public string? MsgStatus { get; set; }
        public string? ResponseFromGateway { get; set; }
        public DateTime? InteractionDateTime { get; set; }
    }
}
