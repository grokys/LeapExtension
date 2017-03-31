using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace LeapExtension
{
    class LeapCaret
    {
        IWpfTextView textView;
        IAdornmentLayer adornmentLayer;
        LeapCaretControl caretControl;

        public LeapCaret(IWpfTextView textView)
        {
            this.textView = textView;
            adornmentLayer = textView.GetAdornmentLayer("LeapExtension");
            textView.LayoutChanged += LayoutChanged;
        }

        private void LayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            if (caretControl == null)
            {
                caretControl = new LeapCaretControl();
            }
            else
            {
                adornmentLayer.RemoveAdornment(caretControl);
            }

            var span = new SnapshotSpan(textView.TextSnapshot, new Span(17, 1));
            var marker = textView.TextViewLines.GetMarkerGeometry(span);

            if (marker != null)
            {
                Canvas.SetLeft(caretControl, marker.Bounds.Left);
                Canvas.SetTop(caretControl, marker.Bounds.Top);
                caretControl.Height = marker.Bounds.Height;

                adornmentLayer.AddAdornment(
                    AdornmentPositioningBehavior.TextRelative,
                    span,
                    "Cursor1",
                    caretControl,
                    null);
            }
        }
    }
}
