using DxLibDLL;
using DxSharp;
using DxSharp.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CrystalResonanceDesktop.Data.Control
{
	/// <summary>
	/// リスト状のコントロールを表します
	/// </summary>
	public class ListControl<T> : Control where T : Control
	{
		public ListControl(Point location, int padding, Orientation orientation = Orientation.Vertical, Control parentControl = null)
			: base(location, parentControl)
		{
			Padding = padding;
			Orientation = orientation;
		}

		public ListControl(Point location, int padding, Size size, Orientation orientation = Orientation.Vertical, Control parentControl = null)
			: base(location, parentControl)
		{
			Padding = padding;
			Size = size;
			IsAutoSize = false;
			Orientation = orientation;
		}

		public override Size Size
		{
			get
			{
				if (IsAutoSize)
				{
					var paddingAll = Padding * (Items.Count * 2);

					// 垂直方向
					if (Orientation == Orientation.Vertical)
					{
						return new Size(Items.Max(i => i.Size.Width) + Padding * 2, Items.Sum(i => i.Size.Height) + paddingAll);
					}
					// 水平方向
					else if (Orientation == Orientation.Horizontal)
					{
						return new Size(Items.Sum(i => i.Size.Width) + paddingAll, Items.Max(i => i.Size.Height) + Padding * 2);
					}
					else
						throw new InvalidOperationException("Invalid ListControl.Orientation");
				}
				else
					return SizeReal;
			}
			set { SizeReal = value; }
		}
		private Size SizeReal = Size.Empty;

		/// <summary>
		/// このコントロールが自動的にサイズを決定するかどうかを示す値を取得または設定します
		/// </summary>
		public bool IsAutoSize { get; set; } = true;

		/// <summary>
		/// 項目同士の余白を取得または設定します
		/// </summary>
		public int Padding { get; set; }

		/// <summary>
		/// 配置方向を取得または設定します
		/// </summary>
		public Orientation Orientation { get; set; }

		/// <summary>
		/// 属しているコントロールの一覧を取得します
		/// </summary>
		public List<T> Items { get; private set; } = new List<T>();

		/// <summary>
		/// 状態を更新します
		/// </summary>
		public override void Update()
		{
			foreach (var itemIndex in Enumerable.Range(0, Items.Count))
			{
				var item = Items[itemIndex];

				// 現在の項目までの項目の一覧
				var untilItems = Items.GetRange(0, itemIndex);

				var untilPadding = Padding * (untilItems.Count * 2 + 1);

				// 垂直方向
				if (Orientation == Orientation.Vertical)
				{
					item.Location = new Point(Padding, untilItems.Sum(i => i.Size.Height) + untilPadding);
				}
				// 水平方向
				else if (Orientation == Orientation.Horizontal)
				{
					item.Location = new Point(untilItems.Sum(i => i.Size.Width) + untilPadding, Padding);
				}
				else
					throw new InvalidOperationException("Invalid ListControl.Orientation");

				if (item.ParentControl != this)
					item.ParentControl = this;

				item.Update();
			}
		}

		public virtual void DrawItems()
		{
			foreach (var item in Items)
			{
				item.Draw();
			}
		}

		public virtual void DrawBorder()
		{
			var a = new Box(AbsoluteLocation, Size, Color.White, false);
			a.Draw();
		}

		/// <summary>
		/// コントロールを描画します
		/// </summary>
		public override void Draw()
		{
			var core = SystemCore.Instance;

			DX.SetDrawArea(AbsoluteLocation.X, AbsoluteLocation.Y, AbsoluteLocation.X + Size.Width, AbsoluteLocation.Y + Size.Height);
			DrawItems();
			DX.SetDrawArea(0, 0, core.WindowSize.Width, core.WindowSize.Height);

			DrawBorder();
		}
	}
}
