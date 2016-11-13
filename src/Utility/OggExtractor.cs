using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace CrystalResonanceDesktop.Utility
{
	public abstract class OggExtractor
	{
		private Random Random { get; set; } = new Random();
		public string FFmpegFilePath { get; private set; } = @".\ffmpeg.exe";

		public OggExtractor() { }

		public OggExtractor(string ffmpegFilePath)
		{
			FFmpegFilePath = ffmpegFilePath;
		}

		protected async Task<string> Convert(string downloadUrl, int targetStreamNum, bool isCopy)
		{
			var unixtime = (int)(DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

			var outputFilePath = $"{Path.GetTempPath()}{unixtime}_{Random.Next(1000, 10000)}.ogg";

			await Task.Run(() =>
			{
				var psi = new ProcessStartInfo(FFmpegFilePath, $"-y -i {downloadUrl} {(isCopy ? "- acodec copy " : "")}-map 0:{targetStreamNum} {outputFilePath}");
				psi.WindowStyle = ProcessWindowStyle.Hidden;

				using (var ffmpeg = Process.Start(psi))
				{
					while (!ffmpeg.HasExited) ;
				}
			});

			return outputFilePath;
		}

		public abstract Task<string> Extract(Uri watchPageUrl);
	}
}
