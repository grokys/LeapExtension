using System;
using System.Reactive;
using System.Reactive.Linq;
using LeapExtension.Models;
using Microsoft.VisualStudio.Text.Editor;
using ReactiveUI;

namespace LeapExtension
{
    class LeapsDocument
    {
        readonly LeapsClient client;
        readonly string documentId;
        readonly IWpfTextView textView;

        public LeapsDocument(LeapsClient client, string documentId, IWpfTextView textView)
        {
            this.client = client;
            this.documentId = documentId;
            this.textView = textView;

            client.Connected.Subscribe(ClientConnected);
            client.DocumentReceived.ObserveOn(RxApp.MainThreadScheduler).Subscribe(ClientDocumentReceived);
            client.Connect();
        }

        void ClientConnected(Unit unit)
        {
            client.Edit("anon", documentId);
        }

        void ClientDocumentReceived(LeapDocumentModel document)
        {
            using (var edit = textView.TextBuffer.CreateEdit())
            {
                edit.Replace(0, textView.TextBuffer.CurrentSnapshot.Length, document.Content);
                edit.Apply();
            }
        }
    }
}
