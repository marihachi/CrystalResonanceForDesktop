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
			var sounds = SoundStorage.Instance;

			// リソース読み込み
			if (sounds.Item("opening") == null)
				sounds.Add("opening", new Sound("Resource/opening.mp3", 100));

			if (images.Item("logo") == null)
				images.Add("logo", new DxSharp.Data.Image("Resource/logo.png", 100));

			// bgm 再生
			var opening = sounds.Item("opening");
			opening.CurrentTime = 0;
			opening.Play();

			// メニュー 初期化
			var style = new ButtonStyle(
				fonts.Item("メイリオ20"),
				new ButtonStyleStatus(Color.FromArgb(160, 255, 255, 255), Color.Transparent, Color.FromArgb(160, 255, 255, 255)),
				new ButtonStyleStatus(Color.FromArgb(200, 255, 255, 255), Color.Transparent, Color.FromArgb(200, 255, 255, 255)),
				new ButtonStyleStatus(Color.FromArgb(255, 255, 255, 255), Color.Transparent, Color.FromArgb(255, 255, 255, 255)));

			TitleMenu = new MenuControl(new Point(core.WindowSize.Width / 2 - (200 + 10), core.WindowSize.Height * 3 / 5), 10, new Size(400, 50), Color.Transparent, style);

			TitleMenu.Add("Game Start", (s, e) => {
				IsInitialized = false;
				sounds.Item("opening").Stop();
				scenes.TargetScene = scenes.FindByName("GameMusicSelect");
			});

			TitleMenu.Add("Setting", (s, e) => {
				IsInitialized = false;
				sounds.Item("opening").Stop();
				scenes.TargetScene = scenes.FindByName("Setting");
			});

			TitleMenu.Add("Close", (s, e) => {
				IsInitialized = false;
				sounds.Item("opening").Stop();
				core.Close();
			});

			// 波紋エフェクト クリア
			Ripples.Clear();
		}

		public void Update()
		{
			var input = Input.Instance;
			var sounds = SoundStorage.Instance;

			if (!IsInitialized)
			{
				IsInitialized = true;

				Initialize();
			}

			// 波紋エフェクト 更新
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

			// 更新
			sounds.Item("opening").Update();
			TitleMenu.Update();
		}

		public void Draw()
		{
			var core = SystemCore.Instance;
			var images = ImageStorage.Instance;
			var fonts = FontStorage.Instance;
			var input = Input.Instance;

			// ロゴ
			var logo = images.Item("logo");
			logo.Draw(new Point(core.WindowSize.Width / 2 - logo.Size.Width / 2, core.WindowSize.Height / 2 - logo.Size.Height / 2  - 150));

			// メニュー
			TitleMenu.Draw();

			// 波紋エフェクト
			foreach (var ripple in Ripples)
				ripple.Draw();

			if (core.IsShowDebugImageBorder)
			{
				fonts.Item("メイリオ16").Draw(new Point(0, 0), $"Mouse: {input.Mouse.PointerLocation}", Color.White);
			}
		}
	}
}
