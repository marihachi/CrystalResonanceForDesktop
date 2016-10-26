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
		private bool _IsInitial { get; set; } = true;

		private Task task { get; set; }

		private MusicManager Manager { get; set; }

		private Box SideBar { get; set; }
		private Box MessageBox { get; set; }

		public void Update()
		{
			var core = SystemCore.Instance;

			if (_IsInitial)
			{
				_IsInitial = false;

				Manager = new MusicManager();

				MessageBox = new Box(new Point(0, 0), new Size(core.WindowSize.Width, 0), Color.White, true, 0, Position.LeftMiddle);

				task = Task.Run(async () =>
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

			if (Input.Instance.GetKey(KeyType.Escape).InputTime == 1)
			{
				SceneStorage.Instance.TargetScene = SceneStorage.Instance.FindByName("GameMusicSelect");

				Manager.Score.Song.Stop();
			}

			if (task?.IsCompleted ?? false)
			{
				Manager.Update();
			}

			SideBar.Update();
			MessageBox.Update();
		}

		public void Draw()
		{
			Manager.Draw();

			SideBar.Draw();

			MessageBox.Draw();

			if (!(task?.IsCompleted ?? false) && !MessageBox.IsFading)
				FontStorage.Instance.Item("メイリオ20").Draw(new Point(0, 0), "Now Loading ...", SystemCore.Instance.BackColor, Position.CenterMiddle);
		}
	}
}
