using System;
using System.Collections.Generic;
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
        readonly Dictionary<string, LeapCaret> carets = new Dictionary<string, LeapCaret>();

        public LeapsDocument(LeapsClient client, string documentId, IWpfTextView textView)
        {
            this.client = client;
            this.documentId = documentId;
            this.textView = textView;

            client.Connected.Subscribe(ClientConnected);
            client.DocumentReceived.ObserveOn(RxApp.MainThreadScheduler).Subscribe(ClientDocumentReceived);
            client.UpdatesReceived.ObserveOn(RxApp.MainThreadScheduler).Subscribe(ClientUpdatesReceived);
            client.Connect();
        }

        void ClientConnected(Unit unit)
        {
            client.Edit("anon", documentId);
        }

        void ClientDocumentReceived(LeapDocumentModel document)
        {
            if (document.Id == documentId)
            {
                using (var edit = textView.TextBuffer.CreateEdit())
                {
                    edit.Replace(0, textView.TextBuffer.CurrentSnapshot.Length, document.Content);
                    edit.Apply();
                }
            }
        }

        void ClientUpdatesReceived(UserUpdateModel[] updates)
        {
            foreach (var update in updates)
            {
                if (update.Message.Position.HasValue)
                {
                    UpdateCaret(update.Client.UserId, update.Message.Position.Value);
                }
            }
        }

        private void UpdateCaret(string userId, int position)
        {
            LeapCaret caret;

            if (!carets.TryGetValue(userId, out caret))
            {
                caret = new LeapCaret(textView);
                carets.Add(userId, caret);
            }

            caret.Position = position;
        }
    }
}
