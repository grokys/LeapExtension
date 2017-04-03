using System;
using System.Windows.Controls;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace LeapExtension
{
    class LeapCaret
    {
        readonly IWpfTextView textView;
        readonly IAdornmentLayer adornmentLayer;
        LeapCaretControl caretControl;
        int position;

        public LeapCaret(IWpfTextView textView)
        {
            this.textView = textView;
            adornmentLayer = textView.GetAdornmentLayer("LeapExtension");
            textView.LayoutChanged += (s, e) => Update();
        }

        public int Position
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    position = value;
                    Update();
                }
            }
        }

        void Update()
        {
            if (caretControl == null)
            {
                caretControl = new LeapCaretControl();
            }
            else
            {
                adornmentLayer.RemoveAdornment(caretControl);
            }

            var pos = Math.Min(position, textView.TextSnapshot.Length - 1);
            var span = new SnapshotSpan(textView.TextSnapshot, new Span(pos, 1));
            var marker = textView.TextViewLines.GetLineMarkerGeometry(span);

            if (marker != null)
            {
                var point = pos == position ? marker.Bounds.TopLeft : marker.Bounds.TopRight;
                Canvas.SetLeft(caretControl, point.X);
                Canvas.SetTop(caretControl, point.Y);
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
