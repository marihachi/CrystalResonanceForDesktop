using DxSharp;
using DxSharp.Data;
using DxSharp.Storage;
using DxSharp.Utility;
using DxSharp.Data.Enum;

namespace CrystalResonanceDesktop.Scenes
{
	public class Setting : IScene
	{
		private bool IsInitialized { get; set; }

		private void Initialize()
		{

		}

		public void Update()
		{
			var core = SystemCore.Instance;
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
		}

		public void Draw()
		{

		}
	}
}
