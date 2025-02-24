using System.ComponentModel.DataAnnotations;

namespace OrderBookingFormApp.Models.ViewModels
{
    public class SalesBookedDataVM
    {
        public string? customerName { get; set; }
        public string? mobileNo { get; set; }
        public string? phoneNumbers { get; set; }
        public string? otherContactNo { get; set; }
        public string? email { get; set; }
        public string? panNo { get; set; }
        public string? fatherOrHusbandName { get; set; }
        public DateTime? dob { get; set; }
        public DateTime? doa { get; set; }
        public string? age { get; set; }
        public string? address { get; set; }
        public string? pincode { get; set; }
        public string? village { get; set; }
        public string? city { get; set; }
        public string? thaluk { get; set; }
        public string? district { get; set; }
        public string? state { get; set; }
        public string? dmsObfNo { get; set; }
        public DateTime? bookingdate { get; set; }
        public string? model { get; set; }
        public string? variant { get; set; }
        public string? color { get; set; }
        public string? tentativeWaitPeriod { get; set; }
        public string? exShowroomPrice { get; set; }
        public string? registrationCharges { get; set; }
        public string? insurancePrice { get; set; }
        public string? tempRegCharges { get; set; }
        public string? ewOptional { get; set; }
        public string? accessories { get; set; }
        public string? othersIfAny { get; set; }
        public string? othersDescribe { get; set; }
        public string? discount { get; set; }
        public string? cgst14Percent { get; set; }
        public string? sgst14Percent { get; set; }
        public string? cess1Percent { get; set; }
        public string? onRoadPrice { get; set; }
        public string? amount { get; set; }
        public string? paymentMode { get; set; }
        public string? paymentReference { get; set; }
        public string? exchangestatus { get; set; }
        public string? msilListedCorporate { get; set; }
        public string? corporateName { get; set; }
        public string? finance { get; set; }

        public string? existingCarModel { get; set; }
        public string? registrationNo { get; set; }

        public DateTime? expecteddate { get; set; }
        public DateTime? promiseddate { get; set; }
        public string? customertype { get; set; }
        public string? profession { get; set; }
        public string? sourcetype { get; set; }
        public string? sourcetypedetails { get; set; }
    }
}
