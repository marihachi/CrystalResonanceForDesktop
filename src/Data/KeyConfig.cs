﻿using DxSharp.Utility;
using System.Collections.Generic;

namespace CrystalResonanceDesktop.Data
{
	public class KeyConfigItem
	{
		public KeyConfigItem(Key key = null, PadButton padButton = null)
		{
			Key = key;
			PadButton = padButton;
		}

		public int InputTime
		{
			get
			{
				return (Key?.InputTime ?? 0) > (PadButton?.InputTime ?? 0) ? Key.InputTime : PadButton?.InputTime ?? 0;
			}
		}

		public Key Key { get; set; }
		public PadButton PadButton { get; set; }

		public override string ToString()
		{
			return $"KeyConfigItem: {{ Key: {Key?.ToString() ?? "null"}, PadButton: {PadButton?.ToString() ?? "null"} }}";
		}
	}

	public class KeyConfig
	{
		public static KeyConfig Instance { get; set; } = new KeyConfig();

		private KeyConfig() { }

		private KeyConfig(KeyConfigItem ok, KeyConfigItem cancel, KeyConfigItem pause, IEnumerable<KeyConfigItem> lanes = null)
		{
			OK = ok;
			Cancel = cancel;
			Pause = pause;

			if (lanes != null)
				Lanes.AddRange(lanes);
		}

		private KeyConfig(KeyConfigItem ok, KeyConfigItem cancel, KeyConfigItem pause, KeyConfigItem up, KeyConfigItem down, KeyConfigItem right, KeyConfigItem left, IEnumerable<KeyConfigItem> lanes = null)
		{
			OK = ok;
			Cancel = cancel;
			Pause = pause;

			Up = up;
			Down = down;
			Right = right;
			Left = left;

			if (lanes != null)
				Lanes.AddRange(lanes);
		}

		public KeyConfigItem OK { get; set; }
		public KeyConfigItem Cancel { get; set; }
		public KeyConfigItem Pause { get; set; }

		public KeyConfigItem Up { get; set; } = new KeyConfigItem(Input.Instance.GetKey(DxSharp.Data.Enum.KeyType.Up));
		public KeyConfigItem Down { get; set; } = new KeyConfigItem(Input.Instance.GetKey(DxSharp.Data.Enum.KeyType.Down));
		public KeyConfigItem Right { get; set; } = new KeyConfigItem(Input.Instance.GetKey(DxSharp.Data.Enum.KeyType.Right));
		public KeyConfigItem Left { get; set; } = new KeyConfigItem(Input.Instance.GetKey(DxSharp.Data.Enum.KeyType.Left));

		public List<KeyConfigItem> Lanes { get; set; } = new List<KeyConfigItem>() { // TODO: 仮設定
			new KeyConfigItem(Input.Instance.GetKey(DxSharp.Data.Enum.KeyType.D)),
			new KeyConfigItem(Input.Instance.GetKey(DxSharp.Data.Enum.KeyType.F)),
			new KeyConfigItem(Input.Instance.GetKey(DxSharp.Data.Enum.KeyType.J)),
			new KeyConfigItem(Input.Instance.GetKey(DxSharp.Data.Enum.KeyType.K))
		};
	}
}
