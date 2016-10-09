using DxSharp.Data;
using System.Collections.Generic;

namespace CrystalResonanceDesktop.Data
{
	public class MusicScore
	{
		public MusicScore(string songTitle, int bpm, double offset = 0)
		{
			SongTitle = songTitle;
			BPM = bpm;
			Offset = offset;
		}

		/// <summary>
		/// 曲名
		/// </summary>
		public string SongTitle { get; set; }

		/// <summary>
		/// 曲
		/// </summary>
		public Sound Song { get; set; }

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
		public List<MusicLane> Lanes { get; set; } = new List<MusicLane>();
	}
}
