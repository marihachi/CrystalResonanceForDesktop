using CrystalResonanceDesktop.Utility;
using DxSharp.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CrystalResonanceDesktop.Data
{
	public class MusicScore
	{
		public MusicScore(string songTitle, int bpm, Uri songUrl, int laneCount, double offset = 0, IEnumerable<MusicBar> bars = null)
		{
			SongTitle = songTitle;
			BPM = bpm;
			SongUrl = songUrl;
			LaneCount = laneCount;
			Offset = offset;
			Bars = bars?.ToList() ?? new List<MusicBar>();
		}

		/// <summary>
		/// 曲名
		/// </summary>
		public string SongTitle { get; set; }

		/// <summary>
		/// 曲のURL
		/// </summary>
		public Uri SongUrl { get; set; }

		/// <summary>
		/// 曲を取得
		/// </summary>
		public Sound Song { get; private set; }

		/// <summary>
		/// テンポ
		/// </summary>
		public int BPM { get; set; }

		/// <summary>
		/// タイミングを調整するための値を取得または設定します(ミリ秒単位)
		/// </summary>
		public double Offset { get; set; }

		/// <summary>
		/// このスコアに属しているレーンを取得または設定します
		/// </summary>
		public List<MusicBar> Bars { get; set; }

		/// <summary>
		/// レーン数
		/// </summary>
		public int LaneCount { get; set; }

		/// <summary>
		/// SongUrlから曲データを抽出します
		/// </summary>
		public async Task ExtractSong()
		{
			var extractor = new YoutubeOggExtractor();
			var filepath = await extractor.Extract(SongUrl);
			Song = new Sound(filepath);
			File.Delete(filepath);
		}
	}
}
