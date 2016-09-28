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
			_Frames = new List<int>(Enumerable.Repeat(0, FrameRate));
		}

		/// <summary>
		/// 平均値
		/// </summary>
		private double _Average { get; set; } = 0.0;

		/// <summary>
		/// フレームレート
		/// </summary>
		public int FrameRate { get; set; } = 60;

		/// <summary>
		/// 現在のカウント
		/// </summary>
		private int _FpsCount { get; set; } = 0;

		/// <summary>
		/// １回目の時間
		/// </summary>
		private int _FirstTime { get; set; } = 0;

		/// <summary>
		/// ２回目の時間
		/// </summary>
		private int _SecondTime { get; set; } = 0;

		/// <summary>
		/// 待つべき時間
		/// </summary>
		private int _WaitTime { get; set; }

		/// <summary>
		/// 平均を計算するため60回の1周時間を記録
		/// </summary>
		private List<int> _Frames { get; set; }

		private bool _IsInit { get; set; } = true;

		public bool IsDrawFrame { get; private set; } = true;

		public double NextDrawMoment { get; private set; }
		public double NextDrawFrame { get; private set; }

		/// <summary>
		/// フレーム待ち時間を制御します。
		/// </summary>
		public void Wait(bool isWait = true)
		{
			if (_FpsCount == 0)
				if (_FirstTime == 0)
					_WaitTime = 0;
				else
					_WaitTime = _FirstTime + 1000 - DX.GetNowCount();
			else
				_WaitTime = (int)(_FirstTime + _FpsCount * (1000.0 / FrameRate)) - DX.GetNowCount();

			if (isWait)
				if (_WaitTime > 0)
					Thread.Sleep(_WaitTime);

			var nowTime = DX.GetNowCount();

			if (_FpsCount == 0)
				_FirstTime = nowTime;

			if (_SecondTime == 0)
				_SecondTime = _FirstTime;

			_Frames[_FpsCount] = nowTime - _SecondTime;

			_SecondTime = nowTime;

			if (_FpsCount == FrameRate - 1)
				_Average = _Frames.GetRange(0, FrameRate).Average();

			_FpsCount++;

			if (_FpsCount == FrameRate)
				_FpsCount = 0;

			DX.DrawString(0, 0, $"{(1000 / _Average):00.0}", 0);
		}

		/// <summary>
		/// FPS値を描画します。
		/// </summary>
		public void Draw(DxSharp.Data.Font font, Color textColor)
		{
			if (_Average != 0)
				font.Draw(new Point(0, 0), $"{(1000 / _Average):00.0}", textColor);
		}
	}
}
