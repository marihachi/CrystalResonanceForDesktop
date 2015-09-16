#pragma once

#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"

#include "../IState.hpp"

// 結果画面の場面を表します
class GameResult : public IState
{
public: static GameResult &Instance() { static auto instance = GameResult(); return instance; }
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
			DrawString(0, 0, "Result", 0);
		}
	}
};