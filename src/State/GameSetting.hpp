#pragma once

#include "DxLib.h"
#include <string>

#include "../IState.hpp"

using namespace std;

class GameSetting : public IState
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
		DrawString(0, 0, "Setting", 0);
	}
};