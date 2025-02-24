namespace OrderBookingFormApp.CustomMiddleware
{
    public static class DeviceDetectionMiddlewareExtension
    {
        public static IApplicationBuilder UseDeviceDetection(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DeviceDetectionMiddleware>();
        }
    }
}
