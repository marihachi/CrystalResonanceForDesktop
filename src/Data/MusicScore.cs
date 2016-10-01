using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalResonanceDesktop.Data
{
	public class MusicScore
	{
		/// <summary>
		/// 現在の小節
		/// </summary>
		public int NowBarIndex { get; set; }

		/// <summary>
		/// 小節のオフセット
		/// </summary>
		public double Offset { get; set; }

		/// <summary>
		/// このスコアに属しているレーンを取得または設定します
		/// </summary>
		public List<MusicLane> Lanes { get; set; }

		/// <summary>
		/// レーンを指定して現在の小節を取得します
		/// </summary>
		/// <param name="laneNum">レーンのインデックス</param>
		/// <returns></returns>
		public MusicBar NowBar(int laneNum)
		{
			return Lanes[laneNum].Bars[NowBarIndex];
		}
	}
}
