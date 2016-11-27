using CrystalResonanceDesktop.Data;
using CrystalResonanceDesktop.Data.Control;
using CrystalResonanceDesktop.Utility;
using DxSharp;
using DxSharp.Data;
using DxSharp.Data.Enum;
using DxSharp.Storage;
using DxSharp.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CrystalResonanceDesktop.Scenes
{
	public class GameMusicSelect : IScene
	{
		public SwipeableListControl<Control> SongList { get; set; }

		private Task LoadTask { get; set; }
		private Exception LoadTaskException { get; set; }

		private bool IsInitialized { get; set; }

		private void Initialize()
		{
			var scenes = SceneStorage.Instance;

			SongList = new SwipeableListControl<Control>(new Point(0, 0), 30, Color.White, new Size((int)(SystemCore.Instance.WindowSize.Width * (49 / 64d)), SystemCore.Instance.WindowSize.Height), System.Windows.Forms.Orientation.Vertical);

			var dirs = Directory.GetDirectories("Song/");

			LoadTask = Task.Run(async () =>
			{
				try
				{
					foreach (var index in Enumerable.Range(0, dirs.Length))
					{
						var score = await MusicScore.DeserializeAsync($"{dirs[index]}/score.json");
						
						var button = new ButtonControl(
							new Point(0, 0),
							new Size(700, 60),
							$"{score.SongTitle}  (BPM: {score.BPM})",
							new ButtonControl.ButtonStyle(
								FontStorage.Instance.Item("メイリオ20"),
								new ButtonControl.ButtonStyleStatus(Color.White, Color.Transparent, Color.White),
								new ButtonControl.ButtonStyleStatus(Color.White, Color.Transparent, Color.White),
								new ButtonControl.ButtonStyleStatus(Color.White, Color.Transparent, Color.White)));

						button.Click += (s, e) =>
						{
							var main = (GameMain)scenes.FindByName("GameMain");

							main.Manager = new MusicManager();
							main.Manager.Score = score;

							scenes.TargetScene = scenes.FindByName("GameMain");
							IsInitialized = false;
						};

						SongList.Items.Add(button);
					}

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

			if (input.GetKey(KeyType.Escape).InputTime == 1)
			{
				scenes.TargetScene = scenes.FindByName("Title");
				IsInitialized = false;
			}

			/*
			if (input.GetKey(KeyType.Enter).InputTime == 1)
			{
				scenes.TargetScene = scenes.FindByName("GameMain");
				IsInitialized = false;
			}
			*/

			SongList.Update();
		}

		public void Draw()
		{
			var fonts = FontStorage.Instance;

			SongList.Draw();
		}
	}
}
