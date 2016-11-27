using CrystalResonanceDesktop.Utility;
using DxLibDLL;
using DxSharp;
using DxSharp.Data;
using DxSharp.Data.Enum;
using DxSharp.Storage;
using DxSharp.Utility;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrystalResonanceDesktop
{
	class Program
	{
		static void Main(string[] args)
		{
			var fonts = FontStorage.Instance;
			var inputs = Input.Instance;

			using (var core = SystemCore.Initialize(new Size(1280, 720), Color.FromArgb(82, 195, 202), "Crystal Resonance Desktop", () => {
				DX.SetAlwaysRunFlag(1);
				DX.SetWaitVSyncFlag(0);
			}))
			{
				try
				{
					var sceneManager = SceneStorage.Instance;

					sceneManager.TargetScene = sceneManager.FindByName("Title");

					fonts.Add("メイリオ16", new DxSharp.Data.Font("メイリオ", 16));
					fonts.Add("メイリオ20", new DxSharp.Data.Font("メイリオ", 20));

					while (core.Update())
					{
						if (inputs.GetKey(KeyType.LShift).InputTime > 0 && inputs.GetKey(KeyType.D).InputTime == 1)
							core.IsShowDebugImageBorder ^= true;

						sceneManager.Update();
						FpsHelper.Instance.Wait();
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(
						$"エラーが発生しました。お手数ですが開発元に報告してください。\r\n\r\n{ExceptionContents(ex, "")}",
						$"実行時エラー({ex.GetType()})", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private static string ExceptionContents(Exception ex, string message)
		{
			message += $"[内容]:\r\n";
			message += $"{ex.Message}\r\n\r\n";
			message += $"[スタックトレース]:\r\n";
			message += $"{ex.StackTrace}\r\n";

			if (ex.InnerException != null)
			{
				message += "[内部例外]: {\r\n";
				message += ExceptionContents(ex.InnerException, message);
				message += "}\r\n";
				return message;
			}

			return message;
		}
	}
}
