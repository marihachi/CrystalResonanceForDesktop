using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalResonanceDesktop.Data
{
	/// <summary>
	/// 小節を表します
	/// </summary>
	public class MusicBar
	{
		/// <summary>
		/// この小節に属しているノートを取得または設定します
		/// </summary>
		public List<MusicNote> Notes { get; set; }

		/// <summary>
		/// この小節の拍子を取得します
		/// </summary>
		public int Measure { get { return Notes.Count; } }
	}
}
