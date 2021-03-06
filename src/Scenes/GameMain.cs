﻿using CrystalResonanceDesktop.Data;
using CrystalResonanceDesktop.Utility;
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

namespace CrystalResonanceDesktop.Scenes
{
	public class GameMain : IScene
	{
		private bool IsInitialized { get; set; }

		private Task LoadTask { get; set; }
		private Exception LoadTaskException { get; set; }

		public MusicManager Manager { get; set; }

		private Box SideBar { get; set; }
		private Box MessageBox { get; set; }

		private void Initialize()
		{
			var core = SystemCore.Instance;
			var images = ImageStorage.Instance;

			// リソース読み込み
			if (images.Item("logoMini") == null)
				images.Add("logoMini", new DxSharp.Data.Image("Resource/logoMini.png", 100));

			// メッセージボックス 初期化
			MessageBox = new Box(new Point(0, 0), new Size(core.WindowSize.Width, 0), Color.FromArgb(186, 231, 234), true, 0, Position.LeftMiddle);

			// サイドバー 初期化
			SideBar = new Box(new Point(0, 0), new Size(0, core.WindowSize.Height), Color.FromArgb(117, 207, 213), true, 0);
			SideBar.FadeOpacity(100);
			SideBar.FadeSize(new Size(280, core.WindowSize.Height));

			// 非同期読み込み
			LoadTask = Task.Run(async () =>
			{
				try
				{
					MessageBox.FadeSize(new Size(core.WindowSize.Width, 200), 0.5);
					MessageBox.FadeOpacity(100, 0.5);

					Manager.ScoreStatus = new ScoreStatus(0, Manager.Score);
					await Manager.Score.ExtractSong();

					Manager.Score.CurrentDifficultyType = Data.Enum.MusicDifficultyType.Easy;

					Manager.Start();

					MessageBox.FadeOpacity(0, 0.5);
					MessageBox.FadeSize(new Size(core.WindowSize.Width, 0), 0.5);
				}
				catch (Exception e)
				{
					LoadTaskException = e;
				}
			});
		}

		public void Update()
		{
			var input = Input.Instance;
			var scenes = SceneStorage.Instance;

			if (!IsInitialized)
			{
				IsInitialized = true;
				Initialize();
			}

			if (LoadTaskException != null)
				throw new Exception("Task内で例外が発生しました", LoadTaskException);

			if (LoadTask.IsCompleted)
			{
				Manager.Update();

				if (input.GetKey(KeyType.Escape).InputTime == 1)
				{
					scenes.TargetScene = scenes.FindByName("GameMusicSelect");
					IsInitialized = false;
					Manager.Score?.Song?.Stop();
				}
			}

			// 更新
			SideBar.Update();
			MessageBox.Update();
		}

		public void Draw()
		{
			var fonts = FontStorage.Instance;
			var core = SystemCore.Instance;
			var images = ImageStorage.Instance;

			var font16 = fonts.Item("メイリオ16");
			var font20 = fonts.Item("メイリオ20");

			// 判定ライン
			using (new AlphaBlend((int)(255 * 0.5)))
				DX.DrawLine(0, 650, SystemCore.Instance.WindowSize.Width, 650, 0xffffff);

			// レーン軸の決定
			var margin = 50;
			var xCoodinates = new List<int>();
			foreach (var i in Enumerable.Range(1, 4))
				xCoodinates.Add(SideBar.Size.Width + margin + ((core.WindowSize.Width - SideBar.Size.Width - margin * 2) / 5) * i);

			if (Manager.Score != null)
			{
				// エフェクト
				foreach (var laneIndex in Enumerable.Range(0, Manager.Score.CurrentDifficulty.LaneCount))
				{
					var inputLane = KeyConfig.Instance.Lanes[laneIndex];

					var laneX = xCoodinates[laneIndex];
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

				// ノート
				Manager.DrawNotes(xCoodinates);

				// サイドバー
				SideBar.Draw();
				if (SideBar.Opacity == 100)
				{
					font20.Draw(new Point(50, core.WindowSize.Height * 3 / 4 - 30), $"♪{Manager.Score.SongTitle}", Color.White, Position.LeftTop);
					font20.Draw(new Point(50, core.WindowSize.Height * 3 / 4), $"{Manager.Combo} combo", Color.White, Position.LeftTop);
				}

				// ロゴ
				var logoMini = images.Item("logoMini");
				logoMini.Draw(new Point(SideBar.Size.Width - 280 + 280 / 2 - logoMini.Size.Width / 2, 30), 1);
			}

			// メッセージ
			MessageBox.Draw();

			if (!LoadTask.IsCompleted && !MessageBox.IsFading)
				font20.Draw(new Point(0, 0), "Now Loading ...", core.BackColor, Position.CenterMiddle);

			// デバッグ
			if (SystemCore.Instance.IsShowDebugImageBorder)
			{
				if (Manager.ScoreStatus != null)
				{
					font16.Draw(new Point(5, 20 * 1), $"beatLocation: {Manager.ScoreStatus.BeatLocation:00.0}", Color.White);
					font16.Draw(new Point(5, 20 * 2), $"beatIndex: {Manager.ScoreStatus.BeatIndex}", Color.White);
					font16.Draw(new Point(5, 20 * 3), $"beatOffset: {Manager.ScoreStatus.BeatOffset:00.0}", Color.White);
					font16.Draw(new Point(5, 20 * 4), $"barLocation: {Manager.ScoreStatus.BarLocation:00.0}", Color.White);
					font16.Draw(new Point(5, 20 * 5), $"barIndex: {Manager.ScoreStatus.BarIndex}", Color.White);
					font16.Draw(new Point(5, 20 * 6), $"barOffset: {Manager.ScoreStatus.BarOffset:00.0}", Color.White);
					font16.Draw(new Point(5, 20 * 7), $"barBeatLocation1: {Manager.ScoreStatus.BarBeatLocation:00.0}", Color.White);
					font16.Draw(new Point(5, 20 * 8), $"countLocation: {Manager.ScoreStatus.CountLocation:00.0}", Color.White);
				}
				font16.Draw(new Point(5, 20 * 9), $"NoteSpeedBase: {Manager.NoteSpeedBase}", Color.White);
			}
		}
	}
}
