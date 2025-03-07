using System;
using Android.Runtime;
using PSPDFKit.UI;
using PSPDFKit.Instant;

namespace PSPDFKit.Instant {
	public partial class InstantPdfActivity : PdfActivity {

		public InstantPdfDocument GetInstantDocument () => Document.JavaCast<InstantPdfDocument> ();
		public InstantPdfFragment GetInstantPdfFragment () => PdfFragment.JavaCast<InstantPdfFragment> ();
	}
}
