using CrystalResonanceDesktop.Utility;
using DxLibDLL;
using DxSharp;
using DxSharp.Data;
using DxSharp.Data.Enum;
using DxSharp.Storage;
using DxSharp.Utility;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace CrystalResonanceDesktop
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var fonts = FontStorage.Instance;
				var inputs = Input.Instance;

				if (!File.Exists(new YoutubeOggExtractor().FFmpegFilePath))
				{
					MessageBox.Show("ffmpegの実行ファイルが見つかりません。", "ランタイムエラー");
					return;
				}

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
						fonts.Add("メイリオ18", new DxSharp.Data.Font("メイリオ", 18));
						fonts.Add("メイリオ20", new DxSharp.Data.Font("メイリオ", 20));
						fonts.Add("メイリオ22", new DxSharp.Data.Font("メイリオ", 22));

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
						if (FirstException(ex) is ReflectionTypeLoadException)
							throw ex;

						MessageBox.Show(
							$"エラーが発生しました。お手数ですが開発元に報告してください。\r\n\r\n{ExceptionContents(ex, "")}",
							$"ランタイムエラー({ex.GetType()})", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}

			}
			catch (Exception ex) when (FirstException(ex) is ReflectionTypeLoadException)
			{
				var typeLoadException = FirstException(ex) as ReflectionTypeLoadException;
				var loaderExceptions = typeLoadException.LoaderExceptions;

				MessageBox.Show("必要なライブラリファイルが不足しています。", "ランタイムエラー");
			}
		}

		private static Exception FirstException(Exception ex)
		{
			if (ex.InnerException == null)
				return ex;

			return FirstException(ex.InnerException);
		}

		private static string ExceptionContents(Exception ex, string message)
		{
			message += $"[種類]:\r\n";
			message += $"{ex.GetType()}\r\n";
			message += $"[内容]:\r\n";
			message += $"{ex.Message}\r\n";
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
