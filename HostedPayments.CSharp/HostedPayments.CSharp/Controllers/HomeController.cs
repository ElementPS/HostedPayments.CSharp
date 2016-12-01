using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using HostedPayments.CSharp.Infrastructure;

namespace HostedPayments.CSharp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Buy()
        {
            var configurationData = new ConfigurationData();

            XNamespace express = "https://transaction.elementexpress.com";

            XDocument doc = new XDocument(new XElement(express + "TransactionSetup",
                                               new XElement(express + "Credentials",
                                                   new XElement(express + "AccountID", configurationData.AccountId),
                                                   new XElement(express + "AccountToken", configurationData.AccountToken),
                                                   new XElement(express + "AcceptorID", configurationData.AcceptorId)
                                                            ),
                                                new XElement(express + "Application",
                                                    new XElement(express + "ApplicationID", configurationData.ApplicationId),
                                                    new XElement(express + "ApplicationVersion", configurationData.ApplicationVersion),
                                                    new XElement(express + "ApplicationName", configurationData.ApplicationName)
                                                            ),
                                                new XElement(express + "Terminal",
                                                    new XElement(express + "TerminalID", "01"),
                                                    new XElement(express + "CardholderPresentCode", "2"),
                                                    new XElement(express + "CardInputCode", "5"),
                                                    new XElement(express + "TerminalCapabilityCode", "3"),
                                                    new XElement(express + "TerminalEnvironmentCode", "2"),
                                                    new XElement(express + "CardPresentCode", "2"),
                                                    new XElement(express + "MotoECICode", "1"),
                                                    new XElement(express + "CVVPresenceCode", "1")
                                                            ),
                                                new XElement(express + "Transaction",
                                                    new XElement(express + "TransactionAmount", "6.55")
                                                            ),
                                                new XElement(express + "TransactionSetup",
                                                    new XElement(express + "TransactionSetupMethod", "1"),
                                                    new XElement(express + "Embedded", "1"),
                                                    new XElement(express + "AutoReturn", "1"),
                                                    new XElement(express + "ReturnURL", configurationData.ReturnURL),
                                                    new XElement(express + "CustomCss", @"
                                                        body{margin-left: 50px; font-family: arial; font-size: large; height: 100%; clear: left; border: none;}
                                                        .tdHeader{border: none; background-color: transparent; font-size: large}
                                                        .content{border: none}
                                                        .inputText{font-size: large}
                                                        .selectOption{font-size: large}
                                                        .tdLabel{padding-right: 40px}
                                                        .buttonEmbedded{color: #fff; background-color: transparent; border: 1px solid transparent; border-radius: 2px;}
                                                        .buttonEmbedded:hover{color: #fff; background-color: #f6801a; border-color: transparent}
                                                        .buttonCancel{color: #fff; background-color: transparent; border: 1px solid transparent; border-radius: 2px;}
                                                        .buttonCancel:hover{color: #fff; background-color: #f6801a; border-color: transparent}")
                                                            )
                                                       )
                                         );

            try
            {
                var response = new Infrastructure.HttpSender().Send(doc.ToString(), configurationData.ExpressXMLEndpoint, string.Empty);

                var document = XDocument.Parse(response);
                XNamespace ns = "https://transaction.elementexpress.com";
                XElement txnSetupId = document.Root.Element(ns + "Response").Element(ns + "Transaction").Element(ns + "TransactionSetupID");
                var transSetupId = txnSetupId.Value;
                ViewBag.URL = "https://certtransaction.hostedpayments.com/?TransactionSetupID=" + transSetupId;
            }
            catch (Exception ex)
            { 

            }

            return View();
        }

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


    }
}