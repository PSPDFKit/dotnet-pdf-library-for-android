PSPDFKit SDK for Xamarin

The PSPDFKit SDK is the leading framework for displaying, annotating and editing PDFs on iOS, macOS, Android, Windows, Electron and the Web.

PSPDFKit Instant adds real-time collaboration features to seamlessly share, edit, and annotate PDF documents.


## Additional Required Setup (Please Read!)

You can try PSPDFKit in a few simple steps and get the library up and running in your app with little to no effort.

1. If you are a PSPDFKit customer, get the license key from your [customer portal](https://customers.pspdfkit.com/customers/sign_in).

2. Set your license key inside your MainActivity.cs in between the using statements and the namespace declaration using an assembly attribute.
Setting the license is not needed if you are evaluating PSPDFKit.

using System;
using PSPDFKit;

[assembly: MetaData (
	name: "pspdfkit_license_key",
	Value = "YOUR_LICENSE_KEY_GOES_HERE"
)]

namespace YourAwesomeApp { ... }

3. Now you can start using the SDK.


## Examples

You can find several example projects in https://github.com/PSPDFKit/Xamarin-Android, including a catalog and Xamarin.Forms.

Visit https://pspdfkit.com/guides/ios/current/other-languages/xamarin/ for more information.
