#pragma once

#include "StandardInclude.hpp"
#include "EntityInclude.hpp"
#include "HelperInclude.hpp"

#include "IState.hpp"

class Core
{
public: static Core &GetInstance(void) { static Core instance; return instance; }
private: Core() { }

private:
	vector<IState*> _StateList;
	string _NowStateName;
	Size _ScreenSize = Size(0, 0);

public:

	// 場面を追加します
	void AddState(IState* state)
	{
		_StateList.push_back(state);
	}

	// 現在の場面名を設定します
	void SetNowStateName(string nowStateName)
	{
		_NowStateName = nowStateName;
	}

	// 現在の場面名を取得します
	string GetNowStateName()
	{
		return _NowStateName;
	}

	Size ScreenSize()
	{
		return _ScreenSize;
	}

	// 対象場面のUpdateメソッドを呼び出します
	void UpdateTriger()
	{
		for (auto state : _StateList)
			if (state->StateName() == _NowStateName)
			{
				state->Update();
				return;
			}
		throw exception((_NowStateName + "という場面は見つかりませんでした。").c_str());
	}

	// 登録されている全ての場面のDrawメソッドを呼び出します
	void DrawTriger()
	{
		for (auto state : _StateList)
		{
			StateEventArgs e(state->StateName() == _NowStateName);
			state->Draw(e);
		}
	}

	// インスタンスを初期化します
	bool Initialize(string title, Size size, int backR, int backG, int backB)
	{
		_ScreenSize = size;

		if (SetMainWindowText((title + string(" - 起動中です...")).c_str()) != 0)
			return false;

		if (SetGraphMode(size.Width(), size.Height(), 32) != DX_CHANGESCREEN_OK)
			return false;

		if (SetBackgroundColor(backR, backG, backB) != 0)
			return false;

		if (ChangeWindowMode(true) != DX_CHANGESCREEN_OK)
			return false;

		if (DxLib_Init() != 0)
			return false;

		if (SetDrawScreen(DX_SCREEN_BACK) != 0)
			return false;

		if (SetMainWindowText(title.c_str()) != 0)
			return false;

		return true;
	}

	// 毎フレーム呼び出す必要がある基本処理を呼び出します
	bool ProcessContext()
	{
		if (ScreenFlip() != 0)
			return false;

		if (ProcessMessage() != 0)
			return false;

		if (ClearDrawScreen() != 0)
			return false;

		if (_NowStateName == "Close")
			return false;

		InputHelper::GetInstance().UpdateKeyInputTime();

		InputHelper::GetInstance().UpdateMouseInputTime();

		return true;
	}

	// インスタンスを破棄します
	bool Finalize()
	{
		if (DxLib_End() != 0)
			return false;

		return true;
	}
};