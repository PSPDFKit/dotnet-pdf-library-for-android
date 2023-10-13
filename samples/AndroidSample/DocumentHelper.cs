using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;

namespace SampleTools {
	public static partial class Utils {
		public static readonly int BufferSize = 8192;

		public static Android.Net.Uri ExtractAsset (Context ctx, string assetName)
		{
			var docPath = Path.Combine (ctx.GetExternalFilesDir (null)!.AbsolutePath, assetName);
			if (!File.Exists (docPath)) {
				using (var br = new BinaryReader (ctx.Assets!.Open (assetName))) {
					using (var bw = new BinaryWriter (new FileStream (docPath, FileMode.Create))) {
						var buffer = new byte[2048];
						var len = 0;
						while ((len = br.Read (buffer, 0, buffer.Length)) > 0) {
							bw.Write (buffer, 0, len);
						}
					}
				}
			}
			var file = new Java.IO.File (docPath);
			return Android.Net.Uri.FromFile (file)!;
		}

		public static async Task<Android.Net.Uri> DownloadDocument (Context ctx, Android.Net.Uri uriToDownload, string saveFilePath, IProgress<DownloadBytesProgress> progessReporter)
		{
			int receivedBytes = 0;
			int totalBytes = 0;

			using (var assetFileDescriptor = ctx.ContentResolver!.OpenAssetFileDescriptor (uriToDownload, "r"))
			using (var fileOutputStream = new Java.IO.FileOutputStream (saveFilePath))
			using (var inputStream = assetFileDescriptor!.CreateInputStream ()) {
				var buffer = new byte [BufferSize];
				totalBytes = (int)assetFileDescriptor.Length;

				for (;;) {
					var bytesRead = await inputStream!.ReadAsync (buffer, 0, buffer.Length);
					if (bytesRead == 0) {
						await Task.Yield();
						break;
					}
					await fileOutputStream.WriteAsync (buffer, 0, buffer.Length);

					receivedBytes += bytesRead;
					if (progessReporter != null) {
						var args = new DownloadBytesProgress (uriToDownload.ToString ()!, saveFilePath, receivedBytes, totalBytes);
						progessReporter.Report(args);
					}
				}
				inputStream.Close ();
				fileOutputStream.Close ();
			}
			var file = new Java.IO.File (saveFilePath);
			var docUri = Android.Net.Uri.FromFile (file);
			return docUri!;
		}

		public static string ToTitleCase (this string s, bool replaceUnderscore = false, string removeStr = null)
		{
			var ret = s;

			if (removeStr != null)
				ret = ret.Replace (removeStr, string.Empty);

			if (replaceUnderscore)
				ret = ret.Replace ("_", " ");

			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase (ret.ToLower ());
		}

		public static bool RequestExternalStorageRwPermission (Activity activity, int requestCode)
		{
			// On Android 6.0+ we ask for SD card access permission.
			// Since documents can be annotated we ask for write permission as well.
			if (Build.VERSION.SdkInt >= BuildVersionCodes.M) {
				if (ContextCompat.CheckSelfPermission (activity, Manifest.Permission.ReadExternalStorage)
						!= Permission.Granted) {
					ActivityCompat.RequestPermissions (
							activity,
							new [] {
								Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage
							},
							requestCode);
					return false;
				}
			}
			return true;
		}
	}

	public class DownloadBytesProgress 
	{		
		public DownloadBytesProgress (string url, string filePath, int bytesReceived, int totalBytes)
		{
			DownloadUrl = url;
			BytesReceived = bytesReceived;
			TotalBytes = totalBytes;
			SaveFilePath = filePath;
		}

		public int TotalBytes { get; private set; }

		public int BytesReceived { get; private set; }

		public float PercentComplete { get { return (float)BytesReceived / TotalBytes; } }

		public string DownloadUrl { get; private set; }

		public string SaveFilePath { get; private set; }

		public bool IsFinished { get { return BytesReceived == TotalBytes; } }
	}
}

