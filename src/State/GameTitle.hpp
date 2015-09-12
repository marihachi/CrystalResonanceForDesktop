#pragma once

#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"

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
			auto centerScreenW = Core::GetInstance().ScreenSize.GetWidth() / 2;
			auto centerScreenH = Core::GetInstance().ScreenSize.GetHeight() / 2;
			
			int logoSizeW, logoSizeH;
			GetGraphSize(logoHandle, &logoSizeW, &logoSizeH);

			DrawGraph(centerScreenW - (logoSizeW / 2), centerScreenH - (logoSizeH / 2) - 150, logoHandle, 1);
		}
	}
};