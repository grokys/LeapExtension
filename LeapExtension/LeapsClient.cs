using System;
using System.ComponentModel.Composition;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading;
using LeapExtension.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocket4Net;

namespace LeapExtension
{
    class LeapsClient : IDisposable
    {
        readonly Uri address;
        readonly WebSocket socket;
        readonly Subject<Unit> connected = new Subject<Unit>();
        readonly Subject<LeapDocumentModel> documentReceived = new Subject<LeapDocumentModel>();

        public LeapsClient(Uri address)
        {
            this.address = address;
            socket = new WebSocket4Net.WebSocket(address.ToString());
        }

        public IObservable<Unit> Connected => connected;
        public IObservable<LeapDocumentModel> DocumentReceived => documentReceived;

        public void Connect()
        {
            socket.Open();
            socket.Opened += Socket_Opened;
            socket.MessageReceived += Socket_MessageReceived;
        }

        public void Edit(string userId, string documentId)
        {
            var command = new EditCommandModel
            {
                UserId = "anon",
                Token = "",
                DocumentId = documentId,
            };

            socket.Send(JsonConvert.SerializeObject(command));
        }

        private void Socket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var responseType = (string)JObject.Parse(e.Message)["response_type"];

            switch (responseType)
            {
                case "document":
                    var document = JsonConvert.DeserializeObject<DocumentResponseModel>(e.Message);
                    documentReceived.OnNext(document.LeapDocument);
                    break;
            }
        }

        private void Socket_Opened(object sender, EventArgs e)
        {
            connected.OnNext(Unit.Default);
        }

        public void Dispose()
        {
            socket.Close();
            socket.Dispose();
        }
    }
}
