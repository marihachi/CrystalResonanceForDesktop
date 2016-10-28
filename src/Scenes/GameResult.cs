using DxSharp.Data;

namespace CrystalResonanceDesktop.Scenes
{
	public class GameResult : IScene
	{
		private bool IsInitialized { get; set; }

		private void Initialize()
		{

		}

		public void Update()
		{
			if (!IsInitialized)
			{
				IsInitialized = true;

				Initialize();
			}
		}

		public void Draw()
		{

		}
	}
}
