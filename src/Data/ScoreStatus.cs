using System.Linq;

namespace CrystalResonanceDesktop.Data
{
	public class ScoreStatus
	{
		public ScoreStatus(double beatLocation, MusicScore targetScore)
		{
			BeatLocation = beatLocation;
			TargetScore = targetScore;
		}

		public double BeatLocation { get; private set; }

		public MusicScore TargetScore { get; private set; }

		public double BarLocation
		{
			get
			{
				// 現在の小節インデックスを求める
				var spanSum = 0.0;
				foreach (var i in Enumerable.Range(0, TargetScore.Bars.Count))
					spanSum += TargetScore.Bars[i].Span;
				return (TargetScore.Bars.Count * BeatIndex / (4 * spanSum));
			}
		}
		public int BarIndex { get { return (int)BarLocation; } }
		public double BarOffset { get { return BarBeatLocation / (NowBar.Span * 4); } }

		public MusicBar NowBar { get { return TargetScore.Bars[BarIndex]; } }

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

		public void SetTargetScore(MusicScore value)
		{
			TargetScore = value;
		}
	}
}
