using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderBookingFormApp.Models
{
    [Table("sms_templates")]
    public class SMSTemplates
    {
        [Key]
        public int Id { get; set; }
        public string? SMSTemplateId { get; set; }
        public string? SMSTemplateName { get; set; }
        public string? SMSApi { get; set; }
        public string? SMSTemplate { get; set; }
        public string? SMSSenderHeaderId { get; set; }
        public string? SMSRoute { get; set; }
        public string? SMSType { get; set; }
        public string? DealerName { get; set; }
        public bool IsActive { get; set; }
    }
}
