﻿using System;
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
		public ListControl(Point location, int padding, Orientation orientation = Orientation.Vertical)
			: base(location)
		{
			Padding = padding;
			Orientation = orientation;
		}

		public override Size Size
		{
			get
			{
				var paddingAll = Padding * (Items.Count * 2);

				// 垂直方向
				if (Orientation == Orientation.Vertical)
				{
					return new Size(Items.Max(i => i.Size.Width) + Padding, Items.Sum(i => i.Size.Height) + paddingAll);
				}
				// 水平方向
				else if (Orientation == Orientation.Horizontal)
				{
					return new Size(Items.Sum(i => i.Size.Width) + paddingAll, Items.Max(i => i.Size.Height) + Padding);
				}
				else
					throw new InvalidOperationException("Invalid ListControl.Orientation");
			}
		}

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

				item.ParentControl = this;

				item.Update();
			}
		}

		/// <summary>
		/// コントロールを描画します
		/// </summary>
		public override void Draw()
		{
			foreach (var item in Items)
			{
				item.Draw();
			}
		}
	}
}