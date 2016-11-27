using System.Drawing;
using System.Windows.Forms;

namespace CrystalResonanceDesktop.Data.Control
{
	/// <summary>
	/// スワイプもしくはマウスでスクロール可能なリスト状のコントロールを表します
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SwipeableListControl<T> : ScrollableListControl<T> where T : Control
	{
		public SwipeableListControl(Point location, int padding, Color borderColor, Size size, Orientation orientation = Orientation.Vertical, Control parentControl = null)
			: base(location, padding, borderColor, size, orientation, parentControl)
		{

		}

		/// <summary>
		/// 状態を更新します
		/// </summary>
		public override void Update()
		{
			base.Update();
		}

		/// <summary>
		/// スクロールバーを描画します
		/// </summary>
		protected override void DrawScrollBar()
		{

		}
	}
}
