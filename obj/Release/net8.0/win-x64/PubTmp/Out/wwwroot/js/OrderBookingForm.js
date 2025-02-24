var curLat, curLong;
document.addEventListener('DOMContentLoaded', function () {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            function (position) {
                curLat = position.coords.latitude;
                curLong = position.coords.longitude;
            }
        )
    }
});

/*** Start Signature related capture functionalities ***/
var canvas = document.getElementById('canvas');
var ctx = canvas.getContext("2d");
var signaturePad = new SignaturePad(canvas);

resizeCanvas();
function resizeCanvas() {
    var data;
    if (!signaturePad.isEmpty()) {
        data = signaturePad.toDataURL();
    }
    var ratio = Math.max(window.devicePixelRatio || 1, 1);
    canvas.width = canvas.offsetWidth * ratio;
    canvas.height = canvas.offsetHeight * ratio;
    ctx.scale(ratio, ratio);

    signaturePad.clear();
    if (data) {
        signaturePad.fromDataURL(data);
    }
}
window.addEventListener("resize", resizeCanvas);

var hiddenCanvas = document.createElement('canvas');
var hiddenCtx = hiddenCanvas.getContext('2d');

document.getElementById('btnErase').addEventListener('click', function () {
    signaturePad.clear();
});
/*** End Signature related capture functionalities ***/


/*** Start User Image Capture Functionality ***/
let capturedImageElement; //variable to store the captured image element For display captured image
let capturedFile; // To store the file object to send to server

document.getElementById('capture-button').addEventListener('change', function (event) {
    // Save signature data in session storage
    const signatureData = signaturePad.toDataURL();
    sessionStorage.setItem('signatureData', signatureData);

    clearImage();

    // Handle the captured file
    const file = event.target.files[0];
    if (file) {
        //capturedFile = file;
        //const reader = new FileReader();
        //reader.onload = function (event) {
        //    const imageDataURL = event.target.result;
        //    displayCapturedImage(imageDataURL);
        //};
        //reader.readAsDataURL(file);
        compressImage(file, 800, 0.9, function (compressedFile) {
            capturedFile = compressedFile;

            const reader = new FileReader();
            reader.onload = function (event) {
                const imageDataURL = event.target.result;
                displayCapturedImage(imageDataURL);
            };
            reader.readAsDataURL(compressedFile);
        });
    }

    // Restore saved signature if available
    const savedSignature = sessionStorage.getItem('signatureData');
    if (savedSignature) {
        signaturePad.fromDataURL(savedSignature);
    }
});

function displayCapturedImage(imageDataURL) {
    const image = new Image();
    image.src = imageDataURL;
    image.style.maxWidth = '100%';
    image.style.maxHeight = '100%';
    image.className = 'camera-preview';
    image.style.objectFit = 'cover';

    // Remove previous image, if any
    if (capturedImageElement) {
        capturedImageElement.parentNode.removeChild(capturedImageElement);
    }

    // Append the new image
    const displayCapturedImage = document.querySelector('#divCaptureImage');
    displayCapturedImage.innerHTML = '';
    displayCapturedImage.appendChild(image);
    capturedImageElement = image;
}

document.getElementById('btnRetake').addEventListener('click', function () {
    document.getElementById('capture-button').click();
});

function clearImage() {
    if (capturedImageElement) {
        capturedImageElement.parentNode.removeChild(capturedImageElement);
        capturedImageElement = null;
    }
}
/*** End User Image Capture Functionality ***/


/*** Start Aadhaar Front Image Capture Functionality ***/
let aadhaarFrontImageCaptureElement; //variable to store the captured aadhaar front image element For display captured image
let aadhaarFrontImageCapturedFile; // To store the file object to send to server

document.getElementById('adhar-front-capture-btn').addEventListener('change', function (event) {
    const signatureData = signaturePad.toDataURL();
    sessionStorage.setItem('signatureData', signatureData);

    clearAadhaarFrontImage();

    const aadhaarFrontImgFile = event.target.files[0];
    if (aadhaarFrontImgFile) {
        //aadhaarFrontImageCapturedFile = aadhaarFrontImgFile;
        //const reader = new FileReader();
        //reader.onload = function (event) {
        //    const aadhaarFrontImageDataURL = event.target.result;
        //    displayAadhaarFrontCapturedImage(aadhaarFrontImageDataURL);
        //};
        //reader.readAsDataURL(aadhaarFrontImgFile);

        compressImage(aadhaarFrontImgFile, 800, 0.7, function (compressedFile) {
            aadhaarFrontImageCapturedFile = compressedFile;

            const reader = new FileReader();
            reader.onload = function (event) {
                const aadhaarFrontImageDataURL = event.target.result;
                displayAadhaarFrontCapturedImage(aadhaarFrontImageDataURL);
            };
            reader.readAsDataURL(compressedFile);
        });
    }

    const savedSignature = sessionStorage.getItem('signatureData');
    if (savedSignature) {
        signaturePad.fromDataURL(savedSignature);
    }
});

