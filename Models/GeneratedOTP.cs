using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderBookingFormApp.Models
{
    [Table("generated_otp")]
    public class GeneratedOTP
    {
        [Key]
        public int Id { get; set; }

        public long? CustomerId { get; set; }
        public int? OTP { get; set; }
        public DateTime? TimeStamp { get; set; } = DateTime.Now.AddMinutes(1);
    }
}
