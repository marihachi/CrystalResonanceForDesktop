using Codeplex.Data;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CrystalResonanceDesktop.Utility
{
	public class SoundCloudOggExtractor : OggExtractor
	{
		public SoundCloudOggExtractor() : base() { }
		public SoundCloudOggExtractor(string ffmpegFilePath) : base(ffmpegFilePath) { }

		/// <summary>
		/// SoundCloudのトラックをogg形式の音声ファイルとして抽出します
		/// </summary>
		/// <param name="watchPageUrl">視聴ページのURL</param>
		/// <returns>一時フォルダ上のファイルパス</returns>
		public override async Task<string> Extract(Uri listenPageUrl)
		{
			if (listenPageUrl == null)
				throw new ArgumentNullException("listenPageUrl");

			var clientId = "376f225bf427445fc4bfb6b99b72e0bf";
			// var clientId2 = "06e3e204bdd61a0a41d8eaab895cb7df";
			// var clientId3 = "02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea";

			var resolveUrl = "http://api.soundcloud.com/resolve.json?url={0}&client_id={1}";
			var trackStreamUrl = "http://api.soundcloud.com/i1/tracks/{0}/streams?client_id={1}";

			string downloadUrl;

			using (var client = new HttpClient())
			{
				// get track id
				var res = await client.GetAsync(string.Format(resolveUrl, listenPageUrl, clientId));
				var resolveJsonString = await res.Content.ReadAsStringAsync();
				var resolveJson = DynamicJson.Parse(resolveJsonString);

				var trackId = (int)resolveJson.id;

				// get download url
				res = await client.GetAsync(string.Format(trackStreamUrl, trackId, clientId));
				var trackStreamJsonString = await res.Content.ReadAsStringAsync();
				var trackStreamJson = DynamicJson.Parse(trackStreamJsonString);

				downloadUrl = trackStreamJson.http_mp3_128_url;

				return await Convert(downloadUrl, 0, false);
			}
		}
	}
}
