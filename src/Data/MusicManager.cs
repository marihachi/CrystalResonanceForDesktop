using CrystalResonanceDesktop.Data.Enum;
using CrystalResonanceDesktop.Utility;
using DxSharp;
using DxSharp.Data;
using DxSharp.Data.Enum;
using DxSharp.Storage;
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
		/// 譜面の状態を取得または設定します
		/// </summary>
		public ScoreStatus ScoreStatus { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public int NoteSpeedBase { get; set; } = 350;

		/// <summary>
		/// 現在のコンボ数を取得または設定します
		/// </summary>
		public int Combo { get; set; }

		/// <summary>
		/// 現在のセッションの最大コンボ数を取得または設定します
		/// </summary>
		public int SessionMaxCombo { get; set; }

		/// <summary>
		/// 非同期で譜面データを読み込みます
		/// </summary>
		public async Task LoadScoreAsync()
		{
			// TODO: 仮
			Score = new MusicScore("ウラココロ", 120, new Uri("https://www.youtube.com/watch?v=qo1v9oiMolM"), 4, -500 * 5 / 16);
			await Score.ExtractSong();

			foreach (int i in Enumerable.Range(0, 256))
			{
				var bar = new MusicBar(Score, 8, 1);

				var lane_sim = new MusicLane(bar);
				lane_sim.Notes.AddRange(new List<MusicNote> { new MusicNote(1, lane_sim) });

				var lane_hi = new MusicLane(bar);
				lane_hi.Notes.AddRange(new List<MusicNote> { new MusicNote(2, lane_hi), new MusicNote(3, lane_hi), new MusicNote(4, lane_hi), new MusicNote(5, lane_hi), new MusicNote(6, lane_hi), new MusicNote(7, lane_hi), new MusicNote(8, lane_hi) });

				var lane_kick = new MusicLane(bar);
				lane_kick.Notes.AddRange(new List<MusicNote> { new MusicNote(1, lane_kick), new MusicNote(5, lane_kick) });

				var lane_sn = new MusicLane(bar);
				lane_sn.Notes.AddRange(new List<MusicNote> { new MusicNote(3, lane_sn), new MusicNote(7, lane_sn) });

				bar.Lanes.AddRange(new List<MusicLane> { lane_sn, lane_kick, lane_hi, lane_sim });
				Score.Bars.Add(bar);

				var bar2 = new MusicBar(Score, 8, 1);

				var lane2_sn = new MusicLane(bar2);
				lane2_sn.Notes.AddRange(new List<MusicNote> { new MusicNote(3, lane2_sn), new MusicNote(7, lane2_sn) });

				bar2.Lanes.AddRange(new List<MusicLane> { lane2_sn });

				Score.Bars.Add(bar2);
			}

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

		/// <summary>
		/// 距離情報の構造をレーン毎の操作しやすい形式に最適化します (CalcNoteDistance内から呼ばれています)
		/// </summary>
		/// <param name="noteDistancesList">小節毎に管理された最適化前のNoteDistanceInfoのリスト</param>
		/// <returns>レーン毎に管理された最適化済みのNoteDistanceInfoのリスト</returns>
		private IEnumerable<IEnumerable<NoteDistanceInfo>> ReconstructNoteDistance(IEnumerable<IEnumerable<NoteDistanceInfo>> noteDistancesList)
		{
			foreach (var laneLocation in Enumerable.Range(0, Score.LaneCount))
			{
				var infos = new List<NoteDistanceInfo>();

				foreach (var noteDistanceInfos in noteDistancesList)
					foreach (var noteDistanceInfo in (from info in noteDistanceInfos where info.LaneIndex == laneLocation select info))
						infos.Add(noteDistanceInfo);

				yield return infos; // noteDistanceInfoLists2[lane][bar]
			}
		}

		/// <summary>
		/// 現在の再生位置からの各ノートの符号付の距離を算出
		/// </summary>
		/// <returns>レーン毎に管理された最適化済みのNoteDistanceInfoのリスト</returns>
		private IEnumerable<IEnumerable<NoteDistanceInfo>> CalcNoteDistance()
		{
			var noteDistanceInfoLists = new List<List<NoteDistanceInfo>>(); // noteDistanceInfoLists[bar][lane]

			// Debug.WriteLine($"-- CalcNoteDistance --");

			if (ScoreStatus.BarIndex >= 0)
			{
				foreach (var barIndex in Enumerable.Range(ScoreStatus.BarIndex - 1, 3))
				{
					if (barIndex > -1)
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

								// 対象小節の長さ
								var barSize = bar.Span * 4;

								// 現在小節の長さ
								var nowBarSize = ScoreStatus.NowBar.Span * 4;

								// 現在小節の位置
								var nowLocation = nowBarSize * ScoreStatus.BarOffset;

								// 対象ノートの小節内位置
								var noteLocation = barSize * ((double)note.CountLocation / bar.Count);

								double distance;

								if (ScoreStatus.BarIndex - barIndex >= 1)
								{
									// 前の小節
									distance = -(barSize - noteLocation) - nowLocation;
									
									// Debug.WriteLine($"prev: {{ {barIndex}:{noteIndex}, distance: {(int)(distance * 1000)} }} -({barSize} - {noteLocation}) + {nowLocation}");
								}
								else if (ScoreStatus.BarIndex - barIndex <= -1)
								{
									// 次の小節
									distance = nowBarSize - nowLocation + noteLocation;
									
									// Debug.WriteLine($"next: {{ {barIndex}:{noteIndex}, distance: {(int)(distance * 1000)} }} {nowBarSize} - {nowLocation} + {noteLocation}");
								}
								else
								{
									// 現在の小節
									distance = noteLocation - nowLocation;
									
									// Debug.WriteLine($"now: {{ {barIndex}:{noteIndex}, distance: {(int)(distance * 1000)} }} {noteLocation} - {nowLocation}");
								}

								distance *= 1000;

								// Debug.WriteLine($"note[1,0,0].distance: {distance}");

								barInfos.Add(new NoteDistanceInfo(Score, laneIndex, barIndex, noteIndex, (int)distance));
							}
						}
					}
				}
			}
			
			// Debug.WriteLine($"-- end CalcNoteDistance --");

			return ReconstructNoteDistance(noteDistanceInfoLists);
		}

		/// <summary>
		/// レーン毎に最も近いノートを算出
		/// </summary>
		/// <param name="noteDistancesList">レーン毎に管理された最適化済みのNoteDistanceInfoのリスト</param>
		/// <returns>レーン毎の最寄ノートの NoteDistanceInfo</returns>
		private IEnumerable<NoteDistanceInfo> CalcNearestNotes(IEnumerable<IEnumerable<NoteDistanceInfo>> noteDistancesList)
		{
			foreach (var noteDistances in noteDistancesList)
			{
				yield return noteDistances.FindMin(i => Math.Abs(i.NoteDistance));
			}
		}

		/// <summary>
		/// 入力の判定をします
		/// </summary>
		private void Judgement()
		{
			if (Score.Song.IsPlaying)
			{
				var noteDistancesList = CalcNoteDistance();

				var isInputted = (from l in KeyConfig.Instance.Lanes where l.InputTime == 1 select l).Count() != 0;

				if (isInputted)
				{
					var nearestNoteDistances = CalcNearestNotes(noteDistancesList);

					var targetLaneIndexes = new List<int>();

					foreach (var laneInputIndex in Enumerable.Range(0, KeyConfig.Instance.Lanes.Count))
						if (KeyConfig.Instance.Lanes[laneInputIndex].InputTime == 1)
							targetLaneIndexes.Add(laneInputIndex);

					foreach (var targetLaneIndex in targetLaneIndexes)
					{
						var nearestNoteInfo = nearestNoteDistances.ElementAt(targetLaneIndex);

						if (nearestNoteInfo != null)
						{
							if (Math.Abs(nearestNoteInfo.NoteDistance) <= 220) // 判定の範囲内のとき
							{
								nearestNoteInfo.TargetNote.PushState = true;

								if (Math.Abs(nearestNoteInfo.NoteDistance) <= 70) // perfect
								{
									nearestNoteInfo.TargetNote.Rating = NotePushRating.Perfect;
									Combo++;
								}
								else if (Math.Abs(nearestNoteInfo.NoteDistance) <= 130) // good
								{
									nearestNoteInfo.TargetNote.Rating = NotePushRating.Good;
									Combo++;
								}
								else if (Math.Abs(nearestNoteInfo.NoteDistance) <= 180) // bad
								{
									nearestNoteInfo.TargetNote.Rating = NotePushRating.Bad;
									Combo = 0;
								}
								else // miss
								{
									nearestNoteInfo.TargetNote.Rating = NotePushRating.Miss;
									Combo = 0;
								}

								if (Combo > SessionMaxCombo)
									SessionMaxCombo = Combo;

								Debug.WriteLine($"Judgement: {{ barIndex: {Score.Bars.IndexOf(nearestNoteInfo.TargetNote.ParentLane.ParentBar)}, distance: {nearestNoteInfo.NoteDistance} }}");
							}
						}
					}
				}
				
				foreach (var noteDistances in noteDistancesList)
				{
					foreach (var noteDistance in noteDistances)
					{
						if (!noteDistance.TargetNote.PushState)
						{
							// 判定範囲を超えてマイナス方向にあるとき
							if (noteDistance.NoteDistance < -180 && noteDistance.TargetNote.Rating == NotePushRating.None)
							{
								Debug.WriteLine($"Judgement: {{ barIndex: {Score.Bars.IndexOf(noteDistance.TargetNote.ParentLane.ParentBar)}, distance: {noteDistance.NoteDistance} }}");
								Combo = 0;
								noteDistance.TargetNote.Rating = NotePushRating.Miss;
							}
						}
					}
				}
				
			}
		}

		/// <summary>
		/// ノートを描画します
		/// </summary>
		public void DrawNotes(List<int> laneXcoordinates)
		{
			var images = ImageStorage.Instance;

			if (Score != null && ScoreStatus != null)
			{
				// 表示する小節の範囲
				foreach (int i in Enumerable.Range(-1, 3))
				{
					var targetBarIndex = ScoreStatus.BarIndex + i;

					// 小節のインデックスがマイナスなら処理しない
					if (targetBarIndex >= 0)
					{
						foreach (var laneIndex in Enumerable.Range(0, Score.Bars[targetBarIndex].Lanes.Count))
						{
							var targetLane = Score.Bars[targetBarIndex].Lanes[laneIndex];
							var bar = ScoreStatus.TargetScore.Bars[targetBarIndex];
							var barDisplaySize = NoteSpeedBase * 4 * bar.Span;
							var laneX = laneXcoordinates[laneIndex];

							foreach (var noteIndex in Enumerable.Range(0, targetLane.Notes.Count))
							{
								var note = targetLane.Notes[noteIndex];

								var count = (double)note.CountLocation / bar.Count;
								var location = barDisplaySize * (count - ScoreStatus.BarOffset + i);

								if (note.Rating == NotePushRating.None)
								{
									images.Item("note").Draw(new Point((int)(laneX - images.Item("note").Size.Width / 2.0), (int)(650 - location - images.Item("note").Size.Height / 2.0)));
								}
							}
						}
					}
				}
			}
		}
	}
}
