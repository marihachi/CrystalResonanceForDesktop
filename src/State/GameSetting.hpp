#pragma once

#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"

#include "../IState.hpp"

// 設定画面の場面を表します
class GameSetting : public IState
{
public: static GameSetting &Instance() { static auto instance = GameSetting(); return instance; }
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
		auto &core = Core::Instance();
		auto &input = InputHelper::Instance();

		if (Core::Instance().NowStateName() == StateName())
		{
			if (input.Key[KEY_INPUT_ESCAPE] == 1)
			{
				core.NowStateName("Title");
			}
		}
	}

	// 描画(常時)
	void Draw(StateEventArgs e)
	{
		if (e.IsActive())
		{
			DrawString(0, 0, "Setting", 0);
		}
	}
};