/**
 * Twilio Client configuration -- enables browser-to-phone calls and vice versa
 */

// Store some selectors for elements we'll reuse
var callStatus = $("#call-status");
var hangUpButton = $(".hangup-button");
var callConstituentButton = $(".call-constituent-button");

/* Helper function to update the call status bar */
function updateCallStatus(status) {
    callStatus.text(status);
}

/* Get a Twilio Client token with an AJAX request */
$(document).ready(function () {
    $.post("/Token/Generate", { page: window.location.pathname }, function (data) {
        // Set up the Twilio Client Device with the token
        Twilio.Device.setup(data.token);
    });
});

/* Callback to let us know Twilio Client is ready */
Twilio.Device.ready(function (device) {
    updateCallStatus("Ready");
});

/* Report any errors to the call status display */
Twilio.Device.error(function (error) {
    updateCallStatus("ERROR: " + error.message);
});

/* Callback for when Twilio Client initiates a new connection */
Twilio.Device.connect(function (connection) {
    // Enable the hang up button and disable the call button
    hangUpButton.prop("disabled", false);
    callConstituentButton.prop("disabled", true);

    // If phoneNumber is part of the connection, this is a call from a
    // Caller to a Constituent's phone
    if ("phoneNumber" in connection.message) {
        updateCallStatus("In call with " + connection.message.phoneNumber);
    }
});

///* Callback for when a call ends -- this was moved to the view to interact with the modal */
//Twilio.Device.disconnect(function (connection) {
//    // Disable the hangup button and enable the call button
//    hangUpButton.prop("disabled", true);
//    callConstituentButton.prop("disabled", false);

//    updateCallStatus("Ready");
//});

/* Call a Constituent from within the view */
function callConstituent(phoneNumber) {
    updateCallStatus("Calling " + phoneNumber + "...");

    var params = { "phoneNumber": phoneNumber };
    Twilio.Device.connect(params);
}

///* End a call  -- this was moved to the view to interact with the modal */
//function hangUp() {
//    Twilio.Device.disconnectAll();

//    $.get("/Voice/Delete?id=" + $('#id').attr('value'), function (data) {
//        console.log("Deleted call assignment.");
//    });
//}