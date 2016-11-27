using DxSharp.Data;
using DxSharp.Utility;
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
		public ListControl(Point location, int padding, Color borderColor, Orientation orientation = Orientation.Vertical, Control parentControl = null)
			: base(location, parentControl)
		{
			Padding = padding;
			BorderColor = borderColor;
			Orientation = orientation;
		}

		public ListControl(Point location, int padding, Color borderColor, Size size, Orientation orientation = Orientation.Vertical, Control parentControl = null)
			: base(location, parentControl)
		{
			Padding = padding;
			BorderColor = borderColor;
			Size = size;
			IsAutoSize = false;
			Orientation = orientation;
		}

		/// <summary>
		/// サイズを取得または設定します
		/// ※IsAutoSize が True のときは設定されたサイズは無視され、 ItemsSize の値が返されます
		/// </summary>
		public override Size Size
		{
			get { return IsAutoSize ? ItemsSize : SizeReal; }
			set { SizeReal = value; }
		}
		private Size SizeReal = Size.Empty;

		/// <summary>
		/// このコントロールが自動的にサイズを決定するかどうかを示す値を取得または設定します
		/// </summary>
		public bool IsAutoSize { get; set; } = true;

		/// <summary>
		/// 
		/// </summary>
		public virtual Size ItemsSize
		{
			get
			{
				var paddingAll = Padding * (Items.Count * 2);

				// 垂直方向
				if (Orientation == Orientation.Vertical)
				{
					if (Items.Count >= 1)
						return new Size(Items.Max(i => i.Size.Width) + Padding * 2, Items.Sum(i => i.Size.Height) + paddingAll);
					else
						return Size.Empty;
				}
				// 水平方向
				else if (Orientation == Orientation.Horizontal)
				{
					if (Items.Count >= 1)
						return new Size(Items.Sum(i => i.Size.Width) + paddingAll, Items.Max(i => i.Size.Height) + Padding * 2);
					else
						return Size.Empty;
				}
				else
					throw new InvalidOperationException("Invalid ListControl.Orientation");
			}
		}

		/// <summary>
		/// 境界色を取得または設定します
		/// </summary>
		public Color BorderColor { get; set; }

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
		/// 項目の位置を更新します
		/// </summary>
		/// <param name="item"></param>
		/// <param name="itemIndex"></param>
		protected virtual void UpdateItemLocation(T item, int itemIndex)
		{
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
		}

		/// <summary>
		/// 状態を更新します
		/// </summary>
		public override void Update()
		{
			foreach (var itemIndex in Enumerable.Range(0, Items.Count))
			{
				var item = Items[itemIndex];

				UpdateItemLocation(item, itemIndex);

				if (item.ParentControl != this)
					item.ParentControl = this;

				item.Update();
			}
		}

		/// <summary>
		/// 項目を描画します
		/// </summary>
		protected virtual void DrawItem(T item)
		{
			item.Draw();
		}

		/// <summary>
		/// 境界を描画します
		/// </summary>
		protected virtual void DrawBorder()
		{
			var a = new Box(AbsoluteLocation, Size, BorderColor, false);
			a.Draw();
		}

		/// <summary>
		/// コントロールを描画します
		/// </summary>
		public override void Draw()
		{
			using (new DrawArea(AbsoluteLocation, Size))
			{
				foreach (var item in Items)
				{
					DrawItem(item);
				}
			}

			DrawBorder();
		}
	}
}
