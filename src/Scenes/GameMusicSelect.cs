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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static CrystalResonanceDesktop.Data.Control.ButtonControl;

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
			var core = SystemCore.Instance;
			var scenes = SceneStorage.Instance;
			var fonts = FontStorage.Instance;

			// 曲リストのコントロール 初期化
			SongList = new SwipeableListControl<Control>(
				new Point(0, 0),
				30,
				Color.White,
				new Size((int)(core.WindowSize.Width * (49 / 64d)), core.WindowSize.Height),
				System.Windows.Forms.Orientation.Vertical);

			// Songディレクトリ下のサブディレクトリを取得
			if (!Directory.Exists("Song/"))
				Directory.CreateDirectory("Song/");
			var dirs = new List<string>(Directory.GetDirectories("Song/"));

			// 非同期読み込み
			LoadTask = Task.Run(async () =>
			{
				try
				{
					foreach (var index in Enumerable.Range(0, dirs.Count))
					{
						var score = await MusicScore.DeserializeAsync($"{dirs[index]}/score.json");

						var button = new ButtonControl(
							new Point(0, 0),
							new Size(700, 60),
							$"{score.SongTitle}  (BPM: {score.BPM})",
							new ButtonStyle(
								fonts.Item("メイリオ20"),
								new ButtonStyleStatus(Color.FromArgb(160, 255, 255, 255), Color.FromArgb(30, Color.White), Color.FromArgb(180, 255, 255, 255)),
								new ButtonStyleStatus(Color.FromArgb(200, 255, 255, 255), Color.FromArgb(30, Color.White), Color.FromArgb(220, 255, 255, 255)),
								new ButtonStyleStatus(Color.FromArgb(255, 255, 255, 255), Color.FromArgb(30, Color.White), Color.FromArgb(255, 255, 255, 255))));

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

			// 更新
			SongList.Update();
		}

		public void Draw()
		{
			// 曲リスト
			SongList.Draw();
		}
	}
}
