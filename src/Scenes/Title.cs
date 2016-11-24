using CrystalResonanceDesktop.Data;
using CrystalResonanceDesktop.Data.Control;
using DxSharp;
using DxSharp.Data;
using DxSharp.Storage;
using DxSharp.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using static CrystalResonanceDesktop.Data.Control.ButtonControl;

namespace CrystalResonanceDesktop.Scenes
{
	public class Title : IScene
	{
		private bool IsInitialized { get; set; }

		private MenuControl TitleMenu { get; set; }
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

			var style = new ButtonStyle(
				fonts.Item("メイリオ20"),
				new ButtonStyleStatus(Color.FromArgb(160, 255, 255, 255), Color.Transparent, Color.FromArgb(160, 255, 255, 255)),
				new ButtonStyleStatus(Color.FromArgb(200, 255, 255, 255), Color.Transparent, Color.FromArgb(200, 255, 255, 255)),
				new ButtonStyleStatus(Color.FromArgb(255, 255, 255, 255), Color.Transparent, Color.FromArgb(255, 255, 255, 255)));

			TitleMenu = new MenuControl(new Point(core.WindowSize.Width / 2 - (200 + 10), core.WindowSize.Height * 3 / 5), 10, new Size(400, 50), Color.White, style);

			TitleMenu.Add("Game Start", (s, e) => {
				IsInitialized = false;
				scenes.TargetScene = scenes.FindByName("GameMusicSelect");
			});

			TitleMenu.Add("Setting", (s, e) => {
				IsInitialized = false;
				scenes.TargetScene = scenes.FindByName("Setting");
			});

			TitleMenu.Add("Close", (s, e) => {
				IsInitialized = false;
				core.Close();
			});

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

			for (var i = 0; i < Ripples.Count; i++)
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
			var fonts = FontStorage.Instance;

			images.Item("logo").Draw(new Point(0, -150));

			TitleMenu.Draw();

			foreach (var ripple in Ripples)
				ripple.Draw();

			if (SystemCore.Instance.IsShowDebugImageBorder)
			{
				var font = fonts.Item("メイリオ16");
				font.Draw(new Point(0, 0), $"Mouse: {Input.Instance.Mouse.PointerLocation}", Color.White);
			}
		}
	}
}
