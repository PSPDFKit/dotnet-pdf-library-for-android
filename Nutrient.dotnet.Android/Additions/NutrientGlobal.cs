using Android.Content;
using PSPDFKit.Initialization;

[assembly: MetaData(
    name: "nutrient_automatic_initialize",
    Value = "false"
)]

namespace PSPDFKit
{

    public sealed partial class NutrientGlobal {

		internal static CrossPlatformTechnology ProductIdentifier = CrossPlatformTechnology.DotNetBindings;

		public static void Initialize (Context context, string? licenseKey, IList<string>? fontPaths = null)
		{
			if (context is null)
				throw new NullReferenceException (nameof (context));

            var options = new InitializationOptions(licenseKey: licenseKey, fontPaths: fontPaths ?? [], crossPlatformTechnology: ProductIdentifier, applicationPolicy: null);
            Initialize (context, options);
		}


        public static void Initialize(Context context, string? licenseKey)
        {
            if (context is null)
                throw new NullReferenceException(nameof(context));

            var options = new InitializationOptions(licenseKey: licenseKey, fontPaths: [], crossPlatformTechnology: ProductIdentifier, applicationPolicy: null);
            Initialize(context, options);
        }
    }
}