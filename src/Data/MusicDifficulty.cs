using System.Collections.Generic;
using System.Linq;
namespace CrystalResonanceDesktop.Data
{
	/// <summary>
	/// 
	/// </summary>
	public class MusicDifficulty
	{
		public MusicDifficulty(int laneCount, IEnumerable<MusicBar> bars = null)
		{
			LaneCount = laneCount;
			Bars = bars?.ToList() ?? new List<MusicBar>();
		}

		/// <summary>
		/// このスコアに属しているレーンを取得または設定します
		/// </summary>
		public List<MusicBar> Bars { get; set; }

		/// <summary>
		/// レーン数を取得または設定します。ここに設定された値がレーン数を決定付けます。
		/// </summary>
		public int LaneCount { get; set; }
	}
}
