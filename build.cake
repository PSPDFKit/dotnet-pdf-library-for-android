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

// The Nutrient AAR is hosted on an authenticated Maven registry that
// <AndroidMavenLibrary> does not support, so it has to be fetched manually.
var NUTRIENTURL = $"https://my.nutrient.io/maven/io/nutrient/nutrient/{ANDROID_VERSION}/nutrient-{ANDROID_VERSION}.aar";

var NUTRIENT_AAR_NAME = $"Nutrient-Android-SDK-AAR-{ANDROID_VERSION}.aar";

var NUGET_API_KEY = EnvironmentVariable("NUGET_API_KEY");

Task("FetchDependencies")
	.Does(() =>
	{
		if (!DirectoryExists($"./Nutrient.dotnet.Android/Jars"))
			CreateDirectory($"./Nutrient.dotnet.Android/Jars");

		Information("Downloading the Nutrient SDK AAR...");
		DownloadFile(NUTRIENTURL, $"./Nutrient.dotnet.Android/Jars/{NUTRIENT_AAR_NAME}");
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
	.IsDependentOn("FetchDependencies")
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
	.IsDependentOn("NuGet")
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
