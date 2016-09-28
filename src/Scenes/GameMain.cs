using DxSharp;
using DxSharp.Data;
using System.Drawing;
using DxSharp.Storage;
using CrystalResonanceDesktop.Utility;
using CrystalResonanceDesktop.Data;
using System;
using DxSharp.Data.Enum;
using DxSharp.Utility;

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
			ImageStorage.Instance.Item("detectionFrame").Draw(new Point(0, 0)); // 中央のひし形
		}
	}
}
