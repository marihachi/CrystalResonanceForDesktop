#pragma once

// ゲームの場面を表します
class IState
{
public:
	virtual ~IState() {}
	virtual void Update() = 0;
	virtual void Draw() = 0;
};