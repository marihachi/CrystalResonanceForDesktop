using CrystalResonanceDesktop.Data.Control;
using DxSharp;
using DxSharp.Data;
using DxSharp.Data.Enum;
using DxSharp.Storage;
using DxSharp.Utility;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace CrystalResonanceDesktop.Scenes
{
	public class GameMusicSelect : IScene
	{
		public ScrollableListControl<Control> SongList { get; set; }

		private bool IsInitialized { get; set; }

		private void Initialize()
		{
			SongList = new ScrollableListControl<Control>(new Point(0, 0), 10, Color.White, new Size((int)(SystemCore.Instance.WindowSize.Width * (49 / 64d)), SystemCore.Instance.WindowSize.Height), System.Windows.Forms.Orientation.Vertical);

			foreach (var i in Enumerable.Range(0, 20))
				SongList.Items.Add(
					new ButtonControl(
						new Point(0, 0),
						new Size(150, 50),
						$"button{i + 1}",
						new ButtonControl.ButtonStyle(
							FontStorage.Instance.Item("メイリオ20"),
							new ButtonControl.ButtonStyleStatus(Color.White, Color.Transparent, Color.White),
							new ButtonControl.ButtonStyleStatus(Color.White, Color.Transparent, Color.White),
							new ButtonControl.ButtonStyleStatus(Color.White, Color.Transparent, Color.White)
						)
					)
				);

			SongList.Scroll += (s, e) =>
			{
				Debug.WriteLine(e.NewScrollLocation);
			};
		}

		public void Update()
		{
			var input = Input.Instance;
			var scenes = SceneStorage.Instance;

			if (!IsInitialized)
			{
				IsInitialized = true;

				Initialize();
			}

			if (input.GetKey(KeyType.Escape).InputTime == 1)
			{
				scenes.TargetScene = scenes.FindByName("Title");
				IsInitialized = false;
			}

			if (input.GetKey(KeyType.Enter).InputTime == 1)
			{
				scenes.TargetScene = scenes.FindByName("GameMain");
				IsInitialized = false;
			}

			SongList.Update();
		}

		public void Draw()
		{
			var fonts = FontStorage.Instance;

			fonts.Item("メイリオ20").Draw(new Point(0, 0), "Enter: 開発用演奏テスト", Color.White, Position.CenterMiddle);

			SongList.Draw();
		}
	}
}
