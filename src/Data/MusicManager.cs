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
		/// 譜面の状態を取得します
		/// </summary>
		private ScoreStatus ScoreStatus { get; set; }

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
			var lane_sim = new MusicLane(new List<MusicNote> { new MusicNote(1) });
			var lane_hi = new MusicLane(new List<MusicNote> { new MusicNote(2), new MusicNote(3), new MusicNote(4), new MusicNote(5), new MusicNote(6), new MusicNote(7), new MusicNote(8) });
			var lane_kick = new MusicLane(new List<MusicNote> { new MusicNote(1), new MusicNote(5) });
			var lane_sn = new MusicLane(new List<MusicNote> { new MusicNote(3), new MusicNote(7) });

			var bar = new MusicBar(8, 1, new List<MusicLane> { lane_sn, lane_kick, lane_hi, lane_sim });

			Score = new MusicScore("ウラココロ", 120, sound, -500 * 5 / 16);

			foreach (int i in Enumerable.Range(0, 256))
				Score.Bars.Add(bar);

			// 譜面のステータスを作成
			ScoreStatus = new ScoreStatus(0, Score);
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
				// 最初から現在の再生位置までの拍数
				var beatLocation = songSec * bps;

				ScoreStatus.SetBeatLocation(beatLocation);
			}
		}

		/// <summary>
		/// 描画します
		/// </summary>
		public void Draw()
		{
			var font = FontStorage.Instance.Item("メイリオ16");
			var noteImage = ImageStorage.Instance.Item("note");

			// 表示する小節の範囲
			foreach (int i in Enumerable.Range(-1, 4))
			{
				var targetBarIndex = ScoreStatus.BarIndex + i;

				// 小節のインデックスがマイナスなら処理しない
				if (targetBarIndex > 0)
				{
					foreach (var laneIndex in Enumerable.Range(0, Score.Bars[targetBarIndex].Lanes.Count))
					{
						var targetLane = Score.Bars[targetBarIndex].Lanes[laneIndex];
						var bar = ScoreStatus.TargetScore.Bars[targetBarIndex];
						var barDisplaySize = NoteSpeedBase * 4 * bar.Span;
						var laneX = 280 + 144 * (laneIndex + 1);
						// var rightBottom = new Point(SystemCore.Instance.WindowSize.Width, SystemCore.Instance.WindowSize.Height);
						// var center = new Point(rightBottom.X / 2, rightBottom.Y / 2);

						DX.DrawLine(0, 650, SystemCore.Instance.WindowSize.Width, 650, 0xffffff);

						foreach (var noteIndex in Enumerable.Range(0, targetLane.Notes.Count))
						{
							var location = barDisplaySize * targetLane.Notes[noteIndex].LocationCount / bar.Count - barDisplaySize * ScoreStatus.BarOffset + (i) * barDisplaySize;

							noteImage.Draw(new Point(laneX - noteImage.Size.Width / 2, (int)(650 - location - noteImage.Size.Height / 2.0)));
						}
					}
				}
			}

			if (SystemCore.Instance.IsShowDebugImageBorder)
			{
				font.Draw(new Point(5, 20 * 1), $"beatLocation: {ScoreStatus.BeatLocation:00.0}", Color.White);
				font.Draw(new Point(5, 20 * 2), $"barIndex: {ScoreStatus.BarIndex}", Color.White);
				font.Draw(new Point(5, 20 * 3), $"BarOffset: {ScoreStatus.BarOffset:00.0}", Color.White);
				font.Draw(new Point(5, 20 * 4), $"barBeatLocation: {ScoreStatus.BarBeatLocation:00.0}", Color.White);
				font.Draw(new Point(5, 20 * 5), $"countLocation: {ScoreStatus.CountLocation:00.0}", Color.White);
			}
		}
	}
}
