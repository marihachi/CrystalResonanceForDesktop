using DxSharp;
using DxSharp.Data;
using System.Drawing;
using DxSharp.Storage;
using CrystalResonanceDesktop.Utility;
using CrystalResonanceDesktop.Data;
using System;
using System.Collections.Generic;
using DxSharp.Utility;
using System.Linq;

namespace CrystalResonanceDesktop.Scenes
{
	class Title : IScene
	{
		private bool _IsInitial { get; set; } = true;
		private Menu _TitleMenu { get; set; }
		private List<Ripple> _Ripples { get; set; } = new List<Ripple>();
		private Random _Random { get; set; } = new Random();

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
					FontStorage.Instance.Item("メイリオ20"),
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
						SceneStorage.Instance.TargetScene = SceneStorage.Instance.FindByName("GameMusicSelect");
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

			if ((Input.Instance.MouseLeft == 1 || _Random.Next(0, 1000) < 4) && _Ripples.Count <= 6)
			{
				var x = _Random.Next(0, 1280);
				var y = _Random.Next(0, 720);

				_Ripples.Add(new Ripple(new Point(x, y)));
			}

			for(var i = 0; i < _Ripples.Count; i++)
			{
				_Ripples[i].AddRadius(2);

				if (_Ripples[i].Radius > 1280 * 1.42)
				{
					_Ripples.RemoveAt(i);
					i--;
				}
			}

			_TitleMenu.Update();
		}

		public void Draw()
		{
			var logo = ImageStorage.Instance.Item("logo");
			logo.Draw(new Point(0, -150));

			_TitleMenu.Draw();

			foreach (var ripple in _Ripples)
				ripple.Draw();
		}
	}
}
