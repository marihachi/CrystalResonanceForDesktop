using CrystalResonanceDesktop.Data.Controls.Interface;
using DxSharp;
using DxSharp.Data.Enum;
using DxSharp.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CrystalResonanceDesktop.Data.Control
{
	/// <summary>
	/// メニューコントロールを表します。
	/// </summary>
	public class Menu : IBasePoint, IControl
	{
		/// <summary>
		/// 新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="location"></param>
		/// <param name="itemSize"></param>
		public Menu(Point location, Size itemSize, Position basePoint, int margin, DxSharp.Data.Font font, Button.ButtonStyle normalStyle = null, Button.ButtonStyle hoverStyle = null, Button.ButtonStyle activeStyle = null)
		{
			Location = location;
			ItemSize = itemSize;
			Size = new Size(itemSize.Width, 0);
			BasePoint = basePoint;
			Margin = margin;
			Font = font;
			NormalStyle = normalStyle;
			HoverStyle = hoverStyle;
			ActiveStyle = activeStyle;
		}

		private List<Button> items { get; set; } = new List<Button>();

		/// <summary>
		/// メニューの位置を取得または設定します。
		/// </summary>
		public Point Location { get; set; }

		/// <summary>
		/// 項目単体のサイズを取得します。
		/// </summary>
		public Size ItemSize { get; private set; }

		/// <summary>
		/// メニュー全体のサイズを取得します。
		/// </summary>
		public Size Size { get; private set; }

		/// <summary>
		/// 項目同士の余白を取得または設定します。
		/// </summary>
		public int Margin { get; set; }

		/// <summary>
		/// メニュー全体で使用するフォント
		/// </summary>
		public DxSharp.Data.Font Font { get; set; }

		/// <summary>
		/// 全般的な項目の通常時のスタイルを取得または設定します。
		/// </summary>
		public Button.ButtonStyle NormalStyle { get; set; }

		/// <summary>
		/// 全般的な項目のホバー時のスタイルを取得または設定します。
		/// </summary>
		public Button.ButtonStyle HoverStyle { get; set; }

		/// <summary>
		/// 全般的な項目のアクティブ時のスタイルを取得または設定します。
		/// </summary>
		public Button.ButtonStyle ActiveStyle { get; set; }

		/// <summary>
		/// 画面上の基準位置を取得または設定します。
		/// </summary>
		public Position BasePoint { get; set; }

		private Point internalLocation(int index)
		{
			var location = Location;
			location += new Size(0, index * (ItemSize.Height + Margin));

			return this.GetAbsoluteLocation(location, SystemCore.Instance.WindowSize, ItemSize);
		}

		/// <summary>
		/// このコントロールに項目を追加します。
		/// </summary>
		/// <param name="text">文字列</param>
		/// <param name="normalStyle">通常状態で使用されるスタイル</param>
		/// <param name="activeStyle">クリックされた状態で使用されるスタイル</param>
		/// <param name="hoverStyle">マウスポインタが項目上ある状態で使用されるスタイル</param>
		public void Add(string text, Button.ButtonStyle normalStyle = null, Button.ButtonStyle hoverStyle = null, Button.ButtonStyle activeStyle = null)
		{
			var button = new Button(
				text,
				ItemSize,
				Position.LeftTop,
				new Point(0, 0), // Draw()呼び出し時に設定されるので仮の値を設定
				Font,
				normalStyle ?? NormalStyle,
				hoverStyle ?? HoverStyle,
				activeStyle ?? ActiveStyle);
			
			button.Click += buttonClick;

			if(items.Count == 0)
				Size = new Size(ItemSize.Width, Size.Height + ItemSize.Height);
			else
				Size = new Size(ItemSize.Width, Size.Height + ItemSize.Height + Margin);

			items.Add(button);
		}

		private void buttonClick(object sender, EventArgs e)
		{
			var target = (Button)sender;
			OnItemClick(new MenuEventArgs(items.IndexOf(target)));
		}

		/// <summary>
		/// メニューに関したイベントを処理するメソッドを表します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void MenuEventHandler(object sender, MenuEventArgs e);

		/// <summary>
		/// 項目がクリックされたときに発生します。
		/// </summary>
		public event MenuEventHandler ItemClick;

		/// <summary>
		/// 項目の ItemClick イベントを発生させます。
		/// </summary>
		/// <param name="e">イベント情報</param>
		protected virtual void OnItemClick(MenuEventArgs e)
		{
			if (ItemClick != null)
				ItemClick(this, e);
		}

		/// <summary>
		/// 更新します。
		/// </summary>
		public void Update()
		{
			items.ForEach((item) =>
			{
				item.Update();
			});
		}

		/// <summary>
		/// 描画します。
		/// </summary>
		public void Draw()
		{
			items.ForEach((item) =>
			{
				item.Location = internalLocation(items.IndexOf(item));
				item.Draw();
			});
		}

		/// <summary>
		/// Menuのイベント情報を表します。
		/// </summary>
		public class MenuEventArgs : EventArgs
		{
			public MenuEventArgs(int itemIndex)
			{
				ItemIndex = itemIndex;
			}

			public int ItemIndex { get; private set; }
		}
	}
}
