using DxSharp;
using DxSharp.Data;
using System.Drawing;
using DxSharp.Storage;
using CrystalResonanceDesktop.Data;
using System;
using System.Collections.Generic;
using DxSharp.Utility;

namespace CrystalResonanceDesktop.Scenes
{
	public class Title : IScene
	{
		private bool IsInitialized { get; set; }

		private Menu _TitleMenu { get; set; }
		private List<Ripple> _Ripples { get; set; } = new List<Ripple>();
		private Random _Random { get; set; } = new Random();

		public void Update()
		{
			var core = SystemCore.Instance;
			var input = Input.Instance;
			var scenes = SceneStorage.Instance;
			var fonts = FontStorage.Instance;
			var images = ImageStorage.Instance;

			if (!IsInitialized)
			{
				IsInitialized = true;

				if (images.Item("logo") == null)
					images.Add("logo", new DxSharp.Data.Image("Resource/logo.png", 100, DxSharp.Data.Enum.Position.CenterMiddle));

				_TitleMenu = new Menu(
					new Point(0, 100),
					new Size(400, 50),
					DxSharp.Data.Enum.Position.CenterMiddle,
					20,
					fonts.Item("メイリオ20"),
					new ButtonStyle(Color.FromArgb(160, 255, 255, 255), Color.Transparent, Color.FromArgb(160, 255, 255, 255)),
					new ButtonStyle(Color.FromArgb(200, 255, 255, 255), Color.Transparent, Color.FromArgb(200, 255, 255, 255)),
					new ButtonStyle(Color.FromArgb(255, 255, 255, 255), Color.Transparent, Color.FromArgb(255, 255, 255, 255)));

				_TitleMenu.Add("Game Start");
				_TitleMenu.Add("Setting");
				_TitleMenu.Add("Close");

				_TitleMenu.ItemClick += (s, e) =>
				{
					IsInitialized = false;

					// game start
					if (e.ItemIndex == 0)
					{
						scenes.TargetScene = scenes.FindByName("GameMusicSelect");
					}

					// setting
					if (e.ItemIndex == 1)
					{
						scenes.TargetScene = scenes.FindByName("Setting");
					}

					// close
					if (e.ItemIndex == 2)
					{
						core.Close();
					}
				};

				_Ripples.Clear();
			}

			if ((input.Mouse.LeftInputTime == 1 || _Random.Next(0, 1000) < 4) && _Ripples.Count <= 6)
			{
				var x = _Random.Next(0, 1280);
				var y = _Random.Next(0, 720);

				_Ripples.Add(new Ripple(new Point(x, y)));
			}

			for(var i = 0; i < _Ripples.Count; i++)
			{
				_Ripples[i].AddRadius(2);

				if (_Ripples[i].Radius > 1280 * 1.21)
				{
					_Ripples.RemoveAt(i);
					i--;
				}
			}

			_TitleMenu.Update();
		}

		public void Draw()
		{
			var images = ImageStorage.Instance;

			images.Item("logo").Draw(new Point(0, -150));

			_TitleMenu.Draw();

			foreach (var ripple in _Ripples)
				ripple.Draw();
		}
	}
}
