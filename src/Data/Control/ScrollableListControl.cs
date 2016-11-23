using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrystalResonanceDesktop.Data.Control
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ScrollableListControl<T> : ListControl<T> where T: Control
	{
		public ScrollableListControl(Point location, int padding, Size frameSize, Orientation orientation = Orientation.Vertical, Control parentControl = null)
			: base(location, padding, orientation, parentControl)
		{
			FrameSize = frameSize;
		}

		public Size FrameSize { get; set; }

		public Point ScrollLocation { get; set; }

		/// <summary>
		/// コントロールがスクロールされたときに発生します
		/// </summary>
		public event EventHandler<EventArgs> Scroll;

		/// <summary>
		/// 項目の Scroll イベントを発生させます
		/// </summary>
		/// <param name="e">イベント情報</param>
		protected virtual void OnScroll(EventArgs e)
		{
			Scroll?.Invoke(this, e);
		}

		/// <summary>
		/// 状態を更新します
		/// </summary>
		public override void Update()
		{
			base.Update();
		}

		/// <summary>
		/// コントロールを描画します
		/// </summary>
		public override void Draw()
		{
			base.Draw();
		}
	}
}
