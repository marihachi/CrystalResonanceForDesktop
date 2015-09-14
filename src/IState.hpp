#pragma once

#include "StandardInclude.hpp"

// イベントが発生した時の引数情報
class StateEventArgs
{
private:
	bool _IsActive;

public:
	// 新しいインスタンスを初期化します
	StateEventArgs(bool isActive)
	{
		_IsActive = isActive;
	}

	// この場面が実行中であるかどうかを示す値を取得します
	bool IsActive()
	{
		return _IsActive;
	}
};

// ゲームの場面を表すインターフェイスクラスです
class IState
{
public:
	virtual ~IState() {}

	// 場面名を取得します
	virtual string StateName() = 0;

	// 更新(ターゲット時のみ)
	virtual void Update() = 0;

	// 描画(常時)
	virtual void Draw(StateEventArgs e) = 0;
};