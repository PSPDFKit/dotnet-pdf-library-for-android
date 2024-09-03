using Android.Runtime;
using Java.Interop;

// Copied the necessary stuff from generated file - PSPDFKit.Listeners.Scrolling.IDocumentScrollListener.cs
// and removed the same code subsequently by adding following in Transforms/Metadata.xml:
// <remove-node path="/api/package[@name='com.pspdfkit.listeners.scrolling']/interface[@name='DocumentScrollListener']/method[@name='onDocumentScrolled']" />
// < remove - node path = "/api/package[@name='com.pspdfkit.listeners.scrolling']/interface[@name='DocumentScrollListener']/method[@name='onScrollStateChanged']" />
//
// This was needed to avoid the multiple defination error for Handler as two events
// (one for new event - DocumentScrolled and ScrollStateChanged - added and one for deprecated.
namespace PSPDFKit.Listeners.Scrolling
{
    // Metadata.xml XPath interface reference: path="/api/package[@name='com.pspdfkit.listeners.scrolling']/interface[@name='DocumentScrollListener']"
    public partial interface IDocumentScrollListener : IJavaObject, IJavaPeerable
    {
        private static readonly JniPeerMembers _members = new XAPeerMembers("com/pspdfkit/listeners/scrolling/DocumentScrollListener", typeof(IDocumentScrollListener), isInterface: true);

        // Metadata.xml XPath method reference: path="/api/package[@name='com.pspdfkit.listeners.scrolling']/interface[@name='DocumentScrollListener']/method[@name='onDocumentScrolled' and count(parameter)=6 and parameter[1][@type='int'] and parameter[2][@type='int'] and parameter[3][@type='int'] and parameter[4][@type='int'] and parameter[5][@type='int'] and parameter[6][@type='int']]"
        [Register("onDocumentScrolled", "(IIIIII)V", "GetOnDocumentScrolled_IIIIIIHandler:PSPDFKit.Listeners.Scrolling.IDocumentScrollListenerInvoker, PSPDFKit.dotnet.Android")]
        void OnDocumentScrolled(int currX, int currY, int maxX, int maxY, int extendX, int extendY);

        // Metadata.xml XPath method reference: path="/api/package[@name='com.pspdfkit.listeners.scrolling']/interface[@name='DocumentScrollListener']/method[@name='onScrollStateChanged' and count(parameter)=1 and parameter[1][@type='com.pspdfkit.listeners.scrolling.ScrollState']]"
        [Register("onScrollStateChanged", "(Lcom/pspdfkit/listeners/scrolling/ScrollState;)V", "GetOnScrollStateChanged_Lcom_pspdfkit_listeners_scrolling_ScrollState_Handler:PSPDFKit.Listeners.Scrolling.IDocumentScrollListenerInvoker, PSPDFKit.dotnet.Android")]
        void OnScrollStateChanged(ScrollState currX);


        delegate void _onDocumentScrolledWithFragmentJniMarshal(IntPtr jnienv, IntPtr klass, IntPtr fragment, int currX, int currY, int maxX, int maxY, int extendX, int extendY);
        private static Delegate? _onDocumentScrolledWithFragmentDelegate;
#pragma warning disable 0169
        [global::System.Obsolete]
        private static Delegate GetOnDocumentScrolledWithFragmentHandler()
        {
            if (_onDocumentScrolledWithFragmentDelegate == null)
                _onDocumentScrolledWithFragmentDelegate = JNINativeWrapper.CreateDelegate(new _onDocumentScrolledWithFragmentJniMarshal(OnDocumentScrolledWithPdfFragment));
            return _onDocumentScrolledWithFragmentDelegate;
        }

        [global::System.Obsolete]
        private static void OnDocumentScrolledWithPdfFragment(IntPtr jnienv, IntPtr native__this, IntPtr native_fragment, int currX, int currY, int maxX, int maxY, int extendX, int extendY)
        {
            var __this = global::Java.Lang.Object.GetObject<IDocumentScrollListener>(jnienv, native__this, JniHandleOwnership.DoNotTransfer)!;
            var fragment = global::Java.Lang.Object.GetObject<UI.PdfFragment>(native_fragment, JniHandleOwnership.DoNotTransfer);
            __this.OnDocumentScrolled(fragment!, currX, currY, maxX, maxY, extendX, extendY);
        }
#pragma warning restore 0169

