using System;
using System.Drawing;

namespace CrystalResonanceDesktop.Data.Control
{
	/// <summary>
	/// メニューコントロールを表します。
	/// </summary>
	public class MenuControl : ListControl<ButtonControl>
	{
		/// <summary>
		/// 新しいインスタンスを初期化します
		/// </summary>
		/// <param name="location"></param>
		/// <param name="itemSize"></param>
		public MenuControl(Point location, Size itemSize, int margin, DxSharp.Data.Font font, ButtonControl.ButtonStyle normalStyle = null, ButtonControl.ButtonStyle hoverStyle = null, ButtonControl.ButtonStyle activeStyle = null)
		{
			Location = location;
			ItemSize = itemSize;
			Size = new Size(itemSize.Width, 0);
			Margin = margin;
			Font = font;
			NormalStyle = normalStyle;
			HoverStyle = hoverStyle;
			ActiveStyle = activeStyle;
		}

		/// <summary>
		/// 項目単体のサイズを取得します
		/// </summary>
		public Size ItemSize { get; private set; }

		/// <summary>
		/// 項目同士の余白を取得または設定します
		/// </summary>
		public int Margin { get; set; }

		/// <summary>
		/// メニュー全体で使用するフォント
		/// </summary>
		public DxSharp.Data.Font Font { get; set; }

		/// <summary>
		/// 全般的な項目の通常時のスタイルを取得または設定します
		/// </summary>
		public ButtonControl.ButtonStyle NormalStyle { get; set; }

		/// <summary>
		/// 全般的な項目のホバー時のスタイルを取得または設定します
		/// </summary>
		public ButtonControl.ButtonStyle HoverStyle { get; set; }

		/// <summary>
		/// 全般的な項目のアクティブ時のスタイルを取得または設定します
		/// </summary>
		public ButtonControl.ButtonStyle ActiveStyle { get; set; }

		/// <summary>
		/// このコントロールに項目を追加します
		/// </summary>
		/// <param name="text">文字列</param>
		/// <param name="normalStyle">通常状態で使用されるスタイル</param>
		/// <param name="activeStyle">クリックされた状態で使用されるスタイル</param>
		/// <param name="hoverStyle">マウスポインタが項目上ある状態で使用されるスタイル</param>
		public void Add(string text, ButtonControl.ButtonStyle normalStyle = null, ButtonControl.ButtonStyle hoverStyle = null, ButtonControl.ButtonStyle activeStyle = null)
		{
			var button = new ButtonControl(
				text,
				ItemSize,
				new Point(0, 0), // Draw()呼び出し時に設定されるので仮の値を設定
				Font,
				normalStyle ?? NormalStyle,
				hoverStyle ?? HoverStyle,
				activeStyle ?? ActiveStyle);
			
			button.Click += buttonClick;

			if(Items.Count == 0)
				Size = new Size(ItemSize.Width, Size.Height + ItemSize.Height);
			else
				Size = new Size(ItemSize.Width, Size.Height + ItemSize.Height + Margin);

			Items.Add(button);
		}

		private void buttonClick(object sender, EventArgs e)
		{
			var target = (ButtonControl)sender;
			OnItemClick(new MenuEventArgs(Items.IndexOf(target)));
		}

		/// <summary>
		/// 項目がクリックされたときに発生します
		/// </summary>
		public event EventHandler<MenuEventArgs> ItemClick;

		/// <summary>
		/// 項目の ItemClick イベントを発生させます
		/// </summary>
		/// <param name="e">イベント情報</param>
		protected virtual void OnItemClick(MenuEventArgs e)
		{
			ItemClick?.Invoke(this, e);
		}

		/// <summary>
		/// Menuのイベント情報を表します
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
