
var target = Argument ("target", "Default");

// Nice online pom dependency explorer
// https://jar-download.com/

var PSPDFKIT_VERSION = "8.10.0";
var SERVICERELEASE_VERSION = "0"; // This is combined with the PSPDFKIT_VERSION variable for the NuGet Package version
var RXANDROID_VERSION = "2.1.0";
var RXJAVA_VERSION = "2.2.4"; // Check Reactive-Streams if updated.
var REACTIVESTREAMS_VERSION = "1.0.2";
var YOUTUBE_VERSION = "1.2.2";
var RELINKER_VERSION = "1.4.5";
var KOTLINSTDLIB_VERSION = "1.3.50"; // Check Annotations version if updated.
var KOTLIANNOTATIONS_VERSION = "13.0";
var KOTLINSTDLIBCOMMON_VERSION = "1.3.50";
var YEARCLASS_VERSION = "2.0.0";

var OKHTTP3_VERSION = "3.9.0"; // Check OKIO version if updated.
var OKHTTP3LOGGING_VERSION = "3.9.0";
var OKIO_VERSION = "1.13.0";


var PSPDFKITURL = $"https://customers.pspdfkit.com/maven/com/pspdfkit/pspdfkit/{PSPDFKIT_VERSION}/pspdfkit-{PSPDFKIT_VERSION}.aar";
var RXANDROIDURL = $"http://search.maven.org/remotecontent?filepath=io/reactivex/rxjava2/rxandroid/{RXANDROID_VERSION}/rxandroid-{RXANDROID_VERSION}.aar";
var RXJAVAURL = $"http://search.maven.org/remotecontent?filepath=io/reactivex/rxjava2/rxjava/{RXJAVA_VERSION}/rxjava-{RXJAVA_VERSION}.jar";
var YOUTUBEURL = $"https://developers.google.com/youtube/android/player/downloads/YouTubeAndroidPlayerApi-{YOUTUBE_VERSION}.zip";
var REACTIVESTREAMSURL = $"http://search.maven.org/remotecontent?filepath=org/reactivestreams/reactive-streams/{REACTIVESTREAMS_VERSION}/reactive-streams-{REACTIVESTREAMS_VERSION}.jar";
var RELINKERURL = $"https://search.maven.org/remotecontent?filepath=com/getkeepsafe/relinker/relinker/{RELINKER_VERSION}/relinker-{RELINKER_VERSION}.aar";
var OKHTTP3URL = $"http://search.maven.org/remotecontent?filepath=com/squareup/okhttp3/okhttp/{OKHTTP3_VERSION}/okhttp-{OKHTTP3_VERSION}.jar";
var OKHTTP3LOGGINGURL = $"https://search.maven.org/remotecontent?filepath=com/squareup/okhttp3/logging-interceptor/{OKHTTP3LOGGING_VERSION}/logging-interceptor-{OKHTTP3LOGGING_VERSION}.jar";
var OKIOURL = $"http://search.maven.org/remotecontent?filepath=com/squareup/okio/okio/{OKIO_VERSION}/okio-{OKIO_VERSION}.jar";
var KOTLINSTDLIBURL = $"http://search.maven.org/remotecontent?filepath=org/jetbrains/kotlin/kotlin-stdlib/{KOTLINSTDLIB_VERSION}/kotlin-stdlib-{KOTLINSTDLIB_VERSION}.jar";
var KOTLINSTDLIBCOMMONURL = $"http://search.maven.org/remotecontent?filepath=org/jetbrains/kotlin/kotlin-stdlib-common/{KOTLINSTDLIBCOMMON_VERSION}/kotlin-stdlib-common-{KOTLINSTDLIBCOMMON_VERSION}.jar";
var KOTLIANNOTATIONSURL = $"http://search.maven.org/remotecontent?filepath=org/jetbrains/annotations/{KOTLIANNOTATIONS_VERSION}/annotations-{KOTLIANNOTATIONS_VERSION}.jar";
var YEARCLASSURL = $"http://search.maven.org/remotecontent?filepath=com/facebook/device/yearclass/yearclass/{YEARCLASS_VERSION}/yearclass-{YEARCLASS_VERSION}.jar";

