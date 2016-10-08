using System.Collections.Generic;

namespace CrystalResonanceDesktop.Data
{
	public class MusicLane
	{
		/// <summary>
		/// このレーンに属している小節の一覧を取得します
		/// </summary>
		public List<MusicBar> Bars { get; } = new List<MusicBar>();
	}
}
