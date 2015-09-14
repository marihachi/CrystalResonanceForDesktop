#pragma once

#include "StandardInclude.hpp"

// 更新処理を表すインターフェイスクラスです
class IUpdateable
{
public:
	virtual ~IUpdateable() { }

	// 更新名
	virtual string UpdateId() = 0;

	// 更新
	virtual void Update() = 0;
};