using DxSharp;
using DxSharp.Data;
using System.Drawing;
using DxSharp.Storage;
using CrystalResonanceDesktop.Data;
using System;
using System.Collections.Generic;
using DxSharp.Utility;
using CrystalResonanceDesktop.Data.Control;

namespace CrystalResonanceDesktop.Scenes
{
	public class Title : IScene
	{
		private bool IsInitialized { get; set; }

		private Menu TitleMenu { get; set; }
		private List<Ripple> Ripples { get; set; } = new List<Ripple>();
		private Random Random { get; set; } = new Random();

		private void Initialize()
		{
			var core = SystemCore.Instance;
			var scenes = SceneStorage.Instance;
			var fonts = FontStorage.Instance;
			var images = ImageStorage.Instance;

			if (images.Item("logo") == null)
				images.Add("logo", new DxSharp.Data.Image("Resource/logo.png", 100, DxSharp.Data.Enum.Position.CenterMiddle));

			TitleMenu = new Menu(
				new Point(0, 100),
				new Size(400, 50),
				DxSharp.Data.Enum.Position.CenterMiddle,
				20,
				fonts.Item("メイリオ20"),
				new Button.ButtonStyle(Color.FromArgb(160, 255, 255, 255), Color.Transparent, Color.FromArgb(160, 255, 255, 255)),
				new Button.ButtonStyle(Color.FromArgb(200, 255, 255, 255), Color.Transparent, Color.FromArgb(200, 255, 255, 255)),
				new Button.ButtonStyle(Color.FromArgb(255, 255, 255, 255), Color.Transparent, Color.FromArgb(255, 255, 255, 255)));

			TitleMenu.Add("Game Start");
			TitleMenu.Add("Setting");
			TitleMenu.Add("Close");

			TitleMenu.ItemClick += (s, e) =>
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

			Ripples.Clear();
		}

		public void Update()
		{
			var input = Input.Instance;

			if (!IsInitialized)
			{
				IsInitialized = true;

				Initialize();
			}

			if ((input.Mouse.LeftInputTime == 1 || Random.Next(0, 1000) < 4) && Ripples.Count <= 6)
			{
				var x = Random.Next(0, 1280);
				var y = Random.Next(0, 720);

				Ripples.Add(new Ripple(new Point(x, y)));
			}

			for(var i = 0; i < Ripples.Count; i++)
			{
				Ripples[i].AddRadius(2);

				if (Ripples[i].Radius > 1280 * 1.21)
				{
					Ripples.RemoveAt(i);
					i--;
				}
			}

			TitleMenu.Update();
		}

		public void Draw()
		{
			var images = ImageStorage.Instance;

			images.Item("logo").Draw(new Point(0, -150));

			TitleMenu.Draw();

			foreach (var ripple in Ripples)
				ripple.Draw();
		}
	}
}
