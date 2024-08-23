$(document).ready(function () {
    var url = new URL(window.location.href);
    var tenant = "";
    var stylesheet = "";
    if (url.searchParams.has("tenantId")) {
        tenant = url.searchParams.get("tenantId");
        if (tenant === "67524f15-f870-440d-a2d3-075264371001") {
            tenant = "agilebridge";
            stylesheet = "https://ssotestsarfk.blob.core.windows.net/styles/platform.css";
        }
        else {
            tenant = "lightstone";
            stylesheet = "https://ssotestsarfk.blob.core.windows.net/styles/platform.css";
        }
    }
    else {
        tenant = "lightstone";
        stylesheet = "https://ssotestsarfk.blob.core.windows.net/styles/platform.css";
    }
    // var styleSheet = 'https://cdn.test.platform.coaxle.co.za/tenantmanagement-api-test/styles/tenant.css?tenantId=' + tenantId;
    document.getElementsByTagName("head")[0].insertAdjacentHTML(
        "beforeend",
        "<link rel='stylesheet' href='" + stylesheet + "' />");
});

// var iti;
// var lip;

// function setupUi() {
//     //hide country code
//     document.getElementById('mfaCountryCode').style.visibility = 'hidden';
//     //hide the change button
//     if (document.getElementById('phoneVerificationControl-readOnly_but_change_claims') !== null) {
//         document.getElementById('phoneVerificationControl-readOnly_but_change_claims').style.visibility = 'hidden';
//         continueLogin();
//     }
//     if (document.getElementById('phoneVerificationControl_but_change_claims') !== null) {
//         document.getElementById('phoneVerificationControl_but_change_claims').style.visibility = 'hidden';
//     }

//     if (document.getElementById('mfaPhoneNumber') == null) {
//         setTimeout(() => setupUi(), 2000);
//     }
//     else {

//         document.getElementById('mfaPhoneNumber').hidden = true;
//         let div = document.createElement('div');
//         let intelInputElement = document.createElement('input');
//         intelInputElement.id = 'phoneNumner';
//         intelInputElement.name = 'phoneNumner';
//         intelInputElement.type = 'tel';
//         intelInputElement.className = 'textInput';
//         div.append(intelInputElement);
//         document.querySelector('.TextBox.Phone > .attrEntry').append(div);

//         checkIfReady();
//     }
// }

function continueLogin() {
    //hide button and continue login
    // document.getElementById('continue').style.visibility = 'hidden';
    document.getElementById("continue").click();
}


function configure() {
    iti = intlTelInput(document.getElementById('phoneNumner'), {
        initialCountry: "za",
        preferredCountries: ['za']
    });

    document.getElementById('phoneNumner').addEventListener('countrychange', function () {
        setNumber();
        validateNumber();
    });

    document.getElementById('phoneNumner').addEventListener('change', function () {
        setNumber();
        validateNumber();
    });

    document.getElementById('phoneNumner').addEventListener('keyup', function () {
        setNumber();
        validateNumber();
    });

    setNumber();
}

function setNumber() {
    var number = iti.telInput.value;
    if (number.indexOf('0') === 0) {
        number = number.substring(1, number.length);
        document.getElementById('phoneNumner').value = number;
    }
    document.getElementById('mfaPhoneNumber').value = '+' + iti.getSelectedCountryData().dialCode + number;

    //Set the country code
    document.getElementById('mfaCountryCode').value = iti.getSelectedCountryData().iso2;

}

function validateNumber() {
    document.getElementById('phoneVerificationControl_but_send_code').style.visibility = 'hidden';
    let isValidForCountry = libphonenumber.isValidPhoneNumber(document.getElementById('phoneNumner').value, iti.getSelectedCountryData().iso2.toUpperCase())
    let numberLeng = Math.ceil(Math.log10(document.getElementById('phoneNumner').value));

    if (isValidForCountry) {
        if (iti.getSelectedCountryData().iso2.toUpperCase() == "ZA" && numberLeng < 9) {
            showInValidNumberStyle();
        } else {
            showValidNumberStyle();
        }
    } else {
        showInValidNumberStyle();
    }
}

function showInValidNumberStyle() {
    document.getElementsByClassName("intro")[0].textContent = 'Please provide a valid number ';
    document.getElementsByClassName("intro")[0].style.color = 'red'
    document.getElementById("phoneNumner").style.color = 'red'
    document.getElementById('phoneVerificationControl_but_verify_code').style.visibility = 'hidden';
    document.getElementById('phoneVerificationControl_but_send_new_code').style.visibility = 'hidden';
}
function showValidNumberStyle() {
    document.getElementById('phoneVerificationControl_but_send_code').style.visibility = 'visible';
    document.getElementsByClassName("intro")[0].style.color = '#002072'
    document.getElementById("phoneNumner").style.color = '#002072'
    document.getElementById('phoneVerificationControl_but_verify_code').style.visibility = 'visible';
    document.getElementById('phoneVerificationControl_but_send_new_code').style.visibility = 'visible';
}

function checkIfReady() {
    try {
        configure();
    } catch {
        setTimeout(() => {
            checkIfReady();
        }, 100);
    }
}

function isSignUpReadyForLogin() {

    //this is for the register of new user
    //$('#phoneVerificationControl_code_sent_message').attr("aria-hidden")
    if ($('#phoneVerificationControl_code_sent_message').attr("aria-hidden") == 'true' && document.getElementById('phoneVerificationControl_but_verify_code').style.display == 'none' && document.getElementById('phoneVerificationControl_but_send_code').style.display == 'none' && document.getElementById("continue") !== undefined) {
        continueLogin();
    } else {
        setTimeout(() => checkIfSignUpIsReady(), 500);
    }
}

function checkIfSignUpIsReady() {
    try {
        isSignUpReadyForLogin();
    } catch {
        setTimeout(() => {
            isSignUpReadyForLogin();
        }, 100);
    }
}

// setupUi();
checkIfSignUpIsReady();