function displayAadhaarFrontCapturedImage(imageDataURL) {
    const aadhaarFrontImage = new Image();
    aadhaarFrontImage.src = imageDataURL;
    aadhaarFrontImage.style.maxWidth = '100%';
    aadhaarFrontImage.style.maxHeight = '100%';
    aadhaarFrontImage.className = 'camera-preview';
    aadhaarFrontImage.style.objectFit = 'cover';

    if (aadhaarFrontImageCaptureElement) {
        aadhaarFrontImageCaptureElement.parentNode.removeChild(aadhaarFrontImageCaptureElement);
    }

    const displayAadhaarFrontCapturedImage = document.querySelector('#adhar-front-capture-image');
    displayAadhaarFrontCapturedImage.innerHTML = '';
    displayAadhaarFrontCapturedImage.appendChild(aadhaarFrontImage);
    aadhaarFrontImageCaptureElement = aadhaarFrontImage;
}

document.getElementById('adhar-front-retake-btn').addEventListener('click', function () {
    document.getElementById('adhar-front-capture-btn').click();
});

function clearAadhaarFrontImage() {
    if (aadhaarFrontImageCaptureElement) {
        aadhaarFrontImageCaptureElement.parentNode.removeChild(aadhaarFrontImageCaptureElement);
        aadhaarFrontImageCaptureElement = null;
    }
}
/*** End Aadhaar Front Image Capture Functionality ***/


/*** Start Aadhaar Back Image Capture Functionality ***/
let aadhaarBackImageCaptureElement; //variable to store the captured aadhaar back image element For display captured image
let aadhaarBackImageCapturedFile; // To store the file object to send to server

document.getElementById('adhar-back-capture-btn').addEventListener('change', function (event) {
    const signatureData = signaturePad.toDataURL();
    sessionStorage.setItem('signatureData', signatureData);

    clearAadhaarBackImage();

    const aadhaarBackImgFile = event.target.files[0];
    if (aadhaarBackImgFile) {
        //aadhaarBackImageCapturedFile = aadhaarBackImgFile;
        //const reader = new FileReader();
        //reader.onload = function (event) {
        //    const aadhaarBackImageDataURL = event.target.result;
        //    displayAadhaarBackCapturedImage(aadhaarBackImageDataURL);
        //};
        //reader.readAsDataURL(aadhaarBackImgFile);

        compressImage(aadhaarBackImgFile, 800, 0.7, function (compressedFile) {
            aadhaarBackImageCapturedFile = compressedFile;

            const reader = new FileReader();
            reader.onload = function (event) {
                const aadhaarBackImageDataURL = event.target.result;
                displayAadhaarBackCapturedImage(aadhaarBackImageDataURL);
            };
            reader.readAsDataURL(compressedFile);
        });
    }

    const savedSignature = sessionStorage.getItem('signatureData');
    if (savedSignature) {
        signaturePad.fromDataURL(savedSignature);
    }
});

function displayAadhaarBackCapturedImage(imageDataURL) {
    const aadhaarBackImage = new Image();
    aadhaarBackImage.src = imageDataURL;
    aadhaarBackImage.style.maxWidth = '100%';
    aadhaarBackImage.style.maxHeight = '100%';
    aadhaarBackImage.className = 'camera-preview';
    aadhaarBackImage.style.objectFit = 'cover';

    if (aadhaarBackImageCaptureElement) {
        aadhaarBackImageCaptureElement.parentNode.removeChild(aadhaarBackImageCaptureElement);
    }

    const displayAadhaarBackCapturedImage = document.querySelector('#adhar-back-capture-image');
    displayAadhaarBackCapturedImage.innerHTML = '';
    displayAadhaarBackCapturedImage.appendChild(aadhaarBackImage);
    aadhaarBackImageCaptureElement = aadhaarBackImage;
}

document.getElementById('adhar-back-retake-btn').addEventListener('click', function () {
    document.getElementById('adhar-back-capture-btn').click();
});

function clearAadhaarBackImage() {
    if (aadhaarBackImageCaptureElement) {
        aadhaarBackImageCaptureElement.parentNode.removeChild(aadhaarBackImageCaptureElement);
        aadhaarBackImageCaptureElement = null;
    }
}
/*** End Aadhaar Back Image Capture Functionality ***/


