using DxSharp.Data;
using System.Collections.Generic;

namespace CrystalResonanceDesktop.Data
{
	public class MusicScore
	{
		public MusicScore(string songTitle, int bpm)
		{
			SongTitle = songTitle;
			BPM = bpm;
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
		/// このスコアに属しているレーンを取得または設定します
		/// </summary>
		public List<MusicLane> Lanes { get; set; } = new List<MusicLane>();
	}
}
