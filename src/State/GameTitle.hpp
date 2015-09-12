#pragma once

#include "DxLib.h"
#include <string>

#include "../IState.hpp"

using namespace std;

class GameTitle : public IState
{
public: static GameTitle &GetInstance(void) { static auto instance = GameTitle(); return instance; }
private: GameTitle() { }

public:
	// 場面名を取得します
	string StateName()
	{
		return "Title";
	}

	// 更新(ターゲット時のみ)
	void Update()
	{

	}

	// 描画(常時)
	void Draw()
	{
		DrawString(0, 0, "Title", 0);
	}
};