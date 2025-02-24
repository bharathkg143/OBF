using Microsoft.EntityFrameworkCore;
using OrderBookingFormApp.Models;

namespace OrderBookingFormApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<SalesBookedData> SalesBookedDatas { get; set; }
        public DbSet<GeneratedOTP> GeneratedOTPs { get; set; }
        public DbSet<SMSInteraction> SMSInteractions { get; set; }
        public DbSet<ConfirmSalesBooking> ConfirmSalesBookings { get; set; }
        public DbSet<SMSTemplates> SMSTemplates { get; set; }
        public DbSet<EnquiryAPIResponse> EnquiryAPIResponses { get; set; }
        public DbSet<BookingInfoAPIResponse> BookingInfoAPIResponses { get; set; }
        public DbSet<BookingConfirmStatus> BookingConfirmStatuses { get; set; }

    }
}
