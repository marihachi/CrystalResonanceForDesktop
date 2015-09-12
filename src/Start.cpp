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

	vector<IState*> StateList;
	StateList.push_back(&GameTitle::GetInstance());
	StateList.push_back(&GameMain::GetInstance());
	StateList.push_back(&GameResult::GetInstance());
	StateList.push_back(&GameSetting::GetInstance());

	while (core.ProcessContext())
	{
		string stateName;
		switch (core.StateNumber)
		{
		case Core::State::Title:
			stateName = "Title";
			break;
		case Core::State::GameMain:
			stateName = "Main";
			break;
		case Core::State::Setting:
			stateName = "Setting";
			break;
		case Core::State::Result:
			stateName = "Result";
			break;
		default:
			stateName = "";
			break;
		}

		for (auto state : StateList)
			if (state->StateName() == stateName)
				state->Update();

		for (auto state : StateList)
			state->Draw();
	}

	core.Finalize();

	return 0;
}