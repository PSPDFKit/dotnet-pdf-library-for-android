
var target = Argument("target", "Default");

// Nice online pom dependency explorer
// https://jar-download.com/

var NUTRIENT_VERSION = "10.0.2"; // REMEMBER TO UPDATE THE SERVICE RELEASE VERSION TO 0 FOR NEXT RELEASE
var SERVICERELEASE_VERSION = "1"; // This is combined with the NUTRIENT_VERSION variable for the NuGet Package version
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


var NUTRIENTURL = $"https://my.nutrient.io/maven/io/nutrient/nutrient/{NUTRIENT_VERSION}/nutrient-{NUTRIENT_VERSION}.aar";
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

var NUTRIENT_AAR_NAME = $"Nutrient-Android-SDK-AAR-{NUTRIENT_VERSION}.aar";

var NUGET_API_KEY = EnvironmentVariable("NUGET_API_KEY");

Task("FetchDependencies")
	.Does(() =>
	{

		if (!DirectoryExists($"./Nutrient.dotnet.Android/Jars"))
			CreateDirectory($"./Nutrient.dotnet.Android/Jars");

		Information("Downloading all the dependencies...");
		DownloadFile(NUTRIENTURL, $"./Nutrient.dotnet.Android/Jars/{NUTRIENT_AAR_NAME}");
		DownloadFile(RELINKERURL, $"./Nutrient.dotnet.Android/Jars/relinker-{RELINKER_VERSION}.aar");
		DownloadFile(YEARCLASSURL, $"./Nutrient.dotnet.Android/Jars/yearclass-{YEARCLASS_VERSION}.jar");
	});

Task("ExtractAars")
	.IsDependentOn("FetchDependencies")
	.Does(() =>
	{
		Information("Unzipping needed dependencies...");

		var delDirSettings = new DeleteDirectorySettings { Recursive = true, Force = true };
		if (DirectoryExists($"./Nutrient.dotnet.Android/Jars/YouTubeAndroidPlayerApi-{YOUTUBE_VERSION}"))
			DeleteDirectory($"./Nutrient.dotnet.Android/Jars/YouTubeAndroidPlayerApi-{YOUTUBE_VERSION}", delDirSettings);
		if (DirectoryExists($"./Nutrient.dotnet.Android/Jars/rxandroid-{RXANDROID_VERSION}"))
			DeleteDirectory($"./Nutrient.dotnet.Android/Jars/rxandroid-{RXANDROID_VERSION}", delDirSettings);
		if (DirectoryExists($"./Nutrient.dotnet.Android/Jars/relinker-{RELINKER_VERSION}"))
			DeleteDirectory($"./Nutrient.dotnet.Android/Jars/relinker-{RELINKER_VERSION}", delDirSettings);

		Unzip($"./Nutrient.dotnet.Android/Jars/relinker-{RELINKER_VERSION}.aar", $"./Nutrient.dotnet.Android/Jars/relinker-{RELINKER_VERSION}");
		CopyFile($"./Nutrient.dotnet.Android/Jars/relinker-{RELINKER_VERSION}/classes.jar", $"./Nutrient.dotnet.Android/Jars/relinker-{RELINKER_VERSION}.jar");

		if (DirectoryExists($"./Nutrient.dotnet.Android/Jars/relinker-{RELINKER_VERSION}"))
		{
			DeleteDirectory($"./Nutrient.dotnet.Android/Jars/relinker-{RELINKER_VERSION}", delDirSettings);
			DeleteFile($"./Nutrient.dotnet.Android/Jars/relinker-{RELINKER_VERSION}.aar");
		}
	});

