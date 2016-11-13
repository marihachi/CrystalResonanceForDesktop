using System;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExtractor;

namespace CrystalResonanceDesktop.Utility
{
	public class YoutubeOggExtractor : OggExtractor
	{
		public YoutubeOggExtractor() : base() { }
		public YoutubeOggExtractor(string ffmpegFilePath) : base(ffmpegFilePath) { }

		/// <summary>
		/// Youtubeの動画をogg形式の音声ファイルとして抽出します
		/// </summary>
		/// <param name="watchPageUrl">視聴ページのURL</param>
		/// <returns>一時フォルダ上のファイルパス</returns>
		public override async Task<string> Extract(Uri watchPageUrl)
		{
			if (watchPageUrl == null)
				throw new ArgumentNullException("watchPageUrl");

			var videoInfo = await Task.Run(() =>
			{
				var infos = DownloadUrlResolver.GetDownloadUrls(watchPageUrl.AbsoluteUri);
				var info = (from i in infos where i.AudioType == AudioType.Vorbis orderby i.AudioBitrate descending select i).First();

				if (info.RequiresDecryption)
					DownloadUrlResolver.DecryptDownloadUrl(info);

				return info;
			});

			return await Convert(videoInfo.DownloadUrl, 1, true);
		}
	}
}
