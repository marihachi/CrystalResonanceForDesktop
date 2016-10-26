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
		private bool _IsInitial { get; set; } = true;

		public void Update()
		{
			var core = SystemCore.Instance;

			if (_IsInitial)
			{
				_IsInitial = false;
			}

			if (Input.Instance.GetKey(KeyType.Escape).InputTime == 1)
			{
				SceneStorage.Instance.TargetScene = SceneStorage.Instance.FindByName("Title");
			}

			if (Input.Instance.GetKey(KeyType.Enter).InputTime == 1)
			{
				SceneStorage.Instance.TargetScene = SceneStorage.Instance.FindByName("GameMain");
			}
		}

		public void Draw()
		{
			FontStorage.Instance.Item("メイリオ20").Draw(new Point(0, 0), "Enter: 開発用演奏テスト", Color.White, Position.CenterMiddle);
		}
	}
}
