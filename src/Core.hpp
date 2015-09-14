#pragma once

#include "StandardInclude.hpp"
#include "EntityInclude.hpp"
#include "HelperInclude.hpp"

#include "IState.hpp"

// 場面の制御などを提供します
class Core
{
public: static Core &GetInstance(void) { static Core instance; return instance; }
private: Core() { }

private:
	vector<IUpdateable*> _UpdateableList;
	vector<IState*> _StateList;
	string _NowStateName;
	Size _ScreenSize = Size(0, 0);

public:

	// 更新処理を追加します
	// ※例外が発生する可能性のあるメソッドです
	void AddUpdate(IUpdateable* updateable)
	{
		bool isFound = false;
		for (auto item : _UpdateableList)
		{
			if (item->UpdateId() == updateable->UpdateId())
			{
				isFound = true;
				break;
			}
		}

		if (!isFound)
			_UpdateableList.push_back(updateable);
		else
			throw exception(("更新処理の追加に失敗しました(UpdateId: " + updateable->UpdateId() + ")").c_str());
	}

	// 場面を追加します
	// ※例外が発生する可能性のあるメソッドです
	void AddState(IState* state)
	{
		bool isFound = false;
		for (auto item : _StateList)
		{
			if (item->StateName() == state->StateName())
			{
				throw exception(("場面の追加に失敗しました(StateName: " + state->StateName() + ")").c_str());
			}
		}
		_StateList.push_back(state);
		try
		{
			AddUpdate(state);
		}
		catch (exception ex)
		{
			throw exception(("場面の更新処理の追加に失敗しました(StateName: " + state->StateName() + ")").c_str());
		}
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

	// 登録されている全てのUpdateメソッドを呼び出します
	void UpdateTriger()
	{
		for (auto updateable : _UpdateableList)
		{
			updateable->Update();

			bool isFound = false;
			for (auto state : _StateList)
				if (updateable->UpdateId() == state->UpdateId())
				{
					isFound = true;
					break;
				}

			if (!isFound)
				throw exception((_NowStateName + "という場面は見つかりませんでした。").c_str());
		}
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

		SetDoubleStartValidFlag(true);

		SetOutApplicationLogValidFlag(false);

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
		auto &input = InputHelper::GetInstance();

		if (ScreenFlip() != 0)
			return false;

		if (ProcessMessage() != 0)
			return false;

		if (ClearDrawScreen() != 0)
			return false;

		if (_NowStateName == "Quit")
			return false;

		input.UpdateKeyInputTime();

		input.UpdateMouseInputTime();

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