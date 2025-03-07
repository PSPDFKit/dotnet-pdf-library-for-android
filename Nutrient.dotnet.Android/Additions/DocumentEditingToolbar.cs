using System;
using Android.Runtime;
namespace PSPDFKit.UI.Toolbar {
	public partial class DocumentEditingToolbar {
		public override void BindController (Java.Lang.Object obj)
		{
			BindController (obj.JavaCast<global::PSPDFKit.UI.SpecialMode.Controller.IDocumentEditingController> ());
		}
	}
}
