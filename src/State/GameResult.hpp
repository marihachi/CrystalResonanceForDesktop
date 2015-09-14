#pragma once

#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"

#include "../IState.hpp"

class GameResult : public IState
{
public: static GameResult &GetInstance(void) { static auto instance = GameResult(); return instance; }
private: GameResult() { }

public:
	// 場面名を取得します
	string StateName()
	{
		return "Result";
	}

	// 更新(ターゲット時のみ)
	void Update()
	{
		auto &core = Core::GetInstance();
		auto &input = InputHelper::GetInstance();

		if (input.KeyState[KEY_INPUT_ESCAPE] == 1)
		{
			core.SetNowStateName("Title");
		}
	}

	// 描画(常時)
	void Draw(StateEventArgs e)
	{
		if (e.IsActive())
		{
			DrawString(0, 0, "Result", 0);
		}
	}
};