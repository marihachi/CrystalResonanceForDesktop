using DxLibDLL;
using DxSharp.Utility;
using System.Drawing;

namespace CrystalResonanceDesktop.Data
{
	public class Ripple
	{
		public Ripple(Point location, int radius = 0)
		{
			Location = location;
			Radius = radius;
		}

		public Point Location { get; private set; }

		public int Radius { get; private set; }

		public void AddRadius(int value)
		{
			Radius += value;
		}

		public void Draw()
		{
			using (new AlphaBlend((int)(255 * 0.5)))
				DX.DrawCircle(Location.X, Location.Y, Radius, 0xffffff, 0);
		}
	}
}
