namespace OrderBookingFormApp.Models.ApiModels
{
    public class EnquiryAM
    {
        public List<EnquiryData>? enquiryData { get; set; }
        public class EnquiryData
        {
            public string? Status { get; set; }
            public string? Bool { get; set; }
            public string? EnquiryType { get; set; }
            public string? CustomerCode { get; set; }
            public string? Title { get; set; }
            public string? CustomerName { get; set; }
            public string? AddressArea { get; set; }
            public string? City { get; set; }
            public string? MobileNumber { get; set; }
            public string? ResidenceNumber { get; set; }
            public string? OfficeNumber { get; set; }
            public string? AlternateMobileNumber { get; set; }
            public string? CommunicateTo { get; set; }
            public string? Email { get; set; }
            public string? Organization { get; set; }
            public string? OrganizationDetails { get; set; }
            public string? PrefferedPaymentMode { get; set; }
            public string? Comments { get; set; }
            public string? VoucherType { get; set; }
            public string? Date { get; set; }
            public string? Time { get; set; }
            public string? Branch { get; set; }
            public string? BuyerType { get; set; }
            public string? Executive { get; set; }
            public string? Evaluator { get; set; }
            public string? SourceOfEnquiry { get; set; }
            public string? EnquiryNumber { get; set; }
            public string? CustomerRequests { get; set; }
            public string? PrincipalBranch { get; set; }
            public string? MakerCategory { get; set; }
            public string? OldVehicleModel { get; set; }
            public string? OldVehicleVariant { get; set; }
            public string? OldVehicleColour { get; set; }
            public string? OldVehicleYearOfModel { get; set; }
            public string? ExpectedPrice { get; set; }
            public string? EvaluatedPrice { get; set; }
            public string? ActivityPlan { get; set; }
            public string? Scheduled { get; set; }
            public string? AssignedTo { get; set; }
            public string? InstructionForFollowUp { get; set; }
            public string? VehicleComments { get; set; }
            public string? VehicleModel { get; set; }
            public string? Vehicle { get; set; }
            public string? Colour { get; set; }
            public string? Description { get; set; }
            public string? Quantity { get; set; }
            public string? LikelyToPurchaseWithin { get; set; }
            public string? ExpectedDate { get; set; }
            public string? EnquiryStatus { get; set; }
            public string? NextFollowupDate { get; set; }
            public string? Day { get; set; }
            public string? PreOwnedActionPlan { get; set; }
            public string? PreOwnedAssignedTo { get; set; }
            public string? PreOwnedActivityPlan { get; set; }
            public string? OpenReason { get; set; }
            public string? PreOwnedInstructionForFollowUp { get; set; }
            public string? ConvertedToBooking { get; set; }
            public string? PermitSales { get; set; }
            public string? EnquiryLost { get; set; }
            public string? EnquiryLostReason { get; set; }
            public string? CoDealer { get; set; }
            public string? PurchasedOtherVehicleMake { get; set; }
            public string? PurchasedOtherVehicleModel { get; set; }
            public string? PreOwnedComments { get; set; }
            public string? Remarks { get; set; }
        }
    }
}
