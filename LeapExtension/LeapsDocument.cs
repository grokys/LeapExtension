using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using LeapExtension.Models;
using Microsoft.VisualStudio.Text;
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
            client.TransformsReceived.ObserveOn(RxApp.MainThreadScheduler).Subscribe(ClientTransformsReceived);
            client.UpdatesReceived.ObserveOn(RxApp.MainThreadScheduler).Subscribe(ClientUpdatesReceived);
            client.Connect();
        }

        void ApplyTransform(TransformModel transform)
        {
            using (var edit = textView.TextBuffer.CreateEdit())
            {
                edit.Replace(transform.Position, Math.Max(0, transform.NumDelete), transform.Insert ?? string.Empty);
                edit.Apply();
            }
        }

        void ClientConnected(Unit unit)
        {
            client.JoinDocument("vs", documentId);
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

            client.UpdateCursor(textView.Caret.Position.BufferPosition.Position);
            textView.TextBuffer.Changed += TextBufferChanged;
            textView.Caret.PositionChanged += CaretPositionChanged;
        }

        void ClientTransformsReceived(TransformModel[] transforms)
        {
            foreach (var transform in transforms)
            {
                ApplyTransform(transform);
            }
        }

        void ClientUpdatesReceived(UserUpdateModel[] updates)
        {
            foreach (var update in updates)
            {
                if (update.Message.Position.HasValue)
                {
                    UpdateRemoteCaret(update.Client.UserId, update.Message.Position.Value);
                }
            }
        }

        void UpdateRemoteCaret(string userId, int position)
        {
            LeapCaret caret;

            if (!carets.TryGetValue(userId, out caret))
            {
                caret = new LeapCaret(textView);
                caret.User = userId;
                carets.Add(userId, caret);
            }

            caret.Position = position;
        }

        void CaretPositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            client.UpdateCursor(e.NewPosition.BufferPosition.Position);
        }

        void TextBufferChanged(object sender, TextContentChangedEventArgs e)
        {
            var caretPosition = textView.Caret.Position.BufferPosition.Position;

            foreach (var change in e.Changes)
            {
                client.SendTransform(change.OldPosition, change.OldText.Length, change.NewText);
                caretPosition = change.OldPosition + change.NewLength;
            }

            client.UpdateCursor(caretPosition);
        }
    }
}
