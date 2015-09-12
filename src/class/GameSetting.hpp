#pragma once

#include "DxLib.h"
#include <string>

#include "IState.hpp"

using namespace std;

class GameSetting : public IState
{
public: static GameSetting &GetInstance(void) { static auto instance = GameSetting(); return instance; }
private: GameSetting() { }

public:
	string StateName()
	{
		return "Setting";
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