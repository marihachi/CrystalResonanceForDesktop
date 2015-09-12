#pragma once

#include "../StandardInclude.hpp"

#include "../StateInterface.hpp"

class GameTitle : public StateInterface
{
public: static GameTitle &GetInstance(void) { static auto instance = GameTitle(); return instance; }
private: GameTitle() { }

private:
	bool IsInitial = true;
	int logoHandle;

public:
	// 場面名を取得します
	string StateName()
	{
		return "Title";
	}

	// 更新(ターゲット時のみ)
	void Update()
	{
		if (IsInitial)
		{
			IsInitial = false;

			logoHandle = LoadGraph("Image/logo.png", 1);
		}
	}

	// 描画(常時)
	void Draw()
	{
		if (Core::GetInstance().GetNowStateName() == StateName())
		{
			auto centerScreenX = Core::GetInstance().ScreenSizeX / 2;
			auto centerScreenY = Core::GetInstance().ScreenSizeY / 2;
			
			int logoSizeX, logoSizeY;
			GetGraphSize(logoHandle, &logoSizeX, &logoSizeY);

			DrawGraph(centerScreenX - (logoSizeX / 2), centerScreenY - (logoSizeY / 2) - 150, logoHandle, 1);
		}
	}
};