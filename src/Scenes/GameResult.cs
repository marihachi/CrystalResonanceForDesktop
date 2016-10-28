using DxSharp;
using DxSharp.Data;

namespace CrystalResonanceDesktop.Scenes
{
	public class GameResult : IScene
	{
		private bool IsInitialized { get; set; }

		public void Update()
		{
			if (!IsInitialized)
			{
				IsInitialized = true;
			}
		}

		public void Draw()
		{

		}
	}
}
