using CrystalResonanceDesktop.Data;
using DxSharp;
using DxSharp.Data;
using DxSharp.Data.Enum;
using DxSharp.Storage;
using DxSharp.Utility;
using System.Collections.Generic;
using System.Linq;

namespace CrystalResonanceDesktop.Scenes
{
	class GameMain : IScene
	{
		private bool _IsInitial { get; set; } = true;

		private MusicManager Manager { get; set; }

		public void Update()
		{
			var core = SystemCore.Instance;

			if (_IsInitial)
			{
				_IsInitial = false;

				ImageStorage.Instance.Add("note", new Image("Resource/note.png", 100, Position.LeftTop));

				Manager = new MusicManager();
				Manager.LoadScore();
				Manager.Start();
			}

			if (Input.Instance.GetKey(KeyType.Escape).InputTime == 1)
			{
				SceneStorage.Instance.TargetScene = SceneStorage.Instance.FindByName("GameMusicSelect");
			}

			Manager.Update();
		}

		public void Draw()
		{
			Manager.Draw();
		}
	}
}
