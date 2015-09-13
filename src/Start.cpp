#include "StandardInclude.hpp"
#include "EntityInclude.hpp"

#include "Core.hpp"

#include "State\GameTitle.hpp"
#include "State\GameMain.hpp"
#include "State\GameResult.hpp"
#include "State\GameSetting.hpp"

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	Core &core = Core::GetInstance();

	// 初期化
	if (!core.Initialize("Crystal Resonance for Desktop", 1280, 720, 82, 195, 202))
		return -1;

	// 場面の追加
	core.AddState(&GameTitle::GetInstance());
	core.AddState(&GameMain::GetInstance());
	core.AddState(&GameResult::GetInstance());
	core.AddState(&GameSetting::GetInstance());

	// 現在の場面を設定
	core.SetNowStateName("Title");

	// ゲームループ
	while (core.ProcessContext())
	{
		try
		{
			core.UpdateTriger();
		}
		catch (exception ex)
		{
			MessageBox(NULL, ("UpdateTrigerメソッドで例外が発生しました\n(詳細: " + string(ex.what()) + ")").c_str(), "エラー", 0);
			break;
		}
		core.DrawTriger();
	}

	// 解放
	if (!core.Finalize())
		return -1;

	return 0;
}