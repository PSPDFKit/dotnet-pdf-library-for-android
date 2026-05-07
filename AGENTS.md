# Nutrient .NET for Android

Agent instructions for the `dotnet-pdf-library-for-android` repository. This is a Java-to-C# binding library for the Nutrient Android PDF SDK.

## Architecture Overview

This project produces a single NuGet package (`Nutrient.dotnet.Android`) that wraps the native Nutrient Android SDK AAR, enabling .NET developers to use Nutrient PDF functionality from C#.

### How the Binding Works

1. The native Nutrient Android SDK is distributed as an AAR (Android Archive).
2. The binding project uses .NET for Android's binding generator to produce C# types from the Java API surface.
3. `Transforms/Metadata.xml` customizes the generated bindings (namespace mapping, API removal, type adjustments).
4. `Additions/` contains hand-written C# code that extends or fixes the auto-generated bindings.
5. The result is `Nutrient.dotnet.Android.dll`, packaged as a NuGet.

### Directory Structure

```text
android/
├── build.cake / build.sh / build.ps1  # Cake build script and entry points
├── VERSION                            # Current Nutrient Android SDK version
├── Directory.Build.props              # Shared NuGet version (PSVersion)
├── Directory.Build.targets            # MSBuild workarounds
├── Nutrient.dotnet.Android/           # Main binding project
│   ├── Jars/                          # Downloaded AAR + dependency JARs
│   ├── Transforms/                    # Binding customizations (Metadata.xml, enum mappings)
│   └── Additions/                     # Hand-written C# extensions
├── Nutrient.dotnet.Android.Tests/     # Test project
├── nuget/                             # NuGet packaging assets (pkgs/, readme, icon)
├── samples/                           # Sample app(s) demonstrating SDK usage
├── scripts/                           # Build validation scripts
└── *.sln                              # Solution files
```

When working in this project, list directory contents to discover the current files — specific filenames in `Additions/`, `Transforms/`, and `samples/` may change across versions.

### Build Pipeline

The build uses [Cake](https://cakebuild.net) as its build orchestrator:

```text
FetchDependencies → ExtractAars → BuildNutrient → [NuGet]
```

- **FetchDependencies**: Downloads the Nutrient AAR and runtime JARs from Maven (check `build.cake` for the current dependency list).
- **ExtractAars**: Unzips AARs to extract `classes.jar` for binding.
- **BuildNutrient**: Runs `dotnet build` on the binding project to produce the DLL.
- **NuGet**: Packs the DLL + JARs + targets into a `.nupkg`.
- **Clean**: Removes all build artifacts, Jars/, bin/, obj/.

### Key Build Commands

```bash
# Full build (download + compile)
./build.sh --nutrient-version=<VERSION>

# Clean
dotnet cake --target=Clean

# Build NuGet package
./build.sh --nutrient-version=<VERSION>
dotnet cake --target=NuGet --nutrient-version=<VERSION>
```

### Namespace Mapping

Java packages are mapped to .NET namespaces in `Transforms/Metadata.xml`. Read this file to discover the current mappings. Internal APIs are removed from the binding surface.

### Additions Layer

The `Additions/` directory contains hand-written C# extensions that supplement the auto-generated binding (e.g., SDK initialization wrappers, constructor fixes, enhanced APIs). List the directory contents to discover the current files and their purposes.

### MSBuild Workarounds

`Directory.Build.targets` fixes Kotlin Multiplatform duplicate class conflicts (e.g., Compose Runtime Annotation JVM vs Android variants). This file is also packed into the NuGet as `build/Nutrient.dotnet.Android.targets` so the fix applies to NuGet consumers too.

### Sample Project

The `samples/` directory contains sample app(s) demonstrating SDK initialization, PDF viewing, and document loading. Read the sample code to understand current usage patterns.

The sample uses `<ProjectReference>` to the binding project. For NuGet-based integration, replace with a `<PackageReference>` to the published NuGet package.

### Version Management

- `VERSION` file contains the Nutrient Android SDK version.
- `Directory.Build.props` stores `<PSVersion>` shared across projects.
- All version references must stay in sync — read `VERSION` and `Directory.Build.props` to check the current state.

### Build Quality Gates

`scripts/check-critical-binding-warnings.sh` runs post-build and fails on:

- `BG8A04`: Metadata.xml entries that match no nodes (stale rules)
- `BG8402`: Duplicate field binding conflicts
