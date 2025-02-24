using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using OrderBookingFormApp.Data;
using OrderBookingFormApp.Models;
using OrderBookingFormApp.Models.ViewModels;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using static System.Net.WebRequestMethods;

namespace OrderBookingFormApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string? id, string? orderid, string? token, string? source)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    HttpContext.Session.SetString("RequestSource", "AUTOSHERPAS");

                    string[] userIds = id.Split(',');

                    long customerId = long.Parse(userIds[0]);
                    long vehicleId = long.Parse(userIds[1]);

                    if (!string.IsNullOrEmpty(token) && !string.IsNullOrWhiteSpace(token))
                    {
                        BookingConfirmStatus? customerBookingStatus = _context.BookingConfirmStatuses.Where(x => x.CustomerId == customerId && x.VehicleId == vehicleId && x.ConfirmedBookingToken == token).FirstOrDefault();
                        if (customerBookingStatus != null)
                        {
                            ViewBag.Status = "Warning";
                            ViewBag.Message = "This booking has already been confirmed. For any enquiry please contact excecutives.";
                            return View();
                        }

                        HttpContext.Session.SetString("BookingConfirmationToken", token);
                    }
                    else
                    {
                        ViewBag.Status = "Error";
                        ViewBag.Message = "Booking Confimation Token not found. Please contact excecutives and try again.";
                        return View();
                    }

                    if (customerId != 0 && vehicleId != 0)
                    {
                        string sbStatus = SaveSalesBookedData(customerId, vehicleId);

                        if (sbStatus == "Saved")
                        {
                            HttpContext.Session.SetInt32("CustomerID", Convert.ToInt32(customerId));
                            HttpContext.Session.SetInt32("VehicleID", Convert.ToInt32(vehicleId));
                            return View();
                        }
                        ViewBag.Status = "Error";
                        ViewBag.Message = sbStatus;
                        return View();
                    }
                    ViewBag.Status = "Error";
                    ViewBag.Message = "Incorrect URL. Please check the URL or contact Excecutives and try again.";
                    return View();
                }

                if (!string.IsNullOrEmpty(orderid))
                {
                    HttpContext.Session.SetString("RequestSource", "ADVAITHA");

                    if (!string.IsNullOrEmpty(token) && !string.IsNullOrWhiteSpace(token))
                    {
                        BookingConfirmStatus? customerBookingStatus = _context.BookingConfirmStatuses.Where(x => x.ConfirmedBookingToken == token && x.OrderBookingId == orderid).FirstOrDefault();
                        if (customerBookingStatus != null)
                        {
                            ViewBag.Status = "Warning";
                            ViewBag.Message = "This booking has already been confirmed. For any enquiry please contact excecutives.";
                            return View();
                        }

                        HttpContext.Session.SetString("BookingConfirmationToken", token);
                    }
                    else
                    {
                        ViewBag.Status = "Error";
                        ViewBag.Message = "Booking Confimation Token not found. Please contact excecutives and try again.";
                        return View();
                    }

                    //if (!string.IsNullOrEmpty(orderid))
                    //{
                    //    string sbStatus = await _bookingDetailsApiService.GetSalesBookingDetailsAsync(orderid);

                    //    if (sbStatus == "Saved")
                    //    {
                    //        HttpContext.Session.SetString("OrderBookingId", orderid);
                    //        return View();
                    //    }
                    //    ViewBag.Status = "Error";
                    //    ViewBag.Message = sbStatus;
                    //    return View();
                    //}
                    ViewBag.Status = "Error";
                    ViewBag.Message = "Incorrect URL. Please check the URL or contact Excecutives and try again.";
                    return View();
                }

                ViewBag.Status = "Error";
                ViewBag.Message = "Incorrect URL. Please check the URL or Contact Excecutives and try again.";
                return View();
            }
            catch (Exception ex)
            {
                string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;

                ViewBag.Status = "Error";
                ViewBag.Message = $"Error: {exception}";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> PhoneNumber(string phnNumber)
        {
            try
            {
                string? requestSource = HttpContext.Session.GetString("RequestSource");
                int? customerId = HttpContext.Session.GetInt32("CustomerID");
                int? vehicleId = HttpContext.Session.GetInt32("VehicleID");
                string? orderBookingId = HttpContext.Session.GetString("OrderBookingId");

                string mobileNumber = phnNumber.Trim();
                if ((!mobileNumber.StartsWith("+91") && mobileNumber.Length == 10) || (mobileNumber.StartsWith("+91") && mobileNumber.Length == 13))
                {
                    int otpToSend = GenarateOTP();

                    string otpStatus = await SendOtpSmsAsync(mobileNumber, otpToSend, requestSource ?? "");

                    if (otpStatus == "Success")
                    {
                        HttpContext.Session.SetString("MobileNumber",mobileNumber);

                        GeneratedOTP otpDetails = new GeneratedOTP();

                        if (requestSource == "AUTOSHERPAS")
                        {
                            var removeOtpDetails = await _context.GeneratedOTPs.FirstOrDefaultAsync(m => m.CustomerId == customerId);
                            if (removeOtpDetails != null)
                            {
                                _context.GeneratedOTPs.Remove(removeOtpDetails);
                                await _context.SaveChangesAsync();
                            }

                            otpDetails.CustomerId = customerId;
                            otpDetails.OTP = otpToSend;
                            await _context.GeneratedOTPs.AddAsync(otpDetails);
                            await _context.SaveChangesAsync();
                        }
                        else if (requestSource == "ADVAITHA")
                        {
                            //var removeOtpDetails = await _context.GeneratedOTPs.FirstOrDefaultAsync(m => m.OrderBookingId == orderBookingId);
                            //if (removeOtpDetails != null)
                            //{
                            //    _context.GeneratedOTPs.Remove(removeOtpDetails);
                            //    await _context.SaveChangesAsync();
                            //}

                            //otpDetails.OrderBookingId = orderBookingId;
                            otpDetails.OTP = otpToSend;
                            await _context.GeneratedOTPs.AddAsync(otpDetails);
                            await _context.SaveChangesAsync();
                        }

                        dynamic numbers = new
                        {
                            normalNumber = mobileNumber,
                            maskedNumber = MaskedNumber(mobileNumber)
                        };

                        TempData["MaskedPhoneNumber"] = numbers.maskedNumber;
                        TempData["Success"] = "OTP Sent to your Registered Mobile Number..";
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false, message = $"Error while sending OTP..  {otpStatus}" });
                    }
                }
                return Json(new { success = false, message = "Entered Mobile Number is not valid..!" });
            }
            catch (Exception ex)
            {
                string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;

                return Json(new { success = false, message = $"Error: {exception}" });
            }
        }

        [HttpGet]
        [Route("auth-otp")]
        public IActionResult EnterOTP()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ValidateOTP(string otp)
        {
            try
            {
                int? customerId = HttpContext.Session.GetInt32("CustomerID");
                int? vehicleId = HttpContext.Session.GetInt32("VehicleID");
                string? requestSource = HttpContext.Session.GetString("RequestSource");
                string? orderBookingId = HttpContext.Session.GetString("OrderBookingId");

                long? genereateOTP;

                //if(requestSource == "AUTOSHERPAS")
                //{
                //    genereateOTP = _context.GeneratedOTPs.Where(x => x.CustomerId == customerId).Select(x => x.OTP).FirstOrDefault();
                //}
                //else if(requestSource == "ADVAITHA")
                //{
                //    genereateOTP = _context.GeneratedOTPs.Where(x => x.OrderBookingId == orderBookingId).Select(x => x.OTP).FirstOrDefault();
                //}
                //else
                //{
                //    genereateOTP = 0;
                //}

                //if (genereateOTP != null)
                //{
                //    if (!string.IsNullOrEmpty(otp))
                //    {
                //        if (genereateOTP.ToString() == otp)
                //        {
                //            return Json(new { success = true });
                //        }
                //        else
                //        {
                //            return Json(new { success = false, message = "Entered OTP is not valid. Please try again.." });
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;

                return Json(new { success = false, message = $"Error: {exception}" });
            }
            return View();
        }

        private string SaveSalesBookedData(long customerId, long vehicleId)
        {
            try
            {
                SalesBookedData? removeOldSalesBookedData = _context.SalesBookedDatas.Where(x => x.CustomerID == customerId && x.VehicleID == vehicleId).FirstOrDefault();
                if (removeOldSalesBookedData != null)
                {
                    _context.SalesBookedDatas.Remove(removeOldSalesBookedData);
                    _context.SaveChanges();
                }

                string query = "CALL spGetSalesBookedData(@customerId, @vehicleId)";
                MySqlParameter[] parameters = new[] {
                            new MySqlParameter("@customerId",customerId),
                            new MySqlParameter("@vehicleId",vehicleId)
                };

                List<SalesBookedDataVM>? sbDataList = _context.Database.SqlQueryRaw<SalesBookedDataVM>(query, parameters).ToList();

                if (sbDataList != null && sbDataList.Count > 0)
                {
                    SalesBookedDataVM? sbData = sbDataList.LastOrDefault();

                    if (sbData != null)
                    {
                        SalesBookedData salesBookedData = new SalesBookedData
                        {
                            CustomerID = customerId,
                            VehicleID = vehicleId,
                            CustomerName = sbData.customerName,
                            MobileNo = sbData.mobileNo,
                            PhoneNumbers = sbData.phoneNumbers,
                            OtherContactNo = sbData.otherContactNo,
                            Email = sbData.email,
                            PanCardNo = sbData.panNo,
                            FatherOrHusbandName = sbData.fatherOrHusbandName,
                            DOB = Convert.ToDateTime(sbData.dob).Date.ToString("dd-MM-yyyy"),
                            DOA = Convert.ToDateTime(sbData.doa).Date.ToString("dd-MM-yyyy"),
                            Age = sbData.age,
                            Address = sbData.address,
                            PinCode = sbData.pincode,
                            Village = sbData.village,
                            City = sbData.city,
                            Taluk = sbData.thaluk,
                            District = sbData.district,
                            State = sbData.state,
                            DmsObfNo = sbData.dmsObfNo,
                            BookingDate = Convert.ToDateTime(sbData.bookingdate).Date.ToString("dd-MM-yyyy"),
                            Model = sbData.model,
                            Variant = sbData.variant,
                            Color = sbData.color,
                            TentativeWaitPeriod = sbData.tentativeWaitPeriod,
                            ExShowroomPrice = sbData.exShowroomPrice,
                            RegistrationCharges = sbData.registrationCharges,
                            InsurancePrice = sbData.insurancePrice,
                            TempRegCharges = sbData.tempRegCharges,
                            EwOptional = sbData.ewOptional,
                            Accessories = sbData.accessories,
                            OthersIfAny = sbData.othersIfAny,
                            OthersDescribe = sbData.othersDescribe,
                            Discount = sbData.discount,
                            CGST14Percent = sbData.cgst14Percent,
                            SGST14Percent = sbData.sgst14Percent,
                            CESS1Percent = sbData.cess1Percent,
                            OnRoadPrice = sbData.onRoadPrice,
                            Amount = sbData.amount,
                            PaymentMode = sbData.paymentMode,
                            PaymentReference = sbData.paymentReference,
                            Exchangestatus = sbData.exchangestatus,
                            MSILListedCorporate = sbData.msilListedCorporate,
                            CorporateName = sbData.corporateName,
                            Finance = sbData.finance,
                            ExistingCarModel = sbData.existingCarModel,
                            RegistrationNo = sbData.registrationNo,
                            Expecteddate = Convert.ToDateTime(sbData.expecteddate).Date.ToString("dd-MM-yyyy"),
                            Promiseddate = Convert.ToDateTime(sbData.promiseddate).Date.ToString("dd-MM-yyyy"),
                            CustomerType = sbData.customertype,
                            Profession = sbData.profession,
                            SourceOfEnquiry = sbData.sourcetype,
                            SourceDetails = sbData.sourcetypedetails,
                            ExpectedDateForEnquiryApi = sbData.expecteddate
                        };
                        _context.SalesBookedDatas.Add(salesBookedData);
                        _context.SaveChanges();

                        return "Saved";
                    }
                }
                return "User Booking Data Not Found. Please contact Excecutive..";
            }
            catch (Exception ex)
            {
                string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;

                return $"Error While getting data: {exception}";
            }
        }

        private async Task<string> SendOtpSmsAsync(string mobileNumber, int otp, string source)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            int? vehicleId = HttpContext.Session.GetInt32("VehicleID");
            string? orderBookingId = HttpContext.Session.GetString("OrderBookingId");


            SMSTemplates? smsTemplate = new SMSTemplates();
            string? apiUrl = string.Empty;
            string? smsBody = string.Empty;
            string? message = string.Empty;
            string? msgStatus = string.Empty;
            string? apiResponseContent = string.Empty;

            try
            {
                smsTemplate = await _context.SMSTemplates.Where(x => x.SMSTemplateName == "Autosherpa_otp" && x.IsActive == true).FirstOrDefaultAsync();
                if (smsTemplate != null)
                {
                    if (!string.IsNullOrEmpty(smsTemplate.SMSTemplate))
                    {
                        smsBody = smsTemplate.SMSTemplate.Replace("{{OTP}}", otp.ToString());
                    }

                    apiUrl = smsTemplate.SMSApi + "numbers=" + mobileNumber.ToString() + "&message=" + smsBody;

                    using (var httpClient = new HttpClient())
                    {
                        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();

                            var jsonResponse = Newtonsoft.Json.Linq.JObject.Parse(content);
                            if (jsonResponse["success"] != null && jsonResponse["success"]?.ToString() == "1")
                            {
                                message = "Success";
                                msgStatus = "Success";
                                apiResponseContent = content;
                            }
                            else
                            {
                                msgStatus = "Error";
                                apiResponseContent = jsonResponse["text"]?.ToString() ?? "Unknown error";
                                message = $"{apiResponseContent}";
                            }
                        }
                        else
                        {
                            msgStatus = "Error";
                            string content = await response.Content.ReadAsStringAsync();
                            apiResponseContent = content;
                            message = $"{apiResponseContent}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msgStatus = "Error";
                message = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;

                return message;
            }

            finally
            {
                SMSInteraction smsInteraction = new SMSInteraction();
                if (customerId != null && customerId.HasValue && customerId != 0)
                {
                    smsInteraction.CustomerId = customerId;
                }

                if (!string.IsNullOrEmpty(orderBookingId))
                {
                    //smsInteraction.OrderBookingId = orderBookingId;
                }
                
                smsInteraction.MobileNumber = long.Parse(mobileNumber);
                smsInteraction.Message = smsBody;
                smsInteraction.SmsType = "OTP";
                smsInteraction.MsgStatus = msgStatus;
                smsInteraction.ResponseFromGateway = apiResponseContent;
                smsInteraction.InteractionDateTime = DateTime.Now;

                await _context.SMSInteractions.AddAsync(smsInteraction);
                await _context.SaveChangesAsync();
            }

            return message;
        }

        private int GenarateOTP()
        {
            var random = new Random();
            return random.Next(1000, 9999);
        }

        private string MaskedNumber(string phoneNumber)
        {
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                if (phoneNumber.Length >= 10 && !phoneNumber.StartsWith("+91"))
                {
                    return phoneNumber.Substring(0, 2) + "XXXX" + phoneNumber.Substring(6);
                }
                else if (phoneNumber.Length >= 13 && phoneNumber.StartsWith("+91"))
                {
                    return phoneNumber.Substring(0, 5) + "XXXX" + phoneNumber.Substring(9);
                }
                return phoneNumber;
            }
            return phoneNumber;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
