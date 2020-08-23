using CometD.NetCore.Bayeux;
using CometD.NetCore.Bayeux.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesforceStreamConnector
{
    class MessageProcessor : IMessageListener
    {
        public void OnMessage(IClientSessionChannel channel, IMessage message)
        {
            var convertedJson = message.Json;
            var obj = JsonConvert.DeserializeObject<Rootobject>(convertedJson);
            Console.WriteLine(obj.data.sobject.Id + " " + obj.data.sobject.Name);
            Console.WriteLine(convertedJson);
            Console.WriteLine("------------------------------------------------");
        }
    }

}
