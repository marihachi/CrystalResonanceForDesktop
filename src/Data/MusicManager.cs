using DxLibDLL;
using DxSharp;
using DxSharp.Data;
using DxSharp.Data.Enum;
using DxSharp.Storage;
using DxSharp.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		/// 非同期で譜面データを読み込みます
		/// </summary>
		public async Task LoadScoreAsync()
		{
			// TODO: 仮
			var lane_sim = new MusicLane(new List<MusicNote> { new MusicNote(1) });
			var lane_hi = new MusicLane(new List<MusicNote> { new MusicNote(2), new MusicNote(3), new MusicNote(4), new MusicNote(5), new MusicNote(6), new MusicNote(7), new MusicNote(8) });
			var lane_kick = new MusicLane(new List<MusicNote> { new MusicNote(1), new MusicNote(5) });
			var lane_sn = new MusicLane(new List<MusicNote> { new MusicNote(3), new MusicNote(7) });

			var bar = new MusicBar(8, 1, new List<MusicLane> { lane_sn, lane_kick, lane_hi, lane_sim });

			Score = new MusicScore("ウラココロ", 120, new Uri("https://www.youtube.com/watch?v=qo1v9oiMolM"), bar.Lanes.Count, -500 * 5 / 16);
			await Score.ExtractSong();

			Score.Song.Volume = 30;

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
				if (KeyConfig.Instance.Up.InputTime == 1)
					NoteSpeedBase += 50;

				if (KeyConfig.Instance.Down.InputTime == 1 && NoteSpeedBase > 100)
					NoteSpeedBase -= 50;
			}

			Judgement();
		}

		private class NoteDistanceInfo
		{
			public NoteDistanceInfo(int laneIndex, int barIndex, int noteIndex, int noteDistance)
			{
				LaneIndex = laneIndex;
				BarIndex = barIndex;
				NoteIndex = noteIndex;
				NoteDistance = noteDistance;
			}

			// レーンのインデックス
			public int LaneIndex { get; set; }

			// 判定ラインから近いノートの小節インデックス
			public int BarIndex { get; set; }

			// 判定ラインから近いノートの小節内インデックス
			public int NoteIndex { get; set; }

			// 現在位置からの距離(x1000)
			public int NoteDistance { get; set; }
		}

		/// <summary>
		/// 現在の再生位置からの各ノートの距離を算出
		/// </summary>
		/// <returns></returns>
		private List<List<NoteDistanceInfo>> CalcNoteDistance()
		{
			var noteDistanceInfoLists = new List<List<NoteDistanceInfo>>(); // noteDistanceInfoLists[bar][lane]

			foreach (var barIndex in Enumerable.Range(ScoreStatus.BarIndex - 1, 3))
			{
				if (barIndex != -1)
				{
					var barInfos = new List<NoteDistanceInfo>();

					if (noteDistanceInfoLists.Count <= barIndex)
						noteDistanceInfoLists.Add(barInfos);

					foreach (var laneIndex in Enumerable.Range(0, Score.Bars[barIndex].Lanes.Count))
					{
						foreach (var noteIndex in Enumerable.Range(0, Score.Bars[barIndex].Lanes[laneIndex].Notes.Count))
						{
							var bar = Score.Bars[barIndex];
							var note = bar.Lanes[laneIndex].Notes[noteIndex];

							// 対象の小節の長さ
							var barSize = bar.Span * 4;

							var nowBarSize = ScoreStatus.NowBar.Span * 4;

							// 現在の小節の位置
							var nowLocation = nowBarSize * (1 - ScoreStatus.BarOffset);

							// 対象のノートの小節内位置
							var noteLocation = barSize * (1 - (double)note.CountLocation / bar.Count);

							double distance;

							if (ScoreStatus.BarIndex - barIndex == 1)
							{
								distance = nowBarSize - nowLocation + noteLocation;
							}
							else if (ScoreStatus.BarIndex - barIndex == -1)
							{
								distance = barSize - noteLocation + nowLocation;
							}
							else
							{
								distance = Math.Abs(nowLocation - noteLocation);
							}

							distance *= 1000;

							// Debug.WriteLine($"lane: {laneIndex}, bar: {barIndex}, note: {noteIndex}, distance: {distance}");

							barInfos.Add(new NoteDistanceInfo(laneIndex, barIndex, noteIndex, (int)distance));
						}
					}
				}
			}

			return noteDistanceInfoLists;
		}

		/// <summary>
		/// 距離情報の構造をレーン毎の操作しやすい形式に最適化します (CalcNearestNotes内から呼ばれています)
		/// </summary>
		/// <param name="noteDistanceInfosList"></param>
		/// <returns></returns>
		private List<List<NoteDistanceInfo>> ReconstructNoteDistance(List<List<NoteDistanceInfo>> noteDistanceInfosList)
		{
			var resList = new List<List<NoteDistanceInfo>>(); // noteDistanceInfoLists2[lane][bar]

			foreach (var laneLocation in Enumerable.Range(0, Score.LaneCount))
			{
				resList.Add(new List<NoteDistanceInfo>());

				foreach (var barLocation in Enumerable.Range(0, noteDistanceInfosList.Count))
				{
					var noteDistanceInfos = noteDistanceInfosList[barLocation];

					foreach (var noteDistanceInfo in (from info in noteDistanceInfos where info.LaneIndex == laneLocation select info))
					{
						resList[laneLocation].Add(noteDistanceInfo);
					}
				}
			}

			return resList;
		}

		/// <summary>
		/// レーン毎に最も近いノートを算出
		/// </summary>
		/// <param name="noteDistanceInfosList"></param>
		/// <returns></returns>
		private List<NoteDistanceInfo> CalcNearestNotes(List<List<NoteDistanceInfo>> noteDistanceInfosList)
		{
			noteDistanceInfosList = ReconstructNoteDistance(noteDistanceInfosList);

			var resList = new List<NoteDistanceInfo>();

			foreach (var noteDistanceInfos in noteDistanceInfosList)
			{
				NoteDistanceInfo minInfo = null;

				foreach (var noteDistanceInfo in noteDistanceInfos)
				{
					if (minInfo != null)
					{
						if (noteDistanceInfo.NoteDistance < minInfo.NoteDistance)
							minInfo = noteDistanceInfo;
					}
					else
						minInfo = noteDistanceInfo;
				}

				resList.Add(minInfo);
			}

			return resList;
		}

		/// <summary>
		/// 入力の判定をします
		/// </summary>
		private void Judgement()
		{
			var input = Input.Instance;

			if (Score.Song.IsPlaying)
			{
				var isInputted = false;
				foreach (var laneInput in KeyConfig.Instance.Lanes)
					if (laneInput.InputTime == 1)
						isInputted = true;
				
				if (isInputted)
				{
					var nearestNoteInfos = CalcNearestNotes(CalcNoteDistance());

					var target = new List<KeyConfigItem>();

					foreach (var laneInput in KeyConfig.Instance.Lanes)
						if (laneInput.InputTime == 1)
							target.Add(laneInput);

					Debug.WriteLine($"target: {{ {string.Join(", ", target)} }}");
				}
			}
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
								var count = (double)targetLane.Notes[noteIndex].CountLocation / bar.Count;
								var location = barDisplaySize * (count - ScoreStatus.BarOffset + i);

								images.Item("note").Draw(new Point((int)(laneX - images.Item("note").Size.Width / 2.0), (int)(650 - location - images.Item("note").Size.Height / 2.0)));
							}
						}
					}
				}

				foreach (var laneIndex in Enumerable.Range(0, KeyConfig.Instance.Lanes.Count))
				{
					var inputLane = KeyConfig.Instance.Lanes[laneIndex];

					var laneX = 200 + 100 + 176 * (laneIndex + 1);

					var effect = images.Item($"detectFrameEffect{laneIndex + 1}");

					if (inputLane.InputTime == 1)
					{
						effect.FinishFade();
						effect.Opacity = 100;
					}
					else if (inputLane.InputTime == 0)
						effect.Fade(0, .25);

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
