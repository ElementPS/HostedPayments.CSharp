using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace HostedCheckout.MVC.CSharp.Infrastructure
{
    public class ConfigurationData
    {
        public string AccountId = string.Empty;
        public string AccountToken = string.Empty;
        public string AcceptorId = string.Empty;
        public string ApplicationId = string.Empty;
        public string ApplicationVersion = string.Empty;
        public string ApplicationName = string.Empty;
        public string ExpressSOAPEndpoint = string.Empty;
        public string ExpressXMLEndpoint = string.Empty;
        public string ReturnURL = string.Empty;

        public ConfigurationData()
        {
            AccountId = ConfigurationManager.AppSettings["AccountID"];
            AccountToken = ConfigurationManager.AppSettings["AccountToken"];
            AcceptorId = ConfigurationManager.AppSettings["AcceptorID"];
            ApplicationId = ConfigurationManager.AppSettings["ApplicationID"];
            ApplicationVersion = ConfigurationManager.AppSettings["ApplicationVersion"];
            ApplicationName = ConfigurationManager.AppSettings["ApplicationName"];
            ExpressSOAPEndpoint = ConfigurationManager.AppSettings["ExpressSOAPEndpoint"];
            ExpressXMLEndpoint = ConfigurationManager.AppSettings["ExpressXMLEndpoint"];
            ReturnURL = ConfigurationManager.AppSettings["ReturnURL"];
        }
    }
}