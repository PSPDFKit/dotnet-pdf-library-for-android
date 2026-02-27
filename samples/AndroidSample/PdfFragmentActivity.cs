using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

using PSPDFKit;
using PSPDFKit.Configuration;
using PSPDFKit.Configuration.Page;
using PSPDFKit.Listeners.Scrolling;
using PSPDFKit.UI;

namespace AndroidSample;

[Activity(Label = "@string/app_name", Theme = "@style/AppTheme", Exported = false)]
public class PdfFragmentActivity : AndroidX.AppCompat.App.AppCompatActivity
{
    const string ExtraDocumentUri = "document_uri";

    IDocumentScrollListener? documentScrollListener;
    PdfFragment? pdfFragment;

    public static Intent CreateIntent(Context context, Android.Net.Uri documentUri)
    {
        var intent = new Intent(context, typeof(PdfFragmentActivity));
        intent.PutExtra(ExtraDocumentUri, documentUri.ToString());
        intent.AddFlags(ActivityFlags.GrantReadUriPermission);
        return intent;
    }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        NutrientGlobal.Initialize(this, licenseKey: null);
        SetContentView(Resource.Layout.activity_pdf_fragment);

        var documentUriString = Intent?.GetStringExtra(ExtraDocumentUri);
        if (string.IsNullOrWhiteSpace(documentUriString))
        {
            Toast.MakeText(this, "Missing document URI.", ToastLength.Short)?.Show();
            Finish();
            return;
        }

        if (savedInstanceState != null)
            return;

        var documentUri = Android.Net.Uri.Parse(documentUriString);
        if (documentUri == null)
        {
            Toast.MakeText(this, "Invalid document URI.", ToastLength.Short)?.Show();
            Finish();
            return;
        }

        var configuration = new PdfConfiguration.Builder()
            .ScrollDirection(PageScrollDirection.Horizontal!)
            .FitMode(PageFitMode.FitToWidth!)
            .Build();

        pdfFragment = PdfFragment.NewInstance(documentUri, configuration);
        documentScrollListener = DocumentScrollListenerSample.AttachTo(pdfFragment);

        SupportFragmentManager
            .BeginTransaction()
            .Replace(Resource.Id.pdf_fragment_container, pdfFragment)
            .Commit();
    }

    protected override void OnDestroy()
    {
        if (pdfFragment != null && documentScrollListener != null)
        {
            pdfFragment.RemoveDocumentScrollListener(documentScrollListener);
            documentScrollListener.Dispose();
            documentScrollListener = null;
        }

        base.OnDestroy();
    }
}
