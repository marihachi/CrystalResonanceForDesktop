using DxSharp;
using DxSharp.Data;
using DxSharp.Storage;
using DxSharp.Utility;
using DxSharp.Data.Enum;

namespace CrystalResonanceDesktop.Scenes
{
	public class Setting : IScene
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
		}

		public void Draw()
		{

		}
	}
}
