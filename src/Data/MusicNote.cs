using CrystalResonanceDesktop.Data.Enum;

namespace CrystalResonanceDesktop.Data
{
	/// <summary>
	/// ノートを表します
	/// </summary>
	public class MusicNote
	{
		public MusicNote(uint countLocation, MusicNoteType type = MusicNoteType.Normal, bool pushState = false)
		{
			Type = type;
			CountLocation = countLocation;
			PushState = pushState;
		}

		/// <summary>
		/// ノートの種類を取得します
		/// </summary>
		public MusicNoteType Type { get; set; }

		/// <summary>
		/// カウント上の位置を取得または設定します
		/// </summary>
		public uint CountLocation { get; set; }

		/// <summary>
		/// 押されたかどうかの状態を取得または設定します
		/// </summary>
		public bool PushState { get; set; }
	}
}
