#pragma once

#include "../StandardInclude.hpp"

#include "../StateInterface.hpp"

class GameSetting : public StateInterface
{
public: static GameSetting &GetInstance(void) { static auto instance = GameSetting(); return instance; }
private: GameSetting() { }

public:
	// 場面名を取得します
	string StateName()
	{
		return "Setting";
	}

	// 更新(ターゲット時のみ)
	void Update()
	{

	}

	// 描画(常時)
	void Draw()
	{
		if (Core::GetInstance().GetNowStateName() == StateName())
		{
			DrawString(0, 0, "Setting", 0);
		}
	}
};