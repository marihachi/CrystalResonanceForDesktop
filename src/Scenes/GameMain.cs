using CrystalResonanceDesktop.Utility;
using DxSharp;
using DxSharp.Data;
using DxSharp.Data.Enum;
using DxSharp.Storage;
using DxSharp.Utility;
using System.Drawing;

namespace CrystalResonanceDesktop.Scenes
{
	class GameMain : IScene
	{
		private bool _IsInitial { get; set; } = true;

		public void Update()
		{
			var core = SystemCore.Instance;

			if (_IsInitial)
			{
				_IsInitial = false;

				ImageStorage.Instance.Add("detectionFrame", new DxSharp.Data.Image("Resource/detectionFrame.png", 100, Position.CenterMiddle));
			}

			if (Input.Instance.GetKey(KeyType.Escape).InputTime == 1)
			{
				SceneStorage.Instance.TargetScene = SceneStorage.Instance.FindByName("Title");
			}
		}

		public void Draw()
		{
			var core = SystemCore.Instance;

			var detectionFrame = ImageStorage.Instance.Item("detectionFrame");

			// 中央のひし形
			detectionFrame.Draw(new Point(0, 0));
		}
	}
}
