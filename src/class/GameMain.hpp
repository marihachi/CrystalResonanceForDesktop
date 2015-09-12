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
	string StateName()
	{
		return "Main";
	}

	// 更新
	void Update()
	{

	}

	// 描画
	void Draw()
	{

	}
};