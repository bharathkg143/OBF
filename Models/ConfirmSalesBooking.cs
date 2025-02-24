using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderBookingFormApp.Models
{
    [Table("confirm_salesbooking")]
    public class ConfirmSalesBooking
    {
        [Key]
        public long Id { get; set; }
        public long? CustomerID { get; set; }
        public long? VehicleID { get; set; }
        public string? CustomerName { get; set; }
        public string? MobileNo { get; set; }
        public string? PhoneNumbers { get; set; }
        public string? OtherContactNo { get; set; }
        public string? Email { get; set; }
        public string? PanCardNo { get; set; }
        public string? FatherOrHusbandName { get; set; }
        public string? DOB { get; set; }
        public string? DOA { get; set; }
        public string? Age { get; set; }
        public string? Address { get; set; }
        public string? PinCode { get; set; }
        public string? Village { get; set; }
        public string? City { get; set; }
        public string? Taluk { get; set; }
        public string? District { get; set; }
        public string? State { get; set; }
        public string? DmsObfNo { get; set; }
        public string? BookingDate { get; set; }
        public string? Model { get; set; }
        public string? Variant { get; set; }
        public string? Color { get; set; }
        public string? TentativeWaitPeriod { get; set; }
        public string? ExShowroomPrice { get; set; }
        public string? RegistrationCharges { get; set; }
        public string? InsurancePrice { get; set; }
        public string? TempRegCharges { get; set; }
        public string? EwOptional { get; set; }
        public string? Accessories { get; set; }
        public string? OthersIfAny { get; set; }
        public string? OthersDescribe { get; set; }
        public string? Discount { get; set; }
        public string? CGST14Percent { get; set; }
        public string? SGST14Percent { get; set; }
        public string? CESS1Percent { get; set; }
        public string? OnRoadPrice { get; set; }
        public string? Amount { get; set; }
        public string? PaymentMode { get; set; }
        public string? PaymentReference { get; set; }
        public string? Exchangestatus { get; set; }
        public string? MSILListedCorporate { get; set; }
        public string? CorporateName { get; set; }
        public string? Finance { get; set; }
        public string? ExistingCarModel { get; set; }
        public string? RegistrationNo { get; set; }
        public string? Expecteddate { get; set; }
        public string? Promiseddate { get; set; }
        public string? CustomerType { get; set; }
        public string? Profession { get; set; }
        public string? SourceOfEnquiry { get; set; }
        public string? SourceDetails { get; set; }

        public string? CustomerSignature_FilePath { get; set; }
        public string? CustomerPhoto_FilePath { get; set; }
        public string? ObfPDF_FilePath { get; set; }
        public string? AadharFrontImg_FilePath { get; set; }
        public string? AadharBackImg_FilePath { get; set; }
        public string? AadharUploadDoc_FilePath { get; set; }
        public string? PanCardImg_FilePath { get; set; }
        public string? PancardUploadDoc_FilePath { get; set; }

        public string? UserEnteredName { get; set; }
        public DateTime? ConfirmedDateTime { get; set; }
        public bool isSubmit { get; set; }
    }
}
