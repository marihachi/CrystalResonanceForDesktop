using System.Collections.Generic;
using System.Linq;

namespace CrystalResonanceDesktop.Data
{
	public class MusicLane
	{
		public MusicLane(IEnumerable<MusicBar> bars = null)
		{
			Bars = bars?.ToList() ?? new List<MusicBar>();
		}

		/// <summary>
		/// このレーンに属している小節の一覧を取得します
		/// </summary>
		public List<MusicBar> Bars { get; }
	}
}
