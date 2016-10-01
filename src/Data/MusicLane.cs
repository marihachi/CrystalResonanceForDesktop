using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalResonanceDesktop.Data
{
	public class MusicLane
	{
		/// <summary>
		/// このレーンに属している小節を取得または設定します
		/// </summary>
		public List<MusicBar> Bars { get; set; }
	}
}
