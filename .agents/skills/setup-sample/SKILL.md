---
name: setup-sample
description: Help users set up and run the AndroidSample project. Use when asked to run the sample, set up the project, or get started with the Nutrient .NET for Android SDK.
---

# Set Up and Run the Android Sample Project

Guides the user through setting up and running the `AndroidSample` project from this repository.

## Important Rules

- **Always check prerequisites first** before attempting to build.
- **Prefer the NuGet approach** unless the user explicitly wants to build from source.
- **Stop on errors** and help the user diagnose before proceeding.
- **Do not modify the sample project** with workarounds. If something fails, diagnose the root cause.

## Workflow

### Step 1: Check Prerequisites

Verify the development environment is ready:

```bash
# Check .NET SDK is installed
dotnet --version

# Check Android workload is installed
dotnet workload list | grep android

# Check Java SDK
java -version
```

**Required:**
- .NET SDK 9.0+ (with .NET for Android workload)
- Microsoft Mobile OpenJDK 11.0+
- Android SDK with API level 23+

If the Android workload is missing:
```bash
dotnet workload install android
```

If the user wants to run on a device/emulator, verify ADB is available:
```bash
adb devices
```

### Step 2: Choose Integration Path

Ask the user which approach they prefer:

**Option A: NuGet Package (Recommended)** - Easiest setup, uses pre-built packages from nuget.org.

**Option B: Build from Source (Advanced)** - Builds the binding project locally, then builds the sample against it.

### Step 3A: NuGet Approach

1. Read the current version from `VERSION` file.

2. Modify the sample project to use NuGet instead of ProjectReference. In `samples/AndroidSample/AndroidSample.csproj`, replace:
   ```xml
   <ProjectReference Include="..\..\Nutrient.dotnet.Android\Nutrient.dotnet.Android.csproj" />
   ```
   with:
   ```xml
   <PackageReference Include="Nutrient.dotnet.Android" Version="VERSION_FROM_FILE" />
   ```

3. Restore and build:
   ```bash
   cd samples/AndroidSample
   dotnet restore
   dotnet build
   ```

4. To run on an emulator or connected device:
   ```bash
   dotnet build -t:Install
   ```

5. **Remind the user** to revert the `.csproj` change before committing:
   ```bash
   git checkout -- samples/AndroidSample/AndroidSample.csproj
   ```

### Step 3B: Build from Source Approach

1. Read the current version from `VERSION` file.

2. Build the binding project from the repository root:
   ```bash
   ./build.sh --nutrient-version=VERSION_FROM_FILE
   ```
   This downloads the Nutrient Android SDK AAR and all dependencies, then builds the C# binding DLL. Allow up to 10 minutes.

3. Build the sample (it already uses `<ProjectReference>`):
   ```bash
   cd samples/AndroidSample
   dotnet build
   ```

4. To run on an emulator or connected device:
   ```bash
   dotnet build -t:Install
   ```

### Step 4: Configure License Key

The sample runs in trial mode by default (`licenseKey: null` in `MainActivity.cs`). If the user has a license key:

1. Open `samples/AndroidSample/MainActivity.cs`.
2. Find the line:
   ```csharp
   NutrientGlobal.Initialize(this, licenseKey: null);
   ```
3. Replace `null` with the license key string:
   ```csharp
   NutrientGlobal.Initialize(this, licenseKey: "YOUR_LICENSE_KEY");
   ```

License keys are available from https://my.nutrient.io/.

### Step 5: Verify the App Runs

After installing on a device or emulator:

1. Launch the app manually or check it's running:
   ```bash
   adb shell am start -n io.nutrient.AndroidSample/io.nutrient.AndroidSample.MainActivity
   ```

2. The app should show three buttons:
   - **Open Demo Document** - Opens a bundled PDF using PdfActivity
   - **Open Demo Document (Fragment)** - Opens using PdfFragment
   - **Open Document** - Opens a document picker

3. If the app crashes, check logs:
   ```bash
   adb logcat -d | grep -iE "AndroidRuntime|MonoDroid|UNHANDLED" | tail -20
   ```

## Troubleshooting

### Build fails with missing Android workload
```bash
dotnet workload install android
```

### "R.java has multiple classes with same name"
Clean all build artifacts:
```bash
cd samples/AndroidSample
rm -rf bin obj
dotnet build
```

### App crashes with ClassNotFoundException
A runtime dependency may be missing. Check logcat:
```bash
adb logcat -d | grep -iE "ClassNotFoundException|NoClassDefFound"
```

### NuGet package not found
Ensure nuget.org is configured as a package source:
```bash
dotnet nuget list source
```

## Key Files

| File | Purpose |
|------|---------|
| `VERSION` | Current Nutrient Android SDK version |
| `samples/AndroidSample/AndroidSample.csproj` | Sample project file |
| `samples/AndroidSample/MainActivity.cs` | Main activity with license key setup |
| `samples/AndroidSample/Assets/demo.pdf` | Bundled demo PDF document |
| `README.md` | Full integration documentation |
