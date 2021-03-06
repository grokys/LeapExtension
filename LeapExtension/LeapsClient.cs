﻿using System;
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
        readonly Subject<TransformModel[]> transformsReceived = new Subject<TransformModel[]>();
        readonly Subject<UserUpdateModel[]> updatesReceived = new Subject<UserUpdateModel[]>();
        int version;

        public LeapsClient(Uri address)
        {
            this.address = address;
            socket = new WebSocket4Net.WebSocket(address.ToString());
            socket.Opened += Socket_Opened;
            socket.MessageReceived += Socket_MessageReceived;
        }

        public IObservable<Unit> Connected => connected;
        public IObservable<LeapDocumentModel> DocumentReceived => documentReceived;
        public IObservable<TransformModel[]> TransformsReceived => transformsReceived;
        public IObservable<UserUpdateModel[]> UpdatesReceived => updatesReceived;

        public void Connect()
        {
            socket.Open();
        }

        public void JoinDocument(string userId, string documentId)
        {
            var command = new EditCommandModel
            {
                UserId = "vs",
                Token = "",
                DocumentId = documentId,
            };

            socket.Send(JsonConvert.SerializeObject(command));
        }

        public void SendTransform(int position, int numDelete, string insert)
        {
            var command = new SubmitCommandModel
            {
                Transform = new TransformModel
                {
                    Position = position,
                    NumDelete = numDelete,
                    Insert = insert,
                    Version = ++version,
                }
            };

            socket.Send(JsonConvert.SerializeObject(command));
        }

        public void UpdateCursor(int position)
        {
            var command = new UpdateCommandModel
            {
                Position = position,
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
                    version = document.Version;
                    documentReceived.OnNext(document.LeapDocument);
                    break;
                case "transforms":
                    var transforms = JsonConvert.DeserializeObject<TransformsResponseModel>(e.Message);
                    transformsReceived.OnNext(transforms.Transforms);
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
