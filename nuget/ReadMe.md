# Nutrient.NET (Android)

The Nutrient SDK is a framework that allows you to view, annotate, sign, and fill PDF forms on iOS, Android, Windows, macOS, and Web.

Nutrient Instant adds real-time collaboration features to seamlessly share, edit, and annotate PDF documents.

## Integration

Check your `.csproj` file to determine which integration method to follow:

- **Single platform** (e.g., `<TargetFramework>net10.0-android</TargetFramework>`) - Follow [.NET for Android](#net-for-android)
- **Multiple platforms** (e.g., `<TargetFrameworks>net10.0-android;net10.0-ios</TargetFrameworks>`) - Follow [.NET MAUI (Android)](#net-maui-android)

### .NET for Android

1. **Add NuGet package** to your [`.csproj`](https://github.com/PSPDFKit/dotnet-pdf-library-for-android/blob/master/AndroidSample/AndroidSample.csproj) file:

```xml
<ItemGroup>
  <PackageReference Include="Nutrient.dotnet.Android" Version="$VERSION$" />
</ItemGroup>
```

2. **Initialize Nutrient** in your [`MainActivity.cs`](https://github.com/PSPDFKit/dotnet-pdf-library-for-android/blob/master/AndroidSample/MainActivity.cs):

```csharp
NutrientGlobal.Initialize(this, licenseKey: null);
```

3. **Display a PDF** using [`PdfActivity`](https://github.com/PSPDFKit/dotnet-pdf-library-for-android/blob/master/AndroidSample/MainActivity.cs)

### .NET MAUI (Android)

1. **Add NuGet package** conditionally for Android in your [`.csproj`](https://github.com/PSPDFKit/dotnet-pdf-library-for-mobiles/blob/master/combined/dotnet-pdf-library-for-mobiles/dotnet-pdf-library-for-mobiles.csproj) file:

```xml
<Choose>
  <When Condition="'$(TargetFramework)' == 'net10.0-android'">
    <ItemGroup>
      <PackageReference Include="Nutrient.dotnet.Android" Version="$VERSION$" />
    </ItemGroup>
  </When>
</Choose>
```

2. **Create the shared PdfView** in [`Views/PdfView.cs`](https://github.com/PSPDFKit/dotnet-pdf-library-for-mobiles/blob/master/combined/dotnet-pdf-library-for-mobiles/Views/PdfView.cs)

3. **Create the handler interface** in [`Views/IPdfViewHandler.cs`](https://github.com/PSPDFKit/dotnet-pdf-library-for-mobiles/blob/master/combined/dotnet-pdf-library-for-mobiles/Views/IPdfViewHandler.cs)

4. **Create the Android platform handler** in [`Platforms/Android/Handlers/PdfViewHandler.cs`](https://github.com/PSPDFKit/dotnet-pdf-library-for-mobiles/blob/master/combined/dotnet-pdf-library-for-mobiles/Platforms/Android/Handlers/PdfViewHandler.cs)

5. **Register the handler** in [`MauiProgram.cs`](https://github.com/PSPDFKit/dotnet-pdf-library-for-mobiles/blob/master/combined/dotnet-pdf-library-for-mobiles/MauiProgram.cs)

6. **Use PdfView in a XAML page** - see [`Examples/Playground.xaml`](https://github.com/PSPDFKit/dotnet-pdf-library-for-mobiles/blob/master/combined/dotnet-pdf-library-for-mobiles/Examples/Playground.xaml) and [`Examples/Playground.xaml.cs`](https://github.com/PSPDFKit/dotnet-pdf-library-for-mobiles/blob/master/combined/dotnet-pdf-library-for-mobiles/Examples/Playground.xaml.cs)

## Support

Nutrient offers support via https://support.nutrient.io/hc/en-us/requests/new.

Are you evaluating our SDK? That's great, we're happy to help out! To make sure this is fast, please use a work email and have someone from your company fill out our sales form: https://www.nutrient.io/contact-sales?=sdk

Visit https://www.nutrient.io/guides/android/dotnet/ for more information on how to setup and use the SDK.

## Examples

Examples are available at
- https://github.com/PSPDFKit/dotnet-pdf-library-for-android
- https://github.com/PSPDFKit/dotnet-pdf-library-for-mobiles