/*** Start Aadhaar document file upload, if not capture ***/
//function triggerAadhaarUpload() {
//    document.getElementById('aadhar-doc-upload').click();
//}

//let aadhaarUploadedDocFile;
//document.getElementById('aadhar-doc-upload').addEventListener('change', function (event) {
//    console.log('Data captured ', event.target);
//    const file = event.target.files[0];

//    if (file) {
//        const allowedTypes = ['application/pdf', 'image/jpeg', 'image/png'];
//        if (!allowedTypes.includes(file.type)) {
//            toastr.options.positionClass = 'toast-bottom-right';
//            toastr.options.toastClass = "global-custom-toast",
//                toastr.warning('Please upload a valid file (PDF, JPG, JPEG, PNG).');
//            return;
//        }

//        aadhaarUploadedDocFile = file;

//        const previewContainer = document.getElementById('aadhar-doc-preview');
//        previewContainer.innerHTML = '';

//        const fileNameElement = document.createElement('p');
//        fileNameElement.textContent = `${file.name}`;
//        fileNameElement.className = 'mt-0 text-secondary';

//        previewContainer.appendChild(fileNameElement);
//    }
//});
/*** End Aadhaar document file upload, if not capture ***/


/*** Start Pancard Image Capture Functionality ***/
let pancardImageCaptureElement; //variable to store the captured aadhaar back image element For display captured image
let pancardImageCapturedFile; // To store the file object to send to server

document.getElementById('pancard-capture-btn').addEventListener('change', function (event) {
    const signatureData = signaturePad.toDataURL();
    sessionStorage.setItem('signatureData', signatureData);

    clearPancardImage();

    const pancardImgFile = event.target.files[0];
    if (pancardImgFile) {
        //pancardImageCapturedFile = pancardImgFile;
        //const reader = new FileReader();
        //reader.onload = function (event) {
        //    const pancardImageDataURL = event.target.result;
        //    displayPancardCapturedImage(pancardImageDataURL);
        //};
        //reader.readAsDataURL(pancardImgFile);

        compressImage(pancardImgFile, 800, 0.7, function (compressedFile) {
            pancardImageCapturedFile = compressedFile;

            const reader = new FileReader();
            reader.onload = function (event) {
                const pancardImageDataURL = event.target.result;
                displayPancardCapturedImage(pancardImageDataURL);
            };
            reader.readAsDataURL(compressedFile);
        });
    }

    const savedSignature = sessionStorage.getItem('signatureData');
    if (savedSignature) {
        signaturePad.fromDataURL(savedSignature);
    }
});

function displayPancardCapturedImage(imageDataURL) {
    const pancardImage = new Image();
    pancardImage.src = imageDataURL;
    pancardImage.style.maxWidth = '100%';
    pancardImage.style.maxHeight = '100%';
    pancardImage.className = 'camera-preview';
    pancardImage.style.objectFit = 'cover';

    if (pancardImageCaptureElement) {
        pancardImageCaptureElement.parentNode.removeChild(pancardImageCaptureElement);
    }

    const displayPancardCapturedImage = document.querySelector('#pancard-capture-image');
    displayPancardCapturedImage.innerHTML = '';
    displayPancardCapturedImage.appendChild(pancardImage);
    pancardImageCaptureElement = pancardImage;
}

document.getElementById('pancard-retake-btn').addEventListener('click', function () {
    document.getElementById('pancard-capture-btn').click();
});

function clearPancardImage() {
    if (pancardImageCaptureElement) {
        pancardImageCaptureElement.parentNode.removeChild(pancardImageCaptureElement);
        pancardImageCaptureElement = null;
    }
}
/*** End Pancard Image Capture Functionality ***/


/*** Start pancard document file upload, if not capture ***/
//function triggerPancardUpload() {
//    document.getElementById('pancard-doc-upload').click();
//}

//let pancardUploadedDocFile;
//document.getElementById('pancard-doc-upload').addEventListener('change', function (event) {
//    const panFile = event.target.files[0];

//    if (panFile) {
//        const allowedTypes = ['application/pdf', 'image/jpeg', 'image/png'];
//        if (!allowedTypes.includes(panFile.type)) {
//            toastr.options.positionClass = 'toast-bottom-right';
//            toastr.options.toastClass = "global-custom-toast",
//                toastr.warning('Please upload a valid file (PDF, JPG, JPEG, PNG).');
//            return;
//        }

//        pancardUploadedDocFile = panFile;

//        const pancardPpreviewContainer = document.getElementById('pancard-doc-preview');
//        pancardPpreviewContainer.innerHTML = '';

