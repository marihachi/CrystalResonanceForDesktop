#pragma once

#include <string>

using namespace std;

// ゲームの場面を表すインターフェイスクラスです
class IState
{
public:
	virtual ~IState() {}
	virtual string StateName() = 0;
	virtual void Update() = 0;
	virtual void Draw() = 0;
};