using DxSharp;
using DxSharp.Storage;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrystalResonanceDesktop
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var core = SystemCore.Initialize(new Size(1280, 720), "Crystal Resonance Desktop"))
			{
				try
				{
					var sceneManager = new SceneStorage();
					sceneManager.TargetScene = sceneManager.FindByName("TestScene");

					SystemCore.Instance.FontStorage.Add("てすと", "メイリオ", 16);

					while (core.Update())
					{
						sceneManager.Update();
						sceneManager.Draw();

						Utility.FpsHelper.Instance.Wait();
					}
				}
				catch(Exception ex)
				{
					MessageBox.Show($"[内容]:\r\n{ex.Message}\r\n\r\n[スタックトレース]:\r\n{ex.StackTrace}", $"実行時エラー({ex.GetType()})", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
			}
		}
	}
}
