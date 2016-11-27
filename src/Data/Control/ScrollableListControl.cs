using DxSharp.Utility;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CrystalResonanceDesktop.Data.Control
{
	/// <summary>
	/// マウスでスクロール可能なリスト状のコントロールを表します
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ScrollableListControl<T> : ListControl<T> where T : Control
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
		/// スクロール位置を更新します
		/// </summary>
		public virtual void UpdateScrollLocation()
		{
			if (Input.Instance.Mouse.WheelValue != 0)
			{
				var old = ScrollLocation;

				if (Orientation == Orientation.Vertical)
				{
					var y = ScrollLocation.Y - Input.Instance.Mouse.WheelValue * 17;

					if (Input.Instance.Mouse.WheelValue > 0 && ScrollLocation.Y != 0) // 上スクロール
					{
						ScrollLocation = (y > 0) ? new Point(ScrollLocation.X, y) : new Point(ScrollLocation.X, 0);

						OnScroll(new ScrollEventArgs(old, ScrollLocation));
					}

					if (Input.Instance.Mouse.WheelValue < 0 && ScrollLocation.Y != ItemsSize.Height - 1 - Size.Height) // 下スクロール
					{
						ScrollLocation = (Size.Height + y < ItemsSize.Height - 1) ? new Point(ScrollLocation.X, y) : new Point(ScrollLocation.X, ItemsSize.Height - 1 - Size.Height);

						OnScroll(new ScrollEventArgs(old, ScrollLocation));
					}
				}
				else if (Orientation == Orientation.Horizontal)
				{
					var x = ScrollLocation.X - Input.Instance.Mouse.WheelValue * 17;

					if (Input.Instance.Mouse.WheelValue > 0 && ScrollLocation.X != 0) // 左スクロール
					{
						ScrollLocation = (x > 0) ? new Point(x, ScrollLocation.Y) : new Point(0, ScrollLocation.Y);

						OnScroll(new ScrollEventArgs(old, ScrollLocation));
					}

					if (Input.Instance.Mouse.WheelValue < 0 && ScrollLocation.X != ItemsSize.Width - 1 - Size.Width) // 右スクロール
					{
						ScrollLocation = (Size.Width + x < ItemsSize.Width - 1) ? new Point(x, ScrollLocation.Y) : new Point(ItemsSize.Width - 1 - Size.Width, ScrollLocation.Y);

						OnScroll(new ScrollEventArgs(old, ScrollLocation));
					}
				}
				else
					throw new InvalidOperationException("Invalid ListControl.Orientation");
			}
		}

		/// <summary>
		/// 項目の位置を更新します
		/// </summary>
		/// <param name="item"></param>
		/// <param name="itemIndex"></param>
		protected override void UpdateItemLocation(T item, int itemIndex)
		{
			// 現在の項目までの項目の一覧
			var untilItems = Items.GetRange(0, itemIndex);

			var untilPadding = Padding * (untilItems.Count * 2 + 1);

			// 垂直方向
			if (Orientation == Orientation.Vertical)
			{
				item.Location = new Point(Padding, untilItems.Sum(i => i.Size.Height) + untilPadding - ScrollLocation.Y);
			}
			// 水平方向
			else if (Orientation == Orientation.Horizontal)
			{
				item.Location = new Point(untilItems.Sum(i => i.Size.Width) + untilPadding - ScrollLocation.X, Padding);
			}
			else
				throw new InvalidOperationException("Invalid ListControl.Orientation");
		}

		/// <summary>
		/// 状態を更新します
		/// </summary>
		public override void Update()
		{
			UpdateScrollLocation();

			base.Update();
		}

		/// <summary>
		/// スクロールバーを描画します
		/// </summary>
		protected virtual void DrawScrollBar()
		{
			// TODO: そのうちかくよ
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
