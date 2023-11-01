using System;
using System.Collections.Generic;
using Android.Runtime;
using Android.Content;
using Java.Interop;

[assembly: MetaData (
	name: "pspdfkit_automatic_initialize",
	Value = "false"
)]

namespace PSPDFKit {

	public sealed partial class PSPDFKitGlobal {

		internal const string ProductIdentifier = "DotNetBindingsAndroid";

		public static void Initialize (Context context, string? licenseKey, IList<string> fontPaths)
		{
			if (context is null)
				throw new NullReferenceException (nameof (context));

			if (fontPaths is null)
				throw new NullReferenceException (nameof (fontPaths));

			InitializeInternal (context, null);
			Initialize (context, licenseKey, fontPaths, ProductIdentifier);
		}

		public static void Initialize (Context context, string? licenseKey)
		{
			if (context is null)
				throw new NullReferenceException ($"context");

			InitializeInternal (context, null);
			Initialize (context, licenseKey, Array.Empty<string> (), ProductIdentifier);
		}
	}
}