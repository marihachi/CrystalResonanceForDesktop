using DxSharp;
using DxSharp.Data;
using System.Drawing;
using DxSharp.Storage;
using CrystalResonanceDesktop.Utility;
using CrystalResonanceDesktop.Data;

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
					new Size(150, 30),
					DxSharp.Data.Enum.Position.CenterMiddle,
					10,
					FontStorage.Instance.Item("メイリオ16"),
					new ButtonStyle(Color.White, Color.Transparent, Color.White),
					new ButtonStyle(Color.White, Color.Transparent, Color.White),
					new ButtonStyle(Color.White, Color.Transparent, Color.White));

				_TitleMenu.Add("hoge");
				_TitleMenu.Add("piyo");
			}
		}

		public void Draw()
		{
			var logo = ImageStorage.Instance.Item("logo");
			logo.Draw(new Point(0, -150));

			_TitleMenu.Draw();
		}
	}
}
