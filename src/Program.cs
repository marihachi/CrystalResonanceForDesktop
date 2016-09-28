using DxSharp;
using DxSharp.Data;
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
			using (var core = SystemCore.Initialize(new Size(1280, 720), Color.FromArgb(82, 195, 202), "Crystal Resonance Desktop"))
			{
				try
				{
					var sceneManager = SceneStorage.Instance;

					sceneManager.TargetScene = sceneManager.FindByName("Title");

					FontStorage.Instance.Add("メイリオ16", new DxSharp.Data.Font("メイリオ", 16));
					FontStorage.Instance.Add("メイリオ20", new DxSharp.Data.Font("メイリオ", 20));

					while (core.Update())
					{
						sceneManager.Update();
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
