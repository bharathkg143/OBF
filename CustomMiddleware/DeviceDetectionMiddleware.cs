namespace OrderBookingFormApp.CustomMiddleware
{
    public class DeviceDetectionMiddleware
    {
        private readonly RequestDelegate _next;

        public DeviceDetectionMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var userAgent = context.Request.Headers["User-Agent"].ToString();

            if (IsMobileDevice(userAgent)) 
            {
               await _next(context);
            }
            else
            {
                context.Response.StatusCode = 404;
            }
        }

        private bool IsMobileDevice(string userAgent)
        {
            return userAgent.Contains("Android") || userAgent.Contains("iPhone") || userAgent.Contains("iPad");
        }


    }
}
