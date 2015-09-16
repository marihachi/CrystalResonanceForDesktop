#pragma once

#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"

#include "../IState.hpp"

// 選曲画面の場面を表します
class GameMusicSelect : public IState
{
public: static GameMusicSelect &Instance() { static auto instance = GameMusicSelect(); return instance; }
private: GameMusicSelect() { }

public:
	// 場面名を取得します
	string StateName()
	{
		return "MusicSelect";
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
			DrawString(0, 0, "MusicSelect", 0);
		}
	}
};