using DxLibDLL;
using DxSharp;
using DxSharp.Data;
using DxSharp.Data.Enum;
using DxSharp.Storage;
using DxSharp.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
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
			if (ImageStorage.Instance.Item("note") == null)
				ImageStorage.Instance.Add("note", new DxSharp.Data.Image("Resource/note.png", 100, Position.LeftTop));

			var effect = new DxSharp.Data.Image("Resource/detectFrameEffect.png", 0, Position.LeftTop);
			if (ImageStorage.Instance.Item("detectFrameEffect1") == null)
				ImageStorage.Instance.Add("detectFrameEffect1", effect);
			foreach (var i in Enumerable.Range(2, 5))
				if (ImageStorage.Instance.Item($"detectFrameEffect{i}") == null)
					ImageStorage.Instance.Add($"detectFrameEffect{i}", (DxSharp.Data.Image)effect.Clone());
		}

		/// <summary>
		/// 対象の譜面情報
		/// </summary>
		public MusicScore Score { get; set; }

		/// <summary>
		/// 
		/// </summary>
		private int NoteSpeedBase { get; set; } = 350;

		/// <summary>
		/// 譜面の状態を取得します
		/// </summary>
		private ScoreStatus ScoreStatus { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public async Task LoadScoreAsync()
		{
			// 仮
			var lane_sim = new MusicLane(new List<MusicNote> { new MusicNote(1) });
			var lane_hi = new MusicLane(new List<MusicNote> { new MusicNote(2), new MusicNote(3), new MusicNote(4), new MusicNote(5), new MusicNote(6), new MusicNote(7), new MusicNote(8) });
			var lane_kick = new MusicLane(new List<MusicNote> { new MusicNote(1), new MusicNote(5) });
			var lane_sn = new MusicLane(new List<MusicNote> { new MusicNote(3), new MusicNote(7) });

			var bar = new MusicBar(8, 1, new List<MusicLane> { lane_sn, lane_kick, lane_hi, lane_sim });

			Score = new MusicScore("ウラココロ", 120, new Uri("https://www.youtube.com/watch?v=qo1v9oiMolM"), -500 * 5 / 16);
			await Score.ExtractSong();

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
			var core = SystemCore.Instance;
			var input = Input.Instance;
			var image = ImageStorage.Instance;

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

			foreach (var i in Enumerable.Range(1, 4))
				image.Item($"detectFrameEffect{i}").Update();

			if (core.IsShowDebugImageBorder)
			{
				if (input.GetKey(KeyType.Up).InputTime == 1)
					NoteSpeedBase += 50;

				if (input.GetKey(KeyType.Down).InputTime == 1 && NoteSpeedBase > 100)
					NoteSpeedBase -= 50;
			}

			Judgement();
		}

		/// <summary>
		/// 入力の判定をします
		/// </summary>
		private void Judgement()
		{
			var input = Input.Instance;

			// TODO
		}

		/// <summary>
		/// ノートを描画します
		/// </summary>
		public void Draw()
		{
			var fonts = FontStorage.Instance;
			var input = Input.Instance;
			var images = ImageStorage.Instance;

			if (Score != null && ScoreStatus != null)
			{
				// 表示する小節の範囲
				foreach (int i in Enumerable.Range(-1, 3))
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
							var laneX = 200 + 100 + 176 * (laneIndex + 1);

							foreach (var noteIndex in Enumerable.Range(0, targetLane.Notes.Count))
							{
								var count = (double)targetLane.Notes[noteIndex].LocationCount / bar.Count;
								var location = barDisplaySize * (count - ScoreStatus.BarOffset + i);

								images.Item("note").Draw(new Point((int)(laneX - images.Item("note").Size.Width / 2.0), (int)(650 - location - images.Item("note").Size.Height / 2.0)));
							}
						}
					}
				}

				foreach (var laneIndex in Enumerable.Range(0, Score.Bars[ScoreStatus.BarIndex].Lanes.Count))
				{
					var laneX = 200 + 100 + 176 * (laneIndex + 1);

					var effect = images.Item($"detectFrameEffect{laneIndex + 1}");

					KeyType key = 0;
					if (laneIndex == 0) key = KeyType.D;
					if (laneIndex == 1) key = KeyType.F;
					if (laneIndex == 2) key = KeyType.J;
					if (laneIndex == 3) key = KeyType.K;


					if (input.GetKey(key).InputTime == 1)
						effect.Fade(100, .01);
					else if (input.GetKey(key).InputTime == 0)
						effect.Fade(0, .16);

					effect.Draw(new Point(laneX - effect.Size.Width / 2, 650 - effect.Size.Height / 2));
				}
			}

			DX.DrawLine(0, 650, SystemCore.Instance.WindowSize.Width, 650, 0xffffff);

			if (SystemCore.Instance.IsShowDebugImageBorder)
			{
				var font = fonts.Item("メイリオ16");

				if (ScoreStatus != null)
				{
					font.Draw(new Point(5, 20 * 1), $"beatLocation: {ScoreStatus.BeatLocation:00.0}", Color.White);
					font.Draw(new Point(5, 20 * 2), $"beatIndex: {ScoreStatus.BeatIndex}", Color.White);
					font.Draw(new Point(5, 20 * 3), $"beatOffset: {ScoreStatus.BeatOffset:00.0}", Color.White);
					font.Draw(new Point(5, 20 * 4), $"barLocation: {ScoreStatus.BarLocation:00.0}", Color.White);
					font.Draw(new Point(5, 20 * 5), $"barIndex: {ScoreStatus.BarIndex}", Color.White);
					font.Draw(new Point(5, 20 * 6), $"barOffset: {ScoreStatus.BarOffset:00.0}", Color.White);
					font.Draw(new Point(5, 20 * 7), $"barBeatLocation1: {ScoreStatus.BarBeatLocation:00.0}", Color.White);
					font.Draw(new Point(5, 20 * 8), $"countLocation: {ScoreStatus.CountLocation:00.0}", Color.White);
				}
				font.Draw(new Point(5, 20 * 9), $"NoteSpeedBase: {NoteSpeedBase}", Color.White);
			}
		}
	}
}
