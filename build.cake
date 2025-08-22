var NUTRIENT_VERSION = Argument<string>("nutrient-version", null);
var target = Argument("target", "Default");

if (string.IsNullOrEmpty(NUTRIENT_VERSION))
{
	throw new ArgumentException("nutrient-version argument is required. Please provide it using --nutrient-version=X.X.X");
}

// Extract service release version from NUTRIENT_VERSION if it exists (e.g. "10.1.2.3" -> "3")
var versionParts = NUTRIENT_VERSION.Split('.');

// Android version is first 3 parts of the NUTRIENT_VERSION
var ANDROID_VERSION = string.Join(".", versionParts.Take(3));
var SERVICERELEASE_VERSION = versionParts.Length == 4 ? versionParts[3] : "0"; // This is combined with the NUTRIENT_VERSION variable for the NuGet Package version

// Nice online pom dependency explorer
// https://jar-download.com/
var RELINKER_VERSION = "1.4.5";
var YEARCLASS_VERSION = "2.0.0";
var IMMUTABLE_COLLECTIONS_VERSION = "0.3.8";


var NUTRIENTURL = $"https://my.nutrient.io/maven/io/nutrient/nutrient/{ANDROID_VERSION}/nutrient-{ANDROID_VERSION}.aar";
var RELINKERURL = $"https://search.maven.org/remotecontent?filepath=com/getkeepsafe/relinker/relinker/{RELINKER_VERSION}/relinker-{RELINKER_VERSION}.aar";
var YEARCLASSURL = $"http://search.maven.org/remotecontent?filepath=com/facebook/device/yearclass/yearclass/{YEARCLASS_VERSION}/yearclass-{YEARCLASS_VERSION}.jar";
var IMMUTABLE_COLLECTIONS_URL = $"https://search.maven.org/remotecontent?filepath=org/jetbrains/kotlinx/kotlinx-collections-immutable/{IMMUTABLE_COLLECTIONS_VERSION}/kotlinx-collections-immutable-{IMMUTABLE_COLLECTIONS_VERSION}.jar";
var IMMUTABLE_COLLECTIONS_JVM_URL = $"https://search.maven.org/remotecontent?filepath=org/jetbrains/kotlinx/kotlinx-collections-immutable-jvm/{IMMUTABLE_COLLECTIONS_VERSION}/kotlinx-collections-immutable-jvm-{IMMUTABLE_COLLECTIONS_VERSION}.jar";

var NUTRIENT_AAR_NAME = $"Nutrient-Android-SDK-AAR-{ANDROID_VERSION}.aar";

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
		DownloadFile(IMMUTABLE_COLLECTIONS_URL, $"./Nutrient.dotnet.Android/Jars/kotlinx-collections-immutable-{IMMUTABLE_COLLECTIONS_VERSION}.jar");
		DownloadFile(IMMUTABLE_COLLECTIONS_JVM_URL, $"./Nutrient.dotnet.Android/Jars/kotlinx-collections-immutable-jvm-{IMMUTABLE_COLLECTIONS_VERSION}.jar");
	});

Task("ExtractAars")
	.IsDependentOn("FetchDependencies")
	.Does(() =>
	{
		Information("Unzipping needed dependencies...");

		var delDirSettings = new DeleteDirectorySettings { Recursive = true, Force = true };
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
			.WithProperty("AssemblyVersion", ANDROID_VERSION);

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

	XmlPoke("Directory.Build.props", "/Project/PropertyGroup/PSVersion", $"{ANDROID_VERSION}.{SERVICERELEASE_VERSION}+sha.{commit}");

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
	var package = "./nuget/pkgs/Nutrient.dotnet.Android." + ANDROID_VERSION + ".nupkg";
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
