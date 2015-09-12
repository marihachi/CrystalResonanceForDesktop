#include "DxLib.h"

#include <string>
#include <vector>

#include "class\IState.hpp"
#include "class\Core.hpp"
#include "class\GameTitle.hpp"
#include "class\GameMain.hpp"
#include "class\GameResult.hpp"
#include "class\GameSetting.hpp"

using namespace std;

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	Core &core = Core::GetInstance();

	if (!core.Initialize("Crystal Resonance for Desktop", 1280, 720, 255, 255, 255))
		return -1;

	core.AddState(&GameTitle::GetInstance());
	core.AddState(&GameMain::GetInstance());
	core.AddState(&GameResult::GetInstance());
	core.AddState(&GameSetting::GetInstance());

	core.SetNowStateName("Title");

	while (core.ProcessContext())
	{
		core.UpdateTriger();
		core.DrawTriger();
	}

	core.Finalize();

	return 0;
}