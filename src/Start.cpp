#include "DxLib.h"
#include "class\Core.hpp"
#include "class\GameTitle.hpp"
#include "class\GameMain.hpp"
#include "class\GameResult.hpp"

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	Core &core = Core::GetInstance();

	if (!core.Initialize("Crystal Resonance for Desktop", 1280, 720, 255, 255, 255))
		return -1;

	while (core.ProcessContext())
	{
		switch (core.StateNumber)
		{
		case Core::State::Title:

			break;
		case Core::State::GameMain:

			break;
		case Core::State::Setting:

			break;
		case Core::State::Result:

			break;
		default:

			break;
		}
	}

	core.Finalize();

	return 0;
}