using System.Collections.Generic;

namespace CrystalResonanceDesktop.Data
{
	/// <summary>
	/// 小節を表します
	/// </summary>
	public class MusicBar
	{
		public MusicBar(uint count = 4, double span = 1.0)
		{
			Count = count;
			Span = span;
		}

		/// <summary>
		/// この小節に属しているノートの一覧を取得します
		/// </summary>
		public List<MusicNote> Notes { get; } = new List<MusicNote>();

		/// <summary>
		/// この小節の1から始まるカウント数を取得または設定します
		/// </summary>
		public uint Count { get; set; }

		/// <summary>
		/// この小節の長さの倍率を取得または設定します
		/// </summary>
		/// <example>4/4拍子なら1</example>
		/// <example>3/4拍子なら0.75</example>
		public double Span { get; set; }
	}
}
