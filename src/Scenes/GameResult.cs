using DxSharp;
using DxSharp.Data;

namespace CrystalResonanceDesktop.Scenes
{
	public class GameResult : IScene
	{
		private bool _IsInitial { get; set; } = true;

		public void Update()
		{
			var core = SystemCore.Instance;

			if (_IsInitial)
			{
				_IsInitial = false;
			}

		}

		public void Draw()
		{

		}
	}
}
