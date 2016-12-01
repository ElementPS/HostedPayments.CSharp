# HostedPayments.CSharp

Integration to Element Hosted Payments using CSharp

* Questions?  certification@elementps.com
* **Feature request?** Open an issue.
* Feel like **contributing**?  Submit a pull request.

##Overview

This repository demonstrates an integration to the Element Hosted Payments application using c#.  The code was compiled and tested using Microsoft Visual Studio Express 2013 for Web.  The application will open a web page allowing the user to press a button.  The button event makes a TransactionSetup request to the Express platform and then redirects to the Hosted Payments page.  After the user enters credit card details the Hosted Payments page will process the transaction and then redirect to a URL provided in the TransactionSetup that captures the results of the transaction and displays them in a web page.

![HostedPayments.CSharp](https://github.com/ElementPS/HostedPayments.CSharp/blob/master/screenshot1.PNG)

If you are using custom css you can make your screen look like the sample utilizing the CustomCss element as seen below:

![HostedPayments.CSharp](https://github.com/ElementPS/HostedPayments.CSharp/blob/master/screenshot_css.png)

If you are not using the custom css, you will get the default sytling as seen below:

![HostedPayments.CSharp](https://github.com/ElementPS/HostedPayments.CSharp/blob/master/screenshot2.PNG)

![HostedPayments.CSharp](https://github.com/ElementPS/HostedPayments.CSharp/blob/master/screenshot3.PNG)

##Prerequisites

Please contact your Integration Analyst for any questions about the prerequisite below.

* Create Express test account: http://www.elementps.com/Resources/Create-a-Test-Account

##Documentation/Troubleshooting

* When you create your Express test account an email will be sent containing links to documentation.

##Step 1: Generate a request

You can either generate an XML request or a SOAP request.  The Credentials and Application elements are empty below because these elements are read from the Web.config file.  When you receive an email after creating your test account the email will contain the information necessary to populate these fields in the Web.config.  If you want to customize the css, this is done inside TransactionSetup using the CustomCss element. The TransactionSetup request is displayed below.

This is the XML request:

```
<TransactionSetup xmlns="https://transaction.elementexpress.com">
  <Credentials>
    <AccountID></AccountID>
    <AccountToken></AccountToken>
    <AcceptorID></AcceptorID>
  </Credentials>
  <Application>
    <ApplicationID></ApplicationID>
    <ApplicationVersion>1.0</ApplicationVersion>
    <ApplicationName>HostedPayments.CSharp</ApplicationName>
  </Application>
  <Terminal>
    <TerminalID>01</TerminalID>
    <CardholderPresentCode>2</CardholderPresentCode>
    <CardInputCode>5</CardInputCode>
    <TerminalCapabilityCode>3</TerminalCapabilityCode>
    <TerminalEnvironmentCode>2</TerminalEnvironmentCode>
    <CardPresentCode>2</CardPresentCode>
    <MotoECICode>1</MotoECICode>
    <CVVPresenceCode>1</CVVPresenceCode>
  </Terminal>
  <Transaction>
    <TransactionAmount>6.55</TransactionAmount>
  </Transaction>
  <TransactionSetup>
    <TransactionSetupMethod>1</TransactionSetupMethod>
    <Embedded>1</Embedded>
    <AutoReturn>1</AutoReturn>
    <ReturnURL>http://localhost:51619/Home/Complete</ReturnURL>
    <CustomCss>body{margin-left:50px;font-family:arial;font-size:large;border:none;}</CustomCss>
  </TransactionSetup>
</TransactionSetup>

```

##Step 2: Send Request to the Express API

The HttpSender class is used to send the XML to the Express platform.

```
HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

webRequest.ContentType = "text/xml;charset=\"utf-8\"";
webRequest.Accept = "text/xml";
webRequest.Method = "POST";
webRequest.ContentLength = data.Length;

using (var stream = webRequest.GetRequestStream())
{
  stream.Write(byteData, 0, byteData.Length);
}
```

##Step 3: Receive response from Express API

The response will be in an XML format shown below.

```
<TransactionSetupResponse xmlns='https://transaction.elementexpress.com'>
   <Response>
      <ExpressResponseCode>0</ExpressResponseCode>
      <ExpressResponseMessage>Success</ExpressResponseMessage>
      <ExpressTransactionDate>20160709</ExpressTransactionDate>
      <ExpressTransactionTime>152048</ExpressTransactionTime>
      <ExpressTransactionTimezone>UTC-05:00:00</ExpressTransactionTimezone>
      <Transaction>
         <TransactionSetupID>AFD9802E-D023-402A-A7A1-9F6E64F2D5A5</TransactionSetupID>
      </Transaction>
      <PaymentAccount>
         <TransactionSetupID>AFD9802E-D023-402A-A7A1-9F6E64F2D5A5</TransactionSetupID>
      </PaymentAccount>
      <TransactionSetup>
         <TransactionSetupID>AFD9802E-D023-402A-A7A1-9F6E64F2D5A5</TransactionSetupID>
         <ValidationCode>63A21AA243EF4346</ValidationCode>
      </TransactionSetup>
    </Response>
  </TransactionSetupResponse>
```

##Step 4: Parse response data and redirect to Hosted Payment page

You may parse the XML response in any manner.  One method is shown below to extract the TransactionSetupID from the XML response.  The critical value is the TransactionSetupID and you may parse it from any of the TransactionSetupID elements.  The code below shows parsing TransactionSetupID from the Transaction element.

```
var document = XDocument.Parse(response);
XNamespace ns = "https://transaction.elementexpress.com";
XElement txnSetupId = document.Root.Element(ns + "Response").Element(ns + "Transaction").Element(ns + "TransactionSetupID");
var transSetupId = txnSetupId.Value;
```

##Step 5: Display result of transaction processing

The user will now enter credit card data into the Hosted Payments page, press submit, and Hosted Payments page will process the transaction.  Finally Hosted Payments page will post back to the URL provided in ReturnURL of TransactionSetup.  This is the method that is called when Hosted Payments POSTs back.  A model object is created (paymentResponse) to contain all of the values returned from Hosted Payments and then this object is passed along to the View for display.

```
public ActionResult Complete(string HostedPaymentStatus, string TransactionSetupID, string TransactionID,
                            string ExpressResponseCode, string ExpressResponseMessage, string AVSResponseCode,
                            string CVVResponseCode, string ApprovalNumber, string LastFour, string ValidationCode,
                            string CardLogo, string ApprovedAmount, string ServicesID, string PaymentAccountID,
                            string CommercialCardResponseCode, string TipAmount)
{

    var paymentResponse = new PaymentResponse();

    paymentResponse.HostedPaymentStatus = HostedPaymentStatus;
    paymentResponse.TransactionSetupID = TransactionSetupID;
    paymentResponse.TransactionID = TransactionID;
    paymentResponse.ExpressResponseCode = ExpressResponseCode;
    paymentResponse.ExpressResponseMessage = ExpressResponseMessage;
    paymentResponse.AVSResponseCode = AVSResponseCode;
    paymentResponse.CVVResponseCode = CVVResponseCode;
    paymentResponse.ApprovalNumber = ApprovalNumber;
    paymentResponse.LastFour = LastFour;
    paymentResponse.ValidationCode = ValidationCode;
    paymentResponse.CardLogo = CardLogo;
    paymentResponse.ApprovedAmount = ApprovedAmount;
    paymentResponse.ServicesID = ServicesID;
    paymentResponse.PaymentAccountID = PaymentAccountID;
    paymentResponse.CommercialCardResponseCode = CommercialCardResponseCode;
    paymentResponse.TipAmount = TipAmount;

    return View(paymentResponse);
}
```

###©2014-2016 Element Payment Services, Inc., a Vantiv company. All Rights Reserved.

Disclaimer:
This software and all specifications and documentation contained herein or provided to you hereunder (the "Software") are provided free of charge strictly on an "AS IS" basis. No representations or warranties are expressed or implied, including, but not limited to, warranties of suitability, quality, merchantability, or fitness for a particular purpose (irrespective of any course of dealing, custom or usage of trade), and all such warranties are expressly and specifically disclaimed. Element Payment Services, Inc., a Vantiv company, shall have no liability or responsibility to you nor any other person or entity with respect to any liability, loss, or damage, including lost profits whether foreseeable or not, or other obligation for any cause whatsoever, caused or alleged to be caused directly or indirectly by the Software. Use of the Software signifies agreement with this disclaimer notice.