Task("BuildNutrient")
	.Does(() =>
	{
		if (FileExists($"./Nutrient.dotnet.Android/Jars/{NUTRIENT_AAR_NAME}"))
		{
			Information("Building Nutrient.dotnet.Android.dll");
			Information("PLEASE WAIT, it might take a few minutes to build...");

			var msBuildSettings = new DotNetMSBuildSettings()
			.WithProperty("AssemblyVersion", NUTRIENT_VERSION);

			var dotNetBuildSettings = new DotNetBuildSettings
			{
				Configuration = "Release",
				//				Verbosity = DotNetVerbosity.Diagnostic,
				MSBuildSettings = msBuildSettings
			};
			DotNetBuild("./Nutrient.dotnet.Android.sln", dotNetBuildSettings);

			Information(@"DONE! You will find the Nutrient.dotnet.Android.dll inside the 'bin\Release' directory of 'Nutrient.dotnet.Android' folder.");
		}
		else
		{
			Warning($"./Nutrient.dotnet.Android/Jars/{NUTRIENT_AAR_NAME} file not found.");
			Warning($"Nutrient.dotnet.Android.dll was not built.");
		}
	});

Task("Default")
	.IsDependentOn("ExtractAars")
	.IsDependentOn("BuildNutrient")
	.Does(() =>
	{
		Information("Build Done!");
	});

Task("NuGet")
	.IsDependentOn("Default")
	.Does(() =>
{
	if (!DirectoryExists("./nuget/pkgs/"))
		CreateDirectory("./nuget/pkgs");

	var commit = GetGitShortCommit();
	Console.WriteLine($"Commit: {commit}");

	XmlPoke("Directory.Build.props", "/Project/PropertyGroup/PSVersion", $"{NUTRIENT_VERSION}.{SERVICERELEASE_VERSION}+sha.{commit}");

	var dotNetPackSettings = new DotNetPackSettings
	{
		Configuration = "Release",
		NoRestore = true,
		NoBuild = true,
		OutputDirectory = "./nuget/pkgs",
		//		Verbosity = DotNeVerbosity.Diagnostic,
	};

	DotNetPack("./Nutrient.dotnet.Android.sln", dotNetPackSettings);
});

Task("NuGet-Push")
	.IsDependentOn("Nuget")
	.Does(() =>
{
	var package = "./nuget/pkgs/Nutrient.dotnet.Android." + NUTRIENT_VERSION + ".nupkg";
	NuGetPush(package, new NuGetPushSettings
	{
		Source = "https://api.nuget.org/v3/index.json",
		ApiKey = NUGET_API_KEY
	});
});

Task("Clean")
	.Does(() =>
	{
		if (FileExists("./Nutrient.dotnet.Android.dll"))
			DeleteFile("./Nutrient.dotnet.Android.dll");

		var delDirSettings = new DeleteDirectorySettings { Recursive = true, Force = true };

		if (DirectoryExists("./packages/"))
			DeleteDirectory("./packages", delDirSettings);

		if (DirectoryExists("./Nutrient.dotnet.Android/bin/"))
			DeleteDirectory("./Nutrient.dotnet.Android/bin", delDirSettings);

		if (DirectoryExists("./Nutrient.dotnet.Android/obj/"))
			DeleteDirectory("./Nutrient.dotnet.Android/obj", delDirSettings);

		if (DirectoryExists("./Nutrient.dotnet.Android/Jars/"))
		{
			DeleteDirectory("./Nutrient.dotnet.Android/Jars", delDirSettings);
		}
	});

Task("Clean-obj-bin")
	.Does(() =>
	{
		var delDirSettings = new DeleteDirectorySettings { Recursive = true, Force = true };

		var dirs = new[] {
			"./Nutrient.dotnet.Android",
			"./samples/AndroidSample/AndroidSample",
			"./samples/PSPDFCatalog",
			"./samples/XamarinForms/Droid",
			"./samples/XamarinForms/XFSample",
		};

		foreach (var dir in dirs)
		{
			if (DirectoryExists($"{dir}/bin/"))
				DeleteDirectory($"{dir}/bin", delDirSettings);

			if (DirectoryExists($"{dir}/obj/"))
				DeleteDirectory($"{dir}/obj", delDirSettings);
		}
	});

string GetGitShortCommit()
{
	StartProcess("git", new ProcessSettings
	{
		Arguments = "rev-parse --short HEAD",
		RedirectStandardOutput = true
	}, out var lines);
	return lines.FirstOrDefault();
}

RunTarget(target);
