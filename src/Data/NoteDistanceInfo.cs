namespace CrystalResonanceDesktop.Data
{
	/// <summary>
	/// 判定ラインからのノートの距離情報を表します
	/// </summary>
	public class NoteDistanceInfo
	{
		public NoteDistanceInfo(MusicScore score, int laneIndex, int barIndex, int noteIndex, int noteDistance)
		{
			LaneIndex = laneIndex;
			BarIndex = barIndex;
			NoteIndex = noteIndex;
			NoteDistance = noteDistance;
			Score = score;
		}

		/// <summary>
		/// レーンの0から始まるインデックス
		/// </summary>
		public int LaneIndex { get; set; }

		/// <summary>
		/// 小節の0から始まるインデックス
		/// </summary>
		public int BarIndex { get; set; }

		/// <summary>
		/// 小節内でのノートの0から始まるインデックス
		/// </summary>
		public int NoteIndex { get; set; }

		/// <summary>
		/// 現在位置からの距離(x1000)
		/// </summary>
		public int NoteDistance { get; set; }

		public MusicScore Score { get; set; }

		public MusicNote TargetNote { get { return Score.Bars[BarIndex].Lanes[LaneIndex].Notes[NoteIndex]; } }
	}
}
