using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HostedPayments.CSharp.Infrastructure
{
    public class PaymentResponse
    {
        public string HostedPaymentStatus;
        public string TransactionSetupID;
        public string TransactionID;
        public string ExpressResponseCode;
        public string ExpressResponseMessage;
        public string AVSResponseCode;
        public string CVVResponseCode;
        public string ApprovalNumber;
        public string LastFour;
        public string ValidationCode;
        public string CardLogo;
        public string ApprovedAmount;
        public string ServicesID;
        public string PaymentAccountID;
        public string CommercialCardResponseCode;
        public string TipAmount;
    }
}