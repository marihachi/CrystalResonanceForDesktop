using CrystalResonanceDesktop.Utility;
using DxLibDLL;
using DxSharp;
using DxSharp.Data;
using DxSharp.Storage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
		public async Task LoadScoreAsync()
		{
			var extractor = new YoutubeOggExtractor();
			var filepath = await extractor.Extract(new Uri("https://www.youtube.com/watch?v=qo1v9oiMolM"));
			var sound = new Sound(filepath);
			File.Delete(filepath);

			// 仮
			var lane_sim = new MusicLane();
			var bar_sim = new MusicBar(8, 1, new List<MusicNote> { new MusicNote(1) });
			foreach (int i in Enumerable.Range(0, 256))
				lane_sim.Bars.Add(bar_sim);

			var lane_hi = new MusicLane();
			var bar_hi = new MusicBar(8, 1, new List<MusicNote> { new MusicNote(2), new MusicNote(4), new MusicNote(5), new MusicNote(6), new MusicNote(8) });
			foreach (int i in Enumerable.Range(0, 256))
				lane_hi.Bars.Add(bar_hi);

			var lane_kick = new MusicLane();
			var bar_kick = new MusicBar(8, 1, new List<MusicNote> { new MusicNote(1), new MusicNote(5) });
			foreach (int i in Enumerable.Range(0, 256))
				lane_kick.Bars.Add(bar_kick);

			var lane_sn = new MusicLane();
			var bar_sn = new MusicBar(8, 1, new List<MusicNote> { new MusicNote(3), new MusicNote(7) });
			foreach (int i in Enumerable.Range(0, 256))
				lane_sn.Bars.Add(bar_sn);

			Score = new MusicScore("ウラココロ", 120, sound, -500 * 5 / 16, new List<MusicLane> { lane_sn, lane_kick, lane_hi, lane_sim });

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
			var noteImage = ImageStorage.Instance.Item("note");

			foreach (var laneIndex in Enumerable.Range(0, Score.Lanes.Count))
			{
				var status = LaneStatuses[laneIndex];

				// 表示する小節の範囲
				foreach (int i in Enumerable.Range(-1, 4))
				{
					var targetBarIndex = status.BarIndex + i;

					// 小節のインデックスがマイナスなら処理しない
					if (targetBarIndex > 0)
					{
						var bar = status.TargetLane.Bars[targetBarIndex];
						var barDisplaySize = NoteSpeedBase * 4 * bar.Span;
						var laneX = 280 + 144 * (laneIndex + 1);
						var rightBottom = new Point(SystemCore.Instance.WindowSize.Width, SystemCore.Instance.WindowSize.Height);
						var center = new Point(rightBottom.X / 2, rightBottom.Y / 2);

						DX.DrawLine(0, 650, SystemCore.Instance.WindowSize.Width, 650, 0xffffff);

						foreach (var noteIndex in Enumerable.Range(0, bar.Notes.Count))
						{
							var location = barDisplaySize * bar.Notes[noteIndex].LocationCount / bar.Count - barDisplaySize * status.BarOffset + (i) * barDisplaySize;

							noteImage.Draw(new Point(laneX - noteImage.Size.Width / 2, (int)(650 - location - noteImage.Size.Height / 2.0)));
						}
					}
				}

				if (SystemCore.Instance.IsShowDebugImageBorder)
				{
					font.Draw(new Point(180 * laneIndex, 20 * 1), $"Score.Lanes[{laneIndex}]", Color.White);
					font.Draw(new Point(180 * laneIndex, 20 * 2), $"beatLocation: {status.BeatLocation:00.0}", Color.White);
					font.Draw(new Point(180 * laneIndex, 20 * 3), $"barIndex: {status.BarIndex}", Color.White);
					font.Draw(new Point(180 * laneIndex, 20 * 4), $"BarOffset: {status.BarOffset:00.0}", Color.White);
					font.Draw(new Point(180 * laneIndex, 20 * 5), $"barBeatLocation: {status.BarBeatLocation:00.0}", Color.White);
					font.Draw(new Point(180 * laneIndex, 20 * 6), $"countLocation: {status.CountLocation:00.0}", Color.White);
				}
			}
		}

	}
}
