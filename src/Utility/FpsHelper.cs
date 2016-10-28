using DxLibDLL;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace CrystalResonanceDesktop.Utility
{
	/// <summary>
	/// フレーム待ちに関する機能を提供します。
	/// </summary>
	public class FpsHelper
	{
		public static FpsHelper Instance { get; } = new FpsHelper();

		private FpsHelper()
		{
			Frames = new List<int>(Enumerable.Repeat(0, FrameRate));
		}

		/// <summary>
		/// フレームレート
		/// </summary>
		public int FrameRate { get; set; } = 60;

		/// <summary>
		/// 平均値
		/// </summary>
		private double Average { get; set; } = 0.0;

		/// <summary>
		/// 現在のカウント
		/// </summary>
		private int FpsCount { get; set; } = 0;

		/// <summary>
		/// １回目の時間
		/// </summary>
		private int FirstTime { get; set; } = 0;

		/// <summary>
		/// ２回目の時間
		/// </summary>
		private int SecondTime { get; set; } = 0;

		/// <summary>
		/// 待つべき時間
		/// </summary>
		private int WaitTime { get; set; }

		/// <summary>
		/// 平均を計算するため60回の1周時間を記録
		/// </summary>
		private List<int> Frames { get; set; }

		/// <summary>
		/// フレーム待ち時間を制御します。
		/// </summary>
		public void Wait(bool isWait = true)
		{
			if (FpsCount == 0)
				if (FirstTime == 0)
					WaitTime = 0;
				else
					WaitTime = FirstTime + 1000 - DX.GetNowCount();
			else
				WaitTime = (int)(FirstTime + FpsCount * (1000.0 / FrameRate)) - DX.GetNowCount();

			if (isWait)
				if (WaitTime > 0)
					Thread.Sleep(WaitTime);

			var nowTime = DX.GetNowCount();

			if (FpsCount == 0)
				FirstTime = nowTime;

			if (SecondTime == 0)
				SecondTime = FirstTime;

			Frames[FpsCount] = nowTime - SecondTime;

			SecondTime = nowTime;

			if (FpsCount == FrameRate - 1)
				Average = Frames.GetRange(0, FrameRate).Average();

			FpsCount++;

			if (FpsCount == FrameRate)
				FpsCount = 0;

			DX.DrawString(0, 0, $"{(1000 / Average):00.0}", 0);
		}

		/// <summary>
		/// FPS値を描画します。
		/// </summary>
		public void Draw(DxSharp.Data.Font font, Color textColor)
		{
			if (Average != 0)
				font.Draw(new Point(0, 0), $"{(1000 / Average):00.0}", textColor);
		}
	}
}
