using DxSharp.Utility;
using System;
using System.Diagnostics;
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
		public ScrollableListControl(Point location, int padding, Color borderColor, Size size, Orientation orientation = Orientation.Vertical, Control parentControl = null)
			: base(location, padding, borderColor, size, orientation, parentControl)
		{

		}

		public Point ScrollLocation { get; private set; } = Point.Empty;

		/// <summary>
		/// コントロールがスクロールされたときに発生します
		/// </summary>
		public event EventHandler<ScrollEventArgs> Scroll;

		/// <summary>
		/// 項目の Scroll イベントを発生させます
		/// </summary>
		/// <param name="e">イベント情報</param>
		protected virtual void OnScroll(ScrollEventArgs e)
		{
			Scroll?.Invoke(this, e);
		}

		/// <summary>
		/// 状態を更新します
		/// </summary>
		public override void Update()
		{
			base.Update();

			if (Input.Instance.Mouse.WheelValue != 0)
			{
				var old = ScrollLocation;

				if (Orientation == Orientation.Vertical)
				{
					var y = ScrollLocation.Y + Input.Instance.Mouse.WheelValue * 5;

					if (y >= 0)// TODO: ItemsSize, Size を見てスクロールが必要かを判定
					{
						ScrollLocation = new Point(ScrollLocation.X, y);

						OnScroll(new ScrollEventArgs(old, ScrollLocation));
					}
				}
				else if (Orientation == Orientation.Horizontal)
				{
					var x = ScrollLocation.X + Input.Instance.Mouse.WheelValue * 5;

					if (x >= 0)
					{
						ScrollLocation = new Point(x, ScrollLocation.Y);

						OnScroll(new ScrollEventArgs(old, ScrollLocation));
					}
				}
				else
					throw new InvalidOperationException("Invalid ListControl.Orientation");
			}
		}

		/// <summary>
		/// スクロールバーを描画します
		/// </summary>
		protected virtual void DrawScrollBar()
		{

		}

		/// <summary>
		/// コントロールを描画します
		/// </summary>
		public override void Draw()
		{
			base.Draw();
			DrawScrollBar();
		}

		public class ScrollEventArgs : EventArgs
		{
			public ScrollEventArgs(Point oldScrollLocation, Point newScrollLocation)
			{
				OldScrollLocation = oldScrollLocation;
				NewScrollLocation = newScrollLocation;
			}

			public Point OldScrollLocation { get; private set; }

			public Point NewScrollLocation { get; private set; }
		}
	}
}
