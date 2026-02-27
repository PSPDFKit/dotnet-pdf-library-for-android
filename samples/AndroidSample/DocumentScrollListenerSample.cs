using Android.Util;

using PSPDFKit.Listeners.Scrolling;
using PSPDFKit.UI;

namespace AndroidSample;

internal static class DocumentScrollListenerSample
{
    public static IDocumentScrollListener AttachTo(PdfFragment pdfFragment)
    {
        var listener = new LoggingDocumentScrollListener();
        pdfFragment.AddDocumentScrollListener(listener);
        return listener;
    }

    private sealed class LoggingDocumentScrollListener : Java.Lang.Object, IDocumentScrollListener
    {
        public void OnDocumentScrolled(int currX, int currY, int maxX, int maxY, int extendX, int extendY)
        {
            Log.Debug(nameof(DocumentScrollListenerSample), $"Scroll position=({currX}, {currY}) max=({maxX}, {maxY})");
        }

        public void OnScrollStateChanged(PSPDFKit.Listeners.Scrolling.ScrollState state)
        {
            Log.Debug(nameof(DocumentScrollListenerSample), $"Scroll state changed to: {state}");
        }
    }
}
