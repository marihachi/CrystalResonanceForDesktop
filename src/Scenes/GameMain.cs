using CrystalResonanceDesktop.Data;
using DxSharp;
using DxSharp.Data;
using DxSharp.Data.Enum;
using DxSharp.Storage;
using DxSharp.Utility;
using System.Drawing;
using System.Threading.Tasks;

namespace CrystalResonanceDesktop.Scenes
{
	public class GameMain : IScene
	{
		private bool IsInitialized { get; set; }

		private Task LoadTask { get; set; }

		private MusicManager Manager { get; set; }

		private Box SideBar { get; set; }
		private Box MessageBox { get; set; }

		private void Initialize()
		{
			var core = SystemCore.Instance;

			Manager = new MusicManager();

			MessageBox = new Box(new Point(0, 0), new Size(core.WindowSize.Width, 0), Color.White, true, 0, Position.LeftMiddle);

			LoadTask = Task.Run(async () =>
			{
				MessageBox.FadeSize(new Size(core.WindowSize.Width, 200), 0.5);
				MessageBox.FadeOpacity(60, 0.5);

				await Manager.LoadScoreAsync();

				Manager.Start();

				MessageBox.FadeOpacity(0, 0.5);
				MessageBox.FadeSize(new Size(core.WindowSize.Width, 0), 0.5);
			});

			SideBar = new Box(new Point(0, 0), new Size(0, core.WindowSize.Height), Color.White, true, 0);
			SideBar.FadeOpacity(20);
			SideBar.FadeSize(new Size(200, core.WindowSize.Height));
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

			if (LoadTask.IsCompleted)
			{
				Manager.Update();

				if (input.GetKey(KeyType.Escape).InputTime == 1)
				{
					scenes.TargetScene = scenes.FindByName("GameMusicSelect");
					IsInitialized = false;
					Manager.Score.Song.Stop();
				}
			}

			SideBar.Update();
			MessageBox.Update();
		}

		public void Draw()
		{
			var fonts = FontStorage.Instance;
			var core = SystemCore.Instance;

			Manager.Draw();

			SideBar.Draw();

			MessageBox.Draw();

			if (!LoadTask.IsCompleted && !MessageBox.IsFading)
				fonts.Item("メイリオ20").Draw(new Point(0, 0), "Now Loading ...",core.BackColor, Position.CenterMiddle);
		}
	}
}
