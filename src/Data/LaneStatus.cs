using System.Linq;

namespace CrystalResonanceDesktop.Data
{
	public class LaneStatus
	{
		public LaneStatus(double beatLocation, MusicLane targetLane)
		{
			BeatLocation = beatLocation;
			TargetLane = targetLane;
		}

		public double BeatLocation { get; private set; }


		public MusicLane TargetLane { get; private set; }

		public int BarIndex
		{
			get
			{
				// 現在の小節インデックスを求める
				var spanSum = 0.0;
				foreach (var i in Enumerable.Range(0, TargetLane.Bars.Count))
					spanSum += TargetLane.Bars[i].Span;
				return (int)(TargetLane.Bars.Count * BeatIndex / (4 * spanSum));
			}
		}

		public MusicBar NowBar { get { return TargetLane.Bars[BarIndex]; } }

		public int BeatIndex { get { return (int)BeatLocation; } }
		public double BeatOffset { get { return BeatLocation - BeatIndex; } }

		public double BarBeatLocation { get { return (BeatLocation % (4 * NowBar.Span)); } }
		public int BarBeatIndex { get { return (int)BarBeatLocation; } }
		public double BarBeatOffset { get { return BarBeatLocation - BarBeatIndex; } }
		public double CountLocation { get { return BeatLocation * NowBar.Count % (NowBar.Count * NowBar.Span); } }

		public void SetBeatLocation(double value)
		{
			BeatLocation = value;
		}

		public void SetTargetLane(MusicLane value)
		{
			TargetLane = value;
		}
	}
}
