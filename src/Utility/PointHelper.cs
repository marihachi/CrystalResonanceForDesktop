using System.Drawing;

namespace CrystalResonanceDesktop.Utility
{
	/// <summary>
	/// 座標計算に関する機能を提供します。
	/// </summary>
	public class PointHelper
	{
		public static PointHelper Instance { get; } = new PointHelper();

		private PointHelper() { }

		/// <summary>
		/// Point の除算を実行します。(p1 / p2)
		/// </summary>
		/// <param name="p2">nullが渡されると(2, 2)で除算されます。</param>
		public Size Divide(Size p1, Size? p2 = null)
		{
			Size p = p2 ?? new Size(2, 2);
			return new Size(p1.Width / p.Width, p1.Height / p.Height);
		}
	}
}