//        const panFileNameElement = document.createElement('p');
//        panFileNameElement.textContent = `${panFile.name}`;
//        panFileNameElement.className = 'mt-0 text-secondary';

//        pancardPpreviewContainer.appendChild(panFileNameElement);
//    }
//});
/*** End Pancard document file upload, if not capture ***/


//Sending request back to server after SUBMIT btn clicked
document.getElementById('btnSubmit').addEventListener('click', function () {
    var userName = $('#txtUserEnteredNamed').val();
    if (userName == null || userName == "") {
        toastr.options.positionClass = 'toast-bottom-right';
        toastr.options.toastClass = "global-custom-toast",
            toastr.warning('Please enter your name..');
    } else if (signaturePad.isEmpty()) {
        toastr.options.positionClass = 'toast-bottom-right';
        toastr.options.toastClass = "global-custom-toast",
            toastr.warning('Please provide a signature..');
    } else if (!capturedFile) {
        toastr.options.positionClass = 'toast-bottom-right';
        toastr.options.toastClass = "global-custom-toast",
            toastr.warning('Please capture an image..');
    } else if (!aadhaarBackImageCapturedFile || !aadhaarFrontImageCapturedFile) {
        toastr.options.positionClass = 'toast-bottom-right';
        toastr.options.toastClass = "global-custom-toast",
            toastr.warning('Please capture Aadhaar ID both front and back image..');
    } else if (!pancardImageCapturedFile) {
        toastr.options.positionClass = 'toast-bottom-right';
        toastr.options.toastClass = "global-custom-toast",
            toastr.warning('Please capture Pancard image..');
    } else {
        $('#btnSubmit').prop('disabled', true);
        showUploading();

        var signatureData = getSignatureFileWithWatermarks();

        var formData = new FormData();
        var sbdata = {
            CustomerName: $('#txtCustomerName').val(),
            MobileNo: $('#txtMobileNo').val(),
            OtherContactNo: $('#txtOtherContactNo').val(),
            Email: $('#txtEmail').val(),
            PanCardNo: $('#txtPanNo').val(),
            DOB: $('#txtDob').val(),
            FatherOrHusbandName: $('#txtFatherHusbandName').val(),
            Address: $('#txtAddress').val(),
            PinCode: $('#txtPinCode').val(),
            Village: $('#txtVillage').val(),
            City: $('#txtCity').val(),
            Taluk: $('#txtTehsil').val(),
            District: $('#txtDistrict').val(),
            State: $('#txtState').val(),
            DmsObfNo: $('#txtDmsObfNo').val(),
            BookingDate: $('#txtBookingDate').val(),
            Model: $('#txtBookedModel').val(),
            Variant: $('#txtBookedVariant').val(),
            Color: $('#txtColor').val(),
            TentativeWaitPeriod: $('#txtTentativeWaitingPeriod').val(),
            ExShowroomPrice: $('#txtExShowroomPrice').val(),
            RegistrationCharges: $('#txtRegistrationCharges').val(),
            TempRegCharges: $('#txtTempRegCharges').val(),
            InsurancePrice: $('#txtInsurancePrice').val(),
            EwOptional: $('#txtEw').val(),
            Accessories: $('#txtAccessories').val(),
            OthersIfAny: $('#txtOtherIfAny').val(),
            OthersDescribe: $('#txtOthersDescribe').val(),
            Discount: $('#txtDiscount').val(),
            CGST14Percent: $('#txtCgst').val(),
            SGST14Percent: $('#txtSgst').val(),
            CESS1Percent: $('#txtCess').val(),
            OnRoadPrice: $('#txtOnRoadPrice').val(),
            Amount: $('#txtBookingAmount').val(),
            PaymentMode: $('#txtPaymentMode').val(),
            PaymentReference: $('#txtPaymentReference').val(),
            Exchangestatus: $('#txtExchange').val(),
            ExistingCarModel: $('#txtExistingCarModel').val(),
            RegistrationNo: $('#txtRegistrationNo').val(),
            MSILListedCorporate: $('#txtMsilListedCorporate').val(),
            CorporateName: $('#txtCorporateName').val(),
            Finance: $('#txtFinanceAvailed').val(),
            UserEnteredName: $('#txtUserEnteredNamed').val()
        }

        formData.append('custSignature', signatureData);
        formData.append('custImageFile', capturedFile);
        formData.append('confirmBookData', JSON.stringify(sbdata));
        formData.append('aadharFrontImg', aadhaarFrontImageCapturedFile);
        formData.append('aadharBackImg', aadhaarBackImageCapturedFile);
        //formData.append('aadharUploadedDoc', aadhaarUploadedDocFile);
        formData.append('pancardImg', pancardImageCapturedFile);
        //formData.append('pancardUploadedDoc', pancardUploadedDocFile);

        $.ajax({
            type: "POST",
            url: submitOBFUrl,
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                hideUploading();
                if (response.success == true) {
                    window.location.href = confirmOBFSubmissionUrl;
                    toastr.options.positionClass = 'toast-bottom-right';
                    toastr.options.toastClass = "global-custom-toast",
                    toastr.options.timeOut = 10000;
                    toastr.success('Order Booking Form Confirmation Submitted Successfully.');
                }
                else {
                    $('#btnSubmit').prop('disabled', false);
                    toastr.options.positionClass = 'toast-bottom-right';
                    toastr.options.toastClass = "global-custom-toast",
                    toastr.options.timeOut = 10000;
                    toastr.error(response.message);
                }
            },
            error: function (error) {
                $('#btnSubmit').prop('disabled', false);
                hideUploading();
                toastr.options.positionClass = 'toast-bottom-right';
                toastr.options.toastClass = "global-custom-toast",
                toastr.options.timeOut = 10000;
                toastr.error('An error occurred while saving. Please try again.');
            }
        });
    }
});

