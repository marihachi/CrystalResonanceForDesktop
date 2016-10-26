using DxSharp.Utility;

namespace CrystalResonanceDesktop.Data
{
	public class KeyConfigItem
	{
		public KeyConfigItem(Key key = null, PadButton padButton = null)
		{
			Key = key;
			PadButton = padButton;
		}

		public Key Key { get; set; }
		public PadButton PadButton { get; set; }
	}

	public class KeyConfig
	{
		public KeyConfigItem OK { get; set; }
		public KeyConfigItem Cancel { get; set; }
		public KeyConfigItem Pause { get; set; }

		public KeyConfigItem LaneA { get; set; }
		public KeyConfigItem LaneB { get; set; }
		public KeyConfigItem LaneC { get; set; }
		public KeyConfigItem LaneD { get; set; }
	}
}
