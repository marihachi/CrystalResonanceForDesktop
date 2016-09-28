using DxLibDLL;
using DxSharp;
using DxSharp.Data;
using DxSharp.Data.Enum;
using DxSharp.Utility;
using System;
using System.Drawing;

namespace CrystalResonanceDesktop.Data
{
	/// <summary>
	/// ボタンコントロールのスタイルを表します
	/// </summary>
	public class ButtonStyle
	{
		/// <summary>
		/// 新しいインスタンスを初期化します
		/// </summary>
		public ButtonStyle(Color frameColor, Color backColor, Color textColor, DxSharp.Data.Image backImage = null)
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

	/// <summary>
	/// ボタンコントロールを表します。
	/// </summary>
	public class Button : IBasePoint
	{
		/// <summary>
		/// 新しいインスタンスを初期化します。
		/// </summary>
		public Button(string text, Size size, Position basePoint, Point location, DxSharp.Data.Font font, ButtonStyle normalStyle, ButtonStyle hoverStyle, ButtonStyle activeStyle)
		{
			Text = text;
			Size = size;
			BasePoint = basePoint;
			Location = location;
			Font = font;
			NormalStyle = normalStyle;
			HoverStyle = hoverStyle;
			ActiveStyle = activeStyle;
		}

		/// <summary>
		/// マウスポインタがこのボタン上にあるかどうかを取得します。
		/// </summary>
		public bool IsHover
		{
			get
			{
				var bounds = new Rectangle(AbsoluteLocation, Size);
				return bounds.Contains(Input.Instance.MousePos);
			}
		}

		/// <summary>
		/// このボタンが押されているかどうかを取得します。
		/// </summary>
		public bool IsMouseDown
		{
			get
			{
				return !isMouseDownOld ? IsHover && Input.Instance.MouseLeft == 1 : Input.Instance.MouseLeft >= 1;
			}
		}

		private bool isMouseDownOld { get; set; } = false;

		/// <summary>
		/// 画面上の基準位置を取得または設定します。
		/// </summary>
		public Position BasePoint { get; set; }

		/// <summary>
		/// 基準からの相対位置を取得または設定します。
		/// </summary>
		public Point Location { get; set; }

		/// <summary>
		/// 画面上の絶対位置を取得します。
		/// </summary>
		public Point AbsoluteLocation
		{
			get
			{
				return this.GetAbsoluteLocation(Location, SystemCore.Instance.WindowSize, Size);
			}
		}

		/// <summary>
		/// 枠のサイズを取得または設定します。
		/// </summary>
		public Size Size { get; set; }

		/// <summary>
		/// 文字列を取得または設定します。
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// このボタンに使用するフォント
		/// </summary>
		public DxSharp.Data.Font Font { get; set; }

		/// <summary>
		/// 通常時の見た目を取得または設定します。
		/// </summary>
		public ButtonStyle NormalStyle { get; set; }

		/// <summary>
		/// ホバー時の見た目を取得または設定します。
		/// </summary>
		public ButtonStyle HoverStyle { get; set; }

		/// <summary>
		/// アクティブ時の見た目を取得または設定します。
		/// </summary>
		public ButtonStyle ActiveStyle { get; set; }

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
			if (Click != null)
				Click(this, e);
		}

		/// <summary>
		/// 更新します。
		/// </summary>
		public void Update()
		{
			if (isMouseDownOld && !IsMouseDown && IsHover)
				OnClick(new EventArgs());

			isMouseDownOld = IsMouseDown;
		}

		/// <summary>
		/// 描画します。
		/// </summary>
		public void Draw()
		{
			var style = IsMouseDown ? ActiveStyle : (IsHover ? HoverStyle : NormalStyle);

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

			var textSize = new Size(DX.GetDrawStringWidth(Text, Text.Length), DX.GetFontSizeToHandle(Font.Handle));
			var textPoint = new Point(new Size(AbsoluteLocation) + Size.Divide() - textSize.Divide());

			DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, style.TextColor.A);
			DX.DrawStringToHandle(textPoint.X, textPoint.Y, Text, (uint)style.TextColor.ToArgb(), Font.Handle, (uint)style.FrameColor.ToArgb());
			DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);
		}
	}
}
