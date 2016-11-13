using CrystalResonanceDesktop.Data.Enum;

namespace CrystalResonanceDesktop.Data
{
	/// <summary>
	/// 
	/// </summary>
	public class MusicEvent
	{
		public MusicEvent(MusicBar parentBar, MusicEventType type, object value)
		{
			ParentBar = parentBar;
			Type = type;
			Value = value;
		}

		/// <summary>
		/// 
		/// </summary>
		public MusicEventType Type { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public object Value { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public MusicBar ParentBar { get; set; }
	}
}
