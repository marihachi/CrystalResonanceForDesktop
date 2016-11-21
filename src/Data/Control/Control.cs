using System.Drawing;

namespace CrystalResonanceDesktop.Data.Control
{
	/// <summary>
	/// コントロールの基底を表します
	/// </summary>
	public abstract class Control
	{
		public Control(Point location, Control parentControl = null)
		{
			Location = location;
			ParentControl = parentControl;
		}

		/// <summary>
		/// 親コントロールを取得または設定します
		/// </summary>
		public Control ParentControl { get; set; }

		/// <summary>
		/// 相対位置を取得または設定します
		/// </summary>
		public Point Location { get; set; }

		/// <summary>
		/// 絶対位置を取得します
		/// </summary>
		public Point AbsoluteLocation
		{
			get
			{
				if (ParentControl == null)
					return Location;
				return new Point(Location.X + ParentControl.AbsoluteLocation.X, Location.Y + ParentControl.AbsoluteLocation.Y);
			}
		}

		/// <summary>
		/// サイズを取得または設定します
		/// </summary>
		public abstract Size Size { get; }

		public abstract void Update();

		public abstract void Draw();
	}
}
