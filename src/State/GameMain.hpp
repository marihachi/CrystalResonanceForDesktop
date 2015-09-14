#pragma once

#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"

#include "../IState.hpp"

// プレイ画面の場面を表します
class GameMain : public IState
{
public: static GameMain &GetInstance(void) { static auto instance = GameMain(); return instance; }
private: GameMain() { }

public:
	// 場面名を取得します
	string StateName()
	{
		return "Main";
	}

	// 更新(ターゲット時のみ)
	void Update()
	{
		auto &core = Core::GetInstance();
		auto &input = InputHelper::GetInstance();

		if (input.Key[KEY_INPUT_ESCAPE] == 1)
		{
			core.SetNowStateName("Title");
		}
	}

	// 描画(常時)
	void Draw(StateEventArgs e)
	{
		if (e.IsActive())
		{
			DrawString(0, 0, "Main", 0);
		}
	}
};