function getSignatureFileWithWatermarks() {
    hiddenCanvas.width = canvas.width;
    hiddenCanvas.height = canvas.height + 65;
    hiddenCtx.clearRect(0, 0, hiddenCanvas.width, hiddenCanvas.height);
    hiddenCtx.drawImage(canvas, 0, 0);

    const currentDate = new Date();
    const day = String(currentDate.getDate()).padStart(2, '0');
    const month = String(currentDate.getMonth() + 1).padStart(2, '0');
    const year = currentDate.getFullYear();
    let hours = currentDate.getHours();
    const minutes = String(currentDate.getMinutes()).padStart(2, '0');
    const seconds = String(currentDate.getSeconds()).padStart(2, '0');
    const ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12;

    const strTime = `${hours}:${minutes}:${seconds} ${ampm}`;
    const dateString = `${day}/${month}/${year}, ${strTime}`;
    var lati;
    var longi;
    if (curLat !== null && curLat !== "" && curLat !== undefined && curLong !== null && curLong !== "" && curLong !== undefined) {
        lati = `Latitude: ${curLat}`;
        longi = `Longitude: ${curLong}`;
    } else {
        lati = "Latitude: 0.0";
        longi = "Longitude: 0.0";
    }

    const textHeight = 20;
    const watermarkX = 10;
    const watermarkY = canvas.height + 20;

    hiddenCtx.font = '13px Arial';
    hiddenCtx.fillStyle = 'white';
    hiddenCtx.strokeStyle = 'black';
    hiddenCtx.lineWidth = 2;

    hiddenCtx.strokeText(dateString, watermarkX, watermarkY);
    hiddenCtx.fillText(dateString, watermarkX, watermarkY);
    hiddenCtx.strokeText(lati, watermarkX, watermarkY + textHeight);
    hiddenCtx.fillText(lati, watermarkX, watermarkY + textHeight);
    hiddenCtx.strokeText(longi, watermarkX, watermarkY + textHeight * 2);
    hiddenCtx.fillText(longi, watermarkX, watermarkY + textHeight * 2);

    var signedFile = hiddenCanvas.toDataURL('image/png');

    return signedFile;
}

function compressImage(file, maxWidth, quality, callback) {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function (event) {
        const img = new Image();
        img.src = event.target.result;
        img.onload = function () {
            const canvas = document.createElement('canvas');
            const ctx = canvas.getContext('2d');

            let width = img.width;
            let height = img.height;

            // Maintain aspect ratio while resizing
            if (width > maxWidth) {
                height *= maxWidth / width;
                width = maxWidth;
            }

            canvas.width = width;
            canvas.height = height;

            ctx.drawImage(img, 0, 0, width, height);

            canvas.toBlob(
                function (blob) {
                    const compressedFile = new File([blob], file.name, {
                        type: file.type,
                        lastModified: Date.now(),
                    });
                    callback(compressedFile);
                },
                file.type,
                quality // Adjust image quality (0.1 - 1.0)
            );
        };
    };
}

var uploadingToast;
function showUploading() {
    toastr.options.positionClass = 'toast-bottom-right';
    toastr.options.toastClass = "global-custom-toast",
        toastr.options.timeOut = 0;
    toastr.info('Please wait your Confirm Order Booking Form is Submitting..!');
}
function hideUploading() {
    toastr.clear(uploadingToast);
    uploadingToast = toastr.options.timeOut = 5000;
}