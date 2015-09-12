#pragma once

#include "DxLib.h"
#include <string>

#include "../StateInterface.hpp"

using namespace std;

class GameTitle : public StateInterface
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