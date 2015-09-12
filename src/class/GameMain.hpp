#pragma once

#include "DxLib.h"
#include <string>

#include "IState.hpp"

using namespace std;

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

	}

	// 描画(常時)
	void Draw()
	{
		DrawString(0, 0, "Main", 0);
	}
};