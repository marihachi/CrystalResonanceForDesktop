using System;
using System.Drawing;
using static CrystalResonanceDesktop.Data.Control.ButtonControl;

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
		public MenuControl(Point location, int padding, Size itemSize, ButtonStyle defaultStyle = null)
			: base(location, padding)
		{
			ItemSize = itemSize;
			Style = defaultStyle;
		}

		/// <summary>
		/// 項目単体のサイズを取得します
		/// </summary>
		public Size ItemSize { get; private set; }

		/// <summary>
		/// 全般的な項目の通常時のスタイルを取得または設定します
		/// </summary>
		public ButtonStyle Style { get; set; }

		/// <summary>
		/// このコントロールに項目を追加します
		/// </summary>
		/// <param name="text">文字列</param>
		/// <param name="style">使用されるスタイル</param>
		public void Add(string text, EventHandler click, ButtonStyle style = null)
		{
			var button = new ButtonControl(
				new Point(0, 0),
				ItemSize,
				text,
				style ?? Style);

			Items.Add(button);

			button.Click += click;
		}
	}
}
