using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Controls;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace LeapExtension
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class TextViewCreationListener : IWpfTextViewCreationListener
    {
        private static readonly Uri LeapsUri = new Uri("ws://localhost:8080/leaps/ws");

        [Export(typeof(AdornmentLayerDefinition))]
        [Name("LeapExtension")]
        [Order(After = PredefinedAdornmentLayers.Caret)]
        [TextViewRole(PredefinedTextViewRoles.Document)]
        public AdornmentLayerDefinition editorAdornmentLayer = null;

        public void TextViewCreated(IWpfTextView textView)
        {
            ITextDocument document;

            if (textView.TextBuffer.Properties.TryGetProperty(typeof(ITextDocument), out document))
            {
                var dir = Path.GetFileName(Path.GetDirectoryName(document.FilePath));
                var file = Path.GetFileName(document.FilePath);
                var documentId = Path.Combine(dir, file);
                var client = new LeapsClient(LeapsUri);
                textView.Properties.GetOrCreateSingletonProperty(() => new LeapsDocument(client, documentId, textView));
            }
        }
    }
}
