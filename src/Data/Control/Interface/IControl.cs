using System.Drawing;

namespace CrystalResonanceDesktop.Data.Controls.Interface
{
	/// <summary>
	/// コントロールを表すメンバを公開します
	/// </summary>
	public interface IControl
	{
		Point Location { get; set; }
		Size Size { get; }
		void Update();
		void Draw();
	}
}
