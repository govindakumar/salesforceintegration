using CometD.NetCore.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesforceStreamConnector
{
    class SFConnector
    {
        BayeuxClient _bayeuxClient = null;
        string channel = Constants.PUSH_TOPIC_CHANNEL;

        public SFConnector(BayeuxClient bayeuxClient)
        {
            _bayeuxClient = bayeuxClient;
        }

        public void Connect()
        {
            Console.WriteLine("--- Handshake ---");
            _bayeuxClient.Handshake();
            _bayeuxClient.WaitFor(1000, new[] { BayeuxClient.State.CONNECTED });

            Console.WriteLine("--- Setting up Optional Extenders ---");
            var replayEx = new CometD.NetCore.Client.Extension.ReplayExtension();
            var ErrEx = new CometD.NetCore.Client.Extension.ErrorExtension();
            _bayeuxClient.AddExtension(replayEx);
            _bayeuxClient.AddExtension(ErrEx);

            Console.WriteLine("--- Subscribing to Channel : " + channel + "---");
            _bayeuxClient.GetChannel(channel, -1).Subscribe(new MessageProcessor());

            Console.WriteLine("Waiting event from salesforce for the push topic " + channel.ToString());
        }

        public void Disconect()
        {
            Console.WriteLine("--- Disconnecting ---");
            _bayeuxClient.Disconnect();
            _bayeuxClient.WaitFor(1000, new[] { BayeuxClient.State.DISCONNECTED });
            Console.WriteLine("--- Disconnected ---");
        }
    }
}
