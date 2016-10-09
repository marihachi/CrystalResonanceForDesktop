using DxLibDLL;
using DxSharp.Data;
using DxSharp.Storage;
using System;
using System.Drawing;
using System.Linq;

namespace CrystalResonanceDesktop.Data
{
	public class MusicManager
	{
		public MusicManager(MusicScore score)
		{
			Score = score;
		}

		/// <summary>
		/// 対象の譜面情報
		/// </summary>
		public MusicScore Score { get; set; }

		/// <summary>
		/// 
		/// </summary>
		private double BarDisplaySize { get; set; }

		/// <summary>
		/// 
		/// </summary>
		private int NoteSpeedBase { get; set; } = 200;

		/// <summary>
		/// 
		/// </summary>
		public void LoadScore()
		{

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
			var font = FontStorage.Instance.Item("メイリオ16");

			// 1秒に何回か
			var bps = Score.BPM / 60.0;

			// 現在の再生時間[秒]
			var songSec = (Score.Song.CurrentTime + Score.Offset) / 1000.0;

			// 再生時間がマイナスなら処理しない
			if (songSec > 0)
			{
				// 最初から現在の再生位置までの拍数
				var beatLocation = songSec * bps;
				var beatIndex = (int)beatLocation;
				var beatOffset = beatLocation - beatIndex;

				foreach (var laneIndex in Enumerable.Range(0, Score.Lanes.Count))
				{
					var lane = Score.Lanes[laneIndex];

					// 現在の小節インデックスを求める
					var spanSum = 0.0;
					foreach (var i in Enumerable.Range(0, lane.Bars.Count))
						spanSum += lane.Bars[i].Span;
					lane.BarIndex = (int)(lane.Bars.Count * beatIndex / (4 * spanSum));

					var barBeatLocation = (beatLocation % (4 * lane.NowBar.Span));
					var barBeatIndex = (int)barBeatLocation;
					var barBeatOffset = barBeatLocation - barBeatIndex;
					var countLocation = beatLocation * lane.NowBar.Count % (lane.NowBar.Count * lane.NowBar.Span);

					font.Draw(new Point(10 + 180 * laneIndex, 30 * 4), $"Score.Lanes[{laneIndex}]", Color.White);
					font.Draw(new Point(10 + 180 * laneIndex, 30 * 6), $"beatLocation: {beatLocation:F1}", Color.White);
					font.Draw(new Point(10 + 180 * laneIndex, 30 * 7), $"beatIndex: {beatIndex}", Color.White);
					font.Draw(new Point(10 + 180 * laneIndex, 30 * 8), $"beatOffset: {beatOffset:F2}", Color.White);
					font.Draw(new Point(10 + 180 * laneIndex, 30 * 10), $"barIndex: {lane.BarIndex}", Color.White);
					font.Draw(new Point(10 + 180 * laneIndex, 30 * 12), $"barBeatLocation: {barBeatLocation:F1}", Color.White);
					font.Draw(new Point(10 + 180 * laneIndex, 30 * 13), $"barBeatIndex: {barBeatIndex}", Color.White);
					font.Draw(new Point(10 + 180 * laneIndex, 30 * 14), $"barBeatOffset: {barBeatOffset:F1}", Color.White);
					font.Draw(new Point(10 + 180 * laneIndex, 30 * 16), $"countLocation: {countLocation:F1}", Color.White);
				}
			}
		}

		/// <summary>
		/// 描画します
		/// </summary>
		public void Draw()
		{
			foreach (var laneIndex in Enumerable.Range(0, Score.Lanes.Count))
			{
				var bar = Score.Lanes[laneIndex].NowBar;
				var noteDisplaySize = BarDisplaySize / bar.Notes.Count;

				var laneX = 280 + 144 * (laneIndex + 1);

				DX.DrawLine(laneX, 0, laneX, 720, 0xffffff);

				foreach (var noteIndex in Enumerable.Range(0, bar.Notes.Count))
				{
					var note = bar.Notes[noteIndex];

					if (note.Type == Enum.MusicNoteType.Normal)
						ImageStorage.Instance.Item("note").Draw(new Point(laneX - ImageStorage.Instance.Item("note").Size.Width / 2, (int)(650 - noteDisplaySize * noteIndex))); // ノート
				}
			}
		}
	}
}
