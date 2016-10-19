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
	class GameMain : IScene
	{
		private bool _IsInitial { get; set; } = true;

		private Task task { get; set; }

		private MusicManager Manager { get; set; }

		public void Update()
		{
			var core = SystemCore.Instance;

			if (_IsInitial)
			{
				_IsInitial = false;

				ImageStorage.Instance.Add("note", new DxSharp.Data.Image("Resource/note.png", 100, Position.LeftTop));

				Manager = new MusicManager();

				task = Task.Run(async () =>
				{
					await Manager.LoadScoreAsync();

					Manager.Start();
				});
			}

			if (Input.Instance.GetKey(KeyType.Escape).InputTime == 1)
			{
				SceneStorage.Instance.TargetScene = SceneStorage.Instance.FindByName("GameMusicSelect");
			}

			if (task?.IsCompleted ?? false)
			{
				Manager.Update();
			}
		}

		public void Draw()
		{
			if (task?.IsCompleted ?? false)
				Manager.Draw();
			else
				FontStorage.Instance.Item("メイリオ16").Draw(new Point(0, 0), "Now Loading ...", Color.White);
		}
	}
}
