using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using YoutubeExtractor;

namespace CrystalResonanceDesktop.Utility
{
	public class YoutubeOggExtractor
	{
		public YoutubeOggExtractor() { }

		public YoutubeOggExtractor(string ffmpegFilePath)
		{
			FFmpegFilePath = ffmpegFilePath;
		}

		private Random Random { get; set; } = new Random();
		public string FFmpegFilePath { get; private set; } = @".\ffmpeg.exe";

		/// <summary>
		/// Youtubeの動画をogg形式の音声ファイルとして抽出します
		/// </summary>
		/// <param name="watchPageUrl">視聴ページのURL</param>
		/// <returns>一時フォルダ上のファイルパス</returns>
		public async Task<string> Extract(Uri watchPageUrl)
		{
			if (watchPageUrl == null)
				throw new ArgumentNullException("watchPageUrl");

			var videoInfo = await Task.Run(() => {
				var infos = DownloadUrlResolver.GetDownloadUrls(watchPageUrl.AbsoluteUri);
				var info = (from i in infos where i.AudioType == AudioType.Vorbis orderby i.AudioBitrate descending select i).First();

				if (info.RequiresDecryption)
					DownloadUrlResolver.DecryptDownloadUrl(info);

				return info;
			});

			var unixtime = (int)(DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

			var outputFilePath = $"{Path.GetTempPath()}{unixtime}_{Random.Next(1000, 10000)}{videoInfo.AudioExtension}";

			await Task.Run(() => {
				var psi = new ProcessStartInfo(FFmpegFilePath, $"-y -i {videoInfo.DownloadUrl} -acodec copy -map 0:1 {outputFilePath}");
				psi.WindowStyle = ProcessWindowStyle.Hidden;

				using (var ffmpeg = Process.Start(psi))
				{
					while (!ffmpeg.HasExited) ;
				}
			});

			return outputFilePath;
		}
	}
}
