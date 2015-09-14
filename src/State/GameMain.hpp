#pragma once

#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"
#include <math.h>
#include "../IState.hpp"

// プレイ画面の場面を表します
class GameMain : public IState
{
public: static GameMain &GetInstance(void) { static auto instance = GameMain(); return instance; }
private: GameMain() { }

public:
	// 場面名を取得します
	string StateName()
	{
		return "Main";
	}

	string UpdateId()
	{
		return StateName();
	}

	// 更新(ターゲット時のみ)
	void Update()
	{
		auto &core = Core::GetInstance();
		auto &input = InputHelper::GetInstance();

		if (Core::GetInstance().GetNowStateName() == StateName())
		{
			if (input.Key[KEY_INPUT_ESCAPE] == 1)
			{
				core.SetNowStateName("Title");
			}
		}
	}

	// 描画(常時)
	void Draw(StateEventArgs e)
	{
		auto &core = Core::GetInstance();
		auto &input = InputHelper::GetInstance();

		if (e.IsActive())
		{
			auto screenRightBottom = core.ScreenSize().GetWidthHeightAsPoint();
			auto screenCenter = screenRightBottom / 2;

			int boxSize = 100;
			auto top = screenCenter - Point(boxSize * -sqrt(2.0), 0);
			auto left = screenCenter - Point(0, boxSize * -sqrt(2.0));
			auto bottom = screenCenter - Point(boxSize * sqrt(2.0), 0);
			auto right = screenCenter - Point(0, boxSize * sqrt(2.0));

			DrawLine(top.X(), top.Y(), left.X(), left.Y(), 0xffffff, 2);
			DrawLine(left.X(), left.Y(), bottom.X(), bottom.Y(), 0xffffff, 2);
			DrawLine(bottom.X(), bottom.Y(), right.X(), right.Y(), 0xffffff, 2);
			DrawLine(right.X(), right.Y(), top.X(), top.Y(), 0xffffff, 2);


			auto p = screenCenter - input.MousePos;
			DrawFormatString(0, 0, 0xffffff, "Mouse Position(Center): %d, %d", p.X(), p.Y());
		}

	}
};