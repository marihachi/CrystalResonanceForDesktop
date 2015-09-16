#pragma once

#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"
#include <math.h>
#include "../IState.hpp"

// プレイ画面の場面を表します
class GameMain : public IState
{
public: static GameMain &Instance() { static auto instance = GameMain(); return instance; }
private: GameMain() { }

private:
	bool _IsInitial = true;
	int _DetectionBoxImageHandle;

public:
	// 場面名を取得します
	string StateName()
	{
		return "Main";
	}

	// 更新(ターゲット時のみ)
	void Update()
	{
		auto &core = Core::Instance();
		auto &input = InputHelper::Instance();

		if (Core::Instance().NowStateName() == StateName())
		{
			if (_IsInitial)
			{
				_DetectionBoxImageHandle = LoadGraph("Image/detectionFrame.png", 1);
			}

			if (input.Key[KEY_INPUT_ESCAPE] == 1)
			{
				core.NowStateName("Title");
			}
		}
	}

	// 描画(常時)
	void Draw(StateEventArgs e)
	{
		auto &core = Core::Instance();
		auto &input = InputHelper::Instance();

		if (e.IsActive())
		{
			auto screenRightBottom = core.ScreenSize().WidthHeightAsPoint();
			auto screenCenter = screenRightBottom / 2;

			int imageSize[2];
			GetGraphSize(_DetectionBoxImageHandle, &imageSize[0], &imageSize[1]);
			auto detectionBoxRightBottom = Point(imageSize[0], imageSize[1]);
			auto detectionBoxCenter = detectionBoxRightBottom / 2;

			// 中央のひし形
			auto detectionBoxLocation = screenCenter - detectionBoxCenter;
			DrawGraph(detectionBoxLocation.X(), detectionBoxLocation.Y(), _DetectionBoxImageHandle, 1);

			auto p = screenCenter - input.MousePos;
			DrawFormatString(0, 0, 0xffffff, "Mouse Position(Center): %d, %d", p.X(), p.Y());
		}

	}
};