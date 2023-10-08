using System;
using System.Collections.Generic;
using Android.Runtime;
using Java.Interop;

namespace PSPDFKit.Annotations {
	public partial class RedactionAnnotation {

		// Metadata.xml XPath constructor reference: path="/api/package[@name='com.pspdfkit.annotations']/class[@name='RedactionAnnotation']/constructor[@name='RedactionAnnotation' and count(parameter)=0]"
		[Register (".ctor", "()V", "")]
		public unsafe RedactionAnnotation ()
			: base (IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
		{
			const string __id = "()V";

			if (((global::Java.Lang.Object) this).Handle != IntPtr.Zero)
				return;

			try {
				var __r = _members.InstanceMethods.StartCreateInstance (__id, ((object) this).GetType (), null);
				SetHandle (__r.Handle, JniHandleOwnership.TransferLocalRef);
				_members.InstanceMethods.FinishCreateInstance (__id, this, null);
			}
			finally {
			}
		}
	}
}
