using DxSharp;
using DxSharp.Data;
using System.Drawing;
using DxSharp.Storage;
using CrystalResonanceDesktop.Utility;
using CrystalResonanceDesktop.Data;
using System;

namespace CrystalResonanceDesktop.Scenes
{
	class Setting : IScene
	{
		private bool _IsInitial { get; set; } = true;

		public void Update()
		{
			var core = SystemCore.Instance;

			if (_IsInitial)
			{
				_IsInitial = false;
			}

		}

		public void Draw()
		{

		}
	}
}
