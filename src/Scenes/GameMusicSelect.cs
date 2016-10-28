using DxSharp;
using DxSharp.Data;
using DxSharp.Data.Enum;
using DxSharp.Storage;
using DxSharp.Utility;
using System.Drawing;

namespace CrystalResonanceDesktop.Scenes
{
	public class GameMusicSelect : IScene
	{
		private bool IsInitialized { get; set; }

		public void Update()
		{
			var core = SystemCore.Instance;
			var input = Input.Instance;
			var scenes = SceneStorage.Instance;

			if (!IsInitialized)
			{
				IsInitialized = true;
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
		}

		public void Draw()
		{
			var fonts = FontStorage.Instance;

			fonts.Item("メイリオ20").Draw(new Point(0, 0), "Enter: 開発用演奏テスト", Color.White, Position.CenterMiddle);
		}
	}
}
