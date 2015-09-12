#pragma once

#include "DxLib.h"
#include "IState.hpp"

using namespace std;

class GameMain : public IState
{
public: static GameMain &GetInstance(void) { static auto instance = GameMain(); return instance; }
private: GameMain() { }

public:
	// XV
	void Update()
	{

	}

	// •`‰æ
	void Draw()
	{

	}
};