        // Metadata.xml XPath method reference: path="/api/package[@name='com.pspdfkit.listeners.scrolling']/interface[@name='DocumentScrollListener']/method[@name='onDocumentScrolled' and count(parameter)=7 and parameter[1][@type='com.pspdfkit.ui.PdfFragment'] and parameter[2][@type='int'] and parameter[3][@type='int'] and parameter[4][@type='int'] and parameter[5][@type='int'] and parameter[6][@type='int'] and parameter[7][@type='int']]"
        [global::System.Obsolete(@"deprecated")]
        [Register("onDocumentScrolled", "(Lcom/pspdfkit/ui/PdfFragment;IIIIII)V", "GetOnDocumentScrolledWithFragmentHandler:PSPDFKit.Listeners.Scrolling.IDocumentScrollListener, PSPDFKit.dotnet.Android")]
        virtual unsafe void OnDocumentScrolled(UI.PdfFragment fragment, int currX, int currY, int maxX, int maxY, int extendX, int extendY)
        {
            const string id = "onDocumentScrolled.(Lcom/pspdfkit/ui/PdfFragment;IIIIII)V";
            try
            {
                JniArgumentValue* args = stackalloc JniArgumentValue[7];
                args[0] = new JniArgumentValue((fragment == null) ? IntPtr.Zero : ((global::Java.Lang.Object)fragment).Handle);
                args[1] = new JniArgumentValue(currX);
                args[2] = new JniArgumentValue(currY);
                args[3] = new JniArgumentValue(maxX);
                args[4] = new JniArgumentValue(maxY);
                args[5] = new JniArgumentValue(extendX);
                args[6] = new JniArgumentValue(extendY);
                _members.InstanceMethods.InvokeVirtualVoidMethod(id, this, args);
            }
            finally
            {
                GC.KeepAlive(fragment);
            }
        }

        private static Delegate? _onScrollStateChangedWithFragmentDelegate;
#pragma warning disable 0169
        [global::System.Obsolete]
        private static Delegate GetOnScrollStateChangedWithFragmentHandler()
        {
            if (_onScrollStateChangedWithFragmentDelegate == null)
                _onScrollStateChangedWithFragmentDelegate = JNINativeWrapper.CreateDelegate(new _JniMarshal_PPLL_V(OnScrollStateChangedWithFragment));
            return _onScrollStateChangedWithFragmentDelegate;
        }

        [global::System.Obsolete]
        private static void OnScrollStateChangedWithFragment(IntPtr jnienv, IntPtr native__this, IntPtr native_fragment, IntPtr native_state)
        {
            var __this = global::Java.Lang.Object.GetObject<global::PSPDFKit.Listeners.Scrolling.IDocumentScrollListener>(jnienv, native__this, JniHandleOwnership.DoNotTransfer)!;
            var fragment = global::Java.Lang.Object.GetObject<global::PSPDFKit.UI.PdfFragment>(native_fragment, JniHandleOwnership.DoNotTransfer);
            var state = global::Java.Lang.Object.GetObject<global::PSPDFKit.Listeners.Scrolling.ScrollState>(native_state, JniHandleOwnership.DoNotTransfer);
            __this.OnScrollStateChanged(fragment!, state!);
        }
#pragma warning restore 0169

        // Metadata.xml XPath method reference: path="/api/package[@name='com.pspdfkit.listeners.scrolling']/interface[@name='DocumentScrollListener']/method[@name='onScrollStateChanged' and count(parameter)=2 and parameter[1][@type='com.pspdfkit.ui.PdfFragment'] and parameter[2][@type='com.pspdfkit.listeners.scrolling.ScrollState']]"
        [global::System.Obsolete(@"deprecated")]
        [Register("onScrollStateChanged", "(Lcom/pspdfkit/ui/PdfFragment;Lcom/pspdfkit/listeners/scrolling/ScrollState;)V", "GetOnScrollStateChangedWithFragmentHandler:PSPDFKit.Listeners.Scrolling.IDocumentScrollListener, PSPDFKit.dotnet.Android")]
        virtual unsafe void OnScrollStateChanged(global::PSPDFKit.UI.PdfFragment fragment, global::PSPDFKit.Listeners.Scrolling.ScrollState state)
        {
            const string __id = "onScrollStateChanged.(Lcom/pspdfkit/ui/PdfFragment;Lcom/pspdfkit/listeners/scrolling/ScrollState;)V";
            try
            {
                JniArgumentValue* __args = stackalloc JniArgumentValue[2];
                __args[0] = new JniArgumentValue((fragment == null) ? IntPtr.Zero : ((global::Java.Lang.Object)fragment).Handle);
                __args[1] = new JniArgumentValue((state == null) ? IntPtr.Zero : ((global::Java.Lang.Object)state).Handle);
                _members.InstanceMethods.InvokeVirtualVoidMethod(__id, this, __args);
            }
            finally
            {
                global::System.GC.KeepAlive(fragment);
                global::System.GC.KeepAlive(state);
            }
        }

    }

    internal partial class IDocumentScrollListenerInvoker : global::Java.Lang.Object, IDocumentScrollListener
    {
        static Delegate? _cb_onDocumentScrolled_IIIIII;
#pragma warning disable 0169
        static Delegate GetOnDocumentScrolled_IIIIIIHandler()
        {
            if (_cb_onDocumentScrolled_IIIIII == null)
                _cb_onDocumentScrolled_IIIIII = JNINativeWrapper.CreateDelegate(new _JniMarshal_PPIIIIII_V(n_OnDocumentScrolled_IIIIII));
            return _cb_onDocumentScrolled_IIIIII;
        }

        static void n_OnDocumentScrolled_IIIIII(IntPtr jnienv, IntPtr native__this, int currX, int currY, int maxX, int maxY, int extendX, int extendY)
        {
            var __this = global::Java.Lang.Object.GetObject<global::PSPDFKit.Listeners.Scrolling.IDocumentScrollListener>(jnienv, native__this, JniHandleOwnership.DoNotTransfer)!;
            __this.OnDocumentScrolled(currX, currY, maxX, maxY, extendX, extendY);
        }
#pragma warning restore 0169

        IntPtr _id_onDocumentScrolled_IIIIII;
        public unsafe void OnDocumentScrolled(int currX, int currY, int maxX, int maxY, int extendX, int extendY)
        {
            if (_id_onDocumentScrolled_IIIIII == IntPtr.Zero)
                _id_onDocumentScrolled_IIIIII = JNIEnv.GetMethodID(class_ref, "onDocumentScrolled", "(IIIIII)V");
            JValue* __args = stackalloc JValue[6];
            __args[0] = new JValue(currX);
            __args[1] = new JValue(currY);
            __args[2] = new JValue(maxX);
            __args[3] = new JValue(maxY);
            __args[4] = new JValue(extendX);
            __args[5] = new JValue(extendY);
            JNIEnv.CallVoidMethod(((global::Java.Lang.Object)this).Handle, _id_onDocumentScrolled_IIIIII, __args);
        }

        static Delegate? _cb_onScrollStateChanged_Lcom_pspdfkit_listeners_scrolling_ScrollState_;
#pragma warning disable 0169
        static Delegate GetOnScrollStateChanged_Lcom_pspdfkit_listeners_scrolling_ScrollState_Handler()
        {
            if (_cb_onScrollStateChanged_Lcom_pspdfkit_listeners_scrolling_ScrollState_ == null)
                _cb_onScrollStateChanged_Lcom_pspdfkit_listeners_scrolling_ScrollState_ = JNINativeWrapper.CreateDelegate(new _JniMarshal_PPL_V(n_OnScrollStateChanged_Lcom_pspdfkit_listeners_scrolling_ScrollState_));
            return _cb_onScrollStateChanged_Lcom_pspdfkit_listeners_scrolling_ScrollState_;
        }

        static void n_OnScrollStateChanged_Lcom_pspdfkit_listeners_scrolling_ScrollState_(IntPtr jnienv, IntPtr native__this, IntPtr native_currX)
        {
            var __this = global::Java.Lang.Object.GetObject<global::PSPDFKit.Listeners.Scrolling.IDocumentScrollListener>(jnienv, native__this, JniHandleOwnership.DoNotTransfer)!;
            var currX = global::Java.Lang.Object.GetObject<global::PSPDFKit.Listeners.Scrolling.ScrollState>(native_currX, JniHandleOwnership.DoNotTransfer);
            __this.OnScrollStateChanged(currX!);
        }
#pragma warning restore 0169

        IntPtr _id_onScrollStateChanged_Lcom_pspdfkit_listeners_scrolling_ScrollState_;
        public unsafe void OnScrollStateChanged(global::PSPDFKit.Listeners.Scrolling.ScrollState currX)
        {
            if (_id_onScrollStateChanged_Lcom_pspdfkit_listeners_scrolling_ScrollState_ == IntPtr.Zero)
                _id_onScrollStateChanged_Lcom_pspdfkit_listeners_scrolling_ScrollState_ = JNIEnv.GetMethodID(class_ref, "onScrollStateChanged", "(Lcom/pspdfkit/listeners/scrolling/ScrollState;)V");
            JValue* __args = stackalloc JValue[1];
            __args[0] = new JValue((currX == null) ? IntPtr.Zero : ((global::Java.Lang.Object)currX).Handle);
            JNIEnv.CallVoidMethod(((global::Java.Lang.Object)this).Handle, _id_onScrollStateChanged_Lcom_pspdfkit_listeners_scrolling_ScrollState_, __args);
        }

    }

