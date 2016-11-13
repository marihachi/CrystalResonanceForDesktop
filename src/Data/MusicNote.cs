using CrystalResonanceDesktop.Data.Enum;

namespace CrystalResonanceDesktop.Data
{
	/// <summary>
	/// ノートを表します
	/// </summary>
	public class MusicNote
	{
		public MusicNote(int countLocation, MusicLane parentLane, MusicNoteType type = MusicNoteType.Normal, bool pushState = false, NotePushRating rating = NotePushRating.None)
		{
			ParentLane = parentLane;
			Type = type;
			CountLocation = countLocation;
			PushState = pushState;
			Rating = rating;
		}

		/// <summary>
		/// 
		/// </summary>
		public MusicLane ParentLane { get; set; }

		/// <summary>
		/// ノートの種類を取得します
		/// </summary>
		public MusicNoteType Type { get; set; }

		/// <summary>
		/// カウント上の位置を取得または設定します
		/// </summary>
		public int CountLocation { get; set; }

		/// <summary>
		/// 押されたかどうかの状態を取得または設定します
		/// </summary>
		public bool PushState { get; set; }

		/// <summary>
		/// このノートへの入力に対する評価を取得または設定します
		/// </summary>
		public NotePushRating Rating { get; set; }
	}
}
