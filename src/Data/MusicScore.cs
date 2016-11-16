using Codeplex.Data;
using CrystalResonanceDesktop.Data.Enum;
using CrystalResonanceDesktop.Utility;
using DxSharp.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrystalResonanceDesktop.Data
{
	/// <summary>
	/// 譜面を表します
	/// </summary>
	public class MusicScore
	{
		public MusicScore(string songTitle, int bpm, Uri songUrl, double offset = 0, IEnumerable<string> tags = null, IEnumerable<MusicDifficulty> difficulties = null)
		{
			SongTitle = songTitle;
			BPM = bpm;
			SongUrl = songUrl;
			Offset = offset;
			Tags = tags?.ToList() ?? new List<string>();
			Difficulties = difficulties?.ToList() ?? new List<MusicDifficulty>();
		}

		/// <summary>
		/// 曲名を取得または設定します
		/// </summary>
		public string SongTitle { get; set; }

		/// <summary>
		/// 曲のURLを取得または設定します
		/// </summary>
		public Uri SongUrl { get; set; }

		/// <summary>
		/// 曲を取得します
		/// </summary>
		public Sound Song { get; private set; }

		/// <summary>
		/// テンポを取得または設定します
		/// </summary>
		public int BPM { get; set; }

		/// <summary>
		/// タイミングを調整するための値を取得または設定します(ミリ秒単位)
		/// </summary>
		public double Offset { get; set; }

		/// <summary>
		/// タグの一覧を取得します
		/// </summary>
		public List<string> Tags { get; private set; }

		/// <summary>
		/// 難易度の一覧を取得します。
		/// </summary>
		public List<MusicDifficulty> Difficulties { get; private set; }

		/// <summary>
		/// 現在対象となっている難易度の種類を取得または設定します
		/// </summary>
		public MusicDifficultyType CurrentDifficultyType { get; set; }

		/// <summary>
		/// 現在対象となっている難易度の種類を取得します
		/// </summary>
		public MusicDifficulty CurrentDifficulty { get { return Difficulties[(int)CurrentDifficultyType]; } }

		/// <summary>
		/// SongUrlから曲データを抽出します
		/// </summary>
		public async Task ExtractSong()
		{
			OggExtractor extractor;

			if (Regex.IsMatch(SongUrl.AbsoluteUri, @"^https?:\/\/(www\.)?youtube\.com\/watch\?v=.+$"))
				extractor = new YoutubeOggExtractor();
			else if (Regex.IsMatch(SongUrl.AbsoluteUri, @"^https?:\/\/(www\.)?soundcloud\.com\/.+?\/.+?$"))
				extractor = new SoundCloudOggExtractor();
			else
				throw new FormatException("SongUrl is invalid format");

			var filepath = await extractor.Extract(SongUrl);
			Song = new Sound(filepath);
			File.Delete(filepath);
		}

		/// <summary>
		/// 譜面ファイルをデシリアライズして譜面情報(MusicScore)として展開します
		/// </summary>
		/// <param name="scoreFilePath">譜面のファイルパス</param>
		/// <returns></returns>
		public static async Task<MusicScore> DeserializeAsync(string scoreFilePath)
		{
			MusicScore score;

			string jStr = "";

			if (!File.Exists(scoreFilePath))
				throw new ArgumentException($"score file not found (filePath: {scoreFilePath})");

			using (var sr = new StreamReader(scoreFilePath))
				jStr = await sr.ReadToEndAsync();

			var json = DynamicJson.Parse(jStr);

			var scoreVersion = json.meta.score_version;

			if (scoreVersion == 1.0)
			{
				string title = json.meta.title;
				double bpm = json.meta.bmp;
				string songUrl = json.meta.song_url;
				double offset = json.meta.offset;

				score = new MusicScore(title, (int)bpm, new Uri(songUrl), offset); // memo: laneCount=4

				foreach (var tags in json.meta.tags)
				{
					score.Tags.Add((string)tags);
				}

				score.Difficulties.Add(new MusicDifficulty(4));

				foreach (var bar in json.bars)
				{
					var size = ((string)bar.size).Split(':').ToList();

					var count = int.Parse(size[0]);
					var span = size.Count == 2 ? double.Parse(size[1]) : 1.0;

					var musicBar = new MusicBar(score, count, span);

					foreach (var lane in bar.notes)
					{
						var musicLane = new MusicLane(musicBar);

						foreach (string noteInfo in lane)
						{
							var noteInfoList = noteInfo.Split(':');
							var countLocation = int.Parse(noteInfoList[0]);
							var noteType = (MusicNoteType)int.Parse(noteInfoList[1]);

							var musicNote = new MusicNote(countLocation, musicLane, noteType);

							musicLane.Notes.Add(musicNote);
						}

						musicBar.Lanes.Add(musicLane);
					}

					score.CurrentDifficulty.Bars.Add(musicBar);
				}
			}
			else
			{
				throw new ArgumentException($"Unknown score version (value: {scoreVersion})");
			}

			return score;
		}
	}
}