    // event args for com.pspdfkit.listeners.scrolling.DocumentScrollListener.onDocumentScrolled
    public partial class DocumentScrolledEventArgs : global::System.EventArgs
    {
        public DocumentScrolledEventArgs(global::PSPDFKit.UI.PdfFragment fragment, int currX, int currY, int maxX, int maxY, int extendX, int extendY)
        {
            this._fragment = fragment;
            this._currX = currX;
            this._currY = currY;
            this._maxX = maxX;
            this._maxY = maxY;
            this._extendX = extendX;
            this._extendY = extendY;
        }

        global::PSPDFKit.UI.PdfFragment? _fragment;

        public global::PSPDFKit.UI.PdfFragment? Fragment
        {
            get { return _fragment; }
        }

        public DocumentScrolledEventArgs(int currX, int currY, int maxX, int maxY, int extendX, int extendY)
        {
            this._currX = currX;
            this._currY = currY;
            this._maxX = maxX;
            this._maxY = maxY;
            this._extendX = extendX;
            this._extendY = extendY;
        }

        int _currX;

        public int CurrX
        {
            get { return _currX; }
        }

        int _currY;

        public int CurrY
        {
            get { return _currY; }
        }

        int _maxX;

        public int MaxX
        {
            get { return _maxX; }
        }

        int _maxY;

        public int MaxY
        {
            get { return _maxY; }
        }

        int _extendX;

        public int ExtendX
        {
            get { return _extendX; }
        }

        int _extendY;

        public int ExtendY
        {
            get { return ExtendY; }
        }

    }

    // event args for com.pspdfkit.listeners.scrolling.DocumentScrollListener.onScrollStateChanged
    public partial class ScrollStateChangedEventArgs : global::System.EventArgs
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ScrollStateChangedEventArgs(global::PSPDFKit.Listeners.Scrolling.ScrollState currX)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            this._currX = currX;
        }

        global::PSPDFKit.Listeners.Scrolling.ScrollState _currX;

        public global::PSPDFKit.Listeners.Scrolling.ScrollState CurrX
        {
            get { return _currX; }
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ScrollStateChangedEventArgs(global::PSPDFKit.UI.PdfFragment fragment, global::PSPDFKit.Listeners.Scrolling.ScrollState state)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            this._fragment = fragment;
            this._state = state;
        }

        global::PSPDFKit.UI.PdfFragment _fragment;

        public global::PSPDFKit.UI.PdfFragment Fragment
        {
            get { return _fragment; }
        }

        global::PSPDFKit.Listeners.Scrolling.ScrollState _state;

        public global::PSPDFKit.Listeners.Scrolling.ScrollState State
        {
            get { return _state; }
        }

    }

    internal sealed partial class IDocumentScrollListenerImplementor : global::Java.Lang.Object, IDocumentScrollListener
    {

        object? _sender;

        public unsafe IDocumentScrollListenerImplementor(object sender) : base(IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
        {
            const string __id = "()V";
            if (((global::Java.Lang.Object)this).Handle != IntPtr.Zero)
                return;
            var h = JniPeerMembers.InstanceMethods.StartCreateInstance(__id, ((object)this).GetType(), null);
            SetHandle(h.Handle, JniHandleOwnership.TransferLocalRef);
            JniPeerMembers.InstanceMethods.FinishCreateInstance(__id, this, null);
            this._sender = sender;
        }

#pragma warning disable 0649
        public EventHandler<DocumentScrolledEventArgs>? OnDocumentScrolledHandler;
#pragma warning restore 0649

        public void OnDocumentScrolled(global::PSPDFKit.UI.PdfFragment fragment, int currX, int currY, int maxX, int maxY, int extendX, int extendY)
        {
            var __h = OnDocumentScrolledHandler;
            if (__h != null)
                __h(_sender, new DocumentScrolledEventArgs(fragment, currX, currY, maxX, maxY, extendX, extendY));
        }

        public void OnDocumentScrolled(int currX, int currY, int maxX, int maxY, int extendX, int extendY)
        {
            var __h = OnDocumentScrolledHandler;
            if (__h != null)
                __h(_sender, new DocumentScrolledEventArgs(currX, currY, maxX, maxY, extendX, extendY));
        }

#pragma warning disable 0649
        public EventHandler<ScrollStateChangedEventArgs>? OnScrollStateChangedHandler;
#pragma warning restore 0649

        public void OnScrollStateChanged(global::PSPDFKit.Listeners.Scrolling.ScrollState currX)
        {
            var __h = OnScrollStateChangedHandler;
            if (__h != null)
                __h(_sender, new ScrollStateChangedEventArgs(currX));
        }

        public void OnScrollStateChanged(global::PSPDFKit.UI.PdfFragment fragment, global::PSPDFKit.Listeners.Scrolling.ScrollState state)
        {
            var __h = OnScrollStateChangedHandler;
            if (__h != null)
                __h(_sender, new ScrollStateChangedEventArgs(fragment, state));
        }
    }
}
