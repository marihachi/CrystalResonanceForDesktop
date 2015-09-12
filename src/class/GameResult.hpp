#pragma once

#include "DxLib.h"
#include "IState.hpp"

using namespace std;

class GameResult : public IState
{
public: static GameResult &GetInstance(void) { static auto instance = GameResult(); return instance; }
private: GameResult() { }

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