using iText.Html2pdf;
using iText.Kernel.Crypto;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using iText.Signatures;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using iText.Forms.Form.Element;
using iText.Layout.Borders;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Rotativa.AspNetCore;
using OrderBookingFormApp.Models.ViewModels;
using OrderBookingFormApp.Data;
using OrderBookingFormApp.Models;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using OrderBookingFormApp.Services.IServices;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;
using Rotativa.AspNetCore.Options;
using static System.Net.Mime.MediaTypeNames;
using static OrderBookingFormApp.Models.ApiModels.EnquiryAM;

namespace OrderBookingFormApp.Controllers
{
    public class OrderBookingFormController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IEnquiryBookingAPIService _enquiryBookingApiService;
        private readonly IBookingInfoAPIService _bookingInfoApiService;

        public OrderBookingFormController(
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration,
            IEnquiryBookingAPIService enquiryBookingApiService,
            IBookingInfoAPIService bookingInfoApiService
            )
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _enquiryBookingApiService = enquiryBookingApiService;
            _bookingInfoApiService = bookingInfoApiService;
        }

        [HttpGet]
        [Route("order-booking-form")]
        public IActionResult OrderBookingForm()
        {
            SalesBookedData? salesBookedData = new SalesBookedData();
            try
            {
                int? customerId = HttpContext.Session.GetInt32("CustomerID");
                int? vehicleId = HttpContext.Session.GetInt32("VehicleID");
                string? requestSource = HttpContext.Session.GetString("RequestSource");
                string? orderBookingId = HttpContext.Session.GetString("OrderBookingId");

                //if (requestSource == "AUTOSHERPAS")
                //{
                //    salesBookedData = _context.SalesBookedDatas.Where(x => x.CustomerID == customerId && x.VehicleID == vehicleId).OrderByDescending(x => x.Id).FirstOrDefault();
                //}
                //else if(requestSource == "ADVAITHA")
                //{
                //    salesBookedData = _context.SalesBookedDatas.Where(x => x.OrderBookingId == orderBookingId).OrderByDescending(x => x.Id).FirstOrDefault();
                //}

                return View(salesBookedData);
            }
            catch (Exception ex)
            {
                string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;

                ViewBag.ErrorGettingData = exception;
                return View(salesBookedData);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubmitOrderBookingForm(string custSignature, IFormFile custImageFile, string confirmBookData, IFormFile aadharFrontImg, IFormFile aadharBackImg, IFormFile pancardImg)
        {
            try
            {
                long? customerId = HttpContext.Session.GetInt32("CustomerID");
                long? vehicleId = HttpContext.Session.GetInt32("VehicleID");

                ConfirmSalesBooking? confirmSalesBooking = JsonConvert.DeserializeObject<ConfirmSalesBooking>(confirmBookData);

                if (!string.IsNullOrEmpty(customerId.ToString()) 
                    && !string.IsNullOrEmpty(vehicleId.ToString())
                    && !string.IsNullOrEmpty(custSignature)
                    && (custImageFile != null && custImageFile.Length > 0)
                    && !string.IsNullOrEmpty(confirmBookData)
                    && (aadharFrontImg != null && aadharFrontImg.Length > 0 && aadharBackImg != null && aadharBackImg.Length > 0)
                    && (pancardImg != null && pancardImg.Length > 0)
                   )
                {
                    string? drivePath = _configuration["OBFFilesStoragePath"];

                    #region Signature File Saving Section
                    string folderPathForSignatureDocs = Path.Combine(drivePath, "Signatures");
                    var signatureFileName = "sign_" + customerId + "_" + $"{Guid.NewGuid().ToString()}.png";

                    var base64Data = custSignature.Replace("data:image/png;base64,", "");
                    var bytes = Convert.FromBase64String(base64Data);

                    if (!Directory.Exists(folderPathForSignatureDocs))
                    {
                        Directory.CreateDirectory(folderPathForSignatureDocs);
                    }

                    var signatureFileFullPath = Path.Combine(folderPathForSignatureDocs, signatureFileName);
                    await System.IO.File.WriteAllBytesAsync(signatureFileFullPath, bytes);

                    string customerSignedFilePath = "https://kuttukaran.autosherpas.com/PopularSalesOBFUploads/Signatures/" + signatureFileName;
                    #endregion


                    #region Customer Photo File Saving Section
                    string folderPathForCustomerPhotoDocs = Path.Combine(drivePath, "CustomerPhotos");

                    string custPhotoFileName = await SaveFileAsync(custImageFile, folderPathForCustomerPhotoDocs, "photo", customerId.ToString());
                    string customerPhotoFilePath = $"https://kuttukaran.autosherpas.com/PopularSalesOBFUploads/CustomerPhotos/{custPhotoFileName}";
                    #endregion


                    #region Aadhaar Documents or Image File Saving Section  
                    string aadharFrontPhotoFilePath = string.Empty;
                    string aadharBackPhotoFilePath = string.Empty;
                    string aadharUploadedDocFilePath = string.Empty;

                    string folderPathForAadharDocs = Path.Combine(drivePath, "AadhaarID");

                    // Aadhaar Captured front and back image saving
                    if (aadharFrontImg != null && aadharFrontImg.Length > 0 && aadharBackImg != null && aadharBackImg.Length > 0)
                    {
                        // Aadhar front image saving part
                        string aadharFrontFileName = await SaveFileAsync(aadharFrontImg, folderPathForAadharDocs, "aadhaar_front", customerId.ToString());
                        aadharFrontPhotoFilePath = $"https://kuttukaran.autosherpas.com/PopularSalesOBFUploads/AadhaarID/{aadharFrontFileName}";

                        // Aadhar back image saving part
                        string aadharBackFileName = await SaveFileAsync(aadharBackImg, folderPathForAadharDocs, "aadhaar_back", customerId.ToString());
                        aadharBackPhotoFilePath = $"https://kuttukaran.autosherpas.com/PopularSalesOBFUploads/AadhaarID/{aadharBackFileName}";
                    }

                    // Aadhaar uploaded document saving  
                    //if (aadharUploadedDoc != null && aadharUploadedDoc.Length > 0){
                    //    string aadharUploadedDocFileName = await SaveFileAsync(aadharUploadedDoc, folderPathForAadharDocs, "aadhaar_doc", customerId.ToString());
                    //    aadharUploadedDocFilePath = $"https://kuttukaran.autosherpas.com/PopularSalesOBFUploads/AadhaarID/{aadharUploadedDocFileName}";
                    //}
                    #endregion


                    #region Pancard Documents or Image File Saving Section  
                    string pancardPhotoFilePath = string.Empty;
                    string pancardUploadedDocFilePath = string.Empty;

                    string folderPathForPancardDocs = Path.Combine(drivePath, "PancardId");

                    // Pancard image saving part
                    if (pancardImg != null && pancardImg.Length > 0)
                    {
                        string pancardImgFileName = await SaveFileAsync(pancardImg, folderPathForPancardDocs, "pancard", customerId.ToString());
                        pancardPhotoFilePath = $"https://kuttukaran.autosherpas.com/PopularSalesOBFUploads/PancardId/{pancardImgFileName}";
                    }

                    // Pancard uploaded document saving  
                    //if (pancardUploadedDoc != null && pancardUploadedDoc.Length > 0)
                    //{
                    //    string pancardUploadedDocFileName = await SaveFileAsync(pancardUploadedDoc, folderPathForPancardDocs, "pancard_doc", customerId.ToString());
                    //    pancardUploadedDocFilePath = $"https://kuttukaran.autosherpas.com/PopularSalesOBFUploads/PancardId/{pancardUploadedDocFileName}";
                    //}
                    #endregion


                    #region PDF Generation And PDF File Saving Section
                    string obfHtmlContent = OBFHtmlContentForPDFGenaration(confirmSalesBooking, customerSignedFilePath, customerPhotoFilePath, aadharFrontPhotoFilePath, aadharBackPhotoFilePath, pancardPhotoFilePath);

                    string folderPathForObfPdfDocs = Path.Combine(drivePath, "OBForms");
                    var obfPdfFileName = "OBF_" + customerId + "_" + $"{Guid.NewGuid().ToString()}.pdf";

                    var obfPdfFileFullpath = Path.Combine(folderPathForObfPdfDocs, obfPdfFileName);

                    if (!Directory.Exists(folderPathForObfPdfDocs))
                    {
                        Directory.CreateDirectory(folderPathForObfPdfDocs);
                    }

                    using (FileStream fileStream = new FileStream(obfPdfFileFullpath, FileMode.Create))
                    {
                        HtmlConverter.ConvertToPdf(obfHtmlContent, fileStream);
                    }

                    string obfPdfFilePath = "https://kuttukaran.autosherpas.com/PopularSalesOBFUploads/OBForms/" + obfPdfFileName;
                    #endregion


                    #region Remove Old Data If Exists and Saving New to ConfirmSalesBooking Tab
                    List<ConfirmSalesBooking>? removeOldConfirmSalesBookings = await _context.ConfirmSalesBookings.Where(x => x.CustomerID == customerId && x.VehicleID == vehicleId).ToListAsync();

                    if (removeOldConfirmSalesBookings != null && removeOldConfirmSalesBookings.Count > 0)
                    {
                        _context.ConfirmSalesBookings.RemoveRange(removeOldConfirmSalesBookings);
                        await _context.SaveChangesAsync();
                    }

                    confirmSalesBooking.CustomerID = customerId;
                    confirmSalesBooking.VehicleID = vehicleId;
                    confirmSalesBooking.CustomerSignature_FilePath = customerSignedFilePath;
                    confirmSalesBooking.CustomerPhoto_FilePath = customerPhotoFilePath;
                    confirmSalesBooking.ObfPDF_FilePath = obfPdfFilePath;
                    confirmSalesBooking.AadharFrontImg_FilePath = aadharFrontPhotoFilePath;
                    confirmSalesBooking.AadharBackImg_FilePath = aadharBackPhotoFilePath;
                    confirmSalesBooking.AadharUploadDoc_FilePath = aadharUploadedDocFilePath;
                    confirmSalesBooking.PanCardImg_FilePath = pancardPhotoFilePath;
                    confirmSalesBooking.PancardUploadDoc_FilePath = pancardUploadedDocFilePath;
                    confirmSalesBooking.ConfirmedDateTime = DateTime.Now;
                    await _context.ConfirmSalesBookings.AddAsync(confirmSalesBooking);
                    await _context.SaveChangesAsync();
                    #endregion

                    //User Data will be saved to live DB once all saving part done After form submission
                    bool isDataInserted = await InsertUserDataAfterSubmissionAsync(customerId, vehicleId);

                    ConfirmSalesBooking? confirmSalesBookingIsSubmit = await _context.ConfirmSalesBookings.Where(x => x.CustomerID == customerId && x.VehicleID == vehicleId).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    if (isDataInserted)
                    {
                        if (confirmSalesBookingIsSubmit != null)
                        {
                            confirmSalesBookingIsSubmit.isSubmit = true;
                            _context.ConfirmSalesBookings.Update(confirmSalesBookingIsSubmit);
                            await _context.SaveChangesAsync();

                            #region Booking Enquiry API Post Request After Form Submission Section
                            //Getting additional Enquiry Data of customer to send through Post API(for ERP) Once Confirm Booking is submitted by customer
                            EnquiryDataVM? enquiryDataVM = await GetEnquiryDataToPostAPIRequestAsync(customerId, vehicleId);

                            //Already saved customer details
                            SalesBookedData? salesBookedData = await _context.SalesBookedDatas.Where(m => m.CustomerID == customerId && m.VehicleID == vehicleId).OrderByDescending(m => m.Id).FirstOrDefaultAsync();

                            EnquiryAPIResponse apiResponse = new EnquiryAPIResponse();
                            apiResponse.CustomerId = customerId;
                            apiResponse.VehicleId = vehicleId;
                            apiResponse.CustomerName = salesBookedData?.CustomerName;

                            if (enquiryDataVM != null && salesBookedData != null)
                            {
                                //Sending POST Request to API once Enquiry and Booked data are fetched
                                string postResponseMessage = await _enquiryBookingApiService.PostEnquiryDataAsync(enquiryDataVM, salesBookedData, confirmSalesBookingIsSubmit?.ConfirmedDateTime ?? null);

                                if ((postResponseMessage != null && postResponseMessage.Contains("Succussfully Uploaded")) || (postResponseMessage != null && postResponseMessage.Contains("OK")))
                                {
                                    apiResponse.ResponseMsg = postResponseMessage;
                                    apiResponse.IsEnquiryDataPosted = true;
                                }
                                else
                                {
                                    apiResponse.ResponseMsg = postResponseMessage;
                                    apiResponse.IsEnquiryDataPosted = false;
                                }
                            }
                            else
                            {
                                apiResponse.IsEnquiryDataPosted = false;
                                apiResponse.ResponseMsg = $"Enquiry or Booked Data not found or null, booking API Post Request not sent";
                            }
                            await _context.EnquiryAPIResponses.AddAsync(apiResponse);
                            await _context.SaveChangesAsync();
                            #endregion
                        }
                    }
                    else
                    {
                        if (confirmSalesBookingIsSubmit != null)
                        {
                            confirmSalesBookingIsSubmit.isSubmit = false;
                            _context.ConfirmSalesBookings.Update(confirmSalesBookingIsSubmit);
                            await _context.SaveChangesAsync();

                            //If Confirm OBF Data not inserted to Live, then we are not sending Booking API Request
                            EnquiryAPIResponse apiResponse = new EnquiryAPIResponse();
                            apiResponse.CustomerId = customerId;
                            apiResponse.VehicleId = vehicleId;
                            apiResponse.CustomerName = confirmSalesBookingIsSubmit?.CustomerName;
                            apiResponse.IsEnquiryDataPosted = false;
                            apiResponse.ResponseMsg = $"Booking API Request not sent, error: data not inserted to live while submitting Confirm Order Booking form";
                            await _context.EnquiryAPIResponses.AddAsync(apiResponse);
                            await _context.SaveChangesAsync();
                        }
                    }

                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "Please try again..! provide mandatory data before submitting form." });
            }
            catch (Exception ex)
            {
                string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;

                return Json(new { success = false, message = $"An error occurred while saving form. {exception}" });
            }
        }

        [HttpGet]
        [Route("booking-success")]
        public async Task<IActionResult> ConfirmBookingSuccess()
        {
            BookingInfoAPIResponse bookingInfoAPIResponse = new();
            BookingConfirmStatus bookStatus = new();
            try
            {
                long? customerId = HttpContext.Session.GetInt32("CustomerID");
                long? vehicleId = HttpContext.Session.GetInt32("VehicleID");

                ConfirmSalesBooking? confirmedSalesBookingData = await _context.ConfirmSalesBookings.Where(m => m.CustomerID == customerId && m.VehicleID == vehicleId).OrderByDescending(m => m.Id).FirstOrDefaultAsync();
                
                //Storing API Response in BookingInfoAPIResponse Table
                bookingInfoAPIResponse.CustomerId = customerId;
                bookingInfoAPIResponse.VehicleId = vehicleId;
                bookingInfoAPIResponse.CustomerName = confirmedSalesBookingData?.CustomerName;
                bookingInfoAPIResponse.MobileNumber = confirmedSalesBookingData?.MobileNo;

                if (confirmedSalesBookingData != null)
                {
                    //API Call to send Booking information to user through WhatsApp
                    string apiResponse = await _bookingInfoApiService.SendBookingInfoThroughBusinessWhatsAppAsync(confirmedSalesBookingData);

                    if (apiResponse.Contains("submitted"))
                    {
                        bookingInfoAPIResponse.ResponseMsg = apiResponse;
                        bookingInfoAPIResponse.IsBookingSMSSent = true;
                    }
                    else
                    {
                        bookingInfoAPIResponse.ResponseMsg = apiResponse;
                        bookingInfoAPIResponse.IsBookingSMSSent = false;
                    }
                }
                else
                {
                    bookingInfoAPIResponse.ResponseMsg = "Booking Info API Request not sent. User Data not found..!";
                    bookingInfoAPIResponse.IsBookingSMSSent = false;
                }
                await _context.BookingInfoAPIResponses.AddAsync(bookingInfoAPIResponse);

                bookStatus.CustomerId = customerId;
                bookStatus.VehicleId = vehicleId;
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("BookingConfirmationToken")))
                {
                    bookStatus.ConfirmedBookingToken = HttpContext.Session.GetString("BookingConfirmationToken");
                }
                await _context.BookingConfirmStatuses.AddAsync(bookStatus);

                await _context.SaveChangesAsync();
                return View();
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        private async Task<string> SaveFileAsync(IFormFile file, string folderPath, string prefix, string customerId)
        {
            var fileName = $"{prefix}_{customerId}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(folderPath, fileName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"{fileName}";
        }

        private async Task<bool> InsertUserDataAfterSubmissionAsync(long? custId, long? vehiId)
        {
            try
            {
                ConfirmSalesBooking? confirmSalesBooking = await _context.ConfirmSalesBookings.Where(x => x.CustomerID == custId && x.VehicleID == vehiId).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                if (confirmSalesBooking != null)
                {
                    string query = "CALL spInsertLiveData(@inId)";
                    MySqlParameter parameter = new MySqlParameter("@inId", confirmSalesBooking.Id);

                    int dataInserted = await _context.Database.ExecuteSqlRawAsync(query, parameter);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<EnquiryDataVM> GetEnquiryDataToPostAPIRequestAsync(long? custmerId, long? vehicleId)
        {
            EnquiryDataVM? enquiryData = new EnquiryDataVM();
            try
            {
                string query = "CALL bookingapidata(@customerId, @vehicleId)";
                MySqlParameter[] parameter = new[]
                {
                    new MySqlParameter("@customerId",custmerId),
                    new MySqlParameter("@vehicleId", vehicleId)
                };

                var enquiryDataVMs = await _context.Database.SqlQueryRaw<EnquiryDataVM>(query, parameter).ToListAsync();
                if (enquiryDataVMs != null && enquiryDataVMs.Count() > 0)
                {
                    enquiryData = enquiryDataVMs.FirstOrDefault();
                }
                return enquiryData;
            }
            catch (Exception ex)
            {
                return enquiryData;
            }
        }

        private string OBFHtmlContentForPDFGenaration(ConfirmSalesBooking booking, string signature, string customerPhoto, string aadharFrontImg, string aadharBackImg, string panImg)
        {
            //string aadharImageContent = "";
            //string panImageContent = "";

            //string wwwRootPath = _webHostEnvironment.WebRootPath;
            //string imageFolderPath = Path.Combine(wwwRootPath, "Custimg");

            //customerPhoto = imageFolderPath + "/" + "TestPic.jpeg";
            //aadharFrontImg = imageFolderPath + "/" + "TestPic.jpeg";
            //aadharBackImg = imageFolderPath + "/" + "TestPic.jpeg";
            //panImg = imageFolderPath + "/" + "TestPic.jpeg";
            //signature = imageFolderPath + "/" + "sign.PNG";

            //if (!string.IsNullOrEmpty(aadharFrontImg) && !string.IsNullOrEmpty(aadharBackImg))
            //{
            //    aadharImageContent =
            //        $@"<div style=""width: 50%; padding-right: 10px; "" >
            //                 <label style=""display: block; font-weight: bold; margin-bottom:10px"">AADHAR FRONT IMAGE:</label>
            //                 <img class=""obfdocs-img"" src=""{aadharFrontImg}"">
            //           </div>
            //           <div style=""width: 50%; padding-left: 10px;"">
            //                 <label style=""display: block; font-weight: bold;margin-bottom:10px;"">AADHAR BACK IMAGE:</label>
            //                 <img class=""obfdocs-img"" src=""{aadharBackImg}"" >
            //           </div >
            //        ";
            //}

            //if (!string.IsNullOrEmpty(panImg))
            //{
            //    panImageContent =
            //        $@"<div style=""width: 50%; padding-right: 10px;"">
            //                <label style=""display: block; font-weight: bold;margin-bottom:10px"">PAN CARD IMAGE:</label>
            //                <img class=""obfdocs-img"" src=""{panImg}"">
            //           </div>
            //        ";
            //}

            string htmlContent = $@"
        <html>
        <head>
            <style>
                html {{ position: relative; min-height: 100%; }}
                body {{ margin-bottom: 0px; font-family: 'Roboto', sans-serif;color: #333; }}
                .main-container {{ border: 2px solid black; padding: 10px;margin: 0 auto;width: 100%;max-width: 100vw;box-sizing: border-box;min-height: 100vh; }}
                .form-container {{ margin: 10px; width: 100%; }}
                .form-row {{ display: flex; align-items: center; margin-bottom: 5px; flex-wrap: wrap; }}
                .form-label {{ margin-right: 10px; font-size: 16px; font-weight: bold; flex-basis: 100px; flex-shrink: 0; }}
                .form-control {{ flex: 1; min-height: 38px; max-height: 150px;overflow: auto; word-wrap: break-word;overflow-wrap: break-word; box-sizing: border-box; margin-right: 20px; }}
                .form-control:focus {{ box-shadow: none; }}
                .heading-container {{ margin: 10px; background-color: #e0e0e0; height: 40px; display: flex; align-items: center; justify-content: center; }}
                .heading-container h4 {{ color: black; padding: 5px; margin: 0; }}
                .textarea-auto-resize {{ resize: vertical; }}
                .address-field {{ word-wrap: break-word; white-space: pre-wrap; overflow-wrap: break-word; box-sizing: border-box; }}
            </style>
        </head>

        <body>
<div class=""container-fluid main-container"">
     <main role=""main"" class=""pb-3"">
        <div style='text-align: center;padding-top: 0px;'>
            <h1 style=""color: #007bff; font-weight: bold;""><b>ORDER BOOKING FORM</b></h1>
        </div>

<div class="""" style=""margin-left:10px;padding-top: 0px;"">
    <span style=""font-size: 17px;"">
        Dear Customer, Thank you for booking your next vehicle with POPULAR VEHICLES.<br />
        Please check the below details and CONFIRM your booking.
    </span>
</div>

<div id=""divCustomerDetails"">
    <div class=""mt-3 heading-container shadow-sm"">
        <h4 class=""mt-1"">CUSTOMER DETAILS</h4>
    </div>

    <div class=""form-container mt-3"">
        <div class=""form-row"">
            <label style=""font-size: 12px;"" class=""form-label"">CUSTOMER NAME</label>
             <input style=""border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value=""{booking.CustomerName}""></input>
        </div>

        <div class=""form-row"">
            <label style=""font-size: 12px;"" class=""form-label"">MOBILE NO.</label>
            <input style=""border: 1px solid #ced4da;padding: 5px; color: #343a40;"" type=""text"" value='{booking.MobileNo}' class=""form-control""></input>
            <label style=""font-size: 12px;"" class=""form-label"">OTHER CONTACT NO</label>
            <input style=""border: 1px solid #ced4da; padding: 5px; color: #343a40;"" type=""text"" value='{booking.OtherContactNo}' class=""form-control""></input>
        </div>

        <div class=""form-row"">
            <label style=""font-size: 12px;"" class=""form-label"">E-MAIL</label>
            <input style=""border: 1px solid #ced4da;padding: 5px; color: #343a40;"" value='{booking.Email}' class=""form-control""></input>
        </div>

        <div class=""form-row"">
            <label style=""font-size: 12px;"" class=""form-label"">PAN NO.</label>
            <input style=""border: 1px solid #ced4da;padding: 5px; color: #343a40;"" value='{booking.PanCardNo}' class=""form-control shadow-sm""></input>
            <label style=""font-size: 12px;"" class=""form-label"">DOB</label>
            <input style=""border: 1px solid #ced4da;padding: 5px; color: #343a40;"" value='{booking.DOB}'  class=""form-control shadow-sm""></input>
        </div>

        <div class=""form-row"">
            <label style=""font-size: 12px;"" class=""form-label"">FATHER/<br />HUSBAND NAME</label>
            <input style=""border: 1px solid #ced4da;padding: 5px; color: #343a40;"" value='{booking.FatherOrHusbandName}' class=""form-control""></input>
         </div>

        <div class=""form-row"">
            <label style=""font-size: 12px;"" class=""form-label"">ADDRESS</label>
            <textarea style=""height: 55px; width: 100%; overflow: auto;border: 1px solid #ced4da; color: #343a40;font-family: Helvetica, Arial, sans-serif;padding: 5px;"" class=""form-control"">{booking.Address}</textarea>
        </div>

         <div class=""form-row"">
            <label style=""font-size: 12px;"" class=""form-label"">PIN CODE</label>
            <input style=""border: 1px solid #ced4da;padding: 5px; color: #343a40;"" value='{booking.PinCode}' class=""form-control""></input>
            <label style=""font-size: 12px;"" class=""form-label"">VILLAGE</label>
            <input style=""border: 1px solid #ced4da;padding: 5px; color: #343a40;"" value='{booking.Village}' class=""form-control""></input>
        </div>

        <div class=""form-row"">
            <label style=""font-size: 12px;"" class=""form-label"">CITY</label>
            <input style=""border: 1px solid #ced4da;padding: 5px; color: #343a40;"" value='{booking.City}' class=""form-control""></input>
            <label style=""font-size: 12px;"" class=""form-label"">TEHSIL</label>
            <input style=""border: 1px solid #ced4da;padding: 5px; color: #343a40;"" value='{booking.Taluk}' class=""form-control""></input>
        </div>


        <div class=""form-row"">
            <label style=""font-size: 12px;"" class=""form-label"">DISTRICT</label>
            <input style=""border: 1px solid #ced4da;padding: 5px; color: #343a40;"" value='{booking.District}' class=""form-control""></input>
            <label style=""font-size: 12px;"" class=""form-label"">STATE</label>
            <input style=""border: 1px solid #ced4da;padding: 5px; color: #343a40;"" value='{booking.State}' class=""form-control""></input>
        </div>

     </div>
</div>

<div id=""divBookedVehicleDetails"">
    <div class=""mt-3 heading-container shadow-sm"">
        <h4 class=""mt-1"">BOOKED VEHICLE DETAILS</h4>
    </div>

    <div class=""form-container mt-3"">
        <div class=""form-row"">
            <label style="" font-size: 12px;"" class=""form-label"">DMS OBF NO.</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.DmsObfNo}'>
            <label style="" font-size: 12px;"" class=""form-label"">BOOKING DATE</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.BookingDate}'>
        </div>

        <div class=""form-row"">
            <label style="" font-size: 12px;"" class=""form-label"">BOOKED MODEL</label>
            <textarea style=""height: 40px; width: 100%; border: 1px solid #ced4da;padding: 5px; color: #343a40; word-wrap: break-word;font-family: Helvetica, Arial, sans-serif;"" class=""form-control"">{booking.Model}</textarea>
            <label style="" font-size: 12px;"" class=""form-label"">BOOKED VARIANT</label>
            <textarea style=""height: 40px; width: 100%; border: 1px solid #ced4da;padding: 5px; color: #343a40; word-wrap: break-word;font-family: Helvetica, Arial, sans-serif;"" class=""form-control"">{booking.Variant}</textarea>
        </div>

        <div class=""form-row"">
            <label style="" font-size: 12px;"" class=""form-label"">COLOR</label>
            <textarea style=""height: 40px; width: 100%; border: 1px solid #ced4da;padding: 5px; color: #343a40; word-wrap: break-word;font-family: Helvetica, Arial, sans-serif;"" class=""form-control"">{booking.Color}</textarea>
            <label style="" font-size: 12px;"" class=""form-label"">TENTATIVE WAITING PERIOD</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.TentativeWaitPeriod}'>
        </div>

    </div>
</div>

<div id=""divCostVehicleDetails"">
    <div class=""mt-3 heading-container shadow-sm"">
        <h4 class=""mt-1"">COST OF VEHICLE</h4>
    </div>

    <div class=""form-container mt-3"">
        <div class=""form-row"">
            <label style="" font-size: 12px;"" class=""form-label"">EX- SHOWROOM PRICE</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.ExShowroomPrice}'>
            <label style="" font-size: 12px;"" class=""form-label"">REGISTRATION CHARGES</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.RegistrationCharges}'>
        </div>

        <div class=""form-row"">
            <label style="" font-size: 12px;"" class=""form-label"">TEMP. REG CHARGES</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.TempRegCharges}'>
            <label style="" font-size: 12px;"" class=""form-label"">INSURANCE PRICE</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.InsurancePrice}'>
        </div>

        <div class=""form-row"">
            <label style="" font-size: 12px;"" class=""form-label"">EW (OPTIONAL)</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;""  class=""form-control"" value='{booking.EwOptional}'>
            <label style="" font-size: 12px;"" class=""form-label"">ACCESSORIES</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.Accessories}'>
        </div>

        <div class=""form-row"">
            <label style="" font-size: 12px;"" class=""form-label"">OTHERS (IF ANY)</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.OthersIfAny}'>
            <label style="" font-size: 12px;"" class=""form-label"">OTHERS DESCRIBE</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.OthersDescribe}'>
        </div>

        <div class=""form-row"">
            <label style="" font-size: 12px;"" class=""form-label"">DISCOUNT</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.Discount}'>
            <label style="" font-size: 12px;"" class=""form-label"">CGST @ 14%</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.CGST14Percent}'>
        </div>

        <div class=""form-row"">
            <label style="" font-size: 12px;"" class=""form-label"">SGST @ 14% E</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.SGST14Percent}'>
            <label style="" font-size: 12px;"" class=""form-label"">CESS @ 1%</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.CESS1Percent}'>
        </div>

        <div class=""form-row"">
            <label style="" font-size: 12px;"" class=""form-label"">ON ROAD PRICE</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.OnRoadPrice}'>
            <label style="" font-size: 12px;"" class=""form-label"">BOOKING AMOUNT</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.Amount}'>
        </div>

        <div class=""form-row"">
            <label style="" font-size: 12px;"" class=""form-label"">PAYMENT MODE</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.PaymentMode}'>
            <label style="" font-size: 12px;"" class=""form-label"">PAYMENT REFERENCE</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.PaymentReference}'>
        </div>
    </div>
</div>

<div id=""divOtherDetails"">
    <div class=""mt-3 heading-container shadow-sm"">
        <h4 class=""mt-1"">OTHER DETAILS</h4>
    </div>

    <div class=""form-container mt-3"">
        <div class=""form-row"">
            <label style="" font-size: 12px;"" class=""form-label"">EXCHANGE</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.Exchangestatus}'>
            <label style="" font-size: 12px;"" class=""form-label"">EXISTING CAR MODEL</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.ExistingCarModel}'>
        </div>

        <div class=""form-row"">
            <label style="" font-size: 12px;"" class=""form-label"">REGISTRATION NUMBER</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.RegistrationNo}'>
            <label style="" font-size: 12px;"" class=""form-label"">MSIL LISTED CORPORATE</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.MSILListedCorporate}'>
        </div>

        <div class=""form-row"">
            <label style="" font-size: 12px;"" class=""form-label"">CORPORATE NAME</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.CorporateName}'>
            <label style="" font-size: 12px;"" class=""form-label"">FINANCE AVAILED</label>
            <input style="" border: 1px solid #ced4da;padding: 5px; color: #343a40;"" class=""form-control"" value='{booking.Finance}'>
        </div>
    </div>
</div>

<div style=""margin-left:10px;padding-top: 10px;"">
    <span style=""font-size: 17px;"">
        We hereby give our unconditional consent to be contacted by Maruti Suzuki and /its business associates in relation to the products and services of Maruti Suzuki India Limited by means including but not limited to my telephone /Mobile Phone/E-mail/SMS.<br />
        I hereby give my unconditional consent to MSIL to use itself or disclose the data collected by MSIL with its Parent Company and/ or business associates for the purpose of carrying out the essential business functions like business and market development, building and managing external relationships, ensuring data protecion and seamless provision of services, research and development, technology infrastructure and such other purpose as required by law or regulation. I/We acknowledge that Welcome docket has been provided to me. All Pay Order /Demand Draft should be in favor of [Dealership name] payable at [City Name)
    </span>
</div>

<div style=""margin-left: 10px; margin-right: 10px; padding-top: 20px;"">
    <div style=""width: 60%; display: flex; align-items: center;"">
        <label style=""font-size: 16px; flex-shrink: 0; margin: 0; padding-right: 10px;font-weight: bold;"">NAME</label>
        <input type=""text"" style=""flex-grow: 1; border: 1px solid #ced4da; padding: 5px; margin: 0; color: #343a40;"" class=""form-control"" value='{booking.UserEnteredName}'>
    </div>
</div>


<div style=""display: flex; margin-left: 10px; margin-right: 10px; padding-top: 30px; width: 100%;"">
    <div style=""width: 100%;"">
        <div style=""display: flex; margin: 0;"">
            <div style=""width: 50%; padding-right: 10px;"">
                <label style=""display: block; font-weight: bold;margin-bottom:10px"">SIGNATURE:</label>
                <img style=""border: 2px solid #ADD8E6; box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.1); width: 100%; height: auto;max-height: 250px;object-fit: contain;"" src=""{signature}"">
            </div>
        </div>
    </div>
</div>
<div style=""display: flex; margin-left: 10px; margin-right: 10px; padding-top: 30px; width: 100%;page-break-inside: avoid;"">
     <div style=""width: 100%;"">
         <div style=""display: flex; margin: 0;"">
            <div style=""width: 50%; padding-right: 10px;"">
                <label style=""display: block; font-weight: bold;margin-bottom:10px;"">PHOTO:</label>
                <img style=""border: 2px solid #ADD8E6; box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.1); width: 100%; height: auto;max-height: 250px;object-fit: contain;"" src=""{customerPhoto}"">
            </div>
            <div style=""width: 50%; padding-left: 10px;"">
                <label style=""display: block; font-weight: bold;margin-bottom:10px"">PAN CARD IMAGE:</label>
                <img style=""border: 2px solid #ADD8E6; box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.1); width: 100%; height: auto;max-height: 250px;object-fit: contain;"" src=""{panImg}"">
            </div>
         </div>
     </div>
</div>
<div style=""display: flex; margin-left: 10px; margin-right: 10px; padding-top: 30px; width: 100%;page-break-inside: avoid;"">
     <div style = ""width: 100%;"">
         <div style = ""display: flex; margin: 0;"">
            <div style=""width: 50%; padding-right: 10px; "" >
                 <label style=""display: block; font-weight: bold; margin-bottom:10px"">AADHAR FRONT IMAGE:</label>
                 <img style=""border: 2px solid #ADD8E6; box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.1); width: 100%; height: auto;max-height: 250px;object-fit: contain;"" src=""{aadharFrontImg}"">
            </div>
            <div style=""width: 50%; padding-left: 10px;"">
                 <label style=""display: block; font-weight: bold;margin-bottom:10px;"">AADHAR BACK IMAGE:</label>
                 <img style=""border: 2px solid #ADD8E6; box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.1); width: 100%; height: auto;max-height: 250px;object-fit: contain;"" src=""{aadharBackImg}"" >
            </div>
         </div>
    </div>
</div>

        </main>
    </div>
        </body>
        </html>";

            return htmlContent;
        }

        public string ImageToBase64(string imagePath)
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }

    }
}
