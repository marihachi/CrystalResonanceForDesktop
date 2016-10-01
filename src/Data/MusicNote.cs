using CrystalResonanceDesktop.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalResonanceDesktop.Data
{
	/// <summary>
	/// ノートを表します
	/// </summary>
	public class MusicNote
	{
		/// <summary>
		/// ノートの種類を取得します
		/// </summary>
		public MusicNoteType Type { get; private set; }

		/// <summary>
		/// 押されたかどうかの状態を取得または設定します
		/// </summary>
		public bool PushState { get; set; }
	}
}
