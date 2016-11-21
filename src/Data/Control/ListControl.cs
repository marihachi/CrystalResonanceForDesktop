using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CrystalResonanceDesktop.Data.Control
{
	/// <summary>
	/// リスト状のコントロールを表します
	/// </summary>
	public class ListControl<T> : Control where T: Control
	{
		/// <summary>
		/// 項目同士の余白を取得または設定します
		/// </summary>
		public int Padding { get; set; }

		/// <summary>
		/// 配置方向を取得または設定します
		/// </summary>
		public Orientation Orientation { get; set; } = Orientation.Vertical;

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
					item.Location.Offset(Padding, untilItems.Sum(i => i.Size.Height) + untilPadding);
				}
				// 水平方向
				else if (Orientation == Orientation.Horizontal)
				{
					item.Location.Offset(untilItems.Sum(i => i.Size.Width) + untilPadding, Padding);
				}
				else
					throw new InvalidOperationException("Invalid ListControl.Orientation");

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
