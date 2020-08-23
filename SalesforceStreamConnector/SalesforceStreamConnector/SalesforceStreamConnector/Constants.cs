using System;
using System.Collections.Generic;
using System.Text;

namespace SalesforceStreamConnector
{
    class Constants
    {
        // Connection Related Constants / Parameters 
        // All these will have to eventually move to Secret Vaults
        public static string USERNAME = "";
        public static string PASSWORD = "";
        public static string TOKEN = "";
        public static string CONSUMER_KEY = "";
        public static string CONSUMER_SECRET = "";
        public static string TOKEN_REQUEST_ENDPOINTURL = "https://login.salesforce.com/services/oauth2/token";

        // Salesforce Setting Constants
        public static string ENDPOINT_URI = "/cometd/48.0";
        public static int READ_TIMEOUT = 110000;
        //public static string PUSH_TOPIC_CHANNEL = "/topic/PushTestAcc";
        public static string PUSH_TOPIC_CHANNEL = "/topic/ContactDetails";

    }
}
