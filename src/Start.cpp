#include "StandardInclude.hpp"
#include "EntityInclude.hpp"

#include "Core.hpp"

#include "State\GameTitle.hpp"
#include "State\GameMusicSelect.hpp"
#include "State\GameMain.hpp"
#include "State\GameResult.hpp"
#include "State\GameSetting.hpp"

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	auto &core = Core::Instance();
	auto &input = InputHelper::Instance();

	// 初期化
	if (!core.Initialize("Crystal Resonance for Desktop", Size(1280, 720), 82, 195, 202))
		return -1;

	// 場面の追加
	try
	{
		core.AddState(&GameTitle::Instance());
		core.AddState(&GameMusicSelect::Instance());
		core.AddState(&GameMain::Instance());
		core.AddState(&GameResult::Instance());
		core.AddState(&GameSetting::Instance());
	}
	catch (exception ex)
	{
		MessageBox(
			DxLib::GetMainWindowHandle(),
			("AddStateメソッドで例外が発生しました\n(詳細: " + string(ex.what()) + ")").c_str(),
			"エラー",
			MB_OK);
		return -1;
	}

	// 現在の場面を設定
	core.NowStateName("Title");

	// ゲームループ
	while (core.ProcessContext())
	{
		input.UpdateKeyInputTime();

		input.UpdateMouseInputTime();

		core.TrigerUpdate();

		try
		{
			core.TrigerStateUpdate();
		}
		catch (exception ex)
		{
			MessageBox(
				DxLib::GetMainWindowHandle(),
				("StateUpdateTrigerメソッドで例外が発生しました\n(詳細: " + string(ex.what()) + ")").c_str(),
				"エラー",
				MB_OK);
			return -1;
		}

		core.TrigerDraw();
	}

	// 解放
	if (!core.Finalize())
		return -1;

	return 0;
}