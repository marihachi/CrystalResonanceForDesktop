using DxSharp;
using DxSharp.Data;
using System.Drawing;
using DxSharp.Storage;
using CrystalResonanceDesktop.Utility;
using CrystalResonanceDesktop.Data;
using System;

namespace CrystalResonanceDesktop.Scenes
{
	class Title : IScene
	{
		private bool _IsInitial { get; set; } = true;
		private Menu _TitleMenu { get; set; }

		public void Update()
		{
			var core = SystemCore.Instance;

			if (_IsInitial)
			{
				_IsInitial = false;

				ImageStorage.Instance.Add("logo", new DxSharp.Data.Image("Resource/logo.png", 100, DxSharp.Data.Enum.Position.CenterMiddle));

				_TitleMenu = new Menu(
					new Point(0, 100),
					new Size(400, 50),
					DxSharp.Data.Enum.Position.CenterMiddle,
					20,
					FontStorage.Instance.Item("メイリオ16"),
					new ButtonStyle(Color.FromArgb(160, 255, 255, 255), Color.Transparent, Color.FromArgb(160, 255, 255, 255)),
					new ButtonStyle(Color.FromArgb(200, 255, 255, 255), Color.Transparent, Color.FromArgb(200, 255, 255, 255)),
					new ButtonStyle(Color.FromArgb(255, 255, 255, 255), Color.Transparent, Color.FromArgb(255, 255, 255, 255)));

				_TitleMenu.Add("Game Start");
				_TitleMenu.Add("Setting");
				_TitleMenu.Add("Close");

				_TitleMenu.ItemClick += (s, e) =>
				{
					// game start
					if (e.ItemIndex == 0)
					{
						SceneStorage.Instance.TargetScene = SceneStorage.Instance.FindByName("GameMain");
					}

					// setting
					if (e.ItemIndex == 1)
					{
						SceneStorage.Instance.TargetScene = SceneStorage.Instance.FindByName("Setting");
					}

					// close
					if (e.ItemIndex == 2)
					{
						SystemCore.Instance.Close();
					}
				};
			}

			_TitleMenu.Update();
		}

		public void Draw()
		{
			var logo = ImageStorage.Instance.Item("logo");
			logo.Draw(new Point(0, -150));

			_TitleMenu.Draw();
		}
	}
}