var NUGET_API_KEY = EnvironmentVariable("NUGET_API_KEY");

Task ("FetchDependencies")
	.Does (() => {

		if(!DirectoryExists ($"./PSPDFKit.dotnet.Android/Jars"))
			CreateDirectory ($"./PSPDFKit.dotnet.Android/Jars");

		Information ("Downloading all the dependencies...");
		DownloadFile (PSPDFKITURL, $"./PSPDFKit.dotnet.Android/Jars/pspdfkit-{PSPDFKIT_VERSION}.aar");
		DownloadFile (RELINKERURL, $"./PSPDFKit.dotnet.Android/Jars/relinker-{RELINKER_VERSION}.aar");
		DownloadFile (YEARCLASSURL, $"./PSPDFKit.dotnet.Android/Jars/yearclass-{YEARCLASS_VERSION}.jar");
});

Task ("ExtractAars")
	.IsDependentOn ("FetchDependencies")
	.Does (() => {
		Information ("Unzipping needed dependencies...");

		var delDirSettings = new DeleteDirectorySettings { Recursive = true, Force = true };
		if(DirectoryExists ($"./PSPDFKit.dotnet.Android/Jars/YouTubeAndroidPlayerApi-{YOUTUBE_VERSION}"))
			DeleteDirectory ($"./PSPDFKit.dotnet.Android/Jars/YouTubeAndroidPlayerApi-{YOUTUBE_VERSION}", delDirSettings);
		if(DirectoryExists ($"./PSPDFKit.dotnet.Android/Jars/rxandroid-{RXANDROID_VERSION}"))
			DeleteDirectory ($"./PSPDFKit.dotnet.Android/Jars/rxandroid-{RXANDROID_VERSION}", delDirSettings);
		if(DirectoryExists ($"./PSPDFKit.dotnet.Android/Jars/relinker-{RELINKER_VERSION}"))
			DeleteDirectory ($"./PSPDFKit.dotnet.Android/Jars/relinker-{RELINKER_VERSION}", delDirSettings);

		Unzip ($"./PSPDFKit.dotnet.Android/Jars/relinker-{RELINKER_VERSION}.aar", $"./PSPDFKit.dotnet.Android/Jars/relinker-{RELINKER_VERSION}");
  		CopyFile ($"./PSPDFKit.dotnet.Android/Jars/relinker-{RELINKER_VERSION}/classes.jar", $"./PSPDFKit.dotnet.Android/Jars/relinker-{RELINKER_VERSION}.jar");

		if(DirectoryExists ($"./PSPDFKit.dotnet.Android/Jars/relinker-{RELINKER_VERSION}")) {
			DeleteDirectory ($"./PSPDFKit.dotnet.Android/Jars/relinker-{RELINKER_VERSION}", delDirSettings);
			DeleteFile ($"./PSPDFKit.dotnet.Android/Jars/relinker-{RELINKER_VERSION}.aar");
		}
});

Task ("BuildPSPDFKit")
	.Does (() => {
		if (FileExists ($"./PSPDFKit.dotnet.Android/Jars/pspdfkit-{PSPDFKIT_VERSION}.aar")) {
			Information ("Building PSPDFKit.dotnet.Android.dll");
			Information ("PLEASE WAIT, it might take a few minutes to build...");

			var msBuildSettings = new DotNetMSBuildSettings ()
			.WithProperty ("AssemblyVersion", PSPDFKIT_VERSION);

			var dotNetBuildSettings = new DotNetBuildSettings { 
				Configuration = "Release",
//				Verbosity = DotNetVerbosity.Diagnostic,
				MSBuildSettings = msBuildSettings
			};
			DotNetBuild ("./PSPDFKit.dotnet.Android.sln", dotNetBuildSettings);

			Information (@"DONE! You will find the PSPDFKit.dotnet.Android.dll inside the 'bin\Release' directory of 'PSPDFKit.dotnet.Android' folder.");
		} else {
			Warning ($"./PSPDFKit.dotnet.Android/Jars/pspdfkit-{PSPDFKIT_VERSION}.aar file not found.");
			Warning ($"PSPDFKit.dotnet.Android.dll was not built.");
		}
});

