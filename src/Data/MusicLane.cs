﻿using System.Collections.Generic;
using System.Linq;

namespace CrystalResonanceDesktop.Data
{
	/// <summary>
	/// レーンを表します
	/// </summary>
	public class MusicLane
	{
		public MusicLane(MusicBar parentBar, IEnumerable<MusicNote> notes = null)
		{
			ParentBar = parentBar;
			Notes = notes?.ToList() ?? new List<MusicNote>();
		}

		/// <summary>
		/// このレーンに属している小節の一覧を取得します
		/// </summary>
		public List<MusicNote> Notes { get; }

		/// <summary>
		/// 
		/// </summary>
		public MusicBar ParentBar { get; set; }
	}
}
