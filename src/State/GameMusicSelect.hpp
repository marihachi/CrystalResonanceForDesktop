#pragma once

#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"

#include "../StateInterface.hpp"

class GameMusicSelect : public StateInterface
{
public: static GameMusicSelect &GetInstance(void) { static auto instance = GameMusicSelect(); return instance; }
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