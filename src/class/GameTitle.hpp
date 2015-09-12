#pragma once

#include "DxLib.h"
#include "IState.hpp"

using namespace std;

class GameTitle : IState
{
public: static GameTitle &GetInstance(void) { static auto instance = GameTitle(); return instance; }
private: GameTitle() { }

public:
	// 更新
	void Update()
	{

	}

	// 描画
	void Draw()
	{

	}
};