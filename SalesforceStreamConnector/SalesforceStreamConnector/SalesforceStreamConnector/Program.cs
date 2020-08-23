using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using CometD.NetCore.Client;
using CometD.NetCore.Client.Transport;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace SalesforceStreamConnector
{
    class Program
    {
        static void Main(string[] args)
        {
            ILoggerFactory loggerFactory = new LoggerFactory(
                           new[] { new ConsoleLoggerProvider((_, __) => true, true) }
                       );
            //or
            //ILoggerFactory loggerFactory = new LoggerFactory().AddConsole();

            ILogger logger = loggerFactory.CreateLogger<Program>();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Task<AuthResponse> authResponse = Task.Run(() => Authenticator.AsyncAuthRequest());
            authResponse.Wait();
            if (authResponse.Result != null)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
 
                try
                {
                    int readTimeOut = Constants.READ_TIMEOUT;
                    string streamingEndpointURI = Constants.ENDPOINT_URI;
                    var options = new Dictionary<String, Object>
                    {
                        { ClientTransport.TIMEOUT_OPTION, readTimeOut }
                    };

                    NameValueCollection collection = new NameValueCollection();
                    collection.Add(HttpRequestHeader.Authorization.ToString(), "OAuth " + authResponse.Result.access_token);

                    // The LongPollingTransport class also has a constructor that takes iLogger as a parameter
                    // The below call will eventually need to be replaced with that one to ensure Logging takes place
                    // from within the LongPollingTransport
                    var transport = new LongPollingTransport(options, new NameValueCollection { collection }, logger);
                    
                    var serverUri = new Uri(authResponse.Result.instance_url);
                    String endpoint = String.Format("{0}://{1}{2}", serverUri.Scheme, serverUri.Host, streamingEndpointURI);

                    // The BayeuxClient class also has a constructor that takes iLogger as a parameter
                    // The below call will eventually need to be replaced with that one to ensure Logging takes place
                    // from within the BayeuxClient
                    var bayeuxClient = new BayeuxClient(endpoint, new[] { transport });

                    var pushTopicConnection = new SFConnector(bayeuxClient);
                    pushTopicConnection.Connect();
                    //Close the connection
                    Console.WriteLine("Press any key to shut down.\n");
                    Console.ReadKey();
                    pushTopicConnection.Disconect();
                    Console.ReadKey();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

        }
    }
}
