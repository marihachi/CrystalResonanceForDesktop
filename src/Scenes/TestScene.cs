using DxSharp;
using DxSharp.Data;
using System.Drawing;

namespace CrystalResonanceDesktop.Scenes
{
	class TestScene : IScene
	{
		private int count = 0;

		public void Update()
		{
			count++;
		}

		public void Draw()
		{
			SystemCore.Instance.FontStorage.Item("てすと").Draw(new Point(0, 100), $"カウント値: {count}", Color.Black);
		}
	}
}
