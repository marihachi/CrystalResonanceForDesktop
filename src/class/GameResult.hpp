﻿#pragma once

#include "DxLib.h"
#include <string>

#include "IState.hpp"

using namespace std;

class GameResult : public IState
{
public: static GameResult &GetInstance(void) { static auto instance = GameResult(); return instance; }
private: GameResult() { }

public:
	string StateName()
	{
		return "Result";
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