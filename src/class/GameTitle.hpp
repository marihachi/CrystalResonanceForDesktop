#pragma once

#include "DxLib.h"
#include <string>

#include "IState.hpp"

using namespace std;

class GameTitle : public IState
{
public: static GameTitle &GetInstance(void) { static auto instance = GameTitle(); return instance; }
private: GameTitle() { }

public:
	string StateName()
	{
		return "Title";
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