Task ("Default")
	.IsDependentOn ("ExtractAars")
	.IsDependentOn ("BuildPSPDFKit")
	.Does (() => {
		Information ("Build Done!");
});

Task ("NuGet")
	.IsDependentOn("Default")
	.Does (() =>
{
	if(!DirectoryExists("./nuget/pkgs/"))
		CreateDirectory("./nuget/pkgs");

	var commit = GetGitShortCommit ();
	Console.WriteLine ($"Commit: {commit}");

	XmlPoke ("Directory.Build.props", "/Project/PropertyGroup/PSVersion", $"{PSPDFKIT_VERSION}.{SERVICERELEASE_VERSION}+sha.{commit}");

	var dotNetPackSettings = new DotNetPackSettings {
		Configuration = "Release",
		NoRestore = true,
		NoBuild = true,
		OutputDirectory = "./nuget/pkgs",
//		Verbosity = DotNeVerbosity.Diagnostic,
	};

	DotNetPack ("./PSPDFKit.dotnet.Android.sln", dotNetPackSettings);
});

Task ("NuGet-Push")
	.IsDependentOn("Nuget")
	.Does (() =>
{
	var package = "./nuget/pkgs/PSPDFKit.dotnet.Android." + PSPDFKIT_VERSION +".nupkg";
	NuGetPush(package, new NuGetPushSettings {
			Source = "https://api.nuget.org/v3/index.json",
			ApiKey = NUGET_API_KEY
	});
});

Task ("Clean")
	.Does (() => {
		if (FileExists ("./PSPDFKit.dotnet.Android.dll"))
			DeleteFile ("./PSPDFKit.dotnet.Android.dll");

		var delDirSettings = new DeleteDirectorySettings { Recursive = true, Force = true };

		if (DirectoryExists ("./packages/"))
			DeleteDirectory ("./packages", delDirSettings);

		if (DirectoryExists ("./PSPDFKit.dotnet.Android/bin/"))
			DeleteDirectory ("./PSPDFKit.dotnet.Android/bin", delDirSettings);

		if (DirectoryExists ("./PSPDFKit.dotnet.Android/obj/"))
			DeleteDirectory ("./PSPDFKit.dotnet.Android/obj", delDirSettings);

		if (DirectoryExists ("./PSPDFKit.dotnet.Android/Jars/")) {
			DeleteDirectory ("./PSPDFKit.dotnet.Android/Jars", delDirSettings);
		}
});

Task ("Clean-obj-bin")
	.Does (() => {
		var delDirSettings = new DeleteDirectorySettings { Recursive = true, Force = true };

		var dirs = new [] {
			"./PSPDFKit.dotnet.Android",
			"./samples/AndroidSample/AndroidSample",
			"./samples/PSPDFCatalog",
			"./samples/XamarinForms/Droid",
			"./samples/XamarinForms/XFSample",
		};

		foreach (var dir in dirs) {
			if (DirectoryExists ($"{dir}/bin/"))
				DeleteDirectory ($"{dir}/bin", delDirSettings);

			if (DirectoryExists ($"{dir}/obj/"))
				DeleteDirectory ($"{dir}/obj", delDirSettings);
		}
});

string GetGitShortCommit ()
{
	StartProcess ("git", new ProcessSettings {
		Arguments = "rev-parse --short HEAD",
		RedirectStandardOutput = true
	}, out var lines);
	return lines.FirstOrDefault ();
}

RunTarget (target);
