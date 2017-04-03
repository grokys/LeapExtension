using System;
using System.Reactive;
using System.Reactive.Subjects;
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
        readonly Subject<UserUpdateModel[]> updatesReceived = new Subject<UserUpdateModel[]>();

        public LeapsClient(Uri address)
        {
            this.address = address;
            socket = new WebSocket4Net.WebSocket(address.ToString());
        }

        public IObservable<Unit> Connected => connected;
        public IObservable<LeapDocumentModel> DocumentReceived => documentReceived;
        public IObservable<UserUpdateModel[]> UpdatesReceived => updatesReceived;

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
                UserId = "vs",
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
                case "update":
                    var update = JsonConvert.DeserializeObject<UpdateResponseModel>(e.Message);
                    updatesReceived.OnNext(update.UserUpdates);
                    break;
                default:
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
