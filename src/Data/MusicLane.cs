using System.Collections.Generic;
using System.Linq;

namespace CrystalResonanceDesktop.Data
{
	public class MusicLane
	{
		public MusicLane(IEnumerable<MusicNote> notes = null)
		{
			Notes = notes?.ToList() ?? new List<MusicNote>();
		}

		/// <summary>
		/// このレーンに属している小節の一覧を取得します
		/// </summary>
		public List<MusicNote> Notes { get; }
	}
}
