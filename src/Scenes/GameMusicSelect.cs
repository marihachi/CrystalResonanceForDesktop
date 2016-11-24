﻿using CrystalResonanceDesktop.Data.Control;
using DxSharp;
using DxSharp.Data;
using DxSharp.Data.Enum;
using DxSharp.Storage;
using DxSharp.Utility;
using System.Diagnostics;
using System.Drawing;

namespace CrystalResonanceDesktop.Scenes
{
	public class GameMusicSelect : IScene
	{
		public ScrollableListControl<Control> SongList { get; set; }

		private bool IsInitialized { get; set; }

		private void Initialize()
		{
			SongList = new ScrollableListControl<Control>(new Point(0, 0), 10, Color.White, new Size((int)(SystemCore.Instance.WindowSize.Width * (49 / 64d)), SystemCore.Instance.WindowSize.Height));

			SongList.Scroll += (s, e) =>
			{
				Debug.WriteLine(e.NewScrollLocation);
			};
		}

		public void Update()
		{
			var input = Input.Instance;
			var scenes = SceneStorage.Instance;

			if (!IsInitialized)
			{
				IsInitialized = true;

				Initialize();
			}

			if (input.GetKey(KeyType.Escape).InputTime == 1)
			{
				scenes.TargetScene = scenes.FindByName("Title");
				IsInitialized = false;
			}

			if (input.GetKey(KeyType.Enter).InputTime == 1)
			{
				scenes.TargetScene = scenes.FindByName("GameMain");
				IsInitialized = false;
			}

			SongList.Update();
		}

		public void Draw()
		{
			var fonts = FontStorage.Instance;

			fonts.Item("メイリオ20").Draw(new Point(0, 0), "Enter: 開発用演奏テスト", Color.White, Position.CenterMiddle);

			SongList.Draw();
		}
	}
}
