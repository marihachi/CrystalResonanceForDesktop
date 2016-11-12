using System.Collections.Generic;
using System.Linq;

namespace CrystalResonanceDesktop.Data
{
	/// <summary>
	/// 小節を表します
	/// </summary>
	public class MusicBar
	{
		public MusicBar(MusicScore parentScore, uint count = 4, double span = 1.0, IEnumerable<MusicLane> lanes = null)
		{
			ParentScore = parentScore;
			Count = count;
			Span = span;
			Lanes = lanes?.ToList() ?? new List<MusicLane>();
		}

		public MusicScore ParentScore { get; set; }

		/// <summary>
		/// この小節に属しているノートの一覧を取得します
		/// </summary>
		public List<MusicLane> Lanes { get; private set; }

		/// <summary>
		/// この小節の1から始まるカウント数を取得または設定します
		/// </summary>
		public uint Count { get; set; }

		/// <summary>
		/// この小節の4/4拍子を基準とした長さの倍率を取得または設定します
		/// </summary>
		/// <example>4/4拍子なら1</example>
		/// <example>3/4拍子なら0.75</example>
		public double Span { get; set; }
	}
}
