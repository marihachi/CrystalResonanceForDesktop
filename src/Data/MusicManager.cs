using DxLibDLL;
using DxSharp;
using DxSharp.Data;
using DxSharp.Storage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CrystalResonanceDesktop.Data
{
	/// <summary>
	/// 
	/// </summary>
	public class MusicManager
	{
		public MusicManager()
		{

		}

		/// <summary>
		/// 対象の譜面情報
		/// </summary>
		public MusicScore Score { get; set; }

		/// <summary>
		/// 
		/// </summary>
		private int NoteSpeedBase { get; set; } = 200;

		/// <summary>
		/// レーン毎の状態の一覧を取得します
		/// </summary>
		private List<LaneStatus> LaneStatuses { get; } = new List<LaneStatus>();

		/// <summary>
		/// 
		/// </summary>
		public void LoadScore()
		{
			// 仮
			var bar = new MusicBar(4, 1, new List<MusicNote> { new MusicNote(1), new MusicNote(3), new MusicNote(4) });
			var bar_b = new MusicBar(3, 0.75, new List<MusicNote> { new MusicNote(1), new MusicNote(3) });
			var lane = new MusicLane();
			foreach (int i in Enumerable.Range(0, 64))
				lane.Bars.Add(bar);
			Score = new MusicScore("ハナカガリ", 132, new Sound("Resource/ハナカガリ_game.ogg", 50), 0, new List<MusicLane> { lane, lane, lane, lane });

			// レーンの数のステータスを作成
			LaneStatuses.Clear();
			foreach (var i in Score.Lanes)
				LaneStatuses.Add(new LaneStatus(0, i));
		}

		/// <summary>
		/// 
		/// </summary>
		public void Start()
		{
			Score.Song.Play();
		}

		/// <summary>
		/// 更新します
		/// </summary>
		public void Update()
		{
			// 1秒に何回か
			var bps = Score.BPM / 60.0;

			// 現在の再生時間[秒]
			var songSec = (Score.Song.CurrentTime + Score.Offset) / 1000.0;

			// 再生時間がマイナスなら処理しない
			if (songSec > 0)
			{
				foreach (var laneIndex in Enumerable.Range(0, Score.Lanes.Count))
				{
					// 最初から現在の再生位置までの拍数
					var beatLocation = songSec * bps;

					LaneStatuses[laneIndex].SetBeatLocation(beatLocation);
				}
			}
		}

		/// <summary>
		/// 描画します
		/// </summary>
		public void Draw()
		{
			var font = FontStorage.Instance.Item("メイリオ16");

			foreach (var laneIndex in Enumerable.Range(0, Score.Lanes.Count))
			{
				var status = LaneStatuses[laneIndex];

				var bar = status.NowBar;
				var barDisplaySize = NoteSpeedBase * 4 * bar.Span;
				var laneX = 280 + 144 * (laneIndex + 1);
				var rightBottom = new Point(SystemCore.Instance.WindowSize.Width, SystemCore.Instance.WindowSize.Height);
				var center = new Point(rightBottom.X / 2, rightBottom.Y / 2);

				foreach (var noteIndex in Enumerable.Range(0, bar.Notes.Count))
				{
					var note = bar.Notes[noteIndex];

					// ノート
					ImageStorage.Instance.Item("note").Draw(new Point(laneX - ImageStorage.Instance.Item("note").Size.Width / 2, (int)(650 - barDisplaySize * noteIndex)));
				}

				if (SystemCore.Instance.IsShowDebugImageBorder)
				{
					font.Draw(new Point(10 + 180 * laneIndex, 30 * 4), $"Score.Lanes[{laneIndex}]", Color.White);
					font.Draw(new Point(10 + 180 * laneIndex, 30 * 5), $"beatLocation: {status.BeatLocation:00.#}", Color.White);
					font.Draw(new Point(10 + 180 * laneIndex, 30 * 6), $"barIndex: {status.BarIndex}", Color.White);
					font.Draw(new Point(10 + 180 * laneIndex, 30 * 7), $"barBeatLocation: {status.BarBeatLocation:00.#}", Color.White);
					font.Draw(new Point(10 + 180 * laneIndex, 30 * 8), $"countLocation: {status.CountLocation:00.#}", Color.White);
				}
			}
		}

	}
}
