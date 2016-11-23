using DxLibDLL;
using DxSharp.Utility;
using System;
using System.Drawing;

namespace CrystalResonanceDesktop.Data.Control
{
	/// <summary>
	/// ボタンコントロールを表します。
	/// </summary>
	public class ButtonControl : Control
	{
		/// <summary>
		/// 新しいインスタンスを初期化します。
		/// </summary>
		public ButtonControl(Point location, Size size, string text, ButtonStyle style, Control parentControl = null)
			: base(location, parentControl)
		{
			Text = text;
			Size = size;
			Style = style;
		}

		public override Size Size { get; set; }

		/// <summary>
		/// マウスポインタがこのボタン上にあるかどうかを取得します。
		/// </summary>
		public bool IsHover
		{
			get
			{
				var bounds = new Rectangle(AbsoluteLocation, Size);
				return bounds.Contains(Input.Instance.Mouse.PointerLocation);
			}
		}

		/// <summary>
		/// このボタンが押されているかどうかを取得します。
		/// </summary>
		public bool IsMouseDown
		{
			get
			{
				return !isMouseDownOld ? IsHover && Input.Instance.Mouse.LeftInputTime == 1 : Input.Instance.Mouse.LeftInputTime >= 1;
			}
		}
		private bool isMouseDownOld { get; set; } = false;

		/// <summary>
		/// 文字列を取得または設定します。
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// 見た目を取得または設定します。
		/// </summary>
		public ButtonStyle Style { get; set; }

		/// <summary>
		/// クリックされたときに発生します。
		/// </summary>
		public event EventHandler Click;

		/// <summary>
		/// Click イベントを発生させます。
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnClick(EventArgs e)
		{
			Click?.Invoke(this, e);
		}

		/// <summary>
		/// 更新します。
		/// </summary>
		public override void Update()
		{
			if (isMouseDownOld && !IsMouseDown && IsHover)
				OnClick(new EventArgs());

			isMouseDownOld = IsMouseDown;
		}

		/// <summary>
		/// 描画します。
		/// </summary>
		public override void Draw()
		{
			var style = IsMouseDown ? Style.ActiveStyle : (IsHover ? Style.HoverStyle : Style.NormalStyle);

			if (style.BackImage == null)
			{
				DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, style.BackColor.A);
				DX.DrawBox(AbsoluteLocation.X, AbsoluteLocation.Y, (AbsoluteLocation + Size).X, (AbsoluteLocation + Size).Y, (uint)style.BackColor.ToArgb(), 1);

				DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, style.FrameColor.A);
				DX.DrawBox(AbsoluteLocation.X, AbsoluteLocation.Y, (AbsoluteLocation + Size).X, (AbsoluteLocation + Size).Y, (uint)style.FrameColor.ToArgb(), 0);
			}
			else
			{
				style.BackImage.Update();
				style.BackImage.Draw(AbsoluteLocation);
			}

			var textSize = new Size(DX.GetDrawStringWidth(Text, Text.Length), DX.GetFontSizeToHandle(Style.Font.Handle));
			var textPoint = new Point(new Size(AbsoluteLocation) + new Size(Size.Width / 2 - (textSize.Width + 10) / 2, Size.Height / 2 - textSize.Height / 2));

			DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, style.TextColor.A);
			DX.DrawStringToHandle(textPoint.X, textPoint.Y, Text, (uint)style.TextColor.ToArgb(), Style.Font.Handle, (uint)style.FrameColor.ToArgb());
			DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);
		}

		/// <summary>
		/// ボタンコントロールのスタイルを表します
		/// </summary>
		public class ButtonStyle
		{
			/// <summary>
			/// 新しいインスタンスを初期化します
			/// </summary>
			public ButtonStyle(DxSharp.Data.Font font, ButtonStyleStatus normalStyle, ButtonStyleStatus hoverStyle, ButtonStyleStatus activeStyle)
			{
				Font = font;
				NormalStyle = normalStyle;
				HoverStyle = hoverStyle;
				ActiveStyle = activeStyle;
			}

			/// <summary>
			/// 使用するフォント
			/// </summary>
			public DxSharp.Data.Font Font { get; set; }

			/// <summary>
			/// 全般的な項目の通常時のスタイルを取得または設定します
			/// </summary>
			public ButtonStyleStatus NormalStyle { get; set; }

			/// <summary>
			/// 全般的な項目のホバー時のスタイルを取得または設定します
			/// </summary>
			public ButtonStyleStatus HoverStyle { get; set; }

			/// <summary>
			/// 全般的な項目のアクティブ時のスタイルを取得または設定します
			/// </summary>
			public ButtonStyleStatus ActiveStyle { get; set; }
		}

		/// <summary>
		/// ボタンコントロールの状態毎のスタイルを表します
		/// </summary>
		public class ButtonStyleStatus
		{
			/// <summary>
			/// 新しいインスタンスを初期化します
			/// </summary>
			public ButtonStyleStatus(Color frameColor, Color backColor, Color textColor, DxSharp.Data.Image backImage = null)
			{
				FrameColor = frameColor;
				BackColor = backColor;
				BackImage = backImage;
				TextColor = textColor;
			}

			/// <summary>
			/// 枠色を取得または設定します
			/// </summary>
			public Color FrameColor { get; set; }

			/// <summary>
			/// 背景色を取得または設定します
			/// </summary>
			public Color BackColor { get; set; }

			/// <summary>
			/// 背景画像を取得または設定します
			/// </summary>
			public DxSharp.Data.Image BackImage { get; set; }

			/// <summary>
			/// 文字色を取得または設定します
			/// </summary>
			public Color TextColor { get; set; }
		}
	}
}
