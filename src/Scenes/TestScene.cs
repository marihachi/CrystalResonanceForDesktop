using DxSharp;
using DxSharp.Data;
using DxSharp.Storage;
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
			FontStorage.Instance.Item("メイリオ16").Draw(new Point(0, 100), $"カウント値: {count}", Color.Black);
		}
	}
}
