/* General Javascript file */



function TakePaymentRedirect()
{
    console.log("In Redirecting Payment");
    window.open("/WebForms/Default.aspx");
    var dollarAmount1 = document.getElementById("dollaramount").value;
    console.log(dollarAmount1);
    return dollarAmount1;

}

function PaymentAmount()
{
    var dollarAmount = document.getElementById("dollar-amount").value;
    console.log(dollarAmount);
    return dollarAmount